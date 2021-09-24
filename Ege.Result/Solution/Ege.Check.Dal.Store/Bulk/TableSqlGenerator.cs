namespace Ege.Check.Dal.Store.Bulk
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Ege.Dal.Common.Helpers;
    using JetBrains.Annotations;

    internal class TableSqlGenerator : ITableSqlGenerator
    {
        [NotNull] private readonly IDbTypeFactory _dbTypeFactory;

        public TableSqlGenerator([NotNull] IDbTypeFactory dbTypeFactory)
        {
            _dbTypeFactory = dbTypeFactory;
        }

        public string CreateSql([NotNull] DataTable dt, string tableName)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Create table [{0}] (", tableName).AppendLine();
            sb.Append(string.Join(string.Format(",{0}", Environment.NewLine), CreateColumnsSql(dt))).AppendLine();
            sb.Append(")");
            return sb.ToString();
        }

        public string DropSql(string tableName)
        {
            return string.Format("Drop table [{0}]", tableName);
        }

        [NotNull]
        private IEnumerable<string> CreateColumnsSql([NotNull] DataTable dt)
        {
            return
                dt.Columns.Cast<DataColumn>().Select(
                    dataColumn =>
                    string.Format("{0} {1} {2}", dataColumn.ColumnName,
                                  _dbTypeFactory.Create(dataColumn.DataType).ToSqlServer(),
                                  !dataColumn.AllowDBNull
                                      ? " not null "
                                      : string.Empty));
        }
    }
}