using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Fbs.Utility;
using System.IO;
using Fbs.Core.Users;

namespace Fbs.Core
{
    public class AccountRegistrationDocument : CustomTeplateBaseWordDocument
    {
        public AccountRegistrationDocument(UserAccount account)
            : base(System.Configuration.ConfigurationSettings.AppSettings["RegistrationDocumentTemplateFileName"])
        {
            InitializeFields("Organization", account.OrganizationName);
            InitializeFields("Region", account.OrganizationRegionName);
            InitializeFields("Founder", account.OrganizationFounderName);
            InitializeFields("Address", account.OrganizationAddress);
            InitializeFields("OrganizationChief", account.OrganizationChiefName);
            InitializeFields("Fax", account.OrganizationFax);
            InitializeFields("ChiefPhone", account.OrganizationPhone);
            InitializeFields("User", string.Format("{0} {1} {2}",
                    account.LastName, account.FirstName, account.PatronymicName));
            InitializeFields("UserPhone", account.Phone);
            InitializeFields("UserEmail", account.Email);

            this.Error += ErrorHandler;
        }

        public AccountRegistrationDocument(OrgUser user)
            : base(System.Configuration.ConfigurationSettings.AppSettings["RegistrationDocumentTemplateFileName"])
        {
            InitializeFields("Organization", user.RequestedOrganization.FullName);
            InitializeFields("Region", user.RequestedOrganization.Region.Name);
            InitializeFields("Founder", user.RequestedOrganization.OwnerDepartment);
            InitializeFields("Address", user.RequestedOrganization.LawAddress);
            InitializeFields("OrganizationChief", user.RequestedOrganization.DirectorFullName);
            InitializeFields("Fax", user.RequestedOrganization.Fax);
            InitializeFields("ChiefPhone", user.RequestedOrganization.Phone);
            InitializeFields("User", string.Format("{0} {1} {2}",
                    user.lastName, user.firstName, user.patronymicName));
            InitializeFields("UserPhone", user.phone);
            InitializeFields("UserEmail", user.email);

            this.Error += ErrorHandler;
        }

        private void ErrorHandler(object sender, ErrorEventArgs e)
        {
            LogManager.Error(e.GetException());
        }

    }
}
