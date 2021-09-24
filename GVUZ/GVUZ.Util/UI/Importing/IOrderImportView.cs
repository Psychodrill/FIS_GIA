using System.Collections.Generic;
using System.IO;

namespace GVUZ.Util.UI.Importing
{
    public interface IOrderImportView : IUIViewBase
    {
        string ImportStatusText { get; set; }
        bool RunButtonEnabled { get; set; }
        bool AbortButtonEnabled { get; set; }
        int MaxProgress { get; set; }
        int CurrentProgress { get; set; }
        IEnumerable<string> LogText { get; }
        void AppendLog(string message);
        void ClearLog();
        void StartTimer();
        void StopTimer();
        FileInfo GetSavedLogFile();
    }
}