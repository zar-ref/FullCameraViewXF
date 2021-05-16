using FullCameraXF.DataStores;
using System;
using System.Collections.Generic;
using System.Text;

namespace FullCameraXF.ViewModels
{
    public class CameraPageViewModel
    {
        public CameraDataStore CameraDataStore {get ;set;}
        public CameraPageViewModel()
        {
            CameraDataStore = new CameraDataStore();
        }
}
}
