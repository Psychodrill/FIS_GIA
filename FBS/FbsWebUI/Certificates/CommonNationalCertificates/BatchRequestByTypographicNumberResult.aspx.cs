using System;
using System.Web;
using System.Text;
using Fbs.Core;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchRequestByTypographicNumberResult : BasePage
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

                values.Add("TypographicNumber", dataRow["TypographicNumber"].ToString());
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
