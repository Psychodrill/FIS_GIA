using System;
using Esrp.EIISIntegration.EIISClient;
using Esrp.EIISIntegration.Import.MessageParsers;

namespace Esrp.EIISIntegration.Import
{
    internal class EIISClientFactory
    {
        public string SessionId { get; private set; }
        public BaseService Client { get; private set; }

        private string login_;
        private string password_;
        private string serviceUrl_;

        public EIISClientFactory(string serviceUrl, string login, string password)
        {
            if (String.IsNullOrEmpty(serviceUrl))
                throw new ArgumentException("serviceUrl");
            if (String.IsNullOrEmpty(login))
                throw new ArgumentException("login");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("password");

            serviceUrl_ = serviceUrl;
            login_ = login;
            password_ = password;
        }

        public void OpenSession()
        {
            Client = new BaseService() { Url = serviceUrl_, Timeout = 1000 * 300 };
            string getSessionIdResponse = Client.GetSessionId(login_, password_);

            GetSessionIdParser getSessionIdParser = new GetSessionIdParser(getSessionIdResponse);
            if (getSessionIdParser.ResponseIsError)
                throw new ImportException("Ошибка инициализации сессии: " + getSessionIdParser.ErrorDescription);

            SessionId = getSessionIdParser.SessionId;
        }
    }
}
