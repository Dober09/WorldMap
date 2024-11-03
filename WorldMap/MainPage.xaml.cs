using Camera.MAUI;
using Microsoft.Maui.Controls;
using System.IO;
using System.Runtime.CompilerServices;

namespace WorldMap
{
    public partial class MainPage : ContentPage
    {
        private bool isCameraInitialized = false;
        private bool hasPermission = false;
        public MainPage()
        {
            InitializeComponent();

            // Ensure the camera view is properly initialized
            if (cameraView != null)
            {
                CheckAndRequestPermission();
                UpdateCameraViewSize();
            }


        }

        private async Task CheckAndRequestPermission()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if(status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }

                hasPermission = status == PermissionStatus.Granted;

                if (!hasPermission)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await DisplayAlert("Permission Required", "Camer permission is requieed for this feature", "OK");
                    });
                }
                
            }catch (Exception ex)
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
                            await cameraView.StopCameraAsync();
                            await Task.Delay(100);
                            await cameraView.StartCameraAsync();
                            isCameraInitialized = true;
                        }
                        catch (Exception ex)
                        {
                            await DisplayAlert("Camera Error", $"Failed to start camera: {ex.Message}", "OK");
                        }
                    });
                }
            }catch(Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Error",
                        $"Camera initialization error: {ex.Message}", "OK");
                });
            }
        }


        private async void CameraView_CamerasLoaded(object sender, EventArgs e)
        {
            if (!hasPermission)
            {
                await CheckAndRequestPermission();
            }

            if(hasPermission && !isCameraInitialized && cameraView?.NumCamerasDetected > 0)
            {
                await InitializeCamera();
            }
        }

        private void UpdateCameraViewSize()
        {
            var  mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
            double widthInDp = mainDisplayInfo.Width / mainDisplayInfo.Density;
            double heightInDp = mainDisplayInfo.Height / mainDisplayInfo.Density;


            MainThread.BeginInvokeOnMainThread(() =>
            {
                cameraView.WidthRequest = widthInDp;
                cameraView.HeightRequest = heightInDp;
            });
        } 

        //private void CameraView_CamerasLoaded(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cameraView?.NumCamerasDetected > 0)
        //        {
        //            var camera = cameraView.Cameras.FirstOrDefault();
        //            if (camera != null)
        //            {
        //                cameraView.Camera = camera;
        //                MainThread.BeginInvokeOnMainThread(async () =>
        //                {
        //                    try
        //                    {
        //                        await cameraView.StartCameraAsync();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        await DisplayAlert("Camera Error",
        //                            $"Failed to start camera: {ex.Message}", "OK");
        //                    }
        //                });
        //            }
        //        }
        //        else
        //        {
        //            MainThread.BeginInvokeOnMainThread(async () =>
        //            {
        //                await DisplayAlert("No Camera",
        //                    "No cameras were detected on this device.", "OK");
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MainThread.BeginInvokeOnMainThread(async () =>
        //        {
        //            await DisplayAlert("Error",
        //                $"Camera initialization error: {ex.Message}", "OK");
        //        });
        //    }
        //}

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

        private async void OnCaptureClicked(object sender, EventArgs e)
        {
            var stream = await cameraView.TakePhotoAsync();
            try
            {

                if (stream != null)
                {
                    var result = ImageSource.FromStream(() =>
                    {
                        var memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        stream.Dispose();
                        memoryStream.Position = 0;
                        return memoryStream;
                    });


                    flareBtn.Source = "flare.png";
                    snapPreview.Source = result;
                }

            }catch (Exception ex)
            {
                Console.WriteLine($"Error capturing photo: {ex.Message}");
            }
        }

        
        private FlashMode currentFlashMode = FlashMode.Auto;
        private void OnFlashClicked(object sender, EventArgs e)
        {

            switch (currentFlashMode)
            {
                case FlashMode.Disabled:
                    currentFlashMode = FlashMode.Enabled;
                    flashBtn.Source = "flash.png";
                    break;

                case FlashMode.Enabled:
                    currentFlashMode = FlashMode.Auto;
                    flashBtn.Source = "blueflash.png";
                    break;

                case FlashMode.Auto:
                    currentFlashMode = FlashMode.Disabled;
                    break;

            }

            cameraView.FlashMode = currentFlashMode;
        }
    }
}