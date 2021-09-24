using System;
using System.Collections.Generic;

namespace FogSoft.Import
{
	internal class RecordStructure
	{
		public readonly SourceField[] SourceStructure;
		public readonly DestinationField[] DestinationStructure;

		/// <summary>
		/// Описание таблицы-приемника (может быть не задано).</summary>
		public readonly DestinationTable DestinationTable;

		public RecordStructure(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			DestinationTable = GetDestinationTable(type);
				
			SourceStructure = GetSourceFields(type);
			DestinationStructure = GetDestinationFields(SourceStructure).ToArray();
		}

		public int GetOrdinal(string name)
		{
			for (int i = 0; i < DestinationStructure.Length; i++)
			{
				if (name.Equals(DestinationStructure[i].FieldName,
					StringComparison.InvariantCultureIgnoreCase))
					return i;
			}
			throw new IndexOutOfRangeException();
		}

		private static DestinationTable GetDestinationTable(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(DestinationTableAttribute), true);
			if (customAttributes.Length == 0)
				return null;
			return new DestinationTable((DestinationTableAttribute)customAttributes[0]);
		}

		private static SourceField[] GetSourceFields(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof (SourceFieldAttribute), true);
			if (customAttributes.Length == 0)
				throw new ArgumentException(string.Format(Errors.SourceFieldAttributeNotSpecified, type.Name));

			SourceField[] sourceFields = new SourceField[customAttributes.Length];
			
			int processed = 0;
			foreach (SourceFieldAttribute attribute in customAttributes)
			{
				if (attribute.Ordinal > sourceFields.Length)
					throw new InvalidOperationException
						(string.Format(Errors.InvalidOrdinal,
									   attribute.Ordinal, attribute.FieldName, sourceFields.Length));

				processed++;
				sourceFields[attribute.Ordinal] = new SourceField(attribute);
			}

			if (processed != sourceFields.Length)
				throw new InvalidOperationException(string.Format(Errors.ProcessedAttributes,
																  processed, sourceFields.Length));
			return sourceFields;
		}

		private static List<DestinationField> GetDestinationFields(SourceField[] sourceFields)
		{
			List<DestinationField> destinationFields = new List<DestinationField>(sourceFields.Length);
			Dictionary<string, int> usedDestinationFields = 
				new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

			Dictionary<string, int> indexes = new Dictionary<string, int>();
			
			for (int sourceIndex = 0; sourceIndex < sourceFields.Length; sourceIndex++)
			{
				SourceField field = sourceFields[sourceIndex];
				if (field.Ignored) continue;

				int destIndex;
				// с учётом трансляции некоторые столбцы могут повторяться
				int firstSourceIndex;
				if (!indexes.TryGetValue(field.SourceName, out firstSourceIndex))
				{
					indexes.Add(field.SourceName, firstSourceIndex = sourceIndex);
				}
				int finalSourceIndex = field.Translated ? firstSourceIndex : sourceIndex;

				if (usedDestinationFields.TryGetValue(field.FieldName, out destIndex))
				{
					destinationFields[destIndex].AddSourceIndex(finalSourceIndex);
				}
				else
				{
					destinationFields.Add(new DestinationField(field, finalSourceIndex));
					usedDestinationFields.Add(field.FieldName, finalSourceIndex);
				}
			}
			
			return destinationFields;
		}
	}
}
