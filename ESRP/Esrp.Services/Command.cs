namespace Esrp.Services
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Класс Command
    /// </summary>
    internal class Command : IDisposable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Создает новый экземпляр класса <see cref="Command"/> .
        /// </summary>
        /// <param name="procname">
        /// Название процедуры.
        /// </param>
        public Command(string procname)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception(string.Format("Файл конфигурации: настройки соединения с бд отсутствует: {0}", "Esrp.Core.Properties.Settings.EsrpConnectionString"));
            }

            this.Connection = new SqlConnection(connectionString);
            this.Connection.Open();
            this.Cmd = this.Connection.CreateCommand();
            this.Cmd.CommandText = procname;
            this.Cmd.CommandType = CommandType.StoredProcedure;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Параметры
        /// </summary>
        public SqlParameterCollection Parameters
        {
            get
            {
                return this.Cmd.Parameters;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Комманда
        /// </summary>
        private SqlCommand Cmd { get; set; }

        /// <summary>
        /// Подключение
        /// </summary>
        private SqlConnection Connection { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Расположить
        /// </summary>
        public void Dispose()
        {
            this.Cmd.Dispose();
            this.Connection.Dispose();
        }

        /// <summary>
        /// Выполнить Reader
        /// </summary>
        /// <returns>
        /// SqlDataReader
        /// </returns>
        public SqlDataReader ExecuteReader()
        {
            return this.Cmd.ExecuteReader();
        }

        /// <summary>
        /// Выполнить Scalar
        /// </summary>
        /// <returns>
        /// Результат.
        /// </returns>
        public object ExecuteScalar()
        {
            return this.Cmd.ExecuteScalar();
        }

        #endregion
    }
}