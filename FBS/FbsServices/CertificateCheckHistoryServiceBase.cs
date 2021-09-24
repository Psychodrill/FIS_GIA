using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FbsServices
{
    public class OrganizationCheckHistoryService
    {
        private readonly long _orgId;
        private int? _pageCount;
        private readonly bool _uniqueOnly;

        private DataTable _currentPage;

        public OrganizationCheckHistoryService(long orgId, bool uniqueOnly)
        {
            _orgId = orgId;
            this._uniqueOnly = uniqueOnly;
        }

        public int GetPageCount()
        {
            if (!_pageCount.HasValue)
            {
                _pageCount = FetchRecordCount();
            }

            return _pageCount.Value;
        }

        public DataTable GetPage(int skip, int take)
        {
            return _currentPage ?? (_currentPage = FetchRecordsPage(skip, take));
        }

        private DataTable FetchRecordsPage(int skip, int take)
        {
            using (var cmd = new Command("[dbo].[GetCheckHistoryOrgPaged]"))
            {
                cmd.Parameters.Add(new SqlParameter("organizationId", SqlDbType.BigInt)).Value = _orgId;
                cmd.Parameters.Add(new SqlParameter("uniqueOnly", SqlDbType.Bit)).Value = _uniqueOnly;
                cmd.Parameters.Add(new SqlParameter("startRowIndex", SqlDbType.Int)).Value = skip;
                cmd.Parameters.Add(new SqlParameter("rowsOnPageCount", SqlDbType.Int)).Value = take;

                using (var da = cmd.CreateAdapter())
                {
                    DataTable result = new DataTable();
                    da.Fill(result);
                    return result;
                }
            }
        }

        private int FetchRecordCount()
        {
            using (var cmd = new Command("[dbo].[GetCheckHistoryOrgPagesCount]"))
            {
                cmd.Parameters.Add(new SqlParameter("organizationId", SqlDbType.BigInt)).Value = _orgId;
                cmd.Parameters.Add(new SqlParameter("uniqueOnly", SqlDbType.Bit)).Value = _uniqueOnly;

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value)
                {
                    return Convert.ToInt32(res);
                }
            }

            return 0;
        }

    }

    public class CertificateCheckHistoryServiceBase
    {
        protected readonly string Login;
        protected long CheckId;
        private Dictionary<int, string> _subjects;

        public CertificateCheckHistoryServiceBase(string login, long checkId)
        {
            Login = login;
            CheckId = checkId;
        }

        public DataTable GetHistoryResultsByGroup(long groupId)
        {
            using (var cmd = new Command("[dbo].[GetCheckResultsGroupMarks]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = Login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = CheckId;
                cmd.Parameters.Add(new SqlParameter("@groupId", SqlDbType.Int)).Value = groupId;

                using (var da = cmd.CreateAdapter())
                {
                    DataTable result = new DataTable();
                    da.Fill(result);
                    return result;
                }
            }
        }

        public string GetSubjectName(int subjectCode)
        {
            string name;
            if (GetSubjects().TryGetValue(subjectCode, out name))
            {
                return name;
            }

            return null;
        }

        protected Dictionary<int, string> GetSubjects()
        {
            return _subjects ?? (_subjects = LoadSubjectsFromDatabase());
        }

        private Dictionary<int, string> LoadSubjectsFromDatabase()
        {
            const string query = @"select [SubjectCode], [SubjectName] from [dat].[Subjects]";
            using (var cmd = new Command(query, CommandType.Text))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    Dictionary<int, string> result = new Dictionary<int, string>();

                    while (reader.Read())
                    {
                        result.Add(reader.GetInt32(0), reader.GetString(1));
                    }

                    return result;
                }
            }
        }
    }
}