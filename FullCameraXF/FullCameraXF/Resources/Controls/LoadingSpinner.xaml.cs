using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T2020.Mobile.Resources.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingSpinner : StackLayout
    {
        public LoadingSpinner()
        {
            InitializeComponent();
        }
    }
}