using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class UserVM : ObservableObject
    {
        private readonly FirestoreService _firestoreService;
        private readonly AuthService _authService;

        public ObservableCollection<User> Users { get; set; } = [];

        [ObservableProperty]
        private User? currentUser;

        [ObservableProperty]
        private bool isLoading;

        public UserVM(FirestoreService firestoreService, AuthService authService)
        {
            _firestoreService = firestoreService;
            _authService = authService;

            _authService.CurrentUserChanged += OnCurrentUserChanged;
        }

        private void OnCurrentUserChanged(object? sender, User? user)
        {
            CurrentUser = user;
        }

        [RelayCommand]
        public async Task LoadCurrentUser()
        {
            IsLoading = true;
            CurrentUser = await _authService.GetCurrentUserAsync();
            IsLoading = false;
        }

        [RelayCommand]
        public async Task GetUsers()
        {
            Users.Clear();
            var users = await _firestoreService.GetUsers();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        [RelayCommand]
        public async Task GoToCheckIn()
        {
            await Shell.Current.GoToAsync(nameof(Pages.CheckInPage));
        }

        [RelayCommand]
        public async Task GoToCalendar()
        {
            await Shell.Current.DisplayAlert("Info", "Kalendarz - do zaimplementowania", "OK");
            //await Shell.Current.GoToAsync(nameof(Pages.CalendarPage));
        }

        [RelayCommand]
        public async Task GoToInvitations()
        {
            await Shell.Current.GoToAsync(nameof(Pages.InvitationsPage));
        }

        [RelayCommand]
        public async Task GoToRelationships()
        {
            await Shell.Current.GoToAsync(nameof(Pages.RelationshipsPage));
        }

        [RelayCommand]
        public async Task GoToSettings()
        {
            await Shell.Current.DisplayAlert("Info", "Ustawienia - do zaimplementowania", "OK");
        }

        [RelayCommand]
        public async Task SendHelpAlert()
        {
            if (CurrentUser == null) return;

            // TODO: Implementacja alertu pomocy z powiadomieniami push
            await AppShell.DisplaySnackbarAsync("Twoi opiekunowie zostali powiadomieni!");
        }

        [RelayCommand]
        public async Task SignOut()
        {
            await _authService.SignOutAsync();
        }
    }
}