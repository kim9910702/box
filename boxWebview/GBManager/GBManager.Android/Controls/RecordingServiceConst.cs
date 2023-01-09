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
using GBManager.Android.Controls;

namespace GBManager.Android.Controls
{
    public partial class RecordingService
    {
        public const string ACTION_START = "action_start";
        public const string ACTION_STOP = "action_stop";
        public const string EXTRA_RESULT_CODE = "extra_result_code";
        public const string EXTRA_DATA = "extra_data";
        public const string EXTRA_RESTORE = "extra_restore";
        public const string EXTRA_IMEI = "extra_imei";

        public const string CHANNEL_ID = "SCR_REC";
        public const string CHANNEL_NAME = "레코딩";
        public const int NOTIFICATION_ID = 1999;

        public const int REQUEST_CODE_MP = 507;
        public const int REQUEST_CODE_OVERLAY = 508;

        public const double RECORDING_TIMEOUT = 3000;
        public const string RECORDING_TIMEOUT_STRING = "TIMEOUT";
    }
}