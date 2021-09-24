namespace Ege.Check.Dal.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal enum BlankType
    {
        Composition = 0,
        Ab = 1,
        Ab2 = 2,
        C = 3,
    }

    internal class BlankModelCreator : IBlankModelCreator
    {
        private IEnumerable<BlankClientModel> Create(
            string barcode,
            int blankType,
            int? pageCount,
            int projectId,
            string projectCode,
            [NotNull]string examDate,
            [NotNull]string subjectCode,
            [NotNull]NameCoderBlank blankcoder)
        {
            if (!pageCount.HasValue)
            {
                yield break;
            }
            string code;
            switch (blankType)
            {
                case (int) BlankType.Composition:
                    var barcodeSuffixed = string.Format("{0}_{1}_{2}", barcode, projectId, projectCode);
                    for (var i = 0; i < pageCount; ++i)
                    {
                        code = blankcoder.CreateCode(barcodeSuffixed, blankType, i);
                        yield return new BlankClientModel
                        {
                            Title = string.Format("Бланк сочинения. Страница {0}.", i + 1),
                            Url = Path(examDate, subjectCode, code),
                            Code = code,
                        };
                    }
                    break;
                case (int) BlankType.Ab:
                case (int) BlankType.Ab2:
                    code = blankcoder.CreateCodeAb(barcode);
                    yield return new BlankClientModel
                        {
                            Title = "Бланк ответов №1.",
                            Url = Path(examDate, subjectCode, code),
                            Code = code,
                        };
                    break;
                case (int) BlankType.C:
                    for (var i = 0; i < pageCount; i++)
                    {
                        code = blankcoder.CreateCodeC(barcode, i);
                        yield return new BlankClientModel
                            {
                                Title = string.Format("Бланк ответов №2. {0}",
                                                  pageCount > 1 ? string.Format("Страница {0}.", i + 1) : ""),
                                Url = Path(examDate, subjectCode, code),
                                Code = code,
                            };
                    }
                    break;
            }
        }

        private string Path([NotNull] string examDate, [NotNull] string subjectCode, [NotNull] string code)
        {
            return string.Format("{0}/{1}/{2}/{3}/{4}.png", subjectCode, examDate, code[0], code[1], code);
        }

        public string GetListFileUrl(string serverUrl, DateTime examDate, int subjectCode)
        {
            return string.Format("{0}/{1}/{2}/list.txt", serverUrl, SubjectCodeToString(subjectCode), ExamDateToString(examDate));
        }

        public string GetPageCountFileUrl(string serverUrl, DateTime examDate, int subjectCode)
        {
            return string.Format("{0}/{1}/{2}/pagescount.txt", serverUrl, SubjectCodeToString(subjectCode), ExamDateToString(examDate));
        }

        public string ExamDateToString(DateTime examDate)
        {
            return examDate.ToString("yyyy.MM.dd");
        }

        public string SubjectCodeToString(int subjectCode)
        {
            return subjectCode.ToString("00");
        }

        public IEnumerable<BlankClientModel> Create(BlankIntermediateModel blank, NameCoderBlank blankcoder)
        {
            return Create(
                blank.ToEnumerable(), 
                blank.ExamDate,
                blank.SubjectCode, 
                blankcoder);
        }

        public IEnumerable<BlankClientModel> Create(IEnumerable<BlankCacheModel> blanks, DateTime examDate, int subjectCode, NameCoderBlank blankcoder)
        {
            var subject = SubjectCodeToString(subjectCode);
            var date = ExamDateToString(examDate);
            return blanks.Where(b => b != null).SelectMany(b => Create(b.Barcode, b.BlankType, b.PageCount, b.ProjectBatchId, b.ProjectName, date, subject, blankcoder));
        }
    }
}
