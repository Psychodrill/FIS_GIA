namespace GVUZ.Util.UI
{
    public interface IUIViewBase
    {
        void InformationDialog(string text, string caption);
        void WarningDialog(string text, string caption);
        void ErrorDialog(string text, string caption);
        bool GetUserConfirmation(string question);
    }
}