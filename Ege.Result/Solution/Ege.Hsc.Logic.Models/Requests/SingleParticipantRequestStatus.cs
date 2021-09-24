namespace Ege.Hsc.Logic.Models.Requests
{
    using System.ComponentModel;

    public enum SingleParticipantRequestStatus
    {
        /// <summary>
        /// Участник не найден
        /// </summary>
        [Description("Участник (результат) не найден")]
        NotFound,
        /// <summary>
        /// Найдено несколько участников с совпадающим хэшем фио и номером документа
        /// </summary>
        [Description("Найдено более одного участника с указанными данными")]
        Collision,
        /// <summary>
        /// Ошибки при выгрузке бланков
        /// </summary>
        [Description("Бланки участника не найдены на сервере бланков")]
        HasErrors,
        /// <summary>
        /// Все бланки выгружены успешно
        /// </summary>
        [Description("Все бланки выгружены успешно")]
        Success,

        [Description("Бланки участника ещё не загружены с сервера РЦОИ")]
        NotYetDownloaded,

        [Description("Не указан сервер бланков региона")]
        NoServerUrl,
    }
}
