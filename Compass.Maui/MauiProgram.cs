using Compass.Maui.Services;
using Compass.Maui.ViewModels;
using Compass.Maui.Views;
using Microsoft.Extensions.Logging;

namespace Compass.Maui
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
            //注册服务
            builder.Services.AddSingleton<MainView>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPlanService>();




#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
