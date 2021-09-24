namespace Ege.Check.Common.Tests.Hash
{
    using Ege.Check.Common.Hash;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Diagnostics;

    [TestClass]
    public class FioHasherTests
    {
        [NotNull] private FioHasher _hasher;

        [TestInitialize]
        public void Init()
        {
            _hasher = new FioHasher();
        }

        [TestMethod]
        public void TestCorrectNames()
        {
            Assert.AreEqual("805f6a91f58481c048d801782b24b355", _hasher.GetHash("Иванов", "Иван", "Иванович"));
            Assert.AreEqual("e73adde1ff6dd11fe1ad5f544217c103", _hasher.GetHash("Иванов", "Иван", null));
            Assert.AreEqual("25bbb6d1bcba6237be79310656cf7e30", _hasher.GetHash("Петров", "Петр", "Петрович"));
            Assert.AreEqual("2105fc3228489878cb8cb84b3e6f81fe", _hasher.GetHash("Дмитриев", "Дмитрии", "Дмитриевич"));
            Assert.AreEqual("0cef618b6c03dd3173b8c78597dc67c4", _hasher.GetHash("Алексеев", "Алексеи", "Алексеевич"));
            Assert.AreEqual("4220ae7d6bd402f2bc5510210c7b3f23", _hasher.GetHash("Сергеев", "Сергеи", "Сергеевич"));
        }

        [TestMethod]
        public void TestNamesWithUndesirableLetterCharacters()
        {
            Assert.AreEqual("805f6a91f58481c048d801782b24b355", _hasher.GetHash("Йванов", "Йван", "Йванович"));
            Assert.AreEqual("e73adde1ff6dd11fe1ad5f544217c103", _hasher.GetHash("Йванов", "Иван", null));
            Assert.AreEqual("25bbb6d1bcba6237be79310656cf7e30", _hasher.GetHash("Петров", "Пётр", "Петровйч"));
            Assert.AreEqual("2105fc3228489878cb8cb84b3e6f81fe", _hasher.GetHash("Дмитриев", "Дмитрий", "Дмитриевич"));
            Assert.AreEqual("0cef618b6c03dd3173b8c78597dc67c4", _hasher.GetHash("Алексеев", "Алексей", "Алексеевич"));
            Assert.AreEqual("4220ae7d6bd402f2bc5510210c7b3f23", _hasher.GetHash("Сергеев", "Сергей", "Сергеевич"));
        }

        [TestMethod]
        public void TestUntrimmedNames()
        {
            Assert.AreEqual("805f6a91f58481c048d801782b24b355", _hasher.GetHash("  Йванов  \t", "\tЙван  ", " Йванович"));
            Assert.AreEqual("e73adde1ff6dd11fe1ad5f544217c103", _hasher.GetHash("  Йванов  \t", "\tЙван  ", "  "));
            Assert.AreEqual("25bbb6d1bcba6237be79310656cf7e30", _hasher.GetHash("Петров", " Пётр", "Петрович  "));
            Assert.AreEqual("2105fc3228489878cb8cb84b3e6f81fe",
                            _hasher.GetHash("Дмитриев\t", "\tДмитрий", " Дмитриевич "));
            Assert.AreEqual("0cef618b6c03dd3173b8c78597dc67c4",
                            _hasher.GetHash("\t  Алексеев", " Алексей  \t", "\t   Алексеевич\t   "));
            Assert.AreEqual("4220ae7d6bd402f2bc5510210c7b3f23",
                            _hasher.GetHash("   \tСергеев \t", "\t   Сергей   \t", "   \tСергеевич\t  "));
        }
    }
}