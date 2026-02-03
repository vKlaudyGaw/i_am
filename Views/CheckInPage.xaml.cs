using i_am.Models;
using i_am.ViewModels;

namespace i_am.Views
{
    public partial class CheckInPage : ContentPage
    {
        private readonly CheckInVM viewModel;

        public CheckInPage(CheckInVM vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadQuestionsCommand.ExecuteAsync(null);
        }

        private void OnAnswerSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is AnswerOption selectedOption)
            {
                var collectionView = sender as CollectionView;
                var question = collectionView?.BindingContext as Question;

                if (question != null)
                {
                    viewModel.SelectAnswer(question.Id, selectedOption);
                }
            }
        }
    }
}