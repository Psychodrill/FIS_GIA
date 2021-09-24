using System;
using System.Data;

namespace FogSoft.Import
{
	public abstract class AbstractDataRecord : IDataRecord
	{
		public abstract object GetValue(int i);
		public abstract string GetName(int i);
		public abstract int GetOrdinal(string name);

		public abstract string GetDataTypeName(int i);
		public abstract Type GetFieldType(int i);

		public abstract int FieldCount
		{
			get;
		}

		public virtual bool IsDBNull(int i)
		{
			object value = GetValue(i);
			return (value == null || value == DBNull.Value);
		}

		public int GetValues(object[] values)
		{
			if (values == null)
				throw new ArgumentNullException();
			if (values.Length < FieldCount)
				throw new ArgumentException();
			for (int i = 0; i < FieldCount; i++)
				values[i] = GetValue(i);
			return FieldCount;
		}

		public bool GetBoolean(int i)
		{
			return Convert.ToBoolean(GetValue(i));
		}

		public byte GetByte(int i)
		{
			return Convert.ToByte(GetValue(i));
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			throw new NotImplementedException();
		}

		public char GetChar(int i)
		{
			return Convert.ToChar(GetValue(i));
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
			return Convert.ToInt16(GetValue(i));
		}

		public int GetInt32(int i)
		{
			return Convert.ToInt32(GetValue(i));
		}

		public long GetInt64(int i)
		{
			return Convert.ToInt64(GetValue(i));
		}

		public float GetFloat(int i)
		{
			return Convert.ToSingle(GetValue(i));
		}

		public double GetDouble(int i)
		{
			return Convert.ToDouble(GetValue(i));
		}

		public string GetString(int i)
		{
			return Convert.ToString(GetValue(i));
		}

		public decimal GetDecimal(int i)
		{
			return Convert.ToDecimal(GetValue(i));
		}

		public DateTime GetDateTime(int i)
		{
			return Convert.ToDateTime(GetValue(i));
		}

		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		public object this[int i]
		{
			get { return GetValue(i); }
		}

		public object this[string name]
		{
			get { return GetValue(GetOrdinal(name)); }
		}
	}
}
