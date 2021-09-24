namespace FogSoft.Helpers.Mail
{
    /// <summary>
    ///     Индивидуальное сообщение (внутри пакета для отправки SMS).
    /// </summary>
    public class SmsMessage
    {
        public string PhoneNumber { get; set; }
        public string Text { get; set; }

        /// <summary>
        ///     Необязательный параметр, позволяет избежать повторной отправки. Если раннее уже было отправлено SMS
        ///     с таким номером, то повторная отправка не производится, а возвращается номер ранее отправленного SMS.
        /// </summary>
        public int PhoneId { get; set; }
    }
}