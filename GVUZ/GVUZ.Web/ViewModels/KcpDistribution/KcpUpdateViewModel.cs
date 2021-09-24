using GVUZ.Web.Controllers.Admission;

namespace GVUZ.Web.ViewModels.KcpDistribution
{
    public class KcpUpdateViewModel
    {
        public int AdmissionVolumeId { get; set; }
        public KcpBudgetLevelViewModel[] BudgetLevels { get; set; }

        public int AvailableForDistribution { get; set; }

        public int TotalDistributed { get; set; }
    }
}