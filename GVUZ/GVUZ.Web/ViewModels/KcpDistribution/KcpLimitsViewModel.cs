namespace GVUZ.Web.ViewModels.KcpDistribution
{
    public class KcpLimitsViewModel
    {
        private KcpForms<KcpLimitViewModel> _budget;
        private KcpForms<KcpLimitViewModel> _quota;
        private KcpForms<KcpLimitViewModel> _target;

        public KcpForms<KcpLimitViewModel> Budget
        {
            get { return _budget ?? (_budget = new KcpForms<KcpLimitViewModel>()); }
            set { _budget = value; }
        }

        public KcpForms<KcpLimitViewModel> Quota
        {
            get { return _quota ?? (_quota = new KcpForms<KcpLimitViewModel>()); }
            set { _quota = value; }
        }

        public KcpForms<KcpLimitViewModel> Target
        {
            get { return _target ?? (_target = new KcpForms<KcpLimitViewModel>()); }
            set { _target = value; }
        }
    }
}