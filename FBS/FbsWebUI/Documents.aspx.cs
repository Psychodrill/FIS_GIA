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

namespace Fbs.Web
{
    public partial class Documents : System.Web.UI.Page
    {
        private const string DefaultDocTypeCode = "other";//Общие документы
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetCSSClassByQueryString(string queryParam, object paramValue)
        {
            if (String.IsNullOrEmpty(Request[queryParam]) && paramValue.ToString() == DefaultDocTypeCode)
                return "select";
            if (Request[queryParam] == paramValue.ToString())
                return "select";
            return "";
        }
    }
}
