using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ApplicationOrderQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "Id",
                "ApplicationNumber",
                "EntrantName",
                "IdentityDocument",
                "EducationLevelName",
                "EducationFormName",
                "EducationSourceName",
                "CompetitiveGroupName",
                "DirectionName",
                "Benefit",
                "CompetitiveGroupTargetName",
                "Rating"
            };


        public ApplicationOrderQueryViewModel()
            : base(ValidSortKeys[0])
        {
        }

        public ApplicationOrderFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}