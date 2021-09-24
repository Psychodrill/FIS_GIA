namespace Esrp.Web.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Services;

    /// <summary>
    /// Контрол отображающий список групп для ИС
    /// </summary>
    public partial class GroupList : UserControl
    {
        #region Constants and Fields

        private readonly GroupService groupService = new GroupService();

        #endregion

        #region Properties

        /// <summary>
        /// Идентификатор информационной системы
        /// </summary>
        public int? SystemId
        {
            get
            {
                object o = this.ViewState["SystemId"];
                if (o == null)
                {
                    return null;
                }

                return int.Parse(o.ToString());
            }

            set
            {
                this.ViewState["SystemId"] = value;
            }
        }        

        #endregion

        #region Methods

        /// <summary>
        /// Берем идентификатор группы по умолчанию из скрытого поля, обновляем данные в бд
        /// </summary>
        public void UpdateDefaultGroup()
        {
            if (this.hfDefaultGroupId.Value != string.Empty && this.SystemId != null && this.SystemId > 0)
            {
                this.groupService.UpdateDefaultGroup(Convert.ToInt32(this.hfDefaultGroupId.Value), this.SystemId);
            }
        }

        #endregion

        /// <summary>
        /// Обработчик события, которое происходит до SelectMethod
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void DsGroupListOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["systemId"] = this.SystemId;            
        }

        /// <summary>
        /// Обработчик события "Удаление группы по идентификатору"
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Параметры события</param>
        protected void DeleteGroup(Object sender, CommandEventArgs e)
        {
            if (this.groupService.IsEmptyGroupById(Convert.ToInt32(e.CommandArgument)))
            {
                var master = (Common.Templates.Administration)this.Page.Master;
                if (master != null)
                {
                    var messageControl = master.MessageControl;
                    messageControl.Add(MessageType.Error, "Группу удалить нельзя, т.к. в ней есть пользователи!");
                    return;
                }
            }

            try
            {
                this.groupService.DeleteGroupById(Convert.ToInt32(e.CommandArgument));
            }
            catch (Exception ex)
            {
                var master = (Common.Templates.Administration)this.Page.Master;
                if (master != null)
                {
                    var messageControl = master.MessageControl;
                    messageControl.Add(MessageType.Error, "При удалении группы произошли ошибки!");
                }

                throw new BllException(ex.Message);
            }
            
            Response.Redirect(Request.RawUrl);
        }
    }
}