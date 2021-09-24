namespace FbsUnitTestCommon
{
    using Fbs.Utility;

    using NUnit.Framework;

    /// <summary>
    ///This is a test class for RussianToLatinTest and is intended
    ///to contain all RussianToLatinTest Unit Tests
    ///</summary>
    [TestFixture]
    public class RussianToLatinTest
    {
        /// <summary>
        ///A test for Encode
        ///</summary>
        [Test]
        public void EncodeTest()
        {
            string value = "Миша ходит в школу";
            bool preserveCase = true;
            string expected = "Misha khodit v shkolu";
            string actual;
            actual = RussianToLatin.Encode(value, preserveCase);
            Assert.AreEqual(expected, actual);
        }
    }
}