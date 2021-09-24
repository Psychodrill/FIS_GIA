namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.UI.WebControls;

    using Fbs.Core.Organizations;
    using Fbs.Core.Shared;
    using Fbs.Core.UICheckLog;
    using Fbs.Utility;

    using WebControls;

    /// <summary>
    /// Проверка сертификатов по номеру и оценкам
    /// </summary>
    public partial class CheckForOpenedFbs : BasePage
    {
        #region Constants and Fields

        private const int MaxSubjectMark = 100;

        private const int MinSubjectMark = 0;

        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/CheckResultForOpenedFbs.aspx?"
            + "number={0}&SubjectMarks={1}";

        #endregion

        #region Methods

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Выйду если постбэк или нет объекта в сессии
            if (this.Page.IsPostBack || this.Session[UINavigation.SessionId] == null)
            {
                return;
            }

            // Получу состояние контрола навигации
            var state = (UserNavigatorState)this.Session[UINavigation.SessionId];

            // Если происходит переход назад по экшену Back, то восстановлю сохраненные состояния 
            // контролов, иначе удалю сохраненные состояния 
            if (state.OkBack)
            {
                try
                {
                    StateManager.RestoreState(this.Page);
                }
                catch
                {
                    StateManager.ClearState();
                }
            }
            else
            {
                StateManager.ClearState();
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Проверить"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void BtnCheckClick(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            var marks = new StringBuilder();
            var countMarks = 0;

            // Получу введенные пользователем баллы.
            foreach (RepeaterItem item in this.rpSubjects.Items)
            {
                var hiddenFieldId = item.FindControl("hfId") as HiddenField;
                var txtValue = item.FindControl("txtValue") as TextBox;
                string value;
                if (hiddenFieldId != null && txtValue != null && !string.IsNullOrEmpty(value = txtValue.Text.Trim()))
                {
                    float mark;

                    // Проверю правильность введенного значения (число от 0 до 100).
                    if (float.TryParse(value.Replace(',', '.'), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out mark)
                        && mark >= MinSubjectMark && mark <= MaxSubjectMark)
                    {
                        marks.AppendFormat("{0}={1},", hiddenFieldId.Value, mark.ToString(CultureInfo.InvariantCulture));
                        ++countMarks;
                    }
                    else
                    {
                        this.cvSubjectMarks.IsValid = false;
                        return;
                    }
                }
            }

            // Пользователь ввел баллы меньше чем по 2 предметам
            if (countMarks < 2)
            {
                cvSubjectMarksEmpty.IsValid = false;
                return;
            }

            // Сохраню состояния контролов
            StateManager.AddEntry(this.txtNumber.ID, this.txtNumber.Text.FullTrim());
            StateManager.AddEntry(this.rpSubjects.ID, marks.ToString());
            StateManager.SaveState();

            // Перейду на страницу результатов (поиска).
            this.Response.Redirect(
                string.Format(
                    SearchUrl,
                    HttpUtility.UrlEncode(this.txtNumber.Text.FullTrim()),
                    HttpUtility.UrlEncode(marks.ToString().Trim(','))));
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Очистить"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// параметры события
        /// </param>
        protected void BtnResetClick(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.RawUrl);
        }

        #endregion
    }
}