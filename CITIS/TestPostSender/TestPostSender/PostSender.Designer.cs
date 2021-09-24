namespace TestPostSender
{
    partial class PostSender
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.Send = new System.Windows.Forms.Button();
            this.URL = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.InputName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OpenFile = new System.Windows.Forms.Button();
            this.FileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.PathToFile = new System.Windows.Forms.TextBox();
            this.OutText = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(12, 155);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(115, 52);
            this.Send.TabIndex = 0;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // URL
            // 
            this.URL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.URL.Location = new System.Drawing.Point(42, 13);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(496, 20);
            this.URL.TabIndex = 1;
            this.URL.Text = "http://10.0.3.1:8043/XML/UploadXML";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.URL);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(545, 49);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL:";
            // 
            // InputName
            // 
            this.InputName.Location = new System.Drawing.Point(90, 80);
            this.InputName.Name = "InputName";
            this.InputName.Size = new System.Drawing.Size(64, 20);
            this.InputName.TabIndex = 3;
            this.InputName.Text = "inputXML";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ParamName";
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(337, 109);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(87, 23);
            this.OpenFile.TabIndex = 5;
            this.OpenFile.Text = "OpenFile";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Path to file";
            // 
            // PathToFile
            // 
            this.PathToFile.Location = new System.Drawing.Point(90, 111);
            this.PathToFile.Name = "PathToFile";
            this.PathToFile.Size = new System.Drawing.Size(241, 20);
            this.PathToFile.TabIndex = 7;
            // 
            // OutText
            // 
            this.OutText.Location = new System.Drawing.Point(133, 155);
            this.OutText.Name = "OutText";
            this.OutText.Size = new System.Drawing.Size(417, 53);
            this.OutText.TabIndex = 8;
            this.OutText.Text = "";
            // 
            // PostSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 220);
            this.Controls.Add(this.OutText);
            this.Controls.Add(this.PathToFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OpenFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InputName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Send);
            this.Name = "PostSender";
            this.Text = "PostSender";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.TextBox URL;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox InputName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button OpenFile;
        private System.Windows.Forms.OpenFileDialog FileDialog;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PathToFile;
        private System.Windows.Forms.RichTextBox OutText;
    }
}

