namespace CameraViewPOC;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        await Permissions.RequestAsync<Permissions.Camera>();
		cameraView.Start();
    }

    private void OnCounterClicked(object sender, EventArgs e)
	{
		cameraView.Shot();
	}
}

