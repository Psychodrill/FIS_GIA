using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Principal;
using System.Reflection;
using System.Web;

namespace Esrp.Utility
{
    /// <summary>
    /// Класс с набором функций, упрощающих запись лога сообщений.
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// 
        /// </summary>
        private static log4net.ILog mLog = log4net.LogManager.GetLogger(typeof(LogManager));

        /// <summary>
        /// Печать сообщения об ошибке вида "* Error: имя_исключения, сообщение, трасса стека".
        /// </summary>
        /// <param name="ex"> Исключение </param>
        public static void Error(Exception ex)
        {
            try
            {
                ErrorInfo info = new ErrorInfo(ex);
                mLog.Error(String.Format("* Error: {0}", info.ToString()));
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
        /// <param name="message"> Сообщение </param>        
        public static void Info(string message)
        {
            try
            {
                mLog.Info(String.Format(". {0}", message));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Info", innerEx);
            }
        }
        /// <summary>
        /// Печать предупреждения вида "* Warning: сообщение".
        /// </summary>
        /// <param name="message">Строка для печати</param>        
        public static void Warning(string message)
        {
            try
            {
                mLog.Warn(String.Format("! Warning: {0}", message));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Warning", innerEx);
            }
        }

        public static void Warning(Exception ex)
        {
            try
            {
                mLog.Warn(String.Format("! Warning: {0}", ex.ToString()));
            }
            catch (Exception innerEx)
            {
                throw new ApplicationException("LogManager.Warning", innerEx);
            }
        }

        #region Внутренний класс ErrorInfo
        /// <summary>
        /// Составляет форматированное подробное описание ошибки.
        /// </summary>
        private class ErrorInfo
        {
            private Exception mCurrentException;
            private string _message;

            public ErrorInfo(Exception currentException)
            {
                mCurrentException = currentException;
            }

            public ErrorInfo(string message)
            {
                _message = message;
            }

            private string CurrentUserName
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.User == null)
                        return null;

                    if (String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                        return "anonymous";

                    return HttpContext.Current.User.Identity.Name;
                }
            }

            private string CurrentUserIp
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null)
                        return null;

                    return HttpContext.Current.Request.UserHostAddress;
                }
            }

            private string SourcePageUrl
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null)
                        return null;

                    return HttpContext.Current.Request.Url.ToString();
                }
            }

            private string ReferrerPageUrl
            {
                get
                {
                    if (HttpContext.Current == null || HttpContext.Current.Request == null
                            || HttpContext.Current.Request.UrlReferrer == null)
                        return null;

                    return HttpContext.Current.Request.UrlReferrer.ToString();
                }
            }

            /// <summary>
            /// Возвращает подробную информацию об ошибке.
            /// </summary>
            public override string ToString()
            {
                StringBuilder info = new StringBuilder();

                info.Append(mCurrentException.Message);
                if (CurrentUserName != null)
                    info.AppendFormat("{0}User name: {1}", Environment.NewLine, CurrentUserName);
                if (CurrentUserIp != null)
                    info.AppendFormat("{0}User ip: {1}", Environment.NewLine, CurrentUserIp);
                if (SourcePageUrl != null)
                    info.AppendFormat("{0}Source page: {1}", Environment.NewLine, SourcePageUrl);
                if (ReferrerPageUrl != null)
                    info.AppendFormat("{0}Source referrer: {1}", Environment.NewLine, ReferrerPageUrl);

                if (mCurrentException != null)
                {
                    info.AppendFormat("{0}Exception type: {1}", Environment.NewLine, mCurrentException.GetType().FullName);
                    info.AppendFormat("{0}Exception message: {1}", Environment.NewLine, mCurrentException.Message);
                    info.AppendFormat("{0}Exception stack trace:", Environment.NewLine);
                    info.AppendFormat("{0}{1}", Environment.NewLine, mCurrentException.StackTrace);
                }
                else if (!String.IsNullOrEmpty(_message))
                    info.AppendFormat("{0}Message: {1}", Environment.NewLine, _message);
                info.Append(Environment.NewLine);

                return info.ToString();
            }
        }

        #endregion
    }
}