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
using GBManager.Android.InfoServices;
using GBManager.Interfaces;
using Java.IO;
using Plugin.CurrentActivity;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageService))]
namespace GBManager.Android.InfoServices
{
    public class LocalStorageService : ILocalStorageService
    {
        const string Prefix = "GPM_";
        const string Suffix = ".PM";

        private Java.IO.File GetFileName(string key)
        {
            string fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), $"{Prefix}{key}{Suffix}");
            return new Java.IO.File(fileName);
        }

        public string Read(string key)
        {
            string result="";

            Java.IO.File file = GetFileName(key);
            if (file.Exists())
            {
                FileReader fileReader = new FileReader(file);
                BufferedReader reader = new BufferedReader(fileReader);

                StringBuilder sb = new StringBuilder();
                string line;
                while((line = reader.ReadLine()) != null) 
                    sb.AppendLine(line);

                result = sb.ToString();

                reader.Close();
                fileReader.Close();
            }

            return result;
        }

        public bool Write(string key, string value)
        {
            bool bSucceeded = true;

            Java.IO.File file = GetFileName(key);
            FileWriter writer = new FileWriter(file);
            writer.Append(value);
            writer.Flush();
            writer.Close();

            return bSucceeded;
        }
    }
}