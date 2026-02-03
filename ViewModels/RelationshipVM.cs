using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class RelationshipVM : ObservableObject
    {
        private readonly FirestoreService firestoreService;

        public ObservableCollection<CareRelationship> Relationships { get; set; } = [];

        [ObservableProperty]
        private string currentUserId;

        [ObservableProperty]
        private bool isCaregiver;

        [ObservableProperty]
        private CareRelationship selectedRelationship;

        [ObservableProperty]
        private TimeSpan newCheckInTime;

        public RelationshipVM(FirestoreService firestoreService)
        {
            this.firestoreService = firestoreService;
            NewCheckInTime = new TimeSpan(20, 0, 0);
        }

        [RelayCommand]
        public async Task LoadRelationships()
        {
            Relationships.Clear();
            var relationships = await firestoreService.GetRelationships(CurrentUserId, IsCaregiver);
            foreach (var relationship in relationships)
            {
                Relationships.Add(relationship);
            }
        }

        [RelayCommand]
        public async Task UpdateCheckInTime(CareRelationship relationship)
        {
            if (relationship == null) return;

            relationship.CheckInTime = NewCheckInTime;
            await firestoreService.UpdateRelationship(relationship);
        }

        [RelayCommand]
        public async Task ViewCalendar(string dependentId)
        {
            await Shell.Current.GoToAsync($"CalendarPage?dependentId={dependentId}");
        }

        public void SelectRelationship(CareRelationship relationship)
        {
            SelectedRelationship = relationship;
            NewCheckInTime = relationship.CheckInTime;
        }
    }
}