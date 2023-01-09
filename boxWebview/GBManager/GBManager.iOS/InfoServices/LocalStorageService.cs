using Foundation;
using GBManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using GBManager.iOS.InfoServices;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageService))]
namespace GBManager.iOS.InfoServices
{
    public class LocalStorageService : ILocalStorageService
    {
        const string Prefix = "GPM_";
        const string Suffix = ".PM";

        private string GetFileName(string key)
        {
            string fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), $"{Prefix}{key}{Suffix}");
            return fileName;
        }

        public string Read(string key)
        {
            string result = "";

            string strFileName = GetFileName(key);
            if(File.Exists(strFileName))
                result = File.ReadAllText(GetFileName(key));

            return result;
        }

        public bool Write(string key, string value)
        {
            File.WriteAllText(GetFileName(key), value);
            return true;
        }
    }
}