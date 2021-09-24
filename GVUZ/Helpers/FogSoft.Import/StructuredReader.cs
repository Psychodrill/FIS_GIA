using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace FogSoft.Import
{
	/// <summary>
	/// Provides basic implementation for the simple <see cref="IDataReader"/>
	/// with <see cref="RecordStructure"/> (to implement related methods).</summary>
	public abstract class StructuredReader<T> : AbstractDataReader, IEnumerable<T> where T : Record, new()
	{
		private readonly RecordStructure _structure;
		private T _currentRecord;

		protected StructuredReader()
		{
			_structure = RecordFactory.GetStructure(typeof(T));
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			while (Read())
			{
				yield return Current;
			}
		}

		public override IEnumerator GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		internal SourceField GetSourceField(int index)
		{
			return _structure.SourceStructure[index];
		}

		public override int GetOrdinal(string name)
		{
			return _structure.GetOrdinal(name);
		}

		internal int SourceFieldCount
		{
			get { return _structure.SourceStructure.Length; }
		}

		protected void CreateCurrentRecord(string[] sourceValues)
		{
			_currentRecord = RecordFactory.Create<T>(sourceValues);
		}

		protected void SetCurrentRecordToNull()
		{
			_currentRecord = null;
		}

		protected T Current { get { return _currentRecord; } }

		protected override IDataRecord CurrentRecord
		{
			get { return _currentRecord; }
		}

		public override int FieldCount
		{
			get { return _structure.DestinationStructure.Length; }
		}

		public override string GetName(int i)
		{
			return _structure.DestinationStructure[i].FieldName;
		}

		public override string GetDataTypeName(int i)
		{
			return _structure.DestinationStructure[i].Type.Name;
		}

		public override Type GetFieldType(int i)
		{
			return _structure.DestinationStructure[i].Type;
		}
	}
}
