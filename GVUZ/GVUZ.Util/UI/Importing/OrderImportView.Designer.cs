namespace GVUZ.Util.UI.Importing
{
    partial class OrderImportView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tsImport = new System.Windows.Forms.ToolStrip();
            this.tsiImportRun = new System.Windows.Forms.ToolStripButton();
            this.tsiImportAbort = new System.Windows.Forms.ToolStripButton();
            this.tsiImportLogSave = new System.Windows.Forms.ToolStripButton();
            this.pnlImport = new System.Windows.Forms.Panel();
            this.tbImportLog = new System.Windows.Forms.TextBox();
            this.pbImport = new System.Windows.Forms.ProgressBar();
            this.lbImportTime = new System.Windows.Forms.Label();
            this.lbImportStatus = new System.Windows.Forms.Label();
            this.tmImport = new System.Windows.Forms.Timer(this.components);
            this.saveLogFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tsImport.SuspendLayout();
            this.pnlImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsImport
            // 
            this.tsImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiImportRun,
            this.tsiImportAbort,
            this.tsiImportLogSave});
            this.tsImport.Location = new System.Drawing.Point(0, 0);
            this.tsImport.Name = "tsImport";
            this.tsImport.Size = new System.Drawing.Size(546, 25);
            this.tsImport.TabIndex = 1;
            this.tsImport.Text = "toolStrip1";
            // 
            // tsiImportRun
            // 
            this.tsiImportRun.Image = global::GVUZ.Util.Properties.Resources.FormRunHS;
            this.tsiImportRun.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsiImportRun.Name = "tsiImportRun";
            this.tsiImportRun.Size = new System.Drawing.Size(82, 22);
            this.tsiImportRun.Text = "Запустить";
            this.tsiImportRun.Click += new System.EventHandler(this.tsiImportRun_Click);
            // 
            // tsiImportAbort
            // 
            this.tsiImportAbort.Image = global::GVUZ.Util.Properties.Resources.StopHS;
            this.tsiImportAbort.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsiImportAbort.Name = "tsiImportAbort";
            this.tsiImportAbort.Size = new System.Drawing.Size(79, 22);
            this.tsiImportAbort.Text = "Прервать";
            this.tsiImportAbort.Click += new System.EventHandler(this.tsiImportAbort_Click);
            // 
            // tsiImportLogSave
            // 
            this.tsiImportLogSave.Image = global::GVUZ.Util.Properties.Resources.saveHS;
            this.tsiImportLogSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiImportLogSave.Name = "tsiImportLogSave";
            this.tsiImportLogSave.Size = new System.Drawing.Size(171, 22);
            this.tsiImportLogSave.Text = "Сохранить журнал в файл";
            this.tsiImportLogSave.Click += new System.EventHandler(this.tsiImportLogSave_Click);
            // 
            // pnlImport
            // 
            this.pnlImport.Controls.Add(this.tbImportLog);
            this.pnlImport.Controls.Add(this.pbImport);
            this.pnlImport.Controls.Add(this.lbImportTime);
            this.pnlImport.Controls.Add(this.lbImportStatus);
            this.pnlImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImport.Location = new System.Drawing.Point(0, 25);
            this.pnlImport.Name = "pnlImport";
            this.pnlImport.Size = new System.Drawing.Size(546, 295);
            this.pnlImport.TabIndex = 2;
            // 
            // tbImportLog
            // 
            this.tbImportLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImportLog.Location = new System.Drawing.Point(8, 84);
            this.tbImportLog.Multiline = true;
            this.tbImportLog.Name = "tbImportLog";
            this.tbImportLog.ReadOnly = true;
            this.tbImportLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbImportLog.Size = new System.Drawing.Size(533, 197);
            this.tbImportLog.TabIndex = 3;
            this.tbImportLog.WordWrap = false;
            // 
            // pbImport
            // 
            this.pbImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbImport.Location = new System.Drawing.Point(6, 29);
            this.pbImport.Name = "pbImport";
            this.pbImport.Size = new System.Drawing.Size(535, 23);
            this.pbImport.TabIndex = 2;
            // 
            // lbImportTime
            // 
            this.lbImportTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbImportTime.Location = new System.Drawing.Point(441, 55);
            this.lbImportTime.Name = "lbImportTime";
            this.lbImportTime.Size = new System.Drawing.Size(100, 23);
            this.lbImportTime.TabIndex = 1;
            this.lbImportTime.Text = "label2";
            this.lbImportTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbImportStatus
            // 
            this.lbImportStatus.AutoSize = true;
            this.lbImportStatus.Location = new System.Drawing.Point(5, 12);
            this.lbImportStatus.Name = "lbImportStatus";
            this.lbImportStatus.Size = new System.Drawing.Size(35, 13);
            this.lbImportStatus.TabIndex = 0;
            this.lbImportStatus.Text = "label1";
            // 
            // tmImport
            // 
            this.tmImport.Tick += new System.EventHandler(this.tmImport_Tick);
            // 
            // saveLogFileDialog
            // 
            this.saveLogFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            this.saveLogFileDialog.Title = "Сохранение файла журнала";
            // 
            // OrderImportView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlImport);
            this.Controls.Add(this.tsImport);
            this.Name = "OrderImportView";
            this.Size = new System.Drawing.Size(546, 320);
            this.tsImport.ResumeLayout(false);
            this.tsImport.PerformLayout();
            this.pnlImport.ResumeLayout(false);
            this.pnlImport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsImport;
        private System.Windows.Forms.ToolStripButton tsiImportRun;
        private System.Windows.Forms.ToolStripButton tsiImportAbort;
        private System.Windows.Forms.ToolStripButton tsiImportLogSave;
        private System.Windows.Forms.Panel pnlImport;
        private System.Windows.Forms.TextBox tbImportLog;
        private System.Windows.Forms.ProgressBar pbImport;
        private System.Windows.Forms.Label lbImportTime;
        private System.Windows.Forms.Label lbImportStatus;
        private System.Windows.Forms.Timer tmImport;
        private System.Windows.Forms.SaveFileDialog saveLogFileDialog;
    }
}
