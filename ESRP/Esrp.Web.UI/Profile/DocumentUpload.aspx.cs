namespace Esrp.Web.Personal.Profile
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;

    using Esrp.Core.Users;
    using Esrp.UploadModule;
    using Esrp.Utility;

    /// <summary>
    /// The document upload.
    /// </summary>
    public partial class DocumentUpload : BasePage
    {
        #region Constants and Fields

        /// <summary>
        /// The success uri.
        /// </summary>
        public const string SuccessUri = "/Profile/View.aspx";

        private const string FailedUri = "/Profile/DocumentUpload.aspx";

        private const string InvalidLoginErrorFormat = "Пользователь \"{0}\" не найден";

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets FileId.
        /// </summary>
        public string FileId
        {
            get
            {
                if (this.Session["FileId"] == null)
                {
                    this.Session["FileId"] = Guid.NewGuid().ToString();
                }

                return (string)this.Session["FileId"];
            }
        }

        /// <summary>
        /// Режим работы контролов страницы
        /// </summary>
        /// <remarks>
        /// Если ложь, то при загрузке файла используется javascript и HTTP модуль.
        /// Если истина, то используется стандартный механизм загрузки, через post.
        /// </remarks>
        public bool IsSimpleForm
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["DisableAdvancedDocumentUpload"])
                       || this.Session["Simple"] != null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The page_ error.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Error(object sender, EventArgs e)
        {
            // Получу ошибку
            var checkException = this.Server.GetLastError() as HttpException;

            // Если ошибка "Maximum request length exceeded"
            // !! Не работает на asp.net development server
            if (checkException != null && checkException.ErrorCode == -2147467259)
            {
                this.Session["FileTooLarge"] = true;
                this.Server.ClearError();
                this.Response.Redirect(FailedUri, true);
            }
        }

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Если была ошибка "Maximum request length exceeded", покажу ее (для пользователей
            // без javascript)
            this.cvFileTooLarge.IsValid = this.Session["FileTooLarge"] == null
                                          || !Convert.ToBoolean(this.Session["FileTooLarge"]);

            // Уберу из сессии флаг ошибки
            this.Session.Remove("FileTooLarge");

            if (this.Session["Simple"] == null && !string.IsNullOrEmpty(this.Request.QueryString["Simple"]))
            {
                this.Session.Add("Simple", true);
            }

            var status = this.Application[this.FileId] as UploadStatus;
            if (status != null)
            {
                if (status.IsCanceled)
                {
                    this.Application.Remove(status.FileId);
                }
                else
                {
                    var fs = new FileStream(status.FileName, FileMode.Open);
                    var buffer = new byte[(int)fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    File.Delete(status.FileName);
                    this.UpdateRegistrationDocument(
                        buffer, Utility.FindMimeFromData(status.FileType, null, null), status.FileType);
                    status.CurrentBytesTransfered = status.TotalBytes;
                    this.Application[status.FileId] = status;
                    this.Session.Remove("FileId");
                }
            }
            else if (this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0)
            {
                long fileSize = this.Request.Files[0].InputStream.Length;
                var buffer = new byte[fileSize];
                this.Request.Files[0].InputStream.Read(buffer, 0, (int)fileSize);
                this.UpdateRegistrationDocument(
                    buffer, this.Request.Files[0].ContentType, Path.GetExtension(this.Request.Files[0].FileName));

                // выполню действия после успешной записи файла
                this.ProcessSuccess();
            }
        }

        private void ProcessSuccess()
        {
            // перейду на страницу успеха
            this.Response.Redirect(SuccessUri, true);
        }

        private void UpdateRegistrationDocument(byte[] Data, string ContentType, string Extension)
        {
            // если у заявки 2 пользователя, то отправить оповещение и другому пользователю
            OrgUser orgUser = OrgUserDataAccessor.Get(this.CurrentUserName);
            OrgRequest orgRequest = OrgRequestManager.GetRequest(orgUser.RequestedOrganization.Id);

            foreach (OrgUserBrief orgUserBrief in orgRequest.LinkedUsersOrg)
            {
                if (this.CurrentUserName.ToLower().CompareTo(orgUserBrief.Login.ToLower()) != 0)
                {
                    Utility.AddUserRegistrationDocument(orgUserBrief.Login, ContentType, Extension, Data, true);
                }
            }

            Utility.AddUserRegistrationDocument(this.CurrentUserName, ContentType, Extension, Data, false);
        }

        #endregion
    }
}