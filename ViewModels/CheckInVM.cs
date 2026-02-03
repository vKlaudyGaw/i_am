using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class CheckInVM : ObservableObject
    {
        private readonly FirestoreService firestoreService;
        private DailyCheckIn currentCheckIn;
        private double warningThreshold;

        public ObservableCollection<Question> ClosedQuestions { get; set; } = [];
        public ObservableCollection<ClosedQuestionAnswer> Answers { get; set; } = [];

        [ObservableProperty]
        private Question openQuestion;

        [ObservableProperty]
        private string openQuestionResponse;

        [ObservableProperty]
        private bool isCheckInCompleted;

        [ObservableProperty]
        private bool canSubmit;

        [ObservableProperty]
        private string dependentId;

        public CheckInVM(FirestoreService firestoreService)
        {
            this.firestoreService = firestoreService;
        }

        [RelayCommand]
        public async Task LoadQuestions()
        {
            ClosedQuestions.Clear();
            Answers.Clear();

            currentCheckIn = await firestoreService.GetTodayCheckIn(DependentId);
            IsCheckInCompleted = currentCheckIn?.IsCompleted ?? false;

            if (currentCheckIn != null && currentCheckIn.SelectedClosedQuestionIds?.Count > 0)
            {
                await LoadExistingQuestions();
            }
            else
            {
                await LoadNewRandomQuestions();
            }

            RestorePreviousAnswers();
            UpdateCanSubmit();
        }

        private async Task LoadExistingQuestions()
        {
            var allQuestions = await firestoreService.GetQuestions();

            foreach (var questionId in currentCheckIn.SelectedClosedQuestionIds)
            {
                var question = allQuestions.FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {
                    ClosedQuestions.Add(question);
                }
            }

            OpenQuestion = allQuestions.FirstOrDefault(q => q.Id == currentCheckIn.SelectedOpenQuestionId);
            warningThreshold = currentCheckIn.WarningThreshold;
        }

        private async Task LoadNewRandomQuestions()
        {
            var closedQuestions = await firestoreService.GetActiveClosedQuestions();
            var openQuestions = await firestoreService.GetActiveOpenQuestions();

            var random = new Random(GetDailySeed(DependentId, DateTime.UtcNow.Date));

            var selectedClosed = closedQuestions
                .OrderBy(_ => random.Next())
                .Take(5)
                .OrderBy(q => q.Order)
                .ToList();

            foreach (var question in selectedClosed)
            {
                ClosedQuestions.Add(question);
            }

            OpenQuestion = openQuestions
                .OrderBy(_ => random.Next())
                .FirstOrDefault();

            warningThreshold = CalculateWarningThreshold(selectedClosed);

            await SaveQuestionSelection();
        }

        private async Task SaveQuestionSelection()
        {
            var checkIn = new DailyCheckIn
            {
                DependentId = DependentId,
                Date = DateTime.UtcNow.Date,
                SelectedClosedQuestionIds = ClosedQuestions.Select(q => q.Id).ToList(),
                SelectedOpenQuestionId = OpenQuestion?.Id,
                WarningThreshold = warningThreshold,
                IsCompleted = false
            };

            await firestoreService.InsertCheckIn(checkIn);
            currentCheckIn = checkIn;
        }

        private void RestorePreviousAnswers()
        {
            if (currentCheckIn?.ClosedAnswers == null) return;

            foreach (var answer in currentCheckIn.ClosedAnswers)
            {
                Answers.Add(answer);
            }

            if (currentCheckIn.OpenAnswer != null)
            {
                OpenQuestionResponse = currentCheckIn.OpenAnswer.ResponseText;
            }
        }

        public void SelectAnswer(string questionId, AnswerOption option)
        {
            var existing = Answers.FirstOrDefault(a => a.QuestionId == questionId);
            if (existing != null)
            {
                Answers.Remove(existing);
            }

            var question = ClosedQuestions.FirstOrDefault(q => q.Id == questionId);
            Answers.Add(new ClosedQuestionAnswer
            {
                QuestionId = questionId,
                QuestionText = question?.Text,
                SelectedOptionId = option.Id,
                SelectedOptionText = option.Text,
                Points = option.Points
            });

            UpdateCanSubmit();
        }

        private void UpdateCanSubmit()
        {
            var allClosedAnswered = ClosedQuestions.All(q =>
                Answers.Any(a => a.QuestionId == q.Id));
            var openAnswered = OpenQuestion == null ||
                !string.IsNullOrWhiteSpace(OpenQuestionResponse);

            CanSubmit = allClosedAnswered && openAnswered && !IsCheckInCompleted;
        }

        [RelayCommand]
        public async Task SubmitCheckIn()
        {
            if (!CanSubmit) return;

            var totalScore = Answers.Sum(a => a.Points);

            currentCheckIn.ClosedAnswers = Answers.ToList();
            currentCheckIn.TotalScore = totalScore;
            currentCheckIn.IsCompleted = true;
            currentCheckIn.CompletedAt = DateTime.UtcNow;

            if (OpenQuestion != null && !string.IsNullOrWhiteSpace(OpenQuestionResponse))
            {
                currentCheckIn.OpenAnswer = new OpenQuestionAnswer
                {
                    QuestionId = OpenQuestion.Id,
                    QuestionText = OpenQuestion.Text,
                    ResponseText = OpenQuestionResponse.Trim()
                };
            }

            await firestoreService.UpdateCheckIn(currentCheckIn);
            IsCheckInCompleted = true;

            await Shell.Current.DisplayAlert("Sukces", "Twoje odpowiedzi zosta³y zapisane!", "OK");
        }

        //[RelayCommand]
        //public async Task SendHelpAlert()
        //{
        //    var alert = new HelpAlert
        //    {
        //        DependentId = DependentId,
        //        Timestamp = DateTime.UtcNow,
        //        IsResolved = false
        //    };
        //    await firestoreService.InsertHelpAlert(alert);

        //    await Shell.Current.DisplayAlert("Wys³ano", "Twoi opiekunowie zostali powiadomieni.", "OK");
        //}

        private static double CalculateWarningThreshold(List<Question> questions)
        {
            if (questions == null || questions.Count == 0)
                return -1.0;

            var minPoints = questions
                .Where(q => q.Options != null && q.Options.Count > 0)
                .Select(q => q.Options.Min(o => o.Points))
                .Sum();

            return minPoints / 5.0;
        }

        private static int GetDailySeed(string userId, DateTime date)
        {
            var combined = $"{userId}_{date:yyyyMMdd}";
            return combined.GetHashCode();
        }
    }
}