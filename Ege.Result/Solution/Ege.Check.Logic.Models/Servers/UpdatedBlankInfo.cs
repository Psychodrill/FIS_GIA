namespace Ege.Check.Logic.Models.Servers
{
    using Ege.Check.Logic.Services.Dtos.Models;

    /// <summary>
    /// Информация о бланке, у которого изменилось число страниц
    /// </summary>
    public class UpdatedBlankInfo : IParticipantExamDependentThing
    {
        public int BlankType { get; set; }
        public int PageCount { get; set; }
        public string Code { get; set; }
        public int RegionId { get; set; }
        public int ExamGlobalId { get; set; }
    }
}
