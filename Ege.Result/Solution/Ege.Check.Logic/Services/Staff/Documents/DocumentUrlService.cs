namespace Ege.Check.Logic.Services.Staff.Documents
{
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Repositories.Documents;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class DocumentUrlService : IDocumentUrlService
    {
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IDocumentUrlRepository _documentRepository;

        public DocumentUrlService([NotNull] IDocumentUrlRepository documentRepository,
                                  [NotNull] IDbConnectionFactory connectionFactory)
        {
            _documentRepository = documentRepository;
            _connectionFactory = connectionFactory;
        }

        public async Task<DocumentUrlsCollection> GetAllDocuments()
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _documentRepository.GetAll(connection);
            }
        }

        public async Task UpdateDocuments(DocumentUrlsCollection documents)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _documentRepository.UpdateDocuments(connection, documents.Documents);
            }
        }
    }
}