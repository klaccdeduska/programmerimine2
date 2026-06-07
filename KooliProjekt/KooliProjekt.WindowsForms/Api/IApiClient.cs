using KooliProjekt.WindowsForms.Models;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<List<AutoModel>> GetAutosAsync();
        Task<AutoModel> SaveAutoAsync(AutoModel auto);
        Task<bool> DeleteAutoAsync(int id);
    }
}