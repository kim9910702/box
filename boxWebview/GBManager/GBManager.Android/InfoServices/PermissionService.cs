using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Activity.Result;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using GBManager.Android.InfoServices;
using GBManager.Interfaces;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PermissionService))]
namespace GBManager.Android.InfoServices
{
    class PermissionService : IPermissionService
    {
        public const int REQUEST_CODE = 101;

        public static PermissionService Instance;

        public Action<string> NotifyHandler;

        public struct Permissioninfo
        {
            public bool PermissionPhoneState;
            public bool PermissionLocation;
            public bool PermissionRecord;
            public bool PermissionCamera;
        };

        public PermissionService()
        {
            Instance = this;
        }

        private static string[] RequiredPermissions =
        {
            // PhoneState
            Manifest.Permission.ReadPhoneState,

            // Location
            Manifest.Permission.AccessWifiState,
            Manifest.Permission.ChangeWifiState,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.ChangeNetworkState,

            // Microphone
            Manifest.Permission.RecordAudio,

            // Camera
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.ReadExternalStorage,
        };

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            System.Console.WriteLine($"{requestCode} - {permissions} - {grantResults}");

            if (requestCode == REQUEST_CODE)
                InternalCheckPermission();
        }

        [Obsolete]
        public static bool isDeviceLocationEnabled(Context mContext)
        {
            Context context = CrossCurrentActivity.Current.AppContext;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat && Build.VERSION.SdkInt < BuildVersionCodes.P)
            {
                int locMode = 0;

                try
                {
                    locMode = Settings.Secure.GetInt(context.ContentResolver, Settings.Secure.LocationMode);
                }
                catch (Settings.SettingNotFoundException e)
                {
                    System.Console.WriteLine(e.Message);
                    return false;
                }

                return (SecurityLocationMode)locMode != Settings.Secure.LocationModeOff;
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                LocationManager lm = (LocationManager)context.GetSystemService(Context.LocationService);
                return lm.IsProviderEnabled(LocationManager.GpsProvider) == true;
            }

            return false;
        }

        [Obsolete]
        public static void changeLocationSettings()
        {
            Context context = CrossCurrentActivity.Current.AppContext;
            context.StartActivity(new Intent(global::Android.Provider.Settings.ActionLocationSourceSettings).AddFlags(ActivityFlags.NewTask));
        }

        private void InternalCheckPermission()
        {
            bool AccessWifiState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessWifiState) == Permission.Granted;
            bool ChangeWifiState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.ChangeWifiState) == Permission.Granted;
            bool AccessFineLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessFineLocation) == Permission.Granted;
            bool AccessCoarseLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessCoarseLocation) == Permission.Granted;
            bool ChangeNetworkState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.ChangeNetworkState) == Permission.Granted;

            bool ReadPhoneState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.ReadPhoneState) == Permission.Granted;

            bool RecordAudio = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.RecordAudio) == Permission.Granted;

            bool Camera = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.Camera) == Permission.Granted;
            bool WriteExternalStorage = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.WriteExternalStorage) == Permission.Granted;
            bool ReadExternalStorage = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.ReadExternalStorage) == Permission.Granted;

            PermissionService.Permissioninfo permInfo = new PermissionService.Permissioninfo
            {
                PermissionPhoneState = ReadPhoneState,
                PermissionLocation = AccessWifiState && AccessFineLocation && AccessCoarseLocation && ChangeWifiState && ChangeNetworkState,
                PermissionRecord = RecordAudio,
                PermissionCamera = Camera && WriteExternalStorage && ReadExternalStorage
            };

            // Q 이상부터 ExternalStorage 관련 무시됨
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                permInfo.PermissionCamera = Camera;

            if(NotifyHandler != null)
                NotifyHandler(JsonConvert.SerializeObject(permInfo));
        }

        public void Get(Action<string> notifyHandler)
        {
            NotifyHandler = notifyHandler;
            InternalCheckPermission();
        }

        public void Request(Action<string> notifyHandler)
        {
            NotifyHandler = notifyHandler;
            ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, RequiredPermissions, REQUEST_CODE);
        }
    }
}