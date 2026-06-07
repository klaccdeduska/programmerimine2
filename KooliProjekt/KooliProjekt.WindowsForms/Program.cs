using KooliProjekt.WindowsForms.Api;
using KooliProjekt.WindowsForms.Presenter;

namespace KooliProjekt.WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5086/")
            };

            IApiClient apiClient = new ApiClient(httpClient);

            var form = new Form1();
            var presenter = new AutosPresenter(form, apiClient);

            form.Presenter = presenter;

            Application.Run(form);
        }
    }
}