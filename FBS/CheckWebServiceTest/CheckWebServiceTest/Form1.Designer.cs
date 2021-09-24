namespace CheckWebService
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
            this.components = new System.ComponentModel.Container();
            this.tb_SingleQuerySample = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_Url = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_BatchQuerySample = new System.Windows.Forms.TextBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBatchId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_SingleCheck = new System.Windows.Forms.Button();
            this.btn_LoadSingleExample = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_GetBatchResult = new System.Windows.Forms.Button();
            this.btn_StartBatchCheck = new System.Windows.Forms.Button();
            this.btn_LoadBatchExample = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.tb_Login = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_Client = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_SingleQuerySample
            // 
            this.tb_SingleQuerySample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_SingleQuerySample.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_SingleQuerySample.Location = new System.Drawing.Point(23, 34);
            this.tb_SingleQuerySample.Multiline = true;
            this.tb_SingleQuerySample.Name = "tb_SingleQuerySample";
            this.tb_SingleQuerySample.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_SingleQuerySample.Size = new System.Drawing.Size(708, 280);
            this.tb_SingleQuerySample.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "XML-документ на единичную проверку:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Адрес веб-сервиса:";
            // 
            // tb_Url
            // 
            this.tb_Url.Location = new System.Drawing.Point(125, 6);
            this.tb_Url.Name = "tb_Url";
            this.tb_Url.Size = new System.Drawing.Size(213, 20);
            this.tb_Url.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "XML-документ на пакетную проверку:";
            // 
            // tb_BatchQuerySample
            // 
            this.tb_BatchQuerySample.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_BatchQuerySample.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_BatchQuerySample.Location = new System.Drawing.Point(23, 34);
            this.tb_BatchQuerySample.Multiline = true;
            this.tb_BatchQuerySample.Name = "tb_BatchQuerySample";
            this.tb_BatchQuerySample.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_BatchQuerySample.Size = new System.Drawing.Size(396, 280);
            this.tb_BatchQuerySample.TabIndex = 7;
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbResult.Location = new System.Drawing.Point(12, 474);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResult.Size = new System.Drawing.Size(764, 129);
            this.tbResult.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 458);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Результат:";
            // 
            // tbBatchId
            // 
            this.tbBatchId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBatchId.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbBatchId.Location = new System.Drawing.Point(425, 34);
            this.tbBatchId.Multiline = true;
            this.tbBatchId.Name = "tbBatchId";
            this.tbBatchId.Size = new System.Drawing.Size(304, 280);
            this.tbBatchId.TabIndex = 15;
            this.tbBatchId.Text = "<items>\r\n     <batchId></batchId>\r\n</items>\r\n";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(422, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Идентификатор пакета:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "CSV|*.csv";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(15, 50);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(761, 393);
            this.tabControl1.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_SingleCheck);
            this.tabPage1.Controls.Add(this.btn_LoadSingleExample);
            this.tabPage1.Controls.Add(this.tb_SingleQuerySample);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(753, 367);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Единичные проверки";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_SingleCheck
            // 
            this.btn_SingleCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_SingleCheck.Location = new System.Drawing.Point(176, 320);
            this.btn_SingleCheck.Name = "btn_SingleCheck";
            this.btn_SingleCheck.Size = new System.Drawing.Size(160, 23);
            this.btn_SingleCheck.TabIndex = 16;
            this.btn_SingleCheck.Text = "Выполнить проверку";
            this.btn_SingleCheck.UseVisualStyleBackColor = true;
            this.btn_SingleCheck.Click += new System.EventHandler(this.btn_SingleCheck_Click);
            // 
            // btn_LoadSingleExample
            // 
            this.btn_LoadSingleExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_LoadSingleExample.Location = new System.Drawing.Point(23, 320);
            this.btn_LoadSingleExample.Name = "btn_LoadSingleExample";
            this.btn_LoadSingleExample.Size = new System.Drawing.Size(147, 23);
            this.btn_LoadSingleExample.TabIndex = 15;
            this.btn_LoadSingleExample.Text = "Загрузить пример";
            this.btn_LoadSingleExample.UseVisualStyleBackColor = true;
            this.btn_LoadSingleExample.Click += new System.EventHandler(this.btn_LoadSingleExample_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_GetBatchResult);
            this.tabPage2.Controls.Add(this.btn_StartBatchCheck);
            this.tabPage2.Controls.Add(this.btn_LoadBatchExample);
            this.tabPage2.Controls.Add(this.tb_BatchQuerySample);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.tbBatchId);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(753, 367);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Пакетные проверки (ручной ввод)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_GetBatchResult
            // 
            this.btn_GetBatchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_GetBatchResult.Location = new System.Drawing.Point(425, 320);
            this.btn_GetBatchResult.Name = "btn_GetBatchResult";
            this.btn_GetBatchResult.Size = new System.Drawing.Size(197, 23);
            this.btn_GetBatchResult.TabIndex = 20;
            this.btn_GetBatchResult.Text = "Получить результат проверки";
            this.btn_GetBatchResult.UseVisualStyleBackColor = true;
            this.btn_GetBatchResult.Click += new System.EventHandler(this.btn_GetBatchResult_Click);
            // 
            // btn_StartBatchCheck
            // 
            this.btn_StartBatchCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_StartBatchCheck.Location = new System.Drawing.Point(176, 320);
            this.btn_StartBatchCheck.Name = "btn_StartBatchCheck";
            this.btn_StartBatchCheck.Size = new System.Drawing.Size(160, 23);
            this.btn_StartBatchCheck.TabIndex = 19;
            this.btn_StartBatchCheck.Text = "Начать проверку";
            this.btn_StartBatchCheck.UseVisualStyleBackColor = true;
            this.btn_StartBatchCheck.Click += new System.EventHandler(this.btn_StartBatchCheck_Click);
            // 
            // btn_LoadBatchExample
            // 
            this.btn_LoadBatchExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_LoadBatchExample.Location = new System.Drawing.Point(23, 320);
            this.btn_LoadBatchExample.Name = "btn_LoadBatchExample";
            this.btn_LoadBatchExample.Size = new System.Drawing.Size(147, 23);
            this.btn_LoadBatchExample.TabIndex = 18;
            this.btn_LoadBatchExample.Text = "Загрузить пример";
            this.btn_LoadBatchExample.UseVisualStyleBackColor = true;
            this.btn_LoadBatchExample.Click += new System.EventHandler(this.btn_LoadBatchExample_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(344, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "Логин:";
            // 
            // tb_Login
            // 
            this.tb_Login.Location = new System.Drawing.Point(391, 6);
            this.tb_Login.Name = "tb_Login";
            this.tb_Login.Size = new System.Drawing.Size(102, 20);
            this.tb_Login.TabIndex = 27;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(499, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "Пароль:";
            // 
            // tb_Password
            // 
            this.tb_Password.Location = new System.Drawing.Point(553, 6);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.Size = new System.Drawing.Size(56, 20);
            this.tb_Password.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(615, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Клиент:";
            // 
            // tb_Client
            // 
            this.tb_Client.Location = new System.Drawing.Point(667, 6);
            this.tb_Client.Name = "tb_Client";
            this.tb_Client.Size = new System.Drawing.Size(102, 20);
            this.tb_Client.TabIndex = 31;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 615);
            this.Controls.Add(this.tb_Client);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_Password);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tb_Login);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.tb_Url);
            this.Controls.Add(this.label2);
            this.MinimumSize = new System.Drawing.Size(804, 653);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Проверка работы веб-сервиса проверок";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_SingleQuerySample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_Url;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_BatchQuerySample;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBatchId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tb_Login;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_SingleCheck;
        private System.Windows.Forms.Button btn_LoadSingleExample;
        private System.Windows.Forms.Button btn_GetBatchResult;
        private System.Windows.Forms.Button btn_StartBatchCheck;
        private System.Windows.Forms.Button btn_LoadBatchExample;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_Client;
    }
}

