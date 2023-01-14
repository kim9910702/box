using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EmbedIO;
using EmbedIO.Files;
using BoxAd.Android.Controls;
using BoxAd.Interfaces;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(AssetFileProvider))]
namespace BoxAd.Android.Controls
{
    public class AssetFileProvider : IAssetFileProvider, IFileProvider
    {
        private readonly DateTime _fileTime = DateTime.UtcNow;
        public bool IsImmutable => true;

        public event Action<string> ResourceChanged
        {
            add { }
            remove { }
        }

        public string PathPrefix { get; }

        public AssetFileProvider()
        {
        }

        public AssetFileProvider(string pathPeFix)
        {
            PathPrefix = pathPeFix;
        }

        public IFileProvider Create(string strAssetPrefix)
        {
            return new AssetFileProvider(strAssetPrefix);
        }

        public IEnumerable<MappedResourceInfo> GetDirectoryEntries(string path, IMimeTypeProvider mimeTypeProvider)
        {
            return Enumerable.Empty<MappedResourceInfo>();
        }

        public MappedResourceInfo MapUrlPath(string urlPath, IMimeTypeProvider mimeTypeProvider)
        {
            string resourceName = PathPrefix + urlPath;
   
            System.Diagnostics.Debug.WriteLine($"ASSET REQUEST - {resourceName}");

            long size = 0;
            try
            {
                using var stream = CrossCurrentActivity.Current.Activity.Assets.Open(resourceName);
                if (stream == null || stream == Stream.Null)
                    return null;

/* 사이즈를 리턴하지 않아도 정상 동작함 mmouse77 - [2022/12/12]

                using (BinaryReader br = new BinaryReader(stream))
                {
                    const int maxReadSize = 1024 * 256;
                    byte[] bcontent;

                    for (; (bcontent = br.ReadBytes(maxReadSize)).Length > 0;)
                    {
                        size += bcontent.Length;
                    }
                }
*/

                System.Diagnostics.Debug.WriteLine($"ASSET FOUND - {resourceName}");

            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"!!!!!!!ASSET NOT FOUND - {resourceName}");
                return null;
            }

            var lastSlashPos = urlPath.LastIndexOf('/');
            var name = urlPath.Substring(lastSlashPos + 1);

            return MappedResourceInfo.ForFile(
                resourceName,
                name,
                _fileTime,
                size,
                mimeTypeProvider.GetMimeType(Path.GetExtension(name)));
        }

        public Stream OpenFile(string path)
        {
            return CrossCurrentActivity.Current.Activity.Assets.Open(path);
        }

        public void Start(CancellationToken cancellationToken)
        {
        }
    }
}