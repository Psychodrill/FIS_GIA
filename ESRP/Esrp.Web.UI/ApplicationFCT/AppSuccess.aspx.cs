using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using Esrp.Core.Organizations;
using Esrp.Core.ApplicationFCT;
using Esrp.Utility;

namespace Esrp.Web.ApplicationFCT
{
    public partial class AppSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            if (org == null)
                Response.Redirect("AppNoAccess.aspx");
            Esrp.Core.ApplicationFCT.ApplicationFCT app = ApplicationFCTDataAccessor.Get(org.Id);
            if (app == null)
                Response.Redirect("AppEnter.aspx");
            else
            {
                if (app.FillingStage == 1)
                    Response.Redirect("AppStep2.aspx");
            }
        }

        protected void ShowApp(object sender, EventArgs e)
        {
            Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            string strPDFFile, strShPDFPath;
            if (org != null)
            {
                Esrp.Core.ApplicationFCT.ApplicationFCT app = ApplicationFCTDataAccessor.Get(org.Id);
                if (app != null)
                {
                    strPDFFile = org.Id + "_Forms_1-2_" + DateTime.Now.ToString("ddMMyy")  + ".pdf";
                    strShPDFPath = Request.PhysicalApplicationPath + Config.SharedDocumetsFolder.Trim('/') + "\\" + strPDFFile;
                    ApplicationFCTExport.SaveApplicationFCTInPDF(strShPDFPath, org, app);
                    ResponseWriter.WriteFile(strPDFFile, "application/pdf", strShPDFPath);

                }
            }


        }
    }
}