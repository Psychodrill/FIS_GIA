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
using System.IO;

namespace Fbs.Web.Administration.Documents
{
    public partial class Create : BasePage
    {
        private const string SuccessUri = "/Administration/Documents/Edit.aspx?id={0}";

        private Fbs.Core.Document mCurrentDocument;
        private Fbs.Core.Document CurrentDocument
        {
            get
            {
                if (mCurrentDocument == null)
                {
                    mCurrentDocument = new Fbs.Core.Document();
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
            mCurrentDocument.ContextCodes = DDLContext.SelectedValue ;
            mCurrentDocument.Content = fuDocument.FileBytes;
            mCurrentDocument.ContentSize = fuDocument.PostedFile.ContentLength;
            mCurrentDocument.ContentType = fuDocument.PostedFile.ContentType;
            mCurrentDocument.Extension = Path.GetExtension(fuDocument.FileName);
            mCurrentDocument.RelativeUrl = txtRelativeUrl.Text.Trim();
        }

        private void ProcessSuccess()
        {
            Response.Redirect(String.Format(SuccessUri, CurrentDocument.Id));
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
    }
}
