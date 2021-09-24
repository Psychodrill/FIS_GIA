using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OlympicImport.Services
{
    public class OlympicSubjectRepository
    {
        private HashSet<OlympicSubjectCsvRecord> _storedRecords;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly OlympicFbsSubjectMapper _fbsSubjectMapper;

        public OlympicSubjectRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _fbsSubjectMapper = new OlympicFbsSubjectMapper(_connectionStringProvider);
        }

        public OlympicSubjectCsvRecord GetOrCreate(string subjectName)
        {
            OlympicSubjectCsvRecord record = GetStoredRecords().SingleOrDefault(x => x.SubjectName.Equals(subjectName, StringComparison.OrdinalIgnoreCase));

            if (record != null)
            {
                return record;
            }

            record = CreatePersistentRecord(subjectName);
            
            GetStoredRecords().Add(record);

            return record;
        }

        private OlympicSubjectCsvRecord CreatePersistentRecord(string subjectName)
        {
            var record = new OlympicSubjectCsvRecord();
            record.SubjectName = subjectName;
            record.FbsSubjectId = _fbsSubjectMapper.MapToFbs(record.SubjectName);

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
                            cmd.CommandText = "insert into [dbo].[OlympicSubjects](subject_name) values(@subjectName)";
                            cmd.Parameters.Add(new SqlParameter("@subjectName", SqlDbType.VarChar)).Value = subjectName;
                            cmd.ExecuteNonQuery();   
                        }

                        using (SqlCommand id = cn.CreateCommand())
                        {
                            id.Transaction = tx;
                            id.CommandType = CommandType.Text;
                            id.CommandText = "select @@IDENTITY";

                            record.Id = Convert.ToInt64(id.ExecuteScalar());
                        }

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"insert into [dbo].[OlympicToFbsSubjectMap](olympic_subject_id, ege_subject_id) values(@olympicSubjectId, @fbsSubjectId)";
                            cmd.Parameters.Add(new SqlParameter("@olympicSubjectId", SqlDbType.BigInt)).Value = record.Id;
                            cmd.Parameters.Add(new SqlParameter("@fbsSubjectId", SqlDbType.BigInt)).Value = DBNull.Value;
                            cmd.Prepare();

                            foreach (long fbsSubjectId in record.FbsSubjectId)
                            {
                                cmd.Parameters["@fbsSubjectId"].Value = fbsSubjectId;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();

                        return record;
                    }
                    catch (Exception)
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        private HashSet<OlympicSubjectCsvRecord> GetStoredRecords()
        {
            return _storedRecords ?? (_storedRecords = new HashSet<OlympicSubjectCsvRecord>(LoadPersistedRecords()));
        }

        private IEnumerable<OlympicSubjectCsvRecord> LoadPersistedRecords()
        {
            using (SqlConnection cn = new SqlConnection(_connectionStringProvider.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        List<OlympicSubjectCsvRecord> records = new List<OlympicSubjectCsvRecord>();

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select ol.id, ol.subject_name from [dbo].[OlympicSubjects] ol";
                            cmd.Transaction = tx;

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    records.Add(new OlympicSubjectCsvRecord
                                    {
                                        Id = reader.GetInt64(0),
                                        SubjectName = reader.GetString(1)
                                    });
                                }
                            }
                        }

                        if (records.Count == 0)
                        {
                            tx.Commit();
                            return records;
                        }

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select distinct olmap.ege_subject_id from [dbo].[OlympicToFbsSubjectMap] olmap where olmap.ege_subject_id is not null and olmap.olympic_subject_id = @subjectId";
                            cmd.Parameters.Add(new SqlParameter("@subjectId", SqlDbType.BigInt)).Value = DBNull.Value;
                            cmd.Transaction = tx;
                            cmd.Prepare();

                            for (int i = 0; i < records.Count; i++)
                            {
                                cmd.Parameters[0].Value = records[i].Id;
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        records[i].FbsSubjectId.Add(reader.GetInt64(0));
                                    }
                                }
                            }
                        }

                        tx.Commit();

                        return records;
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