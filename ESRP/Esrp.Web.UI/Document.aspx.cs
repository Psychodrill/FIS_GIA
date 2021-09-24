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
using Esrp.Core;

namespace Esrp.Web
{
    using Esrp.Utility;

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
            Esrp.Core.Document doc = Esrp.Core.Document.GetDocument(DocumentId);
            
            // Проверю существование документа и его "активность". Пользователь не имеет права 
            // смотреть неактивные документы.
            if (doc == null || !doc.IsActive)
            {
                if (doc == null)
                {
                    LogManager.Warning(string.Format("документ c id {0} не содержит тело (null) в бд. url {1}", this.DocumentId, this.Request.RawUrl));    
                }
                else
                {
                    LogManager.Warning(string.Format("документ c id {0} не активен, поэтому не выдается пользователю. url {1}", this.DocumentId, this.Request.RawUrl));    
                }
                
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
