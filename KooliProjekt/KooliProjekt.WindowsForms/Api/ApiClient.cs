using System.Net.Http.Json;
using KooliProjekt.WindowsForms.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KooliProjekt.WindowsForms.Api
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

        public async Task<OperationResult<bool>> DeleteAutoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Autos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return await CreateErrorResult<bool>(response);
            }

            var deleted = await response.Content.ReadFromJsonAsync<bool>();

            return new OperationResult<bool>
            {
                Value = deleted
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
                var token = JToken.Parse(content);

                if (token is JArray array)
                {
                    foreach (var item in array)
                    {
                        result.AddError(item.ToString());
                    }

                    return result;
                }

                if (token is JObject obj)
                {
                    if (obj["title"] != null)
                    {
                        result.AddError(obj["title"]!.ToString());
                    }

                    if (obj["errors"] is JObject errorsObject)
                    {
                        foreach (var property in errorsObject.Properties())
                        {
                            foreach (var error in property.Value)
                            {
                                result.AddPropertyError(property.Name, error.ToString());
                            }
                        }
                    }

                    if (obj["propertyErrors"] is JObject propertyErrorsObject)
                    {
                        foreach (var property in propertyErrorsObject.Properties())
                        {
                            foreach (var error in property.Value)
                            {
                                result.AddPropertyError(property.Name, error.ToString());
                            }
                        }
                    }

                    if (obj["Errors"] is JArray errorsArray)
                    {
                        foreach (var error in errorsArray)
                        {
                            result.AddError(error.ToString());
                        }
                    }

                    if (obj["errors"] is JArray lowerErrorsArray)
                    {
                        foreach (var error in lowerErrorsArray)
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
    }
}