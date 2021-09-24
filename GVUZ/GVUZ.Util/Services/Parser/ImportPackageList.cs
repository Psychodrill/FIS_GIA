using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace GVUZ.Util.Services.Parser
{
    public class ImportPackageList : IEnumerable<ImportPackageDto>, IEnumerator<ImportPackageDto>
    {
        private SqlConnection _cn;
        private SqlTransaction _tx;
        private readonly ParseImportPackageFilter _filter;
        private int? _packageCount;
        private readonly int _pageSize;
        public const int DefaultPageSize = 100;
        private readonly string _connectionString;
        private SqlDataReader _reader;
        private SqlCommand _readerCommand;
        private int _offset;

        public EventHandler<ImportPackageListReadProgressEventArgs> Progress;

        public ImportPackageList(string connectionString, ParseImportPackageFilter filter, int pageSize = DefaultPageSize)
        {
            if (pageSize <= 0)
            {
                _pageSize = DefaultPageSize;
            }

            _pageSize = pageSize;
            _filter = filter;
            _connectionString = connectionString;
        }

        public int Count
        {
            get { return GetPackageCount(); }
        }

        #region IEnumerator
        public bool MoveNext()
        {
            if (GetPackageCount() == 0)
            {
                Reset();
                return false;
            }

            if (!ReadPackage())
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }

                if (_offset < GetPackageCount())
                {
                    _reader = LoadNext();
                    _offset += _pageSize;
                    if (ReadPackage())
                    {
                        return true;
                    }
                }

                Reset();
                return false;
            }

            return true;
        }

        public void Reset()
        {
            CommitTransaction();
            ResetInternal();
        }

        private void ResetInternal()
        {
            _pcnt = 0;
            Current = null;
            _offset = 0;
            //_packageCount = null;
            
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }

            if (_readerCommand != null)
            {
                _readerCommand.Dispose();
                _readerCommand.Dispose();
            }

            if (_cn != null)
            {
                if (_cn.State == ConnectionState.Open)
                {
                    _cn.Close();
                }
                _cn.Dispose();
                _cn = null;
            }
        }

        public ImportPackageDto Current { get; private set; }
        public bool ReportProgress { get; set; }
        public int  ProgressNotificationRate { get; set; }
        private decimal _pcnt;

        #endregion

        private bool ReadPackage()
        {
            if (_reader != null && _reader.Read())
            {
                Current = new ImportPackageDto
                    {
                        PackageId = _reader.GetInt32(0),
                        PackageData = _reader.IsDBNull(1) ? null : _reader.GetString(1),
                        CreateDate = _reader.GetDateTime(2),
                        LastDateChanged = _reader.GetDateTime(3),
                        InstitutionId = _reader.GetInt32(4)
                    };

                if (ReportProgress && ProgressNotificationRate > 0)
                {
                    int progress = (int)Math.Ceiling((++_pcnt / GetPackageCount()) * 100);

                    if (progress % ProgressNotificationRate == 0)
                    {
                        OnReportProgress(progress);
                    }
                }

                return true;
            }

            return false;
        }

        private void OnReportProgress(int percentComplete)
        {
            var handler = Progress;

            if (handler != null)
            {
                handler(this, new ImportPackageListReadProgressEventArgs(GetPackageCount(), (int)_pcnt, percentComplete));
            }
        }

        private SqlCommand GetReaderCommand()
        {
            if (_readerCommand == null)
            {
                const string selectQuery = @"select b.*
from (
SELECT a.*, ROW_NUMBER() OVER (ORDER BY PackageId) rnum
from (
SELECT
t.[PackageID],
t.[PackageData],
t.[CreateDate],
t.[LastDateChanged],
t.[InstitutionID]
from [dbo].[ImportPackage] t WITH (NOLOCK) INNER JOIN #TMP_PARSE_ID tmp on t.[PackageID] = tmp.[PackageId]) a
) b
where b.rnum between @skip and @take";

                SqlCommand cmd = CreateCommand(selectQuery);

                SqlParameter skip = cmd.CreateParameter();
                skip.ParameterName = "@skip";
                skip.Size = sizeof(int);
                skip.SqlDbType = SqlDbType.Int;

                SqlParameter take = cmd.CreateParameter();
                take.ParameterName = "@take";
                take.Size = sizeof(int);
                take.SqlDbType = SqlDbType.Int;

                cmd.Parameters.Add(skip);
                cmd.Parameters.Add(take);

                cmd.Prepare();

                _readerCommand = cmd;
            }

            return _readerCommand;
        }

        private SqlDataReader LoadNext()
        {
            Debug.WriteLine("DB HIT LoadNextPage");

            SqlCommand cmd = GetReaderCommand();
            cmd.Transaction = GetTransaction();
            cmd.Parameters["@skip"].Value = _offset + 1;
            cmd.Parameters["@take"].Value = _offset + _pageSize;

            return cmd.ExecuteReader();
        }

        private int GetPackageCount()
        {
            if (!_packageCount.HasValue)
            {
                _packageCount = GetPackageCountInternal();
            }

            return _packageCount.Value;
        }

        private int GetPackageCountInternal()
        {
            Debug.WriteLine("DB HIT GetPackageCount");

            const string createTempTable = @"CREATE TABLE #TMP_PARSE_ID([PackageId] int IDENTITY(1, 1) NOT NULL, CONSTRAINT [PK_TMP_PARSE_{0}] PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
))";
            string tempTable = string.Format(createTempTable, Guid.NewGuid().ToString("N").ToUpper());

            using (SqlCommand cmd = CreateCommand(tempTable))
            {
                cmd.ExecuteNonQuery();
            }

            StringBuilder insertSelect = new StringBuilder();
            insertSelect.AppendLine("SET IDENTITY_INSERT #TMP_PARSE_ID ON");
            insertSelect.Append("INSERT INTO #TMP_PARSE_ID([PackageId]) SELECT DISTINCT t.[PackageID] from [dbo].[ImportPackage] t WITH (NOLOCK)");

            insertSelect.Append(" LEFT JOIN [dbo].[ImportPackageParsed] p WITH (NOLOCK) on t.[PackageID] = p.[PackageId] WHERE p.[PackageId] IS NULL AND t.[PackageData] IS NOT NULL AND t.[TypeID] = 1 AND t.[Content] LIKE('%приказ%')");

            if (_filter != null)
            {
                if (_filter.InstitutionId != null && _filter.InstitutionId.Length > 0)
                {
                    insertSelect.Append(" AND t.[InstitutionID] IN (");
                    insertSelect.Append(string.Join(", ", _filter.InstitutionId));
                    insertSelect.Append(")");
                }

                if (_filter.MinDate.HasValue)
                {
                    insertSelect.Append(" AND t.[CreateDate] >= @minDate");
                }
            }

            insertSelect.AppendLine();
            insertSelect.Append("SET IDENTITY_INSERT #TMP_PARSE_ID OFF");
            using (SqlCommand cmd = CreateCommand(insertSelect.ToString()))
            {
                if (_filter != null && _filter.MinDate.HasValue)
                {
                    SqlParameter minDate = cmd.CreateParameter();
                    minDate.ParameterName = "@minDate";
                    minDate.SqlDbType = SqlDbType.DateTime;
                    minDate.Value = _filter.MinDate.Value.Date;
                    cmd.Parameters.Add(minDate);
                }

                cmd.ExecuteNonQuery();
            }

            const string selectCount = "select count(*) from #TMP_PARSE_ID";

            using (SqlCommand cmd = CreateCommand(selectCount))
            {
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                CommitTransaction();
                return result;
            }
        }

        private SqlCommand CreateCommand(string commandText = null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Transaction = GetTransaction();
            cmd.Connection = cmd.Transaction.Connection;
            cmd.CommandText = commandText;
            cmd.CommandTimeout = 120;
            return cmd;
        }

        private SqlTransaction GetTransaction()
        {
            if (_cn == null)
            {
                _cn = new SqlConnection(_connectionString);
            }

            if (_tx == null)
            {
                if (_cn.State == ConnectionState.Closed)
                {
                    _cn.Open();
                }

                _tx = _cn.BeginTransaction(IsolationLevel.ReadCommitted);
            }

            return _tx;
        }

        private void CommitTransaction()
        {
            if (_tx != null)
            {
                _tx.Commit();
                _tx.Dispose();
                _tx = null;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                {
                    try
                    {
                        _reader.Dispose();
                        _reader = null;
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        _reader = null;
                    }
                    
                }

                if (_tx != null)
                {
                    _tx.Dispose();
                    _tx = null;
                }

                ResetInternal();
            }
        }

        public IEnumerator<ImportPackageDto> GetEnumerator()
        {
            return this;
        }

        ~ImportPackageList()
        {
            Dispose(false);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}