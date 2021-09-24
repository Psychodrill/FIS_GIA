namespace Ege.Check.Dal.Store.Repositories.Participants
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IParticipantRepository
    {
        [NotNull]
        Task<ParticipantCollectionCacheModel> GetByHash([NotNull] DbConnection connection, string hash);

        [NotNull]
        Task<string> GetCodeByRbdId([NotNull] DbConnection connection, Guid rbdId);
    }
}