using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fbs.Web.Parameters
{
    public class CurrentUserParameter : System.Web.UI.WebControls.Parameter
    {
        protected override object Evaluate(HttpContext context, Control control)
        {
            if (HttpContext.Current != null)
                return HttpContext.Current.User.Identity.Name;

            return String.Empty;
        }
    }
}
