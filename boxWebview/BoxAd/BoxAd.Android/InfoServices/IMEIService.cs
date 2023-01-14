using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Display;
using Android.Media;
using Android.Media.Projection;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using BoxAd.Android;
using BoxAd.Android.Controls;
using BoxAd.Android.InfoServices;
using BoxAd.Anroid.InfoServices;
using BoxAd.Interfaces;
using Java.Interop;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using Swan.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

[assembly: Xamarin.Forms.Dependency(typeof(IMEIService))]
namespace BoxAd.Anroid.InfoServices
{
    public class IMEIService : IIMEIService
    {
        public static Action<string> IMEICompleteHandler;
        public const int REQUEST_CODE = 105;

        public enum CELLINFO_RESULT
        {
            PERMISION=0,
            HASCELLINFO,
            NOCELLINFO,
        }

        public IMEIService()
        {
        }

        public string Get()
        {
            IMEICompleteHandler = null;
            List<string> imeiList = new List<string>();
            string IMEI = string.Empty;

            if (Build.VERSION.SdkInt < BuildVersionCodes.Q)
            {
                global::Android.Telephony.TelephonyManager mTelephonyMgr = (global::Android.Telephony.TelephonyManager)CrossCurrentActivity.Current.AppContext.GetSystemService(global::Android.Content.Context.TelephonyService);
                if (mTelephonyMgr != null)
                {
                    if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                    {
                        for (int i = 0; i < 2; i++)
                        {
#pragma warning disable CS0618
                            if (mTelephonyMgr.GetDeviceId(i) != null)
                                imeiList.Add(mTelephonyMgr.GetDeviceId(i));
                            else
                                break;
#pragma warning restore CS0618
                        }
                    }

                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            if (mTelephonyMgr.GetImei(i) != null)
                                imeiList.Add(mTelephonyMgr.GetImei(i));
                            else
                                break;
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(imeiList);
        }

        public void GetWithCompleteHandler(Action<string> completeHandler)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                IMEICompleteHandler = completeHandler;
                RecordingService.RequestScreenCapture();
            }
        }

        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            System.Console.WriteLine($"{requestCode} - {permissions} - {grantResults}");

            if (requestCode == REQUEST_CODE)
                GBMBridge.Instance.GetIMEIInfo();
        }

        public static CELLINFO_RESULT HasIMEI()
        {
            bool AccessFineLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessFineLocation) == Permission.Granted;
            bool AccessCoarseLocation = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.AccessCoarseLocation) == Permission.Granted;
            bool ReadPhoneState = ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.AppContext, Manifest.Permission.ReadPhoneState) == Permission.Granted;
            if (AccessFineLocation == false || AccessCoarseLocation == false || ReadPhoneState == false)
            {
                ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, new string[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.AccessCoarseLocation, Manifest.Permission.ReadPhoneState }, REQUEST_CODE);
                return CELLINFO_RESULT.PERMISION;
            }

            global::Android.Telephony.TelephonyManager mTelephonyMgr = (global::Android.Telephony.TelephonyManager)CrossCurrentActivity.Current.AppContext.GetSystemService(global::Android.Content.Context.TelephonyService);
            IList<CellInfo> cellList = mTelephonyMgr.AllCellInfo;
            if (cellList != null)
            {
                if (cellList.Count > 0)
                    return CELLINFO_RESULT.HASCELLINFO;
            }

            return CELLINFO_RESULT.NOCELLINFO;
        }

        public static void OnCancelMediaProjection()
        {
            if (IMEICompleteHandler != null)
                IMEICompleteHandler("[]");
        }

/*
        async Task CaptureScreenShot()
        {
            await Task.Delay(1000);
            var screenshot = await Screenshot.CaptureAsync();
            MemoryStream stream = await screenshot.OpenReadAsync() as MemoryStream;

            byte[] bitmapData = stream.ToArray();

            Java.IO.File picturesFolder = global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryDcim);
            string date = DateTime.Now.ToString().Replace("/", "-").Replace(":", "-");
            try
            {
                string filePath = System.IO.Path.Combine(picturesFolder.AbsolutePath + "/Camera", "Screnshot-" + date + ".png");
                using (System.IO.FileStream SourceStream = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate))
                {
                    SourceStream.Seek(0, System.IO.SeekOrigin.End);
                    await SourceStream.WriteAsync(bitmapData, 0, bitmapData.Length);
                }
                System.Console.WriteLine(filePath);

                global::Android.Content.Intent intent = new global::Android.Content.Intent(global::Android.Content.Intent.ActionQuickView);
                intent.AddFlags(ActivityFlags.NewTask);

                Java.IO.File fileScreenshot = new Java.IO.File(filePath);

                global::Android.Net.Uri uri = global::Android.Net.Uri.FromFile(fileScreenshot);
                intent.SetDataAndType(uri, "image/png");

                CrossCurrentActivity.Current.AppContext.StartActivity(intent);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
*/
    }
}