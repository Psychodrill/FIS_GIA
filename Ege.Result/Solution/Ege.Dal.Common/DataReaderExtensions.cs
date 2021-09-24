namespace Ege.Dal.Common
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    internal static class DataReaderExtensions
    {
        public static async Task<string> GetNullableStringAsync([NotNull] this DbDataReader reader, int colIndex)
        {
            return !(await reader.IsDBNullAsync(colIndex)) ? reader.GetString(colIndex) : string.Empty;
        }

        public static async Task<int?> GetNullableInt32Async([NotNull] this DbDataReader reader, int colIndex)
        {
            return !(await reader.IsDBNullAsync(colIndex)) ? reader.GetInt32(colIndex) : (int?) null;
        }

        public static async Task<bool?> GetNullableBooleanAsync([NotNull] this DbDataReader reader, int colIndex)
        {
            return !(await reader.IsDBNullAsync(colIndex)) ? reader.GetBoolean(colIndex) : (bool?) null;
        }

        public static async Task<DateTime?> GetNullableDateTimeAsync([NotNull] this DbDataReader reader, int colIndex)
        {
            return !(await reader.IsDBNullAsync(colIndex)) ? reader.GetDateTime(colIndex) : (DateTime?) null;
        }

        public static async Task<Guid?> GetNullableGuidAsync([NotNull] this DbDataReader reader, int colIndex)
        {
            return !(await reader.IsDBNullAsync(colIndex)) ? reader.GetGuid(colIndex) : (Guid?)null;
        }
    }
}