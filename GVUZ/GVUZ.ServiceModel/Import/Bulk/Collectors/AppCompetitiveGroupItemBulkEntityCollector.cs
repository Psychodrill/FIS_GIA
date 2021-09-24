using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.Bulk.Model;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class AppCompetitiveGroupItemBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, ApplicationCompetitiveGroupItemBulkEntity>
    {
        public AppCompetitiveGroupItemBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) { }

        public override IEnumerable<Model.Base.IBulkEntity> Collect()
        {
            return _collection
                    .Aggregate(new List<ApplicationCompetitiveGroupItemBulkEntity>(), (total, next) =>
                    {
                        total.AddRange(next.NewSourcesAndForms.Select(group => new ApplicationCompetitiveGroupItemBulkEntity
                        {
                            ParentId = next.Id,
                            UID = "",
                            ImportPackageId = _packageId,
                            InstitutionId = _institutionId,
                            ApplicationUID = group.ApplicationUID,
                            CompetitiveGroupItemUID = group.CompetitiveGroupItemUID,
                            CompetitiveGroupTargetUID = group.CompetitiveGroupTargetUID,
                            CompetitiveGroupUID = group.CompetitiveGroupUID,
                            EducationForm = group.EducationForm,
                            EducationSource = group.EducationSource,
                            Priority = group.Priority
                        }));
                        return total;
                    });
        }
    }
}
