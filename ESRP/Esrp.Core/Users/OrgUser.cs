// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrgUser.cs" company="">
// </copyright>
// <summary>
//   Пользователь с точки зрения привязки к организации
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Esrp.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Security;

    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;

    /// <summary>
    /// Пользователь с точки зрения привязки к организации
    /// </summary>
    public class OrgUser:IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// Имя ИС
        /// </summary>
        public List<string> fullSystemNameList = new List<string>();

        /// <summary>
        /// The password format.
        /// </summary>
        private const MachineKeyValidation PasswordFormat = MachineKeyValidation.SHA1;

        /// <summary>
        /// The requested organization_.
        /// </summary>
        private OrganizationRequest RequestedOrganization_ = new OrganizationRequest();

        /// <summary>
        /// The password_.
        /// </summary>
        private string password_;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrgUser"/> class.
        /// </summary>
        public OrgUser()
        {
            this.editorIp = ClientIp;
            this.editorLogin = ClientLogin;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrgUser"/> class.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        internal OrgUser(IDataReader reader)
            : this()
        {
            this.login = reader["Login"].ToString();
            this.lastName = reader["LastName"].ToString();
            this.firstName = reader["FirstName"].ToString();
            this.patronymicName = reader["PatronymicName"].ToString();

            this.CanEdit = Convert.ToBoolean(reader["CanEdit"]);
            this.CanEditRegistrationDocument = Convert.ToBoolean(reader["CanEditRegistrationDocument"]);

            this.RequestedOrganization.Id = (reader["OrganizationId"] != DBNull.Value)
                                                ? Convert.ToInt32(reader["OrganizationId"])
                                                : -1; // Тут возникала ошибка при распечатке справки и др. действиях

            this.RequestedOrganization.Region = new CatalogElement(
                reader, "OrganizationRegionId", "OrganizationRegionName");
            this.RequestedOrganization.FullName = reader["OrganizationName"].ToString();
            this.RequestedOrganization.OwnerDepartment = reader["OrganizationFounderName"].ToString();
            this.RequestedOrganization.DirectorFullName = reader["OrganizationChiefName"].ToString();
            this.RequestedOrganization.LawAddress = reader["OrganizationAddress"].ToString();
            this.RequestedOrganization.TownName = reader["TownName"].ToString();
            this.RequestedOrganization.Fax = reader["OrganizationFax"].ToString();
            this.RequestedOrganization.Phone = reader["OrganizationPhone"].ToString();
            this.RequestedOrganization.ReceptionOnResultsCNE = (reader["ReceptionOnResultsCNE"] != DBNull.Value)
                                                                   ? Convert.ToInt32(reader["ReceptionOnResultsCNE"])
                                                                   : (int?)null;
            this.RequestedOrganization.KPP = reader["KPP"].ToString();
            this.RequestedOrganization.INN = reader["OrganizationINN"].ToString();
            this.RequestedOrganization.OGRN = reader["OrganizationOGRN"].ToString();
            this.RequestedOrganization.DirectorPosition = reader["OrganizationDirectorPosition"].ToString();
            this.RequestedOrganization.ShortName = reader["OrganizationShortName"].ToString();
            this.RequestedOrganization.FactAddress = reader["OrganizationFactAddress"].ToString();
            this.RequestedOrganization.PhoneCityCode = reader["OrganizationPhoneCode"].ToString();
            this.RequestedOrganization.AccreditationSertificate = reader["AccreditationSertificate"].ToString();
            this.RequestedOrganization.EMail = reader["OrganizationEMail"].ToString();
            this.RequestedOrganization.Site = reader["OrganizationSite"].ToString();
            this.RequestedOrganization.IsFilial = (reader["OrganizationIsFilial"] != DBNull.Value)
                                                  && Convert.ToBoolean(reader["OrganizationIsFilial"]);
            if (reader["RCModelId"] == DBNull.Value)
            {
                // Есть случаи когда это поле null в таблице OrganizationRequest2010
                this.RequestedOrganization.RCModelId = -1;
            }
            else
            {
                this.RequestedOrganization.RCModelId = Convert.ToInt32(reader["RCModelId"]);
            }


            this.RequestedOrganization.RCModelName = (reader["ModelName"] != DBNull.Value)
                                                         ? Convert.ToString(reader["ModelName"])
                                                         : string.Empty;

            this.RequestedOrganization.RCDescription = (reader["RCDescription"] != DBNull.Value)
                                                           ? Convert.ToString(reader["RCDescription"])
                                                           : "Не указано";

            this.RequestedOrganization.IsPrivate = (reader["OrganizationIsPrivate"] != DBNull.Value)
                                                   && Convert.ToBoolean(reader["OrganizationIsPrivate"]);

            if (reader["OrgKindId"] != DBNull.Value)
            {
                this.RequestedOrganization.Kind = new CatalogElement(reader, "OrgKindId", "OrgKindName");
            }

            this.phone = reader["Phone"].ToString();
            this.position = reader["Position"].ToString();
            this.email = reader["Email"].ToString();
            this.ipAddresses = reader["IpAddresses"].ToString();

            this.status = UserAccount.ConvertStatusCode(reader["Status"].ToString());

            if (reader["RegistrationDocument"] != DBNull.Value)
            {
                this.registrationDocument = (byte[])reader["RegistrationDocument"];
            }

            this.registrationDocumentContentType = reader["RegistrationDocumentContentType"].ToString();
            this.hasFixedIp = reader["HasFixedIp"].ToString();

            this.RequestedOrganization.OrgType = new CatalogElement(reader, "OrgTypeId", "OrgTypeName");
            if (reader["OReqId"] != DBNull.Value)
            {
                this.RequestedOrganization.OrganizationId = Convert.ToInt32(reader["OReqId"]);
            }

            this.AdminComment = reader["AdminComment"].ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets ClientIp.
        /// </summary>
        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        /// <summary>
        /// Gets ClientLogin.
        /// </summary>
        public static string ClientLogin
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return HttpContext.Current.User.Identity.Name;
            }
        }

        /// <summary>
        /// Список идентификаторов ИС для пользователя
        /// </summary>
        public List<string> SystemsId = new List<string>();  

        /// <summary>
        /// Gets or sets AdminComment.
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets a value indicating whether CanEdit.
        /// </summary>
        public bool CanEdit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether CanEditRegistrationDocument.
        /// </summary>
        public bool CanEditRegistrationDocument { get; private set; }

        /// <summary>
        /// Организация, к которой привязан пользователь
        /// </summary>
        /// <remarks>
        /// Это заявка на регистрацию пользователя на доступ к системам для работы с организацией.
        /// </remarks>
        public OrganizationRequest RequestedOrganization
        {
            get
            {
                return this.RequestedOrganization_;
            }

            set
            {
                this.RequestedOrganization_ = value;
            }
        }

        /// <summary>
        /// Gets or sets editorIp.
        /// </summary>
        public string editorIp { get; set; }

        /// <summary>
        /// Gets or sets editorLogin.
        /// </summary>
        public string editorLogin { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        public string firstName { get; set; }

        /// <summary>
        /// Gets or sets hasFixedIp.
        /// </summary>
        public string hasFixedIp { get; set; }

        /// <summary>
        /// Gets or sets ipAddresses.
        /// </summary>
        public string ipAddresses { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        public string lastName { get; set; }

        /// <summary>
        /// Gets or sets login.
        /// </summary>
        public string login { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string password
        {
            get
            {
                return this.password_;
            }

            set
            {
                this.passwordHash = HashPassword(value);
                this.password_ = value;
            }
        }

        /// <summary>
        /// Gets or sets passwordHash.
        /// </summary>
        public string passwordHash { get; set; }

        /// <summary>
        /// Gets or sets patronymicName.
        /// </summary>
        public string patronymicName { get; set; }

        /// <summary>
        /// Gets or sets phone.
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public string position { get; set; }

        /// <summary>
        /// Документ, подтверждающий правильность заполнения полей при регистрации
        /// </summary>
        public byte[] registrationDocument { get; set; }

        /// <summary>
        /// Gets or sets registrationDocumentContentType.
        /// </summary>
        public string registrationDocumentContentType { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        public UserAccount.UserAccountStatusEnum status { get; set; }

        /// <summary>
        /// Gets statusCode.
        /// </summary>
        public string statusCode
        {
            get
            {
                switch (this.status)
                {
                    case UserAccount.UserAccountStatusEnum.Activated:
                        return "activated";
                    case UserAccount.UserAccountStatusEnum.Deactivated:
                        return "deactivated";
                    case UserAccount.UserAccountStatusEnum.Consideration:
                        return "consideration";
                    case UserAccount.UserAccountStatusEnum.Revision:
                        return "revision";
                    case UserAccount.UserAccountStatusEnum.Registration:
                        return "registration";
                    case UserAccount.UserAccountStatusEnum.Readonly:
                        return "readonly";
                }

                return null;
            }
        } 

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Проверка может ли пользователь быть активирован.
        /// </summary>
        /// <returns>
        /// The can be activated.
        /// </returns>
        public bool CanBeActivated()
        {
            switch (this.status)
            {
                case UserAccount.UserAccountStatusEnum.Deactivated:
                case UserAccount.UserAccountStatusEnum.Readonly:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Проверка может ли пользователь быть деактивирован.
        /// </summary>
        /// <returns>
        /// The can be deactivated.
        /// </returns>
        public bool CanBeDeactivated()
        {
            switch (this.status)
            {
                case UserAccount.UserAccountStatusEnum.Activated:
                case UserAccount.UserAccountStatusEnum.Readonly:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Получение полного имени пользователя (Ф+" "+И+" "+О)
        /// </summary>
        /// <returns>
        /// The get full name.
        /// </returns>
        public string GetFullName()
        {
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(this.lastName))
            {
                result.Append(this.lastName);
            }

            if (!string.IsNullOrEmpty(this.firstName))
            {
                if (result.Length > 0)
                {
                    result.Append(" ");
                }

                result.Append(this.firstName);
            }

            if (!string.IsNullOrEmpty(this.patronymicName))
            {
                if (result.Length > 0)
                {
                    result.Append(" ");
                }

                result.Append(this.patronymicName);
            }

            return result.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The hash password.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The hash password.
        /// </returns>
        private static string HashPassword(string password)
        {
            if (HttpContext.Current == null)
            {
                return password;
            }

            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, PasswordFormat.ToString());
        }

        #endregion

        public void Dispose()
        {
            this.AdminComment = "";
            this.CanEdit = false;
            this.CanEditRegistrationDocument = false;
            this.editorIp = "";
            this.editorLogin = "";
            this.email = "";
            this.firstName = "";
            this.hasFixedIp = "";
            this.ipAddresses = "";
            this.lastName = "";
            this.login = "";
            this.password = "";
            this.passwordHash = "";
            this.patronymicName = "";
            this.phone = "";
            this.position = "";
            this.registrationDocument = null ;
            this.registrationDocumentContentType = "";
            this.RequestedOrganization = null;
            this.status = UserAccount.UserAccountStatusEnum.None;
            this.SystemsId = null;
        }
    }
}