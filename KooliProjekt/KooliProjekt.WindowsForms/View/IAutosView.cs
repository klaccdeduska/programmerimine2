using KooliProjekt.WindowsForms.Models;

namespace KooliProjekt.WindowsForms.View
{
    public interface IAutosView
    {
        IList<AutoModel> Autos { set; }

        int CurrentId { get; set; }
        string CurrentTootja { get; set; }
        string CurrentMudel { get; set; }
        string CurrentNumbrimark { get; set; }

        void ShowError(OperationResult result);
        void ShowMessage(string message);
        bool ConfirmDelete();
    }
}