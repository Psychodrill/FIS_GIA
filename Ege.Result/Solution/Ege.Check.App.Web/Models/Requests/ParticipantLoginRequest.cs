namespace Ege.Check.App.Web.Models.Requests
{
    public class ParticipantLoginRequest
    {
        /// <summary>
        ///     Хэш ФИО
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        ///     Код регистрации
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Номер документа
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        ///     Регион
        /// </summary>
        public int Region { get; set; }

        public string Captcha { get; set; }

        public string Token { get; set; }

        public string reCaptureToken { get; set; }
    }
}