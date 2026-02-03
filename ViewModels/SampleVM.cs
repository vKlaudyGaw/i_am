using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class SampleVM
    {
        private readonly FirestoreService firestoreService;

        public ObservableCollection<SampleModel> SampleData { get; set; } = [];

        public SampleVM(FirestoreService firestoreService)
        {
            this.firestoreService = firestoreService;
        }

        [RelayCommand]
        public async Task GetSampleData()
        {
            SampleData.Clear();
            var data = await firestoreService.GetSampleModels();
            foreach (var item in data)
            {
                SampleData.Add(item);
            }
        }

        [RelayCommand]
        public async Task SaveSampleData()
        {
            var sample = new SampleModel
            {
                Name = "Sample",
                Description = "Sample Description",
                CreatedAt = DateTime.Now
            };
            await firestoreService.InsertSampleModel(sample);
        }
    }
}