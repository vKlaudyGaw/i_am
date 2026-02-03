using Google.Cloud.Firestore;
using i_am.Models;
using i_am.Converters;

namespace i_am.Services
{
    public class FirestoreService
    {
        private FirestoreDb? db;

        private async Task SetupFirestore()
        {
            if (db == null)
            {
                var stream = await FileSystem.OpenAppPackageFileAsync("admin-sdk.json");
                var reader = new StreamReader(stream);
                var contents = reader.ReadToEnd();

                db = new FirestoreDbBuilder
                {
                    ProjectId = "i-am-c9110",
                    ConverterRegistry = new ConverterRegistry
                    {
                        new DateTimeToTimestampConverter(),
                        new TimeSpanToStringConverter()
                    },
                    JsonCredentials = contents
                }.Build();
            }
        }

        #region Sample
        public async Task InsertSampleModel(SampleModel sample)
        {
            await SetupFirestore();
            await db.Collection("SampleModels").AddAsync(sample);
        }

        public async Task<List<SampleModel>> GetSampleModels()
        {
            await SetupFirestore();
            var data = await db.Collection("SampleModels").GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var sampleModel = doc.ConvertTo<SampleModel>();
                    sampleModel.Id = doc.Id;
                    return sampleModel;
                })
                .ToList();
        }
        #endregion

        #region Users
        public async Task InsertUser(User user)
        {
            await SetupFirestore();
            await db.Collection("Users").AddAsync(user);
        }

        public async Task<List<User>> GetUsers()
        {
            await SetupFirestore();
            var data = await db.Collection("Users").GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var user = doc.ConvertTo<User>();
                    user.Id = doc.Id;
                    return user;
                })
                .ToList();
        }

        public async Task<User> GetUserById(string userId)
        {
            await SetupFirestore();
            var doc = await db.Collection("Users").Document(userId).GetSnapshotAsync();
            if (doc.Exists)
            {
                var user = doc.ConvertTo<User>();
                user.Id = doc.Id;
                return user;
            }
            return null;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            await SetupFirestore();
            var data = await db.Collection("Users")
                .WhereEqualTo("Email", email)
                .GetSnapshotAsync();
            var doc = data.Documents.FirstOrDefault();
            if (doc != null)
            {
                var user = doc.ConvertTo<User>();
                user.Id = doc.Id;
                return user;
            }
            return null;
        }

        public async Task UpdateUser(User user)
        {
            await SetupFirestore();
            await db.Collection("Users").Document(user.Id).SetAsync(user);
        }
        #endregion

        #region Invitations
        public async Task InsertInvitation(Invitation invitation)
        {
            await SetupFirestore();
            await db.Collection("Invitations").AddAsync(invitation);
        }

        public async Task<List<Invitation>> GetPendingInvitations(string recipientId)
        {
            await SetupFirestore();
            var data = await db.Collection("Invitations")
                .WhereEqualTo("RecipientId", recipientId)
                .WhereEqualTo("IsAccepted", null)
                .GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var invitation = doc.ConvertTo<Invitation>();
                    invitation.Id = doc.Id;
                    return invitation;
                })
                .ToList();
        }

        public async Task UpdateInvitation(Invitation invitation)
        {
            await SetupFirestore();
            await db.Collection("Invitations").Document(invitation.Id).SetAsync(invitation);
        }
        #endregion

        #region Relationships
        public async Task InsertRelationship(CareRelationship relationship)
        {
            await SetupFirestore();
            await db.Collection("Relationships").AddAsync(relationship);
        }

        public async Task<List<CareRelationship>> GetRelationships(string userId, bool isCaregiver)
        {
            await SetupFirestore();
            var field = isCaregiver ? "CaregiverId" : "DependentId";
            var data = await db.Collection("Relationships")
                .WhereEqualTo(field, userId)
                .GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var relationship = doc.ConvertTo<CareRelationship>();
                    relationship.Id = doc.Id;
                    return relationship;
                })
                .ToList();
        }

        public async Task UpdateRelationship(CareRelationship relationship)
        {
            await SetupFirestore();
            await db.Collection("Relationships").Document(relationship.Id).SetAsync(relationship);
        }
        #endregion

        #region CheckIns
        public async Task InsertCheckIn(DailyCheckIn checkIn)
        {
            await SetupFirestore();
            await db.Collection("CheckIns").AddAsync(checkIn);
        }

        public async Task<DailyCheckIn> GetTodayCheckIn(string dependentId)
        {
            await SetupFirestore();
            var today = DateTime.UtcNow.Date;
            var data = await db.Collection("CheckIns")
                .WhereEqualTo("DependentId", dependentId)
                .WhereEqualTo("Date", today)
                .GetSnapshotAsync();
            var doc = data.Documents.FirstOrDefault();
            if (doc != null)
            {
                var checkIn = doc.ConvertTo<DailyCheckIn>();
                checkIn.Id = doc.Id;
                return checkIn;
            }
            return null;
        }

        public async Task<List<DailyCheckIn>> GetCheckInsForMonth(string dependentId, int year, int month)
        {
            await SetupFirestore();
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);
            var data = await db.Collection("CheckIns")
                .WhereEqualTo("DependentId", dependentId)
                .WhereGreaterThanOrEqualTo("Date", startDate)
                .WhereLessThan("Date", endDate)
                .GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var checkIn = doc.ConvertTo<DailyCheckIn>();
                    checkIn.Id = doc.Id;
                    return checkIn;
                })
                .ToList();
        }

        public async Task UpdateCheckIn(DailyCheckIn checkIn)
        {
            await SetupFirestore();
            await db.Collection("CheckIns").Document(checkIn.Id).SetAsync(checkIn);
        }
        #endregion

        #region Questions
        public async Task InsertQuestion(Question question)
        {
            await SetupFirestore();
            await db.Collection("Questions").AddAsync(question);
        }

        public async Task<List<Question>> GetQuestions()
        {
            await SetupFirestore();
            var data = await db.Collection("Questions").GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var question = doc.ConvertTo<Question>();
                    question.Id = doc.Id;
                    return question;
                })
                .ToList();
        }

        public async Task<List<Question>> GetActiveClosedQuestions()
        {
            await SetupFirestore();
            var data = await db.Collection("Questions")
                .WhereEqualTo("IsActive", true)
                .WhereEqualTo("IsClosed", true)
                .GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var question = doc.ConvertTo<Question>();
                    question.Id = doc.Id;
                    return question;
                })
                .OrderBy(q => q.Order)
                .ToList();
        }

        public async Task<List<Question>> GetActiveOpenQuestions()
        {
            await SetupFirestore();
            var data = await db.Collection("Questions")
                .WhereEqualTo("IsActive", true)
                .WhereEqualTo("IsClosed", false)
                .GetSnapshotAsync();
            return data.Documents
                .Select(doc =>
                {
                    var question = doc.ConvertTo<Question>();
                    question.Id = doc.Id;
                    return question;
                })
                .OrderBy(q => q.Order)
                .ToList();
        }

        public async Task SeedDefaultQuestions()
        {
            await SetupFirestore();
            var existing = await GetQuestions();
            if (existing.Count > 0) return;

            var defaultQuestions = Data.SeedQuestions.GetDefaultQuestions();
            foreach (var question in defaultQuestions)
            {
                await db.Collection("Questions").AddAsync(question);
            }
        }
        #endregion

        #region Alerts
        //public async Task InsertHelpAlert(HelpAlert alert)
        //{
        //    await SetupFirestore();
        //    await db.Collection("HelpAlerts").AddAsync(alert);
        //}

        //public async Task<List<HelpAlert>> GetUnresolvedAlerts(string dependentId)
        //{
        //    await SetupFirestore();
        //    var data = await db.Collection("HelpAlerts")
        //        .WhereEqualTo("DependentId", dependentId)
        //        .WhereEqualTo("IsResolved", false)
        //        .GetSnapshotAsync();
        //    return data.Documents
        //        .Select(doc =>
        //        {
        //            var alert = doc.ConvertTo<HelpAlert>();
        //            alert.Id = doc.Id;
        //            return alert;
        //        })
        //        .ToList();
        //}
        #endregion
    }
}