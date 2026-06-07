namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private Panel topPanel;
        private Button addButton;
        private Button saveButton;
        private Button deleteButton;

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
            addButton = new Button();
            saveButton = new Button();
            deleteButton = new Button();

            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            topPanel.SuspendLayout();
            SuspendLayout();

            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 45;
            topPanel.Controls.Add(addButton);
            topPanel.Controls.Add(saveButton);
            topPanel.Controls.Add(deleteButton);

            addButton.Location = new Point(10, 8);
            addButton.Name = "addButton";
            addButton.Size = new Size(120, 30);
            addButton.Text = "Lisa uus";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += addButton_Click;

            saveButton.Location = new Point(140, 8);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(120, 30);
            saveButton.Text = "Salvesta";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;

            deleteButton.Location = new Point(270, 8);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(120, 30);
            deleteButton.Text = "Kustuta";
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 45);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(800, 405);
            dataGridView1.TabIndex = 0;

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Controls.Add(topPanel);
            Name = "Form1";
            Text = "KooliProjekt WindowsForms";
            Load += Form1_Load;

            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            topPanel.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}