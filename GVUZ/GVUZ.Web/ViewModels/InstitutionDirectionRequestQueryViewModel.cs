using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels
{
    public class InstitutionDirectionRequestQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "InstitutioName",
                "NumRequests",
                "LastRequestDate",
            };


        public InstitutionDirectionRequestQueryViewModel()
            : base(ValidSortKeys[0])
        {
        }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}