using FullCameraXF.ViewComponents.CustomRenderers;
using FullCameraXF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FullCameraXF.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        public CameraPageViewModel _viewModel { get; set; }

        public CameraPage()
        {
            InitializeComponent();
            _viewModel = new CameraPageViewModel();
            BindingContext = _viewModel;
            //var newCamera = new CameraPreview() { Camera = CameraOptions.Rear , HorizontalOptions = LayoutOptions.FillAndExpand , VerticalOptions = LayoutOptions.FillAndExpand};
          
            //cameraStack.Children.Add(newCamera);
        }
        public void OnTakePhoto(object sender, System.EventArgs e)
        {

            MessagingCenter.Send<object>(this, "TakePhoto");
            MessagingCenter.Subscribe<object>(this, "HasFinishedTakingPhoto", (arg) =>
            {
                _viewModel.HasFinishedTakingPhoto = true;

            });

        }

        public void OnAddPhoto(object sender, System.EventArgs e)
        {
            //_viewModel.IsLoading = true;
            _viewModel.AddPhotoCommand.Execute(null);
            //_viewModel.IsLoading = false;
        }


        public void OnToggleFlashLight(object sender, System.EventArgs e)
        {

            if (!_viewModel.FlashLightOn)
            {
                MessagingCenter.Send<object>(this, "ToggleFlashLightOn");
                _viewModel.FlashLightOn = true;
            }

            else
            {
                MessagingCenter.Send<object>(this, "ToggleFlashLightOff");
                _viewModel.FlashLightOn = false;
            }
        }
        public async void OnSwitchCamera(object sender, System.EventArgs e)
        {
            //var currentCamera = (CameraPreview)cameraStack.Children.FirstOrDefault();
            //if (currentCamera.Camera == CameraOptions.Rear)        
  
            if (cameraPreview.Camera == CameraOptions.Rear)
            {
                MessagingCenter.Send<object>(this, "ToggleCameraFront");
                await BeginInvokeOnMainThreadAsync(() => SwitchCamera(CameraOptions.Front));
                 

            }

            else
            {
                MessagingCenter.Send<object>(this, "ToggleCameraRear");
                await BeginInvokeOnMainThreadAsync(() => SwitchCamera(CameraOptions.Rear));
            }



        }


        protected override void OnDisappearing()
        {

            base.OnDisappearing();
            cameraPreview.OnUnsubscribe();
 

        }
        protected override void OnAppearing()
        {
            base.OnDisappearing(); 
            cameraPreview.OnSubscribe();

        }

        public Task SwitchCamera(CameraOptions option)
        {
            cameraStack.Children.Clear();
 
            cameraPreview.OnUnsubscribe();
            cameraPreview = null;
            cameraPreview = new CameraPreview() { Camera = option, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
             
            cameraStack.Children.Add(cameraPreview);
            cameraPreview.OnSubscribe();
            return Task.CompletedTask;
        }

        private Task<bool> BeginInvokeOnMainThreadAsync(Func<Task> a)
        {
            var tcs = new TaskCompletionSource<bool>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await a();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
    }
}