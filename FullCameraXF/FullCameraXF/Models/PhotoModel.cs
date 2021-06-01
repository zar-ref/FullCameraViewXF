using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace FullCameraXF.Models
{
    public class PhotoModel : BaseModel
    {
        public byte[] PhotoByteArray { get; set; }
        public ImageSource PhotoImageSource
        {
            get
            {
                if (PhotoByteArray == null)
                    return null;
                if (PhotoByteArray.Length == 0)
                    return null;
                else
                {
                    Stream stream = new MemoryStream(PhotoByteArray);
                    var imageSource = ImageSource.FromStream(() => stream);
                    return imageSource;
                }
            }
        }
    }
}
