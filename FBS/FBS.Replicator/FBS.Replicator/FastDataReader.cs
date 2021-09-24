using System.Collections.Generic;
using System.Data;

namespace FBS.Replicator
{
    public class FastDataReader
    {
        public FastDataReader(IDataReader reader)
        {
            _reader = reader;
            _ordinals = new Dictionary<string, int>();

            for (int i = 0; i < _reader.FieldCount; i++)
            {
                _ordinals.Add(reader.GetName(i), i);
            }
        }

        private readonly IDataReader _reader;
        private readonly Dictionary<string, int> _ordinals;

        public object GetObject(string columnName)
        {
            return _reader.GetValue(GetOrdinal(columnName));
        }

        public bool IsNull(string columnName)
        {
            return _reader.IsDBNull(GetOrdinal(columnName));
        }

        private int GetOrdinal(string columnName)
        {
            if (!_ordinals.ContainsKey(columnName))
                return _reader.GetOrdinal(columnName);
            else
                return _ordinals[columnName];
        }
    }
}
