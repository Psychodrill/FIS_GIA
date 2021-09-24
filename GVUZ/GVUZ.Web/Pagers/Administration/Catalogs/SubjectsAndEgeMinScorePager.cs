using System;
using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.Administration.Catalogs;

namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    public class SubjectsAndEgeMinScorePager : PagerBase<Subject, SubjectsAndEgeMinScoreViewModel>
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
                case 2:
                    return queryable.OrderBy(x => x.Name);
                case -3:
                    return queryable.OrderByDescending(x => x.SubjectEgeMinValue.FirstOrDefault().MinValue);
                case 3:
                    return queryable.OrderBy(x => x.SubjectEgeMinValue.FirstOrDefault().MinValue);
                default:
                    throw new ArgumentException("sortId");
            }
        }

        protected override void Load(IQueryable<Subject> queryable, SubjectsAndEgeMinScoreViewModel model)
        {
            model.SubjectsAndScores =
                queryable.Select(
                    x => new SubjectsAndEgeMinScoreViewModel.ScoreData
                        {
                            SubjectID = x.SubjectID,
                            SubjectName = x.Name,
                            MinValue = x.SubjectEgeMinValue.FirstOrDefault().MinValue
                        })
                         .ToArray();
        }
    }
}