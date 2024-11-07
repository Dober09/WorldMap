using Microsoft.Extensions.Logging;
using WorldMap.Services;
using WorldMap.View;
using WorldMap.ViewModel;

namespace WorldMap
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
                    
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                    fonts.AddFont("Poppins-Medium.ttf", "PoppinsMedium");
                    fonts.AddFont("Poppins-Light.ttf", "PoppinsLight");
                    fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                    fonts.AddFont("Poppins-Italic.ttf", "PoppinsItalic");
                });

            builder.Services.AddSingleton<IDataService, DataServices>();
            builder.Services.AddSingleton<CountryViewModel>();
            builder.Services.AddTransient<LandingPage>();


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
