using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;

namespace Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck
{
    public partial class ResultByNumberAndLastName : BasePage
    {
        private string CNENumber
        {
            get { return Request.QueryString["Number"]; }
        }
        private string LastNameHash
        {
            get { return Request.QueryString["LastName"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(CNENumber) || String.IsNullOrEmpty(LastNameHash))
            {
                ProcessNotFound();
            }

            DataTable FoundedCNE = Fbs.Core.HashedChecks.CheckDataAccessor.CheckByNumber(CNENumber, LastNameHash, null , null , null );
            bool ShowResults = false;
            if (FoundedCNE.Rows.Count > 0)
            {
                bool Exists = Convert.ToBoolean(FoundedCNE.Rows[0]["IsExist"]);
                if (Exists)
                {
                    ShowResults = true;
                }
            }

          
            if (ShowResults)
            {
                LNumber.Text = CNENumber;
                
                LYear.Text = FoundedCNE.Rows[0]["Year"].ToString();
                LRegion.Text = FoundedCNE.Rows[0]["RegionName"].ToString();
                LStatus.Text = FoundedCNE.Rows[0]["Status"].ToString();

                LTypographicNumber.Text = FoundedCNE.Rows[0]["TypographicNumber"].ToString();
                DGSubjects.DataSource = FoundedCNE;
                DGSubjects.DataBind();

                ProcessFound();
            }
            else
            {
                ProcessNotFound();
            }
        }

        private void ProcessFound()
        {
            TResults.Visible = true;
            pNotExist.Visible = false ;
        }

        private void ProcessNotFound()
        {
            TResults.Visible = false;
            pNotExist.Visible = true;
        }
    }
}
