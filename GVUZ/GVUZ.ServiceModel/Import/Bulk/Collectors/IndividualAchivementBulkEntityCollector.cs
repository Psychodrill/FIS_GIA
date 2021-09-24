using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using FogSoft.Helpers;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class IndividualAchivementBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, IndividualAchivementBulkEntity>
    {
        public IndividualAchivementBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) { }

        public override IEnumerable<Model.Base.IBulkEntity> Collect()
        {
            return _collection
                    .Aggregate(new List<IndividualAchivementBulkEntity>(), (total, next) =>
                    {
                        if (next.IndividualAchievements != null)
                        {
                            total.AddRange(next.IndividualAchievements.Select(group => new IndividualAchivementBulkEntity
                            {
                                ParentId = next.Id,
                                UID = "",
                                ImportPackageId = _packageId,
                                InstitutionId = _institutionId,
                                ApplicationUID = next.UID,
                                EntrantDocumentUID = group.IADocumentUID,
                                IAMark = string.IsNullOrEmpty(group.IAMark) ? (decimal?)null : GetMark(group.IAMark),
                                IAName = group.IAName,
                                IAUID = group.IAUID,
                                isAdvantageRight = group.isAdvantageRight
                            }));
                            return total;
                        }
                        else return new List<IndividualAchivementBulkEntity>();
                    });
        }

        private decimal GetMark(string mark)
        {
            decimal result;
            try
            {
                result = decimal.Parse(mark);
            }
            catch
            {
                string markFormatted = mark.Contains(".") ? mark.Replace(".", ",") : mark.Replace(",", ".");
                try
                {
                    result = decimal.Parse(markFormatted);
                }
                catch
                {
                    throw;
                }
            }

            return result;
        }
    }
}
