namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;

    internal class DocumentUrlsCollectionMapper : DataReaderMapper<DocumentUrlsCollection>
    {
        private const string Id = "Id";
        private const string Name = "Name";
        private const string Url = "Url";

        public override async Task<DocumentUrlsCollection> Map(DbDataReader @from)
        {
            var id = GetOrdinal(from, Id);
            var name = GetOrdinal(from, Name);
            var url = GetOrdinal(from, Url);

            var documents = new List<DocumentUrl>();

            while (await from.ReadAsync())
            {
                documents.Add(new DocumentUrl
                    {
                        Id = from.GetInt32(id),
                        Name = from.GetString(name),
                        Url = from.GetString(url),
                    });
            }
            var result = new DocumentUrlsCollection
                {
                    Documents = documents,
                };
            return result;
        }
    }
}