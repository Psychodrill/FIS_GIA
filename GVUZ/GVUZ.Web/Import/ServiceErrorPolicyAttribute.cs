using System;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.Web.Import.Infractrusture.Exceptions;

namespace GVUZ.Web.Import
{
    class ValidationErrorBodyWriter : BodyWriter
    {
        private Exception validationException;
        Encoding utf8Encoding = new UTF8Encoding(false);

        public ValidationErrorBodyWriter(Exception validationException)
            : base(true)
        {
            this.validationException = validationException;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("root");
            writer.WriteAttributeString("type", "object");

            writer.WriteStartElement("ErrorMessage");
            writer.WriteAttributeString("type", "string");
            writer.WriteString(this.validationException.Message);
            writer.WriteEndElement();

            writer.WriteStartElement("MemberNames");
            writer.WriteAttributeString("type", "array");
            foreach (var member in this.validationException.Message)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("type", "string");
                writer.WriteString(member.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }

	/// <summary>
	/// Атрибут, помогающий логировать ошибки с сервисом (внутренней инфраструктуры WCF)
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface)]
	public class ServiceErrorPolicyBehaviorAttribute : Attribute, IContractBehavior, IErrorHandler
    {
		public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
		{
			//LogHelper.LogCurrentDomainUnhandledException(null, new UnhandledExceptionEventArgs(error, false));

            var prop = new HttpResponseMessageProperty {StatusCode = HttpStatusCode.OK};
		    prop.Headers[HttpResponseHeader.ContentType] = "application/xml; charset=utf-8";

		    var message = error is IImportException ? error.ToString() : error.Message;
            fault = Message.CreateMessage(version, null, XmlImportHelper.GenerateErrorElement(message));
            fault.Properties.Add(HttpResponseMessageProperty.Name, prop);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Xml));
		}

		public bool HandleError(Exception error)
		{
            if (error is ImportXmlValidationException) return false;
		    
            LogHelper.LogCurrentDomainUnhandledException(null, new UnhandledExceptionEventArgs(error, false));
			return false;
        }

		public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
		{
		}

		public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
			dispatchRuntime.ChannelDispatcher.ErrorHandlers.Add(this);
        }

		public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
		}

		public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}
    }
}