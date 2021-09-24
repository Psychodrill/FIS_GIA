namespace GVUZ.ImportServiceTest.Forms
{
    partial class ImportPackages
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tb_ThreadsCount = new System.Windows.Forms.NumericUpDown();
            this.lb_Threads = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Export = new System.Windows.Forms.Button();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lb_Results = new System.Windows.Forms.ListBox();
            this.tb_Path = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ThreadsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(684, 107);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Адрес сервиса";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(152, 77);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(520, 20);
            this.textBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "Результирующий путь:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Метод:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "/checkapplication (результат проверки заявлений в пакете)",
            "/checkapplication/single (проверка одного заявления)",
            "/delete (удаление)",
            "/delete/result (получение результата удаления)",
            "/dictionary (список справочников)",
            "/dictionarydetails (детали по справочнику)",
            "/import (импорт)",
            "/import/application/single (импорт одного заявления)",
            "/import/result (получение результата импорта)",
            "/validate (валидация)",
            "/institutioninfo (получение сведений по ОУ)",
            "/institutioninfo/partof (получение части сведений по ОУ)",
            "/test/checkapplication (тест проверки заявления)",
            "/test/delete (тест удаления)",
            "/test/dictionary (тест справочника)",
            "/test/dictionarydetails (тест деталей справочника)",
            "/test/import (тест импорта)"});
            this.comboBox1.Location = new System.Drawing.Point(152, 49);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(520, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Базовый адрес:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tb_ThreadsCount);
            this.groupBox2.Controls.Add(this.lb_Threads);
            this.groupBox2.Controls.Add(this.btn_Export);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.tb_Path);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(684, 159);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Параметры загрузки";
            // 
            // tb_ThreadsCount
            // 
            this.tb_ThreadsCount.Location = new System.Drawing.Point(118, 77);
            this.tb_ThreadsCount.Name = "tb_ThreadsCount";
            this.tb_ThreadsCount.Size = new System.Drawing.Size(76, 20);
            this.tb_ThreadsCount.TabIndex = 16;
            this.tb_ThreadsCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // lb_Threads
            // 
            this.lb_Threads.AutoSize = true;
            this.lb_Threads.Location = new System.Drawing.Point(24, 79);
            this.lb_Threads.Name = "lb_Threads";
            this.lb_Threads.Size = new System.Drawing.Size(88, 13);
            this.lb_Threads.TabIndex = 15;
            this.lb_Threads.Text = "Кол-во потоков:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(446, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Папка с результатами выгрузки:";
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(21, 113);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(122, 25);
            this.btn_Export.TabIndex = 13;
            this.btn_Export.Text = "СТАРТ";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // progress
            // 
            this.progress.Dock = System.Windows.Forms.DockStyle.Top;
            this.progress.Location = new System.Drawing.Point(0, 266);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(684, 23);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress.TabIndex = 14;
            // 
            // lb_Results
            // 
            this.lb_Results.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Results.FormattingEnabled = true;
            this.lb_Results.HorizontalScrollbar = true;
            this.lb_Results.Location = new System.Drawing.Point(0, 289);
            this.lb_Results.Name = "lb_Results";
            this.lb_Results.Size = new System.Drawing.Size(684, 273);
            this.lb_Results.TabIndex = 15;
            // 
            // tb_Path
            // 
            this.tb_Path.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.ImportServiceTest.Properties.Settings.Default, "tb_Path", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Path.Location = new System.Drawing.Point(21, 44);
            this.tb_Path.Name = "tb_Path";
            this.tb_Path.Size = new System.Drawing.Size(418, 20);
            this.tb_Path.TabIndex = 13;
            this.tb_Path.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.tb_Path;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.ImportServiceTest.Properties.Settings.Default, "BasePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox1.Location = new System.Drawing.Point(152, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(520, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.BasePath;
            this.textBox1.TextChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // ImportPackages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.lb_Results);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImportPackages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Загрузка пакетов";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_ThreadsCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown tb_ThreadsCount;
        private System.Windows.Forms.Label lb_Threads;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_Path;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListBox lb_Results;
    }
}