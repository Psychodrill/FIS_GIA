namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Ответ
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadAnswers")]
    public class AnswerDto : IParticipantExamDependentThing
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
        ///     Часть экзамена
        /// </summary>
        [PrimaryKeyPart]
        public TaskType TaskTypeCode { get; set; }

        /// <summary>
        ///     Номер задания
        /// </summary>
        [PrimaryKeyPart]
        public int TaskNumber { get; set; }

        /// <summary>
        ///     Ответ
        /// </summary>
        public string AnswerValue { get; set; }

        /// <summary>
        ///     Полученный балл
        /// </summary>
        public int Mark { get; set; }

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