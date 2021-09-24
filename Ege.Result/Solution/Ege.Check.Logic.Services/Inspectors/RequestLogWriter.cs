namespace Ege.Check.Logic.Services.Inspectors
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;
    using Common.Logging;
    using JetBrains.Annotations;

    class RequestLogWriter : IRequestLogWriter
    {
        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<RequestLogWriter>();

        private readonly string _requestLogPath;
        private readonly bool _shouldWrite;

        public RequestLogWriter()
        {
            _requestLogPath = ConfigurationManager.AppSettings["RequestLogStorage"];
            _shouldWrite = !string.IsNullOrEmpty(_requestLogPath);
            if (_requestLogPath != null && _shouldWrite && !Directory.Exists(_requestLogPath))
            {
                Directory.CreateDirectory(_requestLogPath);
            }
        }

        public async Task Log(MemoryStream stream, Type dto, Guid responseId)
        {
            if (!_shouldWrite)
            {
                return;
            }
            if (stream == null)
            {
                Logger.WarnFormat("Decompressed stream is not memory stream, not logging (response id {0})", responseId);
                return;
            }
            var directory = Path.Combine(_requestLogPath, DateTime.Today.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (_requestLogPath != null)
            {
                var fileName = Path.Combine(directory, string.Format("{0}-{1}.xml", dto.Name, responseId));
                using (var requestLogFile = File.OpenWrite(fileName))
                {
                    await stream.CopyToAsync(requestLogFile);
                    Logger.TraceFormat("Request written to {0}", fileName);
                }
                stream.Position = 0;
            }
        }
    }
}
