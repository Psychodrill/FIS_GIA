using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FbsServices
{
    public class BatchPrintService
    {
        public const int DefaultParticipantsOnPage = 100;

        private readonly long _organizationId;
        private readonly string _connectionString;
        private readonly int _participantsOnPage;

        public event EventHandler<PrintNoteEventArgs> PrintNote; 

        public BatchPrintService(long organizationId)
            : this(organizationId, DefaultParticipantsOnPage)
        {   
        }

        public BatchPrintService(long organizationId, int participantsOnPage)
        {
            _organizationId = organizationId;
            _participantsOnPage = participantsOnPage;
            _connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ConnectionString;
        }

        public void Run()
        {
            if (PrintNote == null)
            {
                return;
            }

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandText = "create table #BPIDS (Id BIGINT, SenderTypeId INT, RowNumber INT IDENTITY(1, 1));";
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }

                        int totalParticipants;

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[GetCheckResultsHistoryOrgPagesCount]";
                            cmd.Parameters.Add(new SqlParameter("@organizationId", SqlDbType.BigInt)).Value = _organizationId;
                            cmd.Parameters.Add(new SqlParameter("@uniqueChecks", SqlDbType.Bit)).Value = true;
                            totalParticipants = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (totalParticipants == 0)
                        {
                            tx.Commit();
                            return;
                        }
                        
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[GetCheckResultsHistoryOrgPaged]";
                            cmd.Parameters.Add(new SqlParameter("@organizationId", SqlDbType.BigInt)).Value = _organizationId;
                            cmd.Parameters.Add(new SqlParameter("@startRowIndex", SqlDbType.Int)).Value = DBNull.Value;
                            cmd.Parameters.Add(new SqlParameter("@participantsOnPageCount", SqlDbType.Int)).Value = _participantsOnPage;

                            cmd.Prepare();

                            DataTable pageView = new DataTable();

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.AcceptChangesDuringFill = false;

                                for (int startRowIndex = 1;
                                     startRowIndex <= totalParticipants;
                                     startRowIndex += _participantsOnPage)
                                {
                                    cmd.Parameters["@startRowIndex"].Value = startRowIndex;
                                    pageView.Clear();
                                    pageView.AcceptChanges();
                                    da.Fill(pageView);

                                    if (pageView.Rows.Count == 0)
                                    {
                                        break;
                                    }

                                    long prevCheckId = 0;
                                    for (int i = 0; i < pageView.Rows.Count; i++)
                                    {
                                        long curCheckId = pageView.Rows[i].Field<long>("Id");
                                        if (prevCheckId != curCheckId)
                                        {
                                            prevCheckId = curCheckId;
                                            DataView data = new DataView(pageView, string.Format("Id = {0}", curCheckId), null, DataViewRowState.CurrentRows);
                                            OnPrintNote(data);
                                        }
                                    }
                                }
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

        public void RunPage(bool uniqueOnly, int skip)
        {
            if (PrintNote == null)
            {
                return;
            }

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                var timeout = 600;

                using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandTimeout = timeout;
                            cmd.CommandText = "create table #BPIDS (Id BIGINT, SenderTypeId INT, RowNumber INT IDENTITY(1, 1));";
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                        }

                        int totalParticipants;

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandTimeout = timeout;
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[GetCheckResultsHistoryOrgPagesCount]";
                            cmd.Parameters.Add(new SqlParameter("@organizationId", SqlDbType.BigInt)).Value = _organizationId;
                            cmd.Parameters.Add(new SqlParameter("@uniqueChecks", SqlDbType.Bit)).Value = uniqueOnly;
                            totalParticipants = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (totalParticipants == 0)
                        {
                            tx.Commit();
                            return;
                        }

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandTimeout = timeout;
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[GetCheckResultsHistoryOrgPaged]";
                            cmd.Parameters.Add(new SqlParameter("@organizationId", SqlDbType.BigInt)).Value = _organizationId;
                            cmd.Parameters.Add(new SqlParameter("@startRowIndex", SqlDbType.Int)).Value = skip;
                            cmd.Parameters.Add(new SqlParameter("@participantsOnPageCount", SqlDbType.Int)).Value = _participantsOnPage;

                            cmd.Prepare();

                            DataTable pageView = new DataTable();

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.AcceptChangesDuringFill = false;

                                pageView.Clear();
                                pageView.AcceptChanges();
                                da.Fill(pageView);

                                if (pageView.Rows.Count == 0)
                                {
                                    return;
                                }

                                long prevCheckId = 0;
                                for (int i = 0; i < pageView.Rows.Count; i++)
                                {
                                    long curCheckId = pageView.Rows[i].Field<long>("Id");
                                    if (prevCheckId != curCheckId)
                                    {
                                        prevCheckId = curCheckId;
                                        DataView data = new DataView(pageView, string.Format("Id = {0}", curCheckId), null, DataViewRowState.CurrentRows);
                                        OnPrintNote(data);
                                    }
                                }
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

        private void OnPrintNote(DataView pageView)
        {
            var handler = PrintNote;
            if (handler != null)
            {
                handler(this, new PrintNoteEventArgs(pageView));
            }
        }
    }

    public class PrintNoteEventArgs : EventArgs
    {
        public PrintNoteEventArgs(DataView data)
        {
            PrintNoteDataSource = data;
        }

        public DataView PrintNoteDataSource { get; private set; }
    }
}