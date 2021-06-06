using FullCameraXF.Models;
using FullCameraXF.Pages;
using FullCameraXF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FullCameraXF
{
    public partial class MainPage : ContentPage
    {
        public MainPageViewModel _viewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.Run(async () =>
             {
                 _viewModel.IsLoading = true;       
                 await BeginInvokeOnMainThreadAsync(() => ConstructPhotosGalleryAsync(_viewModel.PhotoList, 3));
                 _viewModel.IsLoading = false;
             });



            SubscribeToUpdatePhotosGalleryMessage();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            UnsubscribeToUpdatePhotosGalleryMessage();
        }
        public void SubscribeToUpdatePhotosGalleryMessage()
        {
            MessagingCenter.Subscribe<object>(this, "UpdatePhotoGallery", async (sender) =>
           {
              
               await Task.Run(async () =>
              {
                  _viewModel.IsLoading = true;                   
                  await BeginInvokeOnMainThreadAsync(() => ConstructPhotosGalleryAsync(_viewModel.PhotoList, 3));
                  _viewModel.IsLoading = false;
              });
           });

        }
        public void UnsubscribeToUpdatePhotosGalleryMessage()
        {
            MessagingCenter.Unsubscribe<object>(this, "UpdatePhotoGallery");
        }
       

     
        public async Task ConstructPhotosGalleryAsync(List<PhotoModel> photoList, int numberOfColumns)
        {


            photoGalleryStack.Children.Clear();
            StackLayout rowHorizontalStack = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(4) };
            if (photoList.Count == 0)
            {
                rowHorizontalStack.Children.Add(await ConstructTakePhotoCardAsync());
                photoGalleryStack.Children.Add(rowHorizontalStack);

                return;
            }
            for (int i = 0; i < photoList.Count + 1; i++)
            {

                if (i == 0)
                {

                    rowHorizontalStack.Children.Add(await ConstructTakePhotoCardAsync());
                }
                else
                {
                    rowHorizontalStack.Children.Add(await ConstructPhotoCardAsync(photoList[i - 1], LayoutOptions.Start));
                }

                if (i > 1 && (i + 1) % numberOfColumns == 0)
                {
                    photoGalleryStack.Children.Add(rowHorizontalStack);
                    rowHorizontalStack = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(4) };
                }
            }

            if (rowHorizontalStack.Children.Count > 0)
                photoGalleryStack.Children.Add(rowHorizontalStack);
            return;
        }

        public async Task<StackLayout> ConstructTakePhotoCardAsync()
        {
            StackLayout stack = new StackLayout() { BackgroundColor = Color.LightGray };
            ImageButton takePhotoBtn = new ImageButton()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Source = "icon_large_camera",
                BackgroundColor = Color.Transparent,
                Command = _viewModel.GoToCameraPageCommand,
                HeightRequest = 100,
                WidthRequest = 100,
            };
            stack.Children.Add(takePhotoBtn);
            return await Task.FromResult(stack);
        }

        public async Task<StackLayout> ConstructPhotoCardAsync(PhotoModel item, LayoutOptions position)
        {
            StackLayout stack = new StackLayout() { HorizontalOptions = position };
            Grid imgGrid = new Grid()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1 , GridUnitType.Star)}
                }
            };
            Frame deletePhotoFrame = new Frame()
            {
                HeightRequest = 28,
                WidthRequest = 28,
                CornerRadius = 14,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                Padding = new Thickness(0)
            };

            ImageButton deletePhotoBtn = new ImageButton()
            {
                InputTransparent = false,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(3),
                Source = "icon_trash",
                BackgroundColor = Color.Transparent,
                Command = _viewModel.DeletePictureCommand,
                CommandParameter = item,
                //add command to delete image...
            };
            deletePhotoFrame.Content = deletePhotoBtn;
            imgGrid.Children.Add(new Image()
            {
                Source = item.PhotoImageSource,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFill,
                HeightRequest = 100,
                WidthRequest = 100,
                
            }, 0, 0);
            imgGrid.Children.Add(new Button()
            {
       
                HeightRequest = 100,
                WidthRequest = 100,
                Command = _viewModel.GoToPicturePageCommand,
                CommandParameter = item,
                BackgroundColor = Color.Transparent
            }, 0, 0);
            imgGrid.Children.Add(deletePhotoFrame, 0, 0);
            stack.Children.Add(imgGrid);
            return await Task.FromResult(stack);
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
