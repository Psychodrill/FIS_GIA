namespace Ege.Check.Dal.Store.Repositories.PagesCount
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Bulk.Load;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    class PagesCountRepository : Repository, IPagesCountRepository
    {
        [NotNull]
        private readonly IBulkLoader _bulkLoader;

        public PagesCountRepository([NotNull]IBulkLoader bulkLoader)
        {
            _bulkLoader = bulkLoader;
        }

        public async Task Merge(
            SqlConnection connection,
            IEnumerable<KeyValuePair<KeyValuePair<int, ExamMemoryCacheModel>, ICollection<PageCountData>>> received)
        {
            var dt = new DataTable();
            dt.Columns.Add("RegionId", typeof(int));
            dt.Columns.Add("ExamGlobalId", typeof(int));
            dt.Columns.Add("ExamDate", typeof(DateTime));
            dt.Columns.Add("Barcode", typeof(string));
            dt.Columns.Add("ProjectBatchId", typeof(int));
            dt.Columns.Add("ProjectName", typeof(string));
            dt.Columns.Add("PageCount", typeof(int));
            var truncate = connection.CreateCommand();
            truncate.CommandText = "truncate table PagesCountBulk";
            await truncate.ExecuteNonQueryAsync();
            foreach (var regionExam in received)
            {
                foreach (var pageCount in regionExam.Value ?? Enumerable.Empty<PageCountData>())
                {
                    dt.Rows.Add(
                        regionExam.Key.Key, 
                        regionExam.Key.Value.Id, 
                        regionExam.Key.Value.Date,
                        
                        pageCount.Barcode, 
                        pageCount.ProjectBatchId, 
                        pageCount.ProjectName, 
                        pageCount.PageCount);
                }
            }
            await _bulkLoader.LoadDataAsync(dt, "PagesCountBulk", connection);
            var cmd = StoredProcedureCommand(connection, "MergePagesCount");
            await cmd.ExecuteNonQueryAsync();
        }
    }
}