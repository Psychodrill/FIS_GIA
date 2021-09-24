using GVUZ.Web.Helpers;
using GVUZ.Web.SQLDB;

namespace GVUZ.Web.ViewModels.Shared
{
    public class EducationFormId
    {
        public short FormId { get; set; }

        public override string ToString()
        {
            return EducationFormsHelper.GetDisplayForm(FormId);
        }

        public override int GetHashCode()
        {
            return FormId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            EducationFormId other = obj as EducationFormId;

            if (other == null)
            {
                return false;
            }

            return FormId == other.FormId;
        }
    }
}