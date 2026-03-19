using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace i_am.ViewModels
{
    public partial class InvitationVM : ObservableObject
    {
        private readonly FirestoreService _firestoreService;
        private readonly AuthService _authService;

        public ObservableCollection<Invitation> PendingInvitations { get; } = [];

        [ObservableProperty]
        private string? recipientEmail;

        [ObservableProperty]
        private bool isLoading;

        public InvitationVM(FirestoreService firestoreService, AuthService authService)
        {
            _firestoreService = firestoreService;
            _authService = authService;
        }

        [RelayCommand]
        public async Task LoadPendingInvitations()
        {
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser?.Id == null) return;

            IsLoading = true;
            PendingInvitations.Clear();

            var invitations = await _firestoreService.GetPendingInvitations(currentUser.Id);
            Debug.WriteLine($"Loaded {invitations.Count} invitations");
            
            foreach (var invitation in invitations)
            {
                Debug.WriteLine($"Invitation - Id: {invitation.Id}, Sender: {invitation.SenderName}");
                PendingInvitations.Add(invitation);
            }

            IsLoading = false;
        }

        [RelayCommand]
        public async Task SendInvitation()
        {
            if (string.IsNullOrWhiteSpace(RecipientEmail))
            {
                await Shell.Current.DisplayAlert("B³¹d", "WprowadŸ adres email", "OK");
                return;
            }

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null) return;

            if (RecipientEmail.Equals(currentUser.Email, StringComparison.OrdinalIgnoreCase))
            {
                await Shell.Current.DisplayAlert("B³¹d", "Nie mo¿esz wys³aæ zaproszenia do siebie", "OK");
                return;
            }

            var recipient = await _firestoreService.GetUserByEmail(RecipientEmail);
            if (recipient == null)
            {
                await Shell.Current.DisplayAlert("B³¹d", "Nie znaleziono u¿ytkownika", "OK");
                return;
            }

            if (currentUser.IsCaregiver == recipient.IsCaregiver)
            {
                var roleText = currentUser.IsCaregiver ? "opiekunem" : "podopiecznym";
                await Shell.Current.DisplayAlert("B³¹d", $"Nie mo¿esz wys³aæ zaproszenia do u¿ytkownika, który te¿ jest {roleText}", "OK");
                return;
            }

            var invitation = new Invitation
            {
                SenderId = currentUser.Id,
                SenderName = currentUser.Username,
                SenderEmail = currentUser.Email,
                SenderIsCaregiver = currentUser.IsCaregiver,
                RecipientId = recipient.Id,
                RecipientEmail = recipient.Email,
                IsAccepted = null,
                CreatedAt = DateTime.UtcNow
            };

            await _firestoreService.InsertInvitation(invitation);

            RecipientEmail = string.Empty;

            await Shell.Current.DisplayAlert("Sukces", $"Zaproszenie zosta³o wys³ane do {recipient.Username}!", "OK");
        }

        [RelayCommand]
        public async Task AcceptInvitation(Invitation invitation)
        {
            Debug.WriteLine($"AcceptInvitation called");
            
            if (invitation == null)
            {
                Debug.WriteLine("Invitation is NULL!");
                return;
            }

            Debug.WriteLine($"Invitation Id: {invitation.Id}");

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                Debug.WriteLine("CurrentUser is NULL!");
                return;
            }

            Debug.WriteLine($"CurrentUser Id: {currentUser.Id}");

            try
            {
                // Aktualizuj zaproszenie
                invitation.IsAccepted = true;
                await _firestoreService.UpdateInvitation(invitation);
                Debug.WriteLine("Invitation updated");

                // Utwórz relacjê
                var relationship = new CareRelationship
                {
                    CaregiverId = invitation.SenderIsCaregiver ? invitation.SenderId : currentUser.Id,
                    DependentId = invitation.SenderIsCaregiver ? currentUser.Id : invitation.SenderId,
                    CreatedAt = DateTime.UtcNow,
                    CheckInTime = new TimeSpan(20, 0, 0)
                };

                Debug.WriteLine($"Creating relationship - Caregiver: {relationship.CaregiverId}, Dependent: {relationship.DependentId}");
                
                await _firestoreService.InsertRelationship(relationship);
                Debug.WriteLine("Relationship inserted");

                PendingInvitations.Remove(invitation);

                await Shell.Current.DisplayAlert("Sukces", $"Zaproszenie od {invitation.SenderName} zaakceptowane!", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                Debug.WriteLine($"Stack: {ex.StackTrace}");
                await Shell.Current.DisplayAlert("B³¹d", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task RejectInvitation(Invitation invitation)
        {
            if (invitation == null) return;

            try
            {
                invitation.IsAccepted = false;
                await _firestoreService.UpdateInvitation(invitation);
                PendingInvitations.Remove(invitation);

                await Shell.Current.DisplayAlert("Info", "Zaproszenie odrzucone", "OK");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR: {ex.Message}");
                await Shell.Current.DisplayAlert("B³¹d", ex.Message, "OK");
            }
        }
    }
}