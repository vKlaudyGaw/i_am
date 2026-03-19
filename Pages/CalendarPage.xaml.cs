using i_am.ViewModels;

namespace i_am.Pages
{
    public partial class CalendarPage : ContentPage
    {
        private readonly CalendarVM viewModel;

        public CalendarPage(CalendarVM vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadMonthDataCommand.ExecuteAsync(null);
        }
    }
}