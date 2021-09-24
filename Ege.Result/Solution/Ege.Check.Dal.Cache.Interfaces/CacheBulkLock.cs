namespace Ege.Check.Dal.Cache.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using global::Common.Logging;
    using JetBrains.Annotations;

    public class CacheBulkLock<TKey, TCached> : IDisposable
        where TCached: class
    {
        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<CacheBulkLock<TKey, TCached>>();

        public ICacheLockWrapper Lock { get; set; }

        [NotNull]
        public IReadOnlyList<CacheBulkElement<TKey, TCached>> Elements { get; set; }

        [NotNull]
        public string LockKey { get; set; }

        [NotNull]
        public ICacheWrapper CacheWrapper { get; set; }

        public void Dispose()
        {
            if (Lock != null)
            {
                Logger.TraceFormat("UnlockPut {0} started (handle {1}, thread {2})", typeof(TCached), Lock, System.Threading.Thread.CurrentThread.ManagedThreadId);
                CacheWrapper.BulkPut(Elements.Select(e => new KeyValuePair<string, TCached>(e.KeyString, e.Value)));
                CacheWrapper.PutAndUnlock(LockKey, null, Lock);
                Logger.TraceFormat("UnlockPut {0} finished (handle {1}, thread {2})", typeof(TCached), Lock, System.Threading.Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
