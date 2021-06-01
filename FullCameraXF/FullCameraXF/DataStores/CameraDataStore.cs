using FullCameraXF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FullCameraXF.DataStores
{
    public class CameraDataStore
    {

        public CameraDataStore() { }

        public static string Base64Photo { get; set; }
        public static void SetBase64Photo(string base64Photo)
        {
            Base64Photo = base64Photo;
        }



        public void AddPhotoToCameraFlowDataStorePhotoList()
        {
            CameraFlowDataStore.PhotoList.Add(new PhotoModel() { PhotoByteArray = Convert.FromBase64String(Base64Photo) });
            SetBase64Photo(null);
        }
    }
}
