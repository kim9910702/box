using System;
using System.Collections.Generic;
using System.Text;

namespace GBManager.Interfaces
{
    public interface ILocalStorageService
    {
        bool Write(string key, string value);
        string Read(string key);
    }
}
