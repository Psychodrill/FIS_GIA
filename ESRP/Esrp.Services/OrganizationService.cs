namespace Esrp.Services
{
    using System.Data;
    using System.Data.Common;
    using System.Data.Linq;
    using System.Data.SqlClient;

    using Esrp.Core.DataAccess;

    using Esrp.Web.ViewModel.Organizations;

    /// <summary>
    /// Сервис для работы с организациями
    /// </summary>
    public class OrganizationService
    {
        /// <summary>
        /// Получить письмо о переносе сроков
        /// </summary>
        /// <param name="orgId">Идентификатор организации</param>
        /// <returns>письмо о переносе сроков</returns>
        public Letter GetLetter(int orgId)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"SELECT LetterToReschedule, LetterToRescheduleContentType, LetterToRescheduleName
			          FROM dbo.Organization2010			
			          WHERE Id = @orgId";
                sqlCommand.Parameters.AddWithValue("@orgId", orgId);
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var letter = new Letter
                        {
                            Content = new Binary((byte[])reader["LetterToReschedule"]),
                            ContentType = reader["LetterToRescheduleContentType"].ToString(),
                            Name = reader["LetterToRescheduleName"].ToString()
                        };
                        return letter;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получить письмо о переносе сроков (из таблицы история изменений организации)
        /// </summary>
        /// <param name="orgId">Идентификатор организации</param>
        /// <param name="versionNumber">Номер версии</param>
        /// <returns>письмо о переносе сроков</returns>
        public Letter GetLetter(int orgId, int versionNumber)
        {
            using (var executor = new DbExecutor())
            {
                SqlCommand sqlCommand = executor.CreateCommand();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText =
                    @"SELECT LetterToReschedule, LetterToRescheduleContentType, LetterToRescheduleName
			          FROM dbo.OrganizationUpdateHistory		
			          WHERE OriginalOrgId = @orgId AND Version=@versionNumber";
                sqlCommand.Parameters.AddWithValue("@orgId", orgId);
                sqlCommand.Parameters.AddWithValue("@versionNumber", versionNumber);
                using (DbDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var letter = new Letter
                        {
                            Content = new Binary((byte[])reader["LetterToReschedule"]),
                            ContentType = reader["LetterToRescheduleContentType"].ToString(),
                            Name = reader["LetterToRescheduleName"].ToString()
                        };
                        return letter;
                    }
                }
            }

            return null;
        }
    }
}