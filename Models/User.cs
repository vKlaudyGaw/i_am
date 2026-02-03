using Google.Cloud.Firestore;

namespace i_am.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? Email { get; set; }

        [FirestoreProperty]
        public string? PhoneNumber { get; set; }

        [FirestoreProperty]
        public string? Username { get; set; }

        [FirestoreProperty]
        public bool IsCaregiver { get; set; }

        [FirestoreProperty]
        public string? FcmToken { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }

        [FirestoreProperty]
        public DateTime? LastActiveAt { get; set; }

        public bool IsDependent => !IsCaregiver;
    }
}