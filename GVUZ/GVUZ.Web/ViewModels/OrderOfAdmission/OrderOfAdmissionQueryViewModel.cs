using System;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
            {
                "OrderId",
                "OrderName",
                "OrderNumber",
                "OrderDate",
                "OrderStatusId",
                "CampaignName",
                "Stage",
                "EducationLevel", 
                "EducationForm",
                "EducationSource",
                "NumberOfApplicants",
                "IsForBeneficiary",
                "IsForeigner"
            };


        public OrderOfAdmissionQueryViewModel()
            : base(ValidSortKeys[1])
        {
        }

        public OrderOfAdmissionFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}