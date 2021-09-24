namespace FbsBatchSearchUtility
{
    partial class LoginForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.useSystemAuth = new System.Windows.Forms.CheckBox();
            this.saveCreditionals = new System.Windows.Forms.CheckBox();
            this.settingsLinkButton = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // loginTextBox
            // 
            this.loginTextBox.Location = new System.Drawing.Point(17, 31);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(228, 20);
            this.loginTextBox.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(17, 70);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(228, 20);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(173, 147);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 4;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButtonClick);
            // 
            // useSystemAuth
            // 
            this.useSystemAuth.AutoSize = true;
            this.useSystemAuth.Location = new System.Drawing.Point(17, 96);
            this.useSystemAuth.Name = "useSystemAuth";
            this.useSystemAuth.Size = new System.Drawing.Size(228, 17);
            this.useSystemAuth.TabIndex = 5;
            this.useSystemAuth.Text = "Использовать системную авторизацию";
            this.useSystemAuth.UseVisualStyleBackColor = true;
            this.useSystemAuth.CheckedChanged += new System.EventHandler(this.UseSystemAuthCheckedChanged);
            // 
            // saveCreditionals
            // 
            this.saveCreditionals.AutoSize = true;
            this.saveCreditionals.Location = new System.Drawing.Point(17, 119);
            this.saveCreditionals.Name = "saveCreditionals";
            this.saveCreditionals.Size = new System.Drawing.Size(174, 17);
            this.saveCreditionals.TabIndex = 6;
            this.saveCreditionals.Text = "Запомнить параметры входа";
            this.saveCreditionals.UseVisualStyleBackColor = true;
            // 
            // settingsLinkButton
            // 
            this.settingsLinkButton.AutoSize = true;
            this.settingsLinkButton.Location = new System.Drawing.Point(14, 152);
            this.settingsLinkButton.Name = "settingsLinkButton";
            this.settingsLinkButton.Size = new System.Drawing.Size(114, 13);
            this.settingsLinkButton.TabIndex = 7;
            this.settingsLinkButton.TabStop = true;
            this.settingsLinkButton.Text = "Изменить настройки";
            this.settingsLinkButton.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SettingLinkButtonLinkClicked);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 177);
            this.Controls.Add(this.settingsLinkButton);
            this.Controls.Add(this.saveCreditionals);
            this.Controls.Add(this.useSystemAuth);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.loginTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Авторизация БД";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.CheckBox useSystemAuth;
        private System.Windows.Forms.CheckBox saveCreditionals;
        private System.Windows.Forms.LinkLabel settingsLinkButton;
    }
}