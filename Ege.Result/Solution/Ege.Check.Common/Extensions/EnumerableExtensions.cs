namespace Ege.Check.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    public static class EnumerableExtensions
    {
        [NotNull]
        public static T[] ToArrayIfNeeded<T>(this IEnumerable<T> enumerable)
        {
            return enumerable as T[] ?? (enumerable != null ? enumerable.ToArray() : new T[0]);
        }

        [NotNull]
        public static List<T> ToListIfNeeded<T>(this IEnumerable<T> enumerable)
        {
            return enumerable as List<T> ?? (enumerable != null ? enumerable.ToList() : new List<T>());
        }

        [NotNull]
        public static IEnumerable<T[]> ArrayBatch<T>(
                [NotNull]this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                {
                    bucket = new T[size];
                }

                bucket[count++] = item;

                if (count != size)
                {
                    continue;
                }

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
            {
                yield return bucket.Take(count).ToArray();
            }
        }
    }
}
