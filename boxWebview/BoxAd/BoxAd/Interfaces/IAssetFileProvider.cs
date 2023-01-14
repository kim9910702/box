using EmbedIO.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxAd.Interfaces
{
    public interface IAssetFileProvider
    {
        IFileProvider Create(string strAssetPrefix);
    }
}
