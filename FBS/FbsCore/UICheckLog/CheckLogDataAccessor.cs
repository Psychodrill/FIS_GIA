namespace Fbs.Core.UICheckLog
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// The check log data accessor.
    /// </summary>
    public class CheckLogDataAccessor
    {
        #region Public Methods

        /// <summary>
        /// The add cne number check event.
        /// </summary>
        /// <param name="accountLogin">
        /// The account login.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <param name="cneNumber">
        /// The cne number.
        /// </param>
        /// <returns>
        /// The add cne number check event.
        /// </returns>
        public static int AddCNENumberCheckEvent(
            string accountLogin, string lastName, string firstName, string patronymicName, string cneNumber)
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCNEWebUICheckEvent";

                cmd.Parameters.AddWithValue("@AccountLogin", accountLogin);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@PatronymicName", patronymicName);
                cmd.Parameters.AddWithValue("@CNENumber", cneNumber);
                var eventId = new SqlParameter("@EventId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(eventId);

                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@EventId"].Value);
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// The add marks check event.
        /// </summary>
        /// <param name="accountLogin">
        /// The account login.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <param name="marks">
        /// The marks.
        /// </param>
        /// <returns>
        /// The add marks check event.
        /// </returns>
        public static int AddMarksCheckEvent(
            string accountLogin, string lastName, string firstName, string patronymicName, string marks)
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCNEWebUICheckEvent";

                cmd.Parameters.AddWithValue("@AccountLogin", accountLogin);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@PatronymicName", patronymicName);
                cmd.Parameters.AddWithValue("@RawMarks", marks);

                var eventId = new SqlParameter("@EventId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(eventId);

                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@EventId"].Value);

                conn.Close();
            }

            return result;
        }

        public static int AddMarksCheckEvent(string accountLogin, string cneNumber, string marks)
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCNEWebUICheckEvent";

                cmd.Parameters.AddWithValue("@AccountLogin", accountLogin);
                cmd.Parameters.AddWithValue("@CNENumber", cneNumber);
                cmd.Parameters.AddWithValue("@RawMarks", marks);

                var eventId = new SqlParameter("@EventId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(eventId);

                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@EventId"].Value);

                conn.Close();
            }

            return result;
        }

        public static int AddPassportCheckEvent(
           string accountLogin,           
           string passportSeria,
           string passportNumber,
           string marks)
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCNEWebUICheckEvent";

                cmd.Parameters.AddWithValue("@AccountLogin", accountLogin);                
                cmd.Parameters.AddWithValue("@PassportSeria", passportSeria);
                cmd.Parameters.AddWithValue("@PassportNumber", passportNumber);
                cmd.Parameters.AddWithValue("@RawMarks", marks);

                var eventId = new SqlParameter("@EventId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(eventId);

                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@EventId"].Value);

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// The add passport check event.
        /// </summary>
        /// <param name="accountLogin">
        /// The account login.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <param name="passportSeria">
        /// The passport seria.
        /// </param>
        /// <param name="passportNumber">
        /// The passport number.
        /// </param>
        /// <returns>
        /// The add passport check event.
        /// </returns>
        public static int AddPassportCheckEvent(
            string accountLogin, 
            string lastName, 
            string firstName, 
            string patronymicName, 
            string passportSeria, 
            string passportNumber)
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCNEWebUICheckEvent";

                cmd.Parameters.AddWithValue("@AccountLogin", accountLogin);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@PatronymicName", patronymicName);
                cmd.Parameters.AddWithValue("@PassportSeria", passportSeria);
                cmd.Parameters.AddWithValue("@PassportNumber", passportNumber);
                var eventId = new SqlParameter("@EventId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(eventId);

                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@EventId"].Value);
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// The add tn check event.
        /// </summary>
        /// <param name="accountLogin">
        /// The account login.
        /// </param>
        /// <param name="lastName">
        /// The last name.
        /// </param>
        /// <param name="firstName">
        /// The first name.
        /// </param>
        /// <param name="patronymicName">
        /// The patronymic name.
        /// </param>
        /// <param name="typographicNumber">
        /// The typographic number.
        /// </param>
        /// <returns>
        /// The add tn check event.
        /// </returns>
        public static int AddTNCheckEvent(
            string accountLogin, string lastName, string firstName, string patronymicName, string typographicNumber)
        {
            int result;
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "AddCNEWebUICheckEvent";

                cmd.Parameters.AddWithValue("@AccountLogin", accountLogin);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@PatronymicName", patronymicName);
                cmd.Parameters.AddWithValue("@TypographicNumber", typographicNumber);

                var eventId = new SqlParameter("@EventId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(eventId);

                cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@EventId"].Value);

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// The update check event.
        /// </summary>
        /// <param name="eventId">
        /// The event id.
        /// </param>
        /// <param name="foundedCNEId">
        /// The founded cne id.
        /// </param>
        public static void UpdateCheckEvent(string eventId, string foundedCNEId)
        {
            if (string.IsNullOrEmpty(foundedCNEId))
            {
                return;
            }

            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText =
                    @"UPDATE CNEWebUICheckLog SET FoundedCNEId=@FoundedCNEId
                                WHERE Id=@Id";
                cmd.Parameters.AddWithValue("FoundedCNEId", foundedCNEId);
                cmd.Parameters.AddWithValue("Id", eventId);

                cmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static CheckLogEntry Get(int id)
        {
            using (var conn = new SqlConnection(DBSettings.ConnectionString))
            {
                 conn.Open();
                var cmd = conn.CreateCommand();

                cmd.CommandText = "Select * from CNEWebUICheckLog inner join Account on Account.Id=AccountId where CNEWebUICheckLog.Id=@id ";
                cmd.Parameters.AddWithValue("id", id);
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return null;
                return new CheckLogEntry
                {
                    Id=Int32.Parse(reader["Id"].ToString()),
                    CNENumber=reader["CNENumber"]==DBNull.Value?null:reader["CNENumber"].ToString(),
                    EventDate=DateTime.Parse(reader["EventDate"].ToString()),
                    FirstName = reader["FirstName"] == DBNull.Value ? null : reader["FirstName"].ToString(),
                    LastName = reader["LastName"] == DBNull.Value ? null : reader["LastName"].ToString(),
                    PatronymicName = reader["PatronymicName"] == DBNull.Value ? null : reader["PatronymicName"].ToString(),
                    Marks = reader["Marks"] == DBNull.Value ? null : reader["Marks"].ToString(),
                    PassportNumber = reader["PassportNumber"] == DBNull.Value ? null : reader["PassportNumber"].ToString(),
                    PassportSeria = reader["PassportSeria"] == DBNull.Value ? null : reader["PassportSeria"].ToString(),
                    TypographicNumber = reader["TypographicNumber"] == DBNull.Value ? null : reader["TypographicNumber"].ToString(),
                    Login=reader["Login"].ToString()                    
                };

            }
        }
        #endregion
    }
}