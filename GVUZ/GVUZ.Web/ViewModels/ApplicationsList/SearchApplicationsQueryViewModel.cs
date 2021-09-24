using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class SearchApplicationsQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "ApplicationId",
                "ApplicationNumber",
                "RegistrationDate",
                "StatusName",
                "EntrantName",
                "IdentityDocument"
            };


        public SearchApplicationsQueryViewModel()
            : base("RegistrationDate", true)
        {
        }

        public SearchApplicationsFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}