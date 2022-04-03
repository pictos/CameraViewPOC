using CameraViewPOC.Platforms.Android;

namespace CameraViewPOC;

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
            })
#if ANDROID
            .ConfigureMauiHandlers(h =>
            {
                h.AddHandler<CameraView, CameraViewHandler>();
            })
#endif
            ;
        return builder.Build();
    }
}
