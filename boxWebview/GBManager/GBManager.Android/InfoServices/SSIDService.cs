using Android;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.Net.Wifi;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Text;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using GBManager.Android.InfoServices;
using GBManager.Interfaces;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using Swan;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(SSIDService))]
namespace GBManager.Android.InfoServices
{
    class SSIDService : ISSIDService
    {
        public const int REQUEST_CODE = 900;
        public static string INVALID_SSID = "<unknown ssid>";
        public static SSIDService Instance;

        Action<string> completeHandler;

        public SSIDService()
        {
            Instance = this;
        }

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            System.Console.WriteLine($"{requestCode} - {permissions} - {grantResults}");

            if (requestCode == REQUEST_CODE)
                InternalGetSSID();
        }

        public string Get()
        {
            string ssid = INVALID_SSID;
            return ssid;
        }

        private void InternalGetSSID()
        {
            if (completeHandler == null)
                return;

            SSIDInfo ssid = new SSIDInfo
            {
                ssid = INVALID_SSID,
                result = 0
            };

            Permission AccessWifiState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessWifiState);
            Permission AccessCoarseLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessCoarseLocation);
            Permission AccessFineLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessFineLocation);
            Permission ChangeWifiState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.ChangeWifiState);

            if (AccessWifiState == (int)Permission.Granted
                && AccessWifiState == (int)Permission.Granted
                && AccessFineLocation == (int)Permission.Granted
                && ChangeWifiState == (int)Permission.Granted)
            {
#pragma warning disable CS0618
                WifiManager wifiManager = (WifiManager)CrossCurrentActivity.Current.AppContext.GetSystemService(Context.WifiService);
                if (wifiManager != null && !string.IsNullOrEmpty(wifiManager.ConnectionInfo.SSID))
                {
                    ssid.ssid = wifiManager.ConnectionInfo.SSID.Replace("\"", string.Empty);
                    ssid.result = 1;
                };
#pragma warning restore CS0618
            }

            else
            {
                ssid.result = -1;
            }

            completeHandler(JsonConvert.SerializeObject(ssid));
        }

#pragma warning disable CS0612
        public void GetWithCompletionAction(Action<string> complete)
        {
            bool bLocationEnabled = PermissionService.isDeviceLocationEnabled(CrossCurrentActivity.Current.AppContext);
            if(bLocationEnabled == false)
            {
                PermissionService.changeLocationSettings();
                return;
            }

            string ssid = INVALID_SSID;

            completeHandler = complete;

            Permission AccessWifiState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessWifiState);
            Permission AccessCoarseLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessCoarseLocation);
            Permission AccessFineLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessFineLocation);

            if (AccessWifiState != (int)Permission.Granted
                || AccessCoarseLocation != (int)Permission.Granted
                || AccessFineLocation != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, new string[] { Manifest.Permission.AccessWifiState, Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation }, REQUEST_CODE);
            }
            else
                InternalGetSSID();
        }
#pragma warning restore CS0612
    }
}