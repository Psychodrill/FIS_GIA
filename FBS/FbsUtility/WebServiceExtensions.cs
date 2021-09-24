namespace Fbs.Utility
{
    using System;
    using System.Configuration;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;

    /// <summary>
    /// расширения клиентов веб сервисов
    /// </summary>
    public static class WebServiceExtensions
    {
        /// <summary>
        /// реализовать вызов веб сервиса через прокси. 
        /// Логин\пароль для прокси берутся из конфига (proxyLogin, proxyPassword)
        /// </summary>
        /// <typeparam name="T">
        /// тип клиента
        /// </typeparam>
        /// <param name="client">
        /// клиент веб сервиса
        /// </param>
        /// <returns>
        /// скоуп контекста вызова веб сервиса (должен быть выбзван dispose при завершении вызова)
        /// </returns>
        public static OperationContextScope ThroughProxy<T>(this ClientBase<T> client) where T : class
        {
            var basicBinding = client.Endpoint.Binding as BasicHttpBinding;
            if (basicBinding == null || basicBinding.ProxyAddress == null || string.IsNullOrEmpty(basicBinding.ProxyAddress.AbsoluteUri))
            {
                return null;
            }

            var scope = new OperationContextScope(client.InnerChannel);
            var httpRequestProperty = new HttpRequestMessageProperty();
            var proxyLogin = ConfigurationManager.AppSettings["proxyLogin"];
            var proxyPassword = ConfigurationManager.AppSettings["proxyPassword"];

            httpRequestProperty.Headers[System.Net.HttpRequestHeader.ProxyAuthorization] = "Basic " +
                Convert.ToBase64String(Encoding.ASCII.GetBytes(proxyLogin + ":" + proxyPassword));
            httpRequestProperty.Headers["Proxy-Connection"] = "keep-alive";

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
            return scope;
        }
    }
}