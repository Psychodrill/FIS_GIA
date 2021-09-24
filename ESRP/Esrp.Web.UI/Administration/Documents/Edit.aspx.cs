using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Esrp.Core;
using System.IO;

namespace Esrp.Web.Administration.Documents
{
    public partial class Edit : BasePage
    {
        private const string DocumentQueryKey = "id";
        private const string ErrorDocumentNotFound = "Документ не найден";
        private const string SuccessUri = "/Administration/Documents/Edit.aspx?id={0}";

        // Название отдаваемого файла
        private const string FileName = "Document";

        private long DocumentId
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[DocumentQueryKey]))
                    throw new NullReferenceException(ErrorDocumentNotFound);

                long result;
                if (!long.TryParse(Request.QueryString[DocumentQueryKey], out result))
                    throw new NullReferenceException(ErrorDocumentNotFound);

                return result;
            }
        }

        private Esrp.Core.Document mCurrentDocument;
        public Esrp.Core.Document CurrentDocument
        {
            get
            {
                if (mCurrentDocument == null)
                {
                    if ((mCurrentDocument = Esrp.Core.Document.GetDocument(DocumentId)) == null)
                        throw new NullReferenceException(ErrorDocumentNotFound);
                    if (Page.IsPostBack)
                        DataBindCurrenDocument();
                }
                return mCurrentDocument;
            }
        }

        private void DataBindCurrenDocument()
        {
            mCurrentDocument.IsActive = Convert.ToBoolean(ddlIsActive.SelectedValue);
            mCurrentDocument.Description = txtDescription.Text.Trim();
            mCurrentDocument.Name = txtName.Text.Trim();
            mCurrentDocument.ContextCodes = DDLContext.SelectedValue;
            mCurrentDocument.RelativeUrl = txtRelativeUrl.Text.Trim();
            if (fuDocument.PostedFile.ContentLength > 0)
            {
                mCurrentDocument.Content = fuDocument.FileBytes;
                mCurrentDocument.ContentSize = fuDocument.PostedFile.ContentLength;
                mCurrentDocument.ContentType = fuDocument.PostedFile.ContentType;
                mCurrentDocument.Extension = Path.GetExtension(fuDocument.FileName);
            }
        }

        protected void validFileSize_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = fuDocument.FileBytes.Length > 0;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Создам новый документ
            CurrentDocument.Update();

            // Выполню действия после успешного создания документа
            ProcessSuccess();
        }

        private void ProcessSuccess()
        {
            Response.Redirect(String.Format(SuccessUri, CurrentDocument.Id));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("List.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLink.HRef"];
            }

            // Заполню соответствующие контролы
            txtName.Text = CurrentDocument.Name;
            txtDescription.Text = CurrentDocument.Description;
            DDLContext.SelectedValue = CurrentDocument.ContextCodes;
            ddlIsActive.SelectedValue = CurrentDocument.IsActive.ToString();
            txtRelativeUrl.Text = CurrentDocument.RelativeUrl;

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование документа “{0}”", CurrentDocument.Name);
        }
    }
}
