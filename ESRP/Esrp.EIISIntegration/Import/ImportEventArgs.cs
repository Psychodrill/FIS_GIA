using System;

namespace Esrp.EIISIntegration.Import
{
    internal class ImportEventArgs : EventArgs
    {
        public string ImporterName { get; private set; }
        public string ImporterCode { get; private set; }
        public string Data { get; private set; }

        public ImportEventArgs(string importerName, string importerCode)
            : this(importerName, importerCode, null)
        {
        }

        public ImportEventArgs(string importerName, string importerCode, string data)
        {
            ImporterName = importerName;
            ImporterCode = importerCode;
            Data = data;
        }
    }
}
