using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using i_am.Services;
using UserModel = i_am.Models.User;

namespace i_am.Pages
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly FirestoreService _firestoreService;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _phoneNumber;

        [ObservableProperty]
        private bool _isCaregiver;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isLoading;

        public SignUpViewModel(FirebaseAuthClient authClient, FirestoreService firestoreService)
        {
            _authClient = authClient;
            _firestoreService = firestoreService;
        }

        [RelayCommand]
        private async Task SignUp()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Wypełnij wszystkie wymagane pola";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                // Rejestracja w Firebase Auth
                var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(Email, Password, Username);

                // Tworzenie profilu użytkownika w Firestore
                var newUser = new UserModel
                {
                    Id = userCredential.User.Uid,
                    Email = Email,
                    Username = Username,
                    PhoneNumber = PhoneNumber,
                    IsCaregiver = IsCaregiver,
                    CreatedAt = DateTime.UtcNow,
                    LastActiveAt = DateTime.UtcNow
                };

                await _firestoreService.InsertUserWithId(newUser);

                await Shell.Current.GoToAsync("//HomePage");
            }
            catch (FirebaseAuthException ex)
            {
                ErrorMessage = ex.Reason switch
                {
                    AuthErrorReason.EmailExists => "Ten adres email jest już zarejestrowany",
                    AuthErrorReason.WeakPassword => "Hasło jest za słabe (minimum 6 znaków)",
                    AuthErrorReason.InvalidEmailAddress => "Nieprawidłowy format adresu email",
                    _ => $"Błąd rejestracji: {ex.Message}"
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
        private async Task NavigateSignIn()
        {
            await Shell.Current.GoToAsync("//SignIn");
        }
    }
}
