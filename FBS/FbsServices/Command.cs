namespace FbsServices
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
        /// <param name="procname">Название хранимой процедуры</param>
        public Command(string procname)
            : this(procname, CommandType.StoredProcedure)
        {
        }

        /// <summary>
        /// Создает новый экземпляр класса <see cref="Command"/> .
        /// </summary>
        /// <param name="procname">
        /// Текст команды
        /// </param>
        /// <param name="commandType">Тип команды</param>
        public Command(string procname, CommandType commandType)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception(string.Format("Файл конфигурации: настройки соединения с бд отсутствует: {0}", "Fbs.Core.Properties.Settings.FbsConnectionString"));
            }

            this.Connection = new SqlConnection(connectionString);
            this.Connection.Open();
            this.Cmd = this.Connection.CreateCommand();
            this.Cmd.CommandText = procname;
            this.Cmd.CommandType = commandType;
            this.Cmd.CommandTimeout = 600;
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

        public SqlDataAdapter CreateAdapter()
        {
            return new SqlDataAdapter(this.Cmd);
        }

        public int ExecuteNonQuery()
        {
            return this.Cmd.ExecuteNonQuery();
        }

        #endregion
    }
}