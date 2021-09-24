namespace GVUZ.Web.Infrastructure
{
    public static class CacheManager
    {
        public static ICacheProvider Current 
        {
            get
            { 
                return DefaultCacheProvider.Instance;
            }
        }
    }
}