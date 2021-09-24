namespace Ege.Check.Logic.Tests.BlankServers
{
    using System;
    using Ege.Check.Logic.BlankServers;
    using Ege.Check.Logic.Models.Servers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PageCountDataParserTests
    {
        [TestMethod]
        public void TestParse()
        {
            var parser = new PageCountDataParser();
            PageCountData result;

            result = parser.Parse("2489065204376_11_04final_checker:2");
            Assert.IsNotNull(result);
            Assert.AreEqual("2489065204376", result.Barcode);
            Assert.AreEqual(11, result.ProjectBatchId);
            Assert.AreEqual("04final_checker", result.ProjectName);
            Assert.AreEqual(2, result.PageCount);

            result = parser.Parse("2485564399810_14_04final_checker_shmeker:1");
            Assert.IsNotNull(result);
            Assert.AreEqual("2485564399810", result.Barcode);
            Assert.AreEqual(14, result.ProjectBatchId);
            Assert.AreEqual("04final_checker_shmeker", result.ProjectName);
            Assert.AreEqual(1, result.PageCount);

            try
            {
                parser.Parse("123_123:123");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            try
            {
                parser.Parse("123_123_123");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            try
            {
                parser.Parse("123_12345678901234567890123456_123:1");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            try
            {
                parser.Parse("123_123_123:12345678901234567890123456");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
