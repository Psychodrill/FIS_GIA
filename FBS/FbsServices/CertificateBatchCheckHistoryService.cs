using System;
using System.Data;
using System.Data.SqlClient;

namespace FbsServices
{
    public class CertificateBatchCheckHistoryService : CertificateCheckHistoryServiceBase
    {
        public CertificateBatchCheckHistoryService(string login, long checkId) 
            : base(login, checkId)
        {
        }
        
        public long BatchCheckId
        {
            get { return CheckId; }
        }

        public bool CheckBatchReady(long batchId, int checkType)
        {
            using (var cmd = new Command("[dbo].[GetBatchCheckReady]"))
            {
                cmd.Parameters.Add(new SqlParameter("@batchId", SqlDbType.BigInt)).Value = batchId;
                cmd.Parameters.Add(new SqlParameter("@checkType", SqlDbType.Int)).Value = checkType;

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value)
                {
                    CheckId = Convert.ToInt64(res);
                }
            }

            return CheckId > 0;
        }

        public int GetRecordsCount()
        {
            using (var cmd = new Command("[dbo].[GetCheckResultsHistoryPagesCount]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = Login ?? (object) DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = CheckId;

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value)
                {
                    return Convert.ToInt32(res);
                }

                return 0;
            }
        }

        public DataTable GetRecordsPage(int skip, int take)
        {
            using (var cmd = new Command("[dbo].[GetCheckResultsHistoryPaged]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = Login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = CheckId;
                cmd.Parameters.Add(new SqlParameter("@startRowIndex", SqlDbType.Int)).Value = skip;
                cmd.Parameters.Add(new SqlParameter("@participantsOnPageCount", SqlDbType.Int)).Value = take;

                using (var da = cmd.CreateAdapter())
                {
                    DataTable page = new DataTable();
                    da.Fill(page);
                    return page;
                }
            }
        }

        public DataTable GetRecordsPageObsolete(int skip, int take)
        {
            using (var cmd = new Command("[dbo].[GetCheckResultsHistoryPaged_Obsolete]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = Login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = CheckId;
                cmd.Parameters.Add(new SqlParameter("@startRowIndex", SqlDbType.Int)).Value = skip;
                cmd.Parameters.Add(new SqlParameter("@participantsOnPageCount", SqlDbType.Int)).Value = take;

                using (var da = cmd.CreateAdapter())
                {
                    DataTable page = new DataTable();
                    da.Fill(page);
                    return page;
                }
            }
        }
    }
}