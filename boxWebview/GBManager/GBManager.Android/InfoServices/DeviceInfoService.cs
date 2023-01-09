using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GBManager.Android.InfoServices;
using GBManager.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceInfoService))]
namespace GBManager.Android.InfoServices
{
    class DeviceInfoService : IDeviceInfoService
    {
        public string Get()
        {
            DeviceInfoData infoData = new DeviceInfoData();
            infoData.Init();

            return JsonConvert.SerializeObject(infoData);
        }
    }
}