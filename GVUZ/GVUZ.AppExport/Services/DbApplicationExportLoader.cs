using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using GVUZ.Model.Entrants;

namespace GVUZ.AppExport.Services
{
    public class DbApplicationExportLoader : IApplicationExportLoader
    {
        private readonly string _connectionString;
        private readonly long _institutionId;
        private readonly long _yearStart;
        private readonly long _pageSize;
        private readonly CancellationToken _token;
        public const int DefaultPageSize = 100;

        public event EventHandler<ApplicationExportEventArgs> ApplicationFetched;

        public DbApplicationExportLoader(string connectionString, CancellationToken token, long pageSize, long institutionId, long yearStart)
        {
            _connectionString = connectionString;
            _institutionId = institutionId;
            _yearStart = yearStart;
            _pageSize = pageSize <= 0 ? DefaultPageSize : pageSize;
            _token = token;
        }

        public void Load()
        {
            if (ApplicationFetched == null)
            {
                return;
            }

            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();

                using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (var cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"CREATE TABLE #TMP_EXPORT_APP_ID(id bigint, rn bigint)";
                            cmd.ExecuteNonQuery();
                        }

                        long totalRecords;

                        using (var cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "[dbo].[ApplicationExportRecordCount]";
                            cmd.Parameters.Add(new SqlParameter("@institutionId", SqlDbType.BigInt)).Value = _institutionId;
                            cmd.Parameters.Add(new SqlParameter("@yearStart", SqlDbType.Int)).Value = _yearStart;
                            SqlParameter recordCount = new SqlParameter("@totalRecords", SqlDbType.BigInt);
                            recordCount.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(recordCount);

                            cmd.ExecuteNonQuery();

                            totalRecords = Convert.ToInt64(recordCount.Value);
                        }

                        if (totalRecords > 0)
                        {
                            using (var cmd = cn.CreateCommand())
                            {
                                cmd.Transaction = tx;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "[dbo].[ApplicationExportRecordPage]";
                                cmd.Parameters.Add(new SqlParameter("@institutionId", SqlDbType.BigInt, sizeof (long))).Value = _institutionId;
                                cmd.Parameters.Add(new SqlParameter("@yearStart", SqlDbType.Int, sizeof (int))).Value = _yearStart;
                                cmd.Parameters.Add(new SqlParameter("@pageSize", SqlDbType.BigInt, sizeof (long))).Value = _pageSize;
                                var skip = new SqlParameter("@skip", SqlDbType.BigInt, sizeof (long));
                                skip.Value = DBNull.Value;
                                cmd.Parameters.Add(skip);

                                cmd.Prepare();

                                using (var ds = new DataSetApplicationExport())
                                {
                                    ds.EnforceConstraints = false;

                                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                    {
                                        da.AcceptChangesDuringFill = false;
                                        da.TableMappings.Clear();
                                        da.TableMappings.Add("Table", ds.Applications.TableName);
                                        da.TableMappings.Add("Table1", ds.FinSourceAndEduForms.TableName);
                                        da.TableMappings.Add("Table2", ds.EntranceTestResults.TableName);

                                        for (long currentIndex = 0;
                                             currentIndex <= totalRecords;
                                             currentIndex += _pageSize)
                                        {
                                            if (_token.IsCancellationRequested)
                                            {
                                                break;
                                            }

                                            skip.Value = currentIndex;

                                            da.Fill(ds);
                                            
                                            if (_token.IsCancellationRequested)
                                            {
                                                break;
                                            }

                                            ProcessPageResults(ds);
                                            
                                            ds.Clear();
                                        }
                                    }
                                }
                            }
                        }

                        tx.Commit();
                    }
                    catch (SqlException)
                    {
                        tx.Rollback();
                        throw;
                    }
                }
                
            }
        }

        private void ProcessPageResults(DataSetApplicationExport pageData)
        {
            foreach (var application in pageData.Applications)
            {
                if (_token.IsCancellationRequested)
                {
                    return;
                }

                var dto = MapApplication(application);

                OnApplicationFetched(dto);
            }
        }

        public int GetInstitutionEsrpId(long orgId)
        {
            int id = 0;
            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();

                using (var tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (var cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select top 1 EsrpOrgID from [dbo].[Institution] where InstitutionID = @id";
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = Convert.ToInt32(orgId);

                            object res = cmd.ExecuteScalar();

                            if (res != null && res != DBNull.Value)
                            {
                                id = Convert.ToInt32(res);
                            }
                        }

                        tx.Commit();
                    }
                    catch (SqlException)
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }

            return id;
        }

        private ApplicationExportDto MapApplication(DataSetApplicationExport.ApplicationsRow app)
        {
            var dto = new ApplicationExportDto
                {
                    AppId = app.AppId,
                    LastDenyDate = app.IsLastDenyDateNull() ? (DateTime?) null : app.LastDenyDate,
                    RegistrationDate = app.RegistrationDate,
                    StatusId = app.StatusId,
                    FinSourceAndEduForms = app.GetFinSourceAndEduForms()
                };

            return dto;
        }

        private void OnApplicationFetched(ApplicationExportDto data)
        {
            var handler = ApplicationFetched;
            if (handler != null && !_token.IsCancellationRequested)
            {
                handler(this, new ApplicationExportEventArgs(data));
            }
        }
    }
}