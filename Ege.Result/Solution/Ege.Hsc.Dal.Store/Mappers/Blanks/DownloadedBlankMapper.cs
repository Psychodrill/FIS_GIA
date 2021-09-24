namespace Ege.Hsc.Dal.Store.Mappers.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Logic.Models.Blanks;

    class DownloadedBlankMapper : IDataTableMapper<IEnumerable<DownloadedBlank>>
    {
        public DataTable Map(IEnumerable<DownloadedBlank> @from)
        {
            var result = new DataTable();
            result.Columns.Add("RbdId", typeof(Guid));
            result.Columns.Add("Hash", typeof(string));
            result.Columns.Add("DocumentNumber", typeof(string));
            result.Columns.Add("Order", typeof(int));
            foreach (var blank in from ?? Enumerable.Empty<DownloadedBlank>())
            {
                if (blank == null)
                {
                    continue;
                }
                result.Rows.Add(blank.RbdId, blank.Hash, blank.DocumentNumber, blank.Order);
            }
            return result;
        }
    }
}
