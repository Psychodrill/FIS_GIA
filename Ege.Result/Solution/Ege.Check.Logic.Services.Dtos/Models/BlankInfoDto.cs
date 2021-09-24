namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Бланк
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadBlankInfo")]
    public class BlankInfoDto : IParticipantExamDependentThing
    {
        public BlankInfoDto()
        {
            CreateDate = DateTime.Now;
        }

        /// <summary>
        ///     Идентификатор участника
        /// </summary>
        public Guid RbdId { get; set; }

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

        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        [PrimaryKeyPart]
        public int ExamGlobalId { get; set; }

        /// <summary>
        ///     Тип бланка: сейчас 1 - Ab, 3 - C
        /// </summary>
        [PrimaryKeyPart]
        public int BlankType { get; set; }

        /// <summary>
        ///     ? - нигде в интерфейсе не отображается
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        ///     ? - нигде в интерфейсе не отображается
        /// </summary>
        public int PrimaryMark { get; set; }

        /// <summary>
        ///     Код
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        ///     Количество страниц
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        ///     Дата создания
        /// </summary>
        public DateTime CreateDate { get; set; }
        
        /// <summary>
        ///     проект
        /// </summary>
        public int ProjectBatchId { get; set; }

        /// <summary>
        ///     имя проекта
        /// </summary>
        public string ProjectName { get; set; }
    }
}