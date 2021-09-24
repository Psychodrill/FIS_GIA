namespace DeliveryTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.TBFrom = new System.Windows.Forms.TextBox();
            this.TBTo = new System.Windows.Forms.TextBox();
            this.TBHost = new System.Windows.Forms.TextBox();
            this.TBPort = new System.Windows.Forms.TextBox();
            this.TBLogin = new System.Windows.Forms.TextBox();
            this.TBPassword = new System.Windows.Forms.TextBox();
            this.ChUseSSL = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(101, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Test!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TBFrom
            // 
            this.TBFrom.Location = new System.Drawing.Point(61, 12);
            this.TBFrom.Name = "TBFrom";
            this.TBFrom.Size = new System.Drawing.Size(211, 20);
            this.TBFrom.TabIndex = 1;
            this.TBFrom.Text = "info@fbsege.ru";
            // 
            // TBTo
            // 
            this.TBTo.Location = new System.Drawing.Point(61, 38);
            this.TBTo.Name = "TBTo";
            this.TBTo.Size = new System.Drawing.Size(211, 20);
            this.TBTo.TabIndex = 2;
            this.TBTo.Text = "kyusupov@armd.ru";
            // 
            // TBHost
            // 
            this.TBHost.Location = new System.Drawing.Point(61, 64);
            this.TBHost.Name = "TBHost";
            this.TBHost.Size = new System.Drawing.Size(211, 20);
            this.TBHost.TabIndex = 3;
            this.TBHost.Text = "localhost";
            // 
            // TBPort
            // 
            this.TBPort.Location = new System.Drawing.Point(61, 90);
            this.TBPort.Name = "TBPort";
            this.TBPort.Size = new System.Drawing.Size(211, 20);
            this.TBPort.TabIndex = 4;
            this.TBPort.Text = "25";
            // 
            // TBLogin
            // 
            this.TBLogin.Location = new System.Drawing.Point(61, 116);
            this.TBLogin.Name = "TBLogin";
            this.TBLogin.Size = new System.Drawing.Size(211, 20);
            this.TBLogin.TabIndex = 5;
            // 
            // TBPassword
            // 
            this.TBPassword.Location = new System.Drawing.Point(61, 142);
            this.TBPassword.Name = "TBPassword";
            this.TBPassword.Size = new System.Drawing.Size(211, 20);
            this.TBPassword.TabIndex = 6;
            // 
            // ChUseSSL
            // 
            this.ChUseSSL.AutoSize = true;
            this.ChUseSSL.Location = new System.Drawing.Point(200, 168);
            this.ChUseSSL.Name = "ChUseSSL";
            this.ChUseSSL.Size = new System.Drawing.Size(72, 17);
            this.ChUseSSL.TabIndex = 7;
            this.ChUseSSL.Text = "Исп. SSL";
            this.ChUseSSL.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "От";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Куда";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Адрес";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Порт";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Логин";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Пароль";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ChUseSSL);
            this.Controls.Add(this.TBPassword);
            this.Controls.Add(this.TBLogin);
            this.Controls.Add(this.TBPort);
            this.Controls.Add(this.TBHost);
            this.Controls.Add(this.TBTo);
            this.Controls.Add(this.TBFrom);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox TBFrom;
        private System.Windows.Forms.TextBox TBTo;
        private System.Windows.Forms.TextBox TBHost;
        private System.Windows.Forms.TextBox TBPort;
        private System.Windows.Forms.TextBox TBLogin;
        private System.Windows.Forms.TextBox TBPassword;
        private System.Windows.Forms.CheckBox ChUseSSL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

