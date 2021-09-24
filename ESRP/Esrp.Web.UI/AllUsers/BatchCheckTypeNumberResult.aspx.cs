using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Esrp.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchCheckTypeNumberResult : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            string fileName = Request.QueryString["FileName"];
            if (!String.IsNullOrEmpty(fileName))
            {
                // Если была ошибка "Maximum request length exceeded"
                dgResultsList.DataSource = Session[fileName];
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
