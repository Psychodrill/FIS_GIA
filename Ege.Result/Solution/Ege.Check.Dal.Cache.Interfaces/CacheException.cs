namespace Ege.Check.Dal.Cache.Interfaces
{
    using System;

    public class CacheException : Exception
    {
        public CacheException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
