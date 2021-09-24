using System.ServiceModel;
using FogSoft.Helpers;
using Rdms.Communication.Exceptions;
using Rdms.Communication.Interface;

namespace GVUZ.Helper.Rdms
{
    internal static class ConnectionFactory
    {
        public static string Username { get; set; }
        public static string Password { get; set; }

        private static void InitProxy<T>(ClientBase<T> proxy, string contractName) where T : class
        {
            proxy.ChannelFactory.Credentials.UserName.UserName = Username;
            proxy.ChannelFactory.Credentials.UserName.Password = Password;

            try
            {
                proxy.ChannelFactory.Endpoint.Address = new EndpointAddress(
                    AppSettings.Get("ConnectionFactory.Address", "").TrimEnd('/') + "/" + contractName);
            }
            catch
            {
                throw new FaultException<IllegaServerException>(
                    new IllegaServerException());
            }
        }

        public static IVersionService GetVersionService()
        {
            var proxy = new VersionServiceProxy();
            InitProxy(proxy, "VersionService.svc");
            return proxy;
        }

        public static IExportService GetExportService()
        {
            var proxy = new ExportServiceProxy();
            InitProxy(proxy, "ExportService.svc");
            return proxy;
        }
    }
}