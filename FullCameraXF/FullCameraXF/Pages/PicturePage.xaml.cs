using FullCameraXF.Models;
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
    public partial class PicturePage : ContentPage
    {
        public PicturePage(PhotoModel photoModel)
        {
            InitializeComponent();
            if (photoModel.PhotoByteArray.Length > 0)
                photo.Source = photoModel.PhotoImageSource;
            else
                photo.Source = null;
        }
    }
}