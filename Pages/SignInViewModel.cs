using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using i_am.Models;
using i_am.Services;

namespace i_am.Pages
{
    public partial class SignInViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly FirestoreService _firestoreService;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isLoading;

        public string Username => _authClient.User?.Info?.DisplayName;
        public bool IsLoggedIn => _authClient.User != null;

        public SignInViewModel(FirebaseAuthClient authClient, FirestoreService firestoreService)
        {
            _authClient = authClient;
            _firestoreService = firestoreService;

            // NIE wywołuj nawigacji w konstruktorze - przenieś do OnAppearing
        }

        // Zmień metodę CheckAutoLoginAsync aby zwracała bool
        public async Task<bool> CheckAutoLoginAsync()
        {
            // Poczekaj na pełną inicjalizację Shell
            await Task.Delay(100);
            
            if (_authClient.User != null && Shell.Current != null)
            {
                await Shell.Current.GoToAsync("//HomePage");
                return true;
            }
            return false;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Wprowadź email i hasło";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _authClient.SignInWithEmailAndPasswordAsync(Email, Password);

                var user = await _firestoreService.GetUserByEmail(Email);
                if (user != null)
                {
                    user.LastActiveAt = DateTime.UtcNow;
                    await _firestoreService.UpdateUser(user);
                }

                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(IsLoggedIn));

                await Shell.Current.GoToAsync("//HomePage");
            }
            catch (FirebaseAuthException ex)
            {
                ErrorMessage = ex.Reason switch
                {
                    AuthErrorReason.WrongPassword => "Nieprawidłowe hasło",
                    AuthErrorReason.UnknownEmailAddress => "Nie znaleziono użytkownika o tym adresie email",
                    AuthErrorReason.InvalidEmailAddress => "Nieprawidłowy format adresu email",
                    AuthErrorReason.UserDisabled => "Konto zostało zablokowane",
                    _ => $"Błąd logowania: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Wystąpił błąd: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync("//SignUp");
        }

        
        //czyszczenie pól
        public void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}
