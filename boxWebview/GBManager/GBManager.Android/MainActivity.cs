using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using GBManager.Android.InfoServices;
using GBManager.Android.Controls;
using Android.Content;
using GBManager.Anroid.InfoServices;
using Xamarin.Essentials;

namespace GBManager.Android
{
    [Activity(Label = "굿바이", LaunchMode = LaunchMode.SingleInstance, Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static public MainActivity Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            LoadApplication(new App());
            
            // 기본 스크린 항상 켜두기
            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeviceDisplay.KeepScreenOn = true;
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            switch(requestCode)
            {
                case PermissionService.REQUEST_CODE:
                    PermissionService.Instance.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    break;

                case SSIDService.REQUEST_CODE:
                    SSIDService.Instance.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    break;

                case IMEIService.REQUEST_CODE:
                    IMEIService.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                    break;
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch(requestCode)
            {
                case RecordingService.REQUEST_CODE_MP:
                case RecordingService.REQUEST_CODE_OVERLAY:
                    RecordingService.OnActivityResultFromMain(requestCode, resultCode, data);
                    break;

                case BrightnessService.REQUEST_CODE:
                    if (global::Android.Provider.Settings.System.CanWrite(CrossCurrentActivity.Current.AppContext))
                        global::Android.Provider.Settings.System.PutInt(CrossCurrentActivity.Current.AppContext.ContentResolver, global::Android.Provider.Settings.System.ScreenBrightness, (int)(BrightnessService.DesiredBrightness * 255));
                    break;
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}