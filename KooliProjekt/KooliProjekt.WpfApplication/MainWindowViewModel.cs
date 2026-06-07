using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;

        private AutoModel _selectedAuto;
        private int _currentId;
        private string _currentTootja;
        private string _currentMudel;
        private string _currentNumbrimark;
        private string _errorMessage;

        public MainWindowViewModel()
            : this(new ApiClient(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5086/")
            }))
        {
        }

        public MainWindowViewModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public ObservableCollection<AutoModel> Autos { get; } = new();

        public AutoModel SelectedAuto
        {
            get => _selectedAuto;
            set
            {
                if (_selectedAuto == value)
                {
                    return;
                }

                _selectedAuto = value;
                NotifyPropertyChanged();

                if (value == null)
                {
                    CurrentId = 0;
                    CurrentTootja = "";
                    CurrentMudel = "";
                    CurrentNumbrimark = "";
                    return;
                }

                CurrentId = value.Id;
                CurrentTootja = value.Tootja;
                CurrentMudel = value.Mudel;
                CurrentNumbrimark = value.Numbrimark;
            }
        }

        public int CurrentId
        {
            get => _currentId;
            set
            {
                if (_currentId == value)
                {
                    return;
                }

                _currentId = value;
                NotifyPropertyChanged();
            }
        }

        public string CurrentTootja
        {
            get => _currentTootja;
            set
            {
                if (_currentTootja == value)
                {
                    return;
                }

                _currentTootja = value;
                NotifyPropertyChanged();

                if (SelectedAuto != null && SelectedAuto.Tootja != value)
                {
                    SelectedAuto.Tootja = value;
                }
            }
        }

        public string CurrentMudel
        {
            get => _currentMudel;
            set
            {
                if (_currentMudel == value)
                {
                    return;
                }

                _currentMudel = value;
                NotifyPropertyChanged();

                if (SelectedAuto != null && SelectedAuto.Mudel != value)
                {
                    SelectedAuto.Mudel = value;
                }
            }
        }

        public string CurrentNumbrimark
        {
            get => _currentNumbrimark;
            set
            {
                if (_currentNumbrimark == value)
                {
                    return;
                }

                _currentNumbrimark = value;
                NotifyPropertyChanged();

                if (SelectedAuto != null && SelectedAuto.Numbrimark != value)
                {
                    SelectedAuto.Numbrimark = value;
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage == value)
                {
                    return;
                }

                _errorMessage = value;
                NotifyPropertyChanged();
            }
        }

        public async Task LoadDataAsync()
        {
            var result = await _apiClient.GetAutosAsync();

            if (result.HasErrors)
            {
                ErrorMessage = FormatErrors(result);
                return;
            }

            ErrorMessage = "";

            Autos.Clear();

            foreach (var auto in result.Value ?? new List<AutoModel>())
            {
                Autos.Add(auto);
            }

            SelectedAuto = Autos.FirstOrDefault();
        }

        private string FormatErrors(OperationResult result)
        {
            var message = new StringBuilder();

            foreach (var error in result.Errors)
            {
                message.AppendLine(error);
            }

            foreach (var propertyError in result.PropertyErrors)
            {
                foreach (var error in propertyError.Value)
                {
                    message.AppendLine($"{propertyError.Key}: {error}");
                }
            }

            if (message.Length == 0)
            {
                message.AppendLine("Tundmatu viga.");
            }

            return message.ToString();
        }
    }
}