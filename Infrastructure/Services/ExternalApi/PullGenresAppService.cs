using Application.Services.ExternalApi;
using Domain.Common;
using Domain.External.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.ExternalApi
{
    public class PullGenresAppService : IPullGenresAppService
    {
        private readonly IOptions<ApiCredentials> apiCredentials;
        public PullGenresAppService(IOptions<ApiCredentials> apiCredentials)
        {
            this.apiCredentials = apiCredentials;
        }
        public async Task<List<ApiGenreDTO>> GetGenres(string uriSpeci)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiCredentials.Value.BaseUri)
            };
            List<ApiGenreDTO> resultList = new List<ApiGenreDTO>();
            try
            {
                string uri = $"{apiCredentials.Value.BaseUri}{uriSpeci}{apiCredentials.Value.PartUri}{apiCredentials.Value.ApiKey}";
                var response = await httpClient.GetAsync(uri);
                if(!response.IsSuccessStatusCode) throw new Exception("");
                string responseAsString = await response.Content.ReadAsStringAsync();
                var responseAsObject = JsonConvert.DeserializeObject<ApiGenreDTOList>(responseAsString);
                resultList = (responseAsObject.genres as IEnumerable<ApiGenreDTO>).ToList();
            }
            catch (Exception e) { }
            return resultList;
        }
    }
}
