using System;
using System.Web;
using System.Web.UI;

namespace Fbs.Web.Controls
{
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Левое меню страницы
    /// </summary>
    public class LeftMenu : System.Web.UI.Control
    {
        #region Properties

        private RootTypeEnum mRootType = RootTypeEnum.Single;
        private string mRootResourceKey;
        private string mTitleTemplate = "<div class=\"title\"><div><h3>{0}</h3></div></div>";
        private string mHeaderTemplate = "<ul>";
        private string mFooterTemplate = "</ul>";
        private string mItemTemplate = "<li class=\"{0}\"><a href=\"{1}\" title=\"{2}\">{2}</a></li>\r\n";

        /// <summary>
        /// Типы корнегово элемента
        /// </summary>
        public enum RootTypeEnum
        {
            None,
            Section, // меню строится относительно заданного раздела 
            Single   // меню строится относительного заданного узла
        }

        /// <summary>
        /// Тип корневого элемента
        /// </summary>
        public RootTypeEnum RootType
        {
            get { return mRootType; }
            set { mRootType = value; }
        }

        /// <summary>
        /// Ключ корневого элемента, от уровня которого строится меню
        /// </summary>
        public string RootResourceKey
        {
            get { return mRootResourceKey; }
            set { mRootResourceKey = value; }
        }

        /// <summary>
        /// Шаблон хидера
        /// </summary>
        public string HeaderTemplate
        {
            get { return mHeaderTemplate; }
            set { mHeaderTemplate = value; }
        }

        /// <summary>
        /// Шаблон футера
        /// </summary>
        public string FooterTemplate
        {
            get { return mFooterTemplate; }
            set { mFooterTemplate = value; }
        }

        /// <summary>
        /// Шаблон заголовка
        /// </summary>
        public string TitleTemplate
        {
            get { return mTitleTemplate; }
            set { mTitleTemplate = value; }
        }

        /// <summary>
        /// Шаблон элемента
        /// </summary>
        /// <remarks>
        /// Доступны метапеременные:
        /// {0} - css класс элемента
        /// {1} - адрес (url) страницы
        /// {2} - заголовок (title) страницы
        /// {3} - описание (description) страницы
        /// </remarks>
        public string ItemTemplate
        {
            get { return mItemTemplate; }
            set { mItemTemplate = value; }
        }

        #endregion

        #region Methods

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            // Верхний уровень раздела 
            SiteMapNode rootNode = Utility.FindSiteMapNodeFromResourceKey(mRootResourceKey);

            string title = string.Empty;
            SiteMapNodeCollection nodes = new SiteMapNodeCollection();

            // Определю заголовок активного раздела и его детей
            ProcessRootTypeValue(rootNode, ref title, ref nodes);

            SiteMapNode selectedNode = SiteMap.CurrentNode;

            foreach (SiteMapNode node in nodes)
                if (SiteMap.CurrentNode.IsDescendantOf(node))
                    selectedNode = node;

            // Отображу заголовок
            writer.Write(mTitleTemplate, title);

            // Выйду если нет дочерних узлов для отображения
            if (nodes.Count == 0)
                return;

            // Отображу хидер
            writer.Write(this.mHeaderTemplate);

            // Проверю в каком режиме работает приложение
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]))
            {
                // Открытая версия приложения
                foreach (SiteMapNode node in nodes)
                {
                    if (!node.ShowInLeftMenu())
                    {
                        continue;
                    }

                    if (!node.ShowInOpenedFbs())
                    {
                        continue;
                    }

                    string cssClass = string.Empty;
                    if (node == selectedNode)
                    {
                        cssClass = "select";
                    }

                    writer.Write(this.mItemTemplate, cssClass, node.Url, node.Title, node.Description);
                }
            }
            else
            {
                // Закрытая версия приложения
                foreach (SiteMapNode node in nodes)
                {
                    if (!node.ShowInLeftMenu())
                    {
                        continue;
                    }

                    if (!node.ShowInClosedFbs())
                    {
                        continue;
                    }

                    string cssClass = string.Empty;
                    if (node == selectedNode)
                    {
                        cssClass = "select";
                    }

                    writer.Write(this.mItemTemplate, cssClass, node.Url, node.Title, node.Description);
                }
            }

            // Отображу футер
            writer.Write(mFooterTemplate);
        }

        // Формирование заголовка и коллекции страниц в зависимости от типа корневого элемента
        private void ProcessRootTypeValue(SiteMapNode rootNode, ref string title,
            ref SiteMapNodeCollection nodes)
        {
            if (rootNode == null)
                return;
            if (SiteMap.CurrentNode == null)
                return;
            switch (mRootType)
            {
                case RootTypeEnum.Section:
                    // Получу коллекцию узлов, в которые может попасть текущий пользователь согласно 
                    // своему набору ролей
                    SiteMapNodeCollection allowedNodes = new SiteMapNodeCollection();
                    foreach (SiteMapNode child in rootNode.ChildNodes)
                        if (Utility.ShowNodebyUserRoles(child.Roles))
                            allowedNodes.Add(child);

                    // Определю узел раздела, к которому относится текущая страница
                    SiteMapNode sectionNode = SiteMap.CurrentNode;
                    foreach (SiteMapNode child in allowedNodes)
                        if (SiteMap.CurrentNode.IsDescendantOf(child))
                            sectionNode = child;

                    title = sectionNode.Title;

                    // Если у выбранного узла задан url, то не покажу его детей
                    if (string.IsNullOrEmpty(sectionNode.Url))
                        foreach (SiteMapNode child in sectionNode.ChildNodes)
                            if (Utility.ShowNodebyUserRoles(child.Roles))
                                nodes.Add(child);

                    break;
                case RootTypeEnum.Single:
                    title = rootNode.Title;

                    foreach (SiteMapNode child in rootNode.ChildNodes)
                        if (Utility.ShowNodebyUserRoles(child.Roles))
                            nodes.Add(child);

                    break;
                default:
                    throw new ApplicationException("Аттрибут RootType имеет недопустимое значение");
            }
        }

        #endregion
    }
}
