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

namespace Fbs.Web
{
    public partial class Document : System.Web.UI.Page
    {
        private const string DocumentQueryKey = "id";
        private const string ErrorDocumentNotFound = "Документ не найден";

        private long DocumentId
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[DocumentQueryKey]))
                    return 0;

                long result;
                if (!long.TryParse(Request.QueryString[DocumentQueryKey], out result))
                    return 0;

                return result;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Загружу документ из БД
            Fbs.Core.Document doc = Fbs.Core.Document.GetDocument(DocumentId);
            
            // Проверю существование документа и его "активность". Пользователь не имеет права 
            // смотреть неактивные документы.
            if (doc == null || !doc.IsActive)
            {
                //throw new NullReferenceException(ErrorDocumentNotFound);
                Response.Write("<p>" + ErrorDocumentNotFound + "</p>");
            }
            else
            {
                // Отдам документ в response
                doc.WriteToResponse();
            }
        }
    }
}
