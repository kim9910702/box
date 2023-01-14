using Foundation;
using BoxAd.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using BoxAd.iOS.InfoServices;
using static Xamarin.Essentials.Permissions;
using CoreLocation;
using System.Diagnostics;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(PermissionService))]
namespace BoxAd.iOS.InfoServices
{
    public class PermissionService : IPermissionService
    {
        private Action<string> NotifyHandler;

        public struct Permissioninfo
        {
            public bool PermissionPhoneState;
            public bool PermissionLocation;
            public bool PermissionRecord;
            public bool PermissionCamera;
        };

        public struct PermissionStatusInfo
        {
            public PermissionStatus PermissionPhoneState;
            public PermissionStatus PermissionLocation;
            public PermissionStatus PermissionRecord;
            public PermissionStatus PermissionCamera;
        };

        public void Get(Action<string> completeHandler)
        {
            NotifyHandler = completeHandler;

            CheckPermissions((permInfo, permStatusInfo) =>
            {
                InternalCheckPermission(permInfo);
            });
        }

        public void Request(Action<string> completeHandler)
        {
            NotifyHandler = completeHandler;

            RequestPermissions((permInfo) =>
            {
                InternalCheckPermission(permInfo);
            });
        }

        private void InternalCheckPermission(Permissioninfo permInfo)
        {
            NotifyHandler?.Invoke(JsonConvert.SerializeObject(permInfo));
        }

#pragma warning disable 1998
        private async void RequestPermissions(Action<Permissioninfo> callBack)
        {
            CheckPermissions(async (permInfo, permStatusInfo) =>
            {
                // goto setting
                if(permStatusInfo.PermissionCamera == PermissionStatus.Denied || permStatusInfo.PermissionLocation == PermissionStatus.Denied || permStatusInfo.PermissionRecord == PermissionStatus.Denied)
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));

                if(permInfo.PermissionCamera == false)
                    permInfo.PermissionCamera = await Permissions.RequestAsync<Permissions.Camera>() == PermissionStatus.Granted;

                if(permInfo.PermissionLocation == false)
                    permInfo.PermissionLocation = await Permissions.RequestAsync<Permissions.LocationWhenInUse>() == PermissionStatus.Granted;

                if(permInfo.PermissionRecord == false)
                    permInfo.PermissionRecord = await Permissions.RequestAsync<Permissions.Microphone>() == PermissionStatus.Granted;

                callBack?.Invoke(permInfo);
            });
        }
#pragma warning restore 1998

        private async void CheckPermissions(Action<Permissioninfo, PermissionStatusInfo> callBack)
        {
            Permissioninfo permInfo = new Permissioninfo
            {
                PermissionPhoneState = true,
                PermissionLocation = false,
                PermissionRecord = false,
                PermissionCamera = false
            };

            PermissionStatusInfo permStatusInfo = new PermissionStatusInfo
            {
                PermissionPhoneState = PermissionStatus.Unknown,
                PermissionCamera = PermissionStatus.Unknown,
                PermissionLocation = PermissionStatus.Unknown,
                PermissionRecord = PermissionStatus.Unknown,
            };

            permStatusInfo.PermissionCamera = await Permissions.CheckStatusAsync<Permissions.Camera>();
            permInfo.PermissionCamera = permStatusInfo.PermissionCamera == PermissionStatus.Granted;

            permStatusInfo.PermissionLocation = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            permInfo.PermissionLocation = permStatusInfo.PermissionLocation == PermissionStatus.Granted;

            permStatusInfo.PermissionRecord = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            permInfo.PermissionRecord = permStatusInfo.PermissionRecord == PermissionStatus.Granted;

            callBack?.Invoke(permInfo, permStatusInfo);
        }
    }
}