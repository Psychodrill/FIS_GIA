namespace GVUZ.Web.Pagers.Administration.Catalogs
{
    //public class QualificationsPager : PagerBase<QualificationType, QualificationsViewModel>
    //{		
    //    protected override IQueryable<QualificationType> GetOrderedBy(IQueryable<QualificationType> queryable, int? sortId)
    //    {
    //        if (!sortId.HasValue)
    //            sortId = NameSortOrder; // by name ( 2 )

    //        switch (sortId.Value)
    //        {
    //            case 1:
    //                return queryable.OrderBy(x => x.QualificationID);
    //            case -1:
    //                return queryable.OrderByDescending(x => x.QualificationID);
    //            case -2:
    //                return queryable.OrderByDescending(x => x.Name);
    //            case 3:
    //                return queryable.OrderBy(x => x.Code);
    //            case -3:
    //                return queryable.OrderByDescending(x => x.Code);
    //            default:
    //                return queryable.OrderBy(x => x.Name);
    //        }
    //    }

    //    protected override void Load(IQueryable<QualificationType> queryable, QualificationsViewModel model)
    //    {
    //        model.Qualifications = queryable.Select(
    //            x => new QualificationsViewModel.QualificationData()
    //            {
    //                QualificationID = x.QualificationID,
    //                Name = x.Name,
    //                Code = x.Code
    //            }).ToArray();
    //    }		
    //}
}