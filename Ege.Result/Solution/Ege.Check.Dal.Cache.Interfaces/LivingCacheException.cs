namespace Ege.Check.Dal.Cache.Interfaces
{
    using System;

    public class LivingCacheException : CacheException
    {
        public LivingCacheException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}