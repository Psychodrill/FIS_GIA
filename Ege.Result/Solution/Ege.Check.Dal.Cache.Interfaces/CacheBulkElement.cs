namespace Ege.Check.Dal.Cache.Interfaces
{
    public class CacheBulkElement<TKey, TCached>
    {
        public TKey Key { get; set; }

        public TCached Value { get; set; }

        public string KeyString { get; set; }
    }
}
