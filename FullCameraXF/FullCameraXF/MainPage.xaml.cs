using FullCameraXF.Models;
using FullCameraXF.Pages;
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
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CameraPage());
        }

        public void EmptyPhotosGallery()
        {
            photoGalleryStack.Children.Clear();
        }
        public StackLayout ConstructPhotosGallery(List<PhotoModel> photoList, int numberOfColumns)
        {
            StackLayout finalColumnVerticalStack = new StackLayout() { Orientation = StackOrientation.Vertical };
            StackLayout rowHorizontalStack = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(4) };
            if (photoList.Count == 0)
            {
                rowHorizontalStack.Children.Add(ConstructTakePhotoCard());
                finalColumnVerticalStack.Children.Add(rowHorizontalStack);
                return finalColumnVerticalStack;
            }
            for (int i = 0; i < photoList.Count + 1; i++)
            {
                if (i == 0)
                {
                   
                        rowHorizontalStack.Children.Add(ConstructTakePhotoCard());
                } 

                if (i > 1 && (i + 1) % numberOfColumns == 0)
                {
                    finalColumnVerticalStack.Children.Add(rowHorizontalStack);
                    rowHorizontalStack = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = new Thickness(4) };
                }
            }

            if (rowHorizontalStack.Children.Count > 0)
                finalColumnVerticalStack.Children.Add(rowHorizontalStack);
            return finalColumnVerticalStack;
        }

        public StackLayout ConstructTakePhotoCard()
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
            return stack;
        }

        public StackLayout ConstructPhotoCard(PhotoModel item, LayoutOptions position)
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
                Command = _viewModel.DeletePhotoCommand,
                CommandParameter = item,
                //add command to delete image...
            };
            deletePhotoFrame.Content = deletePhotoBtn;
            imgGrid.Children.Add(new ImageButton()
            {
                Source = item.PhotoImageSource,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFill,
                HeightRequest = 100,
                WidthRequest = 100,
                Command = _viewModel.GoToProductPicturePageCommand,
                CommandParameter = item
            }, 0, 0);
            imgGrid.Children.Add(deletePhotoFrame, 0, 0);
            stack.Children.Add(imgGrid);
            return stack;
        }
        public StackLayout ConstructPhotoCardViewOnly(PhotoModel item, LayoutOptions position)
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

            imgGrid.Children.Add(new ImageButton()
            {
                Source = item.PhotoImageSource,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFill,
                HeightRequest = 100,
                WidthRequest = 100,
                Command = _viewModel.GoToProductPicturePageCommand,
                CommandParameter = item
            }, 0, 0);

            stack.Children.Add(imgGrid);
            return stack;
        }
    }
}
