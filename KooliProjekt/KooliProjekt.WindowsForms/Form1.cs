using System.Text;
using KooliProjekt.WindowsForms.Models;
using KooliProjekt.WindowsForms.Presenter;
using KooliProjekt.WindowsForms.View;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form, IAutosView
    {
        public AutosPresenter Presenter { private get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        public IList<AutoModel> Autos
        {
            set
            {
                dataGridView1.DataSource = null;
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = value;
            }
        }

        public int CurrentId
        {
            get
            {
                if (int.TryParse(idTextBox.Text, out var id))
                {
                    return id;
                }

                return 0;
            }
            set
            {
                idTextBox.Text = value.ToString();
            }
        }

        public string CurrentTootja
        {
            get => tootjaTextBox.Text;
            set => tootjaTextBox.Text = value;
        }

        public string CurrentMudel
        {
            get => mudelTextBox.Text;
            set => mudelTextBox.Text = value;
        }

        public string CurrentNumbrimark
        {
            get => numbrimarkTextBox.Text;
            set => numbrimarkTextBox.Text = value;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            if (Presenter != null)
            {
                await Presenter.LoadDataAsync();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var auto = dataGridView1.CurrentRow?.DataBoundItem as AutoModel;

            Presenter?.SelectionChanged(auto);
        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            if (Presenter != null)
            {
                await Presenter.AddNewAsync();
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (Presenter != null)
            {
                await Presenter.SaveAsync();
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            if (Presenter != null)
            {
                await Presenter.DeleteAsync();
            }
        }

        public void ShowError(OperationResult result)
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

        public void ShowMessage(string message)
        {
            MessageBox.Show(
                message,
                "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public bool Confirm(string message)
        {
            return MessageBox.Show(
                message,
                "Kinnitus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}