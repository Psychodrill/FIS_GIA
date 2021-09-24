using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fbs.Core.CNEChecks;
using Fbs.Web.Extentions;
namespace Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck
{
    public partial class CheckByMarkSum : BasePage
    {
        protected override void OnInit(EventArgs e)
        {
            
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private bool PageIsValid
        {
            get
            {
                return Request["HPageIsValid"] == "True" && this.IsValid;
            }
        }
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (PageIsValid)
            {
                this.DInputs.Visible = false;
                this.DOut.Visible = true;
                int MarkSum = Int32.Parse(Request["TBMarkSum"]);
                string HashedLastName = Request["TBLastNameHash"];
                string HashedFirstName = Request["TBFirstNameHash"];
                string HashedPatronymicName = Request["TBPatronymicNameHash"];
                int orgId = Int32.Parse(this.orgSelector.Value);
                KeyValuePair<int, CheckByMarkSumResult> result = Fbs.Core.HashedChecks.CheckDataAccessor.CheckByMarkSum(HashedFirstName, HashedLastName, HashedPatronymicName, String.Join(",", this.cblSubjects.Items.Cast<ListItem>().Where(x => x.Selected == true).Select(x => x.Value).ToArray()), MarkSum, orgId);
                this.checkResult.Text = result.Value.ToString(true);
                this.checkNumber.Text = result.Key.ToString();
                if (result.Value == CheckByMarkSumResult.Valid)
                    this.btnAbuse.Visible = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}