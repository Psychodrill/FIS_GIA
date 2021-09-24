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
    public partial class CheckByNumber : BasePage
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
                string CNENumber =  Request["TBNumber"];
                string HashedLastName = Request["TBLastNameHash"];
                string HashedFirstName =  Request["TBFirstNameHash"];
                string HashedPatronymicName =  Request["TBPatronymicNameHash"];
                string Marks = CollectMarks();

                DataTable FoundedCNE = Fbs.Core.HashedChecks.CheckDataAccessor.CheckByNumber(CNENumber, HashedLastName, HashedFirstName, HashedPatronymicName, Marks);
                bool ShowResults = false;
                if (FoundedCNE.Rows.Count > 0)
                {
                    bool Exists =Convert.ToBoolean (FoundedCNE.Rows[0]["IsExist"]);
                    if (Exists)
                    {
                        ShowResults = true;
                    }
                }


               // LNumber.Text = CNENumber;
                if (ShowResults)
                {
                    LYear.Text = FoundedCNE.Rows[0]["Year"].ToString();
                    LRegion.Text = FoundedCNE.Rows[0]["RegionName"].ToString();
                    LStatus.Text = FoundedCNE.Rows[0]["Status"].ToString();
                    
                    LTyphNumber.Text = FoundedCNE.Rows[0]["TypographicNumber"].ToString();
                    DGSubjects.DataSource = FoundedCNE;
                    DGSubjects.DataBind();
                   TResults.Visible = true;
                   RowTyphNumber.Visible = true ;
                }
                else
                {
                    pNotExist.Visible = true;
                }
                rpSubjects.Visible = false;

                DWait.Style[HtmlTextWriterStyle.Display] = "none";
            }
        }

        private string CollectMarks()
        {
            StringBuilder Marks = new StringBuilder();
            foreach (RepeaterItem item in rpSubjects.Items)
            {
                HiddenField hfId = item.FindControl("hfId") as HiddenField;
                TextBox txtValue = item.FindControl("txtValue") as TextBox;
                string value;
                if (hfId != null && txtValue != null && !String.IsNullOrEmpty(value = txtValue.Text.Trim()))
                {
                    float mark;
                     
                    if (float.TryParse(value.Replace(',', '.'),
                                       NumberStyles.Float,
                                       NumberFormatInfo.InvariantInfo,
                                       out mark))
                    {
                        Marks.AppendFormat("{0}={1},", hfId.Value, mark.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }

            return Marks.ToString().Trim(',');
        }
    }
}
