using KooliProjekt.BlazorWASM.Models;

namespace KooliProjekt.BlazorWASM.Api
{
    public interface IApiClient
    {
        Task<OperationResult<List<AutoModel>>> GetAutosAsync();
        Task<OperationResult<AutoModel>> GetAutoAsync(int id);
        Task<OperationResult<AutoModel>> SaveAutoAsync(AutoModel auto);
    }
}