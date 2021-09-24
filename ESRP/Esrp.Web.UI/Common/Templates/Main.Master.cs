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
using Esrp.Core.Systems;

namespace Esrp.Web.Common.Templates
{
    public partial class Main : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //LoginNameHolder.Visible = !string.IsNullOrEmpty(User.Identity.Name);             
            LoginFormHolder.Visible = string.IsNullOrEmpty(User.Identity.Name);
            RememberMeHolder.Visible = !Config.DisableRememberMe; // раньше было !Config.DisableRememberMe;
        }
    }
}
