using Foundation;
using GBManager.Controls;
using GBManager.Interfaces;
using GBManager.iOS.InfoServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpriteKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;

namespace GBManager.iOS.Controls
{
    public class GBMBridge
    {
        GBMWebViewRenderer gbmWebViewRenderer;
        WKUserContentController userController;

        DeviceInfoService deviceInfoService;
        InternalStorageService internalStorageService;
        BrightnessService brightnessService;
        BatteryService batteryInfoService;
        DeviceDisplayService deviceDisplayService;
        SSIDService ssidService;
        LocalStorageService localStorage;
        NICInfoService nicSInfoervice;
        IMEIService imeiService;
        PermissionService permissionService;
        WifiService wifiService;

        public GBMBridge(GBMWebViewRenderer _renderer, WKUserContentController _userController)
        {
            gbmWebViewRenderer = _renderer;
            userController = _userController;

            deviceInfoService = new DeviceInfoService();
            internalStorageService = new InternalStorageService();
            brightnessService = new BrightnessService();
            batteryInfoService = new BatteryService();
            deviceDisplayService = new DeviceDisplayService();
            ssidService = new SSIDService();
            localStorage = new LocalStorageService();
            nicSInfoervice = new NICInfoService();
            imeiService = new IMEIService();
            permissionService = new PermissionService();
            wifiService = new WifiService();
        }

        public void Initialize()
        {
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetDeviceInfo");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetInternalStorageSizeInfo");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetScreenBrightness");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "SetScreenBrightness");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetSSID");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetBatteryInfo");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetDeviceDisplayInfo");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetDeviceDisplayKeepOn");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "SetDeviceDisplayKeepOn");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "ReadLocalStorage");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "WriteLocalStorage");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetNICInfo");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetIMEIInfo");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetPermissions");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "RequestPermissions");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "RequestConnectWifi");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "CheckAttributes");
            userController.AddScriptMessageHandler(gbmWebViewRenderer, "GetMDMID");
        }

        public void Clear()
        {
            userController.RemoveAllUserScripts();
            userController.RemoveScriptMessageHandler("GetDeviceInfo");
            userController.RemoveScriptMessageHandler("GetInternalStorageSizeInfo");
            userController.RemoveScriptMessageHandler("GetScreenBrightness");
            userController.RemoveScriptMessageHandler("SetScreenBrightness");
            userController.RemoveScriptMessageHandler("GetSSID");
            userController.RemoveScriptMessageHandler("GetBatteryInfo");
            userController.RemoveScriptMessageHandler("GetDeviceDisplayInfo");
            userController.RemoveScriptMessageHandler("GetDeviceDisplayKeepOn");
            userController.RemoveScriptMessageHandler("SetDeviceDisplayKeepOn");
            userController.RemoveScriptMessageHandler("ReadLocalStorage");
            userController.RemoveScriptMessageHandler("WriteLocalStorage");
            userController.RemoveScriptMessageHandler("GetNICInfo");
            userController.RemoveScriptMessageHandler("GetIMEIInfo");
            userController.RemoveScriptMessageHandler("GetPermissions");
            userController.RemoveScriptMessageHandler("RequestPermissions");
            userController.RemoveScriptMessageHandler("RequestConnectWifi");
            userController.RemoveScriptMessageHandler("CheckAttributes");
            userController.RemoveScriptMessageHandler("GetMDMID");
        }

        private void EvalJS(string js)
        {
            Debug.WriteLine($"EvalJS : {js}");
            gbmWebViewRenderer.EvaluateJavaScript((NSString)js, (result, error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"EvalJS Error - {js}");
                    Debug.WriteLine($"   error : {error.ToString()}");
                    if (result != null)
                        Debug.WriteLine($"   result : {result?.ToString()}");
                }
            });
        }

        public void OnReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            switch (message.Name)
            {
                case "GetDeviceInfo":
                    GetDeviceInfo();
                    break;

                case "GetInternalStorageSizeInfo":
                    GetInternalStorageSizeInfo();
                    break;

                case "GetScreenBrightness":
                    GetScreenBrightness();
                    break;

                case "SetScreenBrightness":
                    SetScreenBrightness(message.Body?.ValueForKey((NSString)"brightness")?.ToString());
                    break;

                case "GetSSID":
                    GetSSID();
                    break;

                case "GetBatteryInfo":
                    GetBatteryInfo();
                    break;

                case "GetDeviceDisplayInfo":
                    GetDeviceDisplayInfo();
                    break;

                case "GetDeviceDisplayKeepOn":
                    GetDeviceDisplayKeepOn();
                    break;

                case "SetDeviceDisplayKeepOn":
                    SetDeviceDisplayKeepOn(message.Body?.ValueForKey((NSString)"keepOn")?.ToString());
                    break;

                case "ReadLocalStorage":
                    ReadLocalStorage(message.Body?.ValueForKey((NSString)"key")?.ToString());
                    break;

                case "WriteLocalStorage":
                    WriteLocalStorage(message.Body?.ValueForKey((NSString)"key")?.ToString(), message.Body?.ValueForKey((NSString)"value")?.ToString());
                    break;

                case "GetNICInfo":
                    GetNICInfo();
                    break;

                case "GetIMEIInfo":
                    GetIMEIInfo();
                    break;

                case "GetPermissions":
                    GetPermissions();
                    break;

                case "RequestPermissions":
                    RequestPermissions();
                    break;

                case "RequestConnectWifi":
                    RequestConnectWifi(message.Body?.ValueForKey((NSString)"ssid_name")?.ToString(), message.Body?.ValueForKey((NSString)"ssid_pw")?.ToString());
                    break;

                case "CheckAttributes":
                    CheckAttributes(message.Body?.ValueForKey((NSString)"key")?.ToString());
                    break;

                case "GetMDMID":
                    GetMDMID();
                    break;


                default:
                    Debug.WriteLine($"Unhandled Javascritp Function : {message.Name}");
                    break;
            }
        }

        public void OnDispose(bool disposing)
        {
            if (disposing)
                Clear();
        }

        // Javascript Export Functions
        public void GetDeviceInfo()
        {
            Debug.WriteLine("GetDeviceInfo : Javascript function calling c# function.");
            string result = deviceInfoService.Get();

            string js = $"GBM.returnGetDeviceInfo('{result}');";
            EvalJS(js);
        }

        public void GetInternalStorageSizeInfo()
        {
            Debug.WriteLine("GetInternalStorageSize : Javascript function calling c# function.");
            string result = internalStorageService.Get();

            string js = $"GBM.returnGetInternalStorageSizeInfo('{result}');";
            EvalJS(js);
        }

        public void GetScreenBrightness()
        {
            Debug.WriteLine("GetScreenBrightness : Javascript function calling c# function.");
            string result = brightnessService.Get().ToString();

            string js = $"GBM.returnGetScreenBrightness('{result}');";
            EvalJS(js);
        }

        public void SetScreenBrightness(string strBrightness)
        {
            Console.WriteLine($"SetScreenBrightness {strBrightness} : Javascript function calling c# function.");

            float fBrightness;
            if (float.TryParse(strBrightness, out fBrightness))
                brightnessService.Set(fBrightness);
        }

        public void GetSSID()
        {
            Console.WriteLine("GetSSID : Javascript function calling c# function.");
            //string result = ssidService.Get();

            //string js = $"GBM.returnGetSSID('{result}');";
            //EvalJS(js);

            ssidService.GetWithCompletionAction((result) =>
            {
                string js = $"GBM.returnGetSSID('{result}');";
                EvalJS(js);
            });
        }

        public void GetBatteryInfo()
        {
            Console.WriteLine("GetBatteryInfo : Javascript function calling c# function.");
            string result = batteryInfoService.Get();

            string js = $"GBM.returnGetBatteryInfo('{result}');";
            EvalJS(js);
        }

        public void GetDeviceDisplayInfo()
        {
            Console.WriteLine("GetDeviceDisplayInfo : Javascript function calling c# function.");
            string result = deviceDisplayService.Get();

            string js = $"GBM.returnGetDeviceDisplayInfo('{result}');";
            EvalJS(js);
        }

        public void GetDeviceDisplayKeepOn()
        {
            Console.WriteLine("GetDeviceDisplayKeepOn : Javascript function calling c# function.");

            bool bKeepOn = deviceDisplayService.GetKeepScreenOn();
            string result = bKeepOn ? "true" : "false";

            string js = $"GBM.returnGetDeviceDisplayKeepOn('{result}');";
            EvalJS(js);
        }

        public void SetDeviceDisplayKeepOn(string strKeepOn)
        {
            Console.WriteLine($"SetDeviceDisplayKeepOn keepOn[{strKeepOn}]: Javascript function calling c# function.");

            int nKeepOn;
            if (int.TryParse(strKeepOn, out nKeepOn))
                deviceDisplayService.SetKeepScreenOn(nKeepOn != 0);
        }

        public void ReadLocalStorage(string key)
        {
            Console.WriteLine($"ReadLocalStorage ({key}) : Javascript function calling c# function.");

            string readData = localStorage.Read(key);
            readData = readData.Replace("\r", "\\r");
            readData = readData.Replace("\n", "\\n");

            string js = $"GBM.returnReadLocalStorage('{readData}');";
            EvalJS(js);
        }

        public void WriteLocalStorage(string key, string value)
        {
            Console.WriteLine($"WriteLocalStorage ({key}, {value}) : Javascript function calling c# function.");

            localStorage.Write(key, value);
        }

        public void GetNICInfo()
        {
            Console.WriteLine($"GetNICInfo : Javascript function calling c# function.");

            string result = nicSInfoervice.Get();

            string js = $"GBM.returnGetNICInfo('{result}');";
            EvalJS(js);
        }

        public void GetIMEIInfo()
        {
            Console.WriteLine("GetIMEIInfo : Javascript function calling c# function.");

            imeiService.GetWithCompletionHandler((imei) =>
            {
                string js = $"GBM.returnGetIMEIInfo('{imei}');";
                EvalJS(js);
            });
        }

        public void GetPermissions()
        {
            Console.WriteLine("GetPermissions : Javascript function calling c# function.");

            permissionService.Get((result) =>
            {
                string js = $"GBM.returnGetPermissions('{result}');";
                EvalJS(js);
            });
        }

        public void RequestPermissions()
        {
            Console.WriteLine("RequestPermissions : Javascript function calling c# function.");
            permissionService.Request((result) =>
            {
                string js = $"GBM.returnGetPermissions('{result}');";
                EvalJS(js);
            });
        }

        public void RequestConnectWifi(string ssid_name, string ssid_pw)
        {
            Console.WriteLine($"RequestConnectWifi ({ssid_name}, {ssid_pw}: Javascript function calling c# function.");

            wifiService.Connect(ssid_name, ssid_pw, (bConnected) =>
            {
                string result = bConnected ? "true" : "false";

                string js = $"GBM.returnRequestConnectWifi('{result}');";
                EvalJS(js);
            });
        }

        public void CheckAttributes(string key)
        {
            Console.WriteLine($"CheckAttributes ({key}) : Javascript function calling c# function.");

            NSUserDefaults userData = NSUserDefaults.StandardUserDefaults;
            NSDictionary dict = userData.DictionaryForKey("com.apple.configuration.managed");
            if (dict == null)
            {
                string noManaged = $"GBM.returnCheckAttributes('No Managed Dictionary Found');";
                EvalJS(noManaged);

                return;
            }

            string data = "NODATA";

            if (key == null || key.Length == 0)
            {
                data = "";
                IEnumerator<KeyValuePair<NSObject, NSObject>> dictEnum = dict.GetEnumerator();
                while (dictEnum.MoveNext())
                {
                    var cv = dictEnum.Current;
                    string strKey = cv.Key as NSString;
                    string strValue = cv.Value as NSString;
                    data += $"KEY[{strKey}] - VALUE[{strValue}]\\n";
                }
            }
            else
            {
                var nsData = dict[new NSString(key)];
                if (nsData != null)
                    data = nsData.ToString();
            }

            string js = $"GBM.returnCheckAttributes('{data}');";
            EvalJS(js);
        }

        public void GetMDMID()
        {
            Console.WriteLine($"GetMDMID : Javascript function calling c# function.");
            string mdmID = imeiService.GetMDMID();

            string js = $"GBM.returnGetMDMID('{mdmID}');";
            EvalJS(js);
        }
    }
}