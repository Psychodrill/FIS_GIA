namespace Ege.Check.Dal.Cache.LoadServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;

    internal class NopCacheWriter : ICacheWriter<object>
    {
        public Task Write(ICacheWrapper wrapper, IReadOnlyCollection<object> elements)
        {
            return Task.FromResult(0);
        }
    }
}