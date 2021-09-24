namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;

    internal class DocumentUrlsDataTableMapper : IDataTableMapper<IEnumerable<DocumentUrl>>
    {
        private const string IdColumn = "Id";
        private const string NameColumn = "Name";
        private const string UrlColumn = "Url";

        public DataTable Map(IEnumerable<DocumentUrl> @from)
        {
            var result = new DataTable();
            result.Columns.Add(IdColumn, typeof (int));
            result.Columns.Add(NameColumn, typeof (string));
            result.Columns.Add(UrlColumn, typeof (string));

            foreach (var document in from ?? Enumerable.Empty<DocumentUrl>())
            {
                if (document == null)
                {
                    continue;
                }
                result.Rows.Add(document.Id, document.Name, document.Url);
            }
            return result;
        }
    }
}