namespace Ege.Check.Logic.Models.Response
{
    using Ege.Check.Logic.Models.Cache;

    public class ExamResponse
    {
        /// <summary>
        ///     Максимальные баллы, критерии, допустимые символы, проходной балл
        /// </summary>
        public ExamInfoCacheModel ExamInfo { get; set; }

        /// <summary>
        ///     Ответы, полученные за них баллы
        /// </summary>
        public AnswerCollectionCacheModel Answers { get; set; }

        /// <summary>
        ///     Документ ГЭК
        /// </summary>
        public GekDocumentCacheModel GekDocument { get; set; }

        /// <summary>
        ///     URL серверов бланков
        /// </summary>
        public BlanksServerCacheModel Servers { get; set; }
    }
}