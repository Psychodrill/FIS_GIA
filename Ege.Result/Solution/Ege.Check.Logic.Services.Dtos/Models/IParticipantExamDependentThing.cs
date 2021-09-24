namespace Ege.Check.Logic.Services.Dtos.Models
{
    public interface IParticipantExamDependentThing : IParticipantDependentThing
    {
        int ExamGlobalId { get; set; }
    }
}
