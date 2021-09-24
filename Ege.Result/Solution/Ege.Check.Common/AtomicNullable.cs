namespace Ege.Check.Common
{
    public class AtomicNullable<T> where T : struct
    {
        public readonly T Value;

        public AtomicNullable(T value)
        {
            Value = value;
        }

        public static implicit operator AtomicNullable<T>(T value)
        {
            return new AtomicNullable<T>(value);
        }
    }
}