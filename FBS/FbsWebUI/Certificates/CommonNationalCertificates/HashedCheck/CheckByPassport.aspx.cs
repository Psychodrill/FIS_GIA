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
    public partial class CheckByPassport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private bool PageIsValid
        {
            get 
            {
                return Request["HPageIsValid"] == "True";
            }
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (PageIsValid)
            {
                string HashedPassportNumber =  Request["TBPassportNumberHash"];
                string HashedPassportSeria = Request["TBPassportSeriaHash"];
                string HashedLastName = Request["TBLastNameHash"];
                string HashedFirstName =  Request["TBFirstNameHash"];
                string HashedPatronymicName =  Request["TBPatronymicNameHash"];

                DataTable FoundedCNEs = Fbs.Core.HashedChecks.CheckDataAccessor.CheckByPassport(HashedPassportSeria, HashedPassportNumber, HashedLastName, HashedFirstName, HashedPatronymicName);
                bool ShowResults = false;
                if (FoundedCNEs.Rows.Count > 0)
                {
                    bool Exists = Convert.ToBoolean(FoundedCNEs.Rows[0]["IsExist"]);
                  if (Exists  )
                    {
                        ShowResults = true;
                    }
                }
             
                if (ShowResults)
                {
                    DGSearch.DataSource = FoundedCNEs;
                    DGSearch.DataBind();
                }
                else
                {
                    pNotExist.Visible = true;
                }
                DWait.Style[HtmlTextWriterStyle.Display] = "none";
            }
        }
    }
}
