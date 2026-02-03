using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class InvitationVM : ObservableObject
    {
        private readonly FirestoreService firestoreService;

        public ObservableCollection<Invitation> PendingInvitations { get; set; } = [];

        [ObservableProperty]
        private string currentUserId;

        [ObservableProperty]
        private string currentUserName;

        [ObservableProperty]
        private bool currentUserIsCaregiver;

        [ObservableProperty]
        private string recipientEmail;

        public InvitationVM(FirestoreService firestoreService)
        {
            this.firestoreService = firestoreService;
        }

        [RelayCommand]
        public async Task LoadPendingInvitations()
        {
            PendingInvitations.Clear();
            var invitations = await firestoreService.GetPendingInvitations(CurrentUserId);
            foreach (var invitation in invitations)
            {
                PendingInvitations.Add(invitation);
            }
        }

        [RelayCommand]
        public async Task SendInvitation()
        {
            if (string.IsNullOrWhiteSpace(RecipientEmail)) return;

            var invitation = new Invitation
            {
                SenderId = CurrentUserId,
                SenderName = CurrentUserName,
                RecipientEmail = RecipientEmail,
                SenderIsCaregiver = CurrentUserIsCaregiver,
                IsAccepted = null,
                CreatedAt = DateTime.UtcNow
            };
            await firestoreService.InsertInvitation(invitation);

            RecipientEmail = string.Empty;

            await Shell.Current.DisplayAlert("Sukces", "Zaproszenie zosta³o wys³ane!", "OK");
        }

        [RelayCommand]
        public async Task AcceptInvitation(Invitation invitation)
        {
            invitation.IsAccepted = true;
            await firestoreService.UpdateInvitation(invitation);

            var relationship = new CareRelationship
            {
                CaregiverId = invitation.SenderIsCaregiver ? invitation.SenderId : CurrentUserId,
                DependentId = invitation.SenderIsCaregiver ? CurrentUserId : invitation.SenderId,
                CreatedAt = DateTime.UtcNow,
                CheckInTime = new TimeSpan(20, 0, 0)
            };
            await firestoreService.InsertRelationship(relationship);

            PendingInvitations.Remove(invitation);
        }

        [RelayCommand]
        public async Task RejectInvitation(Invitation invitation)
        {
            invitation.IsAccepted = false;
            await firestoreService.UpdateInvitation(invitation);
            PendingInvitations.Remove(invitation);
        }
    }
}