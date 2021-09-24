using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Fbs.Core;

namespace Fbs.Web.Administration.Documents
{
    public partial class Document : BasePage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            // Загружу документ из БД
            Fbs.Core.Document doc = Fbs.Core.Document.GetDocument(DocumentId);

            // Проверю существование документа. Т.к. это административный раздел, то неактивные 
            // документы также доступны для просмотра.
            if (doc == null)
                throw new NullReferenceException(ErrorDocumentNotFound);

            // Отдам документ в response
            doc.WriteToResponse();
        }
    }
}
