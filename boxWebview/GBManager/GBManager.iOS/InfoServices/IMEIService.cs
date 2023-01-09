using Foundation;
using GBManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using GBManager.iOS.InfoServices;
using static CoreFoundation.DispatchSource;
using System.Threading.Tasks;
using System.Net.Http;
using AuthenticationServices;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Diagnostics;

[assembly: Xamarin.Forms.Dependency(typeof(IMEIService))]
namespace GBManager.iOS.InfoServices
{
    public class service_subscription
    {
        public string slot;
        public string imei;
    }

    public class device_attributes
    {
        public string name;
        public string model;
        public string serial_number;
        public List<service_subscription> service_subscriptions;
    }

    public class device_data
    {
        public long id;
        public string type;
        public device_attributes attributes;
    }

    public class device_info_holder
    {
        public device_data data;
    }

    public class IMEIService : IIMEIService
    {
        public string MDM_ID = string.Empty;

        // devmaru API Key
        // public const string SIMPLE_MDM_API_KEY = "iby2oEA1IdVPmem1X9sh9wwLGFzmWGdImLB9EfyFMzSYSl1OIwzLmnLLXa4U77zA";

        // KT API Key
        public const string SIMPLE_MDM_API_KEY = "oI8li8GZQGnw8s74TV4ixlTs3P6gGeopsDn7tvfdhDH58giLeqZ93O8pbQlnhHf1";

        //DELETE https://a.simplemdm.com/api/v1/devices/{DEVICE_ID}
        public const string SIMPLE_MDM_DELETE_API = "https://a.simplemdm.com/api/v1/devices/";

        //GET https://a.simplemdm.com/api/v1/devices/{DEVICE_ID}
        public const string SIMPLE_MDM_DEVICEINFO_API = "https://a.simplemdm.com/api/v1/devices/";

        // DELETE https://a.simplemdm.com/api/v1/devices/{DEVICE_ID}/lost_mode
        public const string SIMPLE_MDM_LOST_MODE_DISABLE = "https://a.simplemdm.com/api/v1/devices/{0}/lost_mode";

        public string Get()
        {
            string imei = string.Empty;

            NSUserDefaults userData = NSUserDefaults.StandardUserDefaults;
            NSDictionary dict = userData.DictionaryForKey("com.apple.configuration.managed");
            if (dict != null)
            {
                imei = "NODATA";
                var nsData = dict[new NSString("imei")];
                if (nsData != null)
                {
                    imei = nsData.ToString();
                    imei = imei.Replace(" ", string.Empty);

                    if (imei.Length == 0)
                        imei = "NOIMEI";
                }
            }

            return imei;
        }

        public void GetWithCompletionHandler(Action<string> completionHandler)
        {
            List<string> imeiList = new List<string>();

            string deviceID = GetMDMID();

            if (deviceID.Length > 0)
            {
                Task.Run(async () =>
                {
                    HttpClient httpClient = new System.Net.Http.HttpClient();
                    string requestURL = $"{SIMPLE_MDM_DEVICEINFO_API}{deviceID}";
                    Debug.WriteLine($"RequestURL [{requestURL}]");
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestURL);

                    string auth = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(SIMPLE_MDM_API_KEY));
                    request.Headers.Add("Authorization", "Basic " + auth);

                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    bool bIsSucceeded = response.IsSuccessStatusCode;
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"StatusCode[{response.StatusCode}] - Body [{responseBody}]");

                    device_info_holder holder = JsonConvert.DeserializeObject<device_info_holder>(responseBody);
                    Debug.WriteLine(holder.ToString());

                    if(holder != null && holder.data != null && holder.data.attributes != null && holder.data.attributes.service_subscriptions != null)
                    {
                        for(int i = 0; i <  holder.data.attributes.service_subscriptions.Count; i++)
                        {
                            string imei = holder.data.attributes.service_subscriptions[i].imei;
                            imei = imei.Replace(" ", string.Empty);

                            imeiList.Add(imei);
                        }
                    }

                    completionHandler?.Invoke(JsonConvert.SerializeObject(imeiList));
                });
            }
            else
            {
                completionHandler?.Invoke(JsonConvert.SerializeObject(imeiList));
            }
        }

        public string GetMDMID()
        {
            if (MDM_ID.Length > 0)
                return MDM_ID;

            //return "1192991";
            NSUserDefaults userData = NSUserDefaults.StandardUserDefaults;
            NSDictionary dict = userData.DictionaryForKey("com.apple.configuration.managed");
            if (dict != null)
            {
                NSObject objID = dict[new NSString("id")];
                if (objID != null)
                {
                    MDM_ID = objID.ToString();
                    return MDM_ID; ;
                }
            }

            return "";
        }

        public void DeleteDevice()
        {
            string deviceID = GetMDMID();
            if (deviceID.Length == 0)
                return;

            Task.Run(async () =>
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                string requestURL = $"{SIMPLE_MDM_DELETE_API}{deviceID}";
                System.Console.WriteLine($"RequestURL [{requestURL}]");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestURL);

                string auth = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(SIMPLE_MDM_API_KEY));
                request.Headers.Add("Authorization", "Basic " + auth);

                HttpResponseMessage response = await httpClient.SendAsync(request);
                bool bIsSucceeded = response.IsSuccessStatusCode;
                string responseBody = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"StatusCode[{response.StatusCode}] - Body [{responseBody}]");
            });

            /*
            // Remove device from simpleMDM
            //FIXME: 바로 지우면 실행 하던 앱도 바로 꺼져버려서 IMEI얻은 직후 삭제 하는건 안되고 나중에 적절한 시점을 잡아 웹쪽에서 API를 직접 콜해서 지우는 과정이 필요할듯 하다.

            // 자바스크립트 코드
            fetch('https://a.simplemdm.com/api/v1/devices/' + device_id, {
                headers: {
                    'Authorization': 'Basic ' + btoa('iby2oEA1IdVPmem1X9sh9wwLGFzmWGdImLB9EfyFMzSYSl1OIwzLmnLLXa4U77zA')
                },
                method: 'DELETE'
            });
            //
            */
        }

        public void LostModeDisable()
        {
            string deviceID = GetMDMID();
            if (deviceID.Length == 0)
                return;

            Task.Run(async () =>
            {
                HttpClient httpClient = new System.Net.Http.HttpClient();
                string requestURL = string.Format(SIMPLE_MDM_LOST_MODE_DISABLE, deviceID);
                System.Console.WriteLine($"RequestURL [{requestURL}]");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestURL);

                string auth = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(SIMPLE_MDM_API_KEY));
                request.Headers.Add("Authorization", "Basic " + auth);

                HttpResponseMessage response = await httpClient.SendAsync(request);
                bool bIsSucceeded = response.IsSuccessStatusCode;
                string responseBody = await response.Content.ReadAsStringAsync();
                System.Console.WriteLine($"StatusCode[{response.StatusCode}] - Body [{responseBody}]");
            });
        }
    }
}