using Foundation;
using GBManager.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using GBManager.iOS.InfoServices;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryService))]
namespace GBManager.iOS.InfoServices
{
    class BatteryService : IBatteryService
    {
        public string Get()
        {
            BatteryInfoData infoData = new BatteryInfoData();
            infoData.Init();

            return JsonConvert.SerializeObject(infoData);
        }
    }
}