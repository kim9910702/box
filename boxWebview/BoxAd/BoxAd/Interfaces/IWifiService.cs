using System;
using System.Collections.Generic;
using System.Text;

namespace BoxAd.Interfaces
{
    public interface IWifiService
    {
        void Connect(string ssid_name, string ssid_password, Action<bool> callBack);
    }
}
