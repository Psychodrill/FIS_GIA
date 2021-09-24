namespace Esrp.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Web.Controls.Attributes;

    /// <summary>
    /// The fbs list view.
    /// </summary>
    public class EsrpListView : ListView
    {
        #region Constants and Fields

        /// <summary>Список номеров редактированных записей в гриде.</summary>
        private List<int> dirtyRows = new List<int>();

        #endregion

        #region Public Properties

        /// <summary>
        /// определяет, когда будет запускаться UpdateRow - сразу после изменения или после вызова метода Update
        /// </summary>
        public bool AutoChange
        {
            get
            {
                if (this.ViewState["AutoChange"] == null)
                {
                    this.ViewState["AutoChange"] = false;
                }

                return (bool)this.ViewState["AutoChange"];
            }

            set
            {
                this.ViewState["AutoChange"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add dirty item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public void AddDirtyItem(int index)
        {
            this.dirtyRows.Add(index);
        }

        /// <summary>
        /// Сохранение модифицированных строк.		
        /// </summary>
        public void Save()
        {
            this.SaveAndNoClear();
            this.dirtyRows.Clear();
        }

        /// <summary>
        /// Сохранение модифицированных строк без очищеня списка
        /// нужно для повторного использования
        /// </summary>
        public void SaveAndNoClear()
        {
            foreach (var row in this.dirtyRows)
            {
                if (row < this.Items.Count)
                {
                    this.UpdateItem(row, false);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Привязывает HandleRowChanged к событию изменения контролов: 
        /// TextBox, CheckBox, DropDownList.
        /// </summary>
        /// <remarks>
        /// Если требуются добавить новый тип контролов, то следует перекрыть этот метод. 
        /// Не забывайте вызывать base.AddChangedHandler, чтобы обработать TextBox, CheckBox, DropDownList.
        /// </remarks>
        /// <param name="control">
        /// </param>
        protected virtual void AddChangedHandler(Control control)
        {
            Type t = control.GetType();
            var attr =
                (DontAddChangeHandlerAttribute)Attribute.GetCustomAttribute(t, typeof(DontAddChangeHandlerAttribute));

            if (attr != null)
            {
                return;
            }

            if (control is TextBox)
            {
                ((TextBox)control).TextChanged += this.HandleRowChanged;
            }
            else if (control is CheckBox)
            {
                ((CheckBox)control).CheckedChanged += this.HandleRowChanged;
            }
            else if (control is DropDownList)
            {
                ((DropDownList)control).SelectedIndexChanged += this.HandleRowChanged;
            }
            else
            {
                if (control is DataBoundControl)
                {
                    return;
                }

                foreach (Control ctrl in control.Controls)
                {
                    this.AddChangedHandler(ctrl);
                }
            }
        }

        /// <summary>
        /// The add control to container.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="container">
        /// The container.
        /// </param>
        /// <param name="addLocation">
        /// The add location.
        /// </param>
        protected override void AddControlToContainer(Control control, Control container, int addLocation)
        {
            base.AddControlToContainer(control, container, addLocation);
            this.AddChangedHandler(control);
        }

        /// <summary>
        /// The data bind.
        /// </summary>
        /// <param name="raiseOnDataBinding">
        /// The raise on data binding.
        /// </param>
        protected override void DataBind(bool raiseOnDataBinding)
        {
            base.DataBind(raiseOnDataBinding);
            this.dirtyRows.Clear();
        }

        /// <summary>
        /// The inher load control state.
        /// </summary>
        /// <param name="savedState">
        /// The saved state.
        /// </param>
        protected void InherLoadControlState(object savedState)
        {
            base.LoadControlState(savedState);
        }

        /// <summary>
        /// The inher save control state.
        /// </summary>
        /// <returns>
        /// The inher save control state.
        /// </returns>
        protected object InherSaveControlState()
        {
            return base.SaveControlState();
        }

        /// <summary>
        /// Перегружен - НЕ ИСПОЛЬЗЫВАТЬ
        /// </summary>
        /// <param name="savedState">
        /// The saved State.
        /// </param>
        protected override void LoadControlState(object savedState)
        {
        }

        /// <summary>
        /// The load view state.
        /// </summary>
        /// <param name="savedState">
        /// The saved state.
        /// </param>
        protected override void LoadViewState(object savedState)
        {
            var objArray = savedState as object[];

            if (objArray != null)
            {
                base.LoadViewState(objArray[0]);

                if (objArray[1] != null)
                {
                    this.dirtyRows = (List<int>)objArray[1];
                }

                if (objArray[2] != null)
                {
                    this.InherLoadControlState(objArray[2]);
                }
            }
        }

        /// <summary>
        /// Перегружен - НЕ ИСПОЛЬЗЫВАТЬ
        /// </summary>
        /// <returns>
        /// The save control state.
        /// </returns>
        protected override object SaveControlState()
        {
            return null;
        }

        /// <summary>
        /// The save view state.
        /// </summary>
        /// <returns>
        /// The save view state.
        /// </returns>
        protected override object SaveViewState()
        {
            object q1 = base.SaveViewState();
            object q2 = this.dirtyRows;
            object q3 = this.InherSaveControlState();
            return new[] { q1, q2, q3 };
        }

        private static ListViewDataItem FindListViewDataItem(Control control)
        {
            if (control.NamingContainer is ListViewDataItem)
            {
                return (ListViewDataItem)control.NamingContainer;
            }

            return FindListViewDataItem(control.NamingContainer);
        }

        /// <summary>
        /// Этот обработчик вызывается при изменении значений в любом контроле.
        /// Необходимо пометить строку, в которой находится контрол, как модифицированную.        
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="args">
        /// </param>
        private void HandleRowChanged(object sender, EventArgs args)
        {
            // контрол принадлежит пространству имён, устанавливаемому строкой. Найдём эту строку.
            ListViewDataItem listItem = FindListViewDataItem((Control)sender);
            if (null == listItem)
            {
                return; // событие сработало для постороннего контрола
            }

            if (this.AutoChange)
            {
                this.UpdateItem(listItem.DataItemIndex, false);
            }
            else
            {
                if (!this.dirtyRows.Contains(listItem.DataItemIndex))
                {
                    this.dirtyRows.Add(listItem.DataItemIndex);
                }
            }
        }

        #endregion
    }
}