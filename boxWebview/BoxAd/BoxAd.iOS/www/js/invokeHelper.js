class GBMBridge
{
    GetDeviceInfo()
    {
        window.webkit.messageHandlers.GetDeviceInfo.postMessage({});
    }

    GetInternalStorageSizeInfo()
    {
        window.webkit.messageHandlers.GetInternalStorageSizeInfo.postMessage({});
    }

    GetScreenBrightness()
    {
        window.webkit.messageHandlers.GetScreenBrightness.postMessage({});
    }

    SetScreenBrightness(brightness)
    {
        window.webkit.messageHandlers.SetScreenBrightness.postMessage({ brightness });
    }

    GetSSID()
    {
        window.webkit.messageHandlers.GetSSID.postMessage({});
    }

    GetDeviceInfo()
    {
        window.webkit.messageHandlers.GetDeviceInfo.postMessage({});
    }

    GetBatteryInfo()
    {
        window.webkit.messageHandlers.GetBatteryInfo.postMessage({});
    }

    GetDeviceDisplayInfo()
    {
        window.webkit.messageHandlers.GetDeviceDisplayInfo.postMessage({});
    }

    GetDeviceDisplayKeepOn()
    {
        window.webkit.messageHandlers.GetDeviceDisplayKeepOn.postMessage({});
    }

    SetDeviceDisplayKeepOn(keepOn)
    {
        window.webkit.messageHandlers.SetDeviceDisplayKeepOn.postMessage({ keepOn });
    }

    ReadLocalStorage(key) {
        window.webkit.messageHandlers.ReadLocalStorage.postMessage({ key });
    }

    WriteLocalStorage(key, value) {
        window.webkit.messageHandlers.WriteLocalStorage.postMessage({ key, value });
    }

    GetNICInfo() {
        window.webkit.messageHandlers.GetNICInfo.postMessage({});
    }

    GetIMEIInfo() {
        window.webkit.messageHandlers.GetIMEIInfo.postMessage({});
    }

    GetPermissions() {
        window.webkit.messageHandlers.GetPermissions.postMessage({});
    }

    RequestPermissions() {
        window.webkit.messageHandlers.RequestPermissions.postMessage({});
    }

    RequestConnectWifi(ssid_name, ssid_pw) {
        window.webkit.messageHandlers.RequestConnectWifi.postMessage({ ssid_name, ssid_pw });
    }

    CheckAttributes(key) {
        window.webkit.messageHandlers.CheckAttributes.postMessage({key});
    }

    GetMDMID() {
        window.webkit.messageHandlers.GetMDMID.postMessage({});
    }
}

GBM = new GBMBridge();

// GBM.GetDeviceInfo : function(string data)
GBM.returnGetDeviceInfo = undefined;

// GBM.GetInternalStorageSizeInfo : function(string data)
GBM.returnGetInternalStorageSizeInfo = undefined;
