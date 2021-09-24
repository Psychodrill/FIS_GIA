namespace Ege.Check.Logic.Services.Participant.Participants
{
    using System;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public class ParticipantServiceResult
    {
        public ParticipantCacheModel Participant { get; set; }
        public bool Collision { get; set; }
    }

    public interface IParticipantService
    {
        /// <summary>
        /// Получить участника по хэшу ФИО, региону и коду регистрации или номеру документа без серии
        /// </summary>
        /// <param name="hash">Хэш ФИО</param>
        /// <param name="code">Код регистрации</param>
        /// <param name="document">Номер документа</param>
        /// <param name="regionId">Регион</param>
        /// <returns>Найденный участник, были ли коллизии по документу</returns>
        [NotNull]
        Task<ParticipantServiceResult> Get([NotNull]string hash, string code, string document, int regionId);

        [NotNull]
        Task<string> GetCodeByRbdId(Guid rbdId);
    }
}