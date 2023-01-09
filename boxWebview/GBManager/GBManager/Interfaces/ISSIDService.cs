using System;
using System.Collections.Generic;
using System.Text;

namespace GBManager.Interfaces
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
