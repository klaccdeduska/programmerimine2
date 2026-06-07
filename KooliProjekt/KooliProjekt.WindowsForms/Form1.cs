using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using KooliProjekt.WindowsForms.Models;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();

        // Поменяй порт на свой WebAPI порт из Swagger
        private const string ApiBaseUrl = "http://localhost:5086";

        public Form1()
        {
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
                var url = $"{ApiBaseUrl}/api/Autos?page=1&pageSize=100";

                var json = await _httpClient.GetStringAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<PagedResult<AutoModel>>(json, options);

                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = result?.Results;
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
    }
}