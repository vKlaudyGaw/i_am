using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class RelationshipVM : ObservableObject
    {
        private readonly FirestoreService _firestoreService;
        private readonly AuthService _authService;

        public ObservableCollection<CareRelationship> Relationships { get; } = [];

        [ObservableProperty]
        private TimeSpan newCheckInTime;

        [ObservableProperty]
        private bool isLoading;

        public RelationshipVM(FirestoreService firestoreService, AuthService authService)
        {
            _firestoreService = firestoreService;
            _authService = authService;
            NewCheckInTime = new TimeSpan(20, 0, 0);
        }

        [RelayCommand]
        public async Task LoadRelationships()
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser?.Id == null) return;

            IsLoading = true;
            Relationships.Clear();

            var relationships = await _firestoreService.GetRelationships(currentUser.Id, currentUser.IsCaregiver);
            
            foreach (var relationship in relationships)
            {
                if (currentUser.IsCaregiver)
                {
                    var dependent = await _firestoreService.GetUserById(relationship.DependentId!);
                    relationship.DependentName = dependent?.Username ?? "Nieznany";
                }
                else
                {
                    var caregiver = await _firestoreService.GetUserById(relationship.CaregiverId!);
                    relationship.CaregiverName = caregiver?.Username ?? "Nieznany";
                }
                
                Relationships.Add(relationship);
            }

            IsLoading = false;
        }

        [RelayCommand]
        public async Task UpdateCheckInTime(CareRelationship relationship)
        {
            if (relationship == null) return;

            relationship.CheckInTime = NewCheckInTime;
            await _firestoreService.UpdateRelationship(relationship);

            await Shell.Current.DisplayAlert("Sukces", "Godzina check-in zosta³a zmieniona", "OK");
        }

        [RelayCommand]
        public async Task ViewCalendar(string? dependentId)
        {
            if (string.IsNullOrEmpty(dependentId)) return;
            
            await Shell.Current.GoToAsync($"CalendarPage?dependentId={dependentId}");
        }
    }
}