using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fbs.Web.Certificates.CommonNationalCertificates;
using Fbs.Web.Helpers;
using FbsServices;

namespace Fbs.Web.Common.Templates
{
    public partial class CertificateResultHistoryDetails : BaseMasterPage
    {
        private bool _requestFieldsMapped;
        private CertificateCheckHistoryService _service;

        private long GroupId
        {
            get { return Convert.ToInt64(Request.QueryString["groupId"]); }
        }

        private long CheckId
        {
            get { return Convert.ToInt64(Request.QueryString["id"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                searchDetailsGrid.DataSource = CreateDataSource();
                searchDetailsGrid.DataBind();
            }

            backToResultsLink1.NavigateUrl = GetResultPageUrl();
            backToResultsLink2.NavigateUrl = GetResultPageUrl();
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

            if (GetParamInt("single") == 1)
            {
                backToResultsLink1.Visible = false;
                backToResultsLink2.Visible = false;
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
                    Guid printTicket = SavePrintNoteData(_service.GetDetails(GroupId));
                    printNoteLink.NavigateUrl = "~/Certificates/CommonNationalCertificates/PrintNoteCommon.aspx?id=" + printTicket;
                }
            }
        }

        private Guid SavePrintNoteData(DataTable source)
        {
            Guid id = Guid.NewGuid();
            PrintNoteData data = PrintNoteData.Parse(new DataView(source));
            Session["CertificateInfo"] = new KeyValuePair<Guid, PrintNoteData>(id, data);
            return id;
        }

        protected string GetResultPageUrl()
        {
            string pageName = GetPageName();

            if (pageName == null)
            {
                return "#";
            }

            return string.Format("/Certificates/CommonNationalCertificates/{0}?id={1}", pageName, CheckId);
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

        protected string GenerateNotFoundPrintLink()
        {
            DataTable notFoundData = _service.GetNotFoundData();

            if (notFoundData != null && notFoundData.Rows.Count > 0)
            {
                DataRow row = notFoundData.Rows[0];
                Dictionary<string, string> values = new Dictionary<string, string>
                    {
                        {"CertNumber", row.IsNull("LicenseNumber") ? string.Empty : row.Field<string>("LicenseNumber")},
                        {"GivenName", row.IsNull("SecondName") ? string.Empty : row.Field<string>("SecondName")},
                        {"FirstName", row.IsNull("Name") ? string.Empty : row.Field<string>("Name")},
                        {"LastName", row.IsNull("Surname") ? string.Empty : row.Field<string>("Surname")},
                        {"PassportNumber", row.IsNull("DocumentNumber") ? string.Empty : row.Field<string>("DocumentNumber")},
                        {"Series", row.IsNull("DocumentSeries") ? string.Empty : row.Field<string>("DocumentSeries")},
                        {"TypographicNumber", row.IsNull("TypographicNumber") ? string.Empty : row.Field<string>("TypographicNumber")}
                    };

                Session["NoteInfo"] = values;
                return "~/Certificates/CommonNationalCertificates/PrintNotFoundNote.aspx";
            }

            return "#";
        }

        protected string RenderMark(int globalResultId, int mark, int subjectCode)
        {
            string markFormat = "{0}";
            if (globalResultId != 1)
            {
                markFormat = "<span style=\"color: red\">{0}</span>";
            }
            string markString;
            if (SubjectsHelper.SubjectHasBoolMark(subjectCode))
            {
                markString = SubjectsHelper.BoolMarkToText(mark);
            }
            else
            {
                markString = mark.ToString();
            }
            return String.Format(markFormat, markString);
        } 
    }
}