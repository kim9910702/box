using Foundation;
using BoxAd.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Essentials;
using BoxAd.iOS.InfoServices;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceDisplayService))]
namespace BoxAd.iOS.InfoServices
{
    class DeviceDisplayService : IDeviceDisplayService
    {
        public string Get()
        {
            DeviceDisplayInfoData infoData = new DeviceDisplayInfoData();
            infoData.Init();

            return JsonConvert.SerializeObject(infoData);
        }

        public bool GetKeepScreenOn()
        {
            return DeviceDisplay.KeepScreenOn;
        }

        public void SetKeepScreenOn(bool bKeepOn)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeviceDisplay.KeepScreenOn = bKeepOn;
            });
        }
    }
}