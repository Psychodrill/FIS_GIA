namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Dal.Common.Mappers;

    class PageCountDataMapper : IDataTableMapper<IEnumerable<PageCountData>>
    {
        private const string BarcodeColumn = "Barcode";
        private const string ProjectBatchIdColumn = "ProjectBatchId";
        private const string ProjectNameColumn = "ProjectName";
        private const string PageCountColumn = "PageCount";

        public DataTable Map(IEnumerable<PageCountData> @from)
        {
            var result = new DataTable();
            result.Columns.Add(BarcodeColumn, typeof(string));
            result.Columns.Add(ProjectBatchIdColumn, typeof(int));
            result.Columns.Add(ProjectNameColumn, typeof(string));
            result.Columns.Add(PageCountColumn, typeof(int));
            foreach (var pageCountData in from ?? Enumerable.Empty<PageCountData>())
            {
                if (pageCountData == null)
                {
                    continue;
                }
                result.Rows.Add(pageCountData.Barcode, pageCountData.ProjectBatchId, pageCountData.ProjectName, pageCountData.PageCount);
            }
            return result;
        }
    }
}
