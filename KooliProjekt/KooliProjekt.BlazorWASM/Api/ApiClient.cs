using System.Net.Http.Json;
using KooliProjekt.BlazorWASM.Models;

namespace KooliProjekt.BlazorWASM.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AutoModel>> GetAutosAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<PagedResult<AutoModel>>(
                "api/Autos?page=1&pageSize=100");

            return result?.Results ?? new List<AutoModel>();
        }
    }
}