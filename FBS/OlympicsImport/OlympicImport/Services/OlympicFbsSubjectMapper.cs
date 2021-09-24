using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OlympicImport.Services
{
    public class OlympicFbsSubjectMapper
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private HashSet<FbsSubject> _fbsSubjects;

        public OlympicFbsSubjectMapper(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IList<long> MapToFbs(string olympicSubjectNames)
        {
            var fbs = GetFbsSubjects();
            List<long> result = new List<long>(fbs.Count);
            string olympicSubjectName = olympicSubjectNames.Replace("языки", string.Empty).Replace("язык", string.Empty).Replace(":", string.Empty).Trim().ToLower();

            foreach (FbsSubject fbsSubject in fbs)
            {
                string fbsSubjectName = fbsSubject.SubjectName.Replace("язык", string.Empty).Trim().ToLower();

                if (fbsSubjectName.Contains(olympicSubjectName))
                {
                    result.Add(fbsSubject.Id);
                }
                else if (olympicSubjectName.Contains(fbsSubjectName))
                {
                    result.Add(fbsSubject.Id);
                }
            }

            if (result.Count == 0 && olympicSubjectName.Contains("иностранны"))
            {
                result.AddRange(FbsForeignSubjects());   
            }

            return result;
        }

        private IEnumerable<long> FbsForeignSubjects()
        {
            yield return 9; // английский
            yield return 10; // немецкий
            yield return 11; // французский
            yield return 13; // испанский
        }

        private HashSet<FbsSubject> GetFbsSubjects()
        {
            return _fbsSubjects ?? (_fbsSubjects = LoadFbsSubjects());
        }

        private HashSet<FbsSubject> LoadFbsSubjects()
        {
            using (SqlConnection cn = new SqlConnection(_connectionStringProvider.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        HashSet<FbsSubject> result = new HashSet<FbsSubject>();

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select SubjectCode, SubjectName from [dat].[Subjects]";

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    result.Add(new FbsSubject
                                        {
                                            Id = reader.GetInt32(0),
                                            SubjectName = reader.GetString(1)
                                        });
                                }
                            }
                        }

                        tx.Commit();

                        return result;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}