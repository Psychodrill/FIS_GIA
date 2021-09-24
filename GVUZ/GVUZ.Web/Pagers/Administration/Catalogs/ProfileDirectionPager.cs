using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class ProfileDirectionPager : PagerBase<EntranceTestProfileDirection, ProfileDirectionViewModel>
    {
        protected override IQueryable<EntranceTestProfileDirection> GetOrderedBy(
            IQueryable<EntranceTestProfileDirection> queryable, int? sortId)
        {
            if (!sortId.HasValue)
                sortId = NameSortOrder;

            //switch (sortId.Value)
            //{
            //    case 1:
            //        return queryable.OrderBy(x => x.InstitutionID);
            //    case -1:
            //        return queryable.OrderByDescending(x => x.InstitutionID);
            //    case -2:
            //        return queryable.OrderByDescending(x => x.Institution.FullName);
            //    case 2:
            //        return queryable.OrderBy(x => x.Institution.FullName);
            //    default:
            //        throw new ArgumentException("sortId");
            //}
            throw new ArgumentException("sortId");
        }

        // todo: optimize
        protected override void Load(IQueryable<EntranceTestProfileDirection> queryable, ProfileDirectionViewModel model)
        {
            int? pageNumber = model.PageNumber;
            if (!pageNumber.HasValue || pageNumber < 0)
                pageNumber = 0;

            //IEnumerable<IGrouping<int, Institution>> institutions = queryable.Select(x => x.Institution)
            //                                                                 .ToArray()
            //                                                                 .GroupBy(x => x.InstitutionID)
            //                                                                 .Skip(pageNumber.Value*PageSize)
            //                                                                 .Take(PageSize);

            ////int i = 0;
           // model.Directions = new ProfileDirectionViewModel.InstitutionProfileDirectionData[institutions.Count()];
            //foreach (var institution in institutions)
            //{
            //    DirectionData[] directionDataArray = institution.First()
            //                                                    .EntranceTestProfileDirection.Select(
            //                                                        x => new DirectionData
            //                                                            {
            //                                                                DirectionID = x.DirectionID,
            //                                                                Name = x.Direction.Name,
            //                                                                Code = x.Direction.Code
            //                                                            }).ToArray();

            //    model.Directions[i++] =
            //        institution.Select(x => new ProfileDirectionViewModel.InstitutionProfileDirectionData
            //            {
            //                Directions = directionDataArray,
            //                InstitutionID = x.InstitutionID,
            //                Name = x.FullName
            //            }).First();
            //}
        }

        protected override int GetCount(IQueryable<EntranceTestProfileDirection> queryable)
        {
            return PageSize + 1;
           // return (Math.Max(queryable.GroupBy(x => x.InstitutionID).Count(), 1) - 1)/PageSize + 1;
        }

        protected override bool UseSkipping()
        {
            return false;
        }
    }
}