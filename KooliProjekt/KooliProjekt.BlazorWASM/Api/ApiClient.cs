using System.Net.Http.Json;
using System.Text.Json;
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

        public async Task<OperationResult<List<AutoModel>>> GetAutosAsync()
        {
            var response = await _httpClient.GetAsync("api/Autos?page=1&pageSize=100");

            if (!response.IsSuccessStatusCode)
            {
                return await CreateErrorResult<List<AutoModel>>(response);
            }

            var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<AutoModel>>();

            return new OperationResult<List<AutoModel>>
            {
                Value = pagedResult?.Results ?? new List<AutoModel>()
            };
        }

        public async Task<OperationResult<AutoModel>> GetAutoAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Autos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return await CreateErrorResult<AutoModel>(response);
            }

            var auto = await response.Content.ReadFromJsonAsync<AutoModel>();

            return new OperationResult<AutoModel>
            {
                Value = auto
            };
        }

        public async Task<OperationResult<AutoModel>> SaveAutoAsync(AutoModel auto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Autos", auto);

            if (!response.IsSuccessStatusCode)
            {
                return await CreateErrorResult<AutoModel>(response);
            }

            var savedAuto = await response.Content.ReadFromJsonAsync<AutoModel>();

            return new OperationResult<AutoModel>
            {
                Value = savedAuto
            };
        }

        private static async Task<OperationResult<T>> CreateErrorResult<T>(HttpResponseMessage response)
        {
            var result = new OperationResult<T>();

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                result.AddError($"API error: {(int)response.StatusCode} {response.ReasonPhrase}");
                return result;
            }

            try
            {
                using var document = JsonDocument.Parse(content);
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in root.EnumerateArray())
                    {
                        result.AddError(item.ToString());
                    }

                    return result;
                }

                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("title", out var title))
                    {
                        result.AddError(title.GetString() ?? "API error");
                    }

                    if (root.TryGetProperty("errors", out var errors))
                    {
                        ReadErrors(result, errors);
                    }

                    if (root.TryGetProperty("propertyErrors", out var propertyErrors))
                    {
                        ReadErrors(result, propertyErrors);
                    }

                    if (root.TryGetProperty("Errors", out var upperErrors) &&
                        upperErrors.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var error in upperErrors.EnumerateArray())
                        {
                            result.AddError(error.ToString());
                        }
                    }

                    if (!result.HasErrors)
                    {
                        result.AddError(content);
                    }

                    return result;
                }
            }
            catch
            {
                result.AddError(content);
                return result;
            }

            result.AddError(content);
            return result;
        }

        private static void ReadErrors<T>(OperationResult<T> result, JsonElement errors)
        {
            if (errors.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in errors.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var error in property.Value.EnumerateArray())
                        {
                            result.AddPropertyError(property.Name, error.ToString());
                        }
                    }
                    else
                    {
                        result.AddPropertyError(property.Name, property.Value.ToString());
                    }
                }
            }
            else if (errors.ValueKind == JsonValueKind.Array)
            {
                foreach (var error in errors.EnumerateArray())
                {
                    result.AddError(error.ToString());
                }
            }
        }
    }
}