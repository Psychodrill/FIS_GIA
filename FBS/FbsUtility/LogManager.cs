namespace Fbs.Utility
{
    using System;
    using System.Text;
    using System.Web;

    using log4net;

    /// <summary>
    /// Класс с набором функций, упрощающих запись лога сообщений.
    /// </summary>
    public class LogManager
    {
        #region Constants and Fields

        /// <summary>
        /// The m log.
        /// </summary>
        private static readonly ILog mLog = log4net.LogManager.GetLogger(typeof(LogManager));

        #endregion

        #region Public Methods

        /// <summary>
        /// Печать сообщения об ошибке вида "* Error: имя_исключения, сообщение, трасса стека".
        /// </summary>
        /// <param name="ex">
        /// Исключение 
        /// </param>
        public static void Error(Exception ex)
        {
            try
            {
                var info = new ErrorInfo(ex);
                mLog.Error(string.Format("* Error: {0}", info));
                if (ex.InnerException != null)
                {
                    mLog.Error(ex.InnerException);
                }
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Error", innerEx);
            }
        }

        public static void Error(string message)
        {
            try
            {
                ErrorInfo info = new ErrorInfo(new Exception(message));
                mLog.Error(String.Format("* Error: {0}", info.ToString()));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Error", innerEx);
            }

        }

        /// <summary>
        /// Печать информационного сообщения вида "! Сообщение". В quiet mode сообщение не печатается.
        /// </summary>
        /// <param name="message">
        /// Сообщение 
        /// </param>
        public static void Info(string message)
        {
            try
            {
                mLog.Info(string.Format(". {0}", message));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Info", innerEx);
            }
        }

        /// <summary>
        /// Печать предупреждения вида "* Warning: сообщение".
        /// </summary>
        /// <param name="message">
        /// Строка для печати
        /// </param>
        public static void Warning(string message)
        {
            try
            {
                mLog.Warn(string.Format("! Warning: {0}", message));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Warning", innerEx);
            }
        }

        /// <summary>
        /// The warning.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        /// <exception cref="ApplicationException">
        /// </exception>
        public static void Warning(Exception ex)
        {
            try
            {
                mLog.Warn(string.Format("! Warning: {0}", ex));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Warning", innerEx);
            }
        }

        #endregion

        /// <summary>
        /// Составляет форматированное подробное описание ошибки.
        /// </summary>
        private class ErrorInfo
        {
            #region Constants and Fields

            private readonly Exception mCurrentException;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ErrorInfo"/> class.
            /// </summary>
            /// <param name="currentException">
            /// The current exception.
            /// </param>
            public ErrorInfo(Exception currentException)
            {
                this.mCurrentException = currentException;
            }

            #endregion

            #region Properties

            private string CurrentUserIp
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null)
                    {
                        return null;
                    }

                    return HttpContext.Current.Request.UserHostAddress;
                }
            }

            private string CurrentUserName
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.User == null)
                    {
                        return null;
                    }

                    if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    {
                        return "anonymous";
                    }

                    return HttpContext.Current.User.Identity.Name;
                }
            }

            private string ReferrerPageUrl
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null
                        || HttpContext.Current.Request.UrlReferrer == null)
                    {
                        return null;
                    }

                    return HttpContext.Current.Request.UrlReferrer.ToString();
                }
            }

            private string SourcePageUrl
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null)
                    {
                        return null;
                    }

                    return HttpContext.Current.Request.Url.ToString();
                }
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Возвращает подробную информацию об ошибке.
            /// </summary>
            /// <returns>
            /// The to string.
            /// </returns>
            public override string ToString()
            {
                var info = new StringBuilder();

                info.Append(this.mCurrentException.Message);
                if (this.CurrentUserName != null)
                {
                    info.AppendFormat("{0}User name: {1}", Environment.NewLine, this.CurrentUserName);
                }

                if (this.CurrentUserIp != null)
                {
                    info.AppendFormat("{0}User ip: {1}", Environment.NewLine, this.CurrentUserIp);
                }

                if (this.SourcePageUrl != null)
                {
                    info.AppendFormat("{0}Source page: {1}", Environment.NewLine, this.SourcePageUrl);
                }

                if (this.ReferrerPageUrl != null)
                {
                    info.AppendFormat("{0}Source referrer: {1}", Environment.NewLine, this.ReferrerPageUrl);
                }

                info.AppendFormat(
                    "{0}Exception type: {1}", Environment.NewLine, this.mCurrentException.GetType().FullName);
                info.AppendFormat("{0}Exception message: {1}", Environment.NewLine, this.mCurrentException.Message);
                info.AppendFormat("{0}Exception stack trace:", Environment.NewLine);
                info.AppendFormat("{0}{1}", Environment.NewLine, this.mCurrentException.StackTrace);
                info.Append(Environment.NewLine);

                return info.ToString();
            }

            #endregion
        }
    }
}