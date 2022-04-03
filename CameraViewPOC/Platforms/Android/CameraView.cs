using Android.Content;
using AndroidX.Camera.Core;
using AndroidX.Camera.Lifecycle;
using AndroidX.Camera.View;
using AndroidX.Core.Content;
using AndroidX.Lifecycle;
using Java.Lang;
using Microsoft.Maui.Handlers;
using Java.Util.Concurrent;

namespace CameraViewPOC.Platforms.Android;


internal sealed class CameraDroidManager
{
    private readonly IMauiContext mauiContext;
    readonly Context context;
    private PreviewView previewView;
    private IExecutorService cameraExecutor;
    private ProcessCameraProvider cameraProvider;
    private ImageCapture imageCapture;

    public CameraDroidManager(IMauiContext mauiContext)
    {
        this.mauiContext = mauiContext;
        context = mauiContext.Context;
    }

    public PreviewView CreateNativeView()
    {
        previewView = new PreviewView(mauiContext.Context);
        cameraExecutor = Executors.NewSingleThreadExecutor();

        return previewView;
    }

    public void Start()
    {
        var cameraProviderFuture = ProcessCameraProvider.GetInstance(context);
        cameraProviderFuture.AddListener(new Runnable(() =>
        {
            cameraProvider = (ProcessCameraProvider)cameraProviderFuture.Get();

            cameraProvider.UnbindAll();

            var cameraPreview = new Preview.Builder()
            .SetCameraSelector(CameraSelector.DefaultBackCamera)
            .Build();
            cameraPreview.SetSurfaceProvider(previewView.SurfaceProvider);


            imageCapture = new ImageCapture.Builder()
            .SetCaptureMode(ImageCapture.CaptureModeMaximizeQuality)
            .Build();


            var owner = (ILifecycleOwner)context;
            var camera = cameraProvider.BindToLifecycle(owner, CameraSelector.DefaultBackCamera, cameraPreview, imageCapture);

            //start the camera with AutoFocus
            MeteringPoint point = previewView.MeteringPointFactory.CreatePoint(previewView.Width / 2, previewView.Height / 2, 0.1F);
            FocusMeteringAction action = new FocusMeteringAction.Builder(point)
                                                                .DisableAutoCancel()
                                                                .Build();
            camera.CameraControl.StartFocusAndMetering(action);

        }), ContextCompat.GetMainExecutor(context));
    }
    
    public void TakePhoto()
    {
        imageCapture.TakePicture(cameraExecutor, new ImageCallBack());
    }

    private class ImageCallBack : ImageCapture.OnImageCapturedCallback
    {
        public override void OnCaptureSuccess(IImageProxy image)
        {
            base.OnCaptureSuccess(image);
            
            var buffer = image.GetPlanes()[0].Buffer;

            if (buffer is null)
                return;

            var imgData = new byte[buffer.Capacity()];
            buffer.Get(imgData);
        }

        public override void OnError(ImageCaptureException exception)
        {
            base.OnError(exception);
        }
    }
}

internal sealed class CameraViewHandler : ViewHandler<CameraView, AndroidX.Camera.View.PreviewView>
{
    CameraDroidManager cameraManager;
    public CameraViewHandler() : base(ViewMapper)
    {

    }

    public void PlatformShot()
    {
        cameraManager.TakePhoto();
    }

    public void PlatformStart()
    {
        cameraManager.Start();
    }

    protected override void ConnectHandler(PreviewView platformView)
    {
        base.ConnectHandler(platformView);
    }

    protected override PreviewView CreatePlatformView()
    {
        cameraManager = new(MauiContext);
        return cameraManager.CreateNativeView();
    }
}
