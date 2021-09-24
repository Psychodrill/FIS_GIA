using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FogSoft.Helpers;
using System.Linq;

namespace FogSoft.Import
{
	/// <summary>
	/// Base class for importing records. Inheritors should be created with <see cref="RecordFactory"/>.</summary>
	public class Record : AbstractDataRecord
	{
		private RecordStructure _structure;

		internal void Initialize(RecordStructure structure, string[] sourceValues)
		{
			if (sourceValues.Length > structure.SourceStructure.Length)
				throw new ArgumentException(Errors.TooManySourceColumns);
			if (sourceValues.Length < structure.SourceStructure.Select(x => x.SourceName).Distinct().Count())
				throw new ArgumentException(Errors.InsufficientInputValues);
			
			_structure = structure;
			SourceValues = sourceValues;
		}

		/// <summary>
		/// Функция трансляции. <seealso cref="SourceFieldAttribute.Translated"/></summary>
		public virtual object Translate(string fieldName, object sourceValue)
		{
			return sourceValue;
		}

		public override string GetDataTypeName(int i)
		{
			return null;
		}

		public override Type GetFieldType(int i)
		{
			return null;
		}

		public override object GetValue(int i)
		{
			if (_structure == null)
				throw new InvalidOperationException(Errors.RecordDoesNotInitialized);
			if (i >= _structure.DestinationStructure.Length)
				throw new ArgumentOutOfRangeException("i");
			
			DestinationField field = _structure.DestinationStructure[i];

			if (field.IsAggregated)
				return GetAggregatedString(field);

			string sourceValue = CleanSourceValue(field.GetSourceIndexes().First());
			
			if (string.IsNullOrEmpty(sourceValue) && !field.Optional && !field.Translated)
				throw new ArgumentException
							(string.Format(Errors.RequiredFieldIsNullOrEmpty, field.FieldName));

			CultureInfo cultureInfo = string.IsNullOrEmpty(field.Culture) ? null : new CultureInfo(field.Culture);
			object source = ParseHelper.ConvertTo(field.Type, sourceValue, null, cultureInfo);
			return field.Translated ? Translate(field.FieldName, source) : source;
		}

		private string CleanSourceValue(int sourceIndex)
		{
			string sourceValue = SourceValues[sourceIndex];
			if (sourceValue == null)
				return null;
			if (!string.IsNullOrEmpty(sourceValue) &&
				!string.IsNullOrEmpty(_structure.SourceStructure[sourceIndex].CleanRegex))
				sourceValue = Regex.Replace
					(sourceValue, _structure.SourceStructure[sourceIndex].CleanRegex, "", RegexOptions.Multiline);

			if (string.IsNullOrEmpty(sourceValue))
				return null;
			return sourceValue;
		}

		private string GetAggregatedString(DestinationField field)
		{
			int indexProcessed = 0;
			StringBuilder builder = new StringBuilder();
			foreach (int sourceIndex in field.GetSourceIndexes())
			{
				string sourceValue = CleanSourceValue(sourceIndex);
				if (string.IsNullOrEmpty(sourceValue))
				{
					if (indexProcessed == 0 && !field.Optional)
						throw new ArgumentException
							(string.Format(Errors.RequiredFieldIsNullOrEmpty, field.FieldName));
				}
				else
				{
					builder.Append(sourceValue).Append(field.AggregateWithSeparator);
				}

				indexProcessed++;
			}
			if (builder.Length > 0 && field.AggregateWithSeparator.Length > 0)
				builder.Length--;
			return builder.ToString();
		}

		public override int FieldCount
		{
			get { return _structure.DestinationStructure.Length; }
		}

		public override string GetName(int i)
		{
			return _structure.DestinationStructure[i].FieldName;
		}

		public override int GetOrdinal(string name)
		{
			return _structure.GetOrdinal(name);
		}

		protected string[] SourceValues { get; private set; }

		/// <summary>
		/// Override this method to skip record from reader.</summary>
		public virtual bool SkipThisRecord { get { return false; } }
	}
}
