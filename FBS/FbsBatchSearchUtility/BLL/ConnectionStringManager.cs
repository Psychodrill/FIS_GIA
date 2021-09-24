namespace FbsBatchSearchUtility.BLL
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Windows.Forms;

    /// <summary>
    /// Управление параметрами подключения. Используется для формирования connectionStrings,
    /// а также загрузкой и созранением настроек.
    /// </summary>
    public class ConnectionStringManager
    {
        #region Constants and Fields

        private const string SETTINGS_FILENAME = "settings.bin";

        private Connection connection = new Connection
            {
               DataBase = "esrp_db", Server = string.Empty, UseSystemCreditionals = true 
            };

        #endregion

        #region Public Properties

        /// <summary>
        /// Текущие параметры подключения
        /// </summary>
        public Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Формирует строку подключения на основе текущих параметров
        /// </summary>
        /// <returns>
        /// Строка подюключения
        /// </returns>
        public string GetConnectionString()
        {
            if (this.connection == null)
            {
                this.LoadConnectionSettings();
            }

            var builder = new SqlConnectionStringBuilder();
            builder.UserID = this.Connection.UserName;
            builder.Password = this.Connection.Password;
            builder.IntegratedSecurity = this.connection.UseSystemCreditionals;
            builder.DataSource = this.connection.Server;
            builder.InitialCatalog = this.connection.DataBase;

            return builder.ConnectionString;
        }

        /// <summary>
        /// Загружает настройки подклчения из бинарного файла.
        /// </summary>
        public void LoadConnectionSettings()
        {
            try
            {
                Stream stream = File.Open(SETTINGS_FILENAME, FileMode.Open);
                var binFormatter = new BinaryFormatter();
                this.connection = (Connection)binFormatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Файл настроек не найден. Зайдите в настройки и укажите новые параметры подключения.", 
                    "Ошибка открытия файла", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Сохраняет текущие параметры подключения в бинарный файл.
        /// </summary>
        public void SaveConnectionSettings()
        {
            try
            {
                Stream stream = File.Open(SETTINGS_FILENAME, FileMode.Create);
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(stream, this.connection);
                stream.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Невозможно сохранить настройки: нет места на диске либо файл уже используется.", 
                    "Ошибка создания файла", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}