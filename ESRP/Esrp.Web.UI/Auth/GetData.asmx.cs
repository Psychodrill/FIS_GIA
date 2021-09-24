namespace Esrp.Web.Auth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Services;

    using Esrp.Core;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;
    using Esrp.Web.Administration.SqlConstructor.Organizations;
    using System.Data;
    using System.Configuration;
    using System.Data.SqlClient;
    using Esrp.Web.Administration.Organizations;
    using System.Collections.Specialized;

    /// <summary>
    /// Summary description for GetData
    /// </summary>
    /// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    /// [System.Web.Script.Services.ScriptService]
    [WebService(Namespace = "urn:ersp:v1")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [ToolboxItem(false)]
    public class GetData : WebService
    {
        /*o	Xml, содержащий сведения о пользователе и сведения о группах, в которые включен пользователь. Перечень данных, которые должен содержать xml:
        *	Сведения о пользователе: таблица Account, поля Id, Login, LastName, FirstName, PatronymicName, OrganizationId, Phone, Email, Status
        *	Сведения о группах, которым принадлежит пользователь: таблица Group, поля Id, Code, Name. Группы определяется на основе таблицы GroupAccount. В xml включаются только те группы, которые соответствуют целевой системе.
        */
        #region Public Methods

        /// <summary>
        /// The get actualization data.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="lastChangeUser">
        /// The last change user.
        /// </param>
        /// <param name="lastChangeOrganization">
        /// The last change organization.
        /// </param>
        /// <returns>
        /// </returns>
        [WebMethod]
        public ActualizationData GetActualizationData(
            string userLogin, DateTime lastChangeUser, DateTime lastChangeOrganization)
        {
            CheckAuth.CheckHttps();
            DateTime dt = OrganizationDataAccessor.GetDateUpdatedByLogin(userLogin);
            var actualizationData = new ActualizationData();
            if (dt != DateTime.MinValue)
            {
                if (dt > lastChangeOrganization)
                {
                    actualizationData.ShouldRenewOrganization = true;
                }
            }

            DateTime userUpdateDate = GeneralSystemManager.GetUserUpdateDate(userLogin);
            if (userUpdateDate != DateTime.MinValue && userUpdateDate > lastChangeUser)
            {
                actualizationData.ShouldRenewUser = true;
            }

            return actualizationData;
        }
        /// <summary>
        /// Поиск организаторов олимпиады
        /// </summary>
        /// <param name="organizerName"></param>
        /// <param name="organizerType"></param>
        /// <param name="region"></param>
        /// <param name="pageNumber"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        [WebMethod]
        public List<OrganizationData> FindOrganizations(string searchBy,string Inn,string organizerName, string organizerType, string region, int pageNumber, out int total)
        {
            List<OrganizationData> ret = new List<OrganizationData>();
            SqlConstructor_GetOrganizations scgo = new SqlConstructor_GetOrganizations_Registration(new NameValueCollection { { "RBSearchBy", searchBy },{"INN",Inn}, { "OrgName", organizerName }, { "OldTypeId", organizerType }, { "RegID",region },{"pageNum",(pageNumber-1).ToString()},{"pageSize","20"} });
            DataTable table = null;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = scgo.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                total = (int)cmd.Parameters["rowCount"].Value;
                
                cmd = scgo.GetSQL();
                cmd.Connection = connection;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }

            foreach (DataRow row in table.Rows)
            {
                OrganizationData data=new OrganizationData();
                data.ID=(int)row["OrgId"];
                data.FullName=row["FullName"].ToString();
                data.INN=row["INN"].ToString();
                data.IsPrivate=(bool)row["IsPrivate"];
                data.RegionId = (int)row["RegId"];
                data.Type = new OrganizationType();
                data.Type.ID = (int)row["TypeId"];
                data.Type.Name = row["TypeName"].ToString();
                ret.Add(data);
            }
            return ret;
        }


        /// <summary>
        /// Метод проверки актуальности сведений о пользователе и об организации
        /// </summary>
        /// <param name="userLogin">Имя учетной записи пользователя</param>
        /// <param name="lastChangeUser">Дата последнего обновления данных о пользователе</param>
        /// <param name="lastChangeOrganization">Дата последнего обновления данных об организации</param>
        /// <param name="numberOfOrganizations">Количество действующих дочерних организаций в базе: 
        /// для головных организаций – количество филиалов, 
        /// для учредителей – количество подведомственных организаций (включая их филиалы)</param>
        /// <param name="lastChangeFounder">Дата последнего обновления сведений об учредителе организации</param>
        /// <param name="lastChangeMainOrg">Дата последнего обновления сведений о головной организации (для филиалов)</param>
        /// <returns>Результаты проверок</returns>
        [WebMethod(MessageName = "GetActualizationDataExtended")]
        public ActualizationDataExtended GetActualizationData(
            string userLogin, DateTime lastChangeUser, DateTime lastChangeOrganization, int numberOfOrganizations, DateTime lastChangeFounder, DateTime lastChangeMainOrg)
        {
            CheckAuth.CheckHttps();
            var actualizationData = new ActualizationDataExtended();

            var userUpdateDate = GeneralSystemManager.GetUserUpdateDate(userLogin);
            if (userUpdateDate != DateTime.MinValue && userUpdateDate > lastChangeUser)
            {
                actualizationData.ShouldRenewUser = true;
            }

            var orgUpdateDate = OrganizationDataAccessor.GetDateUpdatedByLogin(userLogin);
            if (orgUpdateDate != DateTime.MinValue && orgUpdateDate > lastChangeOrganization)
            {
                actualizationData.ShouldRenewOrganization = true;
                actualizationData.ShouldRenewOrganizations = true;
            }

            var founderUpdateDate = OrganizationDataAccessor.GetDateUpdatedFounderByLogin(userLogin);
            if (founderUpdateDate != DateTime.MinValue && founderUpdateDate > lastChangeFounder)
            {
                actualizationData.ShouldRenewFounder = true;
            }

            var mainOrgUpdateDate = OrganizationDataAccessor.GetDateUpdatedMainOrgByLogin(userLogin);
            if (mainOrgUpdateDate != DateTime.MinValue && mainOrgUpdateDate > lastChangeMainOrg)
            {
                actualizationData.ShouldRenewMainOrg = true;
            }

            var numberOrganizationsUpdate = OrganizationDataAccessor.GetNumberOrgByLogin(userLogin);
            if (numberOrganizationsUpdate != numberOfOrganizations)
            {
                actualizationData.ShouldRenewOrganizations = true;
            }

            return actualizationData;
        }
        



        /// <summary>
        /// Метод получения данных об организации 
        /// </summary>
        /// <param name="orgId">ИД организации пользователя</param>
        /// <param name="updateType">Сведения, которые необходимо обновить</param>
        /// <returns>Данные об организации</returns>
        [WebMethod(MessageName = "GetOrganizationDataExtended")]
        public List<OrganizationData> GetOrganizationData(
            int orgId, int updateType)
        {
            CheckAuth.CheckHttps();
            var odata = new List<OrganizationData>();

            if (updateType < 1 || updateType > 15)
            {
                throw new Exception("Неверный параметр updateType");
            }

            if (updateType >= 8)
            {
                updateType = updateType - 8;

                // Головная организация 
                var mainOrg = this.GetMainOrgById(orgId);
                odata.Add(mainOrg);
            }

            if (updateType >= 4)
            {
                updateType = updateType - 4;

                // Учредитель
                var departmentOrg = this.GetDepartmentOrgById(orgId);
                odata.Add(departmentOrg);
            }

            if (updateType >= 2)
            {
                updateType = updateType - 2;

                // Подведомственные организации
                var filialOrg = this.GetFilialOrgById(orgId);
                odata.AddRange(filialOrg);
            }

            if (updateType >= 1)
            {
                // организация
                var org = this.Get(orgId);
                odata.Add(org);
            }

            return odata;
        }

        /// <summary>
        /// The get organization data.
        /// </summary>
        /// <param name="userLogins">
        /// The user logins.
        /// </param>
        /// <returns>
        /// </returns>
        [WebMethod]
        public OrganizationDataExtended[] GetOrganizationData(string[] userLogins)
        {
            CheckAuth.CheckHttps();
            if (userLogins == null)
            {
                userLogins = new string[0];
            }

            var odata = new OrganizationDataExtended[userLogins.Length];
            for (int i = 0; i < userLogins.Length; i++)
            {
                odata[i] = this.GetOneOrganizationData(userLogins[i]);
            }

            return odata;
        }

        /// <summary>
        /// The get user details.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="systemID">
        /// The system id.
        /// </param>
        /// <returns>
        /// </returns>
        [WebMethod]
        public UserDetails GetUserDetails(string userLogin, int systemID)
        {
            CheckAuth.CheckHttps();
            UserAccount userAccount = UserAccount.GetUserAccount(userLogin);
            var dt = new UserDetails();
            dt.UserID = GeneralSystemManager.GetUserID(userLogin);
            dt.Login = userAccount.Login;
            dt.LastName = userAccount.LastName;
            dt.FirstName = userAccount.FirstName;
            dt.PatronymicName = userAccount.PatronymicName;
            dt.Phone = userAccount.Phone;
            dt.Email = userAccount.Login;
            dt.Status = userAccount.InternalStatus;
            dt.OrganizationID = GeneralSystemManager.GetUserOrganizationMain(userLogin);
            if (dt.OrganizationID == 0)
            {
                dt.OrganizationID = null;
            }

            dt.Groups =
                GeneralSystemManager.GetUserGroups(userLogin, systemID).Select(
                    x => new UserGroup { ID = x.ID, Code = x.Code, Name = x.Name }).ToArray();
            return dt;
        }

        /// <summary>
        /// The update user status.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <param name="newStatus">
        /// The new status.
        /// </param>
        /// <returns>
        /// The update user status.
        /// </returns>
        [WebMethod]
        public string UpdateUserStatus(string userLogin, string newStatus)
        {
            string updateUserStatus = UserAccount.UpdateUserStatus(userLogin, newStatus);
            return updateUserStatus;
        }


        /// <summary>
        /// Получение текущего статуса организации.
        /// Чтобы не затрагивать текущий функционал, реализовано отдельно
        /// </summary>
        /// <param name="orgId">Идентификатор организации</param>
        /// <returns>Текущий статус организации:
        /// 1 - Действующая;
        /// 2 - Реорганизованная;
        /// 3 - Ликвидирована;
        /// -1 - Ошибка получения текущего статуса или статус не определён</returns>
        [WebMethod]
        public int GetOrgStatus(int orgId)
        {
            CheckAuth.CheckHttps(); 
            
            int? result = -1;

            try
            {
                result = OrganizationDataAccessor.GetStatus(orgId);
                if (result == null) result = -1;
            }
            catch { }
            return result.Value;
        }
        #endregion

        #region Methods

        private OrganizationDataExtended GetOneOrganizationData(string userLogin)
        {
            var org = OrganizationDataAccessor.GetByLogin(userLogin);
            if (org == null)
            {
                return null;
            }
            
            var odata = new OrganizationDataExtended();
            odata.AccreditationCertificate = org.AccreditationSertificate;
            odata.DepartmentID = org.DepartmentId.HasValue ? org.DepartmentId.Value : 0;
            odata.DirectorFullName = org.DirectorFullName;
            odata.DirectorPosition = org.DirectorPosition;
            odata.EMail = org.EMail;
            odata.FactAddress = String.IsNullOrEmpty(org.FactAddress) ? "НЕ УКАЗАН" : org.FactAddress;
            odata.Fax = org.Fax;
            odata.ID = org.Id;
            odata.INN = org.INN;
            odata.IsAccredited = !String.IsNullOrEmpty(org.AccreditationSertificate);
            odata.Isfilial = org.IsFilial;
            odata.IsPrivate = org.IsPrivate;
            odata.KindId = org.Kind != null && org.Kind.Id.HasValue ? org.Kind.Id.Value : 0;
            odata.LawAddress = org.LawAddress;
            odata.MainID = org.MainId.HasValue ? org.MainId.Value : 0;
            odata.OGRN = org.OGRN;
            odata.OwnerDepartment = org.OwnerDepartment;
            odata.Phone = org.Phone;
            odata.PhoneCityCode = org.PhoneCityCode;
            if (org.Region != null)
            {
                odata.RegionId = org.Region.Id.HasValue ? org.Region.Id.Value : 0;
                odata.Region = new RegionData();
                odata.Region.ID = odata.RegionId;
                odata.Region.Name = org.Region.Name;
                odata.Region.Code = org.Region.Code;
            }

            odata.ShortName = org.ShortName;
            odata.FullName = org.FullName;
            odata.Site = org.Site;
            if(org.OrgType != null)
            {
                odata.TypeId = org.OrgType.Id.HasValue ? org.OrgType.Id.Value : 0;
                odata.Type = new OrganizationType();
                odata.Type.ID = odata.TypeId;
                odata.Type.Name = org.OrgType.Name;
            }

            odata.RequestedLogin = userLogin;
            return odata;
        }

        private OrganizationData GetMainOrgById(int id)
        {
            var mainOrgId = OrganizationDataAccessor.GetMainOrgIdById(id);
            var mainOrg = OrganizationDataAccessor.Get(mainOrgId);
            if (mainOrg == null)
            {
                return null;
            }

            var odata = this.AddOrganizationValues(mainOrg);
            return odata;
        }

        private OrganizationData GetDepartmentOrgById(int id)
        {
            var departmentId = OrganizationDataAccessor.GetFounderIdById(id);
            var department = OrganizationDataAccessor.Get(departmentId);
            if (department == null)
            {
                return null;
            }

            var odata = this.AddOrganizationValues(department);
            return odata;
        }

        private List<OrganizationData> GetFilialOrgById(int orgId)
        {
            List<OrganizationData> ret = new List<OrganizationData>();
            // Получаем Id Учредителя
            var departmentId = OrganizationDataAccessor.GetFounderIdById(orgId);

            // Получаем Id головной компании
            var mainOrgId = OrganizationDataAccessor.GetMainOrgIdById(orgId);
            List<int> ids = new List<int>();
            // Получаем идентификаторы подведомственных организаций 
            if (mainOrgId > 0)
            //для филиала ничего не будет однозначно
            {
                return ret;
            }
            else if(departmentId>=0)
            {
                // это головная организация попробуем что-нть достать
                ids.AddRange(OrganizationDataAccessor.GetFilialIdByMainOrgId(orgId));
                
            }
            else
                // остается только учердитель...
                ids.AddRange(OrganizationDataAccessor.GetFilialIdOrgByFounderId(orgId));

            var filialOrganizationsData = new List<OrganizationData>();
            foreach (var id in ids)
            {
                var org = OrganizationDataAccessor.Get(id);
                if (org != null)
                {
                    var odata = this.AddOrganizationValues(org);
                    filialOrganizationsData.Add(odata);
                }
            }

            return filialOrganizationsData;
        }

        private OrganizationData Get(int id)
        {
            Organization org = OrganizationDataAccessor.Get(id);
            if (org == null)
            {
                return null;
            }
            
            var odata = this.AddOrganizationValues(org);
            return odata;
        }

        private OrganizationData AddOrganizationValues(Organization org)
        {
            var odata = new OrganizationData();
            odata.AccreditationCertificate = org.AccreditationSertificate;
            odata.DepartmentID = org.DepartmentId.HasValue ? org.DepartmentId.Value : 0;
            odata.DirectorFullName = org.DirectorFullName;
            odata.DirectorPosition = org.DirectorPosition;
            odata.EMail = org.EMail;
            odata.FactAddress = String.IsNullOrEmpty(org.FactAddress)?"НЕ УКАЗАН":org.FactAddress;
            odata.Fax = org.Fax;
            odata.ID = org.Id;
            odata.INN = org.INN;
            odata.IsAccredited = !string.IsNullOrEmpty(org.AccreditationSertificate);
            odata.Isfilial = org.IsFilial;
            odata.IsPrivate = org.IsPrivate;
            odata.KindId = org.Kind != null && org.Kind.Id.HasValue ? org.Kind.Id.Value : 0;
            odata.LawAddress = org.LawAddress;
            odata.MainID = org.MainId.HasValue ? org.MainId.Value : 0;
            odata.OGRN = org.OGRN;
            odata.OwnerDepartment = org.OwnerDepartment;
            odata.Phone = org.Phone;
            odata.PhoneCityCode = org.PhoneCityCode;
            if (org.Region != null)
            {
                odata.RegionId = org.Region.Id.HasValue ? org.Region.Id.Value : 0;
                odata.Region = new RegionData();
                odata.Region.ID = odata.RegionId;
                odata.Region.Name = org.Region.Name;
                odata.Region.Code = org.Region.Code;
            }

            odata.ShortName = org.ShortName;
            odata.FullName = org.FullName;
            odata.Site = org.Site;
            if (org.OrgType != null)
            {
                odata.TypeId = org.OrgType.Id.HasValue ? org.OrgType.Id.Value : 0;
                odata.Type = new OrganizationType();
                odata.Type.ID = odata.TypeId;
                odata.Type.Name = org.OrgType.Name;
            }

            return odata;
        }



        #endregion

        /// <summary>
        /// The actualization data.
        /// </summary>
        public class ActualizationData
        {
            #region Public Properties

            /// <summary>
            /// Признак необходимости обновления данных о пользователе
            /// </summary>
            public bool ShouldRenewUser { get; set; }

            /// <summary>
            /// Признак необходимости обновления данных об организации
            /// </summary>
            public bool ShouldRenewOrganization { get; set; }

            #endregion
        }

        /// <summary>
        /// Класс признаков актуальности сведений о пользователе и об организации
        /// </summary>
        public class ActualizationDataExtended : ActualizationData
        {
            #region Public Properties

            /// <summary>
            /// Признак необходимости обновления данных о дочерних организациях
            /// </summary>
            public bool ShouldRenewOrganizations { get; set; }

            /// <summary>
            /// Признак необходимости обновления данных об учредителе
            /// </summary>
            public bool ShouldRenewFounder { get; set; }

            /// <summary>
            /// Признак необходимости обновления данных о головной организации
            /// </summary>
            public bool ShouldRenewMainOrg { get; set; }

            #endregion
        }

        /*	Сведения о самой организации: таблица Organization, поля Id, FullName, ShortName, RegionId, TypeId, KindId, INN, OGRN,
         * OwnerDepartment, IsPrivate, IsFilial, DirectorPosition, DirectorFullName, IsAccredited, 
         * AccreditationSertificate, LawAddress, FactAddress, PhoneCityCode, Phone, Fax, EMail, Site, MainId, DepartmentId
           Логин пользователя, для которого запрашивались сведения об организации;
           Сведения о регионе (определяется по RegionId): таблица Region, поля Id, Code, Name
           Сведения о типе организации (определяется по TypeId). Таблица OrganizationType, поля Id, Name 
        */

        /// <summary>
        /// Расширение сведений о организации
        /// </summary>
        public class OrganizationDataExtended : OrganizationData
        {
            /// <summary>
            /// Gets or sets RequestedLogin.
            /// </summary>
            public string RequestedLogin { get; set; }
        }

        /// <summary>
        /// Сведения о организации
        /// </summary>
        public class OrganizationData
        {
            #region Public Properties

            /// <summary>
            /// Идентификатор организации
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Полное наименование
            /// </summary>
            public string FullName { get; set; }

            /// <summary>
            /// Краткое наименование
            /// </summary>
            public string ShortName { get; set; }

            /// <summary>
            /// Код региона
            /// </summary>
            public int RegionId { get; set; }

            /// <summary>
            /// ИД типа организации
            /// </summary>
            public int TypeId { get; set; }

            /// <summary>
            /// ИД вида организации
            /// </summary>
            public int KindId { get; set; }

            /// <summary>
            /// ИНН
            /// </summary>
            public string INN { get; set; }

            /// <summary>
            /// ОГРН
            /// </summary>
            public string OGRN { get; set; }

            /// <summary>
            /// Учредитель
            /// </summary>
            public string OwnerDepartment { get; set; }

            /// <summary>
            /// Организационно-правовая форма
            /// </summary>
            public bool IsPrivate { get; set; }

            /// <summary>
            /// Является ли  филиалом
            /// </summary>
            public bool Isfilial { get; set; }

            /// <summary>
            /// Должность руководителя
            /// </summary>
            public string DirectorPosition { get; set; }

            /// <summary>
            /// ФИО руководителя
            /// </summary>
            public string DirectorFullName { get; set; }

            /// <summary>
            /// Признак аккредитации
            /// </summary>
            public bool IsAccredited { get; set; }

            /// <summary>
            /// Номер свидетельства об аккредитации
            /// </summary>
            public string AccreditationCertificate { get; set; }

            /// <summary>
            /// Юридический адрес
            /// </summary>
            public string LawAddress { get; set; }

            /// <summary>
            /// Фактический адрес
            /// </summary>
            public string FactAddress { get; set; }

            /// <summary>
            /// Телефонный код города
            /// </summary>
            public string PhoneCityCode { get; set; }

            /// <summary>
            /// Телефон
            /// </summary>
            public string Phone { get; set; }

            /// <summary>
            /// Факс
            /// </summary>
            public string Fax { get; set; }

            /// <summary>
            /// Адрес электронной почты
            /// </summary>
            public string EMail { get; set; }

            /// <summary>
            /// Сайт
            /// </summary>
            public string Site { get; set; }

            /// <summary>
            /// ИД учредителя
            /// </summary>
            public int MainID { get; set; }

            /// <summary>
            /// ИД родительской организации (если организация является филиалом)
            /// </summary>
            public int DepartmentID { get; set; }

            /// <summary>
            /// Gets or sets Region.
            /// </summary>
            public RegionData Region { get; set; }

            /// <summary>
            /// Вид
            /// </summary>
            public OrganizationType Type { get; set; }

            #endregion
        }

        /// <summary>
        /// The organization type.
        /// </summary>
        public class OrganizationType
        {
            #region Public Properties

            /// <summary>
            /// Gets or sets ID.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Наименование типа
            /// </summary>
            public string Name { get; set; }

            #endregion
        }

        /// <summary>
        /// The region data.
        /// </summary>
        public class RegionData
        {
            #region Public Properties

            /// <summary>
            /// Код региона
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Идентификатор региона
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Наименование региона
            /// </summary>
            public string Name { get; set; }

            #endregion
        }

        /// <summary>
        /// The user details.
        /// </summary>
        public class UserDetails
        {
            #region Public Properties

            /// <summary>
            /// Gets or sets Email.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets FirstName.
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            /// Gets or sets Groups.
            /// </summary>
            public UserGroup[] Groups { get; set; }

            /// <summary>
            /// Gets or sets LastName.
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// Gets or sets Login.
            /// </summary>
            public string Login { get; set; }

            /// <summary>
            /// Gets or sets OrganizationID.
            /// </summary>
            public int? OrganizationID { get; set; }

            /// <summary>
            /// Gets or sets PatronymicName.
            /// </summary>
            public string PatronymicName { get; set; }

            /// <summary>
            /// Gets or sets Phone.
            /// </summary>
            public string Phone { get; set; }

            /// <summary>
            /// Gets or sets Status.
            /// </summary>
            public string Status { get; set; }

            /// <summary>
            /// Gets or sets UserID.
            /// </summary>
            public int UserID { get; set; }

            #endregion
        }

        /// <summary>
        /// The user group.
        /// </summary>
        public class UserGroup
        {
            #region Public Properties

            /// <summary>
            /// Gets or sets Code.
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Gets or sets ID.
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// Gets or sets Name.
            /// </summary>
            public string Name { get; set; }

            #endregion
        }
    }
}