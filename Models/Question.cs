using Google.Cloud.Firestore;

namespace i_am.Models
{
    [FirestoreData]
    public class Question
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? Text { get; set; }

        [FirestoreProperty]
        public bool IsClosed { get; set; }

        [FirestoreProperty]
        public int Order { get; set; }

        [FirestoreProperty]
        public bool IsActive { get; set; }

        [FirestoreProperty]
        public List<AnswerOption>? Options { get; set; }

        public bool IsOpen => !IsClosed;
    }

    [FirestoreData]
    public class AnswerOption
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? Text { get; set; }

        [FirestoreProperty]
        public int Points { get; set; }

        [FirestoreProperty]
        public int Order { get; set; }
    }
}