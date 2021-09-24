namespace Ege.Check.Dal.Store.Tests.Mappers
{
    using System.Data.Common;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class RegionSettingsMapperTests
    {
        [TestMethod]
        public async Task TestMap()
        {
            var mapper = new RegionSettingsMapper();
            var sqlDataReader = new Mock<DbDataReader>(MockBehavior.Strict);
            const int examIdOrdinal = 1;
            const int showBlanksOrdinal = 2;
            const int showResultOrdinal = 3;
            const int numberOrdinal = 4;
            const int createDateOrdinal = 5;
            const int phoneOrdinal = 6;
            const int hotLineDataOrdinal = 7;
            const int commonBlanksOrdinal = 8;
            const int compositionBlanksOrdinal = 9;
            const int rowTypeOrdinal = 10;
            const int regionIdOrdinal = 11;
            const int urlOrdinal = 12;

            sqlDataReader.Setup(x => x.GetOrdinal("ExamGlobalId")).Returns(examIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ShowBlanks")).Returns(showBlanksOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ShowResult")).Returns(showResultOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Number")).Returns(numberOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("CreateDate")).Returns(createDateOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("HotLineData")).Returns(phoneOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Description")).Returns(hotLineDataOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("BlanksServer")).Returns(commonBlanksOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("CompositionBlanksServer"))
                         .Returns(compositionBlanksOrdinal)
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("RowType")).Returns(rowTypeOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("RegionId")).Returns(regionIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Url")).Returns(urlOrdinal).Verifiable();

            var closureIterationNumber = 0;
            const int maxClosureIterations = 6;
            sqlDataReader.Setup(x => x.ReadAsync(CancellationToken.None)).Returns(() =>
                {
                    var res = Task.FromResult(closureIterationNumber < maxClosureIterations);
                    closureIterationNumber++;
                    return res;
                });
            sqlDataReader.Setup(x => x.GetInt32(examIdOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(showBlanksOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToBool(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(showResultOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToBool(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(numberOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(numberOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(createDateOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetDateTime(createDateOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToDateTime(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(phoneOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(phoneOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(hotLineDataOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(hotLineDataOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(closureIterationNumber, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(commonBlanksOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(true))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(compositionBlanksOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(true))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(rowTypeOrdinal))
                         .Returns((int ordinalId) => closureIterationNumber == 1 || closureIterationNumber == 4)
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(regionIdOrdinal))
                         .Returns((int ordinalId) => closureIterationNumber <= 3 ? 10 : 20)
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(urlOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(urlOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToString(closureIterationNumber, ordinalId))
                         .Verifiable();

            var regionSettings = (await mapper.Map(sqlDataReader.Object));
            Assert.AreEqual(2, regionSettings.Count);

            var firstRegionSettings = regionSettings[10];

            Assert.IsNotNull(firstRegionSettings);
            var settings = firstRegionSettings.Settings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(2, settings.Count);
            var settingsArray = settings.ToArray();

            Assert.IsNotNull(settingsArray[0].Value);
            Assert.AreEqual(ClosureIterator.ToInt(2, examIdOrdinal), settingsArray[0].Key);
            Assert.AreEqual(ClosureIterator.ToBool(2, showBlanksOrdinal), settingsArray[0].Value.ShowBlanks);
            Assert.AreEqual(ClosureIterator.ToBool(2, showResultOrdinal), settingsArray[0].Value.ShowResult);
            Assert.AreEqual(ClosureIterator.ToString(2, numberOrdinal), settingsArray[0].Value.GekDocument.GekNumber);
            Assert.AreEqual(ClosureIterator.ToDateTime(2, createDateOrdinal), settingsArray[0].Value.GekDocument.GekDate);

            Assert.IsNotNull(settingsArray[1].Value);
            Assert.AreEqual(ClosureIterator.ToInt(3, examIdOrdinal), settingsArray[1].Key);
            Assert.AreEqual(ClosureIterator.ToBool(3, showBlanksOrdinal), settingsArray[1].Value.ShowBlanks);
            Assert.AreEqual(ClosureIterator.ToBool(3, showResultOrdinal), settingsArray[1].Value.ShowResult);
            Assert.AreEqual(ClosureIterator.ToString(3, numberOrdinal), settingsArray[1].Value.GekDocument.GekNumber);
            Assert.AreEqual(ClosureIterator.ToDateTime(3, createDateOrdinal), settingsArray[1].Value.GekDocument.GekDate);

            Assert.AreEqual(ClosureIterator.ToString(1, phoneOrdinal), firstRegionSettings.Info.HotlinePhone);
            Assert.AreEqual(ClosureIterator.ToString(1, hotLineDataOrdinal), firstRegionSettings.Info.Info);

            var secondRegionSettings = regionSettings[20];
            Assert.IsNotNull(secondRegionSettings);
            settings = secondRegionSettings.Settings;
            Assert.IsNotNull(settings);
            Assert.AreEqual(2, settings.Count);
            settingsArray = settings.ToArray();

            Assert.IsNotNull(settingsArray[0].Value);
            Assert.AreEqual(ClosureIterator.ToInt(5, examIdOrdinal), settingsArray[0].Key);
            Assert.AreEqual(ClosureIterator.ToBool(5, showBlanksOrdinal), settingsArray[0].Value.ShowBlanks);
            Assert.AreEqual(ClosureIterator.ToBool(5, showResultOrdinal), settingsArray[0].Value.ShowResult);
            Assert.AreEqual(ClosureIterator.ToString(5, numberOrdinal), settingsArray[0].Value.GekDocument.GekNumber);
            Assert.AreEqual(ClosureIterator.ToDateTime(5, createDateOrdinal), settingsArray[0].Value.GekDocument.GekDate);
            Assert.AreEqual(ClosureIterator.ToString(5, urlOrdinal), settingsArray[0].Value.GekDocument.Url);

            Assert.IsNotNull(settingsArray[1].Value);
            Assert.AreEqual(ClosureIterator.ToInt(6, examIdOrdinal), settingsArray[1].Key);
            Assert.AreEqual(ClosureIterator.ToBool(6, showBlanksOrdinal), settingsArray[1].Value.ShowBlanks);
            Assert.AreEqual(ClosureIterator.ToBool(6, showResultOrdinal), settingsArray[1].Value.ShowResult);
            Assert.AreEqual(ClosureIterator.ToString(6, numberOrdinal), settingsArray[1].Value.GekDocument.GekNumber);
            Assert.AreEqual(ClosureIterator.ToDateTime(6, createDateOrdinal), settingsArray[1].Value.GekDocument.GekDate);
            Assert.AreEqual(ClosureIterator.ToString(6, urlOrdinal), settingsArray[1].Value.GekDocument.Url);

            Assert.AreEqual(ClosureIterator.ToString(4, phoneOrdinal), secondRegionSettings.Info.HotlinePhone);
            Assert.AreEqual(ClosureIterator.ToString(4, hotLineDataOrdinal), secondRegionSettings.Info.Info);

            sqlDataReader.VerifyAll();
        }

        [TestMethod]
        public async Task TestMapNotFound()
        {
            var mapper = new RegionSettingsMapper();
            var sqlDataReader = new Mock<DbDataReader>(MockBehavior.Strict);

            sqlDataReader.Setup(x => x.ReadAsync(CancellationToken.None)).Returns(Task.FromResult(false));
            const int examIdOrdinal = 1;
            const int showBlanksOrdinal = 2;
            const int showResultOrdinal = 3;
            const int numberOrdinal = 4;
            const int createDateOrdinal = 5;
            const int phoneOrdinal = 6;
            const int hotLineDataOrdinal = 7;
            const int commonBlanksOrdinal = 8;
            const int compositionBlanksOrdinal = 9;
            const int rowTypeOrdinal = 10;
            const int regionIdOrdinal = 11;
            const int urlOrdinal = 12;

            sqlDataReader.Setup(x => x.GetOrdinal("ExamGlobalId")).Returns(examIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ShowBlanks")).Returns(showBlanksOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ShowResult")).Returns(showResultOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Number")).Returns(numberOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("CreateDate")).Returns(createDateOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("HotLineData")).Returns(phoneOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Description")).Returns(hotLineDataOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("BlanksServer")).Returns(commonBlanksOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("CompositionBlanksServer"))
                         .Returns(compositionBlanksOrdinal)
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("RowType")).Returns(rowTypeOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("RegionId")).Returns(regionIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Url")).Returns(urlOrdinal).Verifiable();

            var regionSettings = (await mapper.Map(sqlDataReader.Object));
            Assert.IsNotNull(regionSettings);
            Assert.AreEqual(0, regionSettings.Count);
            //Assert.IsNotNull(regionSettings.Info);
            //Assert.IsNotNull(regionSettings.Servers);
            //Assert.IsNotNull(regionSettings.Settings);
            //Assert.AreEqual(0, regionSettings.Settings.Count);
            //Assert.IsNull(regionSettings.Info.Info);
            //Assert.IsNull(regionSettings.Info.HotlinePhone);
            //Assert.IsNull(regionSettings.Servers.Common);
            //Assert.IsNull(regionSettings.Servers.Composition);
            sqlDataReader.VerifyAll();
        }
    }
}