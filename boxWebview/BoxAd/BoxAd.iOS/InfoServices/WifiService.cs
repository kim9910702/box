using Foundation;
using BoxAd.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using BoxAd.iOS.InfoServices;
using NetworkExtension;

[assembly: Xamarin.Forms.Dependency(typeof(WifiService))]
namespace BoxAd.iOS.InfoServices
{
    public class WifiService : IWifiService
    {
        public void Connect(string ssid_name, string ssid_password, Action<bool> callBack)
        {
            SSIDService.GetSSID((ssid_current) =>
            {
                if (ssid_current == ssid_name)
                {
                    callBack(true);
                    return;
                }

                var wifiConfig = new NEHotspotConfiguration(ssid_name, ssid_password, false)
                {
                    JoinOnce = true
                };

                NEHotspotConfigurationManager.SharedManager.ApplyConfiguration(wifiConfig, (NSError error) =>
                {
                    if (error != null)
                    {
                        if (error?.LocalizedDescription == "already associated.")
                            callBack(true);
                        else
                            callBack(false);
                    }
                    else
                        callBack(true);
                });
            });
        }
    }
}