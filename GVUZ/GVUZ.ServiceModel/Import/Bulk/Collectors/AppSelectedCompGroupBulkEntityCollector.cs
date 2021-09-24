using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class AppSelectedCompGroupBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, AppSelectedCompGroupBulkEntity>
    {
        public AppSelectedCompGroupBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) { }

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection
                    .Aggregate(new List<AppSelectedCompGroupBulkEntity>(), (total, next) =>
                    {
                        total.AddRange(next.SelectedCompetitiveGroups.Select(group => new AppSelectedCompGroupBulkEntity
                        {
                            ParentId = next.Id,
                            UID = group,
                            ImportPackageId = _packageId,
                            InstitutionId = _institutionId,
                            CalculatedRating = next.CalcSelectedCompetitiveGroupRating(group),
                            CalculatedBenefitId = next.GetCalculatedBenefitId(group)
                        }));
                        return total;
                    });
        }
    }
}
