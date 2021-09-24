using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Fbs.Core;

namespace Fbs.Web.Certificates.Entrants
{
    public partial class BatchCheck : System.Web.UI.Page
    {
        private const string BatchLineRegex = @"^\d{2}-\d{9}-\d{2}$";
        private const string SuccessUri = "/Certificates/Entrants/BatchCheckSuccess.aspx?id={0}";

        private string[] mFileLines;
        private string[] FileLines
        {
            get
            {
                if (mFileLines == null)
                {
                    mFileLines = Encoding.Default.GetString(fuData.FileBytes).Split(
                        new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < mFileLines.Length; i++)
                        mFileLines[i] = mFileLines[i].Trim();
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
                // Преобразую данные из файла в строку
                string data = Encoding.Default.GetString(fuData.FileBytes);

                // Добавлю данные в бд
                long id = CommonNationalCertificateContext.UpdateEntrantCheckBatch(data, CurrentUser.ClietnLogin);

                Response.Redirect(String.Format(SuccessUri, id));
            }
        }

        protected void cvFileEmpty_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = fuData.PostedFile.ContentLength > 0 && FileLines.Length > 0;
        }

        protected void cvFileFormat_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (FileLines.Length == 0)
                return;

            int errorLine;
            if (!CheckFileFormat(out errorLine))
            {
                args.IsValid = false;
                cvFileFormat.ErrorMessage = String.Format(cvFileFormat.ErrorMessage, errorLine);
            }
        }

        private bool CheckFileFormat(out int errorLine)
        {
            string[] lines = FileLines;
            for (int lineNum =0; lineNum < lines.Length; lineNum++)
                if (!Regex.IsMatch(lines[lineNum], BatchLineRegex))
                {
                    errorLine = lineNum+1;
                    return false;
                }
            errorLine = -1;
            return true;
        }
    }
}
