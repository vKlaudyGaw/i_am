using Google.Cloud.Firestore;

namespace i_am.Models
{
    [FirestoreData]
    public class Invitation
    {
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? SenderId { get; set; }

        [FirestoreProperty]
        public string? SenderName { get; set; }

        [FirestoreProperty]
        public string? SenderEmail { get; set; }

        [FirestoreProperty]
        public string? RecipientEmail { get; set; }

        [FirestoreProperty]
        public string? RecipientId { get; set; }

        [FirestoreProperty]
        public bool SenderIsCaregiver { get; set; }

        [FirestoreProperty]
        public bool? IsAccepted { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        public bool IsPending => IsAccepted == null;
        public bool IsRejected => IsAccepted == false;

        public string InviteTypeText => SenderIsCaregiver 
            ? "Chce zostaæ Twoim opiekunem" 
            : "Chce, ¿ebyœ zosta³ jego opiekunem";
    }
}