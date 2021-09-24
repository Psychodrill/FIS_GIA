namespace Ege.Check.Dal.Store.Tests.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using JetBrains.Annotations;

    public class ClosureIterator
    {
        private readonly int _maxIterations;
        private int _currentIteration;

        public ClosureIterator(int maxIterations)
        {
            _maxIterations = maxIterations;
        }

        public int CurrentIteration
        {
            get { return _currentIteration; }
        }

        public int MaxIterations
        {
            get { return _maxIterations; }
        }

        public bool Next()
        {
            var res = _currentIteration < _maxIterations;
            _currentIteration++;
            return res;
        }

        public IEnumerable<ClosureIteration<T>> Enumerate<T>([NotNull] IEnumerable<T> enumerable)
            where T : class
        {
            var array = enumerable.ToArray();
            for (var i = 1; i <= array.Length; i++)
            {
                yield return new ClosureIteration<T>(array[i - 1], i);
            }
        }

        public static int ToInt(int iteration, int ordinalId)
        {
            return (iteration - 1) + ordinalId*10;
        }

        public static bool ToBool(int iteration, int ordinalId)
        {
            return (ToInt(iteration, ordinalId)%2) == 0;
        }

        public static DateTime ToDateTime(int iteration, int ordinalId)
        {
            return new DateTime(ToInt(iteration, ordinalId));
        }

        public static Guid ToGuid(int iteration, int ordinalId)
        {
            return new Guid(ToInt(iteration, ordinalId), 1, 2, 3, 4, 5, 6, 7, 8, 9, 0);
        }

        public static string ToString(int iteration, int ordinalId)
        {
            return ToInt(iteration, ordinalId).ToString(CultureInfo.InvariantCulture);
        }

        public class ClosureIteration<T>
        {
            public ClosureIteration(T value, int iterationNumber)
            {
                Value = value;
                IterationNumber = iterationNumber;
            }

            public T Value { get; private set; }

            public int IterationNumber { get; set; }

            public int IntValue(int ordinal)
            {
                return ToInt(IterationNumber, ordinal);
            }

            public DateTime DateTimeValue(int ordinal)
            {
                return ToDateTime(IterationNumber, ordinal);
            }

            public bool BoolValue(int ordinal)
            {
                return ToBool(IterationNumber, ordinal);
            }

            public Guid GuidValue(int ordinal)
            {
                return ToGuid(IterationNumber, ordinal);
            }

            public string StringValue(int ordinal)
            {
                return ClosureIterator.ToString(IterationNumber, ordinal);
            }
        }
    }
}