namespace FbsBatchSearchUtility
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.fileNameInput = new System.Windows.Forms.TextBox();
            this.openButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.founder = new System.Windows.Forms.CheckBox();
            this.ogrn = new System.Windows.Forms.CheckBox();
            this.inn = new System.Windows.Forms.CheckBox();
            this.shortOrgName = new System.Windows.Forms.CheckBox();
            this.fullOrgName = new System.Windows.Forms.CheckBox();
            this.resultsDataGrid = new System.Windows.Forms.DataGridView();
            this.IdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShortNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OgrnColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FounderColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.listingControlsPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lastPageButton = new System.Windows.Forms.Button();
            this.nextPageButton = new System.Windows.Forms.Button();
            this.currentPagelabel = new System.Windows.Forms.Label();
            this.previousPageButton = new System.Windows.Forms.Button();
            this.firstPageButton = new System.Windows.Forms.Button();
            this.resultsPerPage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.resultsNumLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.listingControlsPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.SettingsToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1084, 24);
            this.MainMenu.TabIndex = 5;
            this.MainMenu.Text = "MainMenu";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItemClick);
            // 
            // SettingsToolStripMenuItem
            // 
            this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.SettingsToolStripMenuItem.Text = "Настройки";
            this.SettingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItemClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Controls.Add(this.searchButton);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.fileNameInput);
            this.panel1.Controls.Add(this.openButton);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 670);
            this.panel1.TabIndex = 6;
            // 
            // searchButton
            // 
            this.searchButton.Enabled = false;
            this.searchButton.Location = new System.Drawing.Point(172, 197);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 9;
            this.searchButton.Text = "Поиск";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Выберите пакетный файл";
            // 
            // fileNameInput
            // 
            this.fileNameInput.Location = new System.Drawing.Point(12, 26);
            this.fileNameInput.Name = "fileNameInput";
            this.fileNameInput.ReadOnly = true;
            this.fileNameInput.Size = new System.Drawing.Size(154, 20);
            this.fileNameInput.TabIndex = 7;
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(172, 24);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 6;
            this.openButton.Text = "Открыть...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.OpenButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.founder);
            this.groupBox1.Controls.Add(this.ogrn);
            this.groupBox1.Controls.Add(this.inn);
            this.groupBox1.Controls.Add(this.shortOrgName);
            this.groupBox1.Controls.Add(this.fullOrgName);
            this.groupBox1.Location = new System.Drawing.Point(12, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 138);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Используемые поисковые поля";
            // 
            // founder
            // 
            this.founder.AutoSize = true;
            this.founder.Location = new System.Drawing.Point(6, 111);
            this.founder.Name = "founder";
            this.founder.Size = new System.Drawing.Size(162, 17);
            this.founder.TabIndex = 4;
            this.founder.Text = "Наименование учредителя";
            this.founder.UseVisualStyleBackColor = true;
            this.founder.CheckedChanged += new System.EventHandler(this.FounderCheckedChanged);
            // 
            // ogrn
            // 
            this.ogrn.AutoSize = true;
            this.ogrn.Location = new System.Drawing.Point(6, 88);
            this.ogrn.Name = "ogrn";
            this.ogrn.Size = new System.Drawing.Size(55, 17);
            this.ogrn.TabIndex = 3;
            this.ogrn.Text = "ОГРН";
            this.ogrn.UseVisualStyleBackColor = true;
            this.ogrn.CheckedChanged += new System.EventHandler(this.OgrnCheckedChanged);
            // 
            // inn
            // 
            this.inn.AutoSize = true;
            this.inn.Location = new System.Drawing.Point(6, 65);
            this.inn.Name = "inn";
            this.inn.Size = new System.Drawing.Size(50, 17);
            this.inn.TabIndex = 2;
            this.inn.Text = "ИНН";
            this.inn.UseVisualStyleBackColor = true;
            this.inn.CheckedChanged += new System.EventHandler(this.InnCheckedChanged);
            // 
            // shortOrgName
            // 
            this.shortOrgName.AutoSize = true;
            this.shortOrgName.Location = new System.Drawing.Point(6, 42);
            this.shortOrgName.Name = "shortOrgName";
            this.shortOrgName.Size = new System.Drawing.Size(213, 17);
            this.shortOrgName.TabIndex = 1;
            this.shortOrgName.Text = "Краткое наименование организации";
            this.shortOrgName.UseVisualStyleBackColor = true;
            this.shortOrgName.CheckedChanged += new System.EventHandler(this.ShortOrgNameCheckedChanged);
            // 
            // fullOrgName
            // 
            this.fullOrgName.AutoSize = true;
            this.fullOrgName.Location = new System.Drawing.Point(6, 19);
            this.fullOrgName.Name = "fullOrgName";
            this.fullOrgName.Size = new System.Drawing.Size(209, 17);
            this.fullOrgName.TabIndex = 0;
            this.fullOrgName.Text = "Полное наименование организации";
            this.fullOrgName.UseVisualStyleBackColor = true;
            this.fullOrgName.CheckedChanged += new System.EventHandler(this.FullOrgNameCheckedChanged);
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.AllowUserToAddRows = false;
            this.resultsDataGrid.AllowUserToDeleteRows = false;
            this.resultsDataGrid.AllowUserToOrderColumns = true;
            this.resultsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.resultsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdColumn,
            this.FullNameColumn,
            this.ShortNameColumn,
            this.InnColumn,
            this.OgrnColumn,
            this.FounderColumn});
            this.resultsDataGrid.Location = new System.Drawing.Point(260, 24);
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.ReadOnly = true;
            this.resultsDataGrid.Size = new System.Drawing.Size(824, 636);
            this.resultsDataGrid.TabIndex = 7;
            // 
            // IdColumn
            // 
            this.IdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IdColumn.DataPropertyName = "ID";
            this.IdColumn.HeaderText = "ID";
            this.IdColumn.Name = "IdColumn";
            this.IdColumn.ReadOnly = true;
            this.IdColumn.Width = 55;
            // 
            // FullNameColumn
            // 
            this.FullNameColumn.DataPropertyName = "FullName";
            this.FullNameColumn.HeaderText = "Полное наименование";
            this.FullNameColumn.Name = "FullNameColumn";
            this.FullNameColumn.ReadOnly = true;
            this.FullNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ShortNameColumn
            // 
            this.ShortNameColumn.DataPropertyName = "ShortName";
            this.ShortNameColumn.HeaderText = "Краткое наименование";
            this.ShortNameColumn.Name = "ShortNameColumn";
            this.ShortNameColumn.ReadOnly = true;
            this.ShortNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // InnColumn
            // 
            this.InnColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.InnColumn.DataPropertyName = "INN";
            this.InnColumn.HeaderText = "ИНН";
            this.InnColumn.Name = "InnColumn";
            this.InnColumn.ReadOnly = true;
            this.InnColumn.Width = 80;
            // 
            // OgrnColumn
            // 
            this.OgrnColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OgrnColumn.DataPropertyName = "OGRN";
            this.OgrnColumn.HeaderText = "ОГРН";
            this.OgrnColumn.Name = "OgrnColumn";
            this.OgrnColumn.ReadOnly = true;
            // 
            // FounderColumn
            // 
            this.FounderColumn.DataPropertyName = "OwnerDepartment";
            this.FounderColumn.HeaderText = "Наименование учредителя";
            this.FounderColumn.Name = "FounderColumn";
            this.FounderColumn.ReadOnly = true;
            this.FounderColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // listingControlsPanel
            // 
            this.listingControlsPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.listingControlsPanel.Controls.Add(this.panel3);
            this.listingControlsPanel.Controls.Add(this.resultsPerPage);
            this.listingControlsPanel.Controls.Add(this.label4);
            this.listingControlsPanel.Controls.Add(this.resultsNumLabel);
            this.listingControlsPanel.Controls.Add(this.label1);
            this.listingControlsPanel.Controls.Add(this.exportButton);
            this.listingControlsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listingControlsPanel.Enabled = false;
            this.listingControlsPanel.Location = new System.Drawing.Point(260, 660);
            this.listingControlsPanel.MinimumSize = new System.Drawing.Size(824, 0);
            this.listingControlsPanel.Name = "listingControlsPanel";
            this.listingControlsPanel.Size = new System.Drawing.Size(824, 34);
            this.listingControlsPanel.TabIndex = 8;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Controls.Add(this.lastPageButton);
            this.panel3.Controls.Add(this.nextPageButton);
            this.panel3.Controls.Add(this.currentPagelabel);
            this.panel3.Controls.Add(this.previousPageButton);
            this.panel3.Controls.Add(this.firstPageButton);
            this.panel3.Location = new System.Drawing.Point(407, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(258, 34);
            this.panel3.TabIndex = 13;
            // 
            // lastPageButton
            // 
            this.lastPageButton.Location = new System.Drawing.Point(224, 6);
            this.lastPageButton.Name = "lastPageButton";
            this.lastPageButton.Size = new System.Drawing.Size(23, 23);
            this.lastPageButton.TabIndex = 16;
            this.lastPageButton.Text = ">|";
            this.lastPageButton.UseVisualStyleBackColor = true;
            this.lastPageButton.Click += new System.EventHandler(this.LastPageButtonClick);
            // 
            // nextPageButton
            // 
            this.nextPageButton.Location = new System.Drawing.Point(195, 6);
            this.nextPageButton.Name = "nextPageButton";
            this.nextPageButton.Size = new System.Drawing.Size(23, 23);
            this.nextPageButton.TabIndex = 15;
            this.nextPageButton.Text = ">";
            this.nextPageButton.UseVisualStyleBackColor = true;
            this.nextPageButton.Click += new System.EventHandler(this.NextPageButtonClick);
            // 
            // currentPagelabel
            // 
            this.currentPagelabel.Location = new System.Drawing.Point(69, 9);
            this.currentPagelabel.Name = "currentPagelabel";
            this.currentPagelabel.Size = new System.Drawing.Size(120, 16);
            this.currentPagelabel.TabIndex = 14;
            this.currentPagelabel.Text = "0/0";
            this.currentPagelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // previousPageButton
            // 
            this.previousPageButton.Location = new System.Drawing.Point(40, 6);
            this.previousPageButton.Name = "previousPageButton";
            this.previousPageButton.Size = new System.Drawing.Size(23, 23);
            this.previousPageButton.TabIndex = 13;
            this.previousPageButton.Text = "<";
            this.previousPageButton.UseVisualStyleBackColor = true;
            this.previousPageButton.Click += new System.EventHandler(this.PreviousPageButtonClick);
            // 
            // firstPageButton
            // 
            this.firstPageButton.Location = new System.Drawing.Point(11, 6);
            this.firstPageButton.Name = "firstPageButton";
            this.firstPageButton.Size = new System.Drawing.Size(23, 23);
            this.firstPageButton.TabIndex = 12;
            this.firstPageButton.Text = "|<";
            this.firstPageButton.UseVisualStyleBackColor = true;
            this.firstPageButton.Click += new System.EventHandler(this.FirstPageButtonClick);
            // 
            // resultsPerPage
            // 
            this.resultsPerPage.FormattingEnabled = true;
            this.resultsPerPage.Items.AddRange(new object[] {
            "25",
            "100",
            "500",
            "1000",
            "2000"});
            this.resultsPerPage.Location = new System.Drawing.Point(338, 6);
            this.resultsPerPage.Name = "resultsPerPage";
            this.resultsPerPage.Size = new System.Drawing.Size(63, 21);
            this.resultsPerPage.TabIndex = 6;
            this.resultsPerPage.Text = "25";
            this.resultsPerPage.TextChanged += new System.EventHandler(this.ResultsPerPageTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Результатов на страницу:";
            // 
            // resultsNumLabel
            // 
            this.resultsNumLabel.AutoSize = true;
            this.resultsNumLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.resultsNumLabel.Location = new System.Drawing.Point(118, 11);
            this.resultsNumLabel.Name = "resultsNumLabel";
            this.resultsNumLabel.Size = new System.Drawing.Size(14, 13);
            this.resultsNumLabel.TabIndex = 2;
            this.resultsNumLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Всего результатов:";
            // 
            // exportButton
            // 
            this.exportButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.exportButton.Location = new System.Drawing.Point(694, 6);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(127, 23);
            this.exportButton.TabIndex = 0;
            this.exportButton.Text = "Экпорт результатов";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButtonClick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "csv";
            this.saveFileDialog.FileName = "BatchResults.csv";
            this.saveFileDialog.Filter = "CSV files|*.csv";
            this.saveFileDialog.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 694);
            this.Controls.Add(this.listingControlsPanel);
            this.Controls.Add(this.resultsDataGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(1100, 38);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ФБС Утилита пакетного поиска";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
            this.listingControlsPanel.ResumeLayout(false);
            this.listingControlsPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox founder;
        private System.Windows.Forms.CheckBox ogrn;
        private System.Windows.Forms.CheckBox inn;
        private System.Windows.Forms.CheckBox shortOrgName;
        private System.Windows.Forms.CheckBox fullOrgName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fileNameInput;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.DataGridView resultsDataGrid;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Panel listingControlsPanel;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Label resultsNumLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox resultsPerPage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button lastPageButton;
        private System.Windows.Forms.Button nextPageButton;
        private System.Windows.Forms.Label currentPagelabel;
        private System.Windows.Forms.Button previousPageButton;
        private System.Windows.Forms.Button firstPageButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InnColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn OgrnColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FounderColumn;

    }
}

