using System;
using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class CreativeDirectionPager : PagerBase<EntranceTestCreativeDirection, CreativeDirectionViewModel>
    {
        protected override IQueryable<EntranceTestCreativeDirection> GetOrderedBy(
            IQueryable<EntranceTestCreativeDirection> queryable, int? sortId)
        {
            if (!sortId.HasValue)
                sortId = NameSortOrder; // by name

            switch (sortId.Value)
            {
                case 1:
                    return queryable.OrderBy(x => x.DirectionID);
                case -1:
                    return queryable.OrderByDescending(x => x.DirectionID);
                case -2:
                    return queryable.OrderByDescending(x => x.Direction.Name);
                case 2:
                    return queryable.OrderBy(x => x.Direction.Name);
                default:
                    throw new ArgumentException("sortId");
            }
        }

        protected override void Load(IQueryable<EntranceTestCreativeDirection> queryable,
                                     CreativeDirectionViewModel model)
        {
            model.Directions =
                queryable.Select(
                    x => new DirectionData
                        {
                            DirectionID = x.DirectionID,
                            Name = x.Direction.Name,
                            Code = x.Direction.Code
                        })
                         .ToArray();
        }
    }
}