namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
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
    /// Проверка свидетельства по паспортным данным и оценкам
    /// </summary>
    public partial class RequestByPassportForOpenedFbs : BasePage
    {
        #region Constants and Fields

        private const int MaxSubjectMark = 100;

        private const int MinSubjectMark = 0;

        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/RequestByPassportResultForOpenedFbs.aspx?Series={0}&Number={1}&SubjectMarks={2}";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The validate.
        /// </summary>
        public override void Validate()
        {
            base.Validate();

            List<string> seriesErrors = DocumentCheck.DocSeriesCheck(this.txtSeries.Text);
            if (seriesErrors.Count > 0)
            {
                this.vlEnchancedPassportSeries.IsValid = false;
                this.vlEnchancedPassportSeries.ErrorMessage = string.Join("<br />", seriesErrors.ToArray());
            }

            List<string> numberErrors = DocumentCheck.DocNumberCheck(this.txtNumber.Text);
            if (numberErrors.Count > 0)
            {
                this.vlEnchancedPassportNumber.IsValid = false;
                this.vlEnchancedPassportNumber.ErrorMessage = string.Join("<br />", numberErrors.ToArray());
            }
        }

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
            
            // Выйду если постбэк или нет объекта в сесии
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

        /// <summary>
        /// Обработчик события нажатия кнопки "Проверить"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void BtnSearchClick(object sender, EventArgs e)
        {
            if (!this.Page.IsValid)
            {
                return;
            }

            var marks = new StringBuilder();
            int countMarks = 0;

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
                    if (float.TryParse(
                        value.Replace(',', '.'), NumberStyles.Float, NumberFormatInfo.InvariantInfo, out mark)
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

            // Сохраню состояния контролов
            StateManager.AddEntry(this.txtSeries.ID, this.txtSeries.Text.FullTrim());
            StateManager.AddEntry(this.txtNumber.ID, this.txtNumber.Text.FullTrim());
            StateManager.SaveState();

           // Перейду на страницу результатов (поиска).
            this.Response.Redirect(
                string.Format(
                    SearchUrl,
                    HttpUtility.UrlEncode(this.txtSeries.Text.FullTrim()),
                    HttpUtility.UrlEncode(this.txtNumber.Text.FullTrim()),
                    marks));
        }

        #endregion
    }
}