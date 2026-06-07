namespace KooliProjekt.WpfApplication
{
    public interface IApiClient
    {
        Task<OperationResult<List<AutoModel>>> GetAutosAsync();
        Task<OperationResult<AutoModel>> SaveAutoAsync(AutoModel auto);
        Task<OperationResult<bool>> DeleteAutoAsync(int id);
    }
}