using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Provider.SyncStateContract;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;
using GBManager.Android.InfoServices;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using GBManager.Interfaces;
using Java.Lang;
using System.Runtime.Remoting.Contexts;
using Android.AccessibilityServices;
using static Android.Net.ConnectivityManager;
using Java.Util;
using Xamarin.Forms.PlatformConfiguration;
using Java.Interop;

#pragma warning disable CS0618
[assembly: Xamarin.Forms.Dependency(typeof(WifiService))]
namespace GBManager.Android.InfoServices
{
    public class WifiNetworkCallback : NetworkCallback
    {
        private WifiManager _wifiManager;
        Action<bool> _callBack;
        string _desireSSID;

        public WifiNetworkCallback(WifiManager wifiManager, Action<bool> callBack, string requestedSSID)
        {
            _wifiManager = wifiManager;
            _callBack = callBack;
            _desireSSID = requestedSSID;
        }

        public override void OnAvailable(Network network)
        {
            string ssid = _wifiManager.ConnectionInfo.SSID.Replace("\"", string.Empty);

            if (ssid == _desireSSID)
                _callBack(true);
            else
                _callBack(false);
        }
    }

    public class WifiService : IWifiService
    {
        private Action<bool> _completeCallback;
        private Version _version;
        private WifiConfiguration _config;
        private global::Android.Content.Context _context;
        private WifiManager _wifiManager = null;
        private WifiNetworkCallback _networkCallback;

        public WifiService()
        {
            _context = global::Android.App.Application.Context;
            _version = DeviceInfo.Version;
            _wifiManager = _context.GetSystemService(global::Android.Content.Context.WifiService) as WifiManager;
        }

        public void Connect(string ssid_name, string ssid_password, Action<bool> callBack)
        {
            _completeCallback = callBack;

            // Check Wifi Enabled
            if(_wifiManager.IsWifiEnabled == false)
            {
                if(_version.Major < 9)
                {
                    _wifiManager.SetWifiEnabled(true);
                }
                else
                {
                    Intent intent;
                    if (_version.Major == 9)
                        intent = new Intent(global::Android.Provider.Settings.ActionWifiSettings);
                    else
                        intent = new Intent(global::Android.Provider.Settings.Panel.ActionInternetConnectivity);

                    intent.AddFlags(ActivityFlags.NewTask);
                    global::Android.App.Application.Context.StartActivity(intent);

                    _completeCallback(false);
                    return;
                }
            }

            string ssid = _wifiManager.ConnectionInfo.SSID.Replace("\"", string.Empty);
            if (ssid == ssid_name)
            {
                _completeCallback(ssid == ssid_name);
                return;
            }

            // Trying to connect Wifi
            if (_version.Major > 9)
            {
                var specifier = new WifiNetworkSpecifier.Builder()
                               .SetSsid(ssid_name)
                               .SetWpa2Passphrase(ssid_password)
                               .Build();

                var request = new NetworkRequest.Builder()
                               .AddTransportType(TransportType.Wifi)
                               //.AddCapability(NetCapability.Internet)
                               .SetNetworkSpecifier(specifier)
                               .Build();

                ConnectivityManager _connectivityManager = _context.GetSystemService(global::Android.Content.Context.ConnectivityService) as ConnectivityManager;

                if (_networkCallback != null)
                    _connectivityManager.UnregisterNetworkCallback(_networkCallback);

                _networkCallback = new WifiNetworkCallback(_wifiManager, _completeCallback, ssid_name);
                _connectivityManager.RequestNetwork(request, _networkCallback);
            }
            else
            {
                _config = new WifiConfiguration
                {
                    Ssid = "\"" + ssid_name + "\"",
                    PreSharedKey = "\"" + ssid_password + "\""
                };

                try
                {
                    int wifiAdded = _wifiManager.AddNetwork(_config);

                    _wifiManager.Disconnect();

                    _wifiManager.EnableNetwork(wifiAdded, true);

                    if (_wifiManager.Reconnect())
                    {
                        ssid = _wifiManager.ConnectionInfo.SSID.Replace("\"", string.Empty);
                        _completeCallback(ssid == ssid_name);
                    }
                    else 
                    {
                        _completeCallback(false);
                    }
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    _completeCallback(false);
                }
            }
        }
    }
}