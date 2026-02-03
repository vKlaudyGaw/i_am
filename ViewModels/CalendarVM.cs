using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using i_am.Models;
using i_am.Services;
using System.Collections.ObjectModel;

namespace i_am.ViewModels
{
    public partial class CalendarVM : ObservableObject
    {
        private readonly FirestoreService firestoreService;

        public ObservableCollection<DailyCheckIn> MonthCheckIns { get; set; } = [];

        [ObservableProperty]
        private string dependentId;

        [ObservableProperty]
        private int selectedYear;

        [ObservableProperty]
        private int selectedMonth;

        public CalendarVM(FirestoreService firestoreService)
        {
            this.firestoreService = firestoreService;
            SelectedYear = DateTime.Now.Year;
            SelectedMonth = DateTime.Now.Month;
        }

        [RelayCommand]
        public async Task LoadMonthData()
        {
            MonthCheckIns.Clear();
            var checkIns = await firestoreService.GetCheckInsForMonth(DependentId, SelectedYear, SelectedMonth);
            foreach (var checkIn in checkIns.OrderBy(c => c.Date))
            {
                MonthCheckIns.Add(checkIn);
            }
        }

        [RelayCommand]
        public async Task NextMonth()
        {
            if (SelectedMonth == 12)
            {
                SelectedMonth = 1;
                SelectedYear++;
            }
            else
            {
                SelectedMonth++;
            }
            await LoadMonthData();
        }

        [RelayCommand]
        public async Task PreviousMonth()
        {
            if (SelectedMonth == 1)
            {
                SelectedMonth = 12;
                SelectedYear--;
            }
            else
            {
                SelectedMonth--;
            }
            await LoadMonthData();
        }
    }
}