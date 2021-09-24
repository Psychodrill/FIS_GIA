namespace FbsBatchSearchUtility
{
    using System;
    using System.Windows.Forms;

    using FbsBatchSearchUtility.BLL;

    /// <summary>
    /// Форма авторизации пользователя
    /// </summary>
    public partial class LoginForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginForm"/> class.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Экземпляр сервиса выборки. ДОЛЖЕН БЫТЬ УСТАНОВЛЕН ПЕРЕД ОТКРЫТИЕМ ОКНА.
        /// </summary>
        public ReportService ReportServiceInstance { get; set; }

        /// <summary>
        /// Устанавливает чекбокс сохранения параметров подключения
        /// </summary>
        public bool SaveCreditionals
        {
            get { return this.saveCreditionals.Checked; }
            set { this.saveCreditionals.Checked = value; }
        }

        /// <summary>
        /// Устанавливает значение окна с логином.
        /// </summary>
        public string Login
        {
            get { return this.loginTextBox.Text; }
            set { this.loginTextBox.Text = value; }
        }

        /// <summary>
        /// Устанавливает значение окна с паролем
        /// </summary>
        public string Password
        {
            get { return this.passwordTextBox.Text; }
            set { this.passwordTextBox.Text = value; }
        }

        /// <summary>
        /// Устанавливает чекбокс использования системной авторизации в БД
        /// </summary>
        public bool UseSystemAuth
        {
            get { return this.useSystemAuth.Checked; }
            set { this.useSystemAuth.Checked = value; }
        }

        private void UseSystemAuthCheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox.Enabled = !useSystemAuth.Checked;
            loginTextBox.Enabled = !useSystemAuth.Checked;
        }

        private void LoginButtonClick(object sender, EventArgs e)
        {
            this.ReportServiceInstance.ConnectionStringManagerInstance.Connection.UserName = this.loginTextBox.Text;
            this.ReportServiceInstance.ConnectionStringManagerInstance.Connection.Password = this.passwordTextBox.Text;
            this.ReportServiceInstance.ConnectionStringManagerInstance.Connection.UseSystemCreditionals = useSystemAuth.Checked;

            if (!this.ReportServiceInstance.Connect())
            {
                MessageBox.Show("Невозможно подключиться к серверу баз данных с использованием указанных параметров.",
                "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Close();
            }
        }

        private void SettingLinkButtonLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.ConnectionStringManagerInstance = this.ReportServiceInstance.ConnectionStringManagerInstance;
            settingsForm.ShowDialog();
        }
    }
}
