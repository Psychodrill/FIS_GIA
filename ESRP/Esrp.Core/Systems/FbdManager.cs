using System.Data.Common;
using System.Data.SqlClient;
using Esrp.Core.DataAccess;
using FogSoft.Helpers;

namespace Esrp.Core.Systems
{
	public static class FbdManager
	{
		public const string GroupPrefix = "fbd_^";
		public const string AdministratorGroupCode = GroupPrefix + "administrator";
		public const string AuthorizedStaffGroupCode = GroupPrefix + "authorizedstaff";
		public const string UserGroupCode = GroupPrefix + "user";
        public const string OlympicStaffGroupCode = GroupPrefix + "authorizedstaffolympic";

		public static UserAccount GetLastCreatedUserByGroup(int organizationID, string code)
		{
			using (DbExecutor dbExecutor = new DbExecutor())
			{
				SqlCommand cmd = dbExecutor.CreateCommand();
				cmd.CommandText = @"
			SELECT TOP(1) a.LastName, a.Login, a.Position, a.Status
			FROM OrganizationRequestAccount ora JOIN OrganizationRequest2010 or1 ON ora.OrgRequestID = or1.Id
			 JOIN Account a ON a.id = ora.AccountID
			 JOIN GroupAccount ga ON ga.AccountId = a.Id
			 JOIN [Group] g ON ga.GroupId = g.Id
			WHERE g.Code = '{0}' and or1.OrganizationId = @organizationID
			ORDER BY ora.OrgRequestID DESC, a.id desc"
					.FormatWith(code);
				cmd.Parameters.AddWithValue("organizationID", organizationID);
				using (DbDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						return new UserAccount()
										{
											Login = reader["Login"].ToString(),
											Email = reader["Login"].ToString(),
											LastName = reader["LastName"].ToString(),
											Position = reader["Position"].ToString(),
											Status = UserAccount.ConvertStatusCode(reader["Status"].ToString()),
										};
					}
				}
			}

			return null;
		}
	}
}
