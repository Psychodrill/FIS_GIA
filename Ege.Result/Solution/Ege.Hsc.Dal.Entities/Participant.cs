namespace Ege.Hsc.Dal.Entities
{
    using System;

    /// <summary>
    ///     Участники экзаменов
    /// </summary>
    public class Participant
    {
        /// <summary>
        ///     Идентификатор в системе выгрузки бланков
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Глобальный идентификатор участника
        /// </summary>
        public Guid ParticipantRbdId { get; set; }

        /// <summary>
        ///     Хэш ФИО участника
        /// </summary>
        public string ParticipantHash { get; set; }

        /// <summary>
        ///     Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        ///     Идентификатор региона
        /// </summary>
        public int RegionId { get; set; }
    }
}