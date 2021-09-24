using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading;
using FogSoft.Helpers.Threading;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Provides common extensions for the <see cref="ResourceManager"/>
	/// to operates with enums (<see cref="GetResourceString{T}(T,string,System.Globalization.CultureInfo)"/>).</summary>
	[DebuggerStepThrough]
	public static class Resources
	{
		private static readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();
		private static readonly Dictionary<Type, Func<ResourceManager>> ManagerMap 
			= new Dictionary<Type, Func<ResourceManager>>();

		/// <summary>
		/// Resets map for <see cref="ResourceManager"/>s.</summary>
		public static void ResetManagerMap()
		{
			using (ReaderWriterLocker locker = new ReaderWriterLocker(CacheLock))
			{
				locker.EnterWriteLock();
				ManagerMap.Clear();
			}
		}

		/// <summary>
		/// Maps specified type to <see cref="ResourceManager"/>s</summary>
		public static void Map<T>(Func<ResourceManager> func)
		{
			// TODO: добавить возможность не только регистрировать, но и помечать enum'ы атрибутами вместо этого
			if (func == null) throw new ArgumentNullException("func");
			
			using (ReaderWriterLocker locker = new ReaderWriterLocker(CacheLock))
			{
				locker.EnterWriteLock();
				Type type = typeof (T);
				if (ManagerMap.ContainsKey(type))
					ManagerMap[type] = func;
				else
					ManagerMap.Add(type, func);
			}
		}

		/// <summary>
		/// Gets <see cref="ResourceManager"/>, previously specified by <see cref="Map{T}"/>.</summary>
		public static ResourceManager GetManager<T>()
		{
			Type type = typeof(T);

			using (ReaderWriterLocker locker = new ReaderWriterLocker(CacheLock))
			{
				locker.EnterReadLock();
				return ManagerMap[type]();
			}
		}

		public static Stream GetResourceStream<T>(string name, bool optional = false)
		{
			Stream stream = typeof(T).Assembly.GetManifestResourceStream(typeof(T), name);
			if (stream == null && !optional)
				throw new ArgumentException
					(string.Format("Resource '{0}.{1}' does not found.", typeof(T).Namespace, name));
			return stream;
		}

		/// <summary>
		/// Returns resource key for the specified <see cref="Enum"/> value (in "EnumName_ValueName" format).</summary>
		public static string GetResourceName<T>(T value) where T : struct 
		{
			Type type = typeof(T);
			if (!type.IsEnum)
				throw new ArgumentException("Only enums supported.");
			return type.Name + "_" + Enum.GetName(type, value);
		}

		/// <summary>
		/// Returns resource string for the specified <see cref="Enum"/> value
		/// (appropriate resource should has "EnumName_ValueName" name).</summary>
		public static string GetResourceString<T>(this T value, ResourceManager resourceManager,
			string defaultString = null, CultureInfo cultureInfo = null) where T : struct 
		{
			if (resourceManager == null) throw new ArgumentNullException("resourceManager");

			string result = resourceManager.GetString(GetResourceName(value), cultureInfo) ?? defaultString;
			if (result == null)
				throw new ArgumentOutOfRangeException("value");

			return result;
		}

		/// <summary>
		/// Returns resource string for the specified <see cref="Enum"/> value with <see cref="ResourceManager"/>,
		/// registered with <see cref="Map{T}"/>. Appropriate resource should has "EnumName_ValueName" name.</summary>
		public static string GetResourceString<T>(this T value, string defaultString = null, CultureInfo cultureInfo = null)
			where T : struct 
		{
			return GetResourceString(value, GetManager<T>(), defaultString);
		}
	}
}
