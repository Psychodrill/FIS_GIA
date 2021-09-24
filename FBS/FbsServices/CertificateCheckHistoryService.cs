using System;
using System.Data;
using System.Data.SqlClient;

namespace FbsServices
{
    public class CertificateCheckHistoryService : CertificateCheckHistoryServiceBase
    {
        private DataTable _masterData;
        private int? _masterRowCount;
        private DataTable _detailsView;
        private DataTable _notFoundData;

        //private readonly Dictionary<long, DataView> _details = new Dictionary<long, DataView>();

        public CertificateCheckHistoryService(string login, long checkId)
            :base(login, checkId)
        {
        }
        
        public int GetMasterRowCount()
        {
            if (!_masterRowCount.HasValue)
            {
                _masterRowCount = FetchRecordsCount();
            }

            return _masterRowCount.GetValueOrDefault();
        }

        public DataTable GetNotFoundData()
        {
            return _notFoundData;
        }

        public DataTable GetMasterPage(int skip, int take)
        {
            return _masterData ?? (_masterData = FetchRecordsPage(skip, take));
        }

        public DataTable GetDetails(long groupId)
        {
            if (_detailsView == null)
            {
                DataTable view = GetHistoryResultsByGroup(groupId);
                
                if (view.Rows.Count == 1 && view.Rows[0].IsNull("ParticipantId"))
                {
                    _detailsView = view.Clone();
                    _notFoundData = view;
                }
                else
                {
                    _detailsView = view;
                }
            }

            return _detailsView;
        }

        private int FetchRecordsCount()
        {
            using (var cmd = new Command("[dbo].[GetCheckResultsGroupsPagesCount]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = Login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = CheckId;

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value)
                {
                    return Convert.ToInt32(res);
                }

                return 0;
            }
        }

        private DataTable FetchRecordsPage(int skip, int take)
        {
            using (var cmd = new Command("[dbo].[GetCheckResultsGroupsPaged]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.VarChar)).Value = Login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = CheckId;
                cmd.Parameters.Add(new SqlParameter("@startRowIndex", SqlDbType.Int)).Value = skip;
                cmd.Parameters.Add(new SqlParameter("@countOnPage", SqlDbType.Int)).Value = take;

                using (var da = cmd.CreateAdapter())
                {
                    DataTable page = new DataTable();
                    da.Fill(page);
                    return page;
                }
            }
        }

        public static void DeleteBatchCheckHistoryById(string login, long checkId)
        {
            using (var cmd = new Command("[dbo].[DeleteBatchCheckHistoryById]"))
            {
                cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar)).Value = login ?? (object)DBNull.Value;
                cmd.Parameters.Add(new SqlParameter("@checkId", SqlDbType.BigInt)).Value = checkId;
                cmd.ExecuteNonQuery();
            }
        }
    }
}