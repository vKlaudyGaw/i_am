using i_am.Models;

namespace i_am.Data
{
    public static class SeedQuestions
    {
        public static List<Question> GetDefaultQuestions()
        {
            return
            [
                new Question
                {
                    Id = "q_mood",
                    Text = "Jak oceniasz swÛj nastrÛj dzisiaj?",
                    IsClosed = true,
                    Order = 1,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "m1", Text = "Bardzo dobry", Points = 0, Order = 1 },
                        new AnswerOption { Id = "m2", Text = "Dobry", Points = 0, Order = 2 },
                        new AnswerOption { Id = "m3", Text = "Neutralny", Points = 0, Order = 3 },
                        new AnswerOption { Id = "m4", Text = "Z≥y", Points = -1, Order = 4 },
                        new AnswerOption { Id = "m5", Text = "Bardzo z≥y", Points = -2, Order = 5 }
                    ]
                },
                new Question
                {
                    Id = "q_sleep",
                    Text = "Jak spa≥eú/aú ostatniej nocy?",
                    IsClosed = true,
                    Order = 2,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "s1", Text = "Bardzo dobrze", Points = 0, Order = 1 },
                        new AnswerOption { Id = "s2", Text = "Dobrze", Points = 0, Order = 2 },
                        new AnswerOption { Id = "s3", Text = "årednio", Points = -1, Order = 3 },
                        new AnswerOption { Id = "s4", Text = "èle", Points = -2, Order = 4 }
                    ]
                },
                new Question
                {
                    Id = "q_appetite",
                    Text = "Jak oceniasz swÛj apetyt dzisiaj?",
                    IsClosed = true,
                    Order = 3,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "a1", Text = "Normalny", Points = 0, Order = 1 },
                        new AnswerOption { Id = "a2", Text = "Zmniejszony", Points = -1, Order = 2 },
                        new AnswerOption { Id = "a3", Text = "Brak apetytu", Points = -2, Order = 3 }
                    ]
                },
                new Question
                {
                    Id = "q_energy",
                    Text = "Jak oceniasz swÛj poziom energii dzisiaj?",
                    IsClosed = true,
                    Order = 4,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "e1", Text = "Wysoki", Points = 0, Order = 1 },
                        new AnswerOption { Id = "e2", Text = "Normalny", Points = 0, Order = 2 },
                        new AnswerOption { Id = "e3", Text = "Niski", Points = -1, Order = 3 },
                        new AnswerOption { Id = "e4", Text = "Bardzo niski", Points = -2, Order = 4 }
                    ]
                },
                new Question
                {
                    Id = "q_social",
                    Text = "Czy rozmawia≥eú/aú dzisiaj z kimú?",
                    IsClosed = true,
                    Order = 5,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "sc1", Text = "Tak", Points = 0, Order = 1 },
                        new AnswerOption { Id = "sc2", Text = "Nie", Points = -1, Order = 2 }
                    ]
                },
                new Question
                {
                    Id = "q_activity",
                    Text = "Czy wyszed≥eú/aú dzisiaj z domu?",
                    IsClosed = true,
                    Order = 6,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "ac1", Text = "Tak", Points = 0, Order = 1 },
                        new AnswerOption { Id = "ac2", Text = "Nie", Points = -1, Order = 2 }
                    ]
                },
                new Question
                {
                    Id = "q_medication",
                    Text = "Czy wziπ≥eú/aú dzisiaj leki?",
                    IsClosed = true,
                    Order = 7,
                    IsActive = true,
                    Options =
                    [
                        new AnswerOption { Id = "md1", Text = "Tak", Points = 0, Order = 1 },
                        new AnswerOption { Id = "md2", Text = "Nie", Points = -1, Order = 2 },
                        new AnswerOption { Id = "md3", Text = "Nie dotyczy", Points = 0, Order = 3 }
                    ]
                },

                // Pytania otwarte
                new Question
                {
                    Id = "q_open_activities",
                    Text = "Opisz w kilku zdaniach, co dzisiaj robi≥eú/aú?",
                    IsClosed = false,
                    Order = 100,
                    IsActive = true,
                    Options = []
                },
                new Question
                {
                    Id = "q_open_smile",
                    Text = "Co sprawi≥o, øe siÍ dzisiaj uúmiechnπ≥eú/aú?",
                    IsClosed = false,
                    Order = 101,
                    IsActive = true,
                    Options = []
                },
                new Question
                {
                    Id = "q_open_grateful",
                    Text = "Za co jesteú dzisiaj wdziÍczny/a?",
                    IsClosed = false,
                    Order = 102,
                    IsActive = true,
                    Options = []
                },
                new Question
                {
                    Id = "q_open_thoughts",
                    Text = "Czy jest coú, co chcia≥byú/chcia≥abyú przekazaÊ swojemu opiekunowi?",
                    IsClosed = false,
                    Order = 103,
                    IsActive = true,
                    Options = []
                }
            ];
        }
    }
}