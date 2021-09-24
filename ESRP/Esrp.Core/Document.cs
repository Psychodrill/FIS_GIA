namespace Esrp.Core
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web;

    using Esrp.Core.DataAccess;
    using Esrp.Utility;
    using System.Collections.Generic;

    /// <summary>
    /// The document.
    /// </summary>
    public partial class Document
    {
        // Расширение документа по умолчанию
        #region Constants and Fields

        private const string DefaultFileExtension = ".bin";

        private const string InternalContentTypeFormat = "{0}|{1}";

        private static PressContext mContext;

        private string mContentType;

        private string mExtension;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets ClientIp.
        /// </summary>
        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        /// <summary>
        /// Gets ClientLogin.
        /// </summary>
        public static string ClientLogin
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.User.Identity.Name;
            }
        }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        public Binary Content
        {
            get
            {
                if (this.InternalContent != null && this.InternalContent.Length == 0)
                {
                    return null;
                }

                return this.InternalContent;
            }

            set
            {
                if (value != null && value.Length > 0)
                {
                    this.InternalContent = value;
                }
                else
                {
                    this.InternalContent = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets ContentType.
        /// </summary>
        public string ContentType
        {
            get
            {
                if (this.mContentType == null)
                {
                    string[] parts = this.InternalContentType.Split("|".ToCharArray());
                    this.mContentType = parts[0];
                }

                return this.mContentType;
            }

            set
            {
                this.mContentType = value;
                this.SetInternalContentType();
            }
        }

        /// <summary>
        /// Gets or sets Extension.
        /// </summary>
        public string Extension
        {
            get
            {
                if (this.mExtension == null)
                {
                    string[] parts = this.InternalContentType.Split("|".ToCharArray());
                    this.mExtension = parts.Length > 1 ? parts[1] : string.Empty;
                }

                return this.mExtension;
            }

            set
            {
                this.mExtension = value;
                this.SetInternalContentType();
            }
        }

        /// <summary>
        /// Gets or sets RelativeUrl.
        /// </summary>
        public string RelativeUrl
        {
            get
            {
                return this.InternalRelativeUrl;
            }

            set
            {
                if (value == string.Empty)
                {
                    this.InternalRelativeUrl = null;
                    return;
                }

                this.InternalRelativeUrl = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Context.
        /// </summary>
        protected static PressContext Context
        {
            get
            {
                return mContext = new PressContext { ObjectTrackingEnabled = false };
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Опубликовать указанный список документов
        /// </summary>
        /// <param name="ids">
        /// массив идентификаторов документов
        /// </param>
        public static void ActivateDocuments(long[] ids)
        {
            Context.SetActiveDocument(IdsToString(ids), true, ClientLogin, ClientIp);
        }

        /// <summary>
        /// Снять с публикации указанный список документов
        /// </summary>
        /// <param name="ids">
        /// массив идентификаторов документов
        /// </param>
        public static void DeactivateDocuments(long[] ids)
        {
            Context.SetActiveDocument(IdsToString(ids), false, ClientLogin, ClientIp);
        }

        /// <summary>
        /// Удалить указанный список документов
        /// </summary>
        /// <param name="ids">
        /// </param>
        public static void DeleteDocuments(long[] ids)
        {
            Context.DeleteDocument(IdsToString(ids), ClientLogin, ClientIp);
        }

        /// <summary>
        /// The get document.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static Document GetDocument(long id)
        {
            try
            {
                return Context.GetDocument(id).Single();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Возвращает список контекстов документов имеющихсяв системе
        /// </summary>
        /// <param name="onlyActive">Указывает необходимость поиска по активным документам</param>
        /// <returns>Возвращает список в виде словаря где ключ - код контекста, значение его описание</returns>
        public static Dictionary<string,string> GetAvailableContexts(bool onlyActive = true)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"select c.Id, c.Code,c.Name from document d join documentContext dc on d.id=dc.documentid join context c on dc.contextid=c.id"+
                                            (onlyActive?" where d.IsActive='true'":" ")+
                                            "group by c.Code,c.Name,c.Id order by c.Id";
                var res = sqlCommand.ExecuteReader();
                
                while (res.Read())
                {
                    ret.Add(res["Code"].ToString(), res["Name"].ToString());
                }
            }
            return ret;
        }

        /// <summary>
        /// The get document.
        /// </summary>
        /// <param name="relativeUrl">
        /// The relative url.
        /// </param>
        /// <returns>
        /// </returns>
        public static Document GetDocument(string relativeUrl)
        {
            long? documentId = GetDocumentId(relativeUrl);
            if (documentId == null)
            {
                return null;
            }

            return GetDocument((long)documentId);
        }

        /// <summary>
        /// The get document id.
        /// </summary>
        /// <param name="relativeUrl">
        /// The relative url.
        /// </param>
        /// <returns>
        /// </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static long? GetDocumentId(string relativeUrl)
        {
            GetDocumentByUrlResult result = Context.GetDocumentByUrl(relativeUrl).SingleOrDefault();
            if (result == null)
            {
                return null;
            }

            return result.Id;
        }

        /// <summary>
        /// The update.
        /// </summary>
        public void Update()
        {
            Context.InternalUpdateDocument(this);
        }

        /// <summary>
        /// The write to response.
        /// </summary>
        public void WriteToResponse()
        {
            string fileExtension = this.Extension;

            // Если расширение не задано, то использую расширение по умолчанию (".bin")
            if (fileExtension == null || fileExtension.Length < 2)
            {
                fileExtension = DefaultFileExtension;
            }

            // Запишу файл в response
            ResponseWriter.WriteFile(this.Name + fileExtension, this.ContentType, this.Content);
        }

        #endregion

        #region Methods

        private static string IdsToString(long[] ids)
        {
            var result = new string[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                result[i] = ids[i].ToString();
            }

            return string.Join(",", result);
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

        /// <summary>
        /// Склейка ContentType и Extension 
        /// </summary>
        private void SetInternalContentType()
        {
            this.InternalContentType = string.Format(InternalContentTypeFormat, this.mContentType, this.mExtension);
        }

        #endregion
    }

    
    /// <summary>
    /// The press context.
    /// </summary>
    partial class PressContext
    {
        #region Methods

        internal void InternalUpdateDocument(Document document)
        {
            this.UpdateDocument(document);
        }

        #endregion
    }
}