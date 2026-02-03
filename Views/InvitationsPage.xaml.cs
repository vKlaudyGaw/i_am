using i_am.ViewModels;

namespace i_am.Views
{
    public partial class InvitationsPage : ContentPage
    {
        private readonly InvitationVM viewModel;

        public InvitationsPage(InvitationVM vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadPendingInvitationsCommand.ExecuteAsync(null);
        }
    }
}