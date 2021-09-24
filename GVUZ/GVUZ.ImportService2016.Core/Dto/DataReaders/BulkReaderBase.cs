using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkReaderBase<T> : IDataReader
        where T: ImportBase, new()
    {

        protected List<T> _records = new List<T>();
        protected T _current;
        protected readonly List<Func<T, object>> _getters = new List<Func<T,object>>();
        protected readonly Dictionary<string, int> _ordinals = new Dictionary<string,int>();

        //public BulkReaderBase(IEnumerable<T> records)
        //{
            //_getters = new List<Func<T, object>>();
            //_ordinals = new Dictionary<string, int>();
        //    _records = records.ToList();
        //}

        protected object GetDateOrNull(DateTime? datetime)
        {
            if (!datetime.HasValue || datetime.Value.Equals(DateTime.MinValue) || datetime.Value.Year < 1800)
                return (object)DBNull.Value;
            else
            {
                return datetime;
            }
        }

        protected object GetStringOrNull(string value) { return !string.IsNullOrEmpty(value) ? value : (object)DBNull.Value; }
        protected object GetIntOrNull(int? value) {return value.HasValue && value.Value != 0 ? value : (object)DBNull.Value; }

        public virtual void Add(T addinionalRecord)
        {
            _records.Add(addinionalRecord);
        }
        public virtual void AddRange(List<T> addinionalRecords)
        {
            _records.AddRange(addinionalRecords);
        }

        public IEnumerable<SqlBulkCopyColumnMapping> GetColumnMappings()
        {
            return _ordinals.Select(x => new SqlBulkCopyColumnMapping(x.Key, x.Key));
        }

        protected void AddGetter(string field, Func<T, object> getter)
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

        private int counter = 0;
        public bool Read()
        {
            if (counter>=_records.Count())
                return false;

            _current = _records[counter];
            counter++;

            var invalidRow = false;
            if (_current is PackageDataApplication)
                invalidRow &= (_current as PackageDataApplication).IsBroken;

            return !invalidRow || Read();
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
