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
using Fbs.Core;

namespace Fbs.Web.Administration.Documents
{
    public partial class View : BasePage
    {
        private const string DocumentQueryKey = "id";
        private const string ErrorDocumentNotFound = "Документ не найден";

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

        private Fbs.Core.Document mCurrentDocument;
        public Fbs.Core.Document CurrentDocument
        {
            get
            {
                if (mCurrentDocument == null)
                {
                    if ((mCurrentDocument = Fbs.Core.Document.GetDocument(DocumentId)) == null)
                        throw new NullReferenceException(ErrorDocumentNotFound);
                }
                return mCurrentDocument;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            DDLContext.SelectedValue = CurrentDocument.ContextCodes;

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Просмотр документа “{0}”", CurrentDocument.Name);
        }
    }
}
