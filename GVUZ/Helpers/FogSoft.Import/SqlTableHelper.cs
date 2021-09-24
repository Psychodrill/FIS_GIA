using System;
using System.Data.SqlClient;
using System.Text;

namespace FogSoft.Import
{
	public static class SqlTableHelper
	{
		public static void CreateTable<T>(SqlTransaction transaction) where T : Record, new()
		{
			if (transaction == null) throw new ArgumentNullException("transaction");

			RecordStructure structure = RecordFactory.GetStructure(typeof(T));
			if (structure.DestinationTable == null)
				throw new ArgumentException(string.Format(Errors.NoDestinationTable, typeof(T).Name));

			CreateTable(structure, transaction);
		}

		internal static void CreateTable(RecordStructure structure, SqlTransaction transaction)
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("CREATE TABLE ").Quote(structure.DestinationTable.TableName).Append("(");
			foreach (DestinationField field in structure.DestinationStructure)
			{
				builder.Quote(field.FieldName).Append(" ").Append(GetSqlType(field)).Append(" NULL,");
			}
			builder.Length--;
			builder.Append(")");

			SqlCommand command = new SqlCommand(builder.ToString(), transaction.Connection, transaction);
			command.ExecuteNonQuery();
		}

		internal static void CreateUniqueConstraint(RecordStructure structure, SqlTransaction transaction)
		{
			string uniqueColumns = structure.DestinationTable.UniqueColumns;
			if (string.IsNullOrEmpty(uniqueColumns))
				return;
			StringBuilder builder = new StringBuilder();
			builder.Append("ALTER TABLE ").Quote(structure.DestinationTable.TableName)
				.Append(" ADD CONSTRAINT ").Quote("UK_" + structure.DestinationTable.TableName)
				.Append(" UNIQUE(").Append(uniqueColumns).Append(")");
		}

		private static StringBuilder Quote(this StringBuilder builder, string name)
		{
			return builder.Append("[").Append(name).Append("]");
		}

		private static string GetSqlType(DestinationField field)
		{
			switch (Type.GetTypeCode(field.Type))
			{
				case TypeCode.Boolean:
					return "BIT";
				case TypeCode.Byte:
				case TypeCode.SByte:
					return "TINYINT";
				case TypeCode.Char:
					return "CHAR(1)";
				case TypeCode.DateTime:
					return "DATETIME";
				case TypeCode.Decimal:
					return "DECIMAL" + (field.Length > 0 ? "(" + field.Length + "," + field.Precision + ")" : "");
				case TypeCode.Double:
					return "FLOAT";
				case TypeCode.Int16:
					return "SMALLINT";
				case TypeCode.Int32:
					return "INT";
				case TypeCode.Int64:
					return "BIGINT";
				case TypeCode.Single:
					return "REAL";
				case TypeCode.String:
					return "VARCHAR(" + (field.Length > 0 ? field.Length.ToString() : "MAX") + ")" + 
						 " Collate Cyrillic_General_CI_AS";
				default:
					if (field.Type == typeof(Guid))
						return "UNIQUEIDENTIFIER";
					if (field.Type == typeof(Guid))
						return "VARBINARY(" + (field.Length > 0 ? field.Length.ToString() : "MAX") + ")";
					throw new ArgumentException(string.Format(Errors.TypeNotSupportedForDatabaseColumn, field.Type.Name));
			}
		}
	}
}
