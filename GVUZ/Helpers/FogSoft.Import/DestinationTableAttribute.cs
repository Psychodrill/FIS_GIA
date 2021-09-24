using System;

namespace FogSoft.Import
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class DestinationTableAttribute : Attribute
	{
		/// <summary>
		/// Задает параметры для таблицы-приемника.</summary>
		public DestinationTableAttribute(string tableName)
		{
			if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException("tableName");
			TableName = tableName;
			Timeout = 60;
		}

		/// <summary>
		/// Название таблицы в БД.</summary>
		public string TableName { get; private set; }

		/// <summary>
		/// Опциональный список уникальных столбцов через запятую (для временной таблицы).</summary>
		public string UniqueColumns { get; set; }

		/// <summary>
		/// Опциональный SQL-запрос для пост-обработки.</summary>
		public string PostProcessSql { get; set; }

		/// <summary>
		/// Таймаут в секундах для обработки.</summary>
		public int Timeout { get; set; }
	}

	internal class DestinationTable
	{
		public DestinationTable(DestinationTableAttribute attribute)
		{
			TableName = attribute.TableName;
			PostProcessSql = attribute.PostProcessSql;
			Timeout = attribute.Timeout;
			UniqueColumns = attribute.UniqueColumns;
		}

		/// <summary>
		/// Название таблицы в БД.</summary>
		public string TableName { get; private set; }

		/// <summary>
		/// Опциональный SQL-запрос для пост-обработки.</summary>
		public string PostProcessSql { get; private set; }

		/// <summary>
		/// Таймаут в секундах для обработки.</summary>
		public int Timeout { get; private set; }

		/// <summary>
		/// Опциональный список уникальных столбцов через запятую (для временной таблицы).</summary>
		public string UniqueColumns { get; private set; }
	}
}
