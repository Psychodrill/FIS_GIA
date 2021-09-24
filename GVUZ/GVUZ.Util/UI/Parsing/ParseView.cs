using System;
using System.Globalization;
using System.Windows.Forms;

namespace GVUZ.Util.UI.Parsing
{
    public partial class ParseView : UIViewBase, IParseView
    {
        private readonly ParseViewPresenter _presenter;

        public ParseView()
        {
            InitializeComponent();
            _presenter = new ParseViewPresenter(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
            {
                return;
            }

            lbTime.Text = null;
            _presenter.ViewLoaded();
        }

        public int MaxProgress
        {
            get { return pbParse.Maximum; }
            set { Sync(() => pbParse.Maximum = value); }
        }

        public int CurrentProgress
        {
            get { return pbParse.Value; }
            set { Sync(() => pbParse.Value = value); }
        }

        public string ParseStatusText
        {
            get { return lbProgress.Text; }
            set { Sync(() => lbProgress.Text = value); }
        }

        public void StartTimer()
        {
            Sync(() =>
                {
                    lbTime.Text = null;
                    tmImport.Tag = null;
                    tmImport.Start();        
                });
        }

        public void StopTimer()
        {
            Sync(() => tmImport.Stop());
        }

        public bool StartButtonEnabled
        {
            get { return tsiParseRun.Enabled; }
            set { Sync(() => tsiParseRun.Enabled = value); }
        }

        public bool StopButtonEnabled
        {
            get { return tsiParseAbort.Enabled; }
            set { Sync(() => tsiParseAbort.Enabled = value); }
        }

        public bool SetupButtonEnabled
        {
            get { return tsiParseSetup.Enabled; }
            set { Sync(() => tsiParseSetup.Enabled = value); }
        }

        public bool ContentVisible
        {
            get { return pnRunParse.Visible; }
            set { Sync(() => pnRunParse.Visible = value); }
        }

        private void tmImport_Tick(object sender, EventArgs e)
        {
            if (tmImport.Tag == null)
            {
                tmImport.Tag = DateTime.Now;
            }

            TimeSpan elapsed = DateTime.Now - (DateTime)(tmImport.Tag);
            lbTime.Text = elapsed.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }

        

        private void tsiParseRun_Click(object sender, EventArgs e)
        {
            _presenter.StartParse();
        }

        private void tsiParseAbort_Click(object sender, EventArgs e)
        {
            _presenter.StopParse();
        }

        private void tsiParseSetup_Click(object sender, EventArgs e)
        {
            _presenter.SetupParse();
        }
    }
}
