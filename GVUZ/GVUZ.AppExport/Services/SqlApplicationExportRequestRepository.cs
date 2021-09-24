using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GVUZ.AppExport.Services
{
    public class SqlApplicationExportRequestRepository : IApplicationExportRequestRepository
    {
        private readonly string _connectionString;

        public SqlApplicationExportRequestRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection CreateConnection()
        {
            SqlConnection cn = new SqlConnection(_connectionString);
            cn.Open();
            return cn;
        }
        
        public IEnumerable<ApplicationExportRequest> FindByInstitution(long institutionId)
        {
            const string query = @"select RequestId, RequestDate, RequestStatus, InstitutionId, YearStart from [dbo].[ApplicationExportRequest] where InstitutionId = @InstitutionId order by RequestDate desc";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@InstitutionId", SqlDbType.BigInt)).Value = institutionId;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            HashSet<ApplicationExportRequest> results = new HashSet<ApplicationExportRequest>();
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    results.Add(new ApplicationExportRequest
                                        {
                                            RequestId = reader.GetGuid(0),
                                            RequestDate = reader.GetDateTime(1),
                                            RequestStatus = (ApplicationExportRequestStatus)reader.GetInt32(2),
                                            InstitutionId = reader.GetInt64(3),
                                            YearStart = reader.GetInt32(4)
                                        });
                                }
                            }

                            tx.Commit();
                            return results;
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

        public ApplicationExportRequest FindByRequestId(Guid requestId)
        {
            const string query = @"select RequestId, RequestDate, RequestStatus, InstitutionId, YearStart from [dbo].[ApplicationExportRequest] where RequestId = @RequestId";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.UniqueIdentifier)).Value = requestId;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            ApplicationExportRequest result = null;

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows && reader.Read())
                                {
                                    result = new ApplicationExportRequest
                                        {
                                            RequestId = reader.GetGuid(0),
                                            RequestDate = reader.GetDateTime(1),
                                            RequestStatus = (ApplicationExportRequestStatus)reader.GetInt32(2),
                                            InstitutionId = reader.GetInt64(3),
                                            YearStart = reader.GetInt32(4)
                                        };
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

        public void ResetIncomplete()
        {
            const string query = @"update [dbo].[ApplicationExportRequest] set RequestStatus = 0 where RequestStatus IN(0, 1, 2)";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;
                        try
                        {
                            cmd.ExecuteNonQuery();
                            tx.Commit();
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

        public bool HasPending(long institutionId, int yearStart)
        {
            const string query = "select top 1 1 from [dbo].[ApplicationExportRequest] where RequestStatus IN(0, 1, 2) and InstitutionId = @InstitutionId and YearStart = @YearStart";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@InstitutionId", SqlDbType.BigInt)).Value = institutionId;
                    cmd.Parameters.Add(new SqlParameter("@YearStart", SqlDbType.Int)).Value = yearStart;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;

                        try
                        {
                            bool isPending;
                            
                            using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                isPending = reader.HasRows;
                            }

                            tx.Commit();
                            
                            return isPending;
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

        public IEnumerable<Guid> FetchNewId(int maxItems)
        {
            const string query = @"select TOP(@MaxItems) RequestId from [dbo].[ApplicationExportRequest] where RequestStatus = 0 ORDER BY RequestDate asc";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@MaxItems", SqlDbType.Int)).Value = maxItems;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;

                        try
                        {
                            HashSet<Guid> results = new HashSet<Guid>();
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    results.Add(reader.GetGuid(0));
                                }
                            }

                            tx.Commit();
                            return results;
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

        public ApplicationExportRequest AddNew(long institutionId, int yearStart)
        {
            var request = new ApplicationExportRequest
                {
                    InstitutionId = institutionId,
                    RequestDate = DateTime.Now,
                    RequestId = Guid.NewGuid(),
                    RequestStatus = ApplicationExportRequestStatus.New,
                    YearStart = yearStart
                };

            const string query =
                @"insert into [dbo].[ApplicationExportRequest](RequestId, RequestDate, RequestStatus, InstitutionId, YearStart) 
                  values(@RequestId, @RequestDate, @RequestStatus, @InstitutionId, @YearStart)";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.UniqueIdentifier)).Value = request.RequestId;
                    cmd.Parameters.Add(new SqlParameter("@RequestDate", SqlDbType.DateTime)).Value = request.RequestDate;
                    cmd.Parameters.Add(new SqlParameter("@RequestStatus", SqlDbType.Int)).Value = (int)request.RequestStatus;
                    cmd.Parameters.Add(new SqlParameter("@InstitutionId", SqlDbType.BigInt)).Value = request.InstitutionId;
                    cmd.Parameters.Add(new SqlParameter("@YearStart", SqlDbType.Int)).Value = request.YearStart;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;

                        try
                        {
                            cmd.ExecuteNonQuery();
                            tx.Commit();
                            return request;
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

        public void CommitState(IEnumerable<Guid> requestId, ApplicationExportRequestStatus status)
        {
            if (!requestId.Any())
            {
                return;
            }

            const string query = @"update [dbo].[ApplicationExportRequest] set RequestStatus = @status where RequestId = @RequestId";

            using (var cn = CreateConnection())
            {
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.Int)).Value = (int) status;
                    cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.UniqueIdentifier, 40)).Value = DBNull.Value;

                    using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        cmd.Transaction = tx;

                        try
                        {
                            cmd.Prepare();
                            foreach (var guid in requestId)
                            {
                                cmd.Parameters[1].Value = guid;
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
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

        public void CommitState(Guid requestId, ApplicationExportRequestStatus status)
        {
            CommitState(new[] {requestId}, status);
        }
    }
}