using GVUZ.Util.UI.Importing;
using GVUZ.Util.UI.Parsing;

namespace GVUZ.Util.UI
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.parseView1 = new ParseView();
            this.orderImportView1 = new OrderImportView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(562, 381);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.parseView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(554, 355);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Разбор заявлений из пакетов";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.orderImportView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(554, 355);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Включение разобранных заявлений в приказы";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // parseView1
            // 
            this.parseView1.ContentVisible = false;
            this.parseView1.CurrentProgress = 0;
            this.parseView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parseView1.Location = new System.Drawing.Point(3, 3);
            this.parseView1.MaxProgress = 100;
            this.parseView1.Name = "parseView1";
            this.parseView1.ParseStatusText = "lbProgress";
            this.parseView1.SetupButtonEnabled = true;
            this.parseView1.Size = new System.Drawing.Size(548, 349);
            this.parseView1.StartButtonEnabled = true;
            this.parseView1.StopButtonEnabled = true;
            this.parseView1.TabIndex = 0;
            // 
            // orderImportView1
            // 
            this.orderImportView1.AbortButtonEnabled = true;
            this.orderImportView1.CurrentProgress = 0;
            this.orderImportView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderImportView1.ImportStatusText = "label1";
            this.orderImportView1.Location = new System.Drawing.Point(3, 3);
            this.orderImportView1.MaxProgress = 100;
            this.orderImportView1.Name = "orderImportView1";
            this.orderImportView1.RunButtonEnabled = true;
            this.orderImportView1.Size = new System.Drawing.Size(548, 349);
            this.orderImportView1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 381);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Работа с данными ФИС";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ParseView parseView1;
        private OrderImportView orderImportView1;
    }
}

