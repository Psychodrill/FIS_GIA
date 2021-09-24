using System;
using System.Web;
using System.Web.Caching;

namespace GVUZ.Web.Infrastructure
{
    public class DefaultCacheProvider : ICacheProvider
    {
        public static readonly DefaultCacheProvider Instance = new DefaultCacheProvider();

        private DefaultCacheProvider()
        {
        }
        
        private Cache Items
        {
            get { return HttpContext.Current.Cache; }
        }
        
        public void Remove(string key)
        {
            Items.Remove(key);
        }

        public object GetOrCreate(string key, Func<object> addValueFactory)
        {
            object o = Items[key];
            if (o == null)
            {
#if DEBUG
                DebugHelper.CacheMiss(key);
#endif
                o = addValueFactory();
                Add(key, o);
            }
#if DEBUG
            else
            {
                DebugHelper.CacheHit(key);
            }
#endif
            return o;
        }

        public T GetOrCreateItem<T>(string key, Func<T> addValueFactory)
        {
            object o = Items[key];
            if (o == null)
            {
#if DEBUG
                DebugHelper.CacheMiss(key);
#endif
                o = addValueFactory();
                Add(key, o);
            }
#if DEBUG
            else
            {
                DebugHelper.CacheHit(key);
            }
#endif
            return (T)o;
        }

        public object GetOrAdd(string key, object value)
        {
            object o = Items[key];

            if (o != null)
            {
#if DEBUG
                DebugHelper.CacheHit(key);
#endif
                return o;
            }

#if DEBUG
            DebugHelper.CacheMiss(key);
#endif
            Add(key, value);
            return value;
        }

        public T GetOrAddItem<T>(string key, T value)
        {
            object o = Items[key];

            if (o != null)
            {
#if DEBUG
                DebugHelper.CacheHit(key);
#endif
                return (T)o;
            }

#if DEBUG
            DebugHelper.CacheMiss(key);
#endif
            Add(key, value);
            return value;
        }


        public void Add(string key, object value)
        {
            if (value == null)
            {
                return;
            }
#if DEBUG
            Items.Add(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), CacheItemPriority.Low, DebugHelper.CacheRemove);
#else
            Items.Add(key, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), CacheItemPriority.Low, null);
#endif
        }
    }
}