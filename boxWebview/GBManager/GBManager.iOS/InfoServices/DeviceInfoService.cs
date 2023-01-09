﻿using GBManager.iOS.InfoServices;
using GBManager.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceInfoService))]
namespace GBManager.iOS.InfoServices
{
    public class DeviceInfoService : IDeviceInfoService
    {
        public string Get()
        {
            DeviceInfoData infoData = new DeviceInfoData();
            infoData.Init();

            return JsonConvert.SerializeObject(infoData);
        }
    }
}