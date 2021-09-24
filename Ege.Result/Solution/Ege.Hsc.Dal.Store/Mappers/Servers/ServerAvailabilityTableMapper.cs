namespace Ege.Hsc.Dal.Store.Mappers.Servers
{
    using System.Collections.Generic;
    using System.Data;
    using Ege.Dal.Common.Mappers;

    class ServerAvailabilityTableMapper : IDataTableMapper<IDictionary<int, bool>>
    {
        private const string RegionIdColumn = "RegionId";
        private const string IsAvailableColumn = "IsAvailable";

        public DataTable Map(IDictionary<int, bool> @from)
        {
            var result = new DataTable();
            result.Columns.Add(RegionIdColumn, typeof(int));
            result.Columns.Add(IsAvailableColumn, typeof(bool));
            foreach (var kv in from ?? new Dictionary<int, bool>())
            {
                result.Rows.Add(kv.Key, kv.Value);
            }
            return result;
        }
    }
}
