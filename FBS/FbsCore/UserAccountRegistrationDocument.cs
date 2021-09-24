using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Fbs.Utility;

namespace Fbs.Core
{
    public partial class UserAccountRegistrationDocument
    {
        // название отдаваемого файла
        private const string FileName = "Документ-основание регистрации";
        // Расширение документа по умолчанию
        private const string DefaultFileExtension = ".bin";

        private const string InternalContentTypeFormat = "{0}|{1}";

        public static string ClientLogin
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        public System.Data.Linq.Binary Document
        {
            get
            {
                return this.InternalDocument;
            }
            set
            {
                if (value != null && value.Length > 0)
                    this.InternalDocument = value;
                else
                    this.InternalDocument = null;

                if (mUserAccount != null)
                    mUserAccount.InternalRegistrationDocument = this.InternalDocument;
            }
        }

        /// <summary>
        /// Склейка ContentType и Extension 
        /// </summary>
        private void SetInternalContentType()
        {
            InternalContentType = String.Format(InternalContentTypeFormat, mContentType, mExtension);
            if (mUserAccount != null)
                mUserAccount.InternalRegistrationDocumentContentType = InternalContentType;
        }

        private string mContentType;
        public string ContentType
        {
            get
            {
                if (mContentType == null)
                {
                    string[] parts = InternalContentType.Split("|".ToCharArray());
                    mContentType = parts[0];
                }

                return mContentType;
            }
            set
            {
                mContentType = value;
                SetInternalContentType();
            }
        }

        private string mExtension;
        public string Extension
        {
            get
            {
                if (mExtension == null)
                {
                    string[] parts = InternalContentType.Split("|".ToCharArray());
                    mExtension = parts.Length > 1 ? parts[1] : String.Empty;
                }

                return mExtension;
            }
            set
            {
                mExtension = value;
                SetInternalContentType();
            }
        }

        partial void OnCreated()
        {
            this.EditorLogin = ClientLogin;
            this.EditorIp = ClientIp;
        }

        partial void OnLoaded()
        {
            // переозначиваю поля после загрузки данных в объект, т.к. в этом случае значения,  
            // которые присваивались в OnCreated(), сбрасываются
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
            if (this._InternalDocument != null && this._InternalDocument.Length == 0)
                this._InternalDocument = null;
        }

        public UserAccountRegistrationDocument(string login)
        {
            OnCreated();

            this.Login = login;
        }

        UserAccount mUserAccount;

        public UserAccountRegistrationDocument(UserAccount userAccount)
        {
            OnCreated();

            mUserAccount = userAccount;

            this._Login = mUserAccount.Login;
            this._InternalDocument = mUserAccount.InternalRegistrationDocument;
            this._InternalContentType = mUserAccount.InternalRegistrationDocumentContentType;
        }

        public void Update()
        {
            // Загрузку пустого документа игнорируем.
            if (this.InternalDocument == null)
                return;
            if (mUserAccount != null)
            {
                mUserAccount.RefreshStatusBeforeUpdate();
                this.InternalStatus = mUserAccount.InternalStatus;
            }
            AccountContext.BeginLock();
            try
            {
                AccountContext.Instance().UpdateRegistrationDocument(this);
            }
            finally
            {
                AccountContext.EndLock();
            }
            if (mUserAccount != null)
                mUserAccount.InternalStatus = this.InternalStatus;
        }

        public void WriteToResponse(HttpResponse response)
        {
            string fileExtension = this.Extension;
            // Если расширение не задано, то использую расширение по умолчанию (".bin")
            if (fileExtension == null || fileExtension.Length < 2)
                fileExtension = DefaultFileExtension;

            // Запишу файл в response
            ResponseWriter.WriteFile(FileName + fileExtension, this.ContentType, this.Document);

        }


    }
}
