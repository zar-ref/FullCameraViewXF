using System;
using System.Collections.Generic;
using System.Text;

namespace FullCameraXF.DataStores
{
    public class CameraDataStore
    {

        public static string Base64Photo { get; set; }
        public static void SetBase64Photo(string base64Photo)
        {
            Base64Photo = base64Photo;
        }
    }
}
