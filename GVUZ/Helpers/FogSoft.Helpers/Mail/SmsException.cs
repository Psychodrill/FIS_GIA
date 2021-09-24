using System;
using System.Runtime.Serialization;

namespace FogSoft.Helpers.Mail
{
    /// <summary>
    ///     Ошибка с сообщением о проблеме для клиента.
    /// </summary>
    public class SmsException : Exception
    {
        public SmsException(string message) : base(message)
        {
        }

        public SmsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SmsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}