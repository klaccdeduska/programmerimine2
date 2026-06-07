using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Поменяй порт на свой порт WebAPI из Swagger
            var apiBaseUrl = "http://localhost:5086/";

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiBaseUrl)
            };

            IApiClient apiClient = new ApiClient(httpClient);

            Application.Run(new Form1(apiClient));
        }
    }
}