using System.Web.UI;
using System.Xml;
using Fbs.Web.Certificates.CommonNationalCertificates;

namespace Fbs.Web.Administration.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;

    using Fbs.Core;
    using Fbs.Core.CNEChecks;
    using Fbs.Core.Organizations;
    using Fbs.Utility;
    using Fbs.Web.Common.Templates;

    using FbsServices;

    using FbsWebViewModel.CNEC;

    /// <summary>
    ///     История проверок свидетельств организацией
    /// </summary>
    public partial class CertificateListCommon : BasePage
    {
        private OrganizationCheckHistoryService _service;
        private long OrgId
        {
            get { return GetParamLong("OrgID"); }
        }

        protected bool UniqueOnly
        {
            get { return GetParamBool("unique"); }
        }

        private bool GetParamBool(string fieldName)
        {
            string rawValue = Request.QueryString[fieldName];
            
            if (!string.IsNullOrEmpty(rawValue))
            {
                return XmlConvert.ToBoolean(rawValue);
            }

            return false;
        }

        private int Skip
        {
            get
            {
                return Request.QueryString["start"] != null ? Convert.ToInt32(Request.QueryString["start"]) : 0;
            }
        }

        private int Take
        {
            get
            {
                return Request.Cookies.Get("count") != null ? Convert.ToInt32(Request.Cookies.Get("count").Value) : 10;
            }
        }

        private OrganizationCheckHistoryService GetService()
        {
            return _service ?? (_service = new OrganizationCheckHistoryService(OrgId, UniqueOnly));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                uniqueChecks.Checked = UniqueOnly;

                // Запрашиваем данные по организации чтобы узнать ее имя
                Organization org = OrganizationDataAccessor.Get(OrgId);
                
                const string TemplateTitle = "История проверок свидетельств организацией \"{0}\"";
                this.PageTitle = string.Format(TemplateTitle, "Организация не найдена");
                if (org != null)
                {
                    if (string.IsNullOrEmpty(org.ShortName))
                    {
                        this.PageTitle = string.IsNullOrEmpty(org.FullName)
                                             ? string.Format(TemplateTitle, "Название не найдено")
                                             : string.Format(
                                                 TemplateTitle,
                                                 org.FullName.Length > 40 ? org.FullName.Remove(40) : org.FullName);
                    }
                    else
                    {
                        this.PageTitle = string.Format(TemplateTitle, org.ShortName);
                    }

                    var master = (Administration)this.Master;
                    if (master != null)
                    {
                        master.CaptionToolTip = string.Format(
                            TemplateTitle, string.IsNullOrEmpty(org.FullName) ? "Название не найдено" : org.FullName);
                    }
                }
            }
        }

        protected void OnCreateDataSource(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = GetService();
        }

        protected void OnSelectData(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["skip"] = Skip;
            e.InputParameters["take"] = Take;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (gvChecks.Rows.Count == 0)
            {
                printAllPanel.Visible = false;
                uniqueChecks.Visible = false;
            }
            else
            {
                lnkPrintAllHtml.NavigateUrl = RenderPrintAllHtmlLink();
                lnkPrintAllWord.NavigateUrl = RenderPrintAllWordLink();
            }
        }

        protected void OnUniqueCheckSelected(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Administration/Organizations/CertificateListCommon.aspx?OrgID={0}&unique={1}", OrgId, uniqueChecks.Checked ? 1 : 0));
        }

        protected string RenderPrintLink(IDataItemContainer container)
        {
            object checkId = DataBinder.Eval(container, "DataItem.CheckId");
            object groupId = DataBinder.Eval(container, "DataItem.GroupId");
            if ((groupId == null) || (groupId == DBNull.Value))
            {
                groupId = 0;
            }
            if (checkId != null && checkId != DBNull.Value && groupId != null && groupId != DBNull.Value)
            {
                return string.Format("~/Certificates/CommonNationalCertificates/PrintNoteBatchCommon.aspx?checkId={0}&groupId={1}&OrgId={2}", Convert.ToInt64(checkId), Convert.ToInt64(groupId), OrgId);
            }

            return "#";
        }

        protected string RenderPrintAllHtmlLink()
        {
            return string.Format("BatchPrintNotes.aspx?OrgId={0}&unique={1}&skip={2}&take={3}", OrgId, UniqueOnly ? 1 : 0, Skip, Take);
        }

        protected string RenderPrintAllWordLink()
        {
            return string.Format("BatchPrintNotes.aspx?OrgId={0}&word=1&unique={1}&skip={2}&take={3}", OrgId, UniqueOnly ? 1 : 0, Skip, Take);
        }

        protected string RenderDetailsLink(IDataItemContainer container)
        {
            return string.Format("CertificateListCommonDetails.aspx?orgId={0}&checkId={1}&groupId={2}&unique={3}", OrgId, DataBinder.Eval(container, "DataItem.CheckId"), DataBinder.Eval(container, "DataItem.GroupId"), uniqueChecks.Checked ? 1 : 0);
        }
    }
}