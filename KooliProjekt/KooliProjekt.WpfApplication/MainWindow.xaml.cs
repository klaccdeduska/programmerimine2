using System.Net.Http;
using System.Windows;

namespace KooliProjekt.WpfApplication
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5086/")
            };

            IApiClient apiClient = new ApiClient(httpClient);

            _viewModel = new MainWindowViewModel(apiClient);

            DataContext = _viewModel;

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadDataAsync();
        }
    }
}