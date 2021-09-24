namespace Ege.Check.Dal.Store.Repositories.Documents
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IDocumentUrlRepository
    {
        Task<DocumentUrlsCollection> GetAll([NotNull] DbConnection connection);

        Task UpdateDocuments([NotNull] DbConnection connection, ICollection<DocumentUrl> documents);
    }
}