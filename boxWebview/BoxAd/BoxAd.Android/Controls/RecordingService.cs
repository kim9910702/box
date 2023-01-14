using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BoxAd.Android.Controls;
using Android.Content.PM;
using System.Runtime.Remoting.Messaging;
using Android.SE.Omapi;
using Android.Graphics.Drawables;
using BoxAd.Droid;
using Plugin.CurrentActivity;
using Android.Media.Projection;
using Android.Media;
using Xamarin.Essentials;
using Android.Graphics;
using Android.Hardware.Display;
using static Android.Media.ImageReader;
using Java.Nio;
using Android.Service.Autofill;
using Android.Gms.Vision.Texts;
using Android.Gms.Vision;
using System.Threading.Tasks;
using System.Threading;
using BoxAd.Anroid.InfoServices;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms;
using Android.Provider;
using Newtonsoft.Json;

#pragma warning disable CS0618
namespace BoxAd.Android.Controls
{
    [Service(Exported = false, ForegroundServiceType = ForegroundService.TypeMediaProjection, Enabled = true)]
    public partial class RecordingService : Service, IOnImageAvailableListener
    {
        private MediaProjection mediaProjection;
        private ImageReader imageReader;
        private VirtualDisplay virtualDisplay;
        private bool IMEIFound = false;
        public static bool ForegroundServiceStarted;

        private System.Timers.Timer recordingTimer;
        private bool recordingTimeout;

        public static string ACTION_RESTORE_MAIN = "action_restore_main";

        public override void OnCreate()
        {
            recordingTimeout = false;
            recordingTimer = new System.Timers.Timer(RECORDING_TIMEOUT);
            recordingTimer.Elapsed += OnTimeOut;
            recordingTimer.Stop();

            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw null;
        }

        public static void OnActivityResultFromMain(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == REQUEST_CODE_MP)
            {
                if (resultCode == Result.Ok)
                {
                    if (ForegroundServiceStarted == false)
                    {
                        Activity activity = CrossCurrentActivity.Current.Activity;
                        Context context = CrossCurrentActivity.Current.AppContext;

                        Intent intent = GetStartIntent(context, (int)resultCode, data);
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                            context.StartForegroundService(intent);
                        else
                            context.StartService(intent);
                    }
                }
                else
                    IMEIService.OnCancelMediaProjection();
            }

            else if(requestCode == REQUEST_CODE_OVERLAY)
            {
                if (Settings.CanDrawOverlays(CrossCurrentActivity.Current.AppContext))
                    RequestScreenCapture();
                else
                    IMEIService.OnCancelMediaProjection();
            }
        }

        public Notification CreateRecordNotification()
        {
            if(Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notiChannel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.Default);
                NotificationManager notiMgr = (NotificationManager)GetSystemService(NotificationService);
                notiMgr.CreateNotificationChannel(notiChannel);
            }

            Icon notiIcon = Icon.CreateWithResource(this, Droid.Resource.Drawable.ic_arrow_down_24dp);
            PendingIntent pendingIntent = PendingIntent.GetService(this, REQUEST_CODE_MP, GetStopIntent(this), PendingIntentFlags.Immutable);

            var stopAction = new Notification.Action.Builder(notiIcon, "스톱", pendingIntent).Build();

            return new Notification.Builder(this, CHANNEL_ID)
                .SetChannelId(CHANNEL_ID)
                .SetSmallIcon(Droid.Resource.Drawable.ic_arrow_down_24dp)
                .SetContentTitle("스톱")
                .SetUsesChronometer(true)
                .SetOngoing(true)
                .AddAction(stopAction)
                .Build();
        }

        public static Intent GetStartIntent(Context context, int resultCode, Intent data)
        {
            Intent intent = new Intent(context, typeof(RecordingService))
                .SetAction(ACTION_START)
                .PutExtra(EXTRA_RESULT_CODE, resultCode)
                .PutExtra(EXTRA_DATA, data);

            return intent;
        }

        public static Intent GetStopIntent(Context context)
        {
            return new Intent(context, typeof(RecordingService))
                .SetAction(ACTION_STOP);
        }

        public static void RequestScreenCapture()
        {
            Context context = CrossCurrentActivity.Current.AppContext;
            Activity activity = CrossCurrentActivity.Current.Activity;

            if (Build.VERSION.SdkInt == BuildVersionCodes.Q && global::Android.Provider.Settings.CanDrawOverlays(context) == false)
            {
                Intent intent = new Intent(global::Android.Provider.Settings.ActionManageOverlayPermission, global::Android.Net.Uri.Parse("package:" + AppInfo.PackageName));
                activity.StartActivityForResult(intent, REQUEST_CODE_OVERLAY);
                return;
            }

            MediaProjectionManager mr = (MediaProjectionManager)context.GetSystemService(Context.MediaProjectionService);
            activity.StartActivityForResult(mr.CreateScreenCaptureIntent(), REQUEST_CODE_MP);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch(intent.Action)
            {
                case ACTION_START:
                    OnStartService(intent, flags, startId);
                    break;

                case ACTION_STOP:
                    int nRestore = intent.GetIntExtra(EXTRA_RESTORE, 0);
                    string IMEI = intent.GetStringExtra(EXTRA_IMEI);
                    if(IMEI.Length > 0)
                    {
                        if (IMEIService.IMEICompleteHandler != null)
                            IMEIService.IMEICompleteHandler(IMEI);
                    }
                    if (nRestore != 0)
                    {
                        Context context = CrossCurrentActivity.Current.AppContext;

                        Intent mainIntent = new Intent(this, typeof(MainActivity)).AddFlags(ActivityFlags.NewTask).SetAction(ACTION_RESTORE_MAIN);
                        StartActivity(mainIntent);
                    }

                    Task.Run(() =>
                    {
                        Thread.Sleep(500);
                        OnStopService();
                    });
                    break;

                default:
                    break;
            }

            return base.OnStartCommand(intent, flags, startId);
        }

        private bool OnStartService(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            StartForeground(NOTIFICATION_ID, CreateRecordNotification());
            ForegroundServiceStarted = true;

            int resultCode = intent.GetIntExtra(EXTRA_RESULT_CODE, 0);
            Intent intentData = (Intent)intent.GetParcelableExtra(EXTRA_DATA);

            MediaProjectionManager mpMgr = (MediaProjectionManager)GetSystemService(MediaProjectionService);
            mediaProjection = mpMgr.GetMediaProjection(resultCode, intentData);
            if (mediaProjection != null && StartProjection())
            {
                Task.Run(() =>
                {
                    Thread.Sleep(500);
                    Intent deviceInfoIntent = new Intent(global::Android.Provider.Settings.ActionDeviceInfoSettings);
                    deviceInfoIntent.AddFlags(ActivityFlags.NewTask);
                    CrossCurrentActivity.Current.AppContext.StartActivity(deviceInfoIntent);

                    recordingTimeout = false;
                    recordingTimer.Start();
                });
                
                return true;
            }
            else
                System.Console.WriteLine("GetMediaProjection Failed!");

            return false;
        }

        private void OnTimeOut(object sender, EventArgs e) 
        {
            recordingTimeout = true;
            recordingTimer.Stop();
        }

        private void OnStopService()
        {
            ForegroundServiceStarted = false;

            StopProjection();
            StopSelf();
        }

        public bool StartProjection()
        {
            if (mediaProjection == null)
                return false;

            IMEIFound = false;

            Context context = CrossCurrentActivity.Current.AppContext;

            int nWidth = (int)DeviceDisplay.MainDisplayInfo.Width;
            int nHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            int nDensity = (int)DeviceDisplay.MainDisplayInfo.Density;

            imageReader = ImageReader.NewInstance(nWidth, nHeight, (ImageFormatType)Format.Rgba8888, 2);
            if(imageReader != null)
            {
                virtualDisplay = mediaProjection.CreateVirtualDisplay("IMEIVP", nWidth, nHeight, nDensity, (DisplayFlags)VirtualDisplayFlags.AutoMirror, imageReader.Surface, null, null);
                imageReader.SetOnImageAvailableListener(this, null);
            }

            return virtualDisplay != null && imageReader != null;
        }

        public void StopProjection()
        {
            if (virtualDisplay != null)
            {
                virtualDisplay.Release();
                virtualDisplay.Dispose();
                virtualDisplay = null;
            }

            if (imageReader != null)
            {
                imageReader.Dispose();
                imageReader = null;
            }

            if (mediaProjection != null)
            {
                mediaProjection.Stop();
                mediaProjection = null;
            }
        }

        public void OnImageAvailable(ImageReader reader)
        {
            if (IMEIFound)
                return;

            if(recordingTimeout)
            {
                IMEIFound = true;

                Context context = CrossCurrentActivity.Current.AppContext;
                Intent sintent = GetStopIntent(context).PutExtra(EXTRA_RESTORE, 1).PutExtra(EXTRA_IMEI, "[]");
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    context.StartForegroundService(sintent);
                else
                    context.StartService(sintent);

                return;
            }

            global::Android.Media.Image image = reader.AcquireLatestImage();
            if (image == null)
                return;

            global::Android.Media.Image.Plane[] planes = image.GetPlanes();
            if (planes == null || planes.Length == 0)
            {
                if (image != null)
                    image.Close();

                return;
            }

            ByteBuffer buffer = planes[0].Buffer;
            int pixelStride = planes[0].PixelStride;
            int rowStride = planes[0].RowStride;
            int rowPadding = rowStride - pixelStride * image.Width;

            Bitmap bitmap = Bitmap.CreateBitmap(reader.Width + rowPadding / pixelStride, reader.Height, Bitmap.Config.Argb8888);
            bitmap.CopyPixelsFromBuffer(buffer);

            if (image != null)
            {
                image.Close();
                image = null;
            }

            TextRecognizer textOCR = new TextRecognizer.Builder(CrossCurrentActivity.Current.AppContext).Build();
            if(textOCR == null || textOCR.IsOperational == false)
            {
                bitmap.Dispose();

                System.Console.WriteLine("Google Play Service is not installed!");
                return;
            }

            global::Android.Gms.Vision.Frame frame = new global::Android.Gms.Vision.Frame.Builder().SetBitmap(bitmap).Build();
            var detectedItems = textOCR.Detect(frame);

            List<string> imeiList = new List<string>();

            for(int i = 0; i < detectedItems.Size(); i++)
            {
                TextBlock tb = (TextBlock)detectedItems.ValueAt(i);

                //System.Console.WriteLine($"{i} - {tb.Value.Trim()}");

                string _temp = tb.Value.Trim();
                if (_temp.Length != 15)
                    continue;

                bool bIMEFormatValid = true;
                for (int j = 0; j < _temp.Length; j++)
                {
                    if(_temp[j] < '0' || _temp[j] > '9')
                    {
                        bIMEFormatValid = false;
                        break;
                    }
                }

                if (bIMEFormatValid)
                {
                    IMEIFound = true;
                    imeiList.Add(_temp);
                }
            }

            if(imeiList.Count > 0)
            {
                recordingTimer.Stop();

                Context context = CrossCurrentActivity.Current.AppContext;
                Intent sintent = GetStopIntent(context).PutExtra(EXTRA_RESTORE, 1).PutExtra(EXTRA_IMEI, JsonConvert.SerializeObject(imeiList));
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    context.StartForegroundService(sintent);
                else
                    context.StartService(sintent);
            }
        }
    }
}
#pragma warning restore CS0618