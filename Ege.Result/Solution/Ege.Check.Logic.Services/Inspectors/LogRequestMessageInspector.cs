namespace Ege.Check.Logic.Services.Inspectors
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using Common.Logging;
    using JetBrains.Annotations;

    public class LogRequestMessageInspector : IDispatchMessageInspector
    {
        [NotNull]
        public static IDispatchMessageInspector Instance = new LogRequestMessageInspector();

        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<LogRequestMessageInspector>();

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            //MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            //request = buffer.CreateMessage();
            //Logger.TraceFormat("Received:\n{0}", buffer.CreateMessage().ToString());
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}
