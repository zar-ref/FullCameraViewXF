using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace FullCameraXF.ViewComponents.CustomRenderers
{
    public enum CameraOptions
    {
        Rear,
        Front
    }
    public class CameraPreview : View
    {

        public event EventHandler Subscribe;
        public event EventHandler Unsubscribe;

        public void OnSubscribe()
        {
            if (Subscribe != null)
            {
                Subscribe(this, EventArgs.Empty);
            }
        }
        public void OnUnsubscribe()
        {
            if (Unsubscribe != null)
            {
                Unsubscribe(this, EventArgs.Empty);
            }
        }
        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
          propertyName: "Camera",
          returnType: typeof(CameraOptions),
          declaringType: typeof(CameraPreview),
          defaultValue: CameraOptions.Rear);
        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }
    }
}
