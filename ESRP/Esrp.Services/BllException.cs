namespace Esrp.Services
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Класс, описывающий ожидаемые разработчиком исключения происходящие в рамках проекта
    /// </summary>
    [Serializable]
    public class BllException : Exception
    {
        #region Constructors and Destructors

        public BllException()
        {
        }

        public BllException(string message)
            : base(message)
        {
        }

        public BllException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BllException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}