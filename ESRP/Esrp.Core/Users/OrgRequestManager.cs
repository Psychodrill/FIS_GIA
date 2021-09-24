// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrgRequestManager.cs" company="">
//   
// </copyright>
// <summary>
//   The org request manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Esrp.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    using Esrp.Core.DataAccess;
    using Esrp.Core.Organizations;
    using Esrp.Core.Systems;

    using FogSoft.Helpers;

    /// <summary>
    /// The org request manager.
    /// </summary>
    public class OrgRequestManager
    {
        /*
		public static DataTable GetRequestsWithPaging(int firstElementIndex, int elementsNumber)
		{
			List<OrgRequest> requestList = new List<OrgRequest>();
			using (DbExecutor dbExecutor = new DbExecutor())
			{
				DataTable table = new DataTable();
				SqlCommand command = dbExecutor.CreateCommand();
				command.CommandType = CommandType.Text;
				command.CommandText = @"
					SELECT or1.Id, or1.StatusID
					FROM OrganizationRequest2010 or1 
					WHERE or1.OrganizationId = @orgID";
				command.Parameters.AddWithValue("@firstElementIndex", firstElementIndex);
				command.Parameters.AddWithValue("@elementsNumber", elementsNumber);
				using(SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
				{
					dataAdapter.Fill(table);
					return table;
				}
			}
		}
*/
        #region Public Methods

        /// <summary>
        /// The get comment.
        /// </summary>
        /// <param name="orgReqID">
        /// The org req id.
        /// </param>
        /// <returns>
        /// </returns>
        public static OperatorComment GetComment(int orgReqID)
        {
            using (var dbExecutor = new DbExecutor())
            {
                SqlCommand cmd = dbExecutor.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"
		SELECT a.[Login], orol.Comments, orol.DTLastChange
		FROM OrganizationRequestOperatorLog orol JOIN Account a ON a.Id = orol.OperatorID
		WHERE orol.OrganizationRequestID = @orgReqID";
                cmd.Parameters.AddWithValue("@orgReqID", orgReqID);
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var comment = new OperatorComment();
                        comment.OperatorLogin = reader["Login"].To(string.Empty);
                        comment.Comment = reader["Comments"].To(string.Empty);
                        comment.LastChangeDate = reader["DTLastChange"].To(DateTime.MinValue);
                        return comment;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получить данные по последней заявке.		
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="organizationID">
        /// The organization ID.
        /// </param>
        /// <returns>
        /// Возвращаемый объект заполнен не полностью. Поля RequestID, Status, IsForActivation и 
        /// массив пользователей LinkedUsers c полями login, SystemKind, UserStatus
        /// </returns>
        public static OrgRequest GetLastRequest(string login, int organizationID)
        {
            using (var dbExecutor = new DbExecutor())
            {
                SqlCommand sqlCommand = dbExecutor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;


                sqlCommand.CommandText =
                    @"SELECT or1.Id, or1.StatusID, or1.IsForActivation, a.Login, a.[Status], g.SystemID, [system].FullName
                      FROM     
                            (-- получаем последнюю заявку для указанного пользователя
                            SELECT TOP(1) oreq1.id, oreq1.StatusID, oreq1.IsForActivation from OrganizationRequest2010 oreq1 
                            JOIN OrganizationRequestAccount oreqa ON oreq1.Id = oreqa.OrgRequestID
                            JOIN Account acc ON acc.id = oreqa.AccountID 
                      WHERE oreq1.OrganizationId = @organizationID And acc.login = @login ORDER BY oreq1.Id desc) or1 
                      JOIN OrganizationRequestAccount ora ON or1.Id = ora.OrgRequestID
                      JOIN Account a ON a.id = ora.AccountID    
                      JOIN [Group] g ON ora.GroupId = g.Id
                      JOIN [System] [system] on [system].SystemID = g.SystemID";
                sqlCommand.Parameters.AddWithValue("@organizationID", organizationID);
                sqlCommand.Parameters.AddWithValue("@login", login);
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    var request = new OrgRequest();
                    var users = new Dictionary<string, OrgUserBrief>();
                    while (reader.Read())
                    {
                        string requestUserLogin = reader["login"].To(string.Empty);
                        request.RequestID = reader["id"].To(0);
                        request.Status = (UserAccount.UserAccountStatusEnum)reader["StatusID"].To(0);
                        request.IsForActivation = reader["IsForActivation"].To(false);

                        // если пользователь уже добавлен то добавляем к нему ещё один вид системы
                        OrgUserBrief user = users.ContainsKey(requestUserLogin)
                                                ? users[requestUserLogin]
                                                : new OrgUserBrief();
                        user.Login = reader["login"].To(string.Empty);

                        user.FullSystemNameList.Add(reader["FullName"].ToString());
                        var systemKind = (SystemKind)reader["SystemID"].To(0);
                        if (systemKind == SystemKind.Fbs)
                        {
                            user.HasAccessToFbs = true;
                        }

                        if (systemKind == SystemKind.Fbd)
                        {
                            user.HasAccessToFbd = true;
                        }

                        user.Status = UserAccount.ConvertStatusCode(reader["Status"].To(string.Empty));
                        if (!users.ContainsKey(requestUserLogin))
                        {
                            users.Add(user.Login, user);
                        }
                    }

                    request.LinkedUsers = new List<OrgUserBrief>(users.Values);
                    return request.RequestID > 0 ? request : null;
                }
            }
        }

        /// <summary>
        /// The get request.
        /// </summary>
        /// <param name="requestID">
        /// The request id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        /// <exception cref="Exception">
        /// </exception>
        public static OrgRequest GetRequest(int requestID)
        {
            using (var dbExecutor = new DbExecutor())
            {
                SqlCommand sqlCommand = dbExecutor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                @"DECLARE @orgID INT
                    SELECT @orgID = or1.OrganizationId FROM OrganizationRequest2010 or1 WHERE id=@requestID

                    -- информация по организации
                    SELECT or1.id, or1.FullName, or1.ShortName, or1.StatusID, or1.IsForActivation, or1.OwnerDepartment, 
                    or1.DirectorFullName, or1.LawAddress, or1.Phone, or1.Fax, ot.[Name] OrgTypeName, r.[Name] RegionName,
                    or1.OrganizationId,
                    ok.Name as OrgKindName,
                    or1.TypeId,
                    or1.ReceptionOnResultsCNE,
                    or1.IsPrivate,
                    or1.IsFilial,
                    or1.INN,
                    or1.OGRN,
                    or1.KPP,
                    or1.DirectorPosition,
                    or1.DirectorFullName,
                    or1.FactAddress,
                    or1.PhoneCityCode,
                    or1.Email,
                    or1.Site,
                    or1.AccreditationSertificate
                    FROM OrganizationRequest2010 or1 JOIN OrganizationType2010 ot ON ot.Id = or1.TypeId
                    LEFT JOIN OrganizationKind ok ON ok.Id=or1.KindId
                    JOIN Region r ON or1.RegionId = r.Id
                    WHERE or1.id = @requestID

                    -- информация по пользователям в заявке
                    SELECT ora.AccountID, a.[Login], a.LastName, a.FirstName, a.PatronymicName, a.Phone, as1.StatusID,
                    CASE WHEN (SELECT COUNT(*) FROM OrganizationRequestAccount ora1 JOIN [Group] g1 ON g1.Id = ora1.GroupID 
			                    JOIN [System] s ON g1.SystemID = s.SystemID
                            WHERE ora1.AccountID = a.Id AND s.Code = 'fbd' ) > 0 THEN 1 ELSE 0 END AccessToFbd,  
                    CASE WHEN (SELECT COUNT(*) FROM OrganizationRequestAccount ora1 JOIN [Group] g1 ON g1.Id = ora1.GroupID 
			                    JOIN [System] s ON g1.SystemID = s.SystemID
                            WHERE ora1.AccountID = a.Id AND s.Code = 'fbs' ) > 0 THEN 1 ELSE 0 END AccessToFbs,
                    CASE WHEN a.RegistrationDocument IS NOT null THEN 1 ELSE 0 END HasRegDocument,
                    a.AdminComment
                    FROM (select DISTINCT AccountID, OrgRequestID from OrganizationRequestAccount) ora JOIN Account a ON ora.AccountID = a.Id
                    JOIN AccountStatus as1 ON  a.[Status] = as1.Code
                    WHERE ora.OrgRequestID = @requestID

                    -- информация по пользователям в заявке по их организации
                    SELECT a.ID, a.[Login], a.LastName, a.FirstName, a.PatronymicName, a.Phone, as1.StatusID,
                    CASE WHEN (SELECT COUNT(*) FROM OrganizationRequestAccount ora1 JOIN [Group] g1 ON g1.Id = ora1.GroupID 
			                    JOIN [System] s ON g1.SystemID = s.SystemID
                            WHERE ora1.AccountID = a.Id AND s.Code = 'fbd' ) > 0 THEN 1 ELSE 0 END AccessToFbd,  
                    CASE WHEN (SELECT COUNT(*) FROM OrganizationRequestAccount ora1 JOIN [Group] g1 ON g1.Id = ora1.GroupID 
			                    JOIN [System] s ON g1.SystemID = s.SystemID
                            WHERE ora1.AccountID = a.Id AND s.Code = 'fbs' ) > 0 THEN 1 ELSE 0 END AccessToFbs,
                    CASE WHEN a.RegistrationDocument IS NOT null THEN 1 ELSE 0 END HasRegDocument,
                    a.AdminComment
                    FROM Account a
                    JOIN AccountStatus as1 ON  a.[Status] = as1.Code
                    WHERE a.OrganizationId  = @requestID


                    -- ответственные за работу пользователи организации. 
                    SELECT distinct a.login, a.LastName, a.FirstName, a.PatronymicName, a.phone, a.email, 
                    case when g.SystemID = 2 THEN 1 else 0 end AccessToFbs ,
                    case when g.SystemID = 3 THEN 1 else 0 end AccessToFbd
                    FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
                    JOIN Account a ON a.id = ora.AccountID AND ora.OrgRequestID = a.OrganizationId
                    JOIN GroupAccount ga ON ga.GroupId = ora.GroupID AND ga.AccountId = a.Id
                    JOIN [Group] g ON g.Id = ga.GroupId
                    JOIN AccountStatus as1 on a.[Status] = as1.Code
                    WHERE or1.OrganizationId = @orgID AND g.SystemID IN (2,3) AND a.[Status] = 'activated'";
                sqlCommand.Parameters.AddWithValue("@requestID", requestID);
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    var request = new OrgRequest();
                    while (reader.Read())
                    {
                        request.Organization = new OrgBrief();
                        request.RequestID = reader["id"].To(0);
                        request.Status = (UserAccount.UserAccountStatusEnum)reader["StatusID"].To(0);
                        request.IsForActivation = reader["IsForActivation"].To(false);

                        request.Organization.OrgFullName = reader["FullName"].To(string.Empty);
                        request.Organization.OrgShortName = reader["ShortName"].To(string.Empty);
                        request.Organization.OrgTypeName = reader["OrgTypeName"].To(string.Empty);
                        request.Organization.OrgKindName = reader["OrgKindName"].To(string.Empty);
                        request.Organization.RegionName = reader["RegionName"].To(string.Empty);
                        request.Organization.FounderName = reader["OwnerDepartment"].To(string.Empty);
                        request.Organization.DirectorFullName = reader["DirectorFullName"].To(string.Empty);
                        request.Organization.DirectorPosition = reader["DirectorPosition"].To(string.Empty);
                        request.Organization.LawAddress = reader["LawAddress"].To(string.Empty);
                        request.Organization.ReceptionOnResultsCNE = reader[OrganizationDataAccessor.TableColumns.ReceptionOnResultsCNE] == DBNull.Value
                                                                        ? (int?)null
                                                                        : Convert.ToInt32(reader[OrganizationDataAccessor.TableColumns.ReceptionOnResultsCNE]);
                        request.Organization.OrgTypeId = reader["TypeId"] == DBNull.Value
                                                                        ? (int?)null
                                                                        : Convert.ToInt32(reader["TypeId"]);
                        request.Organization.FactAddress = reader["FactAddress"].To(string.Empty);
                        request.Organization.Phone = reader["Phone"].To(string.Empty);
                        request.Organization.Fax = reader["Fax"].To(string.Empty);
                        request.Organization.OrganizationId = reader["OrganizationId"].To(0);
                        request.Organization.IsPrivate = reader["IsPrivate"].To(false);
                        request.Organization.IsFilial = reader["IsFilial"].To(false);
                        request.Organization.INN = reader["INN"].To(string.Empty);
                        request.Organization.OGRN = reader["OGRN"].To(string.Empty);
                        request.Organization.KPP = reader["KPP"].To(string.Empty);
                        request.Organization.PhoneCityCode = reader["PhoneCityCode"].To(string.Empty);
                        request.Organization.EMail = reader["Email"].To(string.Empty);
                        request.Organization.Site = reader["Site"].To(string.Empty);
                        request.Organization.AccreditationSertificate = reader["AccreditationSertificate"].To(string.Empty);
                        break;
                    }

                    if (!reader.NextResult())
                    {
                        throw new Exception("Отсутствует запрос для пользователей по заявке.");
                    }

                    while (reader.Read())
                    {
                        var requestUser = new OrgUserBrief();
                        requestUser.Login = reader["login"].To(string.Empty);
                        requestUser.FirstName = reader["FirstName"].To(string.Empty);
                        requestUser.MiddleName = reader["PatronymicName"].To(string.Empty);
                        requestUser.LastName = reader["LastName"].To(string.Empty);
                        requestUser.Phone = reader["Phone"].To(string.Empty);
                        requestUser.HasAccessToFbs = reader["AccessToFbs"].To(false);
                        requestUser.HasAccessToFbd = reader["AccessToFbd"].To(false);
                        requestUser.Status = (UserAccount.UserAccountStatusEnum)reader["StatusID"].To(0);
                        requestUser.HasRegDocument = reader["HasRegDocument"].To(false);
                        requestUser.AdminComment = reader["AdminComment"].To(string.Empty);
                        request.LinkedUsers.Add(requestUser);
                    }

                    if (!reader.NextResult())
                    {
                        throw new Exception("Отсутствует запрос для пользователей по заявке 2.");
                    }

                    while (reader.Read())
                    {
                        var requestUser = new OrgUserBrief();
                        requestUser.Login = reader["login"].To(string.Empty);
                        requestUser.FirstName = reader["FirstName"].To(string.Empty);
                        requestUser.MiddleName = reader["PatronymicName"].To(string.Empty);
                        requestUser.LastName = reader["LastName"].To(string.Empty);
                        requestUser.Phone = reader["Phone"].To(string.Empty);
                        requestUser.HasAccessToFbs = reader["AccessToFbs"].To(false);
                        requestUser.HasAccessToFbd = reader["AccessToFbd"].To(false);
                        requestUser.Status = (UserAccount.UserAccountStatusEnum)reader["StatusID"].To(0);
                        requestUser.HasRegDocument = reader["HasRegDocument"].To(false);
                        requestUser.AdminComment = reader["AdminComment"].To(string.Empty);
                        request.LinkedUsersOrg.Add(requestUser);
                    }

                    if (!reader.NextResult())
                    {
                        throw new Exception("Отсутствует запрос для ответственных за работу пользователей организации.");
                    }

                    while (reader.Read())
                    {
                        var requestUser = new OrgUserBrief();
                        requestUser.Login = reader["login"].To(string.Empty);
                        requestUser.FirstName = reader["FirstName"].To(string.Empty);
                        requestUser.MiddleName = reader["PatronymicName"].To(string.Empty);
                        requestUser.LastName = reader["LastName"].To(string.Empty);
                        requestUser.Phone = reader["Phone"].To(string.Empty);
                        requestUser.Status = UserAccount.UserAccountStatusEnum.Activated;
                        requestUser.HasAccessToFbs = reader["AccessToFbs"].To(false);
                        requestUser.HasAccessToFbd = reader["AccessToFbd"].To(false);

                        request.Organization.ActivatedUsers.Add(requestUser);
                    }

                    return request;
                }
            }
        }

        /// <summary>
        /// The get requests.
        /// </summary>
        /// <param name="orgID">
        /// The org id.
        /// </param>
        /// <returns>
        /// </returns>
        public static OrgRequest[] GetRequests(int orgID)
        {
            var requestList = new List<OrgRequest>();
            using (var dbExecutor = new DbExecutor())
            {
                SqlCommand command = dbExecutor.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText =
                    @"
					SELECT or1.Id, or1.StatusID
					FROM OrganizationRequest2010 or1 
					WHERE or1.OrganizationId = @orgID";
                command.Parameters.AddWithValue("@orgID", orgID);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        requestList.Add(
                            new OrgRequest
                                {
                                    RequestID = dataReader["Id"].To(0), 
                                    Status = (UserAccount.UserAccountStatusEnum)dataReader["StatusID"].To(0)
                                });
                    }

                    return requestList.ToArray();
                }
            }
        }

        /// <summary>
        /// The save comment.
        /// </summary>
        /// <param name="orgReqID">
        /// The org req id.
        /// </param>
        /// <param name="operatorLogin">
        /// The operator login.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        public static void SaveComment(int orgReqID, string operatorLogin, string comment)
        {
            using (var dbExecutor = new DbExecutor())
            {
                SqlCommand cmd = dbExecutor.CreateCommand();
                cmd.CommandText =
                    @"
DECLARE @curDT DATETIME, @operatorID BIGINT
SELECT @operatorID = a.Id FROM Account a WHERE a.[Login] = @operatorLogin
SET @curDT = GETDATE()

if not exists(SELECT * FROM OrganizationRequestOperatorLog orol WHERE orol.OrganizationRequestID = @orgReqID)
BEGIN
	INSERT INTO OrganizationRequestOperatorLog (OrganizationRequestID, OperatorID, Comments, DTCreate, DTLastChange)
	VALUES (@orgReqID, @operatorID, @comment, @curDT, @curDT)
END
ELSE
BEGIN
	UPDATE OrganizationRequestOperatorLog SET Comments = @comment, OperatorID = @operatorID, DTLastChange = @curDT WHERE OrganizationRequestID = @orgReqID
END";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@orgReqID", orgReqID);
                cmd.Parameters.AddWithValue("@operatorLogin", operatorLogin);
                cmd.Parameters.AddWithValue("@comment", comment);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<OrgUser> RegisteredRequest(List<OrgUser> users, int? orgRequestID, bool isOlympicStaff)
        {
            return OrgUserDataAccessor.UpdateUserAccount(users, orgRequestID,isOlympicStaff);
        }

        /// <summary>
        /// The update organization request status.
        /// </summary>
        /// <param name="orgReqID">
        /// The org req id.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The update organization request status.
        /// </returns>
        public static string UpdateOrganizationRequestStatus(int orgReqID, UserAccount.UserAccountStatusEnum status)
        {
            return UpdateOrganizationRequestStatus(orgReqID, status, null);
        }

        /// <summary>
        /// Если метод выполнился успешно, то метод возвращает null, иначе строка с информацией об ошибке.
        /// </summary>
        /// <param name="orgReqID">
        /// The org Req ID.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// The update organization request status.
        /// </returns>
        public static string UpdateOrganizationRequestStatus(
            int orgReqID, UserAccount.UserAccountStatusEnum status, string comment)
        {
            try
            {
                using (var dbExecutor = new DbExecutor())
                {
                    SqlCommand cmd = dbExecutor.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = @"UpdateOrganizationRequestStatus";
                    cmd.Parameters.AddWithValue("@orgRequestID", orgReqID);
                    cmd.Parameters.AddWithValue("@statusID", (int)status);
                    cmd.Parameters.AddWithValue("@needConsiderLinkedUsers", 1);
                    cmd.Parameters.AddWithValue(
                        "@comment", string.IsNullOrEmpty(comment) ? DBNull.Value : (object)comment);
                    cmd.Parameters.AddWithValue("@editorLogin", Account.ClientLogin);
                    cmd.Parameters.AddWithValue("@editorIp", Account.ClientIp);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exc)
            {
                if (exc.Message.StartsWith("$"))
                {
                    return exc.Message.Substring(1);
                }

                throw;
            }

            return null;
        }

        #endregion
    }
}