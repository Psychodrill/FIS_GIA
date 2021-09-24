using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class ForeignLanguagesPager : PagerBase<ForeignLanguageType, ForeignLanguagesViewModel>
    {
        protected override IQueryable<ForeignLanguageType> GetOrderedBy(IQueryable<ForeignLanguageType> queryable,
                                                                        int? sortId)
        {
            if (!sortId.HasValue)
                sortId = NameSortOrder; // by name

            switch (sortId.Value)
            {
                case 1:
                    return queryable.OrderBy(x => x.LanguageID);
                case -1:
                    return queryable.OrderByDescending(x => x.LanguageID);
                case -2:
                    return queryable.OrderByDescending(x => x.Name);
                default:
                    return queryable.OrderBy(x => x.Name);
            }
        }

        protected override void Load(IQueryable<ForeignLanguageType> queryable, ForeignLanguagesViewModel model)
        {
            model.ForeignLanguages = queryable.Select(
                x => new ForeignLanguagesViewModel.ForeignLanguageData
                    {
                        LanguageID = x.LanguageID,
                        Name = x.Name
                    }).ToArray();
        }
    }
}