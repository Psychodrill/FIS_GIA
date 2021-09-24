using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Fbs.Web.Certificates.CommonNationalCertificates;
using FbsServices;

namespace Fbs.Web.Common.Templates
{
    public partial class CertificateResultHistory : BaseMasterPage
    {
        private long CheckId
        {
            get { return Convert.ToInt64(Request.QueryString["id"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dsMaster.DataSource = CreateDataSource();
                dsMaster.DataBound += dsMaster_DataBound;
                dsMaster.DataBind();
            }
        }

        void dsMaster_DataBound(object sender, EventArgs e)
        {
            DataTable result = GetService().GetMasterPage(0, 0);
            if (result != null && result.Rows.Count == 1)
            {
                string redirectUrl = GetDetailsPageUrl(CheckId,
                                  result.Rows[0].IsNull("GroupId") ? 0 : Convert.ToInt64(result.Rows[0]["GroupId"]));
                redirectUrl = string.Format("{0}&single=1", redirectUrl);

                Response.Redirect(redirectUrl);
                Response.End();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        private ObjectDataSource CreateDataSource()
        {
            resultCountDataSource.TypeName = GetService().GetType().FullName;
            resultCountDataSource.DataObjectTypeName = typeof(int).FullName;
            resultCountDataSource.SelectMethod = "GetMasterRowCount";
            resultCountDataSource.ObjectCreating += InitDataSource;

            var ds = new ObjectDataSource();
            ds.ObjectCreating += InitDataSource;
            ds.Selecting += SetPageOffset;
            ds.TypeName = GetService().GetType().FullName;
            //ds.SelectCountMethod = "GetMasterRowCount";
            ds.SelectMethod = "GetMasterPage";
            //ds.EnablePaging = true;
            //ds.MaximumRowsParameterName = "take";
            //ds.StartRowIndexParameterName = "skip";
            return ds;
        }

        private void SetPageOffset(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["skip"] = this.Page.Request.QueryString["start"] != null ? Convert.ToInt32(this.Page.Request.QueryString["start"]) : 1;
            e.InputParameters["take"] = this.Request.Cookies.Get("count") != null ? Convert.ToInt32(this.Request.Cookies.Get("count").Value) : 10;
        }

        private CertificateCheckHistoryService _service;

        private CertificateCheckHistoryService GetService()
        {
            if (_service == null)
            {
                string login = IsAdmin ? null : CurrentUserName;
                _service = new CertificateCheckHistoryService(login, CheckId);
            }

            return _service;
        }

        private void InitDataSource(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = GetService();
        }

        protected string GetDetailsPageUrl(long id, long groupId)
        {
            string pageName = GetPageName();

            if (pageName == null)
            {
                return "#";
            }

            return string.Format("/Certificates/CommonNationalCertificates/{0}?id={1}&groupId={2}", pageName, id, groupId);
        }

        private string GetPageName()
        {
            IHistoryNavigator page = this.Page as IHistoryNavigator;

            if (page != null)
            {
                return page.GetPageName();
            }

            return null;
        }

        protected void dsMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dsMaster.PageIndex = e.NewPageIndex;
        }
    }
}