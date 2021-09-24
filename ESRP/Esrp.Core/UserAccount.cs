// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserAccount.cs" company="">
//   
// </copyright>
// <summary>
//   Пользователь системы (ВУЗы, СУЗы).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Xml.Linq;

namespace Esrp.Core
{
    using System;
    using System.Linq;

    using Esrp.Core.Common;
    using System.Reflection;

    /// <summary>
    /// Пользователь системы (ВУЗы, СУЗы).
    /// </summary>
    public partial class UserAccount : Account
    {
        #region Constants and Fields

        /// <summary>
        /// The user account status codes.
        /// </summary>
        private static readonly string[] UserAccountStatusCodes = new[]
            {
               string.Empty, "registration", "consideration", "revision", "readonly", "activated", "deactivated" 
            };

        /// <summary>
        /// The m registration document.
        /// </summary>
        private UserAccountRegistrationDocument mRegistrationDocument;

        #endregion

        #region Enums

        /// <summary>
        /// Статус пользователя
        /// </summary>
        public enum UserAccountStatusEnum
        {
            /// <summary>
            /// Неизвестно.
            /// </summary>
            None = 0,

            /// <summary>
            /// На регистрации.
            /// </summary>
            Registration = 1,

            /// <summary>
            /// На согласовании.
            /// </summary>
            Consideration = 2,

            /// <summary>
            /// На доработке.
            /// </summary>
            Revision = 3,

            /// <summary>
            /// Только для чтения.
            /// </summary>
            Readonly = 4,

            /// <summary>
            /// Действующий.
            /// </summary>
            Activated = 5,

            /// <summary>
            /// Отключенный (заблокирован)
            /// </summary>
            Deactivated = 6,
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets AdminComment.
        /// </summary>
        public string AdminComment
        {
            get
            {
                return this.InternalAdminComment;
            }
        }

        /// <summary>
        /// Gets RegistrationDocument.
        /// </summary>
        public UserAccountRegistrationDocument RegistrationDocument
        {
            get
            {
                if (this.mRegistrationDocument == null)
                {
                    this.mRegistrationDocument = new UserAccountRegistrationDocument(this);
                }

                return this.mRegistrationDocument;
            }
        }

        /// <summary>
        /// Gets or sets Status.
        /// </summary>
        public UserAccountStatusEnum Status
        {
            get
            {
                return ConvertStatusCode(this.InternalStatus);

            }

            internal set
            {
                this.InternalStatus = UserAccountStatusCodes[(int)value];
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether IsAutoStatus.
        /// </summary>
        private bool IsAutoStatus
        {
            get
            {
                if (this.EditorLogin == string.Empty)
                {
                    return false;
                }

                return GetType(this.EditorLogin) == typeof(UserAccount);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The convert status code.
        /// </summary>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <returns>
        /// </returns>
        public static UserAccountStatusEnum ConvertStatusCode(string statusCode)
        {
            if (statusCode == null)
            {
                return UserAccountStatusEnum.None;
            }

            for (int index = 0; index < UserAccountStatusCodes.Length; index++)
            {
                if (UserAccountStatusCodes[index] == statusCode.ToLower())
                {
                    return (UserAccountStatusEnum)index;
                }
            }

            return UserAccountStatusEnum.None;
        }

        /// <summary>
        /// The get user account.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        public static UserAccount GetUserAccount(string login)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().GetUserAccount(login).SingleOrDefault();
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The search same user account.
        /// </summary>
        /// <param name="organizationName">
        /// The organization name.
        /// </param>
        /// <returns>
        /// </returns>
        public static UserAccount[] SearchSameUserAccount(string organizationName)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().SearchSameUserAccount(organizationName).ToArray();
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The update user status.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <returns>
        /// The update user status.
        /// </returns>
        public static string UpdateUserStatus(string login, string statusCode)
        {
            return UpdateUserStatus(login, statusCode, null);
        }

        public static string ApplyBatch(string name, XElement xml, int type)
        {
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().ApplyBatch(name, xml, type);
            }
            catch (DbException exc)
            {
                return exc.Message;
            }
            finally
            {
                AccountContext.EndLock();
            }

            return null;
        }

        /// <summary>
        /// The update user status.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <param name="cause">
        /// The cause.
        /// </param>
        /// <returns>
        /// The update user status.
        /// </returns>
        public static string UpdateUserStatus(string login, string statusCode, string cause)
        {
            UserAccount account = GetUserAccount(login);
            account.Status = ConvertStatusCode(statusCode);
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateStatus(account);
            }
            catch (DbException exc)
            {
                return exc.Message;
            }
            finally
            {
                AccountContext.EndLock();
            }

            return null;
        }

        /// <summary>
        /// The confirm registration.
        /// </summary>
        public void ConfirmRegistration()
        {
            this.Status = UserAccountStatusEnum.Activated;
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateStatus(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The deactivate.
        /// </summary>
        /// <param name="cause">
        /// The cause.
        /// </param>
        public void Deactivate(string cause)
        {
            this.Status = UserAccountStatusEnum.Deactivated;
            this.InternalAdminComment = cause;
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateStatus(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The send to revision.
        /// </summary>
        /// <param name="cause">
        /// The cause.
        /// </param>
        public void SendToRevision(string cause)
        {
            this.Status = UserAccountStatusEnum.Revision;
            this.InternalAdminComment = cause;
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateStatus(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The set readonly status.
        /// </summary>
        public void SetReadonlyStatus()
        {
            this.Status = UserAccountStatusEnum.Readonly;
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateStatus(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        public void Update()
        {
            this.RefreshStatusBeforeUpdate();
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateUser(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The refresh status before update.
        /// </summary>
        internal void RefreshStatusBeforeUpdate()
        {
            // Попытаться перевести в состояние "на рассмотрении" для пользователя.
            if (this.IsAutoStatus && this.Status != UserAccountStatusEnum.Activated
                && this.Status != UserAccountStatusEnum.Deactivated)
            {
                this.Status = UserAccountStatusEnum.Consideration;
            }
        }

        /// <summary>
        /// The set password.
        /// </summary>
        /// <param name="passwordHash">
        /// The password hash.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        protected override void SetPassword(string passwordHash, string password)
        {
            this.PasswordHash = passwordHash;
            this.InternalPassword = password;
        }

        /// <summary>
        /// The on created.
        /// </summary>
        partial void OnCreated()
        {
            // Означиваю поля при создании объекта
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        /// <summary>
        /// The on loaded.
        /// </summary>
        partial void OnLoaded()
        {
            // переозначиваю поля после загрузки данных в объект, т.к. в этом случае значения,  
            // которые присваивались в OnCreated(), сбрасываются
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;

            // Пустой документ воспринимаем как отсутствующий.
            if (this._InternalRegistrationDocument != null && this._InternalRegistrationDocument.Length == 0)
            {
                this._InternalRegistrationDocument = null;
            }

            // Устанавливаю значения ContentType и Extension документа регистрации
            if (!string.IsNullOrEmpty(this.RegistrationDocument.InternalContentType))
            {
                string[] parts = this.RegistrationDocument.InternalContentType.Split("|".ToCharArray());
                this.RegistrationDocument.ContentType = parts[0];
                this.RegistrationDocument.Extension = parts.Length > 1 ? parts[1] : string.Empty;
            }
        }

        #endregion

        public override void Dispose()
        {
            foreach(PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.CreateInstance))
            {
                if(pi.GetSetMethod()!=null)
                    pi.SetValue(this,null,null);            
            }
            this._EditorIp = "";
            this._EditorLogin = "";
            this._EducationInstitutionTypeId = null;
            this._EducationInstitutionTypeName = "";
            this._Email = "";
            this._FirstName = "";
            this._HasFixedIp = false;
            this._InternalAdminComment = "";
            this._InternalCanEdit = false;
            this._InternalCanEditRegistrationDocument = false;
            this._InternalRegistrationDocument = null;
            this._InternalRegistrationDocumentContentType = null;
            this._InternalStatus = "";
            this._IpAddresses = "";
            this._IsAgreedTimeConnection = null;
            this._IsAgreedTimeEnterInformation = null;
            this._KPP = "";
            this._LastName = "";
            this._Login = "";
            this._ModelName = "";
            this._OrganizationAddress = "";
            this._OrganizationChiefName = "";
            this._OrganizationFax = "";
            this._OrganizationFounderName = "";
            this._OrganizationINN = "";
            this._OrganizationName = "";
            this._OrganizationOGRN = "";
            this._OrganizationPhone = "";
            this._OrganizationRegionId = null;
            this._OrganizationRegionName = "";
            this._OrgTypeId = null;
            this._Password = "";
            this._PasswordHash = "";
            this._PatronymicName = "";
            this._Phone = "";
            this._RCDescription = "";
            this._RCModelID = null;
            this._ReceptionOnResultsCNE = null;
            this._TimeConnectionToSecureNetwork = null;
            this._TimeEnterInformationInFIS = null;
            base.Dispose();
        }
    }

    /// <summary>
    /// The account context.
    /// </summary>
    public partial class AccountContext
    {
        #region Methods

        /// <summary>
        /// The update registration document.
        /// </summary>
        /// <param name="registrationDocument">
        /// The registration document.
        /// </param>
        internal void UpdateRegistrationDocument(UserAccountRegistrationDocument registrationDocument)
        {
            this.UpdateUserAccountRegistrationDocument(registrationDocument);
        }

        internal void ApplyBatch(string name, XElement xml, int type)
        {
            try
            {
                this.ApplyBatchForRepl(name, xml, type);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("$"))
                {
                    throw new DbException(ex.Message.Substring(1), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// The update status.
        /// </summary>
        /// <param name="userAccount">
        /// The user account.
        /// </param>
        /// <exception cref="DbException">
        /// </exception>
        internal void UpdateStatus(UserAccount userAccount)
        {
            try
            {
                this.UpdateUserAccountStatus(
                    userAccount.Login,
                    userAccount.InternalStatus,
                    userAccount.AdminComment,
                    userAccount.EditorLogin,
                    userAccount.EditorIp);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("$"))
                {
                    throw new DbException(ex.Message.Substring(1), ex);
                }

                throw;
            }
        }

        /// <summary>
        /// The update user.
        /// </summary>
        /// <param name="userAccount">
        /// The user account.
        /// </param>
        internal void UpdateUser(UserAccount userAccount)
        {
            // Проверю не пустой ли документ
            if (userAccount.RegistrationDocument.Document != null
                && userAccount.RegistrationDocument.Document.Length == 0)
            {
                userAccount.RegistrationDocument.Document = null;
                userAccount.RegistrationDocument.ContentType = null;
            }

            this.UpdateUserAccount(userAccount);
        }

        #endregion
    }
}