using System;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportRequestMonitorEventArgs : EventArgs
    {
        public ApplicationExportRequestMonitorEventArgs(Guid[] requestId)
        {
            RequestId = requestId;
        }

        public Guid[] RequestId { get; private set; }
    }
}