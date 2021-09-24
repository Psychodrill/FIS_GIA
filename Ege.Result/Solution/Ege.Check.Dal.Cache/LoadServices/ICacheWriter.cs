namespace Ege.Check.Dal.Cache.LoadServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using JetBrains.Annotations;

    public interface ICacheWriter<in TDto>
    {
        [NotNull]
        Task Write([NotNull]ICacheWrapper wrapper, [NotNull] IReadOnlyCollection<TDto> elements);
    }
}