using System;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportEventArgs : EventArgs
    {
        public ApplicationExportEventArgs(ApplicationExportDto data)
        {
            Data = data;
        }

        public ApplicationExportDto Data { get; private set; }
    }
}