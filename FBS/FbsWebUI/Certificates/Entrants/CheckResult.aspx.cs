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

namespace Fbs.Web.Certificates.Entrants
{
    public partial class CheckResult : System.Web.UI.Page
    {
        protected void rpSearch_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Показываю только один элемент в репитере, остальные скрываю.
            if ((e.Item.Visible = rpSearch.Items.Count < 1))
            {
                HiddenField hfIsExist = e.Item.FindControl("hfIsExist") as HiddenField;
                Panel pNotExist = e.Item.FindControl("pNotExist") as Panel;
                Panel pResult = e.Item.FindControl("pResult") as Panel;

                if (hfIsExist != null && pNotExist != null && pResult != null && !Convert.ToBoolean(hfIsExist.Value))
                {
                    pResult.Visible = false;
                    pNotExist.Visible = true;
                }
            }
        }
    }
}
