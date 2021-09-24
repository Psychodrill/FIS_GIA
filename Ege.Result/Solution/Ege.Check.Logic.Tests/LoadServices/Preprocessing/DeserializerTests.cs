namespace Ege.Check.Logic.LoadServices
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Ege.Check.Logic.LoadServices.Preprocessing;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("../../LoadServices/SerializedExamDto.xml")]
    public class DeserializerTests
    {
        [TestMethod]
        public void DeserializeTest()
        {
            var deserializer = new Deserializer();
            ExamDto[] result;
            using (var fs = new FileStream("SerializedExamDto.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                )
            {
                result = deserializer.Deserialize<ExamDto>(fs);
            }
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);

            Assert.IsNotNull(result[0]);
            Assert.IsNotNull(result[1]);

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(DateTime.Parse("2015-03-17T00:00:00.000+04:00"), result[0].Date);
        //    Assert.AreEqual(ExamWave.Additional, result[0].WaveCode);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(DateTime.Parse("2015-03-14T00:00:00.000+04:00"), result[1].Date);
            Assert.AreEqual(ExamWave.Composition, result[1].WaveCode);
        }

        [TestMethod]
        public void SerializeTest()
        {
            var deserializer = new Deserializer();
            var expected = new[]
                {
      //              new ExamDto(1, DateTime.Parse("2015-03-17T00:00:00.000+04:00"), ExamWave.Additional),
                    new ExamDto(1, DateTime.Parse("2015-03-14T00:00:00.000+04:00"), ExamWave.Composition),
                };
            using (var stream = deserializer.Serialize(expected))
            {
                var actual = deserializer.Deserialize<ExamDto>(stream);
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        ///     Экзамен
        /// </summary>
        [Serializable]
        public class ExamDto : IEquatable<ExamDto>
        {
            private static readonly IEqualityComparer<ExamDto> IdDateWaveCodeComparerInstance =
                new IdDateWaveCodeEqualityComparer();

            public ExamDto(int id, DateTime date, ExamWave waveCode)
            {
                Id = id;
                Date = date;
                WaveCode = waveCode;
            }

            public ExamDto()
            {
            }

            public int Id { get; set; }
            public DateTime Date { get; set; }
            public ExamWave WaveCode { get; set; }

            public static IEqualityComparer<ExamDto> IdDateWaveCodeComparer
            {
                get { return IdDateWaveCodeComparerInstance; }
            }

            public bool Equals(ExamDto other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Id == other.Id && Date.Equals(other.Date) && WaveCode == other.WaveCode;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((ExamDto) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Id;
                    hashCode = (hashCode*397) ^ Date.GetHashCode();
                    hashCode = (hashCode*397) ^ (int) WaveCode;
                    return hashCode;
                }
            }

            public static bool operator ==(ExamDto left, ExamDto right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(ExamDto left, ExamDto right)
            {
                return !Equals(left, right);
            }

            private sealed class IdDateWaveCodeEqualityComparer : IEqualityComparer<ExamDto>, IComparer<ExamDto>
            {
                public int Compare(ExamDto x, ExamDto y)
                {
                    return x.Equals(y) ? 0 : x.Id.CompareTo(y.Id);
                }

                public bool Equals(ExamDto x, ExamDto y)
                {
                    if (ReferenceEquals(x, y)) return true;
                    if (ReferenceEquals(x, null)) return false;
                    if (ReferenceEquals(y, null)) return false;
                    if (x.GetType() != y.GetType()) return false;
                    return x.Id == y.Id && x.Date.Equals(y.Date) && x.WaveCode == y.WaveCode;
                }

                public int GetHashCode(ExamDto obj)
                {
                    unchecked
                    {
                        var hashCode = obj.Id;
                        hashCode = (hashCode*397) ^ obj.Date.GetHashCode();
                        hashCode = (hashCode*397) ^ (int) obj.WaveCode;
                        return hashCode;
                    }
                }
            }
        }
    }
}