using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class EntranceTestsPager : PagerBase<DirectionSubjectLink, EntranceTestsViewModel>
    {
        protected override IQueryable<DirectionSubjectLink> GetOrderedBy(IQueryable<DirectionSubjectLink> queryable,
                                                                         int? sortId)
        {
            return queryable.OrderBy(x => x.ID);
        }

        protected override void Load(IQueryable<DirectionSubjectLink> queryable, EntranceTestsViewModel model)
        {
            DirectionSubjectLink[] links = queryable.ToArray();
            model.Tests = new EntranceTestsViewModel.TestData[links.Length];
            int i = 0;
            foreach (DirectionSubjectLink directionSubjectLink in links)
            {
                model.Tests[i++] = new EntranceTestsViewModel.TestData
                    {
                        ID = directionSubjectLink.ID,
                        Directions = directionSubjectLink.DirectionSubjectLinkDirection
                                                         .Select(x => x.Direction.Name).ToArray(),
                        Subjects = directionSubjectLink.DirectionSubjectLinkSubject
                                                       .Select(x => x.Subject.Name).ToArray()
                    };
            }
        }
    }
}