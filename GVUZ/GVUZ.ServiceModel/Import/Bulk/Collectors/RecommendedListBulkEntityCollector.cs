using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class RecommendedListBulkEntityCollector : BulkEntityCollectorBase<RecommendedListDto, RecommendedListBulkEntity>, IDisposable
    {
        public RecommendedListBulkEntityCollector(IEnumerable<RecommendedListDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId)
        {
        }

        public override IEnumerable<Model.Base.IBulkEntity> Collect()
        {
            List<RecommendedListBulkEntity> result = new List<RecommendedListBulkEntity>();

            foreach (var item in _collection)
            {
                foreach (var recList in item.RecLists)
                {
                    foreach (var sourceForm in recList.FinSourceAndEduForms)
                    {
                        RecommendedListBulkEntity element = new RecommendedListBulkEntity()
                        {
                            Stage = item.Stage,
                            ApplicationNumber = recList.Application.ApplicationNumber,
                            RegistrationDate = recList.Application.RegistrationDate,
                            CompetitiveGroupUID = sourceForm.CompetitiveGroupID,
                            DirectionId = sourceForm.DirectionID,
                            EduFormId = sourceForm.EducationFormID,
                            EduLevelId = sourceForm.EducationLevelID,
                            ImportPackageId = _packageId,
                            InstitutionId = _institutionId
                        };

                        result.Add(element);
                    }
                }
            }

            return result;
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
