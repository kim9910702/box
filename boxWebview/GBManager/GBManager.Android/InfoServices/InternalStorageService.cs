using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GBManager.Interfaces;
using GBManager.Android.InfoServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(InternalStorageService))]
namespace GBManager.Android.InfoServices
{
    public class InternalStorageService : IInternalStorageService
    {
        public string Get()
        {
            InternalStorageSize storageSize = new InternalStorageSize(this);
            return JsonConvert.SerializeObject(storageSize);
        }

        public ulong GetFreeSpace()
        {
            return (ulong)(global::Android.OS.Environment.ExternalStorageDirectory.FreeSpace + global::Android.OS.Environment.RootDirectory.FreeSpace);
        }

        public ulong GetTotalSpace()
        {
            return (ulong)(global::Android.OS.Environment.ExternalStorageDirectory.TotalSpace + global::Android.OS.Environment.RootDirectory.TotalSpace);
        }
    }
}