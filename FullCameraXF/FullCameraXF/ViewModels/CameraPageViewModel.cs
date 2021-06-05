using FullCameraXF.DataStores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FullCameraXF.ViewModels
{
    public class CameraPageViewModel : BaseViewModel
    {
        public CameraDataStore CameraDataStore {get ;set;}
        private bool flashLightOn { get; set; }
        public bool FlashLightOn
        {
            set
            {
                flashLightOn = value;
                OnPropertyChanged("FlashLightOn");
            }
            get
            {
                return flashLightOn;
            }
        }
        private bool hasFinishedTakingPhoto { get; set; }
        public bool HasFinishedTakingPhoto
        {
            set
            {
                hasFinishedTakingPhoto = value;
                OnPropertyChanged("HasFinishedTakingPhoto");
            }
            get
            {
                return hasFinishedTakingPhoto;
            }
        }

        public ICommand AddPhotoCommand { get; set; }

        public CameraPageViewModel()
        {
            CameraDataStore = new CameraDataStore();
            FlashLightOn = false;
            HasFinishedTakingPhoto = false;
            AddPhotoCommand = new Command(() => AddPhoto());
        }

        public async void AddPhoto()
        {
            CameraDataStore.AddPhotoToCameraFlowDataStorePhotoList();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
}
}
