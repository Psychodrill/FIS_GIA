using System;
using System.Data;
using System.Data.Common;

namespace FogSoft.Import
{
	/// <summary>
	/// Provides basic implementation for the simple <see cref="IDataReader"/>.</summary>
	public abstract class AbstractDataReader : DbDataReader
	{
		protected abstract IDataRecord CurrentRecord { get; }

		public override int Depth
		{
			get { return 1; }
		}

		public override bool IsClosed
		{
			get { return false; }
		}

		public override int RecordsAffected
		{
			get { return -1; }
		}

		public override object GetValue(int i)
		{
			return CurrentRecord.GetValue(i);
		}

		public override int GetValues(object[] values)
		{
			return CurrentRecord.GetValues(values);
		}

		public override bool GetBoolean(int i)
		{
			return CurrentRecord.GetBoolean(i);
		}

		public override byte GetByte(int i)
		{
			return CurrentRecord.GetByte(i);
		}

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return CurrentRecord.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public override char GetChar(int i)
		{
			return CurrentRecord.GetChar(i);
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return CurrentRecord.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public override Guid GetGuid(int i)
		{
			return CurrentRecord.GetGuid(i);
		}

		public override short GetInt16(int i)
		{
			return CurrentRecord.GetInt16(i);
		}

		public override int GetInt32(int i)
		{
			return CurrentRecord.GetInt32(i);
		}

		public override long GetInt64(int i)
		{
			return CurrentRecord.GetInt64(i);
		}

		public override float GetFloat(int i)
		{
			return CurrentRecord.GetFloat(i);
		}

		public override double GetDouble(int i)
		{
			return CurrentRecord.GetDouble(i);
		}

		public override string GetString(int i)
		{
			return CurrentRecord.GetString(i);
		}

		public override decimal GetDecimal(int i)
		{
			return CurrentRecord.GetDecimal(i);
		}

		public override DateTime GetDateTime(int i)
		{
			return CurrentRecord.GetDateTime(i);
		}

		protected override DbDataReader GetDbDataReader(int ordinal)
		{
			return (DbDataReader)CurrentRecord.GetData(ordinal);
		}

		public override bool IsDBNull(int i)
		{
			return CurrentRecord.IsDBNull(i);
		}

		public override object this[int i]
		{
			get { return CurrentRecord[i]; }
		}

		public override object this[string name]
		{
			get { return CurrentRecord[name]; }
		}

		public override DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public override bool NextResult()
		{
			throw new NotImplementedException();
		}

		public override void Close()
		{
		}

		protected override void Dispose(bool disposing)
		{			
		}
	}
}
