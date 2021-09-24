namespace Ege.Dal.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using JetBrains.Annotations;

    public static class DbTypeExtensions
    {
        [NotNull] private static readonly Dictionary<DbType, string> ToSqlMap = new Dictionary<DbType, string>
            {
                {DbType.AnsiString, "varchar(max)"},
                {DbType.AnsiStringFixedLength, "varchar(max)"},
                {DbType.Binary, "varbinary(max)"},
                {DbType.Boolean, "bit"},
                {DbType.Byte, "tinyint"},
                {DbType.Date, "Date"},
                {DbType.DateTime, "datetime"},
                {DbType.DateTime2, "datetime2"},
                {DbType.DateTimeOffset, "datetimeoffset"},
                {DbType.Decimal, "decimal"},
                {DbType.Double, "float"},
                {DbType.Guid, "uniqueidentifier"},
                {DbType.Int16, "smallint"},
                {DbType.Int32, "int"},
                {DbType.Int64, "bigint"},
                {DbType.Object, "sql_variant"},
                {DbType.SByte, "smallint"},
                {DbType.Single, "float"},
                {DbType.String, "nvarchar(max)"},
                {DbType.StringFixedLength, "nvarchar(max)"},
                {DbType.Time, "time"},
                {DbType.UInt16, "int"},
                {DbType.UInt32, "bigint"},
                {DbType.UInt64, "decimal(20,0)"},
                {DbType.Xml, "varchar(max)"},
            };

        public static string ToSqlServer(this DbType dbType)
        {
            string result;
            if (!ToSqlMap.TryGetValue(dbType, out result))
            {
                throw new ArgumentOutOfRangeException("dbType", string.Format("Can not found {0} in map", dbType));
            }
            return result;
        }
    }
}