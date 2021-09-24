using System;
using System.Web;
using System.Web.UI;

namespace Fbs.Web.Controls
{
    /// <summary>
    /// Верхнее меню страницы
    /// </summary>
    public class TopMenu : System.Web.UI.Control
    {
        #region Properties

        private string mRootResourceKey;
        private string mHeaderTemplate = "<ul>\r\n";
        private string mFooterTemplate = "</ul>";
        private string mItemTemplate = "<li class=\"{0}\"><div class=\"left\"></div><div class=\"text\"><a href=\"{1}\" title=\"{2}\">{2}</a></div><div class=\"right\"></div></li>\r\n";
        private string mSelectedItemTemplate = "<li class=\"{0}\"><div class=\"left\"></div><div class=\"text\"><a href=\"{1}\" title=\"{2}\">{2}</a></div><div class=\"right\"></div></li>\r\n";

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
        /// <remarks>
        /// Доступны метапеременные:
        /// {0} - css класс элемента
        /// </remarks>
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

        /// <summary>
        /// Шаблон текущего элемента
        /// </summary>
        /// <remarks>
        /// Доступны метапеременные:
        /// {0} - css класс элемента
        /// {1} - заголовок (title) страницы
        /// {2} - описание (description) страницы
        /// </remarks>
        public string SelectedItemTemplate
        {
            get { return mSelectedItemTemplate; }
            set { mSelectedItemTemplate = value; }
        }

        #endregion

        #region Methods

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            // Верхний уровень раздела 
            SiteMapNode rootNode = Utility.FindSiteMapNodeFromResourceKey(mRootResourceKey);

            // Получу коллекцию узлов, в которые может попасть текущий пользователь согласно своему 
            // набору ролей
            SiteMapNodeCollection allowedNodes = new SiteMapNodeCollection();
            foreach (SiteMapNode child in rootNode.ChildNodes)
                if (Utility.ShowNodebyUserRoles(child.Roles))
                    allowedNodes.Add(child);

            // Определю узел раздела, к которому относится текущая страница
            SiteMapNode selectedNode = SiteMap.CurrentNode;
            if (selectedNode != null)
            {
                foreach (SiteMapNode child in allowedNodes)
                    if (SiteMap.CurrentNode.IsDescendantOf(child))
                        selectedNode = child;
            }
            // Покажу хидер
            writer.Write(mHeaderTemplate);

            // Построю элементы списка
            SiteMapNode node;
            for (int nodeIndex = 0; nodeIndex < allowedNodes.Count; nodeIndex++)
            {
                node = allowedNodes[nodeIndex];

                // Определю класс элемента <li>
                string cssClass = DefineItemCssClass(nodeIndex, node, selectedNode, allowedNodes);

                // Создам элемент списка на основе шаблона
                if (Utility.ShowNodebyUserRoles(node.Roles))
                    if (node == selectedNode)
                        writer.Write(mSelectedItemTemplate, cssClass, Utility.GetNodeLinkAddress(node),
                            node.Title, node.Description);
                    else
                        writer.Write(mItemTemplate, cssClass, Utility.GetNodeLinkAddress(node),
                            node.Title, node.Description);
            }

            // Покажу футер
            writer.Write(mFooterTemplate);
        }

        // Определение css класса элемента
        string DefineItemCssClass(int nodeIndex, SiteMapNode node, SiteMapNode selectedNode,
            SiteMapNodeCollection allowedNodes)
        {
            // Если в меню только один элемент, то назначу ему класс single active (считаю, что 
            // если в меню 1 элемент, то он является активным)
            if (allowedNodes.Count == 1)
                return "single active";
            
            string cssClass = string.Empty;

            if (nodeIndex == 0)
                // Первый элемент
                cssClass = "first";
            else if (nodeIndex == allowedNodes.Count - 1)
                // Последний элемент
                cssClass = "last";

            // Активный элемент доплню классом active 
            if (node == selectedNode)
            {
                if (string.IsNullOrEmpty(cssClass))
                    cssClass = "active";
                else
                    cssClass += " active";
            }

            return cssClass;
        }

        #endregion
    }
}
