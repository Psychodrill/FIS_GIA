namespace Ege.Check.Dal.Cache.LoadServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Logic.Models.Servers;
    using JetBrains.Annotations;

    public interface IBlankInfoCacheUpdater
    {
        [NotNull]
        Task UpdatePageCount([NotNull]ICacheWrapper wrapper, [NotNull]IReadOnlyCollection<UpdatedBlankInfo> updates);
    }
}