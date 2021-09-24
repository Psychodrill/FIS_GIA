using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class AcceptedApplicationsQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "ApplicationId",
                "ApplicationNumber",
                "StatusName",
                "LastCheckDate",
                "EntrantName",
                "IdentityDocument",
                "OriginalDocumentsReceived",
                "RegistrationDate",
                "IsRecommended"
            };


        public AcceptedApplicationsQueryViewModel()
            : base("RegistrationDate", true)
        {
        }

        public AcceptedApplicationsFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}