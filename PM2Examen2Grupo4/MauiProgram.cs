using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using Microsoft.Maui.Controls.Maps;

namespace PM2Examen2Grupo4
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit().UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkitMediaElement().UseMauiMaps();
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}