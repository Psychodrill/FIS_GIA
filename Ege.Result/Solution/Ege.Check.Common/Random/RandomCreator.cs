namespace Ege.Check.Common.Random
{
    using System;
    using JetBrains.Annotations;

    internal class RandomCreator : IRandomCreator
    {
        [NotNull] private static readonly Random SeedGenerator = new Random();

        [NotNull] private static readonly object LockObj = new object();

        public Random Create()
        {
            lock (LockObj)
            {
                return new Random(SeedGenerator.Next());
            }
        }
    }
}