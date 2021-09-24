namespace ImportService2016Test
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
            this.cmdImport = new System.Windows.Forms.Button();
            this.bntPackImportFixedID = new System.Windows.Forms.CheckBox();
            this.txtPackageID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkBulksNotDeleted = new System.Windows.Forms.CheckBox();
            this.cmdClearBulk = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.RichTextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdImport
            // 
            this.cmdImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdImport.Location = new System.Drawing.Point(474, 91);
            this.cmdImport.Name = "cmdImport";
            this.cmdImport.Size = new System.Drawing.Size(97, 23);
            this.cmdImport.TabIndex = 0;
            this.cmdImport.Text = "Импорт пакета";
            this.cmdImport.UseVisualStyleBackColor = true;
            this.cmdImport.Click += new System.EventHandler(this.ImportPackBtnClk);
            // 
            // bntPackImportFixedID
            // 
            this.bntPackImportFixedID.Appearance = System.Windows.Forms.Appearance.Button;
            this.bntPackImportFixedID.AutoSize = true;
            this.bntPackImportFixedID.Location = new System.Drawing.Point(12, 84);
            this.bntPackImportFixedID.Name = "bntPackImportFixedID";
            this.bntPackImportFixedID.Size = new System.Drawing.Size(133, 23);
            this.bntPackImportFixedID.TabIndex = 2;
            this.bntPackImportFixedID.Text = "Импорт пакета 584881";
            this.bntPackImportFixedID.UseVisualStyleBackColor = true;
            this.bntPackImportFixedID.Visible = false;
            this.bntPackImportFixedID.CheckedChanged += new System.EventHandler(this.OnImportPackFixedIDbtnclk);
            // 
            // txtPackageID
            // 
            this.txtPackageID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackageID.Location = new System.Drawing.Point(181, 12);
            this.txtPackageID.Name = "txtPackageID";
            this.txtPackageID.Size = new System.Drawing.Size(390, 20);
            this.txtPackageID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 31);
            this.label1.TabIndex = 4;
            this.label1.Text = "Номер пакета или пусто (выберет первый из списка)";
            // 
            // chkBulksNotDeleted
            // 
            this.chkBulksNotDeleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkBulksNotDeleted.Location = new System.Drawing.Point(15, 54);
            this.chkBulksNotDeleted.Name = "chkBulksNotDeleted";
            this.chkBulksNotDeleted.Size = new System.Drawing.Size(178, 24);
            this.chkBulksNotDeleted.TabIndex = 5;
            this.chkBulksNotDeleted.Text = "Не очищать балк-таблицы";
            this.chkBulksNotDeleted.UseVisualStyleBackColor = true;
            // 
            // cmdClearBulk
            // 
            this.cmdClearBulk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClearBulk.Location = new System.Drawing.Point(474, 120);
            this.cmdClearBulk.Name = "cmdClearBulk";
            this.cmdClearBulk.Size = new System.Drawing.Size(97, 23);
            this.cmdClearBulk.TabIndex = 6;
            this.cmdClearBulk.Text = "Очистить bulk";
            this.cmdClearBulk.UseVisualStyleBackColor = true;
            this.cmdClearBulk.Click += new System.EventHandler(this.cmdClearBulk_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessages.Location = new System.Drawing.Point(12, 149);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Size = new System.Drawing.Size(559, 295);
            this.txtMessages.TabIndex = 8;
            this.txtMessages.Text = "";
            // 
            // btnCheck
            // 
            this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheck.Location = new System.Drawing.Point(15, 120);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(97, 23);
            this.btnCheck.TabIndex = 9;
            this.btnCheck.Text = "Проверка";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.CheckBtnClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 455);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.cmdClearBulk);
            this.Controls.Add(this.chkBulksNotDeleted);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPackageID);
            this.Controls.Add(this.bntPackImportFixedID);
            this.Controls.Add(this.cmdImport);
            this.MaximumSize = new System.Drawing.Size(600, 600);
            this.MinimumSize = new System.Drawing.Size(400, 230);
            this.Name = "Form1";
            this.Text = "Тест ВинСервиса";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdImport;
        private System.Windows.Forms.CheckBox bntPackImportFixedID;
        private System.Windows.Forms.TextBox txtPackageID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkBulksNotDeleted;
        private System.Windows.Forms.Button cmdClearBulk;
        private System.Windows.Forms.RichTextBox txtMessages;
        private System.Windows.Forms.Button btnCheck;
    }
}

