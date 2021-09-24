using System.Configuration;

namespace Fbs.Web.Common.Templates
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Fbs.Web.Helpers;
    using Fbs.Web.Certificates.CommonNationalCertificates;
    using Fbs.Core.UICheckLog;

    /// <summary>
    /// Общий обработчик запросов интерактивных проверок любых типов
    /// </summary>
    public partial class CertificatesResult : MasterPage
    {
        private bool _requestFieldsMapped;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlDataSource search = GetQuerySource();
                search.Selecting += dsSearch_Selecting;
                DataView searchResults = search.Select(new DataSourceSelectArguments()) as DataView;
                search.Selecting -= dsSearch_Selecting;

                if (searchResults != null)
                {
                    DataTable master = DistinctParticipants(searchResults);

                    //searchMasterGrid.DataSource = master;
                    //searchMasterGrid.DataBind();

                    if (master.Rows.Count == 1)
                    {
                        showMasterGroupsLink.Visible = false;
                        ShowDetailsGroup(searchResults, master.Rows[0]["GroupId"]);
                    }
                    else if (master.Rows.Count > 1)
                    {
                        searchMasterGrid.DataSource = master;
                        searchMasterGrid.DataBind();
                    }
                } 
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (searchMasterGrid.DataSource == null && searchDetailsGrid.DataSource == null)
            {
                noResultPanel.Visible = true;
                noResultLink.Visible = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableNotFoundNote"]);

                if (noResultLink.Visible)
                {
                    noResultLink.NavigateUrl = GenerateNotFoundPrintLink();
                    noResultLink.Visible = !string.IsNullOrEmpty(noResultLink.NavigateUrl);
                }

                searchMasterGrid.Visible = false;
                searchDetails.Visible = false;
                searchDetailsGrid.Visible = false;
            }
            else
            {
                noResultPanel.Visible = false;

                searchMasterGrid.Visible = searchMasterGrid.DataSource != null;
                searchDetailsGrid.Visible = !searchMasterGrid.Visible;
                searchDetails.Visible = searchDetailsGrid.Visible;
            }
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
                const string certificateId = "CertificateId";

                DataRowView rowView = e.Item.DataItem as DataRowView;

                if (rowView != null)
                {
                    resultField_Surname.Text = rowView.Row.IsNull(surname) ? string.Empty : rowView.Row.Field<string>(surname);
                    resultField_Name.Text = rowView.Row.IsNull(name) ? string.Empty : rowView.Row.Field<string>(name);
                    resultField_SecondName.Text = rowView.Row.IsNull(secondName) ? string.Empty : rowView.Row.Field<string>(secondName);
                    resultField_DocumentNumber.Text = rowView.Row.IsNull(documentNumber) ? string.Empty : rowView.Row.Field<string>(documentNumber);
                    resultField_DocumentSeries.Text = rowView.Row.IsNull(documentSeries) ? string.Empty : rowView.Row.Field<string>(documentSeries);
                    _requestFieldsMapped = true;

                    if (!rowView.Row.IsNull(certificateId))
                    {
                        UpdateCNELogItem(rowView.Row[certificateId].ToString());
                    }
                }
            }
        }

        private bool _CNELogItemUpdated = false;
        private void UpdateCNELogItem(string certificateId)
        {
            if (_CNELogItemUpdated)
                return;
            if (!string.IsNullOrEmpty(this.Request.QueryString["Ev"]))
            {
                CheckLogDataAccessor.UpdateCheckEvent(this.Request.QueryString["Ev"], certificateId);
                _CNELogItemUpdated = true;
            }
        }

        protected string GenerateNotFoundPrintLink()
        {
            ICheckResultCommonBase dataSource = Page as ICheckResultCommonBase;

            if (dataSource != null)
            {
                Dictionary<string, string> values = dataSource.GetNotFoundPrintData();
                if (values != null && values.Count > 0)
                {
                    Session["NoteInfo"] = values;
                    return "~/Certificates/CommonNationalCertificates/PrintNotFoundNote.aspx";
                }
            }

            return null;
        }

        protected void ShowDetailsGroupExecuted(object sender, CommandEventArgs e)
        {
            ShowDetailsGroup(null, e.CommandArgument);
        }

        private SqlDataSource GetQuerySource()
        {
            return ((ICheckResultCommonBase)Page).GetQuerySource();
        }

        private void dsSearch_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.CommandTimeout = 90;
        }

        private void ShowDetailsGroup(DataView searchResults, object groupId)
        {
            DataView details = searchResults ?? GetQuerySource().Select(new DataSourceSelectArguments()) as DataView;

            if (details != null && details.Count > 0)
            {
                details.RowFilter = string.Format("GroupId = {0}", groupId);
                searchDetailsGrid.DataSource = details;
                searchDetailsGrid.DataBind();
                Guid printTicket = SavePrintNoteData(details);
                printNoteLink.NavigateUrl = "~/Certificates/CommonNationalCertificates/PrintNoteCommon.aspx?id=" + printTicket;
            }
        }

        private Guid SavePrintNoteData(DataView source)
        {
            Guid id = Guid.NewGuid();
            PrintNoteData data = PrintNoteData.Parse(source);
            Session["CertificateInfo"] = new KeyValuePair<Guid, PrintNoteData>(id, data);
            return id;
        }

        private static DataTable DistinctParticipants(DataView sourceView)
        {
            DataColumn[] schema = new[]
                {
                    new DataColumn("GroupId", typeof (int)),
                    new DataColumn("Surname"),
                    new DataColumn("Name"),
                    new DataColumn("SecondName"),
                    new DataColumn("DocumentSeries"),
                    new DataColumn("DocumentNumber")
                };

            DataTable table = new DataTable();
            table.Columns.AddRange(schema);
            table.PrimaryKey = new[] { table.Columns[0] };

            HashSet<int> processed = new HashSet<int>();

            foreach (DataRowView sourceRowView in sourceView)
            {
                int sourceGroupId = sourceRowView.Row.Field<int>(table.PrimaryKey[0].ColumnName);

                if (!processed.Contains(sourceGroupId))
                {
                    DataRow targetRow = table.NewRow();

                    foreach (DataColumn targetColumn in table.Columns)
                    {
                        if (sourceRowView.Row.Table.Columns.Contains(targetColumn.ColumnName) && !sourceRowView.Row.IsNull(targetColumn.ColumnName))
                        {
                            targetRow[targetColumn] = sourceRowView.Row[targetColumn.ColumnName];
                        }
                    }

                    table.Rows.Add(targetRow);
                    processed.Add(sourceGroupId);
                }
            }

            //table.AcceptChanges();
            return table;
        }

        protected void ShowMasterGroupsExecuted(object sender, CommandEventArgs e)
        {
            searchDetailsGrid.DataSource = null;
            SqlDataSource search = GetQuerySource();
            search.Selecting += dsSearch_Selecting;
            DataView searchResults = search.Select(new DataSourceSelectArguments()) as DataView;
            search.Selecting -= dsSearch_Selecting;
            DataTable master = DistinctParticipants(searchResults);

            searchMasterGrid.DataSource = master;
            searchMasterGrid.DataBind();
        }

        protected void searchMasterGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            searchMasterGrid.CurrentPageIndex = e.NewPageIndex;
            SqlDataSource search = GetQuerySource();
            search.Selecting += dsSearch_Selecting;
            DataView searchResults = search.Select(new DataSourceSelectArguments()) as DataView;
            search.Selecting -= dsSearch_Selecting;

            if (searchResults != null)
            {
                DataTable master = DistinctParticipants(searchResults);
                searchMasterGrid.DataSource = master;
                searchMasterGrid.DataBind();
            }
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