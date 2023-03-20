using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace CheckClikClient.Handlers
{
    public class CommonHeader
    {
        private static string apiURL = "";
        private readonly AppSettingsKeysInfo _options;

        public CommonHeader(IOptions<AppSettingsKeysInfo> options)
        { 
            _options = options.Value;
            apiURL = _options.apiurl;
        }
        //public static void setHeaders(HttpClient client)
        public void setHeaders(HttpClient client)
        {
            client.BaseAddress = new Uri(apiURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("API_Key", "1233456");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            //JObject obj = new JObject(new JProperty("Username", "admin"),
            //                          new JProperty("Key_Pair", "csinfo"),
            //                          new JProperty("Pair_Value", "infocs"));
            //HttpResponseMessage responseMessage = client.PostAsJsonAsync("api/LoginAPI", obj).Result;
            //if (responseMessage.IsSuccessStatusCode)
            //{

            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseMessage.Content.ReadAsStringAsync().Result);
            //}
        }
    }
}