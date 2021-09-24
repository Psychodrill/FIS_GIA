using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FbsServices;
using Fbs.Web.Helpers;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByPassportResultObsolete : BasePage
    {   
        private CertificateBatchCheckHistoryService _service;

        private CertificateBatchCheckHistoryService GetService()
        {
            return _service ??
                   (_service = new CertificateBatchCheckHistoryService(IsAdmin ? null : CurrentUserName, CheckId));
        }

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
                if (BatchId > 0)
                {
                    if (GetService().CheckBatchReady(BatchId, GetCheckType()))
                    {
                        Response.Redirect(this.Page.Request.Url.LocalPath + "?id=" + GetService().BatchCheckId);
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
                    resultCountDataSource.TypeName = GetService().GetType().FullName;
                    resultCountDataSource.DataObjectTypeName = typeof(int).FullName;
                    resultCountDataSource.SelectMethod = "GetRecordsCount";
                    resultCountDataSource.ObjectCreating += InitDataSource;

                    ObjectDataSource ds = new ObjectDataSource();
                    ds.TypeName = GetService().GetType().FullName;
                    ds.SelectMethod = "GetRecordsPageObsolete";
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
            e.ObjectInstance = GetService();
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
            return "BatchRequestByPassportResultObsoleteDetails.aspx";
        }

        protected string RenderSubjectMark(IDataItemContainer container, string subjectColumn)
        {
            string field = string.Format("DataItem.{0}", subjectColumn);

            int? globalResultId = DataBinder.Eval(container, "DataItem.GlobalStatusID") as int?;
            string markStr = DataBinder.Eval(container, string.Format("DataItem.{0}", subjectColumn)) as string;
            int? mark =null;
            int temp;
            if ((!String.IsNullOrEmpty(markStr)) && (Int32.TryParse(markStr, out temp)))
            {
                mark = temp;
            }

            int subjectCode = Int32.Parse(subjectColumn.Replace("sbj", ""));
            return RenderMark(globalResultId, mark, subjectCode); 
        }

        protected string RenderMark(int? globalResultId, int? mark, int subjectCode)
        {
            if (!globalResultId.HasValue)
                return String.Empty;

            string markFormat = "{0}";
            if (globalResultId != 1)
            {
                markFormat = "<span style=\"color: red\">{0}</span>";
            }
            string markString;
            if (mark.HasValue)
            {
                if (SubjectsHelper.SubjectHasBoolMark(subjectCode))
                {
                    markString = SubjectsHelper.BoolMarkToText(mark.Value);
                }
                else
                {
                    markString = mark.ToString();
                }
            }
            else
            {
                markString = "&nbsp;";
            }
            return String.Format(markFormat, markString);
        } 

        protected string RenderSubjectAppeal(IDataItemContainer container, string subjectApColumn)
        {
            string field = string.Format("DataItem.{0}", subjectApColumn);

            string m = DataBinder.Eval(container, field, "{0}");
            if (string.IsNullOrEmpty(m))
            {
                return "Нет";
            }

            return m == "1" ? "Да" : "Нет";
        }

        private int GetCheckType()
        {
            return (int)CommonCheckType.DocumentNumber;
        }

        protected string RenderLicenseNumber(IDataItemContainer container)
        {
            string statusName = DataBinder.Eval(container, "DataItem.StatusName", "{0}");
            if (!string.IsNullOrEmpty(statusName) && statusName.ToLower().Trim() == "не найдено")
            {
                return statusName.ToLower().Trim(); // в поле свид-во пишем "не найдено"
            }

            string licenseNumber = DataBinder.Eval(container, "DataItem.LicenseNumber", "{0}");

            if (string.IsNullOrEmpty(licenseNumber))
            {
                return "нет свидетельства"; // в поле свид-во пишем "нет свидетельства"
            }
            
            return licenseNumber; // в поле свид-во пишем номер свид-ва
        }

        protected string RenderTypographicNumber(IDataItemContainer container)
        {
            string statusName = DataBinder.Eval(container, "DataItem.StatusName", "{0}");

            if (!string.IsNullOrEmpty(statusName) && statusName.ToLower().Trim() == "не найдено")
            {
                return "&nbsp;"; // поле ТН пустое
            }

            string licenseNumber = DataBinder.Eval(container, "DataItem.LicenseNumber", "{0}");

            if (string.IsNullOrEmpty(licenseNumber))
            {
                return "&nbsp;"; // поле ТН пустое
            }

            return DataBinder.Eval(container, "DataItem.TypographicNumber", "{0}"); // в поле ТН пишем типографский номер
        }
    }
}