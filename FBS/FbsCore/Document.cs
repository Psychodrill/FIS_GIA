using System;
using System.Data.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.CompilerServices;
using Fbs.Utility;

namespace Fbs.Core
{
    public partial class Document
    {
        // Расширение документа по умолчанию
        private const string DefaultFileExtension = ".bin";

        private const string InternalContentTypeFormat = "{0}|{1}";

        private static PressContext mContext = null;
        protected static PressContext Context
        {
            get
            {
                if (mContext == null)
                {
                    mContext = new PressContext();
                    mContext.ObjectTrackingEnabled = false;
                }
                return mContext;
            }
        }

        static public Document GetDocument(long id)
        {
            try
            {
                return Context.GetDocument(id).Single<Document>();
            }
            catch
            {
                return null;
            }
        }

        static public Document GetDocument(string relativeUrl)
        {
            long? documentId = GetDocumentId(relativeUrl);
            if (documentId == null)
                return null;

            return GetDocument((long)documentId);
        }

        /// <summary>
        /// Удалить указанный список документов
        /// </summary>
        /// <param name="ids"></param>
        public static void DeleteDocuments(long[] ids)
        {
            Context.DeleteDocument(IdsToString(ids), ClientLogin, ClientIp);
        }

        /// <summary>
        /// Опубликовать указанный список документов
        /// </summary>
        /// <param name="ids">массив идентификаторов документов</param>
        public static void ActivateDocuments(long[] ids)
        {
            Context.SetActiveDocument(IdsToString(ids), true, ClientLogin, ClientIp);
        }

        /// <summary>
        /// Снять с публикации указанный список документов
        /// </summary>
        /// <param name="ids">массив идентификаторов документов</param>
        public static void DeactivateDocuments(long[] ids)
        {
            Context.SetActiveDocument(IdsToString(ids), false, ClientLogin, ClientIp);
        }

        static private string IdsToString(long[] ids)
        {
            string[] result = new string[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                result[i] = ids[i].ToString();
            return String.Join(",", result);
        }

        public System.Data.Linq.Binary Content
        {
            get
            {
                if (this.InternalContent != null && this.InternalContent.Length == 0)
                    return null;
                return this.InternalContent;
            }
            set
            {
                if (value != null && value.Length > 0)
                    this.InternalContent = value;
                else
                    this.InternalContent = null;
            }
        }

        public string RelativeUrl
        {
            get
            {
                return InternalRelativeUrl;
            }
            set
            {
                if (value == string.Empty)
                {
                    this.InternalRelativeUrl = null;
                    return;
                }
                InternalRelativeUrl = value;
            }
        }

        partial void OnCreated()
        {
            // означиваю поля при создании объекта
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        partial void OnLoaded()
        {
            // переозначиваю поля после загрузки данных в объект, т.к. в этом случае значения,  
            // которые присваивались в OnCreated(), сбрасываются
            this._EditorLogin = ClientLogin;
            this._EditorIp = ClientIp;
        }

        #region Текущий пользователь

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

        #endregion

        public void Update()
        {
            Context.InternalUpdateDocument(this);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        static public long? GetDocumentId(string relativeUrl)
        {
            GetDocumentByUrlResult result = Context.GetDocumentByUrl(relativeUrl).SingleOrDefault();
            if (result == null)
                return null;

            return result.Id;
        }

        /// <summary>
        /// Склейка ContentType и Extension 
        /// </summary>
        private void SetInternalContentType()
        {
            InternalContentType = String.Format(InternalContentTypeFormat, mContentType, mExtension);
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

        public void WriteToResponse()
        {
            string fileExtension = this.Extension;
            // Если расширение не задано, то использую расширение по умолчанию (".bin")
            if (fileExtension == null || fileExtension.Length < 2)
                fileExtension = DefaultFileExtension;

            // Запишу файл в response
            ResponseWriter.WriteFile(this.Name + fileExtension, this.ContentType, this.Content);
        }
    }

    partial class PressContext
    {
        internal void InternalUpdateDocument(Document document)
        {
            UpdateDocument(document);
        }
    }
}
