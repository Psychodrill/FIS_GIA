using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument
{
    internal class IdentityDocumentCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public IdentityDocumentCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) {}

        public override IEnumerable<IBulkEntity> Collect()
        {
            foreach (var app in _collection.Where(c => c.ApplicationDocuments.IdentityDocument != null && c.Entrant != null))
                app.ApplicationDocuments.IdentityDocument.GenderTypeID = app.Entrant.GenderID;

            return _collection.Where(c => c.ApplicationDocuments.IdentityDocument != null)
                    .Select(c => c.ApplicationDocuments.IdentityDocument.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id));
        }
    }
}