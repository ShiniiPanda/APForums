using APForums.Client.Data;
using APForums.Client.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace APForums.Client
{
    public static class MauiProgram
    {
        public static string ServerAddress =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5186/" : "http://localhost:5186/";

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
            builder.Services.AddSingleton<ILoginService, LoginService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IClubService, ClubService>();
            builder.Services.AddSingleton<IForumService, ForumService>();
            builder.Services.AddSingleton<ITagService, TagService>();
            builder.Services.AddSingleton<IEventService, EventService>();
            builder.Services.AddSingleton<ISocialService, SocialService>();
            builder.Services.AddSingleton<IPostService, PostService>();
            builder.Services.AddSingleton<IActivityService, ActivityService>();

            return builder.Build();
        }
    }
}