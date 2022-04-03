using CameraViewPOC.Platforms.Android;

namespace CameraViewPOC;
internal class CameraView : View
{
    public void Shot()
    {
        (this.Handler as CameraViewHandler).PlatformShot();
    }

    public void Start()
    {
        if (Handler is null)
        {
            this.HandlerChanged += (_, __) => Start();
            return;
        }
        (this.Handler as CameraViewHandler).PlatformStart();
    }
}
