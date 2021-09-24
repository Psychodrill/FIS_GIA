using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Esrp.Web.Extentions;

namespace Esrp.Web.Administration.Reports
{
    public partial class OrgRegistrationSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.ReportViewer1.ServerReport.ReportServerUrl =
                    new Uri(ConfigurationManager.AppSettings["ReportingServicesUrl"]);
                this.ReportViewer1.ServerReport.ReportPath = "/"
                                                             +
                                                             ConfigurationManager.AppSettings[
                                                                 "OrgRegistrationSummaryReportName"];
                this.ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //this.ReportViewer1.ServerReport.Refresh();
            base.OnPreRender(e);
        }
    }
}