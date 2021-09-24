using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class UncheckedApplicationsQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "ApplicationId",
                "ApplicationNumber",
                "ViolationErrors",
                "StatusName",
                "LastCheckDate",
                "EntrantName",
                "IdentityDocument",
                "RegistrationDate",
                "IsRecommended"
            };


        public UncheckedApplicationsQueryViewModel()
            : base("RegistrationDate", true)
        {
        }

        public UncheckedApplicationsFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}