namespace FbsUtility
{
    using System;
    using System.Web.Management;

    /// <summary>
    /// Предоставляет информацию об обработанных ощибках приложения.
    /// </summary>
    /// <remarks>
    /// Используется для выявления и анализа ошибок приложания.
    /// </remarks>
    public class HandeledWebErrorEvent : WebErrorEvent
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandeledWebErrorEvent"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        public HandeledWebErrorEvent(string message, object eventSource, int eventCode)
            : base(message, eventSource, eventCode, new Exception())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandeledWebErrorEvent"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public HandeledWebErrorEvent(string message, object eventSource, int eventCode, Exception exception)
            : base(message, eventSource, eventCode, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandeledWebErrorEvent"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <param name="eventCode">
        /// The event code.
        /// </param>
        /// <param name="eventDetailCode">
        /// The event detail code.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public HandeledWebErrorEvent(
            string message, object eventSource, int eventCode, int eventDetailCode, Exception exception)
            : base(message, eventSource, eventCode, eventDetailCode, exception)
        {
        }

        #endregion
    }
}