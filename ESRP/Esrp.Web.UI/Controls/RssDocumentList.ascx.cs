using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Services;

namespace Esrp.Web.Controls
{
    public partial class RssDocumentList : System.Web.UI.UserControl
    {
        public readonly int MAX_DOCS_TITLE_LENGTH = 100;
        public readonly int DOCS_COUNT = 5;
        private readonly DocumentsService documentsService = new DocumentsService();

        protected void Page_Load(object sender, EventArgs e)
        {
            var docs = this.documentsService.SelectDocuments();
            if (docs.Count > DOCS_COUNT)
            {
                // удалим лишние 
                // todo проверить что нужно показывать первые а не последние 3
                docs.RemoveRange(DOCS_COUNT, docs.Count - DOCS_COUNT);
            }
            
            foreach (var item in docs)
            {
                if (item.Name.Length > MAX_DOCS_TITLE_LENGTH)
                {
                    item.Name = item.Name.Remove(MAX_DOCS_TITLE_LENGTH - 3) + "...";
                }
            }
            lvDocuments.DataSource = docs;
            lvDocuments.DataBind();
        }
    }
}