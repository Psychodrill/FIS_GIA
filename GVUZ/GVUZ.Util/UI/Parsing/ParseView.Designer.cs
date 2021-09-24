namespace GVUZ.Util.UI.Parsing
{
    partial class ParseView
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
            this.tsParse = new System.Windows.Forms.ToolStrip();
            this.tsiParseSetup = new System.Windows.Forms.ToolStripButton();
            this.tsiParseRun = new System.Windows.Forms.ToolStripButton();
            this.tsiParseAbort = new System.Windows.Forms.ToolStripButton();
            this.pnRunParse = new System.Windows.Forms.Panel();
            this.lbTime = new System.Windows.Forms.Label();
            this.lbProgress = new System.Windows.Forms.Label();
            this.pbParse = new System.Windows.Forms.ProgressBar();
            this.tmImport = new System.Windows.Forms.Timer(this.components);
            this.tsParse.SuspendLayout();
            this.pnRunParse.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsParse
            // 
            this.tsParse.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiParseSetup,
            this.tsiParseRun,
            this.tsiParseAbort});
            this.tsParse.Location = new System.Drawing.Point(0, 0);
            this.tsParse.Name = "tsParse";
            this.tsParse.Size = new System.Drawing.Size(629, 25);
            this.tsParse.TabIndex = 1;
            this.tsParse.Text = "toolStrip1";
            // 
            // tsiParseSetup
            // 
            this.tsiParseSetup.Image = global::GVUZ.Util.Properties.Resources.PropertiesHS;
            this.tsiParseSetup.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsiParseSetup.Name = "tsiParseSetup";
            this.tsiParseSetup.Size = new System.Drawing.Size(87, 22);
            this.tsiParseSetup.Text = "Настройки";
            this.tsiParseSetup.Click += new System.EventHandler(this.tsiParseSetup_Click);
            // 
            // tsiParseRun
            // 
            this.tsiParseRun.Image = global::GVUZ.Util.Properties.Resources.FormRunHS;
            this.tsiParseRun.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsiParseRun.Name = "tsiParseRun";
            this.tsiParseRun.Size = new System.Drawing.Size(82, 22);
            this.tsiParseRun.Text = "Запустить";
            this.tsiParseRun.Click += new System.EventHandler(this.tsiParseRun_Click);
            // 
            // tsiParseAbort
            // 
            this.tsiParseAbort.Image = global::GVUZ.Util.Properties.Resources.StopHS;
            this.tsiParseAbort.ImageTransparentColor = System.Drawing.Color.Black;
            this.tsiParseAbort.Name = "tsiParseAbort";
            this.tsiParseAbort.Size = new System.Drawing.Size(79, 22);
            this.tsiParseAbort.Text = "Прервать";
            this.tsiParseAbort.Click += new System.EventHandler(this.tsiParseAbort_Click);
            // 
            // pnRunParse
            // 
            this.pnRunParse.Controls.Add(this.lbTime);
            this.pnRunParse.Controls.Add(this.lbProgress);
            this.pnRunParse.Controls.Add(this.pbParse);
            this.pnRunParse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnRunParse.Location = new System.Drawing.Point(0, 25);
            this.pnRunParse.Name = "pnRunParse";
            this.pnRunParse.Size = new System.Drawing.Size(629, 333);
            this.pnRunParse.TabIndex = 2;
            // 
            // lbTime
            // 
            this.lbTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTime.Location = new System.Drawing.Point(475, 130);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(148, 13);
            this.lbTime.TabIndex = 6;
            this.lbTime.Text = "lbTime";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbProgress
            // 
            this.lbProgress.AutoEllipsis = true;
            this.lbProgress.AutoSize = true;
            this.lbProgress.Location = new System.Drawing.Point(5, 74);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(56, 13);
            this.lbProgress.TabIndex = 5;
            this.lbProgress.Text = "lbProgress";
            // 
            // pbParse
            // 
            this.pbParse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbParse.Location = new System.Drawing.Point(5, 93);
            this.pbParse.Name = "pbParse";
            this.pbParse.Size = new System.Drawing.Size(618, 34);
            this.pbParse.TabIndex = 4;
            // 
            // tmImport
            // 
            this.tmImport.Tick += new System.EventHandler(this.tmImport_Tick);
            // 
            // ParseView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnRunParse);
            this.Controls.Add(this.tsParse);
            this.Name = "ParseView";
            this.Size = new System.Drawing.Size(629, 358);
            this.tsParse.ResumeLayout(false);
            this.tsParse.PerformLayout();
            this.pnRunParse.ResumeLayout(false);
            this.pnRunParse.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsParse;
        private System.Windows.Forms.ToolStripButton tsiParseSetup;
        private System.Windows.Forms.ToolStripButton tsiParseRun;
        private System.Windows.Forms.ToolStripButton tsiParseAbort;
        private System.Windows.Forms.Panel pnRunParse;
        private System.Windows.Forms.Label lbTime;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.ProgressBar pbParse;
        private System.Windows.Forms.Timer tmImport;
    }
}
