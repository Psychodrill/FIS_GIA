using System;
using System.Collections;
using System.Web.Caching;

namespace FogSoft.Helpers
{
	/// <summary>
	/// 	Interface for generic cache.
	/// </summary>
	public interface ICache : IEnumerable
	{
		/// <summary>
		/// 	Inserts object into the cache with dependencies, priority, absolute expiration in seconds
		/// 	and <see cref = "CacheItemRemovedCallback" />.
		/// </summary>
		void Insert(string key, object value, int absoluteExpirationInterval = 0,
		            CacheDependency dependencies = null, CacheItemPriority priority = CacheItemPriority.Normal,
		            CacheItemRemovedCallback callback = null);

		/// <summary>
		/// 	Removes object from the cache.
		/// </summary>
		void Remove(string key);

		/// <summary>
		/// 	Returns cached object or null, if not found.
		/// </summary>
		object Get(string key);

		/// <summary>
		/// 	Returns cached object or default value, if not found.
		/// </summary>
		T Get<T>(string key, T defaultValue);

		/// <summary>
		/// 	Returns cached object or inserts new object into cache based on parameters.
		/// </summary>
		T Get<T>(string key, Func<T> initialize, int absoluteExpirationInterval = 0);

		/// <summary>
		/// 	Removes all items with specified prefix.
		/// 	If prefix is not specified, removes all items.
		/// </summary>
		void RemoveAllWithPrefix(params string[] prefixes);

		/// <summary>
		/// 	Whether or not current <see cref = "ICache" /> implementation supports cache dependencies.
		/// </summary>
		bool SupportDependencies { get; }
	}
}
