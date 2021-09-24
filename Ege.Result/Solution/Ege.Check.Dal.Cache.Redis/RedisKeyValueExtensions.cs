namespace Ege.Check.Dal.Cache.Redis
{
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Dal.Cache.Interfaces;
    using JetBrains.Annotations;
    using StackExchange.Redis;

    static class RedisKeyValueExtensions
    {
        public static RedisKey ToRedisKey(this string key)
        {
            return key;
        }

        public static RedisKey[] ToRedisKeys(this IEnumerable<string> keys)
        {
            var collection = keys as IReadOnlyCollection<string>;
            if (collection != null)
            {
                var count = collection.Count;
                var result = new RedisKey[count];
                using (var enumerator = collection.GetEnumerator())
                {
                    for (var i = 0; i < count; ++i)
                    {
                        enumerator.MoveNext();
                        result[i] = enumerator.Current;
                    }
                }
                return result;
            }
            return keys != null ? keys.Select(ToRedisKey).ToArray() : new RedisKey[0];
        }

        public static KeyValuePair<RedisKey, RedisValue> ToRedisKeyValuePair<T>(
            this KeyValuePair<string, T> keyValuePair,
            [NotNull] ICacheSerializer serializer)
        {
            return new KeyValuePair<RedisKey, RedisValue>(keyValuePair.Key, serializer.Serialize(keyValuePair.Value));
        }

        public static KeyValuePair<RedisKey, RedisValue>[] ToRedisKeyValuePairs<T>(
            this IEnumerable<KeyValuePair<string, T>> keyValuePairs,
            [NotNull]ICacheSerializer serializer)
        {
            return keyValuePairs != null
                ? keyValuePairs.Where(kv => kv.Value != null).Select(kv => ToRedisKeyValuePair(kv, serializer)).ToArray()
                : new KeyValuePair<RedisKey, RedisValue>[0];
        }
    }
}

