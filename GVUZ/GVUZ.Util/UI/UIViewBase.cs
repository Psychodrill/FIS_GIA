using System;
using System.Windows.Forms;

namespace GVUZ.Util.UI
{
    public class UIViewBase : UserControl, IUIViewBase
    {
        public void InformationDialog(string text, string caption)
        {
            DisplayDialog(text, caption, MessageBoxIcon.Information);
        }

        public void WarningDialog(string text, string caption)
        {
            DisplayDialog(text, caption, MessageBoxIcon.Warning);
        }

        public void ErrorDialog(string text, string caption)
        {
            DisplayDialog(text, caption, MessageBoxIcon.Error);
        }

        public bool GetUserConfirmation(string question)
        {
            return DisplayDialogResult(question, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void DisplayDialog(string text, string title, MessageBoxIcon icon)
        {
            DisplayDialogResult(text, title, MessageBoxButtons.OK, icon);
        }

        private DialogResult DisplayDialogResult(string text, string title, MessageBoxButtons buttons,
                                                 MessageBoxIcon icon)
        {
            return MessageBox.Show(this, text, title, buttons, icon);
        }

        protected void Sync(Action methodInvoker)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(methodInvoker));
            }
            else
            {
                methodInvoker();
            }
        }
    }
}