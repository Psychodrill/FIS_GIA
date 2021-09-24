using System;
using Esrp.SelfIntegration.TargetESRPClient;

namespace Esrp.SelfIntegration.ReplicationClient
{
    internal class ESRPServiceClientFactory
    {
        public string SessionId { get; private set; }
        public SetData Client { get; private set; }

        private string login_;
        private string password_;
        private string serviceUrl_;

        public ESRPServiceClientFactory(string serviceUrl)
        {
            if (String.IsNullOrEmpty(serviceUrl))
                throw new ArgumentException("serviceUrl");

            serviceUrl_ = serviceUrl;
        }

        public void CreateClient()
        {
            Client = new SetData() { Url = serviceUrl_, Timeout = 1000 * 300 };
        }
    }
}
