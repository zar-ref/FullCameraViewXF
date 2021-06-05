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
        }
        public void OnTakePhoto(object sender, System.EventArgs e)
        {

            MessagingCenter.Send<object>(this, "TakePhoto");
            //MessagingCenter.Subscribe<object>(this, "HasFinishedTakingPhoto", (arg) =>
            //{
            //    _viewModel.HasFinishedTakingPhoto = true;

            //});

        }

        public  void OnAddPhoto(object sender, System.EventArgs e)
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
        public void OnSwitchCamera(object sender, System.EventArgs e)
        {

            if (cameraPreview.Camera == ViewComponents.CustomRenderers.CameraOptions.Rear)
                cameraPreview.Camera = ViewComponents.CustomRenderers.CameraOptions.Front;
            else
                cameraPreview.Camera = ViewComponents.CustomRenderers.CameraOptions.Rear;
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
    }
}