using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FogSoft.Import
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class SourceFieldAttribute : Attribute
	{
		private string _aggregateWithSeparator;
		private Type _type;

		/// <summary>
		/// Задает параметры для описания поля.</summary>
		/// <param name="ordinal">Порядковый номер в файле импорта.</param>
		/// <param name="fieldName">Название поля в БД.</param>
		public SourceFieldAttribute(int ordinal, string fieldName)
		{
			if (ordinal < 0) throw new ArgumentOutOfRangeException("ordinal");
			if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException("fieldName");
			FieldName = fieldName;
			Ordinal = ordinal;
			_type = typeof(string);
		}

		/// <summary>
		/// Задает параметры для описания поля.</summary>
		/// <param name="ordinal">Порядковый номер в файле импорта.</param>
		/// <param name="fieldName">Название поля в БД.</param>
		/// <param name="sourceName">Название поля в файле импорта.</param>
		public SourceFieldAttribute(int ordinal, string fieldName, string sourceName)
		{
			if (ordinal < 0) throw new ArgumentOutOfRangeException("ordinal");
			if (string.IsNullOrEmpty(fieldName)) throw new ArgumentNullException("fieldName");
			if (string.IsNullOrEmpty(sourceName)) throw new ArgumentOutOfRangeException("sourceName");
			FieldName = fieldName;
			Ordinal = ordinal;
			SourceName = sourceName;
			_type = typeof(string);
		}

		private void ValidateAggregation()
		{
			if (AggregateWithSeparator != null && Type != typeof(string))
				throw new InvalidOperationException
					(string.Format(Errors.AggregateNonStringField, FieldName));
		}

		/// <summary>
		/// Название поля в БД.</summary>
		public string FieldName { get; private set; }

		/// <summary>
		/// Порядковый номер в файле импорта (если не используется импорт по названиям,
		/// иначе используется только для упорядочивания полей при считывании).</summary><seealso cref="SourceName"/>
		public int Ordinal { get; private set; }

		/// <summary>
		/// Название поля в файле импорта (если не используется импорт по порядковому номеру).</summary><seealso cref="Ordinal"/>
		public string SourceName { get; private set; }

		/// <summary>
		/// Тип поля (<see cref="string"/> by default).</summary>
		public Type Type
		{
			get { return _type; }
			set
			{
				_type = value;
				ValidateAggregation();
			}
		}

		/// <summary>
		/// Опциональная длина поля.</summary>
		public int Length { get; set; }

		/// <summary>
		/// Опциональная точность поля.</summary>
		public int Precision { get; set; }

		/// <summary>
		/// Обязательно ли значение.</summary>
		public bool Optional { get; set; }

		/// <summary>
		/// Игнорировать при чтении.</summary>
		public bool Ignored { get; set; }

		/// <summary>
		/// Необязательный <see cref="Regex"/> для очистки входного значения
		/// (окружающие кавычки убираются автоматически).</summary>
		public string CleanRegex { get; set; }

		/// <summary>
		/// Необязательное название <see cref="CultureInfo"/> для преобразования входного значения.</summary>
		public string Culture { get; set; }

		/// <summary>
		/// Разделитель, если необходимо склеивать строки с одинаковым <see cref="FieldName"/>
		/// (если null - строки не склеиваются). Достаточно задать у первого атрибута из серии.</summary>
		public string AggregateWithSeparator
		{
			get { return _aggregateWithSeparator; }
			set
			{
				_aggregateWithSeparator = value;
				ValidateAggregation();
			}
		}

		/// <summary>
		/// Значение должно транслироваться в реализации <see cref="Record.Translate"/>.</summary>
		public bool Translated { get; set; }
	}

	[DebuggerDisplay("{FieldName}")]
	internal class SourceField
	{
		public SourceField(SourceFieldAttribute attribute)
		{
			FieldName = attribute.FieldName;
			Type = attribute.Type;
			Optional = attribute.Optional;
			Ignored = attribute.Ignored;
			CleanRegex = attribute.CleanRegex;
			AggregateWithSeparator = attribute.AggregateWithSeparator;
			Length = attribute.Length;
			Precision = attribute.Precision;
			SourceName = attribute.SourceName;
			Culture = attribute.Culture;
			Translated = attribute.Translated;
		}

		/// <summary>
		/// Название поля в БД.</summary>
		public string FieldName { get; private set; }

		/// <summary>
		/// Название поля в файле импорта (если не используется импорт по порядковому номеру).</summary>
		public string SourceName { get; private set; }

		/// <summary>
		/// Тип поля.</summary>
		public Type Type { get; private set; }

		/// <summary>
		/// Обязательно ли значение.</summary>
		public bool Optional { get; private set; }

		/// <summary>
		/// Игнорировать при чтении.</summary>
		public bool Ignored { get; private set; }

		/// <summary>
		/// Разделитель, если необходимо склеивать строки (если null - строки не склеиваются).</summary>
		public string AggregateWithSeparator { get; private set; }

		/// <summary>
		/// Необязательный <see cref="Regex"/> для очистки входного значения
		/// (окружающие кавычки убираются автоматически).</summary>
		public string CleanRegex { get; private set; }

		/// <summary>
		/// Необязательное название <see cref="CultureInfo"/> для преобразования входного значения.</summary>
		public string Culture { get; set; }

		/// <summary>
		/// Опциональная длина поля.</summary>
		public int Length { get; private set; }

		/// <summary>
		/// Опциональная точность поля.</summary>
		public int Precision { get; private set; }

		/// <summary>
		/// Значение должно транслироваться в реализации <see cref="Record.Translate"/>.</summary>
		public bool Translated { get; set; }
	}

	internal class DestinationField
	{
		private readonly List<int> _sourceIndexes;
		public DestinationField(SourceField sourceField, int sourceIndex)
		{
			FieldName = sourceField.FieldName;
			Type = sourceField.Type;
			Optional = sourceField.Optional;
			AggregateWithSeparator = sourceField.AggregateWithSeparator;
			Length = sourceField.Length;
			Precision = sourceField.Precision;
			Culture = sourceField.Culture;
			Translated = sourceField.Translated;
			
			_sourceIndexes = new List<int> { sourceIndex };
		}

		public void AddSourceIndex(int index)
		{
			if (_sourceIndexes.Count > 0 && AggregateWithSeparator == null)
				throw new InvalidOperationException(Errors.AggregationWithoutSeparator);
			_sourceIndexes.Add(index);
		}

		public IEnumerable<int> GetSourceIndexes()
		{
			return _sourceIndexes;
		}

		/// <summary>
		/// Название поля в БД.</summary>
		public string FieldName { get; private set; }

		/// <summary>
		/// Тип поля.</summary>
		public Type Type { get; private set; }

		/// <summary>
		/// Обязательно ли значение.</summary>
		public bool Optional { get; private set; }

		/// <summary>
		/// Разделитель, если необходимо склеивать строки (если null - строки не склеиваются).</summary>
		public string AggregateWithSeparator { get; private set; }

		/// <summary>
		/// Необязательное название <see cref="CultureInfo"/> для преобразования входного значения.</summary>
		public string Culture { get; set; }

		/// <summary>
		/// Нужно ли склеивать строки.</summary>
		public bool IsAggregated { get { return _sourceIndexes.Count > 1; } }

		/// <summary>
		/// Опциональная длина поля.</summary>
		public int Length { get; private set; }

		/// <summary>
		/// Опциональная точность поля.</summary>
		public int Precision { get; private set; }

		/// <summary>
		/// Значение должно транслироваться в реализации <see cref="Record.Translate"/>.</summary>
		public bool Translated { get; set; }
	}
}
