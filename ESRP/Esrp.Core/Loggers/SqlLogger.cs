using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Esrp.Core.DataAccess;

namespace Esrp.Core.Loggers
{
    public class AccountEventLogger
    {
        /// <summary>
        /// Логирует информацию о просмотре профиля для аакаунта
        /// </summary>
        /// <param name="accountId">Идентификатор аккаунта</param>
        public static void LogAccountViewEvent(Int64 accountId)
        {
            Int64 _viewerId = 0;
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"select account.[Id]
	                                        from  dbo.Account account with (nolock, fastfirstrow)
	                                        where account.[Login] = @editorLogin";
                sqlCommand.Parameters.Add(new SqlParameter("@editorLogin",Account.ClientLogin));

                _viewerId = (Int64)sqlCommand.ExecuteScalar();
                sqlCommand.CommandText = @"insert into dbo.AccountLog
			                               (
			                                AccountId
			                                , VersionId
			                                , UpdateDate
			                                , UpdateId
			                                , EditorAccountId
			                                , EditorIp
			                                , [Login]
			                                , PasswordHash
			                                , OrganizationId
			                                , IsOrganizationOwner
			                                , ConfirmYear
			                                , LastName
			                                , FirstName
			                                , PatronymicName
			                                , Phone
			                                , Email
			                                , AdminComment
			                                , IsActive
			                                , Status
			                                , IpAddresses
                                            , HasFixedIp
			                                , IsActiveChange
			                                , IsStatusChange
			                                , IsEdit
			                                , IsPasswordChange
			                                , IsVpnEditorIp
			                                )
                                            select Id,
                                            (select isnull(max(account_log.VersionId), 0)+1
				                                from dbo.AccountLog account_log
				                                where account_log.AccountId =@accountId),
                                            getdate(),
                                            newid(),
                                            @editorId,
                                            @editorIp,
                                            Login
                                            , PasswordHash
			                                , OrganizationId
			                                , IsOrganizationOwner
			                                , ConfirmYear
			                                , LastName
			                                , FirstName
			                                , PatronymicName
			                                , Phone
			                                , Email
			                                , AdminComment
			                                , IsActive
			                                , Status
			                                , IpAddresses, HasFixedIp ,b,c,d,e,f  from account a
                                            cross apply (select 0 as b,0 as c,0 as d,0 as e, case
				                                                                        when exists(select 1
						                                                                        from dbo.VpnIp vpn_ip with (nolock)
						                                                                        where vpn_ip.Ip = @editorIp
							                                                                        and vpn_ip.IsActive = 1) then 1
				                                                                        else 0
			                                                                        end as f) as g
                                            where a.Id=@accountId";
                sqlCommand.Parameters.Add(new SqlParameter("@accountId", accountId));
                sqlCommand.Parameters.Add(new SqlParameter("@editorId", _viewerId));
                sqlCommand.Parameters.Add(new SqlParameter("@editorIp", Account.ClientIp));
                sqlCommand.ExecuteNonQuery();
            }
            
            
        }
        public static void LogAccountViewEvent(string accountLogin)
        {
           
            Int64 accountId = 0;
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"select account.[Id]
	                                        from  dbo.Account account with (nolock, fastfirstrow)
	                                        where account.[Login] = @editorLogin";
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add(new SqlParameter("@editorLogin", accountLogin));
                accountId = (Int64)sqlCommand.ExecuteScalar();
            }
            LogAccountViewEvent(accountId);
        }
    }
}
