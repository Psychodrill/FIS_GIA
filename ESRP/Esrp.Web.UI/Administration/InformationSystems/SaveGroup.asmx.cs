namespace Esrp.Web.Administration.InformationSystems
{
    using System;
    using System.ComponentModel;
    using System.Web.Services;

    /// <summary>
    /// Сервис для создания группы
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class SaveGroup : WebService
    {
        #region Public Methods

        /// <summary>
        /// Веб метод для создания группы
        /// </summary>
        /// <param name="name"> Наименование группы </param>
        /// <param name="code"> Обозначение группы </param>
        /// <param name="systemId"> Идентификатор ИС </param>
        [WebMethod]
        public void CreateGroup(string name, string code, string systemId)
        {
            Esrp.Services.GroupService.CreateGroup(name, code, Convert.ToInt32(systemId));
        }

        /// <summary>
        /// Веб метод для обновления группы
        /// </summary>
        /// <param name="name"> Наименование группы </param>
        /// <param name="code"> Обозначение группы </param>
        /// <param name="groupId"> Идентификатор группы </param>
        [WebMethod]
        public void UpdateGroup(string name, string code, string groupId)
        {
            Esrp.Services.GroupService.UpdateGroup(name, code, Convert.ToInt32(groupId));
        }

        #endregion
    }
}