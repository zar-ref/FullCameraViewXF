using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.IO;
using Android.Graphics;
using Android.Media;
using Orientation = Android.Media.Orientation;
namespace FullCameraXF.Droid.Helpers
{
    public static class AndroidImageHelper
    {
        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);




            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            var ms = new MemoryStream();
            resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 70, ms);
            return ms.ToArray();
        }


        public static byte[] ResizeAndRotateImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            var matrix = new Matrix();
            //matrix.PreRotate(90);
            //matrix.PostScale(-1, 1);


            Bitmap resizedImage = Bitmap.CreateBitmap(originalImage, 0, 0, (int)width, (int)height, matrix, false);

            var ms = new MemoryStream();
            resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
            return ms.ToArray();

        }
    }
}