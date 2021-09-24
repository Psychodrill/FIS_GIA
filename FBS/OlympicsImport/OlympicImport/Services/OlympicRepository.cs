using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OlympicImport.Services
{
    public class OlympicRepository
    {
        private HashSet<OlympicsCsvRecord> _storedRecords;
        private readonly IConnectionStringProvider _connectionStringProvider;
        
        public OlympicRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public OlympicsCsvRecord GetOrAdd(string olympicCodeName, Func<string, OlympicsCsvRecord> createFactory)
        {
            OlympicsCsvRecord record = GetStoredRecords().SingleOrDefault(x => x.CodeName.Equals(olympicCodeName, StringComparison.OrdinalIgnoreCase));
            if (record == null)
            {
                record = createFactory.Invoke(olympicCodeName);
                PersistRecord(record);
                GetStoredRecords().Add(record);
            }

            return record;
        }

        private HashSet<OlympicsCsvRecord> GetStoredRecords()
        {
            return _storedRecords ?? (_storedRecords = new HashSet<OlympicsCsvRecord>(LoadExistingRecords()));
        }

        private IEnumerable<OlympicsCsvRecord> LoadExistingRecords()
        {
            using (SqlConnection cn = new SqlConnection(_connectionStringProvider.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"select id, code_name, olympiad_name, olympiad_number, olympiad_level, olympiad_subject_name, olympiad_subject_profile_name, olympiad_year from [dbo].[Olympics]";

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                HashSet<OlympicsCsvRecord> result = new HashSet<OlympicsCsvRecord>();
                                while (reader.Read())
                                {
                                    result.Add(new OlympicsCsvRecord
                                        {
                                            Id = reader.GetInt64(0),
                                            CodeName = reader.IsDBNull(1) ? null : reader.GetString(1),
                                            OlympiadName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                            OlympiadNumber = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                                            OlympiadLevel = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                            OlympiadSubjectName = reader.IsDBNull(5) ? null : reader.GetString(5),
                                            OlympiadSubjectProfileName = reader.IsDBNull(6) ? null : reader.GetString(6),
                                            OlympiadYear = reader.IsDBNull(7) ? 2015 : reader.GetInt32(7)
                                        });
                                }

                                return result;
                            }
                        }
                    }
                    finally
                    {
                        tx.Commit();    
                    }
                }
            }
        }

        private void PersistRecord(OlympicsCsvRecord record)
        {
            using (SqlConnection cn = new SqlConnection(_connectionStringProvider.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText =
                                @"insert into [dbo].[Olympics](code_name, olympiad_name, olympiad_number, olympiad_level, olympiad_subject_name, olympiad_subject_profile_name, olympiad_year)
                                        values(@codeName, @olympiadName, @olympiadNumber, @olympiadLevel, @olympiadSubjectName, @olympiadSubjectProfileName, @olympiadYear)";

                            cmd.Parameters.Add(new SqlParameter("@codeName", SqlDbType.VarChar)).Value = record.CodeName;
                            cmd.Parameters.Add(new SqlParameter("@olympiadName", SqlDbType.VarChar)).Value = record.OlympiadName;
                            cmd.Parameters.Add(new SqlParameter("@olympiadNumber", SqlDbType.Int)).Value = record.OlympiadNumber;
                            cmd.Parameters.Add(new SqlParameter("@olympiadLevel", SqlDbType.Int)).Value = record.OlympiadLevel.HasValue ? record.OlympiadLevel.Value : (object)DBNull.Value;
                            cmd.Parameters.Add(new SqlParameter("@olympiadSubjectName", SqlDbType.VarChar)).Value = record.OlympiadSubjectName;
                            cmd.Parameters.Add(new SqlParameter("@olympiadSubjectProfileName", SqlDbType.VarChar)).Value = record.OlympiadSubjectProfileName;
                            cmd.Parameters.Add(new SqlParameter("@olympiadYear", SqlDbType.Int)).Value = record.OlympiadYear;

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select @@IDENTITY";

                            record.Id = Convert.ToInt64(cmd.ExecuteScalar());
                        }

                        // subjects
                        using (SqlCommand cmdTheme = cn.CreateCommand(), cmdProfile = cn.CreateCommand())
                        {
                            cmdTheme.Transaction = tx;
                            cmdProfile.Transaction = tx;
                            cmdTheme.CommandType = CommandType.Text;
                            cmdProfile.CommandType = CommandType.Text;
                            cmdTheme.CommandText = @"insert into [dbo].[OlympicThemeSubjects](olympics_id, olympic_subject_id) values(@olympicsId, @subjectId)";
                            cmdProfile.CommandText = @"insert into [dbo].[OlympicProfileSubjects](olympics_id, olympic_subject_id) values(@olympicsId, @subjectId)";
                            cmdTheme.Parameters.Add(new SqlParameter("@olympicsId", SqlDbType.BigInt)).Value = record.Id;
                            cmdTheme.Parameters.Add(new SqlParameter("@subjectId", SqlDbType.BigInt)).Value = DBNull.Value;
                            cmdProfile.Parameters.Add(new SqlParameter("@olympicsId", SqlDbType.BigInt)).Value = record.Id;
                            cmdProfile.Parameters.Add(new SqlParameter("@subjectId", SqlDbType.BigInt)).Value = DBNull.Value;

                            cmdTheme.Prepare();
                            cmdProfile.Prepare();

                            foreach (OlympicSubjectCsvRecord themeSubject in record.OlympicThemeSubjects)
                            {
                                cmdTheme.Parameters["@subjectId"].Value = themeSubject.Id;
                                cmdTheme.ExecuteNonQuery();
                            }

                            foreach (OlympicSubjectCsvRecord profileSubject in record.OlympicProfileSubjects)
                            {
                                cmdProfile.Parameters["@subjectId"].Value = profileSubject.Id;
                                cmdProfile.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                    
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                    
                }
                
            }
        }
    }
}