namespace Ege.Check.Dal.Blanks
{
    using System;
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IBlankModelCreator
    {
        [NotNull]
        string ExamDateToString(DateTime examDate);

        [NotNull]
        string SubjectCodeToString(int subjectCode);

        [NotNull]
        IEnumerable<BlankClientModel> Create([NotNull]BlankIntermediateModel blank, [NotNull] NameCoderBlank blankcoder);

        [NotNull]
        IEnumerable<BlankClientModel> Create(
            [NotNull] IEnumerable<BlankCacheModel> blanks,
            DateTime examDate,
            int subjectCode,
            [NotNull]NameCoderBlank blankcoder);

        [NotNull]
        string GetListFileUrl([NotNull]string serverUrl, DateTime examDate, int subjectCode);

        [NotNull]
        string GetPageCountFileUrl(string serverUrl, DateTime examDate, int subjectCode);
    }
}
