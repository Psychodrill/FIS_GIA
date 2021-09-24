namespace FBSLoader
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.txtBxDest = new System.Windows.Forms.TextBox();
            this.txtBxEsrp = new System.Windows.Forms.TextBox();
            this.txtBxSource2011 = new System.Windows.Forms.TextBox();
            this.txtBxSource2010 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(292, 278);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(236, 61);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Загрузить данные";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cmbYear);
            this.panel1.Controls.Add(this.txtBxDest);
            this.panel1.Controls.Add(this.txtBxEsrp);
            this.panel1.Controls.Add(this.txtBxSource2011);
            this.panel1.Controls.Add(this.txtBxSource2010);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 246);
            this.panel1.TabIndex = 14;
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Items.AddRange(new object[] {
            "2010",
            "2011"});
            this.cmbYear.Location = new System.Drawing.Point(246, 176);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(162, 21);
            this.cmbYear.TabIndex = 28;
            // 
            // txtBxDest
            // 
            this.txtBxDest.Location = new System.Drawing.Point(246, 140);
            this.txtBxDest.Name = "txtBxDest";
            this.txtBxDest.ReadOnly = true;
            this.txtBxDest.Size = new System.Drawing.Size(546, 20);
            this.txtBxDest.TabIndex = 27;
            // 
            // txtBxEsrp
            // 
            this.txtBxEsrp.Location = new System.Drawing.Point(246, 104);
            this.txtBxEsrp.Name = "txtBxEsrp";
            this.txtBxEsrp.ReadOnly = true;
            this.txtBxEsrp.Size = new System.Drawing.Size(546, 20);
            this.txtBxEsrp.TabIndex = 26;
            // 
            // txtBxSource2011
            // 
            this.txtBxSource2011.Location = new System.Drawing.Point(246, 68);
            this.txtBxSource2011.Name = "txtBxSource2011";
            this.txtBxSource2011.ReadOnly = true;
            this.txtBxSource2011.Size = new System.Drawing.Size(546, 20);
            this.txtBxSource2011.TabIndex = 25;
            // 
            // txtBxSource2010
            // 
            this.txtBxSource2010.Location = new System.Drawing.Point(246, 32);
            this.txtBxSource2010.Name = "txtBxSource2010";
            this.txtBxSource2010.ReadOnly = true;
            this.txtBxSource2010.Size = new System.Drawing.Size(546, 20);
            this.txtBxSource2010.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Строка подключения к ЕСРП:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Строка подключения к БД отчетности:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Строка подключения к БД 2011:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Строка подключения к БД 2010:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Версия БД для загрузки:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 382);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Загрузчик ФБС";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.TextBox txtBxDest;
        private System.Windows.Forms.TextBox txtBxEsrp;
        private System.Windows.Forms.TextBox txtBxSource2011;
        private System.Windows.Forms.TextBox txtBxSource2010;
    }
}

