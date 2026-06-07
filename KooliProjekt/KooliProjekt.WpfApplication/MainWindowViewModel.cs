using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;

        private AutoModel _selectedAuto;
        private int _currentId;
        private string _currentTootja;
        private string _currentMudel;
        private string _currentNumbrimark;
        private string _errorMessage;

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
                _selectedAuto = value;
                OnPropertyChanged();

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
                _currentId = value;
                OnPropertyChanged();
            }
        }

        public string CurrentTootja
        {
            get => _currentTootja;
            set
            {
                _currentTootja = value;
                OnPropertyChanged();
            }
        }

        public string CurrentMudel
        {
            get => _currentMudel;
            set
            {
                _currentMudel = value;
                OnPropertyChanged();
            }
        }

        public string CurrentNumbrimark
        {
            get => _currentNumbrimark;
            set
            {
                _currentNumbrimark = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}