using System.Text;
using KooliProjekt.WindowsForms.Models;
using KooliProjekt.WindowsForms.Presenter;
using KooliProjekt.WindowsForms.View;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form, IAutosView
    {
        private AutosPresenter _presenter;

        public AutosPresenter Presenter
        {
            set
            {
                if (_presenter != null)
                {
                    addButton.Click -= _presenter.AddCommand_Click;
                    saveButton.Click -= _presenter.SaveCommand_Click;
                    deleteButton.Click -= _presenter.DeleteCommand_Click;
                }

                _presenter = value;

                if (_presenter != null)
                {
                    addButton.Click += _presenter.AddCommand_Click;
                    saveButton.Click += _presenter.SaveCommand_Click;
                    deleteButton.Click += _presenter.DeleteCommand_Click;
                }
            }
        }

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
            if (_presenter != null)
            {
                await _presenter.LoadDataAsync();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var auto = dataGridView1.CurrentRow?.DataBoundItem as AutoModel;

            _presenter?.SelectionChanged(auto);
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

        public bool ConfirmDelete()
        {
            return MessageBox.Show(
                $"Kas kustutada auto {CurrentTootja} {CurrentMudel}?",
                "Kinnitus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}