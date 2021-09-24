namespace Esrp.Web
{
    using System.Web.UI;
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    /// <summary>
    /// CСтраница с документами
    /// </summary>
    public partial class Documents : Page
    {
        #region Constants and Fields

        private const string DefaultDocTypeCode = "other"; // Общие документы

        #endregion

        #region Public Methods and Operators

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            //SiteMapNode docNode = Utility.FindSiteMapNodeFromResourceKey("Documents").Clone();

            
            //docNode.ReadOnly=false;
            //docNode.ChildNodes=new SiteMapNodeCollection( contexts.Select(x =>
            //                                        new SiteMapNode(SiteMap.Provider, "")
            //                                        {
            //                                            Title = x.Value,
            //                                            Url = string.Format("/Documents.aspx?DocType={0}", x.Key)
            //                                        }).ToArray()
            //);
            SiteMap.SiteMapResolve += new SiteMapResolveEventHandler(SiteMap_SiteMapResolve);
        }

        SiteMapNode SiteMap_SiteMapResolve(object sender, SiteMapResolveEventArgs e)
        {
            
            if (e.Provider.CurrentNode.ResourceKey == "Documents")
            {
                SiteMapNode currentNode = null;
                Dictionary<string, string> contexts = Esrp.Core.Document.GetAvailableContexts();
                e.Provider.CurrentNode.ReadOnly = false;
                e.Provider.CurrentNode.ChildNodes = new SiteMapNodeCollection(contexts.Where(x => x.Key != "Instruction").Select(x =>
                                                        new SiteMapNode(SiteMap.Provider, "")
                                                        {
                                                            Title = x.Value,
                                                            Url = string.Format("/Documents.aspx?DocType={0}", x.Key),
                                                            Roles = new List<string>(),
                                                            ParentNode=e.Provider.CurrentNode
                                                        }).ToArray()
                );
                if (!String.IsNullOrEmpty(e.Context.Request.QueryString["DocType"]))
                    currentNode = e.Provider.CurrentNode.ChildNodes.OfType<SiteMapNode>().FirstOrDefault(x => x.Url == e.Context.Request.Url.PathAndQuery);
                else
                    currentNode = e.Provider.CurrentNode.ChildNodes.OfType<SiteMapNode>().FirstOrDefault(x => x.Url == string.Format("/Documents.aspx?DocType={0}", "other"));
                return currentNode ?? e.Provider.CurrentNode;
            }
            else
                return e.Provider.CurrentNode;
        }
        /// <summary>
        /// The get css class by query string.
        /// </summary>
        /// <param name="queryParam">
        /// The query param.
        /// </param>
        /// <param name="paramValue">
        /// The param value.
        /// </param>
        /// <returns>
        /// The get css class by query string.
        /// </returns>
        public string GetCSSClassByQueryString(string queryParam, object paramValue)
        {
            if (string.IsNullOrEmpty(this.Request[queryParam]) && paramValue.ToString() == DefaultDocTypeCode)
            {
                return "select";
            }

            if (this.Request[queryParam] == paramValue.ToString())
            {
                return "select";
            }

            return string.Empty;
        }

        protected override void OnUnload(EventArgs e)
        {
            SiteMap.SiteMapResolve -= new SiteMapResolveEventHandler(SiteMap_SiteMapResolve);
            base.OnUnload(e);
        }
        #endregion
    }
}