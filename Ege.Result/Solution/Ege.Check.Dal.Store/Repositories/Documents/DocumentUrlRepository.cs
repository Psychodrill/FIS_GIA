namespace Ege.Check.Dal.Store.Repositories.Documents
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class DocumentUrlRepository : Repository, IDocumentUrlRepository
    {
        private const string GetProcedureName = "GetDocumentUrls";
        private const string SetProcedureName = "MergeDocumentsUrls";
        private const string DocumentsParameterName = "@Documents";

        [NotNull] private readonly IDataTableMapper<IEnumerable<DocumentUrl>> _documentsTableMapper;
        [NotNull] private readonly IDataReaderMapper<DocumentUrlsCollection> _mapper;

        public DocumentUrlRepository(
            [NotNull] IDataReaderMapper<DocumentUrlsCollection> mapper,
            [NotNull] IDataTableMapper<IEnumerable<DocumentUrl>> documentsTableMapper)
        {
            _mapper = mapper;
            _documentsTableMapper = documentsTableMapper;
        }

        public async Task<DocumentUrlsCollection> GetAll(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, GetProcedureName);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task UpdateDocuments(DbConnection connection, ICollection<DocumentUrl> documents)
        {
            var command = StoredProcedureCommand(connection, SetProcedureName);
            var documentsTable = _documentsTableMapper.Map(documents);
            AddParameter(command, DocumentsParameterName, documentsTable);
            await command.ExecuteNonQueryAsync();
        }
    }
}