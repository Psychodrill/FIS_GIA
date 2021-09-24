namespace FbsBatchSearchUtility
{
    using System;
    using System.Windows.Forms;

    using FbsBatchSearchUtility.BLL;

    /// <summary>
    /// Окно настроек подключения.
    /// </summary>
    public partial class SettingsForm : Form
    {
        private ConnectionStringManager manager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsForm"/> class.
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Экземпляр менеджера настроек подключения. ДОЛЖЕН БЫТЬ УСТАНОВЛЕН ПЕРЕД ПОДНЯТИЕМ ОКНА.
        /// </summary>
        public ConnectionStringManager ConnectionStringManagerInstance
        {
            set
            {
                this.manager = value;
                this.serverTextBox.Text = this.manager.Connection.Server;
                this.dbNameTextBox.Text = this.manager.Connection.DataBase;
            }
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveAndCloseButtonClick(object sender, EventArgs e)
        {
            this.manager.Connection.Server = this.serverTextBox.Text;
            this.manager.Connection.DataBase = this.dbNameTextBox.Text;
            this.manager.SaveConnectionSettings();
            Close();
        }
    }
}