using System;
using System.Collections.Generic;
using System.Text;

namespace GBManager.Interfaces
{
    public interface IInternalStorageService
    {
        ulong GetTotalSpace();
        ulong GetFreeSpace();
        string Get();
    }

    public struct InternalStorageSize
    {
        public string totalsize;
        public string freesize;

        public InternalStorageSize(IInternalStorageService storageService)
        {
            totalsize = storageService.GetTotalSpace().ToString();
            freesize = storageService.GetFreeSpace().ToString();
        }
    };
}
