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

namespace Fbs.Web.Common.Templates
{
    public partial class Certificates : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Request.QueryString["embed"]))
            {
                this.topHolder.Visible = this.bottomHolder.Visible=this.userBlock.Visible=this.topMenu.Visible=this.topNav.Visible = !Convert.ToBoolean(this.Request.QueryString["embed"]);
            }
          
        }

      
        

    }
}
