using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fbs.Utility
{
    /// <summary>
    /// Элемент состояния
    /// </summary>
    [Serializable()]
    public class StateEntry
    {
        /// <summary>
        /// Id контрола
        /// </summary>
        public string Id;

        /// <summary>
        /// Значение контрола
        /// </summary>
        public object Value;

        public StateEntry(string id, object value)
        {
            this.Id = id;
            this.Value = value;
        }
    }

    /// <summary>
    /// Хранение состояния контролов страницы
    /// </summary>
    public class StateManager
    {
        #region Private properties

        private const string SessionKey = "_state_manager_{76D7CD24-9256-4379-B517-CFD485070EBA}";

        private static List<StateEntry> mStateObject = new List<StateEntry>();

        private static HttpSessionState Session
        {
            get
            {
                if (HttpContext.Current == null)
                    throw new NullReferenceException("HttpContext.Current");

                return HttpContext.Current.Session;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление записи в коллекцию состояний
        /// </summary>
        /// <param name="id">Идентификатор контрола</param>
        /// <param name="value">Значение контрола</param>
        public static void AddEntry(string id, object value)
        {
            mStateObject.Add(new StateEntry(id, value));
        }

        /// <summary>
        /// Добавление записи в коллекцию состояний
        /// </summary>
        /// <param name="entry">Объект типа StateEntry</param>
        public static void AddEntry(StateEntry entry)
        {
            mStateObject.Add(entry);
        }

        /// <summary>
        /// Очистка объекта состояния
        /// </summary>
        public static void ClearState()
        {
            if (Session[SessionKey] != null)
                Session[SessionKey] = null;
        }

        /// <summary>
        /// Сохранение объекта состояний
        /// </summary>
        public static void SaveState()
        {
            SaveSessionObject(mStateObject);
        }
        #endregion

        /// <summary>
        /// Восстановление состояний контролов
        /// </summary>
        /// <param name="page">Объект текущей страницы</param>
        public static void RestoreState(Page page)
        {
            // Получу объект из сесии
            List<StateEntry> state = GetSessionObject();

            // Выйду если в стэйте нет элементов
            if (state.Count == 0)
                return;

            foreach (StateEntry entry in state)
            {
                Control ctl = page.Form.FindControl(entry.Id);
                if (ctl != null)
                {
                    // Восстановлю значения в зависимости от типа контрола
                    if (ctl is CheckBox)
                        ((CheckBox)ctl).Checked = (bool)entry.Value;
                    else if (ctl is TextBox)
                        ((TextBox)ctl).Text = (string)entry.Value;
                    else if (ctl is Repeater)
                        RestoreRepeaterState(entry, (Repeater)ctl);
                    else if (ctl is ListControl )
                        ((ListControl)ctl).SelectedValue = (string)entry.Value;
                    else
                        throw new ApplicationException(
                            string.Format("Недопустимый тип контрола \"{0}\"", ctl.GetType().ToString()));
                }
            }
        }

        #region Private methods

        // Скрытый конструктор
        private StateManager() { }

        // Получение объекта состояния из сесии
        private static List<StateEntry> GetSessionObject()
        {
            if (Session[SessionKey] == null)
                return new List<StateEntry>();

            return (List<StateEntry>)Session[SessionKey];
        }

        // Сохранение объекта в сессии
        private static void SaveSessionObject(List<StateEntry> state)
        {
            Session[SessionKey] = state;
        }

        // Восстановление значений для репитера
        private static void RestoreRepeaterState(StateEntry entry, Repeater repeater)
        {
            string[] pairs = entry.Value.ToString().Split(',');

            List<Pair> marks = new List<Pair>();
            foreach (string pair in pairs)
            {
                string[] param = pair.Split('=');
                if (param.Length == 2)
                    marks.Add(new Pair(param[0], param[1]));
            }

            // Привяжу к нему данные и построю коллекцию Items
            repeater.DataBind();
            foreach (RepeaterItem item in repeater.Items)
            {
                HiddenField hfId = (HiddenField)item.FindControl("hfId");
                TextBox txtValue = (TextBox)item.FindControl("txtValue");

                if (hfId != null && txtValue != null && !String.IsNullOrEmpty(hfId.Value))
                    foreach (Pair mark in marks)
                        if (hfId.Value == mark.First.ToString())
                            txtValue.Text = mark.Second.ToString();
            }
        }

        #endregion

    }
}
