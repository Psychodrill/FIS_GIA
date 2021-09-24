namespace Esrp.Core.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    using Esrp.Core.DataAccess;
    using Esrp.Core.Users;
    using CatalogElements;

    /// <summary>
    /// The org with parent.
    /// </summary>
    public class OrgWithParent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsPrivate.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets MainId.
        /// </summary>
        public int? MainId { get; set; }

        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Gets or sets ShortName.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets TypeId.
        /// </summary>
        public int TypeId { get; set; }

        #endregion
    }

    /// <summary>
    /// Класс доступа к организации
    /// </summary>
    public class OrganizationDataAccessor
    {
        #region Constants and Fields

        /// <summary>
        /// перечисление колонок для операции Insert.
        /// Порядок должен соответствовать порядку в OrgColumnsParameters
        /// </summary>
        private const string OrgColumnsNames =
            @"UpdateDate, FullName, ShortName, INN, OGRN, KPP, OwnerDepartment, IsPrivate, IsFilial,
			 DirectorFullName, DirectorFullNameInGenetive, DirectorFirstName, DirectorLastName, DirectorPatronymicName,
             DirectorPositionInGenetive, DirectorPosition, OUConfirmation, AccreditationSertificate, LawAddress, FactAddress,
			 PhoneCityCode, Phone, Fax, EMail, Site, RegionId, TypeId, KindId,
			 RCModel, RCDescription,
			 CNFederalBudget, CNTargeted, CNLocalBudget, CNPaying, CNFullTime, CNEvening, CNPostal,
			 MainId, StatusId, NewOrgId, DepartmentId, DateChangeStatus, Reason, ReceptionOnResultsCNE, 
             LetterToReschedule, LetterToRescheduleName, LetterToRescheduleContentType, TimeConnectionToSecureNetwork, TimeEnterInformationInFIS, 
             ConnectionSchemeId, ConnectionStatusId, IsAgreedTimeConnection, IsAgreedTimeEnterInformation, TownName, ISLOD_GUID, OrganizationISId, IsAnotherName";

        private const string OrgColumnsParameters =
            @"@UpdateDate,@FullName,@ShortName,@INN,@OGRN, @KPP, @OwnerDepartment,@IsPrivate,@IsFilial,
			 @DirectorFullName, @DirectorFullNameInGenetive, @DirectorFirstName, @DirectorLastName, @DirectorPatronymicName,
             @DirectorPositionInGenetive, @DirectorPosition, @OUConfirmation, @AccreditationSertificate,@LawAddress,@FactAddress,
			 @PhoneCityCode,@Phone,@Fax,@EMail,@Site,@RegionId, @TypeId, @KindId,
			 @RCModel, @RCDescription,
			 @CNFederalBudget, @CNTargeted, @CNLocalBudget, @CNPaying, @CNFullTime, @CNEvening, @CNPostal,
			 @MainId, @StatusId, @NewOrgId, @DepartmentId, @DateChangeStatus, @Reason, @ReceptionOnResultsCNE,
             @LetterToReschedule, @LetterToRescheduleName, @LetterToRescheduleContentType, @TimeConnectionToSecureNetwork, 
             @TimeEnterInformationInFIS, @ConnectionSchemeId, @ConnectionStatusId, @IsAgreedTimeConnection, @IsAgreedTimeEnterInformation, @TownName, @ISLOD_GUID, @OrganizationISId, @IsAnotherName";

        private const string SelectOrgStatement =
            @"SELECT 
						O.Id,O.Version,O.FullName,O.ShortName,O.INN,O.OGRN,O.KPP,O.OwnerDepartment,O.IsPrivate,O.IsFilial,
						O.DirectorFullName,O.DirectorFullNameInGenetive,O.DirectorFirstName,O.DirectorLastName,O.DirectorPatronymicName,
                        O.DirectorPositionInGenetive,O.DirectorPosition,O.OUConfirmation,O.AccreditationSertificate,O.LawAddress,O.FactAddress,
						O.PhoneCityCode,O.Phone,O.Fax,O.EMail,O.Site, O.ReceptionOnResultsCNE,
                        Reg.Code as RegionCode,
						Reg.Id as RegionId, Type.Id as TypeId, Kind.Id as KindId,
						Reg.Name as RegionName, Type.Name as TypeName, Kind.Name as KindName,
						O.RCModel as RCModelId, RC.ModelName as RCModelName, O.RCDescription,
                        O.CNFederalBudget, O.CNTargeted, O.CNLocalBudget, O.CNPaying, O.CNFullTime, O.CNEvening, O.CNPostal,
						O.CNFBFullTime, O.CNFBEvening, O.CNFBPostal, O.CNPayFullTime, O.CNPayEvening, O.CNPayPostal,
						O.MainId, MO.FullName as MainFullName, MO.ShortName as MainShortName,
						O.StatusId, Status.Name as StatusName,
						O.NewOrgId, NO.FullName as NewOrgFullName, NO.ShortName as NewOrgShortName,
						O.DepartmentId, DO.FullName as DepartmentFullName, DO.ShortName as DepartmentShortName, 
                        O.UpdateDate, O.CreateDate, O.DateChangeStatus, O.Reason, O.TimeConnectionToSecureNetwork, O.TimeEnterInformationInFIS,
                        O.IsAgreedTimeConnection, O.IsAgreedTimeEnterInformation,
                        sch.Id as ConnectionSchemeId, sch.Name as ConnectionSchemeName, conStatus.Id as ConnectionStatusId, conStatus.Name as ConnectionStatusName,
                        O.LetterToReschedule, O.LetterToRescheduleName, O.LetterToRescheduleContentType, O.TownName, Lic.RegNumber as LicenseRegNumber, Lic.OrderDocumentDate as LicenseOrderDocumentDate, Lic.StatusName as LicenseStatusName, 
                        MO.OrganizationISId, OIS.Name as OrganizationISName, O.IsAnotherName as IsAnotherName
                        Sup.Number AS SupplementNumber, Sup.OrderDocumentDate AS SupplementOrderDocumentDate, Sup.StatusName AS SupplementStatusName{0}
					FROM 
						{1} O
					INNER JOIN 
						dbo.Region Reg ON Reg.Id=O.RegionId 
					INNER JOIN 
						dbo.OrganizationType2010 Type ON Type.Id=O.TypeId
					INNER JOIN 
						dbo.OrganizationKind Kind ON Kind.Id=O.KindId 
                    LEFT JOIN 
                        dbo.ConnectionScheme sch ON sch.Id=O.ConnectionSchemeId
                    LEFT JOIN 
                        dbo.ConnectionStatus conStatus ON conStatus.Id=O.ConnectionStatusId
					LEFT JOIN 
						[dbo].[RecruitmentCampaigns] RC on O.RCModel = RC.Id
					LEFT JOIN 
						[dbo].[OrganizationOperatingStatus] Status on O.StatusId = Status.Id
					LEFT JOIN
						[dbo].[Organization2010] MO on O.MainId = MO.Id
					LEFT JOIN 
						[dbo].[Organization2010] NO on O.NewOrgId = NO.Id
					LEFT JOIN
						[dbo].[Organization2010] DO on O.DepartmentId = DO.Id
                    LEFT JOIN
						[dbo].[OrganizationIS] OIS on OIS.Id = MO.OrganizationISId
                    OUTER APPLY 
		                (SELECT TOP 1 * FROM [dbo].[License] Lic WHERE Lic.OrganizationId = ISNULL(MO.Id,O.Id) ORDER BY Lic.OrderDocumentDate DESC) Lic
	                OUTER APPLY 
		                (SELECT TOP 1 * FROM [dbo].[LicenseSupplement] Sup WHERE Lic.id = Sup.LicenseId AND Sup.OrganizationId = O.Id ORDER BY Sup.OrderDocumentDate DESC) Sup	
					{2}";

        #endregion

        #region Enums

        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        public enum OPF
        {
            /// <summary>
            /// Негосударственный
            /// </summary>
            Private = 1, 

            /// <summary>
            /// Государственный
            /// </summary>
            State = 0, 

            /// <summary>
            /// Неизвестен (не удалось определить)
            /// </summary>
            Undefinded = -1
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Связать заявку на организацию с существующей органзацией
        /// </summary>
        /// <param name="requestId">
        /// </param>
        /// <param name="organizationId">
        /// </param>
        public static void BindRequestToOrganization(int requestId, int organizationId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText =
                    @"UPDATE dbo.OrganizationRequest2010 
                                    SET OrganizationId=@OrganizationId 
                                    WHERE Id=@RequestId";
                cmd.Parameters.AddWithValue("RequestId", requestId);
                cmd.Parameters.AddWithValue("OrganizationId", organizationId);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public static Organization Get(int id)
        {
            Organization result = null;

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SelectInformationOrg";
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        result = new Organization(reader);
                    }
                }

                if (result != null)
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT 
f.Id, ISNULL(f.OrganizationShortName, ISNULL(f.OrganizationFullName, ISNULL(f.PersonLastName,'')+' '+ISNULL(f.PersonFirstName,'')+' '+ISNULL(f.PersonPatronymic,''))) AS Name 
FROM Founder f                   
INNER JOIN OrganizationFounder fo on F.Id = fo.FounderId
WHERE fo.OrganizationId = @id
ORDER BY Name";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Founders.Add(new CatalogElement(reader, "Id", "Name"));
                        }
                    }
                }

                conn.Close();
            } 

            return result;
        }

        public static int? GetStatus(int orgId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "select StatusId from Organization2010 where Id = @id";
                    command.Parameters.Add(new SqlParameter("@id", orgId));

                    conn.Open();
                    int? result = (int?)command.ExecuteScalar();

                    return result;
                }
            }
        }

        /// <summary>
        /// получить версию организации по id 
        /// </summary>
        /// <param name="id">
        /// id организации
        /// </param>
        /// <param name="version">
        /// версия
        /// </param>
        /// <returns>
        /// организация
        /// </returns>
        public static Organization Get(int id, int version)
        {
            if (version <= 0 || id <= 0)
            {
                return null;
            }

            Organization organization = null;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = GenerateOrgSelectStatement(
                    "dbo.OrganizationUpdateHistory", null, "WHERE O.OriginalOrgId=@OrgId AND O.Version=@Version");

                cmd.Parameters.AddWithValue("@Version", version);
                cmd.Parameters.AddWithValue("@OrgId", id);

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        organization = new Organization(reader) { Id = id };
                    }

                    reader.Close();
                }

                conn.Close();
            }

            if (organization != null)
            {
                return organization;
            }

            // возможна запрашивается текущая организация (самой последней версии)
            Organization lastOrg = Get(id);
            if (lastOrg.Version == version)
            {
                return lastOrg;
            }

            return null;
        }

        /// <summary>
        /// The get by login.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        public static Organization GetByLogin(string login)
        {
            OrgUser user = OrgUserDataAccessor.Get(login);

            if (user == null)
            {
                return null;
            }

            return Get(user.RequestedOrganization.OrganizationId ?? 0);
        }

        /// <summary>
        /// The get date updated by login.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <returns>
        /// </returns>
        public static DateTime GetDateUpdatedByLogin(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
                        SELECT 
		                        case 
		                        when max(o.UpdateDate) is null and max(o1.UpdateDate) is null then null
		                        when max(o.UpdateDate) is null and max(o1.UpdateDate) is not null then max(o1.UpdateDate)
		                        when max(o.UPdateDate) is not null and max(o1.updateDate) is null then max(o.UpdateDate)
		                        when max(o.UpdateDate) >= max(o1.UpdateDate) then max(o.UpdateDate)
		                        else max(o1.UpdateDate) end as UpdateDate
						FROM Account a
						JOIN OrganizationRequest2010 r ON r.Id=a.OrganizationId
						left outer JOIN Organization2010 o ON o.Id=r.OrganizationId
						left outer JOIN Organization2010 o1 ON o1.MainId = o.Id
						WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(res);
            }
        }

        /// <summary>
        /// Количество действующих дочерних организаций в базе: для головных организаций – количество филиалов, для учредителей – количество подведомственных организаций (включая их филиалы)
        /// </summary>
        /// <param name="login">Имя учетной записи пользователя</param>
        /// <returns>Количество действующих дочерних организаций в базе: для головных организаций – количество филиалов, для учредителей – количество подведомственных организаций (включая их филиалы)</returns>
        public static int GetNumberOrgByLogin(string login)
        {
            var result = new List<int>();

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandText =
                    @"SELECT o.Id,o.MainId,case when o.TypeId<>6 AND o.DepartmentId is null THEN 0 ELSE o.DepartmentId END as DepartmentId
						FROM Account a
						JOIN OrganizationRequest2010 r ON r.Id=a.OrganizationId
						JOIN Organization2010 o ON o.Id=r.OrganizationId
						WHERE a.[Login] = @login";
                cmd.Parameters.AddWithValue("@login", login);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mainId = reader["MainId"] == DBNull.Value ? -1 : (int)reader["MainId"];
                        var departmentId = reader["DepartmentId"] == DBNull.Value ? -1 : (int)reader["DepartmentId"];
                        result.Add(mainId);
                        result.Add(departmentId);
						result.Add((int)reader["Id"]);
                    }

                    reader.Close();
                }

                conn.Close();
            }
			int countOrganizations=0;
			//Это филиал либо ничего не найдено и ничего не будет
			if(result.Count==0 || result[0]!=-1)
				return countOrganizations;
		    // Это головная организация
			else if(result[1]>=0)
			{
				countOrganizations = GetCountFilialOrgByMainOrgId(result[2]);
			}
			//остается только учредитель
			else 
				countOrganizations = GetCountFilialOrgByFounderId(result[2]);

            return countOrganizations;
        }

        private static int GetCountFilialOrgByMainOrgId(int id)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"SELECT COUNT(Id)
                        FROM Organization2010	
                        WHERE MainId=@Id AND IsFilial=1";
                sqlCommand.Parameters.AddWithValue("@Id", id);
                object res = sqlCommand.ExecuteScalar();

                return (int)res;
            }
        }

        private static int GetCountFilialOrgByFounderId(int id)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                 @"SELECT COUNT(Id)
                        FROM Organization2010	
                        WHERE DepartmentId=@Id";
                sqlCommand.Parameters.AddWithValue("@Id", id);
                object res = sqlCommand.ExecuteScalar();

                return (int)res;
            }
        }

        public static List<int> GetFilialIdByMainOrgId(int id)
        {
           var result = new List<int>();

            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand cmd = Conn.CreateCommand();

                cmd.CommandText =
                    @"SELECT Id
                        FROM Organization2010	
                        WHERE MainId=@Id AND IsFilial=1";
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["Id"]));
                    }

                    reader.Close();
                }

                Conn.Close();
            }

            return result;
        }

        public static List<int> GetFilialIdOrgByFounderId(int id)
        {
            var result = new List<int>();

            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandText =
                 @"SELECT Id
                        FROM Organization2010	
                        WHERE DepartmentId=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(Convert.ToInt32(reader["Id"]));
                    }

                    reader.Close();
                }

                Conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Получает дату последнего обновления данных о учредителе организации
        /// </summary>
        /// <param name="login">Имя учетной записи пользователя</param>
        /// <returns> Дата последнего обновления данных о учредителе организации</returns>
        public static DateTime GetDateUpdatedFounderByLogin(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT d.UpdateDate
						FROM Account a
						JOIN OrganizationRequest2010 r ON r.Id=a.OrganizationId
						JOIN Organization2010 o ON o.Id=r.OrganizationId
                        JOIN Organization2010 d ON d.Id=o.DepartmentId
						WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(res);
            }
        }

        public static DateTime GetDateUpdatedMainOrgByLogin(string login)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
						SELECT d.UpdateDate
						FROM Account a
						JOIN OrganizationRequest2010 r ON r.Id=a.OrganizationId
						JOIN Organization2010 o ON o.Id=r.OrganizationId
                        JOIN Organization2010 d ON d.Id=o.MainId
						WHERE a.[Login] = @login";
                sqlCommand.Parameters.AddWithValue("@login", login);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(res);
            }
        }

        /// <summary>
        /// Получение Id Головной компании по Id организации
        /// </summary>
        /// <param name="orgId">Id организации</param>
        /// <returns>Получение Id Головной компании по Id организации</returns>
        public static int GetMainOrgIdById(int orgId)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"SELECT o.MainId
						FROM Organization2010 o
						where o.Id = @Id";
                sqlCommand.Parameters.AddWithValue("@Id", orgId);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return -1;
                }

                return (int)res;
            }
        }

        /// <summary>
        /// Получение Id Учредителя по Id организации
        /// </summary>
        /// <param name="orgId">Id организации</param>
        /// <returns>Получение Id Учредителя по Id организации</returns>
        public static int GetFounderIdById(int orgId)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"SELECT case when o.TypeId<>6 AND o.DepartmentId is null THEN 0 ELSE o.DepartmentId END
						FROM Organization2010 o
						where o.Id = @Id";
                sqlCommand.Parameters.AddWithValue("@Id", orgId);
                object res = sqlCommand.ExecuteScalar();
                if (res == null || res == DBNull.Value)
                {
                    return -1;
                }

                return (int)res;
            }
        }

        /// <summary>
        /// Получение списка должностей в род. падеже
        /// </summary>
        /// <returns></returns>
        public static List<DirectorPostion> GetDirectorPositionsInGenetive()
        {
            var result = new List<DirectorPostion>();

            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand cmd = Conn.CreateCommand();
                cmd.CommandText =
                 @"SELECT * FROM DirectorPosition";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DirectorPostion
                            {
                                DirectorPositionName = reader["PositionName"].ToString(),
                                DirectorPositionNameInGenetive = reader["PositionNameInGenetive"].ToString(),
                                Id = Convert.ToInt32(reader["Id"])
                            });
                    }

                    reader.Close();
                }

                Conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Получение организационно-правовой формы по старому типу (для обратной соместимости некоторых форм)
        /// </summary>
        /// <param name="oldTypeId">
        /// Устаревший тип (ВУЗ государственный, ВУЗ негосударственный и т.п.)
        /// </param>
        /// <returns>
        /// </returns>
        public static OPF GetIsPrivate(int oldTypeId)
        {
            switch (oldTypeId)
            {
                case 1:
                case 3:
                    return OPF.State; // государственный
                case 2:
                case 4:
                    return OPF.Private; // негосударственный
                default:
                    return OPF.Undefinded; // Другое
            }
        }

        /// <summary>
        /// Получение нового идентификатора типа по старому (для обратной соместимости некоторых форм)
        /// </summary>
        /// <param name="oldTypeId">
        /// Устаревший тип (ВУЗ государственный, ВУЗ негосударственный и т.п.)
        /// </param>
        /// <returns>
        /// Новый тип (ВУЗ,ССУЗ,РЦОИ...)
        /// </returns>
        public static int GetNewTypeId(int oldTypeId)
        {
            switch (oldTypeId)
            {
                case 1:
                case 2:
                    return 1; // ВУЗ
                case 3:
                case 4:
                    return 2; // ССУЗ
                case 5:
                    return 3; // РЦОИ
                case 6:
                    return 4; // Орган уравления образованием
                case 8:
                    return 6; // Учредитель
                default:
                    return 5; // Другое
            }
        }

        /// <summary>
        /// Комбинирование значения типа с организационно-правовой формой (для обратной соместимости некоторых форм)
        /// </summary>
        /// <param name="newTypeId">
        /// </param>
        /// <param name="isPrivate">
        /// </param>
        /// <returns>
        /// The get old type name.
        /// </returns>
        public static string GetOldTypeName(int newTypeId, bool isPrivate)
        {
            switch (newTypeId)
            {
                case 1:
                    return isPrivate ? "ВУЗ&nbsp;негосударственный" : "ВУЗ&nbsp;государственный";
                case 2:
                    return isPrivate ? "CCУЗ&nbsp;негосударственный" : "CCУЗ&nbsp;государственный";
                case 3:
                    return "РЦОИ";
                case 4:
                    return "Орган управления образованием";
                case 6:
                    return "Учредитель";
                default:
                    return "Другое";
            }
        }

        /// <summary>
        /// The get types.
        /// </summary>
        /// <param name="zeroItem">
        /// The zero item.
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable GetTypes(string zeroItem)
        {
            var result = new DataTable();
            result.Columns.Add("Id", typeof(int));
            result.Columns.Add("Name", typeof(string));

            DataRow row;

            if (!string.IsNullOrEmpty(zeroItem))
            {
                row = result.NewRow();
                row["Id"] = 0;
                row["Name"] = zeroItem;
                result.Rows.Add(row);
            }

            row = result.NewRow();
            row["Id"] = 1;
            row["Name"] = "ВУЗ государственный";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 2;
            row["Name"] = "ВУЗ негосударственный";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 3;
            row["Name"] = "ССУЗ государственный";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 4;
            row["Name"] = "ССУЗ негосударственный";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 5;
            row["Name"] = "РЦОИ";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 6;
            row["Name"] = "Орган управления образованием";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 8;
            row["Name"] = "Учредитель";
            result.Rows.Add(row);

            row = result.NewRow();
            row["Id"] = 7;
            row["Name"] = "Другое";
            result.Rows.Add(row);

            return result;
        }

        /// <summary>
        /// The get with parent.
        /// </summary>
        /// <returns>
        /// </returns>
        public static List<OrgWithParent> GetWithParent()
        {
            var result = new List<OrgWithParent>();

            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();

                SqlCommand cmd = Conn.CreateCommand();

                cmd.CommandText =
                    @"
                        select O.Id, O.TypeId, O.IsPrivate, O.MainId, O.ShortName, O.FullName, O.RegionId from Organization2010 O
                        order by O.ShortName
                    ";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(
                            new OrgWithParent
                                {
                                    Id = Convert.ToInt32(reader["Id"]), 
                                    MainId =
                                        reader["MainId"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["MainId"]), 
                                    ShortName =
                                        reader["ShortName"] == DBNull.Value ? null : reader["ShortName"].ToString(), 
                                    FullName = reader["FullName"] == DBNull.Value ? null : reader["FullName"].ToString(), 
                                    RegionId = Convert.ToInt32(reader["RegionId"]), 
                                    TypeId = Convert.ToInt32(reader["TypeId"]), 
                                    IsPrivate = Convert.ToBoolean(reader["IsPrivate"])
                                });
                    }

                    reader.Close();
                }

                Conn.Close();
            }

            return result;
        }

        /// <summary>
        /// The update or create.
        /// </summary>
        /// <param name="organization">
        /// The organization.
        /// </param>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <returns>
        /// The update or create.
        /// </returns>
        public static int UpdateOrCreate(Organization organization, string userName)
        {
            Organization currentVersion = null;
            if (organization.Id > 0)
            {
                currentVersion = Get(organization.Id);
            }

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                AddOrganizationValues(cmd.Parameters, organization);

                bool isNew = (organization.Id == 0);
                if (isNew)
                {
                    cmd.CommandText =
                        string.Format(
                            @"
						INSERT INTO 
							dbo.Organization2010 
							({0})
						VALUES 
							({1})",
                            OrgColumnsNames,
                            OrgColumnsParameters);
                }
                else
                {
                    cmd.CommandText =
                        @"
						UPDATE 
							dbo.Organization2010 
						SET 
							Version=@Version,UpdateDate=@UpdateDate,FullName=@FullName,ShortName=@ShortName,
							INN=@INN,OGRN=@OGRN,KPP=@KPP,OwnerDepartment=@OwnerDepartment,IsPrivate=@IsPrivate,IsFilial=@IsFilial,
							DirectorFullName=@DirectorFullName,DirectorFullNameInGenetive=@DirectorFullNameInGenetive,
                            DirectorPosition=@DirectorPosition,DirectorPositionInGenetive=@DirectorPositionInGenetive,
                            DirectorFirstName=@DirectorFirstName,DirectorLastName=@DirectorLastName,DirectorPatronymicName=@DirectorPatronymicName,
							OUConfirmation=@OUConfirmation,AccreditationSertificate=@AccreditationSertificate,LawAddress=@LawAddress,
							FactAddress=@FactAddress,PhoneCityCode=@PhoneCityCode,Phone=@Phone,Fax=@Fax,EMail=@EMail,Site=@Site,
							RegionId=@RegionId, TypeId=@TypeId, KindId=@KindId,
							RCModel=@RCModel, RCDescription=@RCDescription,
							CNFederalBudget=@CNFederalBudget, CNTargeted=@CNTargeted, CNLocalBudget=@CNLocalBudget, 
							CNPaying=@CNPaying, CNFullTime=@CNFullTime, CNEvening=@CNEvening, CNPostal=@CNPostal,
							MainId=@MainId, StatusId=@StatusId, NewOrgId=@NewOrgId, DepartmentId=@DepartmentId, 
                            DateChangeStatus=@DateChangeStatus, Reason=@Reason, ReceptionOnResultsCNE=@ReceptionOnResultsCNE,
                            LetterToReschedule=@LetterToReschedule, LetterToRescheduleName=@LetterToRescheduleName, LetterToRescheduleContentType=@LetterToRescheduleContentType, 
                            TimeConnectionToSecureNetwork=@TimeConnectionToSecureNetwork,
                            TimeEnterInformationInFIS=@TimeEnterInformationInFIS, ConnectionSchemeId=@ConnectionSchemeId, ConnectionStatusId=@ConnectionStatusId,
							IsAgreedTimeConnection=@IsAgreedTimeConnection, IsAgreedTimeEnterInformation=@IsAgreedTimeEnterInformation, TownName=@TownName, ISLOD_GUID = @ISLOD_GUID,
                            OrganizationISId=@OrganizationISId, IsAnotherName = @IsAnotherName,
                            UpdatedByUser = 1
						WHERE 
							Id=@Id";
                    cmd.Parameters.AddWithValue("Id", organization.Id);
                    cmd.Parameters.AddWithValue("Version", currentVersion.Version + 1);
                }

                cmd.ExecuteNonQuery();

                if (isNew)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT @@Identity";
                    organization.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("organizationId", organization.Id);
                if (organization.FounderIds.Any())
                {
                    string founderIdsIn = String.Join(",", organization.FounderIds.Select(x=>x.ToString()).ToArray());
                    cmd.CommandText = String.Format("DELETE FROM OrganizationFounder WHERE (OrganizationId = @organizationId) AND (FounderId NOT IN ({0}))", founderIdsIn);
                }
                else
                {
                    cmd.CommandText = "DELETE FROM OrganizationFounder WHERE (OrganizationId = @organizationId)";
                }
                cmd.ExecuteNonQuery();

                cmd.Parameters.AddWithValue("founderId", DBNull.Value);
                foreach (int founderId in organization.FounderIds)
                {
                    cmd.Parameters["founderId"].Value = founderId;
                    cmd.CommandText = @"IF (NOT EXISTS (SELECT * FROM OrganizationFounder WHERE (OrganizationId = @organizationId) AND (FounderId = @founderId))) 
BEGIN 
    INSERT INTO OrganizationFounder(OrganizationId, FounderId) VALUES (@organizationId, @founderId)
END";
                    cmd.ExecuteNonQuery();
                } 

                conn.Close();
            }

            if (currentVersion != null)
            {
                WriteOrgUpdateHistory(currentVersion, organization, userName);
            }

            return organization.Id;
        }

        /// <summary>
        /// получить историю изменений организации с поддержкой пейджинга
        /// </summary>
        /// <param name="organizationId">
        /// id организации
        /// </param>
        /// <param name="startRow">
        /// индекс первой записи (с учетом сортировки по версии)
        /// </param>
        /// <param name="maxRow">
        /// кол-во записей
        /// </param>
        /// <returns>
        /// список записей истории
        /// </returns>
        public List<OrganizationUpdateHistoryEntry> SelectOrgUpdateHistory(int organizationId, int startRow, int maxRow)
        {
            var result = new List<OrganizationUpdateHistoryEntry>();

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                string selectWithRowNumber = GenerateOrgSelectStatement(
                    "dbo.OrganizationUpdateHistory", 
                    "O.OriginalOrgId, O.UpdateDescription, O.EditorUserName, row_number() over(order by O.Version desc) as rn", 
                    "WHERE OriginalOrgId=@OriginalOrgId");

                // пейджеры иногда присылают 0
                if (startRow == 0)
                {
                    startRow++;
                }

                string selectWithPaging =
                    string.Format(
                        "select * from ({0}) rowNumbered where rn between {1} and {2} order by rn", 
                        selectWithRowNumber, 
                        startRow, 
                        startRow - 1 + maxRow);

                cmd.CommandText = selectWithPaging;
                cmd.Parameters.AddWithValue("@OriginalOrgId", organizationId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var organization = new Organization(reader);
                        var historyEntry = new OrganizationUpdateHistoryEntry
                            {
                                Organization = organization, 
                                Id = (int)reader["Id"], 
                                UpdateDescription = (string)reader["UpdateDescription"], 
                                EditorName =
                                    reader["EditorUserName"] is DBNull
                                        ? "<i>SYSTEM</i>"
                                        : (string)reader["EditorUserName"]
                            };
                        var originalOrgId = (int)reader["OriginalOrgId"];
                        organization.Id = originalOrgId;
                        result.Add(historyEntry);
                    }

                    reader.Close();
                }

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// получить кол-во записей в истории изменений организации
        /// </summary>
        /// <param name="organizationId">
        /// id организации
        /// </param>
        /// <returns>
        /// кол-во записей в истории изменений организации
        /// </returns>
        public int SelectOrgUpdateHistoryCount(int organizationId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                string generateOrgSelectStatement = GenerateOrgSelectStatement(
                    "dbo.OrganizationUpdateHistory", null, "WHERE O.OriginalOrgId=@OriginalOrgId");
                cmd.CommandText = string.Format("select count(*) from ({0}) a", generateOrgSelectStatement);
                cmd.Parameters.AddWithValue("@OriginalOrgId", organizationId);

                var selectOrgUpdateHistoryCount = (int)cmd.ExecuteScalar();
                conn.Close();
                return selectOrgUpdateHistoryCount;
            }
        }

        #endregion

        #region Methods

        private static void AddOrganizationValues(SqlParameterCollection parameters, Organization organization)
        {
            parameters.AddWithValue("DateChangeStatus", NullToDBNull(organization.DateChangeStatus));

            parameters.AddWithValue("Reason", organization.Reason);
            parameters.AddWithValue("UpdateDate", DateTime.Now);
            parameters.AddWithValue("FullName", organization.FullName);
            parameters.AddWithValue("ShortName", organization.ShortName);

            parameters.AddWithValue("INN", NullToDBNull(organization.INN));

            parameters.AddWithValue("OGRN", NullToDBNull(organization.OGRN));

            parameters.AddWithValue("KPP", NullToDBNull(organization.KPP));

            parameters.AddWithValue("OwnerDepartment", organization.OwnerDepartment);
            parameters.AddWithValue("IsPrivate", organization.IsPrivate);
            parameters.AddWithValue("IsFilial", organization.IsFilial);
            parameters.AddWithValue("DirectorFullName", organization.DirectorFullName);
            parameters.AddWithValue("DirectorFullNameInGenetive", organization.DirectorFullNameInGenetive);
            parameters.AddWithValue("DirectorFirstName", organization.DirectorFirstName);
            parameters.AddWithValue("DirectorLastName", organization.DirectorLastName);
            parameters.AddWithValue("DirectorPatronymicName", organization.DirectorPatronymicName);
            parameters.AddWithValue("DirectorPosition", organization.DirectorPosition);
            parameters.AddWithValue("DirectorPositionInGenetive", organization.DirectorPositionInGenetive);
            parameters.AddWithValue("OUConfirmation", organization.OUConfirmation);
            parameters.AddWithValue("AccreditationSertificate", organization.AccreditationSertificate);
            parameters.AddWithValue("LawAddress", organization.LawAddress);

            parameters.AddWithValue("TownName", NullToDBNull(organization.TownName));

            parameters.AddWithValue("FactAddress", organization.FactAddress);
            parameters.AddWithValue("PhoneCityCode", organization.PhoneCityCode);
            parameters.AddWithValue("Phone", organization.Phone);
            parameters.AddWithValue("Fax", organization.Fax);
            parameters.AddWithValue("EMail", organization.EMail);
            parameters.AddWithValue("Site", organization.Site);
            parameters.AddWithValue("RegionId", NullToDBNull(organization.Region.Id));
            parameters.AddWithValue("TypeId", NullToDBNull(organization.OrgType.Id));
            parameters.AddWithValue("KindId", NullToDBNull(organization.Kind.Id));

            if (organization.LetterToReschedule == null)
            {
                parameters.Add(new SqlParameter("@LetterToReschedule", SqlDbType.Image));
                parameters["@LetterToReschedule"].Value = DBNull.Value;
                parameters.AddWithValue("@LetterToRescheduleName", DBNull.Value);
                parameters.AddWithValue("@LetterToRescheduleContentType", DBNull.Value);
            }
            else
            {
                parameters.Add(new SqlParameter("@LetterToReschedule", SqlDbType.Image));
                parameters["@LetterToReschedule"].Value = organization.LetterToReschedule;
                parameters.AddWithValue("@LetterToRescheduleName", organization.LetterToRescheduleName);
                parameters.AddWithValue("@LetterToRescheduleContentType", organization.LetterToRescheduleContentType);
            }

            if (organization.TimeConnectionToSecureNetwork == null)
            {
                parameters.AddWithValue("TimeConnectionToSecureNetwork", DBNull.Value);
                parameters.AddWithValue("IsAgreedTimeConnection", DBNull.Value);
            }
            else
            {
                parameters.AddWithValue("TimeConnectionToSecureNetwork", organization.TimeConnectionToSecureNetwork);
                parameters.AddWithValue("IsAgreedTimeConnection", organization.IsAgreedTimeConnection);
            }

            if (organization.TimeEnterInformationInFIS == null)
            {
                parameters.AddWithValue("TimeEnterInformationInFIS", DBNull.Value);
                parameters.AddWithValue("IsAgreedTimeEnterInformation", DBNull.Value);
            }
            else
            {
                parameters.AddWithValue("TimeEnterInformationInFIS", organization.TimeEnterInformationInFIS);
                parameters.AddWithValue("IsAgreedTimeEnterInformation", organization.IsAgreedTimeEnterInformation);
            }

            parameters.AddWithValue("ConnectionSchemeId", organization.ConnectionScheme.Id.GetValueOrDefault(1));

            parameters.AddWithValue("ConnectionStatusId", organization.ConnectionStatus.Id.GetValueOrDefault(1));

            parameters.AddWithValue("RCModel", NullToDBNull(organization.RCModelId));

            parameters.AddWithValue("RCDescription", NullToDBNull(organization.RCDescription));


            parameters.AddWithValue("CNFederalBudget", organization.CNFederalBudget);
            parameters.AddWithValue("CNTargeted", organization.CNTargeted);
            parameters.AddWithValue("CNLocalBudget", organization.CNLocalBudget);
            parameters.AddWithValue("CNPaying", organization.CNPaying);
            parameters.AddWithValue("CNFullTime", organization.CNFullTime);
            parameters.AddWithValue("CNEvening", organization.CNEvening);
            parameters.AddWithValue("CNPostal", organization.CNPostal);

            parameters.AddWithValue("MainId", NullToDBNull(organization.MainId));

            parameters.AddWithValue("StatusId", NullToDBNull(organization.Status.Id));

            parameters.AddWithValue("NewOrgId", NullToDBNull(organization.NewOrgId));

            parameters.AddWithValue("DepartmentId", NullToDBNull(organization.DepartmentId));

            parameters.AddWithValue("ReceptionOnResultsCNE", NullToDBNull(organization.ReceptionOnResultsCNE));

            parameters.AddWithValue("ISLOD_GUID", organization.ISLOD_GUID);
            parameters.AddWithValue("OrganizationISId", NullToDBNull(organization.IS.Id));
            parameters.AddWithValue("IsAnotherName", NullToDBNull(organization.AnotherName));
            
        }

        private static object NullToDBNull(object value)
        {
            if (value == null)
                return DBNull.Value;
            return value;
        }

        /// <summary>
        /// сравнение двух организаций
        /// </summary>
        /// <param name="source">
        /// исходная организация
        /// </param>
        /// <param name="target">
        /// новая организация
        /// </param>
        /// <returns>
        /// текстовое описание разницы
        /// </returns>
        private static string CompareOrgs(Organization source, Organization target)
        {
            var changes = new StringBuilder();

            if (!StringsAreEqueal(source.FullName,target.FullName))
            {
                changes.AppendLine("Полное наименование; ");
            }

            if (!StringsAreEqueal(source.ShortName,target.ShortName))
            {
                changes.AppendLine("Краткое наименование; ");
            }

            if (source.Region.Id != target.Region.Id)
            {
                changes.AppendLine("Регион; ");
            }

            if (source.OrgType.Id != target.OrgType.Id)
            {
                changes.AppendLine("Тип; ");
            }

            if (source.Kind.Id != target.Kind.Id)
            {
                changes.AppendLine("Вид;  ");
            }

            if (!StringsAreEqueal(source.INN, target.INN))
            {
                changes.AppendLine("ИНН; ");
            }

            if (!StringsAreEqueal(source.OGRN, target.OGRN))
            {
                changes.AppendLine("ОГРН; ");
            }

            if (!StringsAreEqueal(source.KPP, target.KPP))
            {
                changes.AppendLine("КПП; ");
            }

            if (target.DepartmentId != -1)
            {
                if (source.DepartmentId != target.DepartmentId)
                {
                    changes.AppendLine("Учредитель; ");
                }
            }

            if (source.IsPrivate != target.IsPrivate)
            {
                changes.AppendLine("Организационно-правовая форма ");
            }

            if (source.IsFilial != target.IsFilial)
            {
                changes.AppendLine("Является филиалом; ");
            }

            if (!StringsAreEqueal(source.DirectorPosition, target.DirectorPosition))
            {
                changes.AppendLine("Должность руководителя; ");
            }

            if (!StringsAreEqueal(source.DirectorPositionInGenetive, target.DirectorPositionInGenetive))
            {
                changes.AppendLine("Должность руководителя (род. падеж); ");
            }

            if (!StringsAreEqueal(source.DirectorFullName, target.DirectorFullName))
            {
                changes.AppendLine("ФИО руководителя; ");
            }

            if (!StringsAreEqueal(source.DirectorFirstName, target.DirectorFirstName))
            {
                changes.AppendLine("Имя руководителя; ");
            }

            if (!StringsAreEqueal(source.DirectorLastName, target.DirectorLastName))
            {
                changes.AppendLine("Фамилия руководителя; ");
            }

            if (!StringsAreEqueal(source.DirectorPatronymicName, target.DirectorPatronymicName))
            {
                changes.AppendLine("Отчество руководителя; ");
            }

            if (!StringsAreEqueal(source.DirectorFullNameInGenetive, target.DirectorFullNameInGenetive))
            {
                changes.AppendLine("ФИО руководителя (род. падеж); ");
            }

            if (!StringsAreEqueal(source.AccreditationSertificate, target.AccreditationSertificate))
            {
                changes.AppendLine("Свидетельство об аккредитации;");
            }

            if (!StringsAreEqueal(source.LawAddress, target.LawAddress))
            {
                changes.AppendLine("Юридический адрес; ");
            }

            if (!StringsAreEqueal(source.TownName, target.TownName))
            {
                changes.AppendLine("Город; ");
            }

            if (!StringsAreEqueal(source.FactAddress, target.FactAddress))
            {
                changes.AppendLine("Фактический адрес; ");
            }

            if (!StringsAreEqueal(source.PhoneCityCode, target.PhoneCityCode))
            {
                changes.AppendLine("Код города; ");
            }

            if (!StringsAreEqueal(source.Phone, target.Phone))
            {
                changes.AppendLine("Телефон; ");
            }

            if (!StringsAreEqueal(source.Fax, target.Fax))
            {
                changes.AppendLine("Факс; ");
            }

            if (!StringsAreEqueal(source.EMail, target.EMail))
            {
                changes.AppendLine("E-mail; ");
            }

            if (!StringsAreEqueal(source.Site, target.Site))
            {
                changes.AppendLine("Сайт; ");
            }

            if (source.CNFederalBudget != target.CNFederalBudget)
            {
                changes.AppendLine(
                    "Контрольные цифры приема граждан, обучающихся за счет средств федерального бюджета; ");
            }

            if (source.CNTargeted != target.CNTargeted)
            {
                changes.AppendLine("Квоты по целевому приему;");
            }

            if (source.CNLocalBudget != target.CNLocalBudget)
            {
                changes.AppendLine(
                    "Объем и структура приема обучающихся за счет средств бюджета субъектов Российской Федерации; ");
            }

            if (source.CNPaying != target.CNPaying)
            {
                changes.AppendLine("Количество мест для обучения на основе договоров с оплатой стоимости обучения; ");
            }

            if (source.CNFullTime != target.CNFullTime)
            {
                changes.AppendLine("Количество мест, выделенных для приема на очную форму обучения; ");
            }

            if (source.CNEvening != target.CNEvening)
            {
                changes.AppendLine("Количество мест, выделенных для приема на очно-заочную форму обучения; ");
            }

            if (source.CNPostal != target.CNPostal)
            {
                changes.AppendLine("Количество мест, выделенных для приема на заочную форму обучения; ");
            }

            if (source.RCModelId != target.RCModelId)
            {
                changes.AppendLine("Модель приемной кампании; ");
            }

            if (!StringsAreEqueal(source.RCDescription, target.RCDescription))
            {
                changes.AppendLine("Модель приемной кампании (указанное пользователем); ");
            }

            if (source.MainId != target.MainId)
            {
                changes.AppendLine("Головная организация; ");
            }

            if (source.Status.Id != target.Status.Id)
            {
                changes.Append("Статус; Фактическая дата изменения статуса; Обоснование; ");
            }

            if (source.ReceptionOnResultsCNE != target.ReceptionOnResultsCNE)
            {
                changes.Append("Прием по результатам ЕГЭ; ");
            }

            if (source.ConnectionScheme.Id != target.ConnectionScheme.Id)
            {
                changes.Append("Схема подключения; ");
            }

            if (source.ConnectionStatus.Id != target.ConnectionStatus.Id)
            {
                changes.Append("Статус подключения; ");
            }

            if (source.TimeConnectionToSecureNetwork != target.TimeConnectionToSecureNetwork)
            {
                changes.Append("Срок подключения к защищенной сети; ");
            }

            if (source.TimeConnectionToSecureNetwork != target.TimeConnectionToSecureNetwork)
            {
                changes.Append("Срок внесения сведений в ФИС ЕГЭ и приема; ");
            }
			if (source.IsAgreedTimeConnection != target.IsAgreedTimeConnection)
            {
                changes.Append("Признак согласования срока подключения к защищенной сети; ");
            }

            if (source.IsAgreedTimeEnterInformation != target.IsAgreedTimeEnterInformation)
            {
                changes.Append("Признак согласования cрока внесения сведений в ФИС ЕГЭ и приема; ");
            }


            if (source.LetterToReschedule != null)
            {
                if (target.LetterToReschedule != null && !source.LetterToReschedule.SequenceEqual(target.LetterToReschedule))
                {
                    changes.Append("Письмо о переносе сроков; ");
                }

                if (target.LetterToReschedule == null)
                {
                    changes.Append("Письмо о переносе сроков; ");
                }
            }
            else
            {
                if (target.LetterToReschedule != null)
                {
                    changes.Append("Письмо о переносе сроков; ");
                }
            }

            if (source.IS.Id != target.IS.Id)
            {
                changes.AppendLine("Информационная система (ИС), используемая в ОО; ");
            }
             
            var ch = changes.ToString();
            if (ch == string.Empty)
            {
                return "Изменений не найдено";
            }

            return ch;
        }

        /// <summary>
        /// нужен вариант сравнения строк именно с учетом пустых и null строк
        /// </summary>
        /// <param name="str1">первая строка для сравнения</param>
        /// <param name="str2">вторая строка для сравнения</param>
        /// <returns>равны или нет</returns>
        private static bool StringsAreEqueal(string str1, string str2)
        {
            if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
            {
                return true;
            }

            return string.Compare(str1, str2, StringComparison.InvariantCulture) == 0;
        }

        private static string GenerateOrgSelectStatement(string tableName, string additionalColumns, string whereClause)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName cannot be empty");
            }

            string addColumns = string.IsNullOrEmpty(additionalColumns) ? string.Empty : "," + additionalColumns;

            return string.Format(SelectOrgStatement, addColumns, tableName, whereClause);
        }

        /// <summary>
        /// Получить изменения в организации и записать их в таблицу истории организации
        /// </summary>
        /// <param name="org">
        /// текущая версия организации
        /// </param>
        /// <param name="updatedOrg">
        /// измененная версия организации
        /// </param>
        /// <param name="userName">
        /// The user Name.
        /// </param>
        private static void WriteOrgUpdateHistory(Organization org, Organization updatedOrg, string userName)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                string orgColumns = OrgColumnsNames.Replace(", ISLOD_GUID", string.Empty);
                string orgParams = OrgColumnsParameters.Replace(", @ISLOD_GUID", string.Empty);

                cmd.CommandText =
                    string.Format(
                        @"
						INSERT INTO 
							dbo.[OrganizationUpdateHistory] 
							([OriginalOrgId], [UpdateDescription], [Version], [EditorUserName], CreateDate, {0})
						VALUES 
							(@OriginalOrgId, @UpdateDescription, @Version, @EditorUserName, @CreateDate, {1})",
                        orgColumns,
                        orgParams);

                cmd.Parameters.AddWithValue("OriginalOrgId", org.Id);
                cmd.Parameters.AddWithValue("CreateDate", org.CreateDate);
                cmd.Parameters.AddWithValue("Version", org.Version);
                cmd.Parameters.AddWithValue("EditorUserName", userName == string.Empty ? "SYSTEM" : userName);

                string diff = CompareOrgs(org, updatedOrg);
                cmd.Parameters.AddWithValue("UpdateDescription", diff);

                AddOrganizationValues(cmd.Parameters, org);
                cmd.Parameters.Remove(cmd.Parameters["ISLOD_GUID"]);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #endregion

        public sealed class DirectorPostion
        {
            public string DirectorPositionName { get; set; }

            public string DirectorPositionNameInGenetive { get; set; }

            public int Id { get; set; }
        }

        /// <summary>
        /// The table columns.
        /// </summary>
        internal sealed class TableColumns
        {
            #region Constants and Fields

            /// <summary>
            /// The accreditation sertificate.
            /// </summary>
            public const string AccreditationSertificate = "AccreditationSertificate";

            /// <summary>
            /// The cn evening.
            /// </summary>
            public const string CNEvening = "CNEvening";

            // Сведения об объеме и структуре приема
            /// <summary>
            /// The cn federal budget.
            /// </summary>
            public const string CNFederalBudget = "CNFederalBudget";

            /// <summary>
            /// The cn full time.
            /// </summary>
            public const string CNFullTime = "CNFullTime";

            /// <summary>
            /// The cn local budget.
            /// </summary>
            public const string CNLocalBudget = "CNLocalBudget";

            /// <summary>
            /// The cn paying.
            /// </summary>
            public const string CNPaying = "CNPaying";

            /// <summary>
            /// The cn postal.
            /// </summary>
            public const string CNPostal = "CNPostal";

            /// <summary>
            /// The cn targeted.
            /// </summary>
            public const string CNTargeted = "CNTargeted";

            /// <summary>
            /// The create date.
            /// </summary>
            public const string CreateDate = "CreateDate";

            /// <summary>
            /// The date change status.
            /// </summary>
            public const string DateChangeStatus = "DateChangeStatus";

            /// <summary>
            /// The department full name.
            /// </summary>
            public const string DepartmentFullName = "DepartmentFullName";

            /// <summary>
            /// The department id.
            /// </summary>
            public const string DepartmentId = "DepartmentId";

            /// <summary>
            /// The department short name.
            /// </summary>
            public const string DepartmentShortName = "DepartmentShortName";

            /// <summary>
            /// The director full name.
            /// </summary>
            public const string DirectorFullName = "DirectorFullName";

            /// <summary>
            /// The director full name in genetive
            /// </summary>
            public const string DirectorFullNameInGenetive = "DirectorFullNameInGenetive";

            /// <summary>
            /// The director first name
            /// </summary>
            public const string DirectorFirstName = "DirectorFirstName";

            /// <summary>
            /// The director last name
            /// </summary>
            public const string DirectorLastName = "DirectorLastName";

            /// <summary>
            /// The director patronymic name
            /// </summary>
            public const string DirectorPatronymicName = "DirectorPatronymicName";

            /// <summary>
            /// The director position.
            /// </summary>
            public const string DirectorPosition = "DirectorPosition";

            /// <summary>
            /// The director position in genetive.
            /// </summary>
            public const string DirectorPositionInGenetive = "DirectorPositionInGenetive";

            /// <summary>
            /// Confirmation
            /// </summary>
            public const string OUConfirmation = "OUConfirmation";

            /// <summary>
            /// The e mail.
            /// </summary>
            public const string EMail = "EMail";

            /// <summary>
            /// The fact address.
            /// </summary>
            public const string FactAddress = "FactAddress";

            /// <summary>
            /// The fax.
            /// </summary>
            public const string Fax = "Fax";

            /// <summary>
            /// The full name.
            /// </summary>
            public const string FullName = "FullName";

            /// <summary>
            /// Прием по результатам ЕГЭ
            /// </summary>
            public const string ReceptionOnResultsCNE = "ReceptionOnResultsCNE";

            /// <summary>
            /// ИНН
            /// </summary>
            public const string INN = "INN";

            /// <summary>
            /// КПП
            /// </summary>
            public const string KPP = "KPP";

            /// <summary>
            /// The id.
            /// </summary>
            public const string Id = "Id";

            /// <summary>
            /// The is filial.
            /// </summary>
            public const string IsFilial = "IsFilial";

            /// <summary>
            /// The is private.
            /// </summary>
            public const string IsPrivate = "IsPrivate";

            /// <summary>
            /// The kind id.
            /// </summary>
            public const string KindId = "KindId";

            /// <summary>
            /// ИС ОО
            /// </summary>
            public const string ISId = "OrganizationISId";
            /// <summary>
            /// Идентификатор схемы подключения
            /// </summary>
            public const string ConnectionSchemeId = "ConnectionSchemeId";

            /// <summary>
            /// Идентификатор статуса подключения
            /// </summary>
            public const string ConnectionStatusId = "ConnectionStatusId";

            public const string ConnectionSchemeName = "ConnectionSchemeName";

            public const string ConnectionStatusName = "ConnectionStatusName";

            /// <summary>
            /// Срок подключения к защищенной сети
            /// </summary>
            public const string TimeConnectionToSecureNetwork = "TimeConnectionToSecureNetwork";

            /// <summary>
            /// Срок внесения сведений в ФИС ЕГЭ и приема
            /// </summary>
            public const string TimeEnterInformationInFIS = "TimeEnterInformationInFIS";

            /// <summary>
            /// Письмо о переносе сроков (в виде байтов)
            /// </summary>
            public const string LetterToReschedule = "LetterToReschedule";

            /// <summary>
            /// Письмо о переносе сроков (имя)
            /// </summary>
            public const string LetterToRescheduleName = "LetterToRescheduleName";

            /// <summary>
            /// Письмо о переносе сроков (тип)
            /// </summary>
            public const string LetterToRescheduleContentType = "LetterToRescheduleContentType";

            /// <summary>
            /// The kind name.
            /// </summary>
            public const string KindName = "KindName";

            /// <summary>
            /// The IS name.
            /// </summary>
            public const string ISName = "OrganizationISName";
            /// <summary>
            /// The law address.
            /// </summary>
            public const string LawAddress = "LawAddress";

            /// <summary>
            /// The town name
            /// </summary>
            public const string TownName = "TownName";

            /// <summary>
            /// The main full name.
            /// </summary>
            public const string MainFullName = "MainFullName";

            /// <summary>
            /// The main id.
            /// </summary>
            public const string MainId = "MainId";

            /// <summary>
            /// The main short name.
            /// </summary>
            public const string MainShortName = "MainShortName";

            /// <summary>
            /// The new org full name.
            /// </summary>
            public const string NewOrgFullName = "NewOrgFullName";

            /// <summary>
            /// The new org id.
            /// </summary>
            public const string NewOrgId = "NewOrgId";

            /// <summary>
            /// The new org short name.
            /// </summary>
            public const string NewOrgShortName = "NewOrgShortName";

            /// <summary>
            /// The ogrn.
            /// </summary>
            public const string OGRN = "OGRN";

            /// <summary>
            /// The owner department.
            /// </summary>
            public const string OwnerDepartment = "OwnerDepartment";

            /// <summary>
            /// The phone.
            /// </summary>
            public const string Phone = "Phone";

            /// <summary>
            /// The phone city code.
            /// </summary>
            public const string PhoneCityCode = "PhoneCityCode";

            /// <summary>
            /// The rc description.
            /// </summary>
            public const string RCDescription = "RCDescription";

            /// <summary>
            /// The rc model id.
            /// </summary>
            public const string RCModelId = "RCModelId";

            /// <summary>
            /// The rc model name.
            /// </summary>
            public const string RCModelName = "RCModelName";

            /// <summary>
            /// The reason.
            /// </summary>
            public const string Reason = "Reason";

            /// <summary>
            /// The region code.
            /// </summary>
            public const string RegionCode = "RegionCode";

            /// <summary>
            /// The region id.
            /// </summary>
            public const string RegionId = "RegionId";

            /// <summary>
            /// The region name.
            /// </summary>
            public const string RegionName = "RegionName";

            /// <summary>
            /// The short name.
            /// </summary>
            public const string ShortName = "ShortName";

            /// <summary>
            /// The site.
            /// </summary>
            public const string Site = "Site";

            /// <summary>
            /// The status id.
            /// </summary>
            public const string StatusId = "StatusId";

            /// <summary>
            /// The status name.
            /// </summary>
            public const string StatusName = "StatusName";

            /// <summary>
            /// The type id.
            /// </summary>
            public const string TypeId = "TypeId";

            /// <summary>
            /// The type name.
            /// </summary>
            public const string TypeName = "TypeName";

            /// <summary>
            /// The update date.
            /// </summary>
            public const string UpdateDate = "UpdateDate";

            /// <summary>
            /// The version.
            /// </summary>
            public const string Version = "Version";

            /// <summary>
            /// Признак согласования срока подключения к защищенной сети
            /// </summary>
            public const string IsAgreedTimeConnection = "IsAgreedTimeConnection";

            /// <summary>
            ///  Признак согласования срока внесения сведений в ФИС приема
            /// </summary>
            public const string IsAgreedTimeEnterInformation = "IsAgreedTimeEnterInformation";


            public const string ISLOD_GUID = "ISLOD_GUID";

            public const string LicenseRegNumber = "LicenseRegNumber";

            public const string LicenseOrderDocumentDate = "LicenseOrderDocumentDate";

            public const string LicenseStatusName = "LicenseStatusName";

            // FIS-1777 - added by akopylov 30.10.2017
            public const string SupplementNumber = "SupplementNumber";

            public const string SupplementOrderDocumentDate = "SupplementOrderDocumentDate";

            public const string SupplementStatusName = "SupplementStatusName";

            public const string IsAnotherName = "IsAnotherName";
            #endregion
        }
    }
}