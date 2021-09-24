using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.ViewModels.Shared
{
    public class EducationFormsSelectList : SelectListViewModel<EducationFormId>
    {
        public EducationFormsSelectList()
        {
            foreach (var form in EducationFormsHelper.EducationFormNames)
            {
                    var id = new EducationFormId { FormId = form.Key};
                    Items.Add(new SelectListItemViewModel<EducationFormId> { Id = id, DisplayName = id.ToString() });
            }
        }
     }
}