namespace Esrp.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;

    using Esrp.Core.DataAccess;
    using Esrp.Core.Systems;

    using FogSoft.Helpers;

    using DbException = Esrp.Core.Common.DbException;

    /// <summary>
    /// Класс доступа к пользователям
    /// </summary>
    public static class OrgUserDataAccessor
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// </returns>
        public static OrgUser Get(string userLogin)
        {
            OrgUser result = null;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.GetUserAccount";

                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 255));
                cmd.Parameters["@login"].Value = userLogin;

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        result = new OrgUser(reader);
                    }

                    reader.Close();
                }

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// The get activated authorized staff for org.
        /// </summary>
        /// <param name="orgID">
        /// The org id.
        /// </param>
        /// <param name="systemKind">
        /// The system kind.
        /// </param>
        /// <returns>
        /// </returns>
        public static OrgUser GetActivatedAuthorizedStaffForOrg(int orgID, SystemKind systemKind)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"
			SELECT a.id, a.Login, a.Position, a.LastName, a.FirstName, a.PatronymicName, a.phone, a.Email
			FROM GroupAccount ga JOIN [Group] g ON ga.GroupId = g.Id
			 JOIN Account a ON a.Id = ga.AccountId
			 JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.Id  
			WHERE g.SystemID = @systemKind AND or1.OrganizationId = @orgID AND 
				a.[Status] = 'activated' AND g.Code LIKE '%authorizedstaff%'
					";
                sqlCommand.Parameters.AddWithValue("@systemKind", (int)systemKind);
                sqlCommand.Parameters.AddWithValue("@orgID", orgID);
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new OrgUser
                            {
                                login = reader["login"].To(string.Empty),
                                lastName = reader["LastName"].To(string.Empty),
                                firstName = reader["FirstName"].To(string.Empty),
                                patronymicName = reader["PatronymicName"].To(string.Empty),
                                position = reader["Position"].To(string.Empty),
                                email = reader["Email"].To(string.Empty),
                                phone = reader["Phone"].To(string.Empty),
                                status = UserAccount.UserAccountStatusEnum.Activated,
                            };
                        return user;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Получение пользователя по заявленной им организации
        /// </summary>
        /// <param name="OrgId">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable GetByOrgAsTable(int OrgId)
        {
            var result = new DataTable();
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText =
                    @"select distinct(ora.OrgRequestId), A.[Login], 
							A.LastName+' '+A.FirstName+' '+A.PatronymicName FIO, A.[Email], A.[Status]
							from Account A
							inner join OrganizationRequest2010 org on org.Id = a.OrganizationId
							inner join OrganizationRequestAccount ora on ora.OrgRequestID = org.Id
							where org.OrganizationId=@OrganizationId";

                cmd.Parameters.AddWithValue("OrganizationId", OrgId);
                conn.Open();
                cmd.Connection = conn;
                result.Load(cmd.ExecuteReader());
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Обновление пользователей, всех в одну заявку
        /// </summary>
        /// <param name="users">Список пользователей, которые должны обновиться</param>
        /// <param name="orgRequestID">Ид заявки</param>
        /// <returns>Список пользователей с обновленными выходными параметрами</returns>
        public static List<OrgUser> UpdateUserAccount(List<OrgUser> users, int? orgRequestID, bool isOlympicStaff)
        {
            var result = new List<OrgUser>();

            using (var connection = new SqlConnection(DBSettings.ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    foreach (var user in users)
                    {
                        var accountId = 0;

                        // Этап 1: Вызов процедуры GetNewRequest
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.GetNewRequest";
                        command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 255));
                        command.Parameters["@login"].Value = user.login;
                        command.Parameters.Add(new SqlParameter("@registrationDocument", SqlDbType.Image));
                        command.Parameters["@registrationDocument"].Value = user.registrationDocument;
                        command.Parameters.Add(new SqlParameter("@organizationRegionId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@organizationFullName", SqlDbType.NVarChar, 2000));
                        command.Parameters.Add(new SqlParameter("@organizationShortName", SqlDbType.NVarChar, 2000));
                        command.Parameters.Add(new SqlParameter("@organizationINN", SqlDbType.NVarChar, 10));
                        command.Parameters.Add(new SqlParameter("@organizationOGRN", SqlDbType.NVarChar, 13));
                        command.Parameters.Add(new SqlParameter("@organizationFounderName", SqlDbType.NVarChar, 2000));
                        command.Parameters.Add(new SqlParameter("@organizationFactAddress", SqlDbType.NVarChar, 500));
                        command.Parameters.Add(new SqlParameter("@organizationLawAddress", SqlDbType.NVarChar, 500));
                        command.Parameters.Add(new SqlParameter("@organizationTownName", SqlDbType.NVarChar, 250));
                        command.Parameters.Add(new SqlParameter("@organizationDirName", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationDirPosition", SqlDbType.NVarChar, 500));
                        command.Parameters.Add(new SqlParameter("@organizationPhoneCode", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationFax", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationIsAccred", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@organizationIsPrivate", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@organizationIsFilial", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@organizationAccredSert", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationEMail", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationSite", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationPhone", SqlDbType.NVarChar, 255));
                        command.Parameters.Add(new SqlParameter("@organizationTypeId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@organizationKindId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@organizationRcModelId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@orgRCDescription", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@ExistingOrgId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@ReceptionOnResultsCNE", SqlDbType.Bit));
                        command.Parameters.Add(new SqlParameter("@orgRequestID", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar, 255));

                        command.Parameters["@status"].Value = user.statusCode;
                        command.Parameters["@organizationTypeId"].Value = user.RequestedOrganization.OrgType.Id;
                        command.Parameters["@organizationKindId"].Value = user.RequestedOrganization.Kind.Id;
                        command.Parameters["@organizationRegionId"].Value = user.RequestedOrganization.Region.Id;
                        command.Parameters["@organizationFullName"].Value = user.RequestedOrganization.FullName;
                        command.Parameters["@organizationShortName"].Value = user.RequestedOrganization.ShortName;
                        command.Parameters["@organizationINN"].Value = user.RequestedOrganization.INN;
                        command.Parameters["@organizationOGRN"].Value = user.RequestedOrganization.OGRN;
                        command.Parameters["@organizationFounderName"].Value =
                            user.RequestedOrganization.OwnerDepartment;
                        command.Parameters["@organizationFactAddress"].Value = user.RequestedOrganization.FactAddress;
                        command.Parameters["@organizationLawAddress"].Value = user.RequestedOrganization.LawAddress;
                        command.Parameters["@organizationTownName"].Value = user.RequestedOrganization.TownName;
                        command.Parameters["@organizationDirName"].Value = user.RequestedOrganization.DirectorFullName;
                        command.Parameters["@organizationDirPosition"].Value =
                            user.RequestedOrganization.DirectorPosition;
                        command.Parameters["@organizationPhoneCode"].Value = user.RequestedOrganization.PhoneCityCode;
                        command.Parameters["@organizationFax"].Value = user.RequestedOrganization.Fax;
                        command.Parameters["@organizationIsAccred"].Value =
                            !string.IsNullOrEmpty(user.RequestedOrganization.AccreditationSertificate);
                        command.Parameters["@organizationIsPrivate"].Value = user.RequestedOrganization.IsPrivate;
                        command.Parameters["@organizationIsFilial"].Value = user.RequestedOrganization.IsFilial;
                        command.Parameters["@organizationAccredSert"].Value =
                            user.RequestedOrganization.AccreditationSertificate;
                        command.Parameters["@organizationEMail"].Value = user.RequestedOrganization.EMail;
                        command.Parameters["@organizationSite"].Value = user.RequestedOrganization.Site;
                        command.Parameters["@organizationPhone"].Value = user.RequestedOrganization.Phone;
                        if (user.RequestedOrganization.RCModelId != 0)
                        {
                            command.Parameters["@organizationRcModelId"].Value = user.RequestedOrganization.RCModelId;
                        }
                        else
                        {
                            command.Parameters["@organizationRcModelId"].Value = DBNull.Value;
                        }

                        command.Parameters["@orgRCDescription"].Value = user.RequestedOrganization.RCDescription;
                        command.Parameters["@ExistingOrgId"].Value = user.RequestedOrganization.OrganizationId;
                        command.Parameters["@orgRequestID"].Value = orgRequestID.HasValue
                                                                        ? (object)orgRequestID.Value
                                                                        : DBNull.Value;

                        command.Parameters["@ReceptionOnResultsCNE"].Value =
                            user.RequestedOrganization.ReceptionOnResultsCNE;
                        command.Parameters.Add(new SqlParameter("@organizationKPP", SqlDbType.NVarChar, 9));
                        command.Parameters["@organizationKPP"].Value = user.RequestedOrganization.KPP;

                        using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                var orgRequestCurrId = int.Parse(reader["orgRequestID"].ToString());
                                if (orgRequestID == null)
                                {
                                    if (orgRequestCurrId <= 0)
                                    {
                                        throw new Exception("Ошибка при создании заявки");
                                    }
                                    
                                    orgRequestID = orgRequestCurrId;
                                }
                                else
                                {
                                    if (orgRequestCurrId != orgRequestID)
                                    {
                                        throw new Exception("Ошибка при создании заявки");
                                    }
                                }

                                user.status = UserAccount.ConvertStatusCode(reader["Status"].ToString());
                            }

                            reader.Close();
                        }

                        command.Parameters.Clear();

                        // Этап 2: Вызов процедуры GetAccountAndLogin
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.GetAccountAndLogin";
                        command.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 255));
                        command.Parameters["@login"].Value = user.login;
                        command.Parameters.Add(new SqlParameter("@passwordHash", SqlDbType.NVarChar, 255));
                        command.Parameters["@passwordHash"].Value = user.passwordHash;
                        command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar, 255));
                        command.Parameters["@lastName"].Value = user.lastName;
                        command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar, 255));
                        command.Parameters["@firstName"].Value = user.firstName;
                        command.Parameters.Add(new SqlParameter("@patronymicName", SqlDbType.NVarChar, 255));
                        command.Parameters["@patronymicName"].Value = user.patronymicName;
                        command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 255));
                        command.Parameters["@phone"].Value = user.phone;
                        command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255));
                        command.Parameters["@email"].Value = user.email;
                        command.Parameters.Add(new SqlParameter("@position", SqlDbType.NVarChar, 255));
                        command.Parameters["@position"].Value = user.position;
                        command.Parameters.Add(new SqlParameter("@ipAddresses", SqlDbType.NVarChar, 4000));
                        command.Parameters["@ipAddresses"].Value = user.ipAddresses;
                        command.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar, 255));
                        command.Parameters["@status"].Value = user.statusCode;
                        command.Parameters.Add(new SqlParameter("@registrationDocument", SqlDbType.Image));
                        command.Parameters["@registrationDocument"].Value = user.registrationDocument;
                        command.Parameters.Add(new SqlParameter("@registrationDocumentContentType", SqlDbType.NVarChar, 255));
                        command.Parameters["@registrationDocumentContentType"].Value = user.registrationDocumentContentType;
                        command.Parameters.Add(new SqlParameter("@editorLogin", SqlDbType.NVarChar, 255)); 
                        command.Parameters["@editorLogin"].Value = user.editorLogin;
                        command.Parameters.Add(new SqlParameter("@editorIp", SqlDbType.NVarChar, 255)); 
                        command.Parameters["@editorIp"].Value = user.editorIp;
                        command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 255));
                        command.Parameters["@password"].Value = user.password;
                        command.Parameters.Add(new SqlParameter("@hasFixedIp", SqlDbType.Bit));
                        command.Parameters["@hasFixedIp"].Value = user.hasFixedIp;
                        command.Parameters.Add(new SqlParameter("@orgRequestID", SqlDbType.Int));
                        command.Parameters["@orgRequestID"].Value = orgRequestID.HasValue ? (object)orgRequestID.Value : DBNull.Value;

                        using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                user.login = reader["login"].ToString();
                                accountId = Convert.ToInt32(reader["accountId"]);
                            }

                            reader.Close();
                        }

                        command.Parameters.Clear();

                        // Этап 3: Вызов процедуры AddNewGroupRole
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dbo.AddNewGroupRole";
                        command.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 255));
                        command.Parameters["@password"].Value = user.password;
                        command.Parameters.Add(new SqlParameter("@organizationTypeId", SqlDbType.Int));
                        command.Parameters["@organizationTypeId"].Value = user.RequestedOrganization.OrgType.Id;
                        command.Parameters.Add(new SqlParameter("@orgRequestID", SqlDbType.Int));
                        command.Parameters["@orgRequestID"].Value = orgRequestID.HasValue ? (object)orgRequestID.Value : DBNull.Value;
                        command.Parameters.Add(new SqlParameter("@accountId", SqlDbType.Int));
                        command.Parameters["@accountId"].Value = accountId;
                        command.Parameters.Add(new SqlParameter("@ListSystemId", SqlDbType.NVarChar));
                        var stringSystemsId = string.Join(", ", user.SystemsId.Cast<object>().Select(c => c.ToString()).ToArray());
                        char[] charsToTrim = { ',' };
                        stringSystemsId = stringSystemsId.Trim(charsToTrim);
                        command.Parameters["@ListSystemId"].Value = stringSystemsId;
                        command.Parameters.Add(new SqlParameter("@isOlympic", SqlDbType.Bit));
                        command.Parameters["@isOlympic"].Value = isOlympicStaff;

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                        result.Add(user);
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Ошибка во время RollbackTransaction()");
                    }

                    throw;
                }

                return result;
            }
        }

        #endregion
    }
}