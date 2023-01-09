using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Provider;
using Android.Net;
using Java.Interop;

using GBManager.Android.InfoServices;
using GBManager.Interfaces;

using Plugin.CurrentActivity;

using Xamarin.Forms;
using AndroidX.Core.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Google.Android.Material.Snackbar;
using Uri = Android.Net.Uri;
using AndroidX.Core.App;
using static Android.Bluetooth.BluetoothClass;
using System.Runtime.Remoting.Contexts;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(BrightnessService))]
namespace GBManager.Android.InfoServices
{
    public class BrightnessService : IBrightnessService
    {
        public const int REQUEST_CODE = 901;

        public static float DesiredBrightness;

        public float Get()
        {
            return float.Parse(Settings.System.GetInt(CrossCurrentActivity.Current.AppContext.ContentResolver, Settings.System.ScreenBrightness).ToString()) / 255.0f;
        }

        public bool Set(float brightness)
        {
            bool bSucceeded;

            if (brightness < 0.0f)
                brightness = 0.0f;

            else if (brightness > 1.0f)
                brightness = 1.0f;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                bSucceeded = Settings.System.CanWrite(CrossCurrentActivity.Current.AppContext);
                if (bSucceeded)
                    Settings.System.PutInt(CrossCurrentActivity.Current.AppContext.ContentResolver, Settings.System.ScreenBrightness, (int)(brightness * 255));
                else
                {
                    DesiredBrightness = brightness;
                    Intent intent = new Intent(Settings.ActionManageWriteSettings, Uri.Parse("package:" + AppInfo.PackageName));
                    CrossCurrentActivity.Current.Activity.StartActivityForResult(intent, REQUEST_CODE);
                }
            }
            else
                bSucceeded = Settings.System.PutInt(CrossCurrentActivity.Current.AppContext.ContentResolver, Settings.System.ScreenBrightness, (int)(brightness * 255));

            return bSucceeded;
        }
    }
}