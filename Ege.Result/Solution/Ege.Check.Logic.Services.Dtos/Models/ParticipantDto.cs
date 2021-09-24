namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Участник
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadParticipants")]
    public class ParticipantDto
    {
        /// <summary>
        ///     Идентификатор участника
        /// </summary>
        public Guid RbdId { get; set; }

        /// <summary>
        ///     Код регистрации
        /// </summary>
        [PrimaryKeyPart]
        public string Code { get; set; }

        /// <summary>
        ///     Хэш фио
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        ///     Номер документа без серии
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        ///     Регион участника
        /// </summary>
        [PrimaryKeyPart]
        public int RegionId { get; set; }

        /// <summary>
        ///     Удалить участника
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}