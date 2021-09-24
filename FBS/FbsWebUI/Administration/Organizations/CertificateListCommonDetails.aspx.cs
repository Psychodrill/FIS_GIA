using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fbs.Core.Organizations;
using Fbs.Web.Certificates.CommonNationalCertificates;
using FbsServices;

namespace Fbs.Web.Administration.Organizations
{
    public partial class CertificateListCommonDetails : BasePage
    {
        private CertificateCheckHistoryService _service;
        private bool _requestFieldsMapped;

        private long OrgId
        {
            get { return GetParamLong("orgId"); }
        }

        private long GroupId
        {
            get { return GetParamLong("groupId"); }
        }

        private long CheckId
        {
            get { return GetParamLong("checkId"); }
        }

        private bool UniqueOnly
        {
            get { return GetParamInt("unique") == 1; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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

                    var master = (Common.Templates.Administration)this.Master;
                    if (master != null)
                    {
                        master.CaptionToolTip = string.Format(
                            TemplateTitle, string.IsNullOrEmpty(org.FullName) ? "Название не найдено" : org.FullName);
                    }
                }

                searchDetailsGrid.DataSource = CreateDataSource();
                searchDetailsGrid.DataBind();
            }

            backToResultsLink.NavigateUrl = GetResultPageUrl();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (searchDetailsGrid.Items.Count == 0)
            {
                searchDetailsGrid.Visible = false;
                searchDetails.Visible = false;
                noResultPanel.Visible = true;
                noResultLink.Visible = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"]);
                if (noResultLink.Visible)
                {
                    noResultLink.NavigateUrl = GenerateNotFoundPrintLink();
                    if (noResultLink.NavigateUrl == "#")
                    {
                        noResultLink.Visible = false;
                    }
                }
            }
            else
            {
                printNoteLink.NavigateUrl = RenderPrintLink();
            }
        }

        private ObjectDataSource CreateDataSource()
        {
            var ds = new ObjectDataSource();
            ds.ObjectCreating += OnInitDataSource;
            ds.TypeName = typeof(CertificateCheckHistoryService).FullName;
            ds.SelectMethod = "GetDetails";
            ds.Selecting += DsOnSelecting;
            return ds;
        }

        private void DsOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters.Add("groupId", GroupId);
        }

        private void OnInitDataSource(object sender, ObjectDataSourceEventArgs e)
        {
            string login = IsAdmin ? null : CurrentUserName;
            _service = new CertificateCheckHistoryService(login, CheckId);
            e.ObjectInstance = _service;
        }

        protected void dgView_RowBound(object sender, DataGridItemEventArgs e)
        {
            if (!_requestFieldsMapped)
            {
                const string surname = "Surname";
                const string name = "Name";
                const string secondName = "SecondName";
                const string documentNumber = "DocumentNumber";
                const string documentSeries = "DocumentSeries";

                DataRowView rowView = e.Item.DataItem as DataRowView;

                if (rowView != null)
                {
                    resultField_Surname.Text = rowView.Row.IsNull(surname) ? string.Empty : rowView.Row.Field<string>(surname);
                    resultField_Name.Text = rowView.Row.IsNull(name) ? string.Empty : rowView.Row.Field<string>(name);
                    resultField_SecondName.Text = rowView.Row.IsNull(secondName) ? string.Empty : rowView.Row.Field<string>(secondName);
                    resultField_DocumentNumber.Text = rowView.Row.IsNull(documentNumber) ? string.Empty : rowView.Row.Field<string>(documentNumber);
                    resultField_DocumentSeries.Text = rowView.Row.IsNull(documentSeries) ? string.Empty : rowView.Row.Field<string>(documentSeries);
                    _requestFieldsMapped = true;
                }
            }
        }

        protected string GetResultPageUrl()
        {
            return string.Format("CertificateListCommon.aspx?OrgId={0}&unique={1}", OrgId, UniqueOnly ? 1 : 0);
        }

        
        protected string GenerateNotFoundPrintLink()
        {
            return RenderPrintLink();
        }

        protected string RenderMark(DataGridItem container)
        {
            string m = DataBinder.Eval(container, "DataItem.Mark", "{0}");
            if (string.IsNullOrEmpty(m))
            {
                return "&nbsp;";
            }

            string statusName = DataBinder.Eval(container, "DataItem.StatusName", "{0}");

            if (string.IsNullOrEmpty(statusName) || statusName.ToLowerInvariant() != "действующий")
            {
                return string.Format("<span style=\"color: red\">{0}</span>", m);
            }
            return m;
        }

        protected string RenderPrintLink()
        {
            return string.Format("~/Certificates/CommonNationalCertificates/PrintNoteBatchCommon.aspx?checkId={0}&groupId={1}&OrgId={2}", CheckId, GroupId, OrgId);
        }
    }
}