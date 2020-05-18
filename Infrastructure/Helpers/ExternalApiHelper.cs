using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Infrastructure.Helpers
{
    public class ExternalApiHelper
    {
        private HttpClient httpClient = new HttpClient();
        public T GetData<T>(string uri) where T : class
        {
            try
            {
                T result = null;
                var response = httpClient.GetAsync(uri).Result;
                if (!response.IsSuccessStatusCode) return null;
                string responseAsString = response.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<T>(responseAsString);
                return result;
            }
            finally { httpClient.CancelPendingRequests(); }
        }

    }
}
