namespace Ege.Check.Logic.Services.ErrorHandling
{
    using System;
    using System.ServiceModel.Channels;
    using Common.Logging;
    using JetBrains.Annotations;

    internal class LoggingErrorHandler : ILoggingErrorHandler
    {
        [NotNull] private static readonly ILog Log = LogManager.GetLogger<LoggingErrorHandler>();

        private static LoggingErrorHandler _instance;

        protected LoggingErrorHandler()
        {
        }

        public static LoggingErrorHandler Instance
        {
            get { return _instance ?? (_instance = new LoggingErrorHandler()); }
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Log.Error(error);
        }

        public bool HandleError(Exception error)
        {
            Log.Error(error);
            return false;
        }
    }
}
