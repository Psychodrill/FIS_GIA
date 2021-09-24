using System;

namespace GVUZ.Web.Infrastructure
{
    public interface ICacheProvider
    {
        void Remove(string key);
        object GetOrCreate(string key, Func<object> addValueFactory);
        object GetOrAdd(string key, object value);
        void Add(string key, object value);
        T GetOrCreateItem<T>(string key, Func<T> addValueFactory);
        T GetOrAddItem<T>(string key, T value);
    }
}