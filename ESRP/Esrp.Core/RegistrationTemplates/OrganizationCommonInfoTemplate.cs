namespace Esrp.Core.RegistrationTemplates
{
    using System.IO;

    using Esrp.Core.Users;
    using Esrp.Utility;

    /// <summary>
    /// The organization common info template.
    /// </summary>
    public abstract class OrganizationCommonInfoTemplate : CustomTeplateBaseWordDocument
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationCommonInfoTemplate"/> class.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="headerBlock">
        /// The header block.
        /// </param>
        /// <param name="templateName">
        /// The template name.
        /// </param>
        protected OrganizationCommonInfoTemplate(OrgUser user, string headerBlock, string templateName)
            : base(templateName)
        {
            if (!string.IsNullOrEmpty(headerBlock))
            {
                this.InitializeFields("HeaderBlock", headerBlock);
            }

            this.InitializeFields("Organization", user.RequestedOrganization.FullName);
            this.InitializeFields("OrganizationType", user.RequestedOrganization.OrgType.Name);
            this.InitializeFields("Region", user.RequestedOrganization.Region.Name);
            this.InitializeFields("Founder", user.RequestedOrganization.OwnerDepartment);
            this.InitializeFields("Address", user.RequestedOrganization.LawAddress);
            this.InitializeFields("OrganizationChief", user.RequestedOrganization.DirectorFullName);
            this.InitializeFields("Fax", user.RequestedOrganization.Fax);
            this.InitializeFields("ChiefPhone", user.RequestedOrganization.Phone);

            this.InitializeFields(
                "RCModelName",
                user.RequestedOrganization.RCModelName != string.Empty
                    ? user.RequestedOrganization.RCModelName
                    : user.RequestedOrganization.RCDescription);

            this.Error += ErrorHandler;
        }

        #endregion

        #region Methods

        private static void ErrorHandler(object sender, ErrorEventArgs e)
        {
            LogManager.Error(e.GetException());
        }

        #endregion
    }
}