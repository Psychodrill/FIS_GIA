namespace GVUZ.Util.UI.Parsing
{
    public interface IParseView : IUIViewBase
    {
        int MaxProgress { get; set; }
        int CurrentProgress { get; set; }
        string ParseStatusText { get; set; }
        void StartTimer();
        void StopTimer();
        bool StartButtonEnabled { get; set; }
        bool StopButtonEnabled { get; set; }
        bool SetupButtonEnabled { get; set; }
        bool ContentVisible { get; set; }
    }
}