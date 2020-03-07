using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Infrastructure.Helpers
{
    public static class ExternalApiHelper
    {
        public static object GetData<T>(string uri) 
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(uri)
            };
            try
            {
                var result = new object();
                var response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode) return result;
                string responseAsString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<T>(responseAsString);
                return (T)result;
            }
            finally { httpClient.CancelPendingRequests(); }
        }

    }
}
