namespace Ege.Check.Logic.Models.Cache
{
    using System;
    using JetBrains.Annotations;

    public class ExamMemoryCacheModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int SubjectCode { get; set; }

        [NotNull]
        public SubjectMemoryCacheModel Subject { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, Date: {1}, SubjectCode: {2}, Subject: {3}", Id, Date, SubjectCode, Subject);
        }
    }
}