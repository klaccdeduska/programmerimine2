namespace KooliProjekt.WpfApplication
{
    public interface IDialogProvider
    {
        void ShowError(string message);
        void ShowMessage(string message);
        bool ConfirmDelete(AutoModel auto);
    }
}