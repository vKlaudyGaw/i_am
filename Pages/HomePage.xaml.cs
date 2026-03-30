using i_am.ViewModels;

namespace i_am.Pages
{
    public partial class HomePage : ContentPage
    {
        private readonly UserVM _viewModel;

        public HomePage(UserVM vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _viewModel = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            if (_viewModel.CurrentUser == null)
            {
                await _viewModel.LoadCurrentUserCommand.ExecuteAsync(null);
            }
        }
    }
}