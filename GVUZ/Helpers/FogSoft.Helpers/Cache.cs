using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using log4net;

namespace FogSoft.Helpers
{
	/// <summary>
	/// 	<see cref = "ICache" /> implementation, based on <see cref = "System.Web.Caching.Cache" />.
	/// </summary>
	//[System.Diagnostics.DebuggerStepThrough]
	public class Cache : ICache
	{
		private static readonly ILog Log =
			LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public const int DefaultExpirationInSeconds = 600;

		public void Insert(string key, object value, int absoluteExpirationInterval = 0, CacheDependency dependencies = null,
		                   CacheItemPriority priority = CacheItemPriority.Normal, CacheItemRemovedCallback callback = null)
		{
			DateTime absoluteExpiration =
				DateTime.Now.AddSeconds(absoluteExpirationInterval == 0 ? DefaultExpirationInSeconds : absoluteExpirationInterval);
			absoluteExpiration = DiversifyExpirationTime(absoluteExpiration);

			bool isDebugEnabled = Log.IsDebugEnabled;
			if (isDebugEnabled)
			{
				if (callback == null)
					callback = RemovedCallback;
				else
					callback += RemovedCallback;
			}

			HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration,
			                         System.Web.Caching.Cache.NoSlidingExpiration, priority, callback);

			if (isDebugEnabled)
			{
				Log.DebugFormat("Insert key: {0}, item: {1}, absoluteExpiration: {2}, priority: {3}",
				                key, value.GetType(), absoluteExpiration, priority);
			}
		}

		public void Remove(string key)
		{
			HttpRuntime.Cache.Remove(key);
		}

		public object Get(string key)
		{
			return HttpRuntime.Cache.Get(key);
		}

		public T Get<T>(string key, T defaultValue)
		{
			object value = Get(key);
			return value != null ? (T) value : defaultValue;
		}

		public T Get<T>(string key, Func<T> initialize, int absoluteExpirationInterval = 0)
		{
			if (initialize == null) throw new ArgumentNullException("initialize");
			object value = Get(key);
			if (value != null)
				return (T) value;
			T newValue = initialize();
			Insert(key, newValue, absoluteExpirationInterval);
			return newValue;
		}

		private static void RemovedCallback(string key, object value,
		                                    CacheItemRemovedReason callbackreason)
		{
			// Log.IsDebugEnabled checked before (see Insert method)
			Log.DebugFormat("Remove key: {0}, object: {1}, reason: {2}", key, value.GetType(), callbackreason);
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return HttpRuntime.Cache.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool SupportDependencies
		{
			get { return true; }
		}

		public void RemoveAllWithPrefix(params string[] prefixes)
		{
			bool noPrefix = prefixes.IsNullOrEmpty() || string.IsNullOrEmpty(prefixes[0]);
			List<string> keys = new List<string>();
			foreach (DictionaryEntry entry in this)
				keys.Add((string) entry.Key);

			// splitted to avoid huge loop comparison in three parts

			if (noPrefix)
			{
				for (int keyIndex = keys.Count - 1; keyIndex >= 0; keyIndex--)
					Remove(keys[keyIndex]);
				return;
			}

			if (prefixes.Length == 0)
			{
				string prefix = prefixes[0];
				for (int keyIndex = keys.Count - 1; keyIndex >= 0; keyIndex--)
				{
					string key = keys[keyIndex];
					if (key.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
						Remove(key);
				}
				return;
			}

			for (int keyIndex = keys.Count - 1; keyIndex >= 0; keyIndex--)
			{
				string key = keys[keyIndex];

				for (int prefixIndex = prefixes.Length - 1; prefixIndex >= 0; prefixIndex--)
				{
					if (key.StartsWith(prefixes[prefixIndex], StringComparison.CurrentCultureIgnoreCase))
						Remove(key);
				}
			}
		}

		private static DateTime DiversifyExpirationTime(DateTime absoluteExpiration)
		{
			TimeSpan span = absoluteExpiration - DateTime.Now;
			int minutes = (int) span.TotalMinutes;
			if (minutes > 0)
			{
				int delta = (int) Math.Log(minutes, 2);
				Random rnd = new Random((int) DateTime.Now.Ticks);
				int addMinutes = rnd.Next(-delta, delta);
				int addSeconds = rnd.Next(-30, 30);
				DateTime newAbsoluteExpiration = absoluteExpiration
					.AddMinutes(addMinutes).AddSeconds(addSeconds);
				return newAbsoluteExpiration;
			}
			return absoluteExpiration;
		}
	}
}
