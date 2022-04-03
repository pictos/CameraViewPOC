using Android.App;
using Android.Runtime;

namespace CameraViewPOC;

[Application]
public class MainApplication : MauiApplication
{
    internal static MainApplication Instance { get; private set; }

    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        Instance = this;
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
