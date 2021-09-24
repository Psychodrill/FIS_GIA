namespace Ege.Check.Logic.Services.Attributes
{
    using System;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using Ege.Check.Logic.Services.ErrorHandling;
    using Ege.Check.Logic.Services.Inspectors;

    [AttributeUsage(AttributeTargets.Class)]
    public class LoggingServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                channelDispatcher.ErrorHandlers.Add(LoggingErrorHandler.Instance);
                //foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                //{
                //    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(LogRequestMessageInspector.Instance);
                //}
            }
        }
    }
}
