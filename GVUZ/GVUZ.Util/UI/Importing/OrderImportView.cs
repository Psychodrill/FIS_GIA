using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace GVUZ.Util.UI.Importing
{
    public partial class OrderImportView : UIViewBase, IOrderImportView
    {
        private readonly OrderImportViewPresenter _presenter;

        public OrderImportView()
        {
            InitializeComponent();
            _presenter = new OrderImportViewPresenter(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
            {
                return;
            }

            lbImportTime.Text = null;
            _presenter.ViewLoaded();
        }

        public string ImportStatusText
        {
            get
            {
                return lbImportStatus.Text;
            }
            set
            {
                Sync(() => lbImportStatus.Text = value);
            }
        }

        public void AppendLog(string message)
        {
            Sync(() => { tbImportLog.AppendText(message);tbImportLog.AppendText(Environment.NewLine); });
        }

        public void ClearLog()
        {
            Sync(() => tbImportLog.Clear());
        }

        public void StartTimer()
        {
            Sync(() => { 
                tmImport.Tag = null;
                tmImport.Start();
            });
        }

        public bool RunButtonEnabled
        {
            get { return tsiImportRun.Enabled; }
            set
            {
                Sync(() => tsiImportRun.Enabled = value);
            }
        }

        public bool AbortButtonEnabled
        {
            get { return tsiImportAbort.Enabled; }
            set
            {
                Sync(() => tsiImportAbort.Enabled = value);
            }
        }

        public void StopTimer()
        {
            Sync(() => tmImport.Stop());
        }

        public FileInfo GetSavedLogFile()
        {
            if (DialogResult.OK == saveLogFileDialog.ShowDialog())
            {
                return new FileInfo(saveLogFileDialog.FileName);
            }

            return null;
        }

        public int MaxProgress
        {
            get { return pbImport.Maximum; }
            set { Sync(() => pbImport.Maximum = value);}
        }

        public int CurrentProgress
        {
            get { return pbImport.Value; }
            set
            {
                Sync(() => pbImport.Value = value);
            }
        }

        public IEnumerable<string> LogText
        {
            get { return tbImportLog.Lines; }
        }

        private void tmImport_Tick(object sender, EventArgs e)
        {
            if (tmImport.Tag == null)
            {
                tmImport.Tag = DateTime.Now;
            }

            TimeSpan elapsed = DateTime.Now - (DateTime)(tmImport.Tag);
            lbImportTime.Text = elapsed.ToString(@"hh\:mm\:ss", CultureInfo.InvariantCulture);
        }

        private void tsiImportRun_Click(object sender, EventArgs e)
        {
            _presenter.StartImport();
        }

        private void tsiImportAbort_Click(object sender, EventArgs e)
        {
            _presenter.StopImport();
        }

        private void tsiImportLogSave_Click(object sender, EventArgs e)
        {
            _presenter.SaveLog();
        }

    }
}
