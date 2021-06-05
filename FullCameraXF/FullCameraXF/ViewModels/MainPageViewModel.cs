using FullCameraXF.DataStores;
using FullCameraXF.Models;
using FullCameraXF.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FullCameraXF.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {

        public CameraFlowDataStore CameraFlowDataStore { get; set; }
        public ICommand GoToPicturePageCommand { get; set; }
        public ICommand GoToCameraPageCommand { get; set; }
        public ICommand DeletePictureCommand { get; set; }
        public List<PhotoModel> PhotoList
        {
            get
            {
                return CameraFlowDataStore.PhotoList.ToList();
            }
        }
        public MainPageViewModel()
        {

            CameraFlowDataStore = new CameraFlowDataStore();
            GoToCameraPageCommand = new Command(() => GoToCameraPage());
            GoToPicturePageCommand = new Command<PhotoModel>((photo) => GoToPicturePage(photo));
            DeletePictureCommand = new Command<PhotoModel>((photo) => DeletePicture(photo));
        }
        public async void GoToPicturePage(PhotoModel photo)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PicturePage(photo));
        }

        public async void GoToCameraPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CameraPage());
        }

        public void DeletePicture(PhotoModel photo)
        {
            CameraFlowDataStore.DeletePhotoFromPhotoList(photo);      
            MessagingCenter.Send<object>(this, "UpdatePhotoGallery");
        }

    }
}
