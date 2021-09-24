using System;
using System.Collections.Generic;
using Esrp.DB;
using Esrp.Integration.Common;

namespace Esrp.EIISIntegration.Catalogs
{
    public class GenericCatalog<T> : CatalogWithCache where T : IEntityWithNaturalKey
    {
        #region Constructors and properties
        private Dictionary<string, int> cacheByNaturalKey_;

        private bool ignoreCasing_;
        private bool ignoreSingleLetters_;
        private string[] ignoreWords_;
        public GenericCatalog(IEnumerable<T> allEntries,  bool ignoreCasing, bool ignoreSingleLetters,string[] ignoreWords)
            : base(true)
        {
            ignoreCasing_ = ignoreCasing;
            ignoreSingleLetters_ = ignoreSingleLetters;
            ignoreWords_ = ignoreWords;

            cacheByNaturalKey_ = new Dictionary<string, int>();

            foreach (T cacheEntry in allEntries)
            {
                if (!String.IsNullOrEmpty(cacheEntry.NaturalKey))
                {
                    AddToCache(StringsHelper.Normalize(cacheEntry.NaturalKey, ignoreCasing_, ignoreSingleLetters_, ignoreWords_), cacheEntry.Id, cacheByNaturalKey_);
                }
            }
        }
        #endregion

        #region Get by natural keys
        public int? GetIdByNaturalKey(string naturalKey)
        {
            if (String.IsNullOrEmpty(naturalKey))
                return null;

            return GetFromCache(StringsHelper.Normalize(naturalKey, ignoreCasing_, ignoreSingleLetters_, ignoreWords_), cacheByNaturalKey_);
        } 
        #endregion
    }
}
