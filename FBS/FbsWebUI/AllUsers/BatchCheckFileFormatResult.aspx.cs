using System;
using System.Web.UI.WebControls;

namespace Fbs.Web.AllUsers
{
    public partial class BatchCheckFileFormatResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
      
                string FileName = this.Request.QueryString["FileName"];
                if (!String.IsNullOrEmpty(FileName))
                {
                    // Если была ошибка "Maximum request length exceeded"
                    dgResultsList.DataSource = this.Session[FileName];
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
            get {return dgResultsList.Items.Count > 0;}
        }
    }
}
