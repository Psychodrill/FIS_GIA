using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;
using System.Linq;
using GVUZ.Model.Institutions;

namespace GVUZ.Model
{
	/// <summary>
	///  ласс дл€ получени€ названий из нередактируемых справочников по идентификаторам.</summary>
	public static partial class DictionaryCache
	{
        private static readonly ConcurrentDictionary<DictionaryTypeEnum, OrderedDictionary> Cache = new ConcurrentDictionary<DictionaryTypeEnum, OrderedDictionary>();
        private static readonly ConcurrentDictionary<DictionaryTypeEnum, Action<OrderedDictionary>> FillActions = new ConcurrentDictionary<DictionaryTypeEnum, Action<OrderedDictionary>>();

		public static DictionaryTypeEnum GetDictionaryByAdmissionLevel(AdmissionItemLevel level)
		{
		    return (DictionaryTypeEnum) Enum.Parse(typeof (DictionaryTypeEnum), Enum.GetName(typeof (AdmissionItemLevel), level));
		}

		public static string GetName(DictionaryTypeEnum dictionaryTypeEnum, int id)
		{
		    try
		    {
		        OrderedDictionary cachedList;

		        if (Cache.TryGetValue(dictionaryTypeEnum, out cachedList))
		        {
		            if (cachedList.Contains(id))
		                return (string) cachedList[(object) id];
		        }

		        if (cachedList == null)
                    Cache.TryAdd(dictionaryTypeEnum, cachedList = new OrderedDictionary());
		        else
		            cachedList.Clear();

		        FillActions[dictionaryTypeEnum](cachedList);

		        if (cachedList.Contains(id))
		            return (string) cachedList[(object) id];

		        throw new ArgumentOutOfRangeException("id");
		    }
		    catch (ArgumentOutOfRangeException)
		    {
		        return string.Format("Ќе найдено значение справочника '{0}' ({1})", 
                    dictionaryTypeEnum.GetEnumDescription(), id);
		    }
		}

        public static IEnumerable<KeyValuePair<int, string>> GetEntries(DictionaryTypeEnum dictionaryTypeEnum)
		{
			OrderedDictionary cachedList;
            if (!Cache.TryGetValue(dictionaryTypeEnum, out cachedList) || cachedList.Count == 0)
            {
                Cache.TryAdd(dictionaryTypeEnum, cachedList = new OrderedDictionary());
                FillActions[dictionaryTypeEnum](cachedList);
            }

            return from object key in cachedList.Keys select new KeyValuePair<int, string>( (key is int ? (int)key : (int)(short)key), (string)cachedList[key]);
		}
	}
}