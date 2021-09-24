namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    /// Связь письменного и устного экзаменов
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadParticipantExamLinks")]
    public class ParticipantExamLinkDto : IParticipantExamDependentThing
    {
        /// <summary>
        ///     Идентификатор участника
        /// </summary>
        public Guid RbdId { get; set; }

        /// <summary>
        ///     Код регистрации
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        ///     Регион участника
        /// </summary>
        public int RegionId { get; set; }
        
        /// <summary>
        ///     Идентификатор письменного экзамена
        /// </summary>
        public int ExamGlobalId { get; set; }

        /// <summary>
        ///     Идентификатор устного экзамена
        /// </summary>
        public int OralExamGlobalId { get; set; }
    }
}
