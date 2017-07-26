using System;
using System.Collections.Generic;
using System.Web;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Images
{
    class APIManager
    {
        private static string baseURL = "https://api.cognitive.microsoft.com/bing/v7.0/images/search";
        private static string API_KEY_1 = "1ce8d103aaf2405188ee561f25868ccb";
        private static string API_KEY_2 = "24fdf7a8b9994de3bb8083f5083017fa";

        private static APIManager instance;

        private APIManager() { }
        public static APIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new APIManager();
                }
                return instance;
            }
        }
        private string BuildUrl(string searchedString)
        {
            var uriBuilder = new UriBuilder(baseURL);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = searchedString;
            query["mkt"] = "en-us";
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        public async Task<JsonValue> FetchImagesAsyn(string searchedString)
        {
            string url = BuildUrl(searchedString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers["Ocp-Apim-Subscription-Key"] = API_KEY_1;

            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    JsonValue jsonDoc = await Task.Run(() => JsonValue.Load(stream));
                    return jsonDoc;
                }
            }
        }

        internal List<string> ParseJson(JsonValue json)
        {
            List<string> result = new List<string>();
            JObject jObject = JObject.Parse(json.ToString());
            int index = 0;
            string str = (string)jObject["value"][index]["contentUrl"];
            while (str != null && index < 30)
            {
                result.Add(str);
                index++;
                str = (string)jObject["value"][index]["contentUrl"];
            }
            return result;
        }
    }
}