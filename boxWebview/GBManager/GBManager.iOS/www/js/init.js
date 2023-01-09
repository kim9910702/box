function evalGetScreenBrightness() {
    GBM.GetScreenBrightness();
}

function evalSetScreenBrightness(brightness) {
    GBM.SetScreenBrightness(brightness);
}

function evalGetSSID() {
    GBM.GetSSID();
}

function evalGetDeviceInfo() {
    GBM.GetDeviceInfo();
}

function evalGetBatteryInfo() {
    GBM.GetBatteryInfo();
}

function evalGetDeviceDisplayInfo() {
    GBM.GetDeviceDisplayInfo();
}

function evalGetDeviceDisplayKeepOn() {
    GBM.GetDeviceDisplayKeepOn();
}

function evalSetDeviceDisplayKeepOn(keepon) {
    GBM.SetDeviceDisplayKeepOn(keepon);
}

function evalGetInternalStorageSizeInfo() {
    GBM.GetInternalStorageSizeInfo();
}

function evalGetIMEIInfo() {
    GBM.GetIMEIInfo();
}

function evalGetPermissions() {
    GBM.GetPermissions();
}

function evalRequestPermissions() {
    GBM.RequestPermissions();
}

function evalRequestConnectWifi(ssid_name, ssid_pw) {
    GBM.RequestConnectWifi(ssid_name, ssid_pw);
}
