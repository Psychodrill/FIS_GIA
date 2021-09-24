using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Core.ApplicationFCT;
using Esrp.Core.Organizations;

namespace Esrp.Web.ApplicationFCT
{
    public partial class AppEnter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            if (org == null)
            {
                Response.Redirect("AppNoAccess.aspx");
                return;
            }


            if ((Request.UrlReferrer != null) && (Request.UrlReferrer.PathAndQuery != null) && (Request.UrlReferrer.PathAndQuery.IndexOf("AppStep1.aspx") >= 0))
            {
                agree.Checked = true;
                return;
            }
            

            int FillingStage = ApplicationFCTDataAccessor.GetFillingStage(org.Id);
            if (FillingStage == 1)
                Response.Redirect("AppStep2.aspx");
            if (FillingStage == 2)
                Response.Redirect("AppSuccess.aspx");
        }

        protected void ValidateEnter(object sender, EventArgs e)
        {
            if (!agree.Checked)
            {
                errpanel.Visible = true;                                
                return;
            }
            Response.Redirect("AppStep1.aspx");
        }
    }
}