using Firebase.Auth;
using i_am.Services;
using UserModel = i_am.Models.User;

namespace i_am.Services
{
    public class AuthService
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly FirestoreService _firestoreService;
        private UserModel? _currentUser;

        public AuthService(FirebaseAuthClient authClient, FirestoreService firestoreService)
        {
            _authClient = authClient;
            _firestoreService = firestoreService;

            _authClient.AuthStateChanged += OnAuthStateChanged;
        }

        public bool IsAuthenticated => _authClient.User != null;
        public string? CurrentUserId => _authClient.User?.Uid;
        public string? CurrentUserEmail => _authClient.User?.Info?.Email;
        public UserModel? CurrentUser => _currentUser;

        public event EventHandler<UserModel?>? CurrentUserChanged;

        private async void OnAuthStateChanged(object? sender, UserEventArgs e)
        {
            if (e.User != null)
            {
                await LoadCurrentUser();
            }
            else
            {
                _currentUser = null;
                CurrentUserChanged?.Invoke(this, null);
            }
        }

        public async Task LoadCurrentUser()
        {
            if (_authClient.User?.Info?.Email != null)
            {
                _currentUser = await _firestoreService.GetUserByEmail(_authClient.User.Info.Email);
                CurrentUserChanged?.Invoke(this, _currentUser);
            }
        }

        public async Task<UserModel?> GetCurrentUserAsync()
        {
            if (_currentUser == null && IsAuthenticated)
            {
                await LoadCurrentUser();
            }
            return _currentUser;
        }

        public async Task SignOutAsync()
        {
            _authClient.SignOut();
            _currentUser = null;
            await Shell.Current.GoToAsync("//SignIn");
        }

        public async Task UpdateFcmToken(string token)
        {
            if (_currentUser != null)
            {
                _currentUser.FcmToken = token;
                await _firestoreService.UpdateUser(_currentUser);
            }
        }
    }
}