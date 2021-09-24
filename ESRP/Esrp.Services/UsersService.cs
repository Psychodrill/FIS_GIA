namespace Esrp.Services
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using Esrp.Core;
    using Esrp.Core.Systems;
    using Esrp.Core.Users;

    using Esrp.Web.ViewModel.Users;

    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UsersService
    {
        /// <summary>
        /// Список пользователей с отобжанение фио, состояния регистрации, логином и ИС
        /// </summary>
        /// <param name="requestId">Идентификатор заявки</param>
        /// <returns>Представление типа UserViewForRequest связанных с заявкой</returns>
        public List<UserViewForRequest> SelectUsersByRequest(int requestId)
        {
            var result = new List<UserViewForRequest>();
            var orgRequest = OrgRequestManager.GetRequest(requestId);
            foreach (var user in orgRequest.LinkedUsers)
            {
                var userView = new UserViewForRequest
                    {
                        Login = user.Login,
                        FIO = user.GetFullName(),
                        Status = user.Status,
                        RegDocument =
                            user.HasRegDocument
                                ? "<a target=_blank href=\"/Profile/ConfirmedDocumentView.aspx?login=" + user.Login
                                  + "\" title=\"Просмотр скана заявки на регистрацию\">просмотр</a>"
                                : "не загружен",
                        SystemNames = string.Empty
                    };
                var systemNames = GeneralSystemManager.AccessedSystems(user.Login);
                userView.SystemNames = string.Join(", ", systemNames.Cast<object>().Select(c => c.ToString()).ToArray());
                char[] charsToTrim = { ',' };
                userView.SystemNames = userView.SystemNames.Trim(charsToTrim);
                result.Add(userView);
            }

            return result;
        }

        /// <summary>
        /// Удалить группы у пользователя
        /// </summary>
        /// <param name="login">логин пользователя</param>
        public void DeleteGroupByLogin(string login)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @" DECLARE @accountId int
                                        SELECT @accountId=Id 
                                        From [Account] WHERE [login]=@loginName
                                    
                                    DELETE FROM GroupAccount where [AccountId]=@accountId";
                cmd.Parameters.AddWithValue("@loginName", login);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Добавить группу пользователю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="groupId">Идентификатор группы</param>
        public void SetGroupByLogin(string login, int groupId)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @" DECLARE @accountId int
                                        SELECT @accountId=Id 
                                        From [Account] WHERE [login]=@loginName
                                    
                                    INSERT INTO [GroupAccount] ([GroupId],[AccountId])
                                    VALUES (@groupId, @accountId)";
                cmd.Parameters.AddWithValue("@loginName", login);
                cmd.Parameters.AddWithValue("@groupId", groupId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
