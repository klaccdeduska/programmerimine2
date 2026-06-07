using System.Windows;

namespace KooliProjekt.WpfApplication
{
    public class DialogProvider : IDialogProvider
    {
        public void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Viga",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(
                message,
                "Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public bool ConfirmDelete(AutoModel auto)
        {
            if (auto == null)
            {
                return false;
            }

            var result = MessageBox.Show(
                $"Kas kustutada auto {auto.Tootja} {auto.Mudel}?",
                "Kinnitus",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }
    }
}