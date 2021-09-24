using System;
using System.Web;
using System.Text;
using Fbs.Core;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using Fbs.Web.Helpers;
namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByPassportResult : BasePage
    {
        public bool HasResults
        {
            get { return dgResultsList.Items.Count > 0; }
        }

        protected override void OnInit(EventArgs e)
        {
            this.dgResultsList.ItemCommand += new DataGridCommandEventHandler(dgResultsList_ItemCommand);
            this.dgResultsList.ItemDataBound += new DataGridItemEventHandler(dgResultsList_ItemDataBound);
            base.OnInit(e);
        }

        private int? _commandIndex;
        void dgResultsList_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (this._commandIndex.HasValue && this._commandIndex == e.Item.ItemIndex)
            {
                
                var dataRow = e.Item.DataItem as DataRowView;
                Dictionary<String, string> values = new Dictionary<string, string>();
                //return String.Format("PrintNotFoundNote.aspx?SubjectMarks={0}&number={1}&check=byNumber", HttpUtility.UrlEncode(this.Request.QueryString["SubjectMarks"]), this.Request.QueryString["number"]);
                values.Add("Series", dataRow["PassportSeria"].ToString());
                values.Add("PassportNumber", dataRow["PassportNumber"].ToString());
                values.Add("FirstName", dataRow["FirstName"].ToString());
                values.Add("LastName", dataRow["LastName"].ToString());
                values.Add("GivenName", dataRow["PatronymicName"].ToString());
                
                this.Session["NoteInfo"] = values;
                this.Response.Redirect("PrintNotFoundNote.aspx", true);
            }
        }

        void dgResultsList_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "PrintNotFound")
            {
                this._commandIndex = e.Item.ItemIndex;
                this.dgResultsList.DataBind();
            }

        }
    }
}
