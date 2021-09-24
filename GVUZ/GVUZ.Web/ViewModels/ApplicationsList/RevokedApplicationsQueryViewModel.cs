using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RevokedApplicationsQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "ApplicationId",
                "ApplicationNumber",
                "LastDenyDate",
                "EntrantName",
                "IdentityDocument",
                "RegistrationDate",
                "IsRecommended"
            };


        public RevokedApplicationsQueryViewModel()
            : base("RegistrationDate", true)
        {
        }

        public RevokedApplicationsFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}