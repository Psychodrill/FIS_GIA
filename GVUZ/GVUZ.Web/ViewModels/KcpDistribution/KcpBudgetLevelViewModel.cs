namespace GVUZ.Web.ViewModels.KcpDistribution
{
    public class KcpBudgetLevelViewModel
    {
        private KcpForms<KcpFieldViewModel> _budget;
        private KcpForms<KcpFieldViewModel> _quota;
        private KcpForms<KcpFieldViewModel> _target;

        public int BudgetLevelId { get; set; }
        public int DistributedAdmissionVolumeId { get; set; }

        public KcpForms<KcpFieldViewModel> Budget
        {
            get { return _budget ?? (_budget = new KcpForms<KcpFieldViewModel>()); }
            set { _budget = value; }
        }

        public KcpForms<KcpFieldViewModel> Quota
        {
            get { return _quota ?? (_quota = new KcpForms<KcpFieldViewModel>()); }
            set { _quota = value; }
        }

        public KcpForms<KcpFieldViewModel> Target
        {
            get { return _target ?? (_target = new KcpForms<KcpFieldViewModel>()); }
            set { _target = value; }
        }
    }
}