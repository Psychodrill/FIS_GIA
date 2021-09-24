using System;
using Fbs.Core;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fbs.Web.Certificates.Entrants
{
    public partial class LoadEntrants : BasePage
    {
        private const string SuccessUri = "/Certificates/Entrants/LoadEntrantsSuccess.aspx";

        private string[] mFileLines;
        private string[] FileLines
        {
            get
            {
                if (mFileLines == null)
                {
                    // Получу строки файла
                    List<string> lines = new List<string>(Encoding.Default.GetString(fuData.FileBytes).Split(
                        new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));

                    // Уберу пробелы в начале и конце строк
                    for (int i = 0; i < lines.Count; i++)
                        lines[i] = lines[i].Trim();

                    // Удалю пустые строки
                    lines.RemoveAll(String.IsNullOrEmpty);

                    mFileLines = lines.ToArray();
                }
                return mFileLines;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                return;

            Page.Validate();
            if (Page.IsValid)
            {
                Entrant.LoadBatch(FileLines);
                Response.Redirect(SuccessUri, true);
            }
        }

        protected void cvFileEmpty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Проверю размер загружаемого файла.
            args.IsValid = fuData.PostedFile.ContentLength > 0 && FileLines.Length > 0;
        }

        protected void cvFileFormat_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (FileLines.Length == 0)
                return;

            int errorLine;
            Entrant.BatchLineFormatError error = Entrant.CheckBatch(FileLines, out errorLine);
            if (error != Entrant.BatchLineFormatError.None)
            {
                args.IsValid = false;
                cvFileFormat.ErrorMessage = String.Format(cvFileFormat.ErrorMessage,
                    Entrant.GetFormatErrorMsg(error), errorLine);
            }
        }
    }
}
