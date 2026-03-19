using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace i_am
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rejestracja tras 
            Routing.RegisterRoute(nameof(Pages.HomePage), typeof(Pages.HomePage));
            Routing.RegisterRoute(nameof(Pages.RegisterPage), typeof(Pages.RegisterPage));
            Routing.RegisterRoute(nameof(Pages.CheckInPage), typeof(Pages.CheckInPage));
            Routing.RegisterRoute(nameof(Pages.InvitationsPage), typeof(Pages.InvitationsPage));
            Routing.RegisterRoute(nameof(Pages.RelationshipsPage), typeof(Pages.RelationshipsPage));
            Routing.RegisterRoute(nameof(Pages.CalendarPage), typeof(Pages.CalendarPage));
        }
        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Font.SystemFontOfSize(18),
                ActionButtonFont = Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            // Toast is currently not working in MCT on Windows
            if (OperatingSystem.IsWindows())
                return;

            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }
    }
}
