namespace CompositionExportServiceClient
{
    partial class Form1
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
            this.btnTestCompositionInfo = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.btnLoadCompositionInfo = new System.Windows.Forms.Button();
            this.chk2015 = new System.Windows.Forms.CheckBox();
            this.chk2016 = new System.Windows.Forms.CheckBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnSaveCompositionInfo = new System.Windows.Forms.Button();
            this.btnCreateCompositionInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestCompositionInfo
            // 
            this.btnTestCompositionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestCompositionInfo.Location = new System.Drawing.Point(519, 65);
            this.btnTestCompositionInfo.Name = "btnTestCompositionInfo";
            this.btnTestCompositionInfo.Size = new System.Drawing.Size(137, 23);
            this.btnTestCompositionInfo.TabIndex = 0;
            this.btnTestCompositionInfo.Text = "Test GetCompositions";
            this.btnTestCompositionInfo.UseVisualStyleBackColor = true;
            this.btnTestCompositionInfo.Click += new System.EventHandler(this.TestCompositions_btnclk);
            // 
            // txtPath
            // 
            this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPath.Location = new System.Drawing.Point(12, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(644, 20);
            this.txtPath.TabIndex = 1;
            this.txtPath.Text = "path";
            // 
            // txtLastName
            // 
            this.txtLastName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastName.Location = new System.Drawing.Point(13, 39);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(643, 20);
            this.txtLastName.TabIndex = 2;
            this.txtLastName.Text = "LastName";
            // 
            // btnLoadCompositionInfo
            // 
            this.btnLoadCompositionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadCompositionInfo.Location = new System.Drawing.Point(519, 159);
            this.btnLoadCompositionInfo.Name = "btnLoadCompositionInfo";
            this.btnLoadCompositionInfo.Size = new System.Drawing.Size(137, 23);
            this.btnLoadCompositionInfo.TabIndex = 3;
            this.btnLoadCompositionInfo.Text = "Load CompositionInfo";
            this.btnLoadCompositionInfo.UseVisualStyleBackColor = true;
            this.btnLoadCompositionInfo.Click += new System.EventHandler(this.LoadCompositions_btnclk);
            // 
            // chk2015
            // 
            this.chk2015.AutoSize = true;
            this.chk2015.Checked = true;
            this.chk2015.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk2015.Location = new System.Drawing.Point(12, 130);
            this.chk2015.Name = "chk2015";
            this.chk2015.Size = new System.Drawing.Size(50, 17);
            this.chk2015.TabIndex = 4;
            this.chk2015.Text = "2015";
            this.chk2015.UseVisualStyleBackColor = true;
            // 
            // chk2016
            // 
            this.chk2016.AutoSize = true;
            this.chk2016.Checked = true;
            this.chk2016.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk2016.Location = new System.Drawing.Point(12, 154);
            this.chk2016.Name = "chk2016";
            this.chk2016.Size = new System.Drawing.Size(50, 17);
            this.chk2016.TabIndex = 5;
            this.chk2016.Text = "2016";
            this.chk2016.UseVisualStyleBackColor = true;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(12, 188);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(644, 251);
            this.txtResult.TabIndex = 6;
            // 
            // btnSaveCompositionInfo
            // 
            this.btnSaveCompositionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveCompositionInfo.Location = new System.Drawing.Point(519, 130);
            this.btnSaveCompositionInfo.Name = "btnSaveCompositionInfo";
            this.btnSaveCompositionInfo.Size = new System.Drawing.Size(137, 23);
            this.btnSaveCompositionInfo.TabIndex = 7;
            this.btnSaveCompositionInfo.Text = "Save CompositionInfo";
            this.btnSaveCompositionInfo.UseVisualStyleBackColor = true;
            this.btnSaveCompositionInfo.Click += new System.EventHandler(this.SaveCompositions_btnclk);
            // 
            // btnCreateCompositionInfo
            // 
            this.btnCreateCompositionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateCompositionInfo.Location = new System.Drawing.Point(283, 126);
            this.btnCreateCompositionInfo.Name = "btnCreateCompositionInfo";
            this.btnCreateCompositionInfo.Size = new System.Drawing.Size(137, 23);
            this.btnCreateCompositionInfo.TabIndex = 8;
            this.btnCreateCompositionInfo.Text = "Create CompositionInfo";
            this.btnCreateCompositionInfo.UseVisualStyleBackColor = true;
            this.btnCreateCompositionInfo.Click += new System.EventHandler(this.CreateComposition_btnclk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 451);
            this.Controls.Add(this.btnCreateCompositionInfo);
            this.Controls.Add(this.btnSaveCompositionInfo);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.chk2016);
            this.Controls.Add(this.chk2015);
            this.Controls.Add(this.btnLoadCompositionInfo);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnTestCompositionInfo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTestCompositionInfo;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Button btnLoadCompositionInfo;
        private System.Windows.Forms.CheckBox chk2015;
        private System.Windows.Forms.CheckBox chk2016;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnSaveCompositionInfo;
        private System.Windows.Forms.Button btnCreateCompositionInfo;
    }
}

