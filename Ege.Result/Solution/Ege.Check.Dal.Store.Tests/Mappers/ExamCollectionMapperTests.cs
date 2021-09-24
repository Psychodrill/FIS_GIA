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
    public class ExamCollectionMapperTests
    {
        [TestMethod]
        public async Task TestMap()
        {
            var mapper = new ExamCollectionMapper();
            var sqlDataReader = new Mock<DbDataReader>(MockBehavior.Strict);
            const int examIdOrdinal = 1;
            const int examDateOrdinal = 2;
            const int subjectOrdinal = 3;
            const int testMarkOrdinal = 4;
            const int mark5Ordinal = 5;
            const int minMarkOrdinal = 6;
            const int statusOrdinal = 7;
            const int hasAppealsOrdinal = 8;
            const int hasResultOrdinal = 9;
            const int appealStatusOrdinal = 10;
            const int isHiddenOrdinal = 11;
            const int isBasicMathOrdinal = 12;
            const int isCompositionOrdinal = 13;
            const int isForeignLanguageOrdinal = 14;
            const int oralExamIdOrdinal = 15;
            const int hasOralResultOrdinal = 16;
            const int oralStatusOrdinal = 17;

            sqlDataReader.Setup(x => x.GetOrdinal("ExamGlobalId")).Returns(examIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ExamDate")).Returns(examDateOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("SubjectName")).Returns(subjectOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("TestMark")).Returns(testMarkOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("Mark5")).Returns(mark5Ordinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("MinValue")).Returns(minMarkOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("ProcessCondition")).Returns(statusOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("HasAppeal")).Returns(hasAppealsOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("HasResult")).Returns(hasResultOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("AppealStatus")).Returns(appealStatusOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("IsHidden")).Returns(isHiddenOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("IsBasicMath")).Returns(isBasicMathOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("IsComposition")).Returns(isCompositionOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("IsForeignLanguage")).Returns(isForeignLanguageOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("OralExamGlobalId")).Returns(oralExamIdOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("HasOralResult")).Returns(hasOralResultOrdinal).Verifiable();
            sqlDataReader.Setup(x => x.GetOrdinal("OralProcessCondition")).Returns(oralStatusOrdinal).Verifiable();

            var clousureIterator = new ClosureIterator(2);
            sqlDataReader.Setup(x => x.ReadAsync(CancellationToken.None))
                         .Returns(() => Task.FromResult(clousureIterator.Next()));

            sqlDataReader.Setup(x => x.GetInt32(examIdOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetDateTime(examDateOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToDateTime(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetString(subjectOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToString(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(testMarkOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(mark5Ordinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(minMarkOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(statusOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(hasAppealsOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(hasResultOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(appealStatusOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(false))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetInt32(appealStatusOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToInt(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(isHiddenOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(isBasicMathOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(isCompositionOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(isForeignLanguageOrdinal))
                         .Returns(
                             (int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(oralExamIdOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(true))
                         .Verifiable();
            sqlDataReader.Setup(x => x.IsDBNullAsync(oralStatusOrdinal, CancellationToken.None))
                         .Returns(() => Task.FromResult(true))
                         .Verifiable();
            sqlDataReader.Setup(x => x.GetBoolean(hasOralResultOrdinal))
                         .Returns((int ordinalId) => ClosureIterator.ToBool(clousureIterator.CurrentIteration, ordinalId))
                         .Verifiable();

            var examsModel = await mapper.Map(sqlDataReader.Object);
            Assert.AreEqual(clousureIterator.MaxIterations, examsModel.Exams.Count);
            var exams = examsModel.Exams.ToArray();
            foreach (var clousureIteration in clousureIterator.Enumerate(examsModel.Exams))
            {
                var exam = clousureIteration.Value;
                Assert.IsNotNull(exam);
                Assert.AreEqual(clousureIteration.IntValue(examIdOrdinal), exam.ExamId);
                Assert.AreEqual(clousureIteration.DateTimeValue(examDateOrdinal), exam.ExamDate);
                Assert.AreEqual(clousureIteration.StringValue(subjectOrdinal), exam.Subject);
                Assert.AreEqual(clousureIteration.IntValue(testMarkOrdinal), exam.TestMark);
                Assert.AreEqual(clousureIteration.IntValue(mark5Ordinal), exam.Mark5);
                Assert.AreEqual(clousureIteration.IntValue(minMarkOrdinal), exam.MinMark);
                Assert.AreEqual(clousureIteration.IntValue(statusOrdinal), exam.Status);
                Assert.AreEqual(clousureIteration.BoolValue(hasAppealsOrdinal), exam.HasAppeal);
                Assert.AreEqual(clousureIteration.BoolValue(hasResultOrdinal), exam.HasResult);
                Assert.AreEqual(clousureIteration.IntValue(appealStatusOrdinal), exam.AppealStatus);
                Assert.AreEqual(clousureIteration.BoolValue(isHiddenOrdinal), exam.IsHidden);
                Assert.AreEqual(clousureIteration.BoolValue(isBasicMathOrdinal), exam.IsBasicMath);
                Assert.AreEqual(clousureIteration.BoolValue(isCompositionOrdinal), exam.IsComposition);
                Assert.AreEqual(clousureIteration.BoolValue(isForeignLanguageOrdinal), exam.IsForeignLanguage);
            }
            sqlDataReader.VerifyAll();
        }
    }
}