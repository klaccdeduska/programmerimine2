using System.ComponentModel;
using KooliProjekt.WindowsForms.Api;
using KooliProjekt.WindowsForms.Models;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        private readonly IApiClient _apiClient;
        private BindingList<AutoModel> _autos = new();

        // Для конструктора Visual Studio Designer
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
            try
            {
                var autos = await _apiClient.GetAutosAsync();

                _autos = new BindingList<AutoModel>(autos);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = _autos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Andmete laadimine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                var auto = new AutoModel
                {
                    Id = 0,
                    Tootja = "Uus tootja",
                    Mudel = "Uus mudel",
                    Numbrimark = "NEW" + DateTime.Now.Ticks.ToString()[^4..]
                };

                await _apiClient.SaveAutoAsync(auto);

                await LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Auto lisamine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.EndEdit();

                var auto = GetSelectedAuto();

                if (auto == null)
                {
                    MessageBox.Show("Vali rida, mida salvestada.");
                    return;
                }

                await _apiClient.SaveAutoAsync(auto);

                await LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Auto salvestamine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            try
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

                await _apiClient.DeleteAutoAsync(auto.Id);

                await LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Auto kustutamine ebaõnnestus: " + ex.Message,
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private AutoModel GetSelectedAuto()
        {
            if (dataGridView1.CurrentRow == null)
            {
                return null;
            }

            return dataGridView1.CurrentRow.DataBoundItem as AutoModel;
        }
    }
}