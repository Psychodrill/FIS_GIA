// ReSharper disable CheckNamespace
namespace Ege.Check.Logic.Services.Participant.ExamLists
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Dal.MemoryCache.CancelledParticipantExams;
    using Ege.Check.Logic.Models.Cache;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ExamListFilterHelperTests
    {
        private void SetComposition(IEnumerable<ExamCacheModel> exams)
        {
            foreach (var exam in exams)
            {
                exam.IsComposition = true;
            }
        }

        [TestMethod]
        public void FilterCompositionsTest()
        {
            var mockCache = new Mock<ICancelledParticipantExamMemoryCache>(MockBehavior.Strict);
            mockCache.Setup(c => c.IsHidden(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            var helper = new ExamListFilterHelper(mockCache.Object);

            var participant = new ParticipantCacheModel {Code = "1"};
            var regionSettings = new Dictionary<int, RegionExamSettingCacheModel> {{0, new RegionExamSettingCacheModel {ShowResult = true}}};
            // something ridiculous
            var exams = new[]
            {
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 1), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 2), HasResult = true},
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 3), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 4), HasResult = true},
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 5), HasResult = false},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 6), HasResult = false},
            };
            SetComposition(exams);

            var result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[0], result.ElementAt(0));

            // many good marks
            exams = new[]
            {
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 5), HasResult = true},
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 1), HasResult = true},
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 3), HasResult = true},
            };
            SetComposition(exams);

            result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[1], result.ElementAt(0));

            // many good marks after a bad mark
            exams = new[]
            {
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 5), HasResult = true},
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 1), HasResult = true},
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 3), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2013, 12, 31), HasResult = true},
            };
            SetComposition(exams);

            result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[1], result.ElementAt(0));

            // a good mark after many bad marks
            exams = new[]
            {
                new ExamCacheModel {Mark5 = 5, ExamDate = new DateTime(2014, 1, 5), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 1), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 3), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2013, 12, 31), HasResult = true},
            };
            SetComposition(exams);

            result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[0], result.ElementAt(0));

            // many bad marks
            exams = new[]
            {
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 1), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 5), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 3), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2013, 12, 31), HasResult = true},
            };
            SetComposition(exams);

            result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[1], result.ElementAt(0));

            // many bad marks, then no result
            exams = new[]
            {
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 1), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 5), HasResult = false},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 3), HasResult = true},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2013, 12, 31), HasResult = true},
            };
            SetComposition(exams);

            result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[2], result.ElementAt(0));

            // no result
            exams = new[]
            {
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 1), HasResult = false},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 5), HasResult = false},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2014, 1, 3), HasResult = false},
                new ExamCacheModel {Mark5 = 2, ExamDate = new DateTime(2013, 12, 31), HasResult = false},
            };
            SetComposition(exams);

            result = helper.ApplyFilter(participant, exams, regionSettings);
            Assert.AreEqual(1, result.Count);
            Assert.AreSame(exams[3], result.ElementAt(0));
        }
    }
}
