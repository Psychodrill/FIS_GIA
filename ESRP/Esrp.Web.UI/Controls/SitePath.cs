namespace Esrp.Web.Controls
{
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Путь по сайту (хлебные крошки)
    /// </summary>
    public class SitePath : System.Web.UI.Control
    {
        #region Properties

        private string mRootNodeTemplate = "/ ";
        private string mNodeTemplate = "<a href=\"{0}\" title=\"{1}\">{1}</a>";
        private string mCurrentNodeTemplate = "{0}";
        private string mPathSeparatorTemplate = " / ";

        /// <summary>
        /// Шаблон для корневой (главной) страницы
        /// </summary>
        /// <remarks>
        /// Доступны метапеременные:
        /// {0} - адрес (url) страницы
        /// {1} - заголовок (title) страницы
        /// {2} - описание (description) страницы
        /// </remarks>
        public string RootNodeTemplate
        {
            get { return mRootNodeTemplate; }
            set { mRootNodeTemplate = value; }
        }

        /// <summary>
        /// Шаблон для страниц
        /// </summary>
        /// <remarks>
        /// Доступны метапеременные:
        /// {0} - адрес (url) страницы
        /// {1} - заголовок (title) страницы
        /// {2} - описание (description) страницы
        /// </remarks>
        public string NodeTemplate
        {
            get { return mNodeTemplate; }
            set { mNodeTemplate = value; }
        }

        /// <summary>
        /// Шаблон для текущей страницы
        /// </summary>
        /// <remarks>
        /// Доступны метапеременные:
        /// {0} - заголовок (title) страницы
        /// {1} - описание (description) страницы
        /// </remarks>
        public string CurrentNodeTemplate
        {
            get { return mCurrentNodeTemplate; }
            set { mCurrentNodeTemplate = value; }
        }

        /// <summary>
        /// Шаблон разделителя страниц
        /// </summary>
        public string PathSeparatorTemplate
        {
            get { return mPathSeparatorTemplate; }
            set { mPathSeparatorTemplate = value; }
        }

        #endregion

        #region Methods

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            var sitePathNodes = new SiteMapNodeCollection();

            // Получу узлы, определяющие путь до текущей страницы. Т.к. структура иерархическая, то 
            // буду обрабатывать их в обратном порядке.
            var currentNode = SiteMap.CurrentNode;
            while (currentNode != null)
            {
                sitePathNodes.Add(currentNode);
                currentNode = currentNode.ParentNode;
            }

            // Покажу верхний узел
            if (!string.IsNullOrEmpty(RootNodeTemplate))
                writer.Write(RootNodeTemplate, SiteMap.RootNode.Url, SiteMap.RootNode.Title,
                    SiteMap.RootNode.Description);

            // Построю путь до текущей страницы
            SiteMapNode node;
            for (int nodeIndex = sitePathNodes.Count - 2; nodeIndex > -1; nodeIndex--)
            {
                node = sitePathNodes[nodeIndex];

                // Покажу узел
                if (node == SiteMap.CurrentNode)
                    writer.Write(CurrentNodeTemplate, node.GetActualTitle(), node.Description);
                else
                    writer.Write(NodeTemplate, Utility.GetNodeLinkAddress(node), 
                        node.GetActualTitle(), node.Description);

                // Покажу разделитель
                if (!string.IsNullOrEmpty(PathSeparatorTemplate) && nodeIndex > 0)
                    writer.Write(PathSeparatorTemplate);
            }
        }

        #endregion
    }
}
