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
using System.Xml.Linq;

namespace Esrp.Web.Profile
{
    public partial class AccountKeyEditSuccess : BasePage
    {
        private const string AccountKeyCodeQueryKey = "key";
        public string AccountKeyCode
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[AccountKeyCodeQueryKey]))
                    return string.Empty;
                return Request.QueryString[AccountKeyCodeQueryKey];
            }
        }
    }
}
