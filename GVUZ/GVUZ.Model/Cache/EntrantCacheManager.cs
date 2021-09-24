using System;
using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;

namespace GVUZ.Model.Cache
{
    public class EntrantCacheManager
    {
        static Dictionary<Type, Dictionary<int, object>> Cache = new Dictionary<Type, Dictionary<int, object>>();

        EntrantCacheManager()
        {
            
        }

        static object locker = new object();
        public static void Add<T>(int _key, T item)
        {
            lock (locker)
            {
                try
                {
                    if (!Cache.ContainsKey(typeof(T)))
                    {
                        Cache.Add(typeof(T), new Dictionary<int, object> {{_key, item}});  
                    }
                    else
                    {
                        var cache = Cache[typeof (T)];
                        if (cache.ContainsKey(_key))
                        {
                            cache[_key] = item;
                        }
                        else
                        {
                            cache.Add(_key, item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log.ErrorFormat("Ошибка в EntrantCacheManager: {0}", ex.Message);
                }
            }
        }

        public static void Remove<T>(int entityId)
        {
            lock (locker)
            {
                try
                {
                    if (!Cache.ContainsKey(typeof (T)))
                        return;

                    var cache = Cache[typeof(T)];
                    if (cache.ContainsKey(entityId))
                        cache.Remove(entityId);
                }
                catch (Exception ex)
                {
                    LogHelper.Log.ErrorFormat("Ошибка в EntrantCacheManager: {0}", ex.Message);
                }
            }
        }

        public static T Get<T>(int _key) where T: class
        {
                try
                {
                    if (!Cache.ContainsKey(typeof(T)))
                    {
                        return null;
                    }
                    else
                    {
                        var cache = Cache[typeof(T)];
                        if (cache.ContainsKey(_key))
                        {
                            return cache[_key] as T;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log.ErrorFormat("Ошибка в EntrantCacheManager: {0}", ex.Message);
                }
            return null;
        }

        public static IEnumerable<T> GetAll<T>(Predicate<T> predicate) where T : class
        {
            try
            {
                if (!Cache.ContainsKey(typeof(T)))
                {
                    return null;
                }
                else
                {
                    var cache = Cache[typeof(T)];

                    return cache.Values.Cast<T>().Where(x => predicate(x));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.ErrorFormat("Ошибка в EntrantCacheManager: {0}", ex.Message);
            }
            return null;
        }

        public static T GetFirst<T>(Predicate<T> predicate) where T : class
        {
            try
           {
                if (!Cache.ContainsKey(typeof(T)))
                {
                    return null;
                }
                else
                {
                    var cache = Cache[typeof(T)];

                    return cache.Values.Cast<T>().FirstOrDefault(x=>predicate(x));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.ErrorFormat("Ошибка в EntrantCacheManager: {0}", ex.Message);
            }
            return null;
        }
    }
}