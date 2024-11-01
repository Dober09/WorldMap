using Camera.MAUI;
using Microsoft.Maui.Controls;

namespace WorldMap
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Ensure the camera view is properly initialized
            if (cameraView != null)
            {
                InitializeCamera();
            }
        }

        private async void InitializeCamera()
        {
            try
            {
                // Wait for permissions
                var status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission required",
                        "Camera permission is required to use this feature.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",
                    $"Permission error: {ex.Message}", "OK");
                return;
            }
        }

        private void CameraView_CamerasLoaded(object sender, EventArgs e)
        {
            try
            {
                if (cameraView?.NumCamerasDetected > 0)
                {
                    var camera = cameraView.Cameras.FirstOrDefault();
                    if (camera != null)
                    {
                        cameraView.Camera = camera;
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            try
                            {
                                await cameraView.StartCameraAsync();
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Camera Error",
                                    $"Failed to start camera: {ex.Message}", "OK");
                            }
                        });
                    }
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("No Camera",
                            "No cameras were detected on this device.", "OK");
                    });
                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Error",
                        $"Camera initialization error: {ex.Message}", "OK");
                });
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (cameraView?.Camera != null)
                    {
                        await cameraView.StopCameraAsync();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error stopping camera: {ex.Message}");
                }
            });
        }
    }
}