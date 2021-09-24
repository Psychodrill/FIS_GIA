using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Web.ReportingServices;
using Esrp.Web.Extentions;
using System.Configuration;

namespace Esrp.Web.Administration.Reports
{
    public partial class Default : System.Web.UI.Page
    {
        private string reportingServicesUrl = ConfigurationManager.AppSettings["ReportingServicesUrl"] + "/Pages/ReportViewer.aspx?{0}&rs:Command=Render";
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteMapNode node = Utility.FindSiteMapNodeFromResourceKey("report");
            List<SiteMapNode> nodes = new List<SiteMapNode>();
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["RSIsR2"]))
            {
                ReportingService2010 service = new ReportingService2010();
                service.Url = ConfigurationManager.AppSettings["ReportingServicesUrl"] + "/ReportService2010.asmx";

                if (node != null)
                    nodes.AddRange(node.ChildNodes.OfType<SiteMapNode>().Where(x => Utility.ShowNodebyUserRoles(x.Roles, x["notroles"] == null ? null : x["notroles"].Split(',').ToList())));
                service.Credentials = new ReportServerCredentials().NetworkCredentials;
                CatalogItem[] items = service.ListChildren("/", true);
                foreach (CatalogItem i in items)
                {
                    if (i.TypeName == "Report")
                    {
                        nodes.Add(new SiteMapNode(SiteMap.Provider, "report") { Url = String.Format(reportingServicesUrl, i.Path), Title = String.IsNullOrEmpty(i.Description) ? i.Name : i.Description });
                    }
                }
            }
            else
            {
                ReportingServices2005.ReportingService2005 service = new ReportingServices2005.ReportingService2005();
                service.Url = ConfigurationManager.AppSettings["ReportingServicesUrl"] + "/ReportService2005.asmx";

                if (node != null)
                    nodes.AddRange(node.ChildNodes.OfType<SiteMapNode>().Where(x => Utility.ShowNodebyUserRoles(x.Roles, x["notroles"] == null ? null : x["notroles"].Split(',').ToList())));
                service.Credentials = new ReportServerCredentials().NetworkCredentials;
                ReportingServices2005.CatalogItem[] items = service.ListChildren("/",true);
                foreach (ReportingServices2005.CatalogItem i in items)
                {
                    if (i.Type==ReportingServices2005.ItemTypeEnum.Report)
                    {
                        nodes.Add(new SiteMapNode(SiteMap.Provider, "report") { Url = String.Format(reportingServicesUrl, i.Path), Title = String.IsNullOrEmpty(i.Description) ? i.Name : i.Description });
                    }
                }

            }

            this.reportsList.DataSource = nodes;

        }
        protected override void OnPreRender(EventArgs e)
        {
            this.DataBind();
            base.OnPreRender(e);
        }
    }
}