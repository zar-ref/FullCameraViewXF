using FullCameraXF.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FullCameraXF.DataStores
{
    public class CameraFlowDataStore
    {

        public static List<PhotoModel> PhotoList { get; set; }

        public CameraFlowDataStore()
        {
            if (PhotoList == null)
                PhotoList = new List<PhotoModel>();
        }

        public void DeletePhotoFromPhotoList(PhotoModel photo)
        {
            PhotoList.Remove(photo);
        }
    }
}
