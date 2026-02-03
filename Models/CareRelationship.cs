using Google.Cloud.Firestore;

namespace i_am.Models
{
    [FirestoreData]
    public class CareRelationship
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? CaregiverId { get; set; }

        [FirestoreProperty]
        public string? DependentId { get; set; }

        [FirestoreProperty]
        public TimeSpan CheckInTime { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }
    }
}