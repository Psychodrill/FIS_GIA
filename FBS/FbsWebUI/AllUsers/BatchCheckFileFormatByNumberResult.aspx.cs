using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace Fbs.Web.AllUsers
{
    public partial class BatchCheckFileFormatByNumberResult : BasePage
    {
        private string FileName
        {
            get { return this.Request.QueryString["FileName"]; }
        }

        private int Start
        {
            get { return this.Request.QueryString["start"] == null ? 1 : Convert.ToInt32(this.Request.QueryString["Start"]); }
        }

        private int Count
        {
            get
            {
                if (this.Request.QueryString["count"] == null)
                {
                    if (this.Request.Cookies.Get("count") == null)
                        return 20;
                    return string.IsNullOrEmpty(this.Request.Cookies.Get("count").Value) ? 20 : Convert.ToInt32(this.Request.Cookies.Get("count").Value);
                }
                return Convert.ToInt32(this.Request.QueryString["count"]);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(FileName))
            {
                var dt = this.Session[FileName] as DataTable;
                if (dt == null)
                    return;
                if (dt.Rows.Count > 0)                    
                    dsResultsListCount.SelectCommand = string.Format("select {0} as cnt", dt.Rows.Count);
                dgResultsList.DataSource = dt.Select().Where(k => Convert.ToInt32(k["RowId"]) >= Start && Convert.ToInt32(k["RowId"]) < Start + Count).CopyToDataTable();
                dgResultsList.DataBind();

            }



        }


        protected void dgResultsList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgResultsList.CurrentPageIndex = e.NewPageIndex;
            dgResultsList.DataBind();
        }

        public bool HasResults
        {
            get { return dgResultsList.Items.Count > 0; }
        }



        public int SelectData
        {
            get
            {
                var dt = this.Session[FileName] as DataTable;
                //if (dt == null) return null;
                //var tdt = new DataTable();
                //var column = new DataColumn {DataType = System.Type.GetType("System.Int32"), ColumnName = "id"};
                //tdt.Columns.Add(column);
                //var row = tdt.NewRow();
                //row["id"] = dt.Rows.Count;
                //tdt.Rows.Add(row);
                return dt.Rows.Count;
            }
        }
    }
}
