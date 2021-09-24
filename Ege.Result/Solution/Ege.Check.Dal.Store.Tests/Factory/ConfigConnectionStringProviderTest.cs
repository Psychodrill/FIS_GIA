namespace Ege.Check.Dal.Store.Tests.Factory
{
    using Ege.Dal.Common.Factory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigConnectionStringProviderTest
    {
        [TestMethod]
        public void TestCheckEge()
        {
            var provider = new ConfigConnectionStringProvider("TestConnectionString", "TestConnectionHsc");
            var res = provider.CheckEge();
            Assert.AreEqual("92FC5CC6-4567-47A1-AA69-8376A9B87AD3", res);
        }
    }
}