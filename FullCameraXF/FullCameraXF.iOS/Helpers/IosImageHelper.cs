using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace FullCameraXF.iOS.Helpers
{
    public static class IosImageHelper
    {

        public static byte[] ResizeAndCompressImageIOS(byte[] imageData, float width, float height)
        {
            try
            {
                UIImage originalImage = ImageFromByteArray(imageData);
                UIImageOrientation orientation = originalImage.Orientation;

                //create a 24bit RGB image
                using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                     (int)width, (int)height, 8,
                                                     4 * (int)width, CGColorSpace.CreateDeviceRGB(),
                                                     CGImageAlphaInfo.PremultipliedFirst))
                {

                    RectangleF imageRect = new RectangleF(0, 0, width, height);

                    // draw the image
                    context.DrawImage(imageRect, originalImage.CGImage);
                    UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);
                    var compressedImage = CompressImage(resizedImage.AsJPEG().ToArray(), 70);
                    return compressedImage;

                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            //
            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }

        private static byte[] CompressImage(byte[] imgBytes, int compressionQuality)
        {
            try
            {
                NSData data = NSData.FromArray(imgBytes);
                var image = UIImage.LoadFromData(data);

                data = image.AsJPEG((nfloat)compressionQuality / 100);
                byte[] bytes = new byte[data.Length];
                System.Runtime.InteropServices.Marshal.Copy(data.Bytes, bytes, 0, Convert.ToInt32(data.Length));
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
