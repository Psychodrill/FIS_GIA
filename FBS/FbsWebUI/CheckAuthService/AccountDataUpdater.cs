namespace Fbs.Web.CheckAuthService
{
    using System;
    using System.Linq;

    using Esrp;
    using Esrp.GetDataReference;

    using Fbs.Core;
    using Fbs.Core.CatalogElements;
    using Fbs.Core.Organizations;

    /// <summary>
    /// Обновляет регистрационные данные пользователя из ЕСРП
    /// </summary>
    public class AccountDataUpdater
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDataUpdater"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        public AccountDataUpdater(string login)
        {
            this.LoginUser = login;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets LoginUser.
        /// </summary>
        public string LoginUser { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Получить актуальные данные пользователя
        /// из ЕСРП и обновить их в ФБС
        /// </summary>
        public void ActualizeRegData()
        {
            var esrpClient = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };

            Organization organizationUser = OrganizationDataAccessor.GetByLoginEsrp(this.LoginUser);
            DateTime updateDateOrganization = organizationUser != null ? organizationUser.UpdateDate : DateTime.MinValue;

            IntrantAccount account = IntrantAccount.GetAccount(this.LoginUser);
            DateTime updateDateUser = account != null ? account.UpdateDate : DateTime.MinValue;

            ActualizationData actualizationData = esrpClient.ActualizeRegData(this.LoginUser, updateDateUser, updateDateOrganization);

            var renewOrganization = actualizationData.ShouldRenewOrganization;
            if (actualizationData.ShouldRenewUser)
            {
                var details = esrpClient.AccountData(this.LoginUser);
                UpdateAccount(details);
            }

            if (renewOrganization)
            {
                this.updateOrganizations(esrpClient.OrganizationsData(new[] { this.LoginUser }));
            }
        }

        public bool CheckUserTicket(Guid guid, int systemId)
        {
            var esrpClient = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };
            return esrpClient.CheckUserTicket(guid, LoginUser, systemId) == 1;
        }

        /// <summary>
        /// Принадлежит ли пользователь группе "fbs_^systems"
        /// </summary>
        /// <returns>
        /// The fbs systems account.
        /// </returns>
        public bool FbsSystemsAccount()
        {
            var esrpClient = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };
            UserDetails userDetails = esrpClient.AccountData(this.LoginUser);

            UserGroup userGroup = userDetails.Groups.Where(x => x.Code == "fbs_^systems").SingleOrDefault();
            return userGroup != null && userDetails.Groups.Count() == 1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обновить/добавить пользователя, полученного из ЕСРП
        /// </summary>
        /// <param name="userDetails">
        /// Данные пользователя
        /// </param>
        private void UpdateAccount(UserDetails userDetails)
        {
            // , string clientSystems)
            Account.UpdateAccountEsrp(
                userDetails.Login, 
                userDetails.LastName, 
                userDetails.FirstName, 
                userDetails.PatronymicName, 
                userDetails.OrganizationID, 
                userDetails.Phone, 
                userDetails.Email, 
                userDetails.Status, 
                true);

            string groups = string.Empty;
            for (int i = 0; i < userDetails.Groups.Length; i++)
            {
                groups += userDetails.Groups[i].ID;
                groups += i != userDetails.Groups.Length - 1 ? ", " : string.Empty;
            }

            foreach (UserGroup group in userDetails.Groups)
            {
                Account.UpdateGroupUserEsrp(userDetails.Login, group.ID, groups);
            }
        }

        /// <summary>
        /// Обновить организации, полученные из ЕСРП по логинам пользователей
        /// </summary>
        /// <param name="organizationsData">
        /// Обновляемые организации
        /// </param>
        private void updateOrganizations(OrganizationData[] organizationsData)
        {
            foreach (OrganizationData orgData in organizationsData)
            {
                var organization = new Organization(
                    orgData.FullName, 
                    orgData.RegionId, 
                    orgData.TypeId, 
                    orgData.KindId, 
                    orgData.IsPrivate, 
                    orgData.INN, 
                    orgData.OGRN, 
                    (int)OrganizationStatus.Operating);
                organization.Id = orgData.ID;
                organization.AccreditationSertificate = orgData.AccreditationCertificate;
                organization.DepartmentId = orgData.DepartmentID;
                organization.DirectorFullName = orgData.DirectorFullName;
                organization.DirectorPosition = orgData.DirectorPosition;
                organization.EMail = orgData.EMail;
                organization.FactAddress = orgData.FactAddress;
                organization.Fax = orgData.Fax;
                organization.IsFilial = orgData.Isfilial;
                organization.LawAddress = orgData.LawAddress;
                organization.MainId = orgData.MainID == 0 ? (int?)null : orgData.MainID;
                organization.OwnerDepartment = orgData.OwnerDepartment;
                organization.Phone = orgData.Phone;
                organization.PhoneCityCode = orgData.PhoneCityCode;
                organization.ShortName = orgData.ShortName;
                organization.Site = orgData.Site;
                organization.Kind = new CatalogElement(orgData.KindId);
                if (organization.MainId.HasValue && organization.MainId.Value > 0)
                {
                    Organization org = OrganizationDataAccessor.Get(organization.MainId.Value);
                    if (org == null)
                    {
                        ActualizeOrganizationData(organization.MainId.Value);
                        org = OrganizationDataAccessor.Get(organization.MainId.Value);

                    }
                }
                if (organization.DepartmentId.HasValue && organization.DepartmentId.Value > 0)
                {
                    Organization org = OrganizationDataAccessor.Get(organization.DepartmentId.Value);
                    if (org == null)
                    {
                        ActualizeOrganizationData(organization.DepartmentId.Value);
                        org = OrganizationDataAccessor.Get(organization.DepartmentId.Value);

                    }
                }
                OrganizationDataAccessor.UpdateOrCreate(organization, true);
            }
        }
        public void ActualizeOrganizationData(int id)
        {
            var esrpClient = new EsrpClient { SystemId = SystemId.Fbs, EsrpUrl = Config.UrlEsrp };
            updateOrganizations(esrpClient.OrganizationDataById(id));
        }
        #endregion
    }
}