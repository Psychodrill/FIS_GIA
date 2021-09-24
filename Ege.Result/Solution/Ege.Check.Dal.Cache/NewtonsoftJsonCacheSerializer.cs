namespace Ege.Check.Dal.Cache
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Newtonsoft.Json;

    public class NewtonsoftJsonCacheSerializer : ICacheSerializer
    {
        public string Serialize(object input)
        {
            return JsonConvert.SerializeObject(input);
        }

        public T Deserialize<T>(string input)
        {
            return input != null ? JsonConvert.DeserializeObject<T>(input) : default(T);
        }
    }
}
