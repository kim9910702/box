using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using GBManager.Interfaces;
using GBManager.iOS.InfoServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

[assembly: Xamarin.Forms.Dependency(typeof(InternalStorageService))]
namespace GBManager.iOS.InfoServices
{
    public class InternalStorageService : IInternalStorageService
    {
        public string Get()
        {
            InternalStorageSize storageSize = new InternalStorageSize(this);
            return JsonConvert.SerializeObject(storageSize);
        }

        public ulong GetTotalSpace()
        {
            NSFileSystemAttributes applicationFolder = NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            return applicationFolder.Size;
        }

        public ulong GetFreeSpace()
        {
            NSFileSystemAttributes applicationFolder = NSFileManager.DefaultManager.GetFileSystemAttributes(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            return applicationFolder.FreeSize;
        }
    }
}