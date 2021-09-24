using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Fbs.Core
{
    /// <summary>
    /// Пользователь системы (ВУЗы, СУЗы).
    /// </summary>
    public partial class UserAccount : Account
    {
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
            /// На доработке.
            /// </summary>
            Revision = 2,       
            /// <summary>
            /// На согласовании.
            /// </summary>
            Consideration = 3,  
            /// <summary>
            /// Действующий.
            /// </summary>
            Activated = 4,      
            /// <summary>
            /// Отключенный.
            /// </summary>
            Deactivated = 5,    
        }

        private static string[] UserAccountStatusCodes = new string[] { string.Empty, 
            "registration", "revision", "consideration", "activated", "deactivated" };

        protected override void SetPassword(string passwordHash, string password)
        {
            this.PasswordHash = passwordHash;
            this.InternalPassword = password;
        }

        private bool IsAutoStatus
        {
            get
            {
                if (this.EditorLogin == string.Empty)
                    return false;
                return Account.GetType(this.EditorLogin) == typeof(UserAccount);
            }
        }

        private UserAccountRegistrationDocument mRegistrationDocument;

        public UserAccountRegistrationDocument RegistrationDocument
        {
            get
            {
                if (mRegistrationDocument == null)
                    mRegistrationDocument = new UserAccountRegistrationDocument(this);

                return mRegistrationDocument;
            }
        }

        public string AdminComment
        {
            get { return this.InternalAdminComment; }
        }

        public UserAccountStatusEnum Status
        {
            get
            {
                return ConvertStatusCode(this.InternalStatus); ; 
            }
            internal set
            {
                this.InternalStatus = UserAccountStatusCodes[(int)value];
            }
        }

        public static UserAccountStatusEnum ConvertStatusCode(string statusCode)
        {
            if (statusCode == null)
                return UserAccountStatusEnum.None;
            for (int index = 0; index < UserAccountStatusCodes.Length ; index++ )
                if (UserAccountStatusCodes[index] == statusCode.ToLower())
                    return (UserAccountStatusEnum)index;
            return UserAccountStatusEnum.None;
        }
        /*
        private string[] mIpAddresses;

        public string[] IpAddresses
        {
            get
            {
                if (mIpAddresses == null && InternalIpAddresses != null)
                    mIpAddresses = InternalIpAddresses.Split(",".ToCharArray());
                return mIpAddresses;
            }
            set
            {
                mIpAddresses = value;
                InternalIpAddresses = string.Join(",", value);
            }
        }
        */
        public void Update()
        {
            RefreshStatusBeforeUpdate();
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

        internal void RefreshStatusBeforeUpdate()
        {
            // Попытаться перевести в состояние "на рассмотрении" для пользователя.
            if (IsAutoStatus && Status != UserAccountStatusEnum.Activated && 
                    Status != UserAccountStatusEnum.Deactivated)
                this.Status = UserAccountStatusEnum.Consideration;
        }

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

        partial void OnCreated()
        {
            // Означиваю поля при создании объекта
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        partial void OnLoaded()
        {
            // переозначиваю поля после загрузки данных в объект, т.к. в этом случае значения,  
            // которые присваивались в OnCreated(), сбрасываются
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;

            // Пустой документ воспринимаем как отсутствующий.
            if (this._InternalRegistrationDocument != null && this._InternalRegistrationDocument.Length == 0)
                this._InternalRegistrationDocument = null;

            // Устанавливаю значения ContentType и Extension документа регистрации
            if (!String.IsNullOrEmpty(this.RegistrationDocument.InternalContentType))
            {
                string[] parts = this.RegistrationDocument.InternalContentType.Split("|".ToCharArray());
                this.RegistrationDocument.ContentType = parts[0];
                this.RegistrationDocument.Extension = parts.Length > 1 ? parts[1] : String.Empty;
            }
        }

        public static UserAccount GetUserAccount(string login)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().GetUserAccount(login).SingleOrDefault<UserAccount>();
            }
            finally
            {
                AccountContext.EndLock();
            }
        }

        public static UserAccount[] SearchSameUserAccount(string organizationName)
        {
            AccountContext.BeginLock();
            try
            {
                return AccountContext.Instance().SearchSameUserAccount(organizationName).ToArray<UserAccount>();
            }
            finally
            {
                AccountContext.EndLock();
            }
        }
    }

    public partial class AccountContext
    {
        internal void UpdateUser(UserAccount userAccount)
        {
            // Проверю не пустой ли документ
            if (userAccount.RegistrationDocument.Document != null && 
                userAccount.RegistrationDocument.Document.Length == 0)
            {
                userAccount.RegistrationDocument.Document = null;
                userAccount.RegistrationDocument.ContentType = null;
            }

            UpdateUserAccount(userAccount);
        }

        internal void UpdateRegistrationDocument(UserAccountRegistrationDocument registrationDocument)
        {
            UpdateUserAccountRegistrationDocument(registrationDocument);
        }

        internal void UpdateStatus(UserAccount userAccount)
        {
            UpdateUserAccountStatus(userAccount.Login, userAccount.InternalStatus, userAccount.AdminComment, 
                    userAccount.EditorLogin, userAccount.EditorIp);
        }
    }
}
