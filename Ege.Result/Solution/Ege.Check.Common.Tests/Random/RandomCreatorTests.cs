namespace Ege.Check.Common.Tests.Random
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Common.Random;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RandomCreatorTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var randomCreator = new RandomCreator();
            var random = new Random();
            const int randomCount = 100;
            const int sequenceLength = 100000;
            var randoms = new Random[randomCount];
            var sequences = new int[randomCount,sequenceLength];
            Parallel.ForEach(Enumerable.Range(0, randomCount), i =>
                {
                    randoms[i] = randomCreator.Create();
                    foreach (var j in Enumerable.Range(0, sequenceLength))
                    {
                        sequences[i, j] = randoms[i].Next();
                    }
                });
            Func<int, IEnumerable<int>> sequenceExtractor =
                i => Enumerable.Range(0, sequenceLength).Select(k => sequences[i, k]);
            // we check that different generators generate different sequences
            foreach (var i in Enumerable.Range(0, randomCount))
                foreach (var j in Enumerable.Range(0, randomCount).Except(i.ToEnumerable()))
                {
                    Assert.IsFalse(sequenceExtractor(i).SequenceEqual(sequenceExtractor(j)));
                }
            // System.Random may start generating a sequence of zeroes if it is accessed concurrently from several threads
            // it almost certainly happens after 1m concurrent requests
            // we check it does not happen
            foreach (var i in Enumerable.Range(0, randomCount))
            {
                Assert.IsFalse(sequenceExtractor(i).Skip(sequenceLength - 5).All(s => s == 0));
            }
        }
    }
}