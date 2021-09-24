using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FogSoft.Helpers;

namespace FogSoft.Import
{
	/// <summary>
	/// Implements reading from normative XML (non-standard structure).
	/// </summary>
	public class NormativeXmlReader<T> : StructuredReader<T> where T : Record, new()
	{
		private const string RootName = "records";
		private const string RecordName = "record";
		private const string ValueName = "value";
		private readonly IEnumerator<XElement> _enumerator;
		private readonly bool _hasRows;

		public NormativeXmlReader(XDocument document)
		{
			if (document == null) throw new ArgumentNullException("document");
			if (document.Root == null || document.Root.Name != RootName)
				throw new ArgumentException(Errors.RootElementNotFound.FormatWith(RootName));

			_hasRows = document.Root.HasElements;
			_enumerator = document.Root.Elements(RecordName).GetEnumerator();
		}

		public override bool Read()
		{
			lStart:
			if (!_enumerator.MoveNext())
			{
				SetCurrentRecordToNull();
				return false;
			}

			CreateCurrentRecord(ReadLine(_enumerator.Current));

			if (Current.SkipThisRecord)
				goto lStart;

			return true;
		}

		public override bool HasRows
		{
			get { return _hasRows; }
		}

		private string[] ReadLine(XElement record)
		{
			string[] result = new string[SourceFieldCount];
			
			for (int i = 0; i < SourceFieldCount; i++)
			{
				SourceField field = GetSourceField(i);
				if (string.IsNullOrEmpty(field.SourceName))
					throw new InvalidOperationException(Errors.SourceFieldNameNotFound.FormatWith(field.FieldName));
				string value = (from v in record.Elements(ValueName)
				                         where (string) v.Attribute("code") == field.SourceName
				                         select v.Value).FirstOrDefault();
				result[i] = string.IsNullOrEmpty(value) ? value : value.Trim();
			}
			
			return result;
		}
	}
}
