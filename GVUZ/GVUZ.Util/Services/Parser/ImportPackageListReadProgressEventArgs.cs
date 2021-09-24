using System;

namespace GVUZ.Util.Services.Parser
{
    public class ImportPackageListReadProgressEventArgs : EventArgs
    {
        public ImportPackageListReadProgressEventArgs(int totalPackages, int readPackages, int progress)
        {
            Progress = progress;
            ReadPackages = readPackages;
            TotalPackages = totalPackages;
        }

        public int TotalPackages { get; private set; }
        public int ReadPackages { get; private set; }
        public int Progress { get; private set; }
    }
}