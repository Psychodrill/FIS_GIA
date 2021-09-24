using System;

namespace GVUZ.Util.Services.Import
{
    public class ApplicationOrderRecordReaderEventArgs : EventArgs
    {
        public ApplicationOrderRecordReaderEventArgs(ApplicationOrderRecord record, int totalRecords)
        {
            TotalRecords = totalRecords;
            Record = record;
        }

        public ApplicationOrderRecord Record { get; private set; }
        public int TotalRecords { get; private set; }
    }
}