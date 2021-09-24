namespace Ege.Check.Dal.Cache.Interfaces
{
    using System;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;

    public class CacheLock<TCached> : IDisposable
        where TCached : class
    {
        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<CacheLock<TCached>>();

        public ICacheLockWrapper Lock { get; set; }

        public TCached Element { get; set; }

        [NotNull]
        public string Key { get; set; }

        [NotNull]
        public ICacheWrapper CacheWrapper { get; set; }

        public void Dispose()
        {
            if (Lock != null)
            {
                Logger.TraceFormat("UnlockPut {0} started (handle {1}, thread {2})", Key, Lock, System.Threading.Thread.CurrentThread.ManagedThreadId);
                CacheWrapper.PutAndUnlock(Key, Element, Lock);
                Logger.TraceFormat("UnlockPut {0} finished (handle {1}, thread {2})", Key, Lock, System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
