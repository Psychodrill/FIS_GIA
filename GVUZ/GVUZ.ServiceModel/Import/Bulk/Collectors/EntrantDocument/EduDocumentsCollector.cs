using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument
{
    internal class EduDocumentsCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public EduDocumentsCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) {}

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.Where(c => c.ApplicationDocuments.EduDocuments != null)
                    .Select(c => c.ApplicationDocuments.EduDocuments
                    .Aggregate(new List<ApplicationDocumentDto>(), (total, next) =>
                    {
                        if (next.AcademicDiplomaDocument != null) total.Add(next.AcademicDiplomaDocument);
                        if (next.BasicDiplomaDocument != null) total.Add(next.BasicDiplomaDocument);
                        if (next.HighEduDiplomaDocument != null) total.Add(next.HighEduDiplomaDocument);
                        if (next.IncomplHighEduDiplomaDocument != null) total.Add(next.IncomplHighEduDiplomaDocument);
                        if (next.MiddleEduDiplomaDocument != null) total.Add(next.MiddleEduDiplomaDocument);
                        if (next.SchoolCertificateBasicDocument != null) total.Add(next.SchoolCertificateBasicDocument);
                        if (next.SchoolCertificateDocument != null) total.Add(next.SchoolCertificateDocument);
                        if (next.EduCustomDocument != null) total.Add(next.EduCustomDocument);
                        if (next.PostGraduateDiplomaDocument != null) total.Add(next.PostGraduateDiplomaDocument);
                        if (next.PhDDiplomaDocument != null) total.Add(next.PhDDiplomaDocument);
                        return total;
                    })
                    .Select(x => x.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id)))
                    .Aggregate(new List<EntrantDocumentBulkEntity>(), (total, next) =>
                    {
                        total.AddRange(next);
                        return total;
                    });
        }
    }
}