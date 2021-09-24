using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class GeneralSubjectsPager : PagerBase<Subject, GeneralSubjectsViewModel>
    {
        protected override IQueryable<Subject> GetOrderedBy(IQueryable<Subject> queryable, int? sortId)
        {
            if (!sortId.HasValue)
                sortId = NameSortOrder; // by name

            switch (sortId.Value)
            {
                case 1:
                    return queryable.OrderBy(x => x.SubjectID);
                case -1:
                    return queryable.OrderByDescending(x => x.SubjectID);
                case -2:
                    return queryable.OrderByDescending(x => x.Name);
                default:
                    return queryable.OrderBy(x => x.Name);
            }
        }

        protected override void Load(IQueryable<Subject> queryable, GeneralSubjectsViewModel model)
        {
            model.GeneralSubjects = queryable.Select(
                x => new GeneralSubjectsViewModel.GeneralSubjectData
                    {
                        SubjectID = x.SubjectID,
                        Name = x.Name
                    }).ToArray();
        }
    }
}