using KooliProjekt.WindowsForms.Api;
using KooliProjekt.WindowsForms.Models;
using KooliProjekt.WindowsForms.View;

namespace KooliProjekt.WindowsForms.Presenter
{
    public class AutosPresenter
    {
        private readonly IAutosView _view;
        private readonly IApiClient _apiClient;

        public AutosPresenter(IAutosView view, IApiClient apiClient)
        {
            _view = view;
            _apiClient = apiClient;
        }

        public async Task LoadDataAsync()
        {
            var result = await _apiClient.GetAutosAsync();

            if (result.HasErrors)
            {
                _view.ShowError(result);
                return;
            }

            _view.Autos = result.Value ?? new List<AutoModel>();
        }

        public void SelectionChanged(AutoModel auto)
        {
            if (auto == null)
            {
                return;
            }

            _view.CurrentId = auto.Id;
            _view.CurrentTootja = auto.Tootja;
            _view.CurrentMudel = auto.Mudel;
            _view.CurrentNumbrimark = auto.Numbrimark;
        }

        public async void AddCommand_Click(object sender, EventArgs e)
        {
            await AddNew();
        }

        public async void SaveCommand_Click(object sender, EventArgs e)
        {
            await Save();
        }

        public async void DeleteCommand_Click(object sender, EventArgs e)
        {
            await Delete();
        }

        public async Task AddNew()
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
                _view.ShowError(result);
                return;
            }

            await LoadDataAsync();
        }

        public async Task Save()
        {
            var auto = new AutoModel
            {
                Id = _view.CurrentId,
                Tootja = _view.CurrentTootja,
                Mudel = _view.CurrentMudel,
                Numbrimark = _view.CurrentNumbrimark
            };

            var result = await _apiClient.SaveAutoAsync(auto);

            if (result.HasErrors)
            {
                _view.ShowError(result);
                return;
            }

            await LoadDataAsync();
        }

        public async Task Delete()
        {
            if (_view.CurrentId <= 0)
            {
                _view.ShowMessage("Vali rida, mida kustutada.");
                return;
            }

            if (!_view.ConfirmDelete())
            {
                return;
            }

            var result = await _apiClient.DeleteAutoAsync(_view.CurrentId);

            if (result.HasErrors)
            {
                _view.ShowError(result);
                return;
            }

            _view.CurrentId = 0;
            _view.CurrentTootja = "";
            _view.CurrentMudel = "";
            _view.CurrentNumbrimark = "";

            await LoadDataAsync();
        }
    }
}