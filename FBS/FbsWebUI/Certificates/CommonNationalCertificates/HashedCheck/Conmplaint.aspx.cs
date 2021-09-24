namespace Fbs.Web.Certificates.CommonNationalCertificates.HashedCheck
{
    using System;
    using System.Web.UI;
    using Fbs.Core.Organizations;

    /// <summary>
    /// Форма жалобы
    /// </summary>
    public partial class Conmplaint : Page
    {

        protected override void OnLoad(EventArgs e)
        {
            this.TBFirstName.Text = this.Request["hFirstName"];
            this.TBLastName.Text = this.Request["hLastName"];
            this.TBPatronymicName.Text = this.Request["hGivenName"];
            this.TBEducationPlace.Text = OrganizationDataAccessor.Get(int.Parse(this.Request["hOrgId"])).FullName;
            this.TBRequestNumber.Text = this.Request["hCheckNumber"];
            base.OnLoad(e);
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}