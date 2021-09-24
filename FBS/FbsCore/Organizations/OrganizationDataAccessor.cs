namespace Fbs.Core.Organizations
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.SqlClient;
	using System.Text;

	using Fbs.Core.Users;

	/// <summary>
	/// Класс доступа к организации
	/// </summary>
	public class OrganizationDataAccessor
	{
		#region Constants and Fields

		private const string InsertNotESRPOrgColumnsNames =
			@"RCModel, RCDescription,
							   CNFBFullTime, CNFBEvening, CNFBPostal, CNPayFullTime, CNPayEvening, CNPayPostal";

		private const string InsertNotESRPOrgColumnsParameters =
			@"@RCModel, @RCDescription,
							  @CNFBFullTime, @CNFBEvening, @CNFBPostal, @CNPayFullTime, @CNPayEvening, @CNPayPostal";

		private const string InsertOrgColumnsNames =
			@"UpdateDate,FullName,ShortName,INN,OGRN,OwnerDepartment,IsPrivate,IsFilial,
							DirectorFullName,DirectorPosition,AccreditationSertificate,LawAddress,FactAddress,
							PhoneCityCode,Phone,Fax,EMail,Site,RegionId, TypeId, KindId,
							MainId, DepartmentId, StatusId, NewOrgId, DisableLog";

		private const string InsertOrgColumnsParameters =
			@"@UpdateDate,@FullName,@ShortName,@INN,@OGRN,@OwnerDepartment,@IsPrivate,@IsFilial,
								@DirectorFullName,@DirectorPosition,@AccreditationSertificate,@LawAddress,@FactAddress,
								@PhoneCityCode,@Phone,@Fax,@EMail,@Site,@RegionId, @TypeId, @KindId,@MainId, @DepartmentId, @StatusId, @NewOrgId, @DisableLog";

		private const string SelectOrgStatement =
            @"SELECT 
						O.DisableLog,O.Id,O.Version,O.FullName,O.ShortName,O.INN,O.OGRN,O.OwnerDepartment,O.IsPrivate,O.IsFilial,
						O.DirectorFullName,O.DirectorPosition,O.AccreditationSertificate,O.LawAddress,O.FactAddress,
						O.PhoneCityCode,O.Phone,O.Fax,O.EMail,O.Site,
						Reg.Id as RegionId, Type.Id as TypeId, Kind.Id as KindId,
						Reg.Name as RegionName, Type.Name as TypeName, Kind.Name as KindName,
						O.RCModel as RCModelId, RC.ModelName as RCModelName, O.RCDescription,
						O.CNFBFullTime, O.CNFBEvening, O.CNFBPostal, O.CNPayFullTime, O.CNPayEvening, O.CNPayPostal,
						O.MainId, MO.FullName as MainFullName, MO.ShortName as MainShortName,
						O.StatusId, Status.Name as StatusName,
						O.NewOrgId, NO.FullName as NewOrgFullName, NO.ShortName as NewOrgShortName,
						O.DepartmentId, DO.FullName as DepartmentFullName, DO.ShortName as DepartmentShortName, O.UpdateDate {0}
					FROM 
						{1} O
					INNER JOIN 
						dbo.Region Reg ON Reg.Id=O.RegionId 
					INNER JOIN 
						dbo.OrganizationType2010 Type ON Type.Id=O.TypeId
					INNER JOIN 
						dbo.OrganizationKind Kind ON Kind.Id=O.KindId 
					INNER JOIN 
						[dbo].[RecruitmentCampaigns] RC on O.RCModel = RC.Id
					INNER JOIN 
						[dbo].[OrganizationOperatingStatus] Status on O.StatusId = Status.Id
					LEFT JOIN
						[dbo].[Organization2010] MO on O.MainId = MO.Id
					LEFT JOIN 
						[dbo].[Organization2010] NO on O.NewOrgId = NO.Id
					LEFT JOIN
						[dbo].[Organization2010] DO on O.DepartmentId = DO.Id
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
			Private, 

			/// <summary>
			/// Государственный
			/// </summary>
			State, 

			/// <summary>
			/// Неизвестен (не удалось определить)
			/// </summary>
			Undefinded
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

        public static Organization Get(int id)
        {
            return Get(Convert.ToInt64(id));
        }
		/// <summary>
		/// The get.
		/// </summary>
		/// <param name="id">
		/// The id.
		/// </param>
		/// <returns>
		/// </returns>
		public static Organization Get(long id)
		{
			Organization result = null;

			using (var conn = new SqlConnection(DBSettings.ConnectionString))
			{
				conn.Open();

				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = GenerateOrgSelectStatement("dbo.Organization2010", null, "WHERE O.Id=@Id");
				cmd.Parameters.AddWithValue("@Id", id);

				using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
				{
					if (reader.Read())
					{
						result = new Organization(reader);
					}

					reader.Close();
				}

				conn.Close();
			}

			return result;
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

			return Get(user.RequestedOrganization.Id);
		}

		/// <summary>
		/// The get by login esrp.
		/// </summary>
		/// <param name="login">
		/// The login.
		/// </param>
		/// <returns>
		/// </returns>
		public static Organization GetByLoginEsrp(string login)
		{
			int organizationId = 0;
			using (var conn = new SqlConnection(DBSettings.ConnectionString))
			{
				conn.Open();

				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = @"SELECT OrganizationId
								   FROM Account A
								   WHERE A.Login = @Login";
				cmd.Parameters.AddWithValue("@Login", login);

				using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
				{
					if (reader.Read())
					{
						organizationId = reader.IsDBNull(0) || reader[0] == null ? 0 : Convert.ToInt32(reader[0]);
					}

					reader.Close();
				}

				conn.Close();
			}

			return organizationId == 0 ? null : Get(organizationId);
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

			using (var conn = new SqlConnection(DBSettings.ConnectionString))
			{
				conn.Open();

				SqlCommand cmd = conn.CreateCommand();

				/*
				cmd.Parameters.AddWithValue("@typeId", GetNewTypeId(typeId));
				cmd.Parameters.AddWithValue("@isPrivate", GetIsPrivate(typeId) == OPF.Private ? true : false);
				 */
				cmd.CommandText =
					@"
						select O.Id, O.TypeId, O.IsPrivate, O.MainId, O.ShortName, O.FullName, O.RegionId from Organization2010 O
						where statusId = 1
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

				conn.Close();
			}

			return result;
		}

	    /// <summary>
	    /// </summary>
	    /// <param name="organization">
	    /// The organization.
	    /// </param>
	    /// <param name="isEsrp">
	    /// The is esrp.
	    /// </param>
	    /// <returns>
	    /// </returns>
	    public static int UpdateOrCreate(Organization organization, bool isEsrp)
        {
            return UpdateOrCreate(organization, isEsrp, "SYSTEM");
        }


		/// <summary>
		/// The update or create.
		/// </summary>
		/// <param name="organization">
		/// The organization.
		/// </param>
		/// <param name="isEsrp">
		/// The is esrp.
		/// </param>
		/// <param name="userName">
		/// The user Name.
		/// </param>
		/// <returns>
		/// The update or create result.
		/// </returns>
		public static int UpdateOrCreate(Organization organization, bool isEsrp, string userName)
		{
			Organization currentVersion = (organization != null && organization.Id != 0) ? Get(organization.Id) : null;

			using (var conn = new SqlConnection(DBSettings.ConnectionString))
			{
				conn.Open();

				SqlCommand cmd = conn.CreateCommand();

				// Новая
				if (currentVersion == null || currentVersion.Id == 0)
				{
					cmd.CommandText =
						string.Format(
							@"
						SET IDENTITY_INSERT Organization2010 ON
						INSERT INTO 
							dbo.Organization2010 
							(
							  Id,{0} {1}
							)
						VALUES 
							(
							  @Id,{2} {3}
							)
						", 
							InsertOrgColumnsNames,
                            !isEsrp ? "," + InsertNotESRPOrgColumnsNames : string.Empty, 
							InsertOrgColumnsParameters,
                            !isEsrp ? "," + InsertNotESRPOrgColumnsParameters : string.Empty);
				}
				else
				{
					cmd.CommandText =
						string.Format(
							@"
						UPDATE 
							dbo.Organization2010 
						SET 
							{0}
							MainId=@MainId, DepartmentId=@DepartmentId,StatusId=@StatusId,NewOrgId=@NewOrgId,Version=@Version
						WHERE 
							Id=@Id", 
							!isEsrp
                                ? @" RCModel=@RCModel, RCDescription=@RCDescription, DisableLog=@DisableLog,
								 CNFBFullTime = @CNFBFullTime, CNFBEvening = @CNFBEvening, CNFBPostal = @CNFBPostal, 
								 CNPayFullTime = @CNPayFullTime, CNPayEvening = @CNPayEvening, CNPayPostal = @CNPayPostal,"
								: @"UpdateDate=@UpdateDate,FullName=@FullName,ShortName=@ShortName,
								INN=@INN,OGRN=@OGRN,OwnerDepartment=@OwnerDepartment,IsPrivate=@IsPrivate,IsFilial=@IsFilial,
								DirectorFullName=@DirectorFullName,DirectorPosition=@DirectorPosition,
								AccreditationSertificate=@AccreditationSertificate,LawAddress=@LawAddress,
								FactAddress=@FactAddress,PhoneCityCode=@PhoneCityCode,Phone=@Phone,Fax=@Fax,EMail=@EMail,Site=@Site,
								RegionId=@RegionId, TypeId=@TypeId, KindId=@KindId,");

					cmd.Parameters.AddWithValue("Version", currentVersion.Version + 1);
				}

				cmd.Parameters.AddWithValue("Id", organization.Id);
				AddOrganizationParamsToQuery(cmd.Parameters, organization, isEsrp);

				cmd.ExecuteNonQuery();
				if (organization.Id == 0)
				{
					cmd.Parameters.Clear();
					cmd.CommandText = "SELECT @@Identity";
					organization.Id = Convert.ToInt32(cmd.ExecuteScalar());
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
					"O.OriginalOrgId, O.UpdateDescription, O.EditorUserName, row_number() over(order by O.version desc) as rn", 
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
                                EditorName = reader["EditorUserName"] is DBNull ? "<i>SYSTEM</i>" : (string)reader["EditorUserName"]
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

		private static void AddOrganizationParamsToQuery(
			SqlParameterCollection parameters, Organization organization, bool isEsrp)
		{
            parameters.AddWithValue("DisableLog", organization.DisableLog);
			parameters.AddWithValue("UpdateDate", DateTime.Now);
			parameters.AddWithValue("FullName", organization.FullName);
			parameters.AddWithValue("ShortName", organization.ShortName);
			if (organization.INN == null)
			{
				parameters.AddWithValue("INN", DBNull.Value);
			}
			else
			{
				parameters.AddWithValue("INN", organization.INN);
			}

			if (organization.OGRN == null)
			{
				parameters.AddWithValue("OGRN", DBNull.Value);
			}
			else
			{
				parameters.AddWithValue("OGRN", organization.OGRN);
			}

			parameters.AddWithValue("OwnerDepartment", organization.OwnerDepartment);
			parameters.AddWithValue("IsPrivate", organization.IsPrivate);
			parameters.AddWithValue("IsFilial", organization.IsFilial);
			parameters.AddWithValue("DirectorFullName", organization.DirectorFullName);
			parameters.AddWithValue("DirectorPosition", organization.DirectorPosition);
			parameters.AddWithValue("AccreditationSertificate", organization.AccreditationSertificate);
			parameters.AddWithValue("LawAddress", organization.LawAddress);
			parameters.AddWithValue("FactAddress", organization.FactAddress);
			parameters.AddWithValue("PhoneCityCode", organization.PhoneCityCode);
			parameters.AddWithValue("Phone", organization.Phone);
			parameters.AddWithValue("Fax", organization.Fax);
			parameters.AddWithValue("EMail", organization.EMail);
			parameters.AddWithValue("Site", organization.Site);
			parameters.AddWithValue("RegionId", organization.Region.Id);
			parameters.AddWithValue("TypeId", organization.OrgType.Id);

			// TODO Вид организации
			if (organization.Kind.Id == null)
			{
				parameters.AddWithValue("KindId", 3);
			}
			else
			{
				parameters.AddWithValue("KindId", organization.Kind.Id);
			}

			if (!isEsrp)
			{
				parameters.AddWithValue("RCModel", organization.RCModelId);
				if (organization.RCDescription == null)
				{
					parameters.AddWithValue("RCDescription", DBNull.Value);
				}
				else
				{
					parameters.AddWithValue("RCDescription", organization.RCDescription);
				}

				parameters.AddWithValue("CNFBFullTime", organization.CNFBFullTime);
				parameters.AddWithValue("CNFBEvening", organization.CNFBEvening);
				parameters.AddWithValue("CNFBPostal", organization.CNFBPostal);
				parameters.AddWithValue("CNPayFullTime", organization.CNPayFullTime);
				parameters.AddWithValue("CNPayEvening", organization.CNPayEvening);
				parameters.AddWithValue("CNPayPostal", organization.CNPayPostal);
			}

			if (organization.MainId.HasValue)
			{
				parameters.AddWithValue("MainId", organization.MainId.Value);
			}
			else
			{
				parameters.AddWithValue("MainId", DBNull.Value);
			}

			if (organization.DepartmentId.HasValue && organization.DepartmentId != 0 && organization.DepartmentId != -1)
			{
				parameters.AddWithValue("DepartmentId", organization.DepartmentId.Value);
			}
			else
			{
				parameters.AddWithValue("DepartmentId", DBNull.Value);
			}

			parameters.AddWithValue("StatusId", organization.Status.Id);
			if (organization.NewOrgId != null)
			{
				parameters.AddWithValue("NewOrgId", organization.NewOrgId.Value);
			}
			else
			{
				parameters.AddWithValue("NewOrgId", DBNull.Value);
			}
		}

		/// <summary>
		/// сравнение двух организаций
		/// </summary>
		/// <param name="source">
		/// исходная организация
		/// </param>
		/// <param name="targer">
		/// новая организация
		/// </param>
		/// <returns>
		/// текстовое описание разницы
		/// </returns>
		private static string CompareOrgs(Organization source, Organization targer)
		{
			var changes = new StringBuilder();

			if (source.FullName != targer.FullName)
			{
				changes.Append("Полное наименование; ");
			}

            if (source.DisableLog != targer.DisableLog)
            {
                changes.Append("Журналирование проверок");
            }

		    if (source.ShortName != targer.ShortName)
			{
				changes.Append("Краткое наименование; ");
			}

			if (source.Region.Id != targer.Region.Id)
			{
				changes.Append("Регион; ");
			}

			if (source.OrgType.Id != targer.OrgType.Id)
			{
				changes.Append("Тип; ");
			}

			if (source.Kind.Id != targer.Kind.Id)
			{
				changes.Append("Вид; ");
			}

			if (source.INN != targer.INN)
			{
				changes.Append("ИНН; ");
			}

			if (source.OGRN != targer.OGRN)
			{
				changes.Append("ОГРН; ");
			}

			if (source.OwnerDepartment != targer.OwnerDepartment)
			{
				changes.Append("Учредитель; ");
			}

			if (source.IsPrivate != targer.IsPrivate)
			{
				changes.Append("Организационно-правовая форма; ");
			}

			if (source.IsFilial != targer.IsFilial)
			{
				changes.Append("Является филиалом; ");
			}

			if (source.DirectorPosition != targer.DirectorPosition)
			{
				changes.Append("Должность руководителя; ");
			}

			if (source.DirectorFullName != targer.DirectorFullName)
			{
				changes.Append("ФИО руководителя; ");
			}

			if (source.AccreditationSertificate != targer.AccreditationSertificate)
			{
				changes.Append("Свидетельство об аккредитации; ");
			}

			if (source.LawAddress != targer.LawAddress)
			{
				changes.Append("Юридический адрес; ");
			}

			if (source.FactAddress != targer.FactAddress)
			{
				changes.Append("Фактический адрес; ");
			}

			if (source.PhoneCityCode != targer.PhoneCityCode)
			{
				changes.Append("Код города; ");
			}

			if (source.Phone != targer.Phone)
			{
				changes.Append("Телефон; ");
			}

			if (source.Fax != targer.Fax)
			{
				changes.Append("Факс; ");
			}

			if (source.EMail != targer.EMail)
			{
				changes.Append("E-mail; ");
			}

			if (source.Site != targer.Site)
			{
				changes.Append("Сайт; ");
			}

			if (source.CNFBFullTime != targer.CNFBFullTime)
			{
				changes.Append("Общее количество мест обучающихся за счет бюджета по очной форме обучения; ");
			}

			if (source.CNFBEvening != targer.CNFBEvening)
			{
				changes.Append("Общее количество мест обучающихся за счет бюджета по очно-заочной форме обучения; ");
			}

			if (source.CNFBPostal != targer.CNFBPostal)
			{
				changes.Append("Общее количество мест обучающихся за счет бюджета по заочной форме обучения; ");
			}

			if (source.CNPayFullTime != targer.CNPayFullTime)
			{
				changes.Append(
					@"Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
							установленное учредителем на очной форме обучения; ");
			}

			if (source.CNPayEvening != targer.CNPayEvening)
			{
				changes.Append(
					@"Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
							установленное учредителем на очно-заочной форме обучения; ");
			}

			if (source.CNPayPostal != targer.CNPayPostal)
			{
				changes.Append(
					@"Общее количество мест обучающихся на основе договоров с оплатой стоимости обучения,
							установленное учредителем на заочной форме обучения; ");
			}

			if (source.RCModelId != targer.RCModelId)
			{
				changes.Append("Модель приемной кампании; ");
			}

			if (source.RCDescription != targer.RCDescription)
			{
				changes.Append("Модель приемной кампании (указанное пользователем); ");
			}

			if (source.MainId != targer.MainId)
			{
				changes.Append("Головная организация; ");
			}

			if (source.Status.Id != targer.Status.Id)
			{
				changes.Append("Статус; ");
			}

			if (source.NewOrgId != targer.NewOrgId)
			{
				changes.Append("Новая организация; ");
			}

			string ch = changes.ToString();
			if (ch == string.Empty)
			{
				return "Изменений не найдено";
			}

			return ch;
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
		private static void WriteOrgUpdateHistory(Organization org, Organization updatedOrg, string userName)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
			{
				conn.Open();
				SqlCommand cmd = conn.CreateCommand();

				cmd.CommandText =
					string.Format(
						@"
						INSERT INTO 
							dbo.[OrganizationUpdateHistory] 
							([OriginalOrgId], [UpdateDescription], [Version], [EditorUserName],{0},{1})
						VALUES 
							(@OriginalOrgId, @UpdateDescription, @Version, @EditorUserName,{2},{3})", 
						InsertOrgColumnsNames, 
						InsertNotESRPOrgColumnsNames, 
						InsertOrgColumnsParameters, 
						InsertNotESRPOrgColumnsParameters);

				cmd.Parameters.AddWithValue("OriginalOrgId", org.Id);
				cmd.Parameters.AddWithValue("Version", org.Version);

				string diff = CompareOrgs(org, updatedOrg);
				cmd.Parameters.AddWithValue("UpdateDescription", diff);

                cmd.Parameters.AddWithValue("EditorUserName", userName == string.Empty ? "SYSTEM" : userName);

				AddOrganizationParamsToQuery(cmd.Parameters, org, false);
				cmd.ExecuteNonQuery();

				conn.Close();
			}
		}

		#endregion
	}
}