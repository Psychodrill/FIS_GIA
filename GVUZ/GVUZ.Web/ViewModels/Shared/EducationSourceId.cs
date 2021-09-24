using GVUZ.Web.Helpers;
using GVUZ.Web.SQLDB;

namespace GVUZ.Web.ViewModels.Shared
{
    public class EducationSourceId
    {
        public short SourceId { get; set; }

        public override string ToString()
        {
            return EducationFormsHelper.GetDisplaySource(SourceId);
        }

        public override int GetHashCode()
        {
            return SourceId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            EducationSourceId other = obj as EducationSourceId;

            if (other == null)
            {
                return false;
            }

            return SourceId == other.SourceId;
        }
    }
}