using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Shared
{
    
    public class EducationSourceSelectList : SelectListViewModel<EducationSourceId>
    {
        public EducationSourceSelectList()
        {
            foreach (var source in EducationFormsHelper.EducationSourceNames)
            {
                var id = new EducationSourceId { SourceId = source.Key };
                Items.Add(new SelectListItemViewModel<EducationSourceId> { Id = id, DisplayName = id.ToString() });
            }
        }
    }
}