namespace Ege.Check.App.Web.Common.Formatters
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using ServiceStack.Text;
    using JsonSerializer = ServiceStack.Text.JsonSerializer;

    class ServiceStackTextJsonMediaFormatter : MediaTypeFormatter
    {
        public ServiceStackTextJsonMediaFormatter()
        {
            JsConfig.DateHandler = DateHandler.ISO8601;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return Task.FromResult(DeserializeFromStream(type, readStream));
        }

        private object DeserializeFromStream(Type type, Stream readStream)
        {
            try
            {
                return JsonSerializer.DeserializeFromStream(type, readStream);
            }
            catch
            {
                return null;
            }
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            JsonSerializer.SerializeToStream(value, type, writeStream);
            return Task.FromResult(0);
        }
    }
}
