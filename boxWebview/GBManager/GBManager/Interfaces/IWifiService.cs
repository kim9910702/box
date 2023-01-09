using System;
using System.Collections.Generic;
using System.Text;

namespace GBManager.Interfaces
{
    public interface IWifiService
    {
        void Connect(string ssid_name, string ssid_password, Action<bool> callBack);
    }
}
