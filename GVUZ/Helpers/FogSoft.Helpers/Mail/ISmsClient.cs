using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FogSoft.Helpers.Mail
{
    /// <summary>
    ///     Интерфейс для отправки SMS.
    /// </summary>
    /// <remarks>
    ///     Формирование пакета для отправки и отслееживание состояния описано здесь:
    ///     http://www.docs-group.ru/services/xml_api.doc
    /// </remarks>
    public interface ISmsClient : IDisposable
    {
        string Host { get; set; }
        string Sender { get; set; }
        string Login { get; set; }
        string Password { get; set; }

        /// <summary>
        ///     Отправляет пакет сообщений клиенту, может вызвать <see cref="SmsException" /> в случае проблем.
        /// </summary>
        void Send(IEnumerable<SmsMessage> messages);
    }

    /// <summary>
    ///     Абстрактный клиент для отправки SMS с реализацией свойств.
    /// </summary>
    public abstract class AbstractSmsClient : Disposable, ISmsClient
    {
        public abstract void Send(IEnumerable<SmsMessage> messages);

        public string Host { get; set; }

        public string Sender { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        protected override void Dispose(bool disposing)
        {
        }
    }

    /// <summary>
    ///     Клиент-заглушка для отправки SMS (ничего не делает).
    /// </summary>
    public class SmsClientStub : AbstractSmsClient
    {
        public override void Send(IEnumerable<SmsMessage> messages)
        {
        }
    }

    /// <summary>
    ///     Клиент для профилирования отправки SMS (выводит информацию в <see cref="Trace" />).
    /// </summary>
    public class ProfilingSmsClient : AbstractSmsClient
    {
        public override void Send(IEnumerable<SmsMessage> messages)
        {
            Trace.WriteLine(string.Format("SMS package from '{0}' via '{1}'", Sender, Host));
            foreach (SmsMessage message in messages)
                Trace.WriteLine(string.Format("'{0}' to {1}", message.Text, message.PhoneNumber));
            Trace.WriteLine("----------------------------------------------------------");
        }
    }
}