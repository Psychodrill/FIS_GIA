using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using log4net;

namespace FogSoft.Helpers.Mail
{
    /// <summary>
    ///     Интерфейс для отправки почты.
    /// </summary>
    public interface ISmtpClient : IDisposable
    {
        string Host { get; set; }
        int Port { get; set; }
        ICredentialsByHost Credentials { get; set; }
        bool EnableSsl { get; set; }

        void Send(MailMessage message);
    }

    /// <summary>
    ///     Клиент для отправки почты по умолчанию (отправляет реальную почту реальным адресатам).
    /// </summary>
    public class DefaultSmtpClient : Disposable, ISmtpClient
    {
        protected SmtpClient _smtpClient = new SmtpClient();

        public ICredentialsByHost Credentials
        {
            get { return _smtpClient.Credentials; }
            set { _smtpClient.Credentials = value; }
        }

        public virtual void Send(MailMessage message)
        {
            _smtpClient.Send(message);
        }

        public string Host
        {
            get { return _smtpClient.Host; }
            set { _smtpClient.Host = value; }
        }

        public int Port
        {
            get { return _smtpClient.Port; }
            set { _smtpClient.Port = value; }
        }

        public bool EnableSsl
        {
            get { return _smtpClient.EnableSsl; }
            set { _smtpClient.EnableSsl = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _smtpClient != null)
            {
                _smtpClient.Dispose();
                _smtpClient = null;
            }
        }
    }

    /// <summary>
    ///     Тот же DefaultSmtpClient, но не блокирует при отправке.
    /// </summary>
    public class AsyncSmtpClient : DefaultSmtpClient
    {
        protected static Queue<MailMessage> mailQueque = new Queue<MailMessage>();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool isSending;

        public override void Send(MailMessage message)
        {
            lock (mailQueque)
            {
                mailQueque.Enqueue(message);
                if (!isSending)
                {
                    ThreadPool.QueueUserWorkItem(SendMailCallback, mailQueque);
                    isSending = true;
                }
            }
        }

        private void SendMailCallback(object state)
        {
            bool finished;
            do
            {
                MailMessage[] messages;
                lock (mailQueque)
                {
                    messages = new MailMessage[mailQueque.Count];
                    mailQueque.CopyTo(messages, 0);
                    mailQueque.Clear();
                }

                foreach (MailMessage message in messages)
                {
                    if (message != null && _smtpClient != null)
                    {
                        try
                        {
                            _smtpClient.Send(message);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error while sending mail", ex);
                        }
                        message.Dispose();
                    }
                }
                lock (mailQueque)
                {
                    finished = (mailQueque.Count <= 0);
                    isSending = finished;
                }
            } while (!finished);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }

    /// <summary>
    ///     Абстрактный клиент для отправки почты с реализацией свойств и пустым <see cref="Dispose" />.
    /// </summary>
    public abstract class AbstractSmtpClient : Disposable, ISmtpClient
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public ICredentialsByHost Credentials { get; set; }


        public abstract void Send(MailMessage message);

        protected override void Dispose(bool disposing)
        {
        }
    }

    /// <summary>
    ///     Клиент для профилирования отправки почты (выводит информацию в <see cref="Trace" />).
    /// </summary>
    public class ProfilingSmtpClient : AbstractSmtpClient
    {
        public override void Send(MailMessage message)
        {
            Trace.WriteLine(string.Format("'{0}' to '{1}'", message.Subject, message.To));
            Trace.WriteLine("----------------------------------------------------------");
            Trace.WriteLine(message.Body);
        }
    }
}