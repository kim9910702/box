using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GBManager.Interfaces
{
    public interface IDeviceDisplayService
    {
        string Get();

        void SetKeepScreenOn(bool bKeepOn);
        bool GetKeepScreenOn();
    }

    public struct DeviceDisplayInfoData
    {
        public string density;
        public string height;
        public string width;
        public string orientation;
        public string refreshrate;
        public string rotation;

        public void Init()
        {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            density = mainDisplayInfo.Density.ToString();
            height = mainDisplayInfo.Height.ToString();
            width = mainDisplayInfo.Width.ToString();
            orientation = mainDisplayInfo.Orientation.ToString();
            refreshrate = mainDisplayInfo.RefreshRate.ToString();
            rotation = mainDisplayInfo.Rotation.ToString();
        }
    };
}
