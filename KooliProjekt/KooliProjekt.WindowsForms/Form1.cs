using System.ComponentModel;
using System.Text;
using KooliProjekt.WindowsForms.Api;
using KooliProjekt.WindowsForms.Models;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        private readonly IApiClient _apiClient;
        private BindingList<AutoModel> _autos = new();

        public Form1()
            : this(new ApiClient(new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5086/")
            }))
        {
        }

        public Form1(IApiClient apiClient)
        {
            _apiClient = apiClient;

            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var result = await _apiClient.GetAutosAsync();

            if (result.HasErrors)
            {
                ShowError(result);
                return;
            }

            _autos = new BindingList<AutoModel>(result.Value ?? new List<AutoModel>());

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _autos;
        }

        private async void addButton_Click(object sender, EventArgs e)
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

            await LoadData();
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();

            var auto = GetSelectedAuto();

            if (auto == null)
            {
                MessageBox.Show("Vali rida, mida salvestada.");
                return;
            }

            var result = await _apiClient.SaveAutoAsync(auto);

            if (result.HasErrors)
            {
                ShowError(result);
                return;
            }

            await LoadData();
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            var auto = GetSelectedAuto();

            if (auto == null)
            {
                MessageBox.Show("Vali rida, mida kustutada.");
                return;
            }

            if (auto.Id <= 0)
            {
                MessageBox.Show("Seda rida ei saa kustutada, sest ID puudub.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Kas kustutada auto {auto.Tootja} {auto.Mudel}?",
                "Kinnitus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            var result = await _apiClient.DeleteAutoAsync(auto.Id);

            if (result.HasErrors)
            {
                ShowError(result);
                return;
            }

            await LoadData();
        }

        private AutoModel GetSelectedAuto()
        {
            if (dataGridView1.CurrentRow == null)
            {
                return null;
            }

            return dataGridView1.CurrentRow.DataBoundItem as AutoModel;
        }

        private void ShowError(OperationResult result)
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

            MessageBox.Show(
                message.ToString(),
                "Viga",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}