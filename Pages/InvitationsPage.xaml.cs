using i_am.Models;
using i_am.ViewModels;

namespace i_am.Pages
{
    public partial class InvitationsPage : ContentPage
    {
        private readonly InvitationVM _viewModel;

        public InvitationsPage(InvitationVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadPendingInvitationsCommand.ExecuteAsync(null);
        }

        private async void OnAcceptClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Invitation invitation)
            {
                await _viewModel.AcceptInvitationCommand.ExecuteAsync(invitation);
            }
        }

        private async void OnRejectClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Invitation invitation)
            {
                await _viewModel.RejectInvitationCommand.ExecuteAsync(invitation);
            }
        }
    }
}