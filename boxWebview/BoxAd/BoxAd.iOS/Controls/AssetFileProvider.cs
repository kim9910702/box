using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using BoxAd.iOS.Controls;
using BoxAd.Interfaces;
using EmbedIO.Files;
using EmbedIO;
using System.IO;
using System.Threading;
using BoxAd.Controls;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AssetFileProvider))]
namespace BoxAd.iOS.Controls
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
            string fileName = Path.Combine(NSBundle.MainBundle.BundlePath, $"{resourceName}");

            long size = 0;
            try
            {
                System.Diagnostics.Debug.WriteLine($"ASSET REQUEST - {resourceName} / {fileName}");

                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    if(fs != null)
                        size = fs.Length;
                }

                System.Diagnostics.Debug.WriteLine($"ASSET FOUND - {resourceName} / {fileName}");

            }
            catch (FileNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine($"!!!!!!!ASSET NOT FOUND - {resourceName}");
                return null;
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e);
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
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public void Start(CancellationToken cancellationToken)
        {
        }
    }
}