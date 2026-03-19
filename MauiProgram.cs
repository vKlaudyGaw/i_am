using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using i_am.Services;
using i_am.ViewModels;
using i_am.Pages;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

namespace i_am
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS || MACCATALYST
                    handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Firebase Auth Client
            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyC5ScBBMofsRGs9qtLIfGDAY_gXr7peUD0",
                AuthDomain = "i-am-c9110.firebaseapp.com",
                Providers =
                [
                    new EmailProvider()
                ]//,
                //UserRepository = new FileUserRepository("I_am")
            }));

            // Services
            builder.Services.AddSingleton<FirestoreService>();
            builder.Services.AddSingleton<AuthService>();

            // Auth ViewModels & Views
            builder.Services.AddSingleton<SignInView>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignUpView>();
            builder.Services.AddSingleton<SignUpViewModel>();

            // ViewModels
            builder.Services.AddTransient<SampleVM>();
            builder.Services.AddTransient<UserVM>();
            builder.Services.AddTransient<CheckInVM>();
            builder.Services.AddTransient<InvitationVM>();
            builder.Services.AddTransient<RelationshipVM>();
            builder.Services.AddTransient<CalendarVM>();

            // Pages
            builder.Services.AddTransient<CheckInPage>();
            builder.Services.AddTransient<InvitationsPage>();
            builder.Services.AddTransient<RelationshipsPage>();
            builder.Services.AddTransient<CalendarPage>();

            return builder.Build();
        }
    }
}
