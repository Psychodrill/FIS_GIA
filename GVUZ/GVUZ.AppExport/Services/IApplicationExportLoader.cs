using System;

namespace GVUZ.AppExport.Services
{
    public interface IApplicationExportLoader
    {
        event EventHandler<ApplicationExportEventArgs> ApplicationFetched;
        void Load();
        int GetInstitutionEsrpId(long orgId);
    }
}