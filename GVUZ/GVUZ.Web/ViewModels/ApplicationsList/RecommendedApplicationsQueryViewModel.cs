using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RecommendedApplicationsQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "ApplicationId",
                "ApplicationNumber",
                "CampaignId",
                "StageName",
                "EntrantName",
                "EducationLevelName",
                "EducationFormName",
                "CompetitiveGroupName",
                "OriginalDocumentsReceived",
                "Rating"
            };


        public RecommendedApplicationsQueryViewModel() 
            : base(ValidSortKeys[1])
        {
        }

        public RecommendedApplicationsFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}