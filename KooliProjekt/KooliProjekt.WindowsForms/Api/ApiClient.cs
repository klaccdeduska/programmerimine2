using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Models;

namespace KooliProjekt.WindowsForms.Api
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

        public async Task<AutoModel> SaveAutoAsync(AutoModel auto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Autos", auto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AutoModel>();

            return result;
        }

        public async Task<bool> DeleteAutoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Autos/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<bool>();

            return result;
        }
    }
}