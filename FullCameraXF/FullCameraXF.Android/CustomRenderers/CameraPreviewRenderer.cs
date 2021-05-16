
using System;
using Android.Content;
using Android.Hardware;
using FullCameraXF.Droid;
using FullCameraXF.Droid.CustomRenderers;
using FullCameraXF.ViewComponents.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FullCameraXF.ViewComponents.CustomRenderers.CameraPreview), typeof(CameraPreviewRenderer))]
namespace FullCameraXF.Droid.CustomRenderers
{
    public class CameraPreviewRenderer : ViewRenderer<FullCameraXF.ViewComponents.CustomRenderers.CameraPreview, FullCameraXF.Droid.CustomRenderers.CameraPreview>
    {
        CameraPreview cameraPreview;

        public CameraPreviewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FullCameraXF.ViewComponents.CustomRenderers.CameraPreview> ex)
        {
            base.OnElementChanged(ex);

            if (ex.OldElement != null)
            {
                // Unsubscribe
                cameraPreview.Click -= OnCameraPreviewClicked;
            }
            if (ex.NewElement != null)
            {
                if (Control == null)
                {
                    cameraPreview = new CameraPreview(Context);
                    SetNativeControl(cameraPreview);
                }
                Control.Preview = Camera.Open((int)ex.NewElement.Camera);
                ex.NewElement.Subscribe += (sender, e) =>
                {
                    Control.Subscribe();
                };
                ex.NewElement.Unsubscribe += (sender, e) =>
                {
                    Control.Unsubscribe();
                };

                // Subscribe
                cameraPreview.Click += OnCameraPreviewClicked;
            }
        }

        void OnCameraPreviewClicked(object sender, EventArgs e)
        {
            if (cameraPreview.IsPreviewing)
            {
                cameraPreview.Preview.StopPreview();
                cameraPreview.IsPreviewing = false;
            }
            else
            {
                cameraPreview.Preview.StartPreview();
                cameraPreview.IsPreviewing = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
           
            base.Dispose(disposing);
        }
    }
}