using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fbs.Web.CheckAuthService;
using Fbs.Core.Organizations;

namespace Fbs.Web.Controls
{
    public partial class OrgSelector : System.Web.UI.UserControl
    {
        /// <summary>
        /// Хранение информации о выбранной организации
        /// </summary>
        public string Value
        {
            get { return this.selectedOrg.Value; }
            set { this.selectedOrg.Value = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Ищем форму на которой лежит контрол - в нашем случае это будет FormView
        /// </summary>
        /// <returns>
        /// Возващает FormView в котором лежит контрол
        /// </returns>
        private Control GetCurrentFormView()
        {
            var currentControl = this.Parent;
            

            return currentControl;
        }

        /// <summary>
        /// The select organization_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void SelectOrganization_Click(object sender, EventArgs e)
        {
            string esrp = Config.UrlEsrp;
            string backUrl = RedirectManager.GetRedirectUrl(Request);

            string sid = Guid.NewGuid().ToString();
            this.SaveFormDataInSession(this.GetCurrentFormView(), sid, null);

            if (this.Page.Request.Url.Query != string.Empty)
            {
                backUrl = backUrl.Replace(this.Page.Request.Url.Query, string.Empty);
            }

            backUrl = string.Format("{0}?sid={1}&id={2}", backUrl, sid, this.Request["id"]);

            this.Response.Redirect(
                string.Format("{0}SelectOrg.aspx?BackUrl={1}", Config.UrlEsrp, HttpUtility.UrlEncode(backUrl)));
        }

        /// <summary>
        /// Восстановление данных формы
        /// </summary>
        /// <remarks>
        /// После того как пользователь выбрал организацию восстанавливаем значения в форме ввода данных
        /// </remarks>
        /// <param name="controlToLoad">
        /// Контрол в котором хранятся данные
        /// </param>
        /// <param name="sid">
        /// Идентификатор записи в хранилище
        /// </param>
        private void LoadFormDataFromSession(Control controlToLoad, string sid)
        {
            this.LoadFormDataFromSession(controlToLoad, sid, true);
        }

        /// <summary>
        /// Восстановление данных формы с очисткой сессии
        /// </summary>
        /// <remarks>
        /// После того как пользователь выбрал организацию восстанавливаем значения в форме ввода данных
        /// </remarks>
        /// /// <param name="controlToLoad">
        /// Контрол в котором хранятся данные
        /// </param>
        /// <param name="sid">
        /// Идентификатор записи в хранилище
        /// </param>
        /// <param name="clearSession">
        /// Очищать ли сессию после чтения из нее данных
        /// </param>
        private void LoadFormDataFromSession(Control controlToLoad, string sid, bool clearSession)
        {
            var formValues = this.Session[sid] as Dictionary<string, object>;

            foreach (Control control in controlToLoad.Controls)
            {
                if (!string.IsNullOrEmpty(control.ClientID) && formValues.ContainsKey(control.ClientID))
                {
                    if (control is TextBox)
                    {
                        (control as TextBox).Text = (string)formValues[control.ClientID];
                    }
                    else if (control is CheckBox)
                    {
                        (control as CheckBox).Checked = (bool)formValues[control.ClientID];
                    }
                    else if (control is DropDownList)
                    {
                        (control as DropDownList).SelectedValue = (string)formValues[control.ClientID];
                    }
                    else if (control is HiddenField)
                    {
                        (control as HiddenField).Value = (string)formValues[control.ClientID];
                    }
                }

                this.LoadFormDataFromSession(control, sid, false);
            }

            if (clearSession)
            {
                this.Session.Remove(sid);
            }
        }

        /// <summary>
        /// Сохранение данных формы
        /// </summary>
        /// <remarks>
        /// Сохраняем данные формы в сессии на то время пока пользователь выбирает организацию из списка
        /// </remarks>
        /// <param name="controlToSave">
        /// Контрол в котором хранятся данные
        /// </param>
        /// <param name="sid">
        /// Идентификатор записи в сессии
        /// </param>
        private void SaveFormDataInSession(Control controlToSave, string sid)
        {
            this.SaveFormDataInSession(controlToSave, sid, null);
        }

        /// <summary>
        /// Сохранение данных формы
        /// </summary>
        /// <remarks>
        /// Сохраняем данные формы в сессии на то время пока пользователь выбирает организацию из списка
        /// </remarks>
        /// <param name="controlToSave">
        /// Контрол в котором хранятся данные
        /// </param>
        /// <param name="sid">
        /// Идентификатор записи в сессии
        /// </param>
        /// <param name="defaultStorage">
        /// Хранилище для значений
        /// </param>
        private void SaveFormDataInSession(Control controlToSave, string sid, Dictionary<string, object> defaultStorage)
        {
            Dictionary<string, object> storage;
            if (defaultStorage == null)
            {
                storage = new Dictionary<string, object>();
            }
            else
            {
                storage = defaultStorage;
            }

            foreach (Control control in controlToSave.Controls)
            {
                if (!string.IsNullOrEmpty(control.ClientID))
                {
                    if (control is TextBox)
                    {
                        storage.Add(control.ClientID, (control as TextBox).Text);
                    }
                    else if (control is CheckBox)
                    {
                        storage.Add(control.ClientID, (control as CheckBox).Checked);
                    }
                    else if (control is DropDownList)
                    {
                        storage.Add(control.ClientID, (control as DropDownList).SelectedValue);
                    }
                    else if (control is HiddenField)
                    {
                        storage.Add(control.ClientID, (control as HiddenField).Value);
                    }

                    this.SaveFormDataInSession(control, sid, storage);
                }
            }

            if (defaultStorage == null)
            {
                this.Session[sid] = storage;
            }
        }
        /// <summary>
        /// Завершающая обработка элементов перед их отрисовкой
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.IsPostBack)
            {
                // Если непустое значение - мы выбирали организацию и состояние страницы нужно восстановить
                string sid = this.Page.Request["sid"];
                if (!string.IsNullOrEmpty(sid) && this.Session[sid] != null)
                {
                    var currentForm = this.GetCurrentFormView();
                    this.LoadFormDataFromSession(currentForm, sid);
                }

                int orgId = 0;
                if (!string.IsNullOrEmpty(this.Page.Request["OrgID"]) &&
                    int.TryParse(this.Page.Request["OrgID"], out orgId))
                {
                    // Актуализируем данные о выбранной организации
                    this.UpdateOrganization(orgId);
                   
                   this.selectedOrg.Value =orgId.ToString();
                   this.selectedOrgName.Text = OrganizationDataAccessor.Get(orgId).FullName;
                }
            }

           
        }

        /// <summary>
        /// Обновляем данные организации из esrp
        /// </summary>
        /// <param name="id">Идентификатор организации</param>
        protected void UpdateOrganization(int id)
        {
            AccountDataUpdater updatesData = new AccountDataUpdater(this.Page.User.Identity.Name);
            updatesData.ActualizeOrganizationData(id);
        }

    }
}