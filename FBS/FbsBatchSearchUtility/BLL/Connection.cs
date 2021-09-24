namespace FbsBatchSearchUtility.BLL
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Класс для хранения данных о соединении.
    /// </summary>
    [Serializable]
    public class Connection : ISerializable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        public Connection()
        {
            this.Server = string.Empty;
            this.DataBase = "esrp_db";
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.UseSystemCreditionals = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// Дополнитеные парметры используются при загрузке настроек из файла.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public Connection(SerializationInfo info, StreamingContext context)
        {
            this.Server = (string)info.GetValue("Server", typeof(string));
            this.DataBase = (string)info.GetValue("DataBase", typeof(string));
            this.UserName = (string)info.GetValue("UserName", typeof(string));
            this.Password = (string)info.GetValue("Password", typeof(string));
            this.UseSystemCreditionals = (bool)info.GetValue("UseSystemCreditionals", typeof(bool));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Имя используемой БД
        /// </summary>
        public string DataBase { get; set; }

        /// <summary>
        /// Пароль к БД (не имеет значения если UseSystemCreditionals = true)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя сервера БД
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Использовать системную авторизацию.
        /// </summary>
        public bool UseSystemCreditionals { get; set; }

        /// <summary>
        /// Пользователь (не имеет значения если UseSystemCreditionals = true)
        /// </summary>
        public string UserName { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get object data.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Server", this.Server);
            info.AddValue("DataBase", this.DataBase);
            info.AddValue("UserName", this.UserName);
            info.AddValue("Password", this.Password);
            info.AddValue("UseSystemCreditionals", this.UseSystemCreditionals);
        }

        #endregion
    }
}