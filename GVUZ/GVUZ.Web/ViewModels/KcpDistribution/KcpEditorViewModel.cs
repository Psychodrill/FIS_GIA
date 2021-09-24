namespace GVUZ.Web.ViewModels.KcpDistribution
{
    public class KcpEditorViewModel
    {
        private KcpLimitsViewModel _limits;
        public int AdmissionVolumeId { get; set; }
        public int AvailableDistributionPoints { get; set; }
        public int DistributedPoints { get; set; }
        public KcpLimitsViewModel Limits
        {
            get { return _limits ?? (_limits = new KcpLimitsViewModel()); }
            set { _limits = value; }
        }

        public KcpBudgetLevelViewModel[] BudgetLevels { get; set; }
    }
}