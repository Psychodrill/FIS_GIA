using System;
using System.Collections.Generic;

namespace GVUZ.AppExport.Services
{
    public interface IApplicationExportRequestRepository
    {
        IEnumerable<ApplicationExportRequest> FindByInstitution(long institutionId);
        IEnumerable<Guid> FetchNewId(int maxItems);
        ApplicationExportRequest FindByRequestId(Guid requestId);
        ApplicationExportRequest AddNew(long institutionId, int yearStart);
        void CommitState(IEnumerable<Guid> requestId, ApplicationExportRequestStatus status);
        void CommitState(Guid requestId, ApplicationExportRequestStatus status);
        void ResetIncomplete();
        bool HasPending(long institutionId, int yearStart);
    }
}