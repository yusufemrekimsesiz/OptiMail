using Microsoft.Extensions.Logging;
using OptiMail.Services;
using OptiMail.ViewModels;


namespace OptiMail
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddSingleton<IProcessedEmailTracker, ProcessedEmailTracker>();
            builder.Services.AddSingleton<ICredentialService, CredentialService>();

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IAiAnalysisService, AiAnalysisService>();

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();

            return builder.Build();
        }
    }
}
