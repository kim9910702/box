using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GBManager.Interfaces
{
    public interface IBatteryService
    {
        string Get();
    }
    public struct BatteryInfoData
    {
        public string chargelevel;
        public string state;
        public string powersource;
        public string energysaverstatus;

        public void Init()
        {
            chargelevel = Battery.ChargeLevel.ToString();
            state = Battery.State.ToString();
            powersource = Battery.PowerSource.ToString();
            energysaverstatus = Battery.EnergySaverStatus.ToString();
        }
    };
}
