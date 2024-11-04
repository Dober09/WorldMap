using Camera.MAUI;

namespace WorldMap.View;

public partial class LandingPage : ContentPage
{
    private bool isCameraInitialized = false;
    private bool hasPermission = false;
    private bool _isScanning = false;
    public LandingPage()
    {
        InitializeComponent();
    }


  
    private async void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (!hasPermission)
        {
            await CheckAndRequestPermission();
        }

        if (hasPermission && !isCameraInitialized && cameraView?.NumCamerasDetected > 0)
        {
           
            await InitializeCamera();
        }

    }

    private async Task CheckAndRequestPermission()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            hasPermission = status == PermissionStatus.Granted;

            if (!hasPermission)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Permission Required", "Camera permission is requieed for this feature", "OK");
                });
            }

        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error", $"Permission error: {ex.Message}", "OK");
            });
        }
    }

    private async Task InitializeCamera()
    {
        try
        {
            var camera = cameraView.Cameras.FirstOrDefault();
            if (camera != null)
            {
                cameraView.Camera = camera;
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    try
                    {
                        //await cameraView.StopCameraAsync();
                        await Task.Delay(100);
                        await cameraView.StartCameraAsync();
                        SetupBarcodeScanning();
                        isCameraInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Camera Error", $"Failed to start camera: {ex.Message}", "OK");
                    }


                });
            }
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await DisplayAlert("Error",
                    $"Camera initialization error: {ex.Message}", "OK");
            });
        }
    }


    private void SetupBarcodeScanning()
    {

        // Defined which barcode formats to scan form
        var barcodeFormats = BarcodeFormat.EAN_13 | BarcodeFormat.UPC_A |
            BarcodeFormat.CODE_128;


        ;
        cameraView.BarCodeOptions = new BarcodeDecodeOptions
        {
            AutoRotate = true,
            PossibleFormats
            = { barcodeFormats },
            TryHarder = true,
        };
        



    }

    private void CameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {

            //Display the barcode result 
            barcodeResult.Text = $"Barcode: {args.Result[0].Text}\nFormat: {args.Result[0].BarcodeFormat}";

            // optionslly pause scanning after defection
            if (_isScanning)
            {
                _isScanning = false;
                cameraView.BarCodeDetectionEnabled = false;
                toogleScannningButton.Text = "Start Scanning";
            }
        });
    }

    private void OnToggleScanningClicked(object sender, EventArgs e)
    {
        _isScanning = ! _isScanning;
        cameraView.BarCodeDetectionEnabled= _isScanning;
        toogleScannningButton.Text = _isScanning ? "Stop Scanning" : "Start Scanning";

        if (!_isScanning) {
            barcodeResult.Text = string.Empty;
       }
    }

    private void OnSwitchCameraClicked(object sender, EventArgs e)
    {
      //  cameraView.Camera = cameraView.Camera?.Name == cameraView.Cameras.First().Name
      //      ? cameraView.Cameras.Last()
      //      : cameraView.Cameras.First();
      // Console.WriteLine(cameraView.Cameras.Count);

        var currentCamera = cameraView.Camera;  
        var cameras = cameraView.Cameras.ToList();
        var currentIdx = cameras.IndexOf(currentCamera);
        var nextIdx = (currentIdx + 1) % cameras.Count;
        cameraView.Camera = cameras[nextIdx];
    }

    /// <summary>
    /// the size of the camare
    /// </summary>
    private void UpdateCameraViewSize()
    {
        var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
        double widthInDp = mainDisplayInfo.Width / mainDisplayInfo.Density;
        double heightInDp = mainDisplayInfo.Height / mainDisplayInfo.Density;


        MainThread.BeginInvokeOnMainThread(() =>
        {
            cameraView.WidthRequest = widthInDp;
            cameraView.HeightRequest = heightInDp;
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MainThread.BeginInvokeOnMainThread(async () =>
        {

            await cameraView.StopCameraAsync();
        });
    }
}