using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.CompositionResults
{
    public class CompositionResultsQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "LastName",
                "FirstName",
                "MiddleName",
                "IdentityDocumentNumber",
                "CompositionCode",
                "CompositionTitle",
                "CompositionResult",
                "RegistrationDate"
            };


        public CompositionResultsQueryViewModel()
            : base(ValidSortKeys[7])
        {
        }

        public CompositionResultsFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}