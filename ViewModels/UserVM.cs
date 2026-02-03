using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class UserVM : ObservableObject
    {
        private readonly FirestoreService firestoreService;

        public ObservableCollection<User> Users { get; set; } = [];

        [ObservableProperty]
        private User currentUser;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string phoneNumber;

        [ObservableProperty]
        private bool isCaregiver;

        public bool IsDependent => !IsCaregiver;

        public UserVM(FirestoreService firestoreService)
        {
            this.firestoreService = firestoreService;
        }

        [RelayCommand]
        public async Task GetUsers()
        {
            Users.Clear();
            var users = await firestoreService.GetUsers();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        [RelayCommand]
        public async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email)) return;

            CurrentUser = await firestoreService.GetUserByEmail(Email);

            if (CurrentUser != null)
            {
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await Shell.Current.DisplayAlert("B³¹d", "Nie znaleziono u¿ytkownika", "OK");
            }
        }

        [RelayCommand]
        public async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username)) return;

            var newUser = new User
            {
                Email = Email,
                Username = Username,
                PhoneNumber = PhoneNumber,
                IsCaregiver = IsCaregiver,
                CreatedAt = DateTime.UtcNow
            };

            await firestoreService.InsertUser(newUser);
            CurrentUser = await firestoreService.GetUserByEmail(Email);

            await Shell.Current.GoToAsync("//HomePage");
        }

        [RelayCommand]
        public async Task GoToRegister()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }

        [RelayCommand]
        public async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task GoToCheckIn()
        {
            await Shell.Current.GoToAsync("CheckInPage");
        }

        [RelayCommand]
        public async Task GoToCalendar()
        {
            await Shell.Current.GoToAsync("CalendarPage");
        }

        [RelayCommand]
        public async Task GoToInvitations()
        {
            await Shell.Current.GoToAsync("InvitationsPage");
        }

        [RelayCommand]
        public async Task GoToRelationships()
        {
            await Shell.Current.GoToAsync("RelationshipsPage");
        }

        [RelayCommand]
        public async Task GoToSettings()
        {
            await Shell.Current.DisplayAlert("Info", "Ustawienia - do zaimplementowania", "OK");
        }

        //[RelayCommand]
        //public async Task SendHelpAlert()
        //{
        //    if (CurrentUser == null) return;

        //    var alert = new HelpAlert
        //    {
        //        DependentId = CurrentUser.Id,
        //        Timestamp = DateTime.UtcNow,
        //        IsResolved = false
        //    };
        //    await firestoreService.InsertHelpAlert(alert);

        //    await Shell.Current.DisplayAlert("Wys³ano", "Twoi opiekunowie zostali powiadomieni.", "OK");
        //}
    }
}