using System;
using System.Collections.Generic;
using System.Text;

namespace BoxAd.Interfaces
{
    public interface ISSIDService
    {
        string Get();
    }

    public struct SSIDInfo
    {
        public int result;
        public string ssid;
    }
}
