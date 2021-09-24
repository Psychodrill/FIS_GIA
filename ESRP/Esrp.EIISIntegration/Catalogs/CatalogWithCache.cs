using System;
using System.Collections.Generic;

namespace Esrp.EIISIntegration.Catalogs
{
    public abstract class CatalogWithCache
    {
        protected bool cacheIds_;

        public CatalogWithCache(bool cacheIds)
        {
            cacheIds_ = cacheIds;
        }

        protected int? GetFromCache(string cacheKey, Dictionary<string, int> cache)
        {
             if (cacheIds_ == false)
                return null;
            if (String.IsNullOrEmpty(cacheKey))
                return null;
            if (cache.ContainsKey(cacheKey))
                return cache[cacheKey];
            return null;
        }

        protected void AddToCache(string cacheKey, int cacheEntry, Dictionary<string, int> cache)
        { 
            if (cacheIds_ == false)
                return;
            if (String.IsNullOrEmpty(cacheKey))
                return;
            if (!cache.ContainsKey(cacheKey))
            {
                cache.Add(cacheKey, cacheEntry);
            }
        } 
    }
}
