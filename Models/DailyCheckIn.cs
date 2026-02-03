using Google.Cloud.Firestore;

namespace i_am.Models
{
    [FirestoreData]
    public class DailyCheckIn
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? DependentId { get; set; }

        [FirestoreProperty]
        public DateTime Date { get; set; }

        [FirestoreProperty]
        public List<ClosedQuestionAnswer>? ClosedAnswers { get; set; }

        [FirestoreProperty]
        public OpenQuestionAnswer? OpenAnswer { get; set; }

        [FirestoreProperty]
        public int TotalScore { get; set; }

        [FirestoreProperty]
        public bool IsCompleted { get; set; }

        [FirestoreProperty]
        public DateTime? CompletedAt { get; set; }

        [FirestoreProperty]
        public List<string>? SelectedClosedQuestionIds { get; set; }

        [FirestoreProperty]
        public string? SelectedOpenQuestionId { get; set; }

        [FirestoreProperty]
        public double WarningThreshold { get; set; }
    }

    [FirestoreData]
    public class ClosedQuestionAnswer
    {
        [FirestoreProperty]
        public string? QuestionId { get; set; }

        [FirestoreProperty]
        public string? QuestionText { get; set; }

        [FirestoreProperty]
        public string? SelectedOptionId { get; set; }

        [FirestoreProperty]
        public string? SelectedOptionText { get; set; }

        [FirestoreProperty]
        public int Points { get; set; }
    }

    [FirestoreData]
    public class OpenQuestionAnswer
    {
        [FirestoreProperty]
        public string? QuestionId { get; set; }

        [FirestoreProperty]
        public string? QuestionText { get; set; }

        [FirestoreProperty]
        public string? ResponseText { get; set; }
    }
}