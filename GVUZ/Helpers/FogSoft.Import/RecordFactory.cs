using System;
using System.Collections.Generic;
using System.Threading;
using FogSoft.Helpers.Threading;

namespace FogSoft.Import
{
	public static class RecordFactory
	{
		private static readonly ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();
		private static readonly Dictionary<Type, RecordStructure> Map = new Dictionary<Type, RecordStructure>();

		public static T Create<T>(string[] sourceValues) where T : Record, new()
		{
			RecordStructure structure = GetStructure(typeof (T));

			T result = new T();
			result.Initialize(structure, sourceValues);
			return result;
		}

		internal static RecordStructure GetStructure(Type type)
		{
			RecordStructure structure;

			using (ReaderWriterLocker locker = new ReaderWriterLocker(CacheLock))
			{
				locker.EnterUpgradeableReadLock();

				if (!Map.TryGetValue(type, out structure))
				{

					using (ReaderWriterLocker writeLocker = new ReaderWriterLocker(CacheLock))
					{
						writeLocker.EnterWriteLock();

						if (Map.TryGetValue(type, out structure))
							return structure;
						structure = new RecordStructure(type);
						Map.Add(type, structure);
						return structure;
					}
				}
			}
			return structure;
		}
	}
}
