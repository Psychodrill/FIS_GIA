using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace GVUZ.Util.Services.Import
{
    public class ApplicationOrderRecordReader
    {
        private readonly string _connectionString;
        private readonly CancellationToken _cancellationToken;
        private int _recordCount;

        public event EventHandler<ApplicationOrderRecordReaderEventArgs> RecordFetched;

        public ApplicationOrderRecordReader(string connectionString, CancellationToken cancellationToken)
        {
            _connectionString = connectionString;
            _cancellationToken = cancellationToken;
        }

        public void Run()
        {
            ResetIncompleteApplications();
            ReadRecords();
        }

        private void ReadRecords()
        {
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
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = Properties.Resources.Import_SelectCount;

                            _recordCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        if (_recordCount == 0)
                        {
                            return;
                        }

                        _cancellationToken.ThrowIfCancellationRequested();

                        using (SqlCommand cmd = cn.CreateCommand())
                        {
                            cmd.Transaction = tx;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = Properties.Resources.Import_SelectRecords;

                            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
                            {
                                while (reader.Read())
                                {
                                    _cancellationToken.ThrowIfCancellationRequested();
                                    OnRecordFetched(ParseRecord(reader));
                                }
                            }
                        }
                    }
#warning Пустой catch TODO : добавить лог
                    catch (OperationCanceledException)
                    {
                    }
                    finally
                    {
                        tx.Commit();
                    }
                }
            }
        }

        private void OnRecordFetched(ApplicationOrderRecord record)
        {
            var handler = RecordFetched;
            if (handler != null)
            {
                handler(this, new ApplicationOrderRecordReaderEventArgs(record, _recordCount));
            }
        }

        /// <summary>
        /// Сброс флага обработки для записей в ImportPackageParsed которые не были обработаны
        /// </summary>
        private void ResetIncompleteApplications()
        {
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
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = Properties.Resources.Import_ResetProcessingStatus;
                            SqlParameter statusProcessing = new SqlParameter("@statusProcessing", SqlDbType.Int);
                            statusProcessing.Value = ApplicationOrderRecord.StatusProcessing;
                            cmd.Parameters.Add(statusProcessing);
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

        private static ApplicationOrderRecord ParseRecord(SqlDataReader reader)
        {
            return new ApplicationOrderRecord
            {
                Id = reader.GetIntValueOrDefault(0),
                PackageId = reader.GetIntValue(1),
                InstitutionId = reader.GetIntValue(2),
                PackageCreatedDate = reader.GetDateTimeValue(3),
                PackageModifiedDate = reader.GetDateTimeValue(4),
                CreatedDate = reader.GetDateTimeValue(5),
                ModifiedDate = reader.GetDateTimeValue(6),
                ApplicationNumber = reader.IsDBNull(7) ? null : reader.GetString(7),
                RegistrationDate = reader.GetDateTimeValue(8),
                DirectionId = reader.GetIntValue(9),
                EducationFormId = reader.GetIntValue(10),
                FinanceSourceId = reader.GetIntValue(11),
                EducationLevelId = reader.GetIntValue(12),
                Stage = reader.GetIntValue(13),
                IsBeneficiary = reader.GetBoolean(14),
                IsForeigner = reader.GetBoolean(15),
                Status = reader.GetIntValueOrDefault(16),
                Comment = reader.IsDBNull(17) ? null : reader.GetString(17)
            };
        }
    }
}