namespace Ege.Check.Logic.Services.Staff.Documents
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IDocumentUrlService
    {
        [NotNull]
        Task<DocumentUrlsCollection> GetAllDocuments();

        [NotNull]
        Task UpdateDocuments([NotNull] DocumentUrlsCollection documents);
    }
}