using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GVUZ.Util.UI.Parsing
{
    public partial class ParseSettingsForm : Form
    {
        public ParseSettingsForm()
        {
            Properties.Settings.Default.Reload();
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
            {
                return;
            }
            
            ClearDirty();
            Properties.Settings.Default.PropertyChanged += SettingsPropertyChanged;
        }

        private bool _isDirty;

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            ClearDirty();
            DialogResult = DialogResult.OK;
        }

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetDirty();
        }

        private void SetDirty()
        {
            _isDirty = true;
            btnSave.Enabled = true;
        }

        private void ClearDirty()
        {
            _isDirty = false;
            btnSave.Enabled = false;
        }

        private void ParseSettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.None &&  // кнопка закрытия с DialogResult (CancelButton формы)
                e.CloseReason != CloseReason.UserClosing)
            {
                Properties.Settings.Default.PropertyChanged -= SettingsPropertyChanged;
                return;
            }

            if (_isDirty)
            {
                DialogResult confirmSave = MessageBox.Show(this, "Сохранить изменения настроек?", "Настройки изменились", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (confirmSave == DialogResult.Yes)
                {
                    Properties.Settings.Default.Save();
                }
                else if (confirmSave == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                Properties.Settings.Default.PropertyChanged -= SettingsPropertyChanged;
                Properties.Settings.Default.Reload();
            }
        }
    }
}
