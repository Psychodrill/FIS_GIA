using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Fbs.Core.Organizations;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;

namespace Fbs.Core.Users
{
    /// <summary>
    /// Пользователь с точки зрения привязки к организации
    /// </summary>
    public class OrgUser
    {
        public static string ClientLogin
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static string ClientIp
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        OrganizationRequest RequestedOrganization_ = new OrganizationRequest();
        /// <summary>
        /// Организация, к которой привязан пользователь
        /// </summary>
        public OrganizationRequest RequestedOrganization
        {
            get { return RequestedOrganization_; }
            set { RequestedOrganization_ = value; }
        }

        string login_;
        public string login
        {
            get
            {
                return login_;
            }
            set
            {
                login_ = value;
            }
        }
        string passwordHash_;
        public string passwordHash
        {
            get
            {
                return passwordHash_;
            }
            set
            {
                passwordHash_ = value;
            }
        }
        string lastName_;
        public string lastName
        {
            get
            {
                return lastName_;
            }
            set
            {
                lastName_ = value;
            }
        }
        string firstName_;
        public string firstName
        {
            get
            {
                return firstName_;
            }
            set
            {
                firstName_ = value;
            }
        }
        string patronymicName_;
        public string patronymicName
        {
            get
            {
                return patronymicName_;
            }
            set
            {
                patronymicName_ = value;
            }
        }
        string phone_;
        public string phone
        {
            get
            {
                return phone_;
            }
            set
            {
                phone_ = value;
            }
        }
        string email_;
        public string email
        {
            get
            {
                return email_;
            }
            set
            {
                email_ = value;
            }
        }
        string ipAddresses_;
        public string ipAddresses
        {
            get
            {
                return ipAddresses_;
            }
            set
            {
                ipAddresses_ = value;
            }
        }
        UserAccount.UserAccountStatusEnum status_;
        public UserAccount.UserAccountStatusEnum status
        {
            get
            {
                return status_;
            }
            set
            {
                status_ = value;
            }
        }

        public string statusCode
        {
            get
            {
                switch (status)
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
                }
                return "";
            }

        }

        byte[] registrationDocument_;
        /// <summary>
        /// Документ, подтверждающий правильность заполнения полей при регистрации
        /// </summary>
        public byte[] registrationDocument
        {
            get
            {
                return registrationDocument_;
            }
            set
            {
                registrationDocument_ = value;
            }
        }
        string registrationDocumentContentType_;
        public string registrationDocumentContentType
        {
            get
            {
                return registrationDocumentContentType_;
            }
            set
            {
                registrationDocumentContentType_ = value;
            }
        }
        string editorLogin_;
        public string editorLogin
        {
            get
            {
                return editorLogin_;
            }
            set
            {
                editorLogin_ = value;
            }
        }
        string editorIp_;
        public string editorIp
        {
            get
            {
                return editorIp_;
            }
            set
            {
                editorIp_ = value;
            }
        }
        string password_;
        public string password
        {
            get
            {
                return password_;
            }
            set
            {
                passwordHash = HashPassword(value);
                password_ = value;
            }
        }
        string hasFixedIp_;
        public string hasFixedIp
        {
            get
            {
                return hasFixedIp_;
            }
            set
            {
                hasFixedIp_ = value;
            }
        }
        string hasCrocEgeIntegration_;
        public string hasCrocEgeIntegration
        {
            get
            {
                return hasCrocEgeIntegration_;
            }
            set
            {
                hasCrocEgeIntegration_ = value;
            }
        }

        bool CanEdit_;
        public bool CanEdit
        {
            get { return CanEdit_; }
            private set { CanEdit_ = value; }
        }

        bool CanEditRegistrationDocument_;
        public bool CanEditRegistrationDocument
        {
            get { return CanEditRegistrationDocument_; }
            private set { CanEditRegistrationDocument_ = value; }
        }

        string AdminComment_;
        public string AdminComment
        {
            get { return AdminComment_; }
            set { AdminComment_ = value; }
        }

        /// <summary>
        /// Получение полного имени пользователя (Ф+" "+И+" "+О)
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(this.lastName))
                result.Append(this.lastName);
            if (!string.IsNullOrEmpty(this.firstName))
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(this.firstName);
            }
            if (!string.IsNullOrEmpty(this.patronymicName))
            {
                if (result.Length > 0)
                    result.Append(" ");
                result.Append(this.patronymicName);
            }
            return result.ToString();
        }

        public OrgUser()
        {
            editorIp = ClientIp;
            editorLogin = ClientLogin;
        }

        internal OrgUser(IDataReader reader)
            : this()
        {

            login = reader["Login"].ToString();
            lastName = reader["LastName"].ToString();
            firstName = reader["FirstName"].ToString();
            patronymicName = reader["PatronymicName"].ToString();

            CanEdit = Convert.ToBoolean(reader["CanEdit"]);
            CanEditRegistrationDocument = Convert.ToBoolean(reader["CanEditRegistrationDocument"]);

            RequestedOrganization.Id = (reader["OrganizationId"] != DBNull.Value) ? Convert.ToInt32(reader["OrganizationId"]) : -1;//Тут возникала ошибка при распечатке справки и др. действиях

            RequestedOrganization.Region = new Fbs.Core.CatalogElements.CatalogElement(reader, "OrganizationRegionId", "OrganizationRegionName");
            RequestedOrganization.FullName = reader["OrganizationName"].ToString();
            RequestedOrganization.OwnerDepartment = reader["OrganizationFounderName"].ToString();
            RequestedOrganization.DirectorFullName = reader["OrganizationChiefName"].ToString();
            RequestedOrganization.LawAddress = reader["OrganizationAddress"].ToString();
            RequestedOrganization.Fax = reader["OrganizationFax"].ToString();
            RequestedOrganization.Phone = reader["OrganizationPhone"].ToString();

            RequestedOrganization.INN = reader["OrganizationINN"].ToString();
            RequestedOrganization.OGRN = reader["OrganizationOGRN"].ToString();
            RequestedOrganization.DirectorPosition = reader["OrganizationDirectorPosition"].ToString();
            RequestedOrganization.ShortName = reader["OrganizationShortName"].ToString();
            RequestedOrganization.FactAddress = reader["OrganizationFactAddress"].ToString();
            RequestedOrganization.PhoneCityCode = reader["OrganizationPhoneCode"].ToString();
            RequestedOrganization.AccreditationSertificate = reader["AccreditationSertificate"].ToString();
            RequestedOrganization.EMail = reader["OrganizationEMail"].ToString();
            RequestedOrganization.Site = reader["OrganizationSite"].ToString();
            RequestedOrganization.IsFilial = (reader["OrganizationIsFilial"] != DBNull.Value) ? Convert.ToBoolean(reader["OrganizationIsFilial"]) :false;
            RequestedOrganization.IsPrivate = (reader["OrganizationIsPrivate"] != DBNull.Value) ? Convert.ToBoolean(reader["OrganizationIsPrivate"]) : false;

            if (reader["OrgKindId"] != DBNull.Value)
            {
                RequestedOrganization.Kind = new Fbs.Core.CatalogElements.CatalogElement(reader, "OrgKindId", "OrgKindName");
            }

            phone = reader["Phone"].ToString();
            email = reader["Email"].ToString();
            ipAddresses = reader["IpAddresses"].ToString();

            status = UserAccount.ConvertStatusCode(reader["Status"].ToString());

            if (reader["RegistrationDocument"] != DBNull.Value)
                registrationDocument = (byte[])reader["RegistrationDocument"];

            registrationDocumentContentType = reader["RegistrationDocumentContentType"].ToString();
            hasFixedIp = reader["HasFixedIp"].ToString();
            hasCrocEgeIntegration = reader["HasCrocEgeIntegration"].ToString();

            RequestedOrganization.OrgType = new Fbs.Core.CatalogElements.CatalogElement(reader, "OrgTypeId", "OrgTypeName");
            if (reader["OReqId"] != DBNull.Value)
            {
                RequestedOrganization.OrganizationId = Convert.ToInt32(reader["OReqId"]);
            }

            AdminComment = reader["AdminComment"].ToString();
        }

        private const MachineKeyValidation PasswordFormat = MachineKeyValidation.SHA1;
        private static string HashPassword(string password)
        {

            if (HttpContext.Current == null)
                return password;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password,
                PasswordFormat.ToString());
        }
    }
}
