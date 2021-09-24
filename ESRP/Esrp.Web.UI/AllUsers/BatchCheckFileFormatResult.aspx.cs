using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace Esrp.Web.Certificates.CommonNationalCertificates
{
    public partial class BatchCheckFileFormatResult : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
      
                string FileName = Request.QueryString["FileName"];
                if (!String.IsNullOrEmpty(FileName))
                {
                    // Если была ошибка "Maximum request length exceeded"
                    dgResultsList.DataSource = Session[FileName];
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
