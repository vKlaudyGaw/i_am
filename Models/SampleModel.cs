using Google.Cloud.Firestore;

namespace i_am.Models
{
    [FirestoreData]
    public class SampleModel
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? Name { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty]
        public DateTime CreatedAt { get; set; }
    }
}