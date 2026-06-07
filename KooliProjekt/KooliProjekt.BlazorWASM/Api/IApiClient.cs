using KooliProjekt.BlazorWASM.Models;

namespace KooliProjekt.BlazorWASM.Api
{
    public interface IApiClient
    {
        Task<List<AutoModel>> GetAutosAsync();
    }
}