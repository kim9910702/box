using System;
using System.Collections.Generic;
using System.Text;

namespace BoxAd.Interfaces
{
    public interface IPermissionService
    {
        void Request(Action<string> completeHandle);
        void Get(Action<string> completeHandler);
    }
}
