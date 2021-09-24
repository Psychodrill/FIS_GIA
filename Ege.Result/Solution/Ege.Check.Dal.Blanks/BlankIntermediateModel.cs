namespace Ege.Check.Dal.Blanks
{
    using System;
    using Ege.Check.Logic.Models.Cache;

    public class BlankIntermediateModel : BlankCacheModel
    {
        public DateTime ExamDate { get; set; }
        public int SubjectCode { get; set; }
    }
}