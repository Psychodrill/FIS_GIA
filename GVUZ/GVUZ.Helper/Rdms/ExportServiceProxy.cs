using System;
using System.ServiceModel;
using Rdms.Communication.Interface;

namespace GVUZ.Helper.Rdms
{
    internal class ExportServiceProxy : ClientBase<IExportService>, IExportService
    {
        public byte[] BuildPackage(int versionId)
        {
            return Channel.BuildPackage(versionId);
        }

        public byte[] GetByDate(int directoryId, DateTime date)
        {
            return Channel.GetByDate(directoryId, date);
        }
    }
}