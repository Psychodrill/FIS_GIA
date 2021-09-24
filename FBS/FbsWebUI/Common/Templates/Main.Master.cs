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
using Esrp;

namespace Fbs.Web.Common.Templates
{
    public partial class Main : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            EsrpClient esrpClient = new EsrpClient()
            {
                EsrpUrl = Config.UrlEsrp,
                SystemId = SystemId.Fbs
            };
            esrpClient.RedirectDefault(Request, Response);
        }

    }
}
