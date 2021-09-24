namespace Fbs.Web.AllUsers
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The batch check file format by passport result.
    /// </summary>
    public partial class BatchCheckFileFormatByPassportResult : BasePage
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether HasResults.
        /// </summary>
        public bool HasResults
        {
            get
            {
                return this.dgResultsList.Items.Count > 0;
            }
        }

        #endregion

        #region Properties

        private int Count
        {
            get
            {
                if (this.Request.QueryString["count"] == null)
                {
                    if (this.Request.Cookies.Get("count") == null)
                    {
                        return 20;
                    }

                    return string.IsNullOrEmpty(this.Request.Cookies.Get("count").Value)
                               ? 20
                               : Convert.ToInt32(this.Request.Cookies.Get("count").Value);
                }

                return Convert.ToInt32(this.Request.QueryString["count"]);
            }
        }

        private string FileName
        {
            get
            {
                return this.Request.QueryString["FileName"];
            }
        }

        private int Start
        {
            get
            {
                return this.Request.QueryString["start"] == null
                           ? 1
                           : Convert.ToInt32(this.Request.QueryString["Start"]);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string FileName = this.Request.QueryString["FileName"];
            if (!string.IsNullOrEmpty(FileName))
            {
                var dt = this.Session[FileName] as DataTable;
                if (dt == null)
                {
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    this.dsResultsListCount.SelectCommand = string.Format("select {0} as cnt", dt.Rows.Count);
                }

                this.dgResultsList.DataSource =
                    dt.Select().Where(
                        k =>
                        Convert.ToInt32(k["RowId"]) >= this.Start
                        && Convert.ToInt32(k["RowId"]) < this.Start + this.Count).CopyToDataTable();
                this.dgResultsList.DataBind();
            }
        }

        /// <summary>
        /// The dg results list_ page index changed.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void dgResultsList_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.dgResultsList.CurrentPageIndex = e.NewPageIndex;
            this.dgResultsList.DataBind();
        }

        #endregion
    }
}