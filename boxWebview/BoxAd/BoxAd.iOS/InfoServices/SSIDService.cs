using Foundation;
using BoxAd.iOS.InfoServices;
using BoxAd.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using UIKit;
using SystemConfiguration;
using CoreLocation;
using NetworkExtension;
using System.Threading.Tasks;
using System.Threading;
using ObjCRuntime;
using System.Diagnostics;
using CoreBluetooth;
using Newtonsoft.Json;
using System.Security.Policy;

[assembly: Xamarin.Forms.Dependency(typeof(SSIDService))]
namespace BoxAd.iOS.InfoServices
{
    class SSIDService : CLLocationManagerDelegate, ISSIDService
    {
        public static string INVALID_SSID = "<unknown ssid>";

        Action<string> completeHandler;

        public string Get()
        {
            return INVALID_SSID;
        }

        public void GetWithCompletionAction(Action<string> complete)
        {
            CLLocationManager locationManager = new CLLocationManager();
            locationManager.Delegate = this;

            completeHandler = complete;

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                locationManager.RequestWhenInUseAuthorization();
        }

        public static void GetSSID(Action<string> callBack)
        {
            if (callBack == null)
                return;

            string ssid = INVALID_SSID;

            CLLocationManager locationManager = new CLLocationManager();
            if (locationManager != null && locationManager.AuthorizationStatus >= CLAuthorizationStatus.Authorized)
            {
                NEHotspotNetwork.FetchCurrent((hotspot) =>
                {
                    ssid = hotspot?.Ssid?.ToString();
                    if (ssid == null)
                        ssid = INVALID_SSID;

                    callBack(ssid);
                });
            }
            else
                callBack(ssid);
        }

        private void InternalGetSSID(CLLocationManager locationManager)
        {
            if (locationManager == null || locationManager.AuthorizationStatus == CLAuthorizationStatus.NotDetermined)
                return;

            SSIDInfo ssid = new SSIDInfo
            {
                ssid = INVALID_SSID,
                result = 0,
            };

            if (locationManager.AuthorizationStatus >= CLAuthorizationStatus.Authorized)
            {
                NEHotspotNetwork.FetchCurrent((hotspot) =>
                {
                    ssid.ssid = hotspot?.Ssid?.ToString();
                    if (ssid.ssid == null)
                        ssid.ssid = INVALID_SSID;

                    if (ssid.ssid.Length > 0)
                        ssid.result = 1;

                    completeHandler(JsonConvert.SerializeObject(ssid));
                });
            }
            else if (completeHandler != null)
            {
                // Denied
                if (locationManager.AuthorizationStatus == CLAuthorizationStatus.Denied)
                {
                    ssid.result = -1;

                    // Open application setting
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                }

                completeHandler(JsonConvert.SerializeObject(ssid));
            }
        }

        public override void DidChangeAuthorization(CLLocationManager locationManager)
        {
            Debug.WriteLine($"DidChangeAuthorization - {locationManager.AuthorizationStatus.ToString()}");
            InternalGetSSID(locationManager);
        }
    }
}