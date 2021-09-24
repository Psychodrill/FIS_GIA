namespace FbsUnitTestCommon
{
    using System.Text;
    using System.Text.RegularExpressions;

    using Fbs.Core.Shared;

    using NUnit.Framework;

    /// <summary>
    /// The string unit test.
    /// </summary>
    [TestFixture]
    public class StringUnitTest
    {
        #region Public Methods

        /// <summary>
        /// The test name normalization.
        /// </summary>
        [Test]
        public void TestNameNormalization()
        {
            var lastname = new[]
                {
                    "    Абдимуса     Кызы	", "    Абдуллина - Билялетдинов    ", "    Абу Аркуб   ", 
                    "     Абу    Сеаф   "
                };
            var lastnameExpected = new[] { "Абдимуса Кызы", "Абдуллина-Билялетдинов", "Абу Аркуб", "Абу Сеаф" };
            for (int i = 0; i < lastname.Length; i++)
            {
                Assert.AreEqual(lastnameExpected[i], lastname[i].FullTrim());
            }
        }

        [Test]
        public void ASDF()
        {
            //string asdf = "БудниковаВикторияАлександровна%250%74%50%%%48%79%%%%%%";
            string asdf = "БудниковаВикторияАлександровна%250";
            MatchCollection matches = Regex.Matches(asdf, @".+%\d+");
            var sb = new StringBuilder();
            foreach (Match match in matches)
            {
                string a = match.Value;
            }
        }

        #endregion
    }
}