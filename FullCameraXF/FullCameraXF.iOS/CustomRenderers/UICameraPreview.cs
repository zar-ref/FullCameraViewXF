using System;
using System.Linq;
using AVFoundation;
using CoreGraphics;
using Foundation;
using FullCameraXF.DataStores;
using FullCameraXF.ViewComponents.CustomRenderers;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace iOSCameraPOC.iOS.CustomRenderers
{
    public class UICameraPreview : UIView
    {
        AVCaptureVideoPreviewLayer previewLayer;
        CameraOptions cameraOptions;

        public event EventHandler<EventArgs> Tapped;

        public AVCaptureSession CaptureSession { get; private set; }

        public bool IsPreviewing { get; set; }


        public AVCaptureStillImageOutput StillImageOutput;
        public UICameraPreview(CameraOptions options)
        {
            cameraOptions = options;
            IsPreviewing = false;
            Initialize();
        }


        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (previewLayer != null)
                previewLayer.Frame = Bounds;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            OnTapped();
        }

        protected virtual void OnTapped()
        {
            var eventHandler = Tapped;
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        void Initialize()
        {
            CaptureSession = new AVCaptureSession();
            previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
            {
                Frame = Bounds,
                VideoGravity = AVLayerVideoGravity.ResizeAspectFill
            };

            var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
            var cameraPosition = (cameraOptions == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
            var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition);

            if (device == null)
            {
                return;
            }
            NSError error;
            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            StillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };



            var input = new AVCaptureDeviceInput(device, out error);
            CaptureSession.AddInput(input);
            CaptureSession.AddOutput(StillImageOutput);
            Layer.AddSublayer(previewLayer);
            CaptureSession.StartRunning();
            IsPreviewing = true;

        }

        internal void Subscribe()
        {
            MessagingCenter.Subscribe<object>(this, "TakePhoto", async (e) =>
            {

                var videoConnection = StillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
                var sampleBuffer = await StillImageOutput.CaptureStillImageTaskAsync(videoConnection);
                var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
                var imageByteArray = jpegImageAsNsData.ToArray();
                var base64string = Convert.ToBase64String(imageByteArray);
                CameraDataStore.SetBase64Photo(base64string);
                //PhotosDataStore.SetHasFinishedTakingPhoto(true);
                MessagingCenter.Send<object>(this, "HasFinishedTakingPhoto");
            });

            MessagingCenter.Subscribe<object>(this, "ToggleFlashLightOn", async (e) =>
            {

                try
                {
                    // Turn On Flashlight  
                    await Flashlight.TurnOnAsync();
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    return;
                }
                catch (PermissionException pEx)
                {
                    return;
                }
                catch (Exception ex)
                {
                    return;
                }

            });
            MessagingCenter.Subscribe<object>(this, "ToggleFlashLightOff", async (e) =>
            {
                try
                {
                    // Turn On Flashlight  
                    await Flashlight.TurnOffAsync();
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    return;
                }
                catch (PermissionException pEx)
                {
                    return;
                }
                catch (Exception ex)
                {
                    return;
                }

            });
        }
        internal void Unsubscribe()
        {
            MessagingCenter.Unsubscribe<object>(this, "TakePhoto");
            MessagingCenter.Unsubscribe<object>(this, "ToggleFlashLightOn");
            MessagingCenter.Unsubscribe<object>(this, "ToggleFlashLightOff");
        }


    }
}
