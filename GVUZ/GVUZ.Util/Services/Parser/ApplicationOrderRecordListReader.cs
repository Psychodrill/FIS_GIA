using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GVUZ.Util.Services.Parser
{
    public class ApplicationOrderRecordListReader : IDataReader
    {
        private readonly IEnumerable<ApplicationOrderRecordDto> _records;
        private ApplicationOrderRecordDto _current;
        private readonly List<Func<ApplicationOrderRecordDto, object>> _getters;
        private readonly Dictionary<string, int> _ordinals;

        public ApplicationOrderRecordListReader(IEnumerable<ApplicationOrderRecordDto> records)
        {
            _records = records;
            _getters = new List<Func<ApplicationOrderRecordDto, object>>();
            _ordinals = new Dictionary<string, int>();

            AddGetter("ApplicationNumber", dto => dto.ApplicationNumber ?? (object)DBNull.Value);
            AddGetter("Comment", dto => dto.Comment ?? (object)DBNull.Value);
            AddGetter("CreatedDate", dto => dto.CreatedDate);
            AddGetter("DirectionID", dto => dto.DirectionId.HasValue ? dto.DirectionId.Value : (object)DBNull.Value);
            AddGetter("EducationFormID", dto => dto.EducationFormId.HasValue ? dto.EducationFormId.Value : (object)DBNull.Value);
            AddGetter("EducationLevelID", dto => dto.EducationLevelId.HasValue ? dto.EducationLevelId.Value : (object)DBNull.Value);
            AddGetter("FinanceSourceID", dto => dto.FinanceSourceId.HasValue ? dto.FinanceSourceId.Value : (object)DBNull.Value);
            AddGetter("InstitutionID", dto => dto.InstitutionId);
            AddGetter("IsBeneficiary", dto => dto.IsBeneficiary);
            AddGetter("IsForeigner", dto => dto.IsForeigner);
            AddGetter("ModifiedDate", dto => dto.ModifiedDate);
            AddGetter("PackageCreatedDate", dto => dto.PackageCreatedDate);
            AddGetter("PackageID", dto => dto.PackageId);
            AddGetter("PackageModifiedDate", dto => dto.PackageModifiedDate);
            AddGetter("RegistrationDate", dto => dto.RegistrationDate.HasValue ? dto.RegistrationDate.Value : (object)DBNull.Value);
            AddGetter("Stage", dto => dto.Stage.HasValue ? dto.Stage.Value : 0);
            AddGetter("Status", dto => dto.Status.HasValue ? dto.Status.Value : (object)DBNull.Value);
        }

        public IEnumerable<SqlBulkCopyColumnMapping> GetColumnMappings()
        {
            return _ordinals.Select(x => new SqlBulkCopyColumnMapping(x.Key, x.Key));
        }

        private void AddGetter(string field, Func<ApplicationOrderRecordDto, object> getter)
        {
            _getters.Add(getter);
            _ordinals.Add(field, _ordinals.Count);
        }

        public object GetValue(int i)
        {
            return _getters[i](_current);
        }

        public int GetOrdinal(string name)
        {
            return _ordinals[name];
        }

        public int FieldCount
        {
            get { return _getters.Count; }
        }

        public bool Read()
        {
            if (_records.GetEnumerator().MoveNext())
            {
                _current = _records.GetEnumerator().Current;
                return true;
            }

            return false;
        }

        #region Not implemented
        public void Dispose()
        {
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        object IDataRecord.this[int i]
        {
            get { throw new NotImplementedException(); }
        }

        object IDataRecord.this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public int Depth { get; private set; }
        public bool IsClosed { get; private set; }
        public int RecordsAffected { get; private set; }
        #endregion
    }
}