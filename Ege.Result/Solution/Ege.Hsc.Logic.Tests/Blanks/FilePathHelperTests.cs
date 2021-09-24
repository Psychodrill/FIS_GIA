namespace Ege.Hsc.Logic.Blanks.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass()]
    public class FilePathHelperTests
    {

        [TestMethod()]
        public void TryParsePathTest()
        {
            var mock = new Mock<IBlankApplicationSettings>(MockBehavior.Strict);
            var helper = new FilePathHelper(mock.Object);

            var res = helper.TryParsePath(@"C:\tmp\ege\blankstorage\80\805f\805f6a91f58481c048d801782b24b355\0000111111\D3BF4B1D-E957-436E-AB26-0231FB1794E2\1");
            Assert.IsNotNull(res);
            Assert.AreEqual("805f6a91f58481c048d801782b24b355", res.Hash);
            Assert.AreEqual("0000111111", res.DocumentNumber);
            Assert.AreEqual(Guid.Parse("D3BF4B1D-E957-436E-AB26-0231FB1794E2"), res.RbdId);
            Assert.AreEqual(1, res.Order);

            res = helper.TryParsePath(@"C:\tmp\ege\blankstorage\80\805f\805f6a91f58481c048d801782b24b355\0000111111\D3BF4B1D\1");
            Assert.IsNull(res);

            res = helper.TryParsePath(@"C:\tmp\ege\blankstorage\80\805f\805f6a91f58481c048d801782b24b355\0000111111\D3BF4B1D-E957-436E-AB26-0231FB1794E2\1111111111111111111111111");
            Assert.IsNull(res);

            res = helper.TryParsePath(@"C:\tmp\ege\blankstorage\80\805f\");
            Assert.IsNull(res);

        }
    }
}
