﻿using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GBManager.Android.InfoServices;
using GBManager.Interfaces;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceDisplayService))]
namespace GBManager.Android.InfoServices
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