using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class OlympicsPager : PagerBase<OlympicType, OlympicsViewModel>
    {
        protected override IQueryable<OlympicType> GetOrderedBy(IQueryable<OlympicType> queryable, int? sortId)
        {
            if (!sortId.HasValue)
                sortId = NameSortOrder; // by name

            switch (sortId.Value)
            {
                case 1:
                    return queryable.OrderBy(x => x.OlympicID);
                case -1:
                    return queryable.OrderByDescending(x => x.OlympicID);
                case -2:
                    return queryable.OrderByDescending(x => x.Name);
                case 3:
                    return queryable.OrderBy(x => x.OlympicNumber);
                case -3:
                    return queryable.OrderByDescending(x => x.OlympicNumber);
                //case 4:
                //    return queryable.OrderBy(x => x.OrganizerName);
                //case -4:
                //    return queryable.OrderByDescending(x => x.OrganizerName);
                //case 5:
                //    return queryable.OrderBy(x => x.OlympicLevelID);
                case 6:
                    return queryable.OrderBy(x => x.OlympicYear);
                //case -5:
                //    return queryable.OrderByDescending(x => x.OlympicLevelID);
                default:
                    return queryable.OrderBy(x => x.Name);
            }
        }

        protected override void Load(IQueryable<OlympicType> queryable, OlympicsViewModel model)
        {
            model.Olympics = queryable.ToList().Select(x => new OlympicsViewModel.OlympicData
                {
                    //OlympicID = x.OlympicID,
                    //Name = x.Name,
                    //OlympicYear = x.OlympicYear,
                    //OlympicNumber = x.OlympicNumber,
                    //OrganizerName = x.OrganizerName,
                    //OlympicLevelID = x.OlympicLevelID,
                    //OlympicLevelName = x.OlympicLevel != null
                    //                       ? x.OlympicLevel.Name
                    //                       : string.Join(", ", x.OlympicTypeSubjectLink
                    //                                            .Select(
                    //                                                c =>
                    //                                                c.OlympicLevel != null ? c.OlympicLevel.Name : "?")
                    //                                            .Distinct().OrderBy(c => c).ToArray())
                }).ToArray();
        }
    }
}