using i_am.ViewModels;

namespace i_am.Views
{
    public partial class RelationshipsPage : ContentPage
    {
        private readonly RelationshipVM viewModel;

        public RelationshipsPage(RelationshipVM vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadRelationshipsCommand.ExecuteAsync(null);
        }
    }
}