using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FullCameraXF;
using FullCameraXF.ViewComponents.CustomRenderers;
using iOSCameraPOC.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraPreviewRenderer))]
namespace iOSCameraPOC.iOS.CustomRenderers
{
	public class CameraPreviewRenderer : ViewRenderer<CameraPreview, UICameraPreview>
	{
		UICameraPreview uiCameraPreview;

		protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				// Unsubscribe
				uiCameraPreview.Tapped -= OnCameraPreviewTapped;
			}
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					uiCameraPreview = new UICameraPreview(e.NewElement.Camera);
					SetNativeControl(uiCameraPreview);
				}
				e.NewElement.Subscribe += (sender, ev) =>
				{
					Control.Subscribe();
				};
				e.NewElement.Unsubscribe += (sender, evs) =>
				{
					Control.Unsubscribe();
				};

				// Subscribe
				uiCameraPreview.Tapped += OnCameraPreviewTapped;
			}
		}

		void OnCameraPreviewTapped(object sender, EventArgs e)
		{
			if (uiCameraPreview.IsPreviewing)
			{
				uiCameraPreview.CaptureSession.StopRunning();
				uiCameraPreview.IsPreviewing = false;
			}
			else
			{
				uiCameraPreview.CaptureSession.StartRunning();
				uiCameraPreview.IsPreviewing = true;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.CaptureSession.Dispose();
				Control.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
