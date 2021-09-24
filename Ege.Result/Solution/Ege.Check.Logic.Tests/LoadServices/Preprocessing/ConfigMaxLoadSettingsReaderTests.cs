namespace Ege.Check.Logic.LoadServices
{
    using Ege.Check.Logic.LoadServices.Preprocessing;
    using Ege.Check.Logic.Models.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigMaxLoadSettingsReaderTests
    {
        [TestMethod]
        public void ReadTest()
        {
            var reader = new ConfigBatchSizeSettingsReader();
            Assert.AreEqual(10, reader.Read(ServiceDto.Answer));
            Assert.AreEqual(11, reader.Read(ServiceDto.Appeal));
            Assert.AreEqual(12, reader.Read(ServiceDto.BlankInfo));
            Assert.AreEqual(5000, reader.Read(ServiceDto.Exam));
            Assert.AreEqual(5000, reader.Read(ServiceDto.Participant));
        }
    }
}