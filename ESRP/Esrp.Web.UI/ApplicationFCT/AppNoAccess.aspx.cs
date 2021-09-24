using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Core.Organizations;

namespace Esrp.Web.ApplicationFCT
{
    public partial class AppNoAccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            if (org != null)
            {
                Response.Redirect("AppEnter.aspx");
                return;
            }

        }
    }
}