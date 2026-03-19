using i_am.Models;
using i_am.ViewModels;
using System.ComponentModel;

namespace i_am.Pages
{
    public partial class RelationshipsPage : ContentPage
    {
        private readonly RelationshipVM _viewModel;

        public RelationshipsPage(RelationshipVM vm)
        {
            InitializeComponent();
            _viewModel = vm;
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadRelationshipsCommand.ExecuteAsync(null);
        }

        private void OnTimePickerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TimePicker.Time) && sender is TimePicker timePicker)
            {
                _viewModel.NewCheckInTime = timePicker.Time;
            }
        }

        private async void OnUpdateCheckInTimeClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is CareRelationship relationship)
            {
                await _viewModel.UpdateCheckInTimeCommand.ExecuteAsync(relationship);
            }
        }

        private async void OnViewCalendarClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is CareRelationship relationship)
            {
                await _viewModel.ViewCalendarCommand.ExecuteAsync(relationship.DependentId);
            }
        }
    }
}