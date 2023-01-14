using Foundation;
using BoxAd.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using BoxAd.iOS.InfoServices;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryService))]
namespace BoxAd.iOS.InfoServices
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