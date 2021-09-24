using System;
using System.Globalization;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using WebControls;
using Fbs.Utility;
using Fbs.Core.Organizations;
using Fbs.Core.UICheckLog;
using Fbs.Web.Helpers;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class RequestByMarks : BasePage, IHistoryNavigator
    {
        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/RequestByMarksResultCommon.aspx?" +
            "LastName={0}&FirstName={1}&PatronymicName={2}&SubjectMarks={3}&Ev={4}";

        private const int MinSubjectMark = 0;
        private const int MaxSubjectMark = 100;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Выйду если постбэк или нет объекта в сесии
            if (Page.IsPostBack || Session[UINavigation.SessionId] == null)
                return;

            // Получу состояние контрола навигации
            UserNavigatorState state = (UserNavigatorState)Session[UINavigation.SessionId];

            // Если происходит переход назад по экшену Back, то восстановлю сохраненные состояния 
            // контролов, иначе удалю сохраненные состояния 
            if (state.OkBack)
            {
                try
                {
                    StateManager.RestoreState(this.Page);
                }
                catch { StateManager.ClearState(); }
            }
            else
                StateManager.ClearState();
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            var marks = new StringBuilder();

            var MarkWasSet = false;

            // Получу введенные пользователем баллы.
            foreach (RepeaterItem item in rpSubjects.Items)
            {
                var hfId = item.FindControl("hfId") as HiddenField;
                var txtValue = item.FindControl("txtValue") as TextBox;
                string value;
                if (hfId != null && txtValue != null && !string.IsNullOrEmpty(value = txtValue.Text.Trim().ToLower()))
                {
                    float mark = 0;

                    // Проверю правильность введенного значения (число от 0 до 100 или 1/0/зачет/незачет для сочинений и изложений).
                    if ((SubjectsHelper.SubjectHasBoolMark(hfId.Value)
                        && (SubjectsHelper.IsBoolMarkValue(value)))
                        ||
                        ((!SubjectsHelper.SubjectHasBoolMark(hfId.Value))
                        && (float.TryParse(value.Replace(',', '.'),
                                       NumberStyles.Float,
                                       NumberFormatInfo.InvariantInfo,
                                       out mark))) && mark >= MinSubjectMark && mark <= MaxSubjectMark)
                    {
                        if (SubjectsHelper.SubjectHasBoolMark(hfId.Value))
                        {
                            mark = SubjectsHelper.BoolMarkFromText(value); 
                        }
                        marks.AppendFormat("{0}={1},", hfId.Value, mark.ToString(CultureInfo.InvariantCulture));
                        MarkWasSet = true;
                    }
                    else
                    {
                        cvSubjectMarks.IsValid = false;
                        return;
                    }
                }
            }

            if (!MarkWasSet) // Пользователь не ввел баллов
            {
                cvSubjectMarks.IsValid = false;
                return;
            }

            // Сохраню состояния контролов.
            StateManager.AddEntry(txtLastName.ID, txtLastName.Text);
            StateManager.AddEntry(txtFirstName.ID, txtFirstName.Text);
            StateManager.AddEntry(txtPatronymicName.ID, txtPatronymicName.Text);
            StateManager.AddEntry(rpSubjects.ID, marks.ToString());
            StateManager.SaveState();

            // Зарегистритую событие
            var login = HttpContext.Current.User.Identity.Name;
            var org = OrganizationDataAccessor.GetByLogin(login);
            if ((org != null && org.DisableLog == false) || org == null)
            {
                var eventId = CheckLogDataAccessor.AddMarksCheckEvent(
                    User.Identity.Name,
                    txtLastName.Text.Trim(),
                    txtFirstName.Text.Trim(),
                    txtPatronymicName.Text.Trim(),
                    marks.ToString());

                // Перейду на страницу результатов (поиска).
                Response.Redirect(string.Format(SearchUrl,
                    HttpUtility.UrlEncode(txtLastName.Text.Trim()),
                    HttpUtility.UrlEncode(txtFirstName.Text.Trim()),
                    HttpUtility.UrlEncode(txtPatronymicName.Text.Trim()),
                    HttpUtility.UrlEncode(marks.ToString().Trim(",".ToCharArray())),
                    HttpUtility.UrlEncode(eventId.ToString())));
            }
            else
            {
                // Перейду на страницу результатов (поиска).
                Response.Redirect(string.Format(SearchUrl,
                    HttpUtility.UrlEncode(txtLastName.Text.Trim()),
                    HttpUtility.UrlEncode(txtFirstName.Text.Trim()),
                    HttpUtility.UrlEncode(txtPatronymicName.Text.Trim()),
                    HttpUtility.UrlEncode(marks.ToString().Trim(",".ToCharArray())),
                    string.Empty));
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void CheckNameValue(object source, ServerValidateEventArgs args)
        {
            args.IsValid = User.IsInRole("CheckCommonNationalCertificateExtended") ||
                args.Value.Length > 0;
        }

        protected void cvChoiceExt_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = !User.IsInRole("CheckCommonNationalCertificateExtended") ||
                txtLastName.Text.Length > 0 || txtFirstName.Text.Length > 0;
        }


        public string GetPageName()
        {
            return "RequestByMarksHistoryResultCommon.aspx";
        }
    }
}
