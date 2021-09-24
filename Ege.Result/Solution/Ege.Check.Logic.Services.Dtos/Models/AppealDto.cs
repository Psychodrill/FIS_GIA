namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Апелляция/изменение статуса апелляции
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadAppeals")]
    public class AppealDto : IParticipantExamDependentThing
    {
        /// <summary>
        ///     Идентификатор участника
        /// </summary>
        public Guid RbdId { get; set; }

        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        public int ExamGlobalId { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public bool AppealType { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public int? Station { get; set; }

        /// <summary>
        ///     Дата создания/изменения статуса
        /// </summary>
        public DateTime CreateDate { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public int? ReviewType { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public string Agent { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public string Phone { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public string Mail { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public int? AgentType { get; set; }

        /// <summary>
        ///     Текущий статус апелляции
        /// </summary>
        public int Status { get; set; }

        ///// <summary>
        /////     ? - нигде в интерфейсе не отображается
        ///// </summary>
        //public bool SignDocument { get; set; }

        /// <summary>
        ///     Регистрационный код участника
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Регион участника
        /// </summary>
        public int RegionId { get; set; }
    }
}