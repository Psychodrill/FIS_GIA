using System;
using System.Data.SqlClient;

namespace FogSoft.Import
{
	public class SqlClientCsvImporter
	{
		public const SqlBulkCopyOptions DefaultBulkCopyOptions =
			SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints;

		public const int DefaultTimeout = 30;

		public object Import(AbstractDataReader reader, SqlTransaction transaction, Type recordType,
			SqlBulkCopyOptions bulkCopyOptions = DefaultBulkCopyOptions)
		{
			if (reader == null) throw new ArgumentNullException("reader");
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (recordType == null) throw new ArgumentNullException("recordType");

			RecordStructure structure = RecordFactory.GetStructure(recordType);
			DestinationTable table = structure.DestinationTable;
			if (table == null)
				throw new ArgumentException(string.Format(Errors.NoDestinationTable, recordType.Name));

			bool isTemporaryTable = table.TableName.StartsWith("#");
			if (isTemporaryTable)
				SqlTableHelper.CreateTable(structure, transaction);

			SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(transaction.Connection, bulkCopyOptions, transaction)
			                          	{
			                          		DestinationTableName = table.TableName,
			                          		BulkCopyTimeout = table.Timeout > 0 ? table.Timeout : DefaultTimeout
			                          	};
			sqlBulkCopy.WriteToServer(reader);

			if (isTemporaryTable)
				SqlTableHelper.CreateUniqueConstraint(structure, transaction);

			if (!string.IsNullOrEmpty(table.PostProcessSql))
			{
				SqlCommand command = new SqlCommand(table.PostProcessSql, transaction.Connection, transaction)
				                     	{
				                     		CommandTimeout = table.Timeout > 0 ? table.Timeout : DefaultTimeout
				                     	};
				return command.ExecuteScalar();
			}
			return null;
		}
	}
}
