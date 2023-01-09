using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using GBManager.Android.InfoServices;
using GBManager.Controls;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xamarin.Forms;
using Newtonsoft.Json;
using GBManager.Android.Controls;
using Plugin.CurrentActivity;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using GBManager.Anroid.InfoServices;
using Java.Nio.Channels;
using GBManager.Interfaces;
using Java.Net;
using System.Net.NetworkInformation;
using Android.Telephony;
using Android;
using Android.Content.PM;

namespace GBManager.Android.Controls
{
    public class GBMBridge : Java.Lang.Object
    {
        GBMWebViewRenderer gbmWebViewRenderer;

        DeviceInfoService deviceInfoService;
        InternalStorageService internalStorageService;
        BrightnessService brightnessService;
        BatteryService batteryInfoService;
        DeviceDisplayService deviceDisplayService;
        SSIDService ssidService;
        PermissionService permissionService;
        IMEIService imeiService;
        WifiService wifiService;
        LocalStorageService localStorageService;
        NICInfoService nicService;

        public static GBMBridge Instance;

        public GBMBridge(GBMWebViewRenderer _renderer)
        {
            gbmWebViewRenderer = _renderer;

            deviceInfoService = new DeviceInfoService();
            internalStorageService = new InternalStorageService();
            brightnessService = new BrightnessService();
            batteryInfoService = new BatteryService();
            deviceDisplayService = new DeviceDisplayService();
            ssidService = new SSIDService();
            permissionService = new PermissionService();
            imeiService = new IMEIService();
            wifiService = new WifiService();
            localStorageService = new LocalStorageService();
            nicService = new NICInfoService();

            Instance = this;
        }

        private void EvalJS(string js)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                System.Diagnostics.Debug.WriteLine($"EvalJS : {js}");
                gbmWebViewRenderer.Control.EvaluateJavascript(js, null);
            });
        }

        [JavascriptInterface]
        [Export("GetDeviceInfo")]
        public void GetDeviceInfo()
        {
            System.Diagnostics.Debug.WriteLine("GetDeviceInfo : Javascript function calling c# function.");
            string result = deviceInfoService.Get();

            string js = $"GBM.returnGetDeviceInfo('{result}');";
            EvalJS(js);
        }

        [JavascriptInterface]
        [Export("GetInternalStorageSizeInfo")]
        public void GetInternalStorageSizeInfo()
        {
            System.Diagnostics.Debug.WriteLine("GetInternalStorageSize : Javascript function calling c# function.");
            string result = internalStorageService.Get();

            string js = $"GBM.returnGetInternalStorageSizeInfo('{result}');";
            EvalJS(js);
        }

        [JavascriptInterface]
        [Export("GetScreenBrightness")]
        public float GetScreenBrightness()
        {
            System.Diagnostics.Debug.WriteLine("GetScreenBrightness : Javascript function calling c# function.");
            var currentBright = brightnessService.Get();

            string js = $"GBM.returnGetScreenBrightness('{currentBright.ToString()}');";
            EvalJS(js);
            return currentBright;
        }

        [Export("SetScreenBrightness")]
        [JavascriptInterface]
        public void SetScreenBrightness(float fBrightness)
        {
            System.Diagnostics.Debug.WriteLine($"SetScreenBrightness {fBrightness} : Javascript function calling c# function.");
            brightnessService.Set(fBrightness);
        }

        [JavascriptInterface]
        [Export("GetBatteryInfo")]
        public void GetBatteryInfo()
        {
            System.Diagnostics.Debug.WriteLine("GetBatteryInfo : Javascript function calling c# function.");
            string result = batteryInfoService.Get();

            string js = $"GBM.returnGetBatteryInfo('{result}');";
            EvalJS(js);
        }

        [JavascriptInterface]
        [Export("GetDeviceDisplayInfo")]
        public void GetDeviceDisplayInfo()
        {
            System.Diagnostics.Debug.WriteLine("GetDeviceDisplayInfo : Javascript function calling c# function.");
            string result = deviceDisplayService.Get();

            string js = $"GBM.returnGetDeviceDisplayInfo('{result}');";
            EvalJS(js);
        }

        [JavascriptInterface]
        [Export("GetDeviceDisplayKeepOn")]
        public void GetDeviceDisplayKeepOn()
        {
            System.Diagnostics.Debug.WriteLine("GetDeviceDisplayKeepOn : Javascript function calling c# function.");
            bool bKeepOn = deviceDisplayService.GetKeepScreenOn();
            string result = bKeepOn ? "true" : "false";

            string js = $"GBM.returnGetDeviceDisplayKeepOn('{result}');";
            EvalJS(js);
        }

        [JavascriptInterface]
        [Export("SetDeviceDisplayKeepOn")]
        public void SetDeviceDisplayKeepOn(bool bKeepOn)
        {
            System.Diagnostics.Debug.WriteLine(@"SetDeviceDisplayKeepOn {bKeepOn}: Javascript function calling c# function.");
            deviceDisplayService.SetKeepScreenOn(bKeepOn);
        }

        [JavascriptInterface]
        [Export("GetSSID")]
        public void GetSSID()
        {
            System.Diagnostics.Debug.WriteLine("GetSSID : Javascript function calling c# function.");

            ssidService.GetWithCompletionAction((result) =>
            {
                string js = $"GBM.returnGetSSID('{result}');";
                EvalJS(js);
            });
        }

        [JavascriptInterface]
        [Export("GetPermissions")]
        public void GetPermissions()
        {
            System.Diagnostics.Debug.WriteLine("GetPermissions : Javascript function calling c# function.");

            permissionService.Get((result) =>
            {
                string js = $"GBM.returnGetPermissions('{result}');";
                EvalJS(js);
            });
        }

        [JavascriptInterface]
        [Export("RequestPermissions")]
        public void RequestPermissions()
        {
            System.Diagnostics.Debug.WriteLine("RequestPermissions : Javascript function calling c# function.");
            permissionService.Request((result) =>
            {
                string js = $"GBM.returnGetPermissions('{result}');";
                EvalJS(js);
            });
        }

        [JavascriptInterface]
        [Export("GetIMEIInfo")]
        public void GetIMEIInfo()
        {
            System.Diagnostics.Debug.WriteLine("GetIMEIInfo : Javascript function calling c# function.");

            var hasIMEI = IMEIService.HasIMEI();

            // Waiting for permissions
            if(hasIMEI == IMEIService.CELLINFO_RESULT.PERMISION)
                return;

            if(hasIMEI == IMEIService.CELLINFO_RESULT.NOCELLINFO)
            {
                string js = $"GBM.returnGetIMEIInfo('[]');";
                EvalJS(js);

                return;
            }

            // Check Actual IMEI Number
            if (Build.VERSION.SdkInt < BuildVersionCodes.Q)
            {
                string IMEI = imeiService.Get();

                string js = $"GBM.returnGetIMEIInfo('{IMEI}');";
                EvalJS(js);
            }
            else
            {
                imeiService.GetWithCompleteHandler((IMEI) =>
                {
                    string js = $"GBM.returnGetIMEIInfo('{IMEI}');";
                    EvalJS(js);
                });
            }
        }

        [JavascriptInterface]
        [Export("RequestConnectWifi")]
        public void RequestConnectWifi(string ssid_name, string ssid_pw)
        {
            System.Diagnostics.Debug.WriteLine($"RequestConnectWifi ({ssid_name}, {ssid_pw}: Javascript function calling c# function.");

            wifiService.Connect(ssid_name, ssid_pw, (bConnected) =>
            {
                string result = bConnected ? "true" : "false";

                string js = $"GBM.returnRequestConnectWifi('{result}');";
                EvalJS(js);
            });
        }

        [JavascriptInterface]
        [Export("ReadLocalStorage")]
        public void ReadLocalStorage(string key)
        {
            System.Diagnostics.Debug.WriteLine($"ReadLocalStorage ({key}): Javascript function calling c# function.");

            string readData = localStorageService.Read(key);
            readData = readData.Replace("\r", "\\r");
            readData = readData.Replace("\n", "\\n");
            string js = $"GBM.returnReadLocalStorage('{readData}');";
            EvalJS(js);
        }

        [JavascriptInterface]
        [Export("WriteLocalStorage")]
        public void WriteLocalStorage(string key, string data)
        {
            System.Diagnostics.Debug.WriteLine($"WriteLocalStorage ({key}, {data}): Javascript function calling c# function.");
            localStorageService.Write(key, data);
        }

        [JavascriptInterface]
        [Export("GetNICInfo")]
        public void GetNICInfo()
        {
            System.Diagnostics.Debug.WriteLine($"GetNICInfo : Javascript function calling c# function.");
            string strNICInfo = nicService.Get();
            string js = $"GBM.returnReadLocalStorage('{strNICInfo}');";
            EvalJS(js);
        }
    }
}