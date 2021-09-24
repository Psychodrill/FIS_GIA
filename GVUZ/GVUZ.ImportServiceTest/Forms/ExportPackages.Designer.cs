namespace GVUZ.ImportServiceTest.Forms
{
    partial class ExportPackages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportPackages));
            this.panel1 = new System.Windows.Forms.Panel();
            this.finish = new System.Windows.Forms.PictureBox();
            this.lb_PackagesIds = new System.Windows.Forms.RichTextBox();
            this.btn_Export = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_Path = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_EsrpDbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sad = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lb_Results = new System.Windows.Forms.ListBox();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.finish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sad)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.finish);
            this.panel1.Controls.Add(this.lb_PackagesIds);
            this.panel1.Controls.Add(this.btn_Export);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.tb_Path);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.tb_EsrpDbName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.sad);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(684, 333);
            this.panel1.TabIndex = 0;
            // 
            // finish
            // 
            this.finish.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("finish.BackgroundImage")));
            this.finish.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.finish.Location = new System.Drawing.Point(421, 7);
            this.finish.Name = "finish";
            this.finish.Size = new System.Drawing.Size(48, 53);
            this.finish.TabIndex = 11;
            this.finish.TabStop = false;
            this.finish.Visible = false;
            // 
            // lb_PackagesIds
            // 
            this.lb_PackagesIds.Location = new System.Drawing.Point(16, 79);
            this.lb_PackagesIds.Name = "lb_PackagesIds";
            this.lb_PackagesIds.Size = new System.Drawing.Size(453, 160);
            this.lb_PackagesIds.TabIndex = 10;
            this.lb_PackagesIds.Text = "";
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(16, 284);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(122, 29);
            this.btn_Export.TabIndex = 7;
            this.btn_Export.Text = "СТАРТ";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(441, 254);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_Path
            // 
            this.tb_Path.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::GVUZ.ImportServiceTest.Properties.Settings.Default, "tb_Path", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tb_Path.Location = new System.Drawing.Point(16, 258);
            this.tb_Path.Name = "tb_Path";
            this.tb_Path.Size = new System.Drawing.Size(418, 20);
            this.tb_Path.TabIndex = 5;
            this.tb_Path.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.tb_Path;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(176, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Папка с результатами выгрузки:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(245, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Идентификаторы пакетов (1 в каждой строке):";
            // 
            // tb_EsrpDbName
            // 
            this.tb_EsrpDbName.Location = new System.Drawing.Point(131, 13);
            this.tb_EsrpDbName.Name = "tb_EsrpDbName";
            this.tb_EsrpDbName.Size = new System.Drawing.Size(120, 20);
            this.tb_EsrpDbName.TabIndex = 1;
            this.tb_EsrpDbName.Text = global::GVUZ.ImportServiceTest.Properties.Settings.Default.tb_EsrpDbName;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название БД ESRP:";
            // 
            // sad
            // 
            this.sad.BackgroundImage = global::GVUZ.ImportServiceTest.Properties.Resources.sad;
            this.sad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.sad.Location = new System.Drawing.Point(421, 7);
            this.sad.Name = "sad";
            this.sad.Size = new System.Drawing.Size(48, 53);
            this.sad.TabIndex = 12;
            this.sad.TabStop = false;
            this.sad.Visible = false;
            // 
            // lb_Results
            // 
            this.lb_Results.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Results.FormattingEnabled = true;
            this.lb_Results.Location = new System.Drawing.Point(0, 356);
            this.lb_Results.Name = "lb_Results";
            this.lb_Results.Size = new System.Drawing.Size(684, 206);
            this.lb_Results.TabIndex = 11;
            // 
            // progress
            // 
            this.progress.Dock = System.Windows.Forms.DockStyle.Top;
            this.progress.Location = new System.Drawing.Point(0, 333);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(684, 23);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress.TabIndex = 10;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(176, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Пробить всем доступ в ЕСРП";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ExportPackages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.lb_Results);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.panel1);
            this.Name = "ExportPackages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выгрузка пакетов из БД";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.finish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_EsrpDbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_Path;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox lb_PackagesIds;
        private System.Windows.Forms.PictureBox finish;
        private System.Windows.Forms.PictureBox sad;
        private System.Windows.Forms.ListBox lb_Results;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}