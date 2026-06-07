using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel : NotifyPropertyChangedBase
    {
        private readonly IApiClient _apiClient;
        private readonly IDialogProvider _dialogProvider;

        private AutoModel _selectedAuto;
        private int _currentId;
        private string _currentTootja;
        private string _currentMudel;
        private string _currentNumbrimark;
        private string _errorMessage;

        public MainWindowViewModel()
            : this(
                new ApiClient(new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5086/")
                }),
                new DialogProvider())
        {
        }

        public MainWindowViewModel(IApiClient apiClient, IDialogProvider dialogProvider)
        {
            _apiClient = apiClient;
            _dialogProvider = dialogProvider;

            AddNewCommand = new RelayCommand(async _ => await AddNewAsync());
            SaveCommand = new RelayCommand(async _ => await SaveAsync());
            DeleteCommand = new RelayCommand(async _ => await DeleteAsync());
        }

        public ObservableCollection<AutoModel> Autos { get; } = new();

        public ICommand AddNewCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

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
                ShowError(result);
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

        private async Task AddNewAsync()
        {
            var auto = new AutoModel
            {
                Id = 0,
                Tootja = "Uus tootja",
                Mudel = "Uus mudel",
                Numbrimark = "NEW" + DateTime.Now.Ticks.ToString()[^4..]
            };

            var result = await _apiClient.SaveAutoAsync(auto);

            if (result.HasErrors)
            {
                ShowError(result);
                return;
            }

            await LoadDataAsync();
        }

        private async Task SaveAsync()
        {
            var auto = new AutoModel
            {
                Id = CurrentId,
                Tootja = CurrentTootja,
                Mudel = CurrentMudel,
                Numbrimark = CurrentNumbrimark
            };

            var result = await _apiClient.SaveAutoAsync(auto);

            if (result.HasErrors)
            {
                ShowError(result);
                return;
            }

            await LoadDataAsync();
        }

        private async Task DeleteAsync()
        {
            if (CurrentId <= 0 || SelectedAuto == null)
            {
                _dialogProvider.ShowMessage("Vali rida, mida kustutada.");
                return;
            }

            if (!_dialogProvider.ConfirmDelete(SelectedAuto))
            {
                return;
            }

            var result = await _apiClient.DeleteAutoAsync(CurrentId);

            if (result.HasErrors)
            {
                ShowError(result);
                return;
            }

            CurrentId = 0;
            CurrentTootja = "";
            CurrentMudel = "";
            CurrentNumbrimark = "";

            await LoadDataAsync();
        }

        private void ShowError(OperationResult result)
        {
            var message = FormatErrors(result);

            ErrorMessage = message;
            _dialogProvider.ShowError(message);
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