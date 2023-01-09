using System;
using System.Collections.Generic;
using System.Text;

namespace GBManager.Interfaces
{
    public interface IPermissionService
    {
        void Request(Action<string> completeHandle);
        void Get(Action<string> completeHandler);
    }
}
