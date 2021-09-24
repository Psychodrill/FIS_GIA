using System;
using System.Web.UI.WebControls;
using Fbs.Web.Certificates.CommonNationalCertificates;
using FbsServices;
using Fbs.Web.Helpers;

namespace Fbs.Web.Common.Templates
{
    public partial class CertificatesBatchResult : BaseMasterPage
    {
        private CertificateBatchCheckHistoryService _service;

        protected long BatchId
        {
            get { return GetParamLong("batchId"); }
        }

        protected long CheckId
        {
            get { return GetParamLong("id"); }
        }

        protected bool BatchWait { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string login = IsAdmin ? null : CurrentUserName;

                _service = new CertificateBatchCheckHistoryService(login, CheckId);

                if (BatchId > 0)
                {
                    if (_service.CheckBatchReady(BatchId, GetCheckType()))
                    {
                        Response.Redirect(this.Page.Request.Url.LocalPath + "?id=" + _service.BatchCheckId);
                        Response.End();
                    }
                    else
                    {
                        BatchWait = true;
                    }

                    DataSourcePagerHead.DataSourceId = null;
                }
                else if (CheckId > 0)
                {
                    DataSourcePagerHead.DataSourceId = resultCountDataSource.ID;
                    resultCountDataSource.TypeName = _service.GetType().FullName;
                    resultCountDataSource.DataObjectTypeName = typeof (int).FullName;
                    resultCountDataSource.SelectMethod = "GetRecordsCount";
                    resultCountDataSource.ObjectCreating += InitDataSource;

                    ObjectDataSource ds = new ObjectDataSource();
                    ds.TypeName = _service.GetType().FullName;
                    ds.SelectMethod = "GetRecordsPage";
                    //ds.SelectCountMethod = "GetRecordsCount";
                    //ds.StartRowIndexParameterName = "skip";
                    //ds.MaximumRowsParameterName = "take";
                    //ds.EnablePaging = true;
                    ds.ObjectCreating += InitDataSource;
                    ds.Selecting += SetPageOffset;
                    dsMaster.DataSource = ds;
                    dsMaster.DataBind();
                }
            }
        }

        private void SetPageOffset(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["skip"] = this.Page.Request.QueryString["start"] != null ? Convert.ToInt32(this.Page.Request.QueryString["start"]) : 1;
            e.InputParameters["take"] = this.Request.Cookies.Get("count") != null ? Convert.ToInt32(this.Request.Cookies.Get("count").Value) : 10;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (BatchWait)
            {
                exportPanel.Visible = false;
            }
        }
        private void InitDataSource(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = _service;
        }

        protected void dsMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dsMaster.PageIndex = e.NewPageIndex;
        }

        protected string GetDetailsPageUrl(object groupId)
        {
            string pageName = GetPageName();

            if (string.IsNullOrEmpty(pageName))
            {
                return "#";
            }

            return string.Format("~/Certificates/CommonNationalCertificates/{0}?id={1}&groupId={2}", pageName, CheckId, groupId);
        }

        protected string GetPageName()
        {
            IHistoryNavigator page = Page as IHistoryNavigator;

            return page == null ? null : page.GetPageName();
        }

        protected int GetCheckType()
        {
            IBatchCheck page = Page as IBatchCheck;

            if (page != null)
            {
                return (int)page.CheckType;
            }

            return 0;
        }

        protected string RenderMark(int? globalResultId, int? mark, int? subjectCode)
        {
            if (!globalResultId.HasValue)
                return String.Empty;

            string markFormat = "{0}";
            if (globalResultId != 1)
            {
                markFormat = "<span style=\"color: red\">{0}</span>";
            }
            string markString;
            if (SubjectsHelper.SubjectHasBoolMark(subjectCode.Value))
            {
                markString = SubjectsHelper.BoolMarkToText(mark.Value);
            }
            else
            {
                markString = mark.ToString();
            }
            return String.Format(markFormat, markString);
        }
    }
}