using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Plugin.LocalNotification;

namespace MobileApplication
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            #if DEBUG
		    builder.Logging.AddDebug();
            #endif
                 
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<ViewTermPage>();
            builder.Services.AddTransient<ViewCoursePage>();
            builder.Services.AddTransient<ViewAssessmentPage>();
            builder.Services.AddTransient<AddTermPage>();
            builder.Services.AddTransient<AddCoursePage>();
            builder.Services.AddTransient<AddAssessmentPage>();
            builder.Services.AddTransient<EditTermPage>();
            builder.Services.AddTransient<EditCoursePage>();
            builder.Services.AddTransient<EditAssessmentPage>();            

            return builder.Build();
        }
    }
}