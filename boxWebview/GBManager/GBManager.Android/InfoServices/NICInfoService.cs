using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GBManager.Android.Controls;
using GBManager.Android.InfoServices;
using GBManager.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(NICInfoService))]
namespace GBManager.Android.InfoServices
{
    public class Nic
    {
        public string Name;
        public string Description;
        public OperationalStatus OperationStatus;
        public NetworkInterfaceType InterfaceType;
        public bool IsV4Support;
        public bool IsV6Support;
        public List<string> IPv4Addresses;
        public List<string> IPv6Addresses;

        public Nic()
        {
            IPv4Addresses = new List<string>();
            IPv6Addresses = new List<string>();
        }
    }

    public class NICInfoService : INICInfoService
    {
        public string Get()
        {
            Nic[] nics = LoadNics();
            return JsonConvert.SerializeObject(nics);
        }

        private Nic[] LoadNics()
        {
            var nics = new List<Nic>();
            var ifs = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            foreach (var i in ifs)
            {
                var n = new Nic
                {
                    Name = i.Name,
                    Description = i.Description,
                    OperationStatus = i.OperationalStatus,
                    InterfaceType = i.NetworkInterfaceType
                };

                if (i.Supports(NetworkInterfaceComponent.IPv4))
                    n.IsV4Support = true;
                if (i.Supports(NetworkInterfaceComponent.IPv6))
                    n.IsV6Support = true;

                foreach (var a in i.GetIPProperties().UnicastAddresses)
                {
                    if (a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                        n.IPv6Addresses.Add(a.Address.ToString());

                    if (a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        n.IPv4Addresses.Add(a.Address.ToString());
                }

                nics.Add(n);
            }

            return nics.ToArray();
        }
    }
}