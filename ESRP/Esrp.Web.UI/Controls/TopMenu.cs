namespace Esrp.Web.Controls
{
    using System.Linq;
    using System.Web;
    using System.Web.UI;

    using Esrp.Core.Systems;

    /// <summary>
    /// Верхнее меню страницы
    /// </summary>
    public class TopMenu : Control
    {
        #region Constants and Fields

        private string mFooterTemplate = "</ul>";

        private string mHeaderTemplate = "<ul>\r\n";

        private string mItemTemplate =
            "<li class=\"{0}\"><div class=\"left\"></div><div class=\"text\"><a href=\"{1}\" title=\"{2}\" class=\"un\">{2}</a></div><div class=\"right\"></div></li>\r\n";

        private string mRootResourceKey;

        private string mSelectedItemTemplate =
            "<li class=\"{0}\"><div class=\"left\"></div><div class=\"text\"><a href=\"{1}\" title=\"{2}\">{2}</a></div><div class=\"right\"></div></li>\r\n";

        #endregion

        #region Public Properties

        /// <summary>
        /// Шаблон футера
        /// </summary>
        public string FooterTemplate
        {
            get
            {
                return this.mFooterTemplate;
            }

            set
            {
                this.mFooterTemplate = value;
            }
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
            get
            {
                return this.mHeaderTemplate;
            }

            set
            {
                this.mHeaderTemplate = value;
            }
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
            get
            {
                return this.mItemTemplate;
            }

            set
            {
                this.mItemTemplate = value;
            }
        }

        /// <summary>
        /// Ключ корневого элемента, от уровня которого строится меню
        /// </summary>
        public string RootResourceKey
        {
            get
            {
                return this.mRootResourceKey;
            }

            set
            {
                this.mRootResourceKey = value;
            }
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
            get
            {
                return this.mSelectedItemTemplate;
            }

            set
            {
                this.mSelectedItemTemplate = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            SiteMapNode selectedNode = SiteMap.CurrentNode;
            // Верхний уровень раздела 
            SiteMapNode rootNode = Utility.FindSiteMapNodeFromResourceKey(this.mRootResourceKey);

            // Получу коллекцию узлов, в которые может попасть текущий пользователь согласно своему 
            // набору ролей

            var allowedNodes = new SiteMapNodeCollection();
            // Определю узел раздела, к которому относится текущая страница
            
            foreach (SiteMapNode child in rootNode.ChildNodes)
            {
                if (Utility.ShowNodebyUserRoles(child.Roles, child["notroles"] == null ? null : child["notroles"].Split(',').ToList()) 
                    && !(child.ResourceKey == "administration" && !GeneralSystemManager.IsOpenSystem())
                )
                {
                    allowedNodes.Add(child);
                }
            }

            
            if (selectedNode != null)
            {
                foreach (SiteMapNode child in allowedNodes)
                {
                    if (selectedNode.IsDescendantOf(child))
                    {

                        selectedNode = child;
                        // проверим является ли текущая страница "общеиспользуемой"
                        if ("IS".Equals(Page.Request.QueryString["UserKey"])
                            && SiteMap.CurrentNode.ParentNode == child
                            && (SiteMap.CurrentNode.Title.Equals("Сменить пароль")
                            || SiteMap.CurrentNode.Title.Equals("Сменили пароль")
                            || SiteMap.CurrentNode.Title.Equals("История изменений")
                            || SiteMap.CurrentNode.Title.Equals("Версия истории изменений")
                            || SiteMap.CurrentNode.Title.Equals("История аунтефикаций")))
                        {
                            selectedNode = child.NextSibling;
                        }                        
                    }
                }
            }

            // Покажу хидер
            writer.Write(this.mHeaderTemplate);

            // Построю элементы списка
            SiteMapNode node;
            for (int nodeIndex = 0; nodeIndex < allowedNodes.Count; nodeIndex++)
            {
                node = allowedNodes[nodeIndex];

                // Определю класс элемента <li>
                string cssClass = this.DefineItemCssClass(nodeIndex, node, selectedNode, allowedNodes);

                // Создам элемент списка на основе шаблона
                if (Utility.ShowNodebyUserRoles(node.Roles))
                {
                    if (node == selectedNode)
                    {
                        writer.Write(
                            this.mSelectedItemTemplate,
                            cssClass,
                            Utility.GetNodeLinkAddress(node),
                            node.Title,
                            node.Description);
                    }
                    else
                    {
                        writer.Write(
                            this.mItemTemplate, cssClass, Utility.GetNodeLinkAddress(node), node.Title, node.Description);
                    }
                }
            }

            // Покажу футер
            writer.Write(this.mFooterTemplate);
        }

        // Определение css класса элемента
        private string DefineItemCssClass(
            int nodeIndex, SiteMapNode node, SiteMapNode selectedNode, SiteMapNodeCollection allowedNodes)
        {
            // Если в меню только один элемент, то назначу ему класс single active (считаю, что 
            // если в меню 1 элемент, то он является активным)
            if (allowedNodes.Count == 1)
            {
                return "single active";
            }

            string cssClass = string.Empty;

            if (nodeIndex == 0)
            {
                // Первый элемент
                cssClass = "first";
            }
            else if (nodeIndex == allowedNodes.Count - 1)
            {
                // Последний элемент
                cssClass = "last";
            }

            // Активный элемент доплню классом active 
            if (node == selectedNode)
            {
                if (string.IsNullOrEmpty(cssClass))
                {
                    cssClass = "active";
                }
                else
                {
                    cssClass += " active";
                }
            }

            return cssClass;
        }

        #endregion
    }
}