namespace Ege.Check.Dal.Cache
{
    using Ege.Check.Dal.Cache.Interfaces;
    using ServiceStack.Text;

    public class ServiceStackCacheSerializer : ICacheSerializer
    {
        public string Serialize(object input)
        {
            return JsonSerializer.SerializeToString(input);
        }

        public T Deserialize<T>(string input)
        {
            return JsonSerializer.DeserializeFromString<T>(input);
        }
    }
}
