namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Экзамен участника
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadParticipantExams")]
    public class ParticipantExamDto : IParticipantDependentThing
    {
        /// <summary>
        ///     Идентификатор участника
        /// </summary>
        public Guid RbdId { get; set; }

        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        [PrimaryKeyPart]
        public int ExamGlobalId { get; set; }

        /// <summary>
        ///     Первичный балл
        /// </summary>
        public int PrimaryMark { get; set; }

        /// <summary>
        ///     Тестовый балл
        /// </summary>
        public int TestMark { get; set; }

        /// <summary>
        ///     2 в случае незачёта, 5 в случае зачёта
        /// </summary>
        public int Mark5 { get; set; }

        /// <summary>
        ///     Статус
        /// </summary>
        public int ProcessCondition { get; set; }

        /// <summary>
        ///     Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Регистрационный код участника
        /// </summary>
        [PrimaryKeyPart]
        public string Code { get; set; }

        /// <summary>
        ///     Регион участника
        /// </summary>
        [PrimaryKeyPart]
        public int RegionId { get; set; }
    }
}