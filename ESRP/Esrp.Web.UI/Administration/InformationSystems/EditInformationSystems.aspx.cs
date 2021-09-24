namespace Esrp.Web.Administration.InformationSystems
{
    using System;
    using System.Web.UI.WebControls;

    using Esrp.Web.Controls;

    using Esrp.Web.ViewModel.InformationSystems;

    /// <summary>
    /// Страница добавление информационной системы
    /// </summary>
    public partial class EditInformationSystems : System.Web.UI.Page
    {
        /// <summary>
        /// Идентификатор информационной системы
        /// </summary>
        public int Id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("InformationSystemsList.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
            this.fvInformationSystem.ItemCommand += new FormViewCommandEventHandler(fvInformationSystem_ItemCommand);
        }

        void fvInformationSystem_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
                Response.Redirect(this.BackLink.HRef, true);
        }

        /// <summary>
        /// Обработчик события, которое возникает до метода Select
        /// в нем необходимо заполнить входной параметр для Select
        /// </summary>
        /// <param name="sender">Источник события </param>
        /// <param name="e">Параметры события</param>
        protected void OdsInformationSystemsOnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (Page.Request.QueryString["SystemId"] != null)
            {
                int returnVal;
                if (int.TryParse(Page.Request.QueryString["SystemId"], out returnVal))
                {
                    e.InputParameters["systemId"] = returnVal;
                    this.Id = returnVal;
                }
                else
                {
                    throw new Exception("Не указан SystemId");
                }
            }
            else
            {
                throw new Exception("Не указан SystemId");
            }
        }


        /// <summary>
        /// Обработчик события, которое возникает до метода Update
        /// </summary>
        /// <param name="sender">Источник события </param>
        /// <param name="e">Параметры события</param>
        protected void OdsInformationSystemOnUpdating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            ((GroupList)this.fvInformationSystem.FindControl("GroupList")).UpdateDefaultGroup();
        }

        /// <summary>
        /// Функция для того чтобы показывать или скрывать элементы страницы
        /// </summary>
        /// <param name="eval">false - скрываем, true - показываем</param>
        /// <returns>строка none|пустая строка</returns>
        protected string CheckVisibility(bool eval)
        {
            if (eval == false)
            {
                return "none";
            }

            return string.Empty;
        }


        /// <summary>
        /// Обработчик события, которое возникает после метода Select
        /// </summary>
        /// <param name="sender">Источник события </param>
        /// <param name="e">Параметры события</param>
        protected void OdsInformationSystemsOnSelected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var entity = (InformationSystemEntity)e.ReturnValue;
            entity.SystemId = this.Id;
        }
    }
}