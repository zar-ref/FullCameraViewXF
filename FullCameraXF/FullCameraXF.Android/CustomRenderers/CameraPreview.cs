using System;
using System.Collections.Generic;
using Android;
using Android.Content;
using Android.Hardware;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Java.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Content.PM;
using Android.Support.V4.App;
using System.IO;

using Android.Hardware.Camera2;

using FullCameraXF.DataStores;
using System.Drawing.Imaging;
using FullCameraXF.Droid.Helpers;

namespace FullCameraXF.Droid.CustomRenderers
{
    [Obsolete]
    public sealed class CameraPreview : ViewGroup, ISurfaceHolderCallback, Camera.IPictureCallback
    {
        SurfaceView surfaceView;
        ISurfaceHolder holder;
        Camera.Size previewSize;
        IList<Camera.Size> supportedPreviewSizes;
        Camera camera;
        IWindowManager windowManager;

        public bool IsPreviewing { get; set; }
        public Camera Preview
        {
            get { return camera; }
            set
            {
                camera = value;
                if (camera != null)
                {
                    supportedPreviewSizes = Preview.GetParameters().SupportedPreviewSizes;
                    RequestLayout();
                }
            }
        }
        public CameraPreview(Context context)
          : base(context)
        {
            surfaceView = new SurfaceView(context);
            AddView(surfaceView);

            windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            IsPreviewing = false;
            holder = surfaceView.Holder;
            holder.AddCallback(this);

          
        }
        internal void Subscribe()
        {
            MessagingCenter.Subscribe<object>(this, "TakePhoto", (e) =>
            {
                try
                {

                    this.Preview.TakePicture(null, null, this);
                }
                catch (Exception ex)
                {

                    var x = ex.Message;

                    try
                    {
                        camera.Reconnect();
                        camera.StartPreview();
                        camera.TakePicture(null, null, this);
                    }
                    catch (Exception yx)
                    {

                        var y = yx.Message;
                    }
                }


            });


            MessagingCenter.Subscribe<object>(this, "ToggleFlashLightOn", (e) =>
            {


                var camParameters = this.Preview.GetParameters();
                camParameters.FlashMode = Camera.Parameters.FlashModeTorch;
                this.Preview.SetParameters(camParameters);

            });
            MessagingCenter.Subscribe<object>(this, "ToggleFlashLightOff", (e) =>
            {
                var camParameters = this.Preview.GetParameters();
                camParameters.FlashMode = Camera.Parameters.FlashModeOff;
                this.Preview.SetParameters(camParameters);

            });
        }

        internal void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<object>(this, "TakePhoto");
            MessagingCenter.Unsubscribe<object>(this, "ToggleFlashLightOn");
            MessagingCenter.Unsubscribe<object>(this, "ToggleFlashLightOff");
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = ResolveSize(SuggestedMinimumWidth, widthMeasureSpec);
            int height = ResolveSize(SuggestedMinimumHeight, heightMeasureSpec);
            SetMeasuredDimension(width, height);

            if (supportedPreviewSizes != null)
            {
                previewSize = GetOptimalPreviewSize(supportedPreviewSizes, width, height);
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

            surfaceView.Measure(msw, msh);
            surfaceView.Layout(0, 0, r - l, b - t);
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            if (Preview == null)
                Preview = Camera.Open();
            try
            {
                if (Preview != null)
                {
                    Preview.SetPreviewDisplay(holder);
                    //Camera.Open();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            if (Preview != null)
            {
                Preview.StopPreview();
                Preview.SetPreviewCallback(null);
                Preview.Release();
                Preview = null;
                //camera.Release();

                //camera = null;
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            var parameters = Preview.GetParameters();
            parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
            RequestLayout();

            switch (windowManager.DefaultDisplay.Rotation)
            {
                case SurfaceOrientation.Rotation0:
                    camera.SetDisplayOrientation(90); //default
                    parameters.SetRotation(90);
                    //camera.SetDisplayOrientation(0);
                    break;
                case SurfaceOrientation.Rotation90:
                    camera.SetDisplayOrientation(0);
                    break;
                case SurfaceOrientation.Rotation270:
                    camera.SetDisplayOrientation(180);
                    break;
            }

            Preview.SetParameters(parameters);
            Preview.StartPreview();
            IsPreviewing = true;
        }

        Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
        {
            const double AspectTolerance = 0.1;
            double targetRatio = (double)w / h;

            if (sizes == null)
            {
                return null;
            }

            Camera.Size optimalSize = null;
            double minDiff = double.MaxValue;

            int targetHeight = h;
            foreach (Camera.Size size in sizes)
            {
                double ratio = (double)size.Width / size.Height;

                if (Math.Abs(ratio - targetRatio) > AspectTolerance)
                    continue;
                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            if (optimalSize == null)
            {
                minDiff = double.MaxValue;
                foreach (Camera.Size size in sizes)
                {
                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }

            return optimalSize;
        }

        public void OnPictureTaken(byte[] data, Camera camera)
        {
            camera.StopPreview();

            FileOutputStream outStream = null;



            if (data != null)
            {
                try
                {

                    //var documentsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                    //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    //var s = ts.TotalMilliseconds;
                    //outStream = new FileOutputStream(documentsPath + "/" + s + ".jpg");
                    //outStream.Write(data);
                    //outStream.Close();

                
                    var byteArrayToconvert = AndroidImageHelper.ResizeImageAndroid(data, 675, 1200);
                    var base64string = Convert.ToBase64String(byteArrayToconvert);


                    CameraDataStore.SetBase64Photo(base64string);
                    //PhotosDataStore.SetHasFinishedTakingPhoto(true);

                    MessagingCenter.Send<object>(this, "HasFinishedTakingPhoto");


                }
                catch (Java.IO.FileNotFoundException e)
                {
                    System.Console.Out.WriteLine(e.Message);
                    camera.StartPreview();
                }
                catch (Java.IO.IOException ie)
                {
                    System.Console.Out.WriteLine(ie.Message);
                    camera.StartPreview();
                }
            }
            //camera.StartPreview();
        }


    }
}