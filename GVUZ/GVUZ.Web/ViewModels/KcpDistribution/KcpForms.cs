namespace GVUZ.Web.ViewModels.KcpDistribution
{
    public interface IKcpFormsViewModel
    {
    }

    public class KcpForms<TForm> where TForm : class, IKcpFormsViewModel, new()
    {
        private TForm _o;
        private TForm _oz;
        private TForm _z;

        public TForm O
        {
            get { return _o ?? (_o = new TForm()); }
            set { _o = value; }
        }

        public TForm OZ
        {
            get { return _oz ?? (_oz = new TForm()); }
            set { _oz = value; }
        }

        public TForm Z
        {
            get { return _z ?? (_z = new TForm()); }
            set { _z = value; }
        }
    }
}