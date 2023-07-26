using APForums.Client.Data;
using Microsoft.Extensions.Logging;

namespace APForums.Client
{
    public static class MauiProgram
    {
        public static string ServerAddress =
            DeviceInfo.Platform == DevicePlatform.Android ? "10" : "https://localhost:7122/";

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}