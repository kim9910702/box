using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace BoxAd.Interfaces
{
    public interface IDeviceInfoService
    {
        string Get();
    }

    public struct DeviceInfoData
    {
        public string deviceType;
        public string idiom;
        public string manufacturer;
        public string model;
        public string name;
        public string platform;
        public string version;
        public string versionstring;

        public void Init()
        {
            deviceType = DeviceInfo.DeviceType.ToString();
            idiom = DeviceInfo.Idiom.ToString();
            manufacturer = DeviceInfo.Manufacturer.ToString();
            model = DeviceInfo.Model.ToString();
            name = DeviceInfo.Name.ToString();
            platform = DeviceInfo.Platform.ToString();
            version = DeviceInfo.Version.ToString();
            versionstring = DeviceInfo.VersionString.ToString();
        }
    };
}
