#if DEBUG
using System.Diagnostics;
using System.Web.Caching;

namespace GVUZ.Web.Infrastructure
{
    public static class DebugHelper
    {
        private const string CacheHitCategory = "CACHE HIT";
        private const string CacheMissCategory = "CACHE MISS";
        private const string CacheRemoveCategory = "CACHE REMOVE";

        public static void CacheHit(string key)
        {  
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(key, CacheHitCategory);
            }
        }

        public static void CacheMiss(string key)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(key, CacheMissCategory);
            }
        }

        public static void CacheRemove(string key, object value, CacheItemRemovedReason reason)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(string.Format("{0}, reason: {1}", key, reason.ToString("G")), CacheRemoveCategory);
            }
        }
    }
}
#endif