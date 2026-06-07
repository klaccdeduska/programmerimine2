namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        internal DataGridView dataGridView1;
        internal Panel topPanel;
        internal Panel editPanel;

        internal Button addButton;
        internal Button saveButton;
        internal Button deleteButton;

        internal Label idLabel;
        internal Label tootjaLabel;
        internal Label mudelLabel;
        internal Label numbrimarkLabel;

        internal TextBox idTextBox;
        internal TextBox tootjaTextBox;
        internal TextBox mudelTextBox;
        internal TextBox numbrimarkTextBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            topPanel = new Panel();
            editPanel = new Panel();

            addButton = new Button();
            saveButton = new Button();
            deleteButton = new Button();

            idLabel = new Label();
            tootjaLabel = new Label();
            mudelLabel = new Label();
            numbrimarkLabel = new Label();

            idTextBox = new TextBox();
            tootjaTextBox = new TextBox();
            mudelTextBox = new TextBox();
            numbrimarkTextBox = new TextBox();

            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            topPanel.SuspendLayout();
            editPanel.SuspendLayout();
            SuspendLayout();

            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 45;
            topPanel.Controls.Add(addButton);
            topPanel.Controls.Add(saveButton);
            topPanel.Controls.Add(deleteButton);

            addButton.Location = new Point(10, 8);
            addButton.Size = new Size(120, 30);
            addButton.Text = "Lisa uus";

            saveButton.Location = new Point(140, 8);
            saveButton.Size = new Size(120, 30);
            saveButton.Text = "Salvesta";

            deleteButton.Location = new Point(270, 8);
            deleteButton.Size = new Size(120, 30);
            deleteButton.Text = "Kustuta";

            editPanel.Dock = DockStyle.Top;
            editPanel.Height = 95;

            idLabel.Text = "Id";
            idLabel.Location = new Point(10, 15);
            idLabel.Size = new Size(90, 23);

            idTextBox.Location = new Point(110, 12);
            idTextBox.Size = new Size(120, 27);
            idTextBox.ReadOnly = true;

            tootjaLabel.Text = "Tootja";
            tootjaLabel.Location = new Point(250, 15);
            tootjaLabel.Size = new Size(90, 23);

            tootjaTextBox.Location = new Point(350, 12);
            tootjaTextBox.Size = new Size(180, 27);

            mudelLabel.Text = "Mudel";
            mudelLabel.Location = new Point(10, 55);
            mudelLabel.Size = new Size(90, 23);

            mudelTextBox.Location = new Point(110, 52);
            mudelTextBox.Size = new Size(120, 27);

            numbrimarkLabel.Text = "Numbrimark";
            numbrimarkLabel.Location = new Point(250, 55);
            numbrimarkLabel.Size = new Size(90, 23);

            numbrimarkTextBox.Location = new Point(350, 52);
            numbrimarkTextBox.Size = new Size(180, 27);

            editPanel.Controls.Add(idLabel);
            editPanel.Controls.Add(idTextBox);
            editPanel.Controls.Add(tootjaLabel);
            editPanel.Controls.Add(tootjaTextBox);
            editPanel.Controls.Add(mudelLabel);
            editPanel.Controls.Add(mudelTextBox);
            editPanel.Controls.Add(numbrimarkLabel);
            editPanel.Controls.Add(numbrimarkTextBox);

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 550);

            Controls.Add(dataGridView1);
            Controls.Add(editPanel);
            Controls.Add(topPanel);

            Name = "Form1";
            Text = "KooliProjekt WindowsForms MVP";
            Load += Form1_Load;

            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            topPanel.ResumeLayout(false);
            editPanel.ResumeLayout(false);
            editPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}