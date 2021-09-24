namespace Ege.Check.App.Web.Models.Requests
{
    public class ExamCancellationRequest
    {
        /// <summary>
        ///     Код участника
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        public int ExamGlobalId { get; set; }

        /// <summary>
        ///     Регион участника
        /// </summary>
        public int RegionId { get; set; }
    }
}