namespace Esrp.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core.CatalogElements;
    using Esrp.Core.Organizations;
    using Esrp.Web.Administration.Organizations;
    using Esrp.Web.Administration.SqlConstructor.Organizations;

    /// <summary>
    /// The registration_st 1.
    /// </summary>
    public partial class Registration_st1 : Page
    {
        #region Constants and Fields

        /// <summary>
        /// Url для обратного перехода
        /// </summary>
        public string BackUrl = string.Empty;

        public string BackUrl_forUse = string.Empty;

        public string ReturnPrmName = "OrgID";

        private SqlConstructor_GetOrganizations m_SqlConstructorGetOrganizations;

        #endregion

        #region Public Methods and Operators
       
        /// <summary>
        /// Выбирать элемент или нет
        /// </summary>
        /// <param name="paramName"> Имя парметра </param>
        /// <param name="value"> Значение параметра </param>
        /// <returns> строка selected=\"selected\" или пусто, в зависимости от этого элемент либо выбирается, либо нет </returns>
        public string SelectValue(string paramName, string value)
        {
            if (this.Request.QueryString[paramName] != null)
            {
                var selectedParam = Request.QueryString[paramName].Split(Convert.ToChar(","));

                foreach (var param in selectedParam)
                {
                    if (string.Compare(param, value, true) == 0)
                    {
                        return "selected=\"selected\"";
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// The is radio button selected.
        /// </summary>
        /// <param name="buttonValue">
        /// The button value.
        /// </param>
        /// <returns>
        /// The is radio button selected.
        /// </returns>
        public string IsRadioButtonSelected(string buttonValue)
        {
            if (this.Request.QueryString["RBSearchBy"] == null)
            {
                return buttonValue == "OrgName" ? "checked=\"checked\"" : string.Empty;
            }
            else
            {
                if (this.Request.QueryString["RBSearchBy"] == "INN")
                {
                    return buttonValue == "INN" ? "checked=\"checked\"" : string.Empty;
                }
                if (this.Request.QueryString["RBSearchBy"] == "OrgName")
                {
                    return buttonValue == "OrgName" ? "checked=\"checked\"" : string.Empty;
                }
                if (this.Request.QueryString["RBSearchBy"] == "Dic")
                {
                    return buttonValue == "Dic" ? "checked=\"checked\"" : string.Empty;
                }
            }
            
            if ((this.Request.QueryString["RBSearchBy"] != null) && (this.Request.QueryString["RBSearchBy"] == "INN"))
            {
                if (buttonValue == "INN")
                {
                    // pnlData.Visible = false;
                    return "checked=\"checked\"";
                }
                else if (buttonValue == "OrgName")
                {
                    // pnlData.Visible = false;
                    return string.Empty;
                }
            }
            else
            {
                if (buttonValue == "INN")
                {
                    // pnlData.Visible = false;
                    return string.Empty;
                }
                else if (buttonValue == "OrgName")
                {
                    // pnlData.Visible = false;
                    return "checked=\"checked\"";
                }
            }

            // pnlData.Visible = false;
            return string.Empty;
        }

        /// <summary>
        /// The is row visible.
        /// </summary>
        /// <param name="rowId">
        /// The row id.
        /// </param>
        /// <returns>
        /// The is row visible.
        /// </returns>
        public string IsRowVisible(string rowId)
        {
            string queryString = this.Request.QueryString["RBSearchBy"];

            // Поиск по номеру ИНН
            if (queryString == "INN")
            {
                if (rowId == "INNRow")
                {
                    return string.Empty;
                }
                else
                {
                    return "style=\"display:none;\"";
                }
            }

            // Выбор из справочника
            if (queryString == "Dic")
            {
                if (rowId == "Dic")
                {
                    return string.Empty;
                }
                else
                {
                    return "style=\"display:none;\"";
                }
            }

            // Поиск по названию, региону и типу
            if (rowId == "RegionRow" || rowId == "OrgTypeRow" || rowId == "OrgNameRow")
            {
                return string.Empty;
            }
            else
            {
                return "style=\"display:none;\"";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BackUrl = HttpUtility.UrlDecode(this.Request.QueryString["BackUrl"]);

            this.BackUrl_forUse = this.Request.QueryString["BackUrl"];
            if (this.BackUrl_forUse == null)
            {
                return;
            }

            if (!this.BackUrl_forUse.Contains("?"))
            {
                this.BackUrl_forUse = this.BackUrl_forUse + "?";
            }
            else
            {
                this.BackUrl_forUse = this.BackUrl_forUse += "&";
            }
        }

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Request.QueryString["ReturnPrmName"]))
            {
                this.ReturnPrmName = this.Request.QueryString["ReturnPrmName"];
            }

            DataTable regionDT = RegionDataAcessor.GetAllInEtalon(null);

            this.RepeaterRegions.DataSource = regionDT;
            this.RepeaterRegions.DataBind();

            // Заполняем выпадающие списки на вкладке "Выбор из справочника"
            this.ScriptManager1.RegisterAsyncPostBackControl(this.dlRegions);
            this.ScriptManager1.RegisterAsyncPostBackControl(this.dlSchoolType);

            if (!this.Page.IsPostBack)
            {
                this.dlRegions.DataSource = RegionDataAcessor.GetAllInEtalon("<Выберите регион>");
                this.dlRegions.DataBind();

                this.dlSchoolType.DataSource = OrganizationDataAccessor.GetTypes("<Выберите тип>");
                this.dlSchoolType.DataBind();

                this.dlRegions_SelectedIndexChanged(null, null);
            }

            // -------------------------------------------------------------
            this.m_SqlConstructorGetOrganizations =
                new SqlConstructor_GetOrganizations_Registration(this.Request.QueryString);

            this.tablePager_top.ItemCount = this.GetOrgsCount();
            this.tablePager_bottom.ItemCount = this.tablePager_top.ItemCount;
            this.m_SqlConstructorGetOrganizations.defaultPageSize = this.tablePager_top.PageSize;
            this.txtInfo.Text = this.tablePager_bottom.ItemCount.ToString();

            if (this.SearchByINN())
            {
                var regex = new Regex("[0-9]{10}");
                if (!regex.Match(this.Request.QueryString["INN"]).Success)
                {
                    // Не 10 цифр
                    this.ShowError("ИНН должен состоять только из цифр и содержать ровно 10 знаков.");
                    return;
                }
            }

            if (this.m_SqlConstructorGetOrganizations.HasFilter())
            {
                this.dgOrgList.DataSource = this.GetOrgs();
                if (this.dgOrgList.DataSource != null)
                {
                    this.dgOrgList.DataBind();
                }

                this.pnlData.Visible = true;
                this.lblNoFilter.Visible = false;
            }
            else
            {
                if (this.Request.QueryString.Count > 1)
                {
                    // Т.е. зашли не первый раз и нажали "искать"
                    this.ShowError();
                }
            }
        }

        /// <summary>
        /// The tree view 1_ selected node changed.
        /// </summary>
        /// <param name="sender">
        /// Источник события 
        /// </param>
        /// <param name="e">
        /// параметры события 
        /// </param>
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BackUrl_forUse + this.ReturnPrmName + "=" + this.TreeView1.SelectedValue);
        }

        private void FindOrgTree()
        {
            var regionId = -1;
            var typeId = -1;

            this.phDicTree.Visible = false;
            this.phEmpty.Visible = false;
            this.phNote.Visible = true;

            if (!int.TryParse(this.dlRegions.SelectedValue, out regionId) || regionId == 0)
            {
                return;
            }

            if (!int.TryParse(this.dlSchoolType.SelectedValue, out typeId) || typeId == 0)
            {
                return;
            }

            this.LoadDicTree(regionId, typeId);
        }

        /// <summary>
        /// The dl regions_ selected index changed.
        /// </summary>
        /// <param name="sender">
        /// Источник события 
        /// </param>
        /// <param name="e">
        /// параметры события 
        /// </param>
        protected void dlRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindOrgTree();
        }

        private TreeNode FindRootNode(string itemId)
        {
            foreach (TreeNode node in this.TreeView1.Nodes)
            {
                if (node.Value == itemId)
                {
                    return node;
                }
            }

            return null;
        }

        private DataTable GetOrgs()
        {
            DataTable table = null;

            var connectionString = ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = this.m_SqlConstructorGetOrganizations.GetSQL();
                cmd.Connection = connection;
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }

            return table;
        }

        private int GetOrgsCount()
        {
            int orgCount = 0;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = this.m_SqlConstructorGetOrganizations.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                orgCount = (int)cmd.Parameters["rowCount"].Value;
            }

            return orgCount;
        }

        private bool HasChildren(string id, List<OrgWithParent> items, int regionId, int typeId)
        {
            foreach (var item in
                items.Where(
                    x =>
                    x.RegionId == regionId && x.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId)
                    &&
                    x.IsPrivate
                    ==
                    (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private)))
            {
                if (item.MainId.ToString() == id)
                {
                    return true;
                }
            }

            return false;
        }

        private void LoadDicTree(int regionId, int typeId)
        {
            var items = OrganizationDataAccessor.GetWithParent();
            this.TreeView1.Nodes.Clear();

            if (
                items.Where(
                    x =>
                    x.RegionId == regionId && x.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId)
                    &&
                    x.IsPrivate
                    ==
                    (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private)).Count() > 0)
            {
                this.phDicTree.Visible = true;
                this.phEmpty.Visible = false;
                this.phNote.Visible = false;
                lblNoFilter.Visible = false;
            }
            else
            {
                this.phDicTree.Visible = false;
                this.phEmpty.Visible = true;
                this.phNote.Visible = false;
            }

            // Добавляем корни
            foreach (var item in items.Where(x => !x.MainId.HasValue))
            {
                if (this.HasChildren(item.Id.ToString(), items, regionId, typeId)
                    ||
                    (item.RegionId == regionId && item.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId)
                     &&
                     item.IsPrivate
                     ==
                     (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private
                          ? true
                          : false)))
                {
                    TreeNode newNode;
                    newNode = string.IsNullOrEmpty(item.ShortName) ? new TreeNode(item.FullName, item.Id.ToString()) : new TreeNode(item.ShortName, item.Id.ToString());

                    newNode.ToolTip = item.FullName;
                    this.TreeView1.Nodes.Add(newNode);
                }
            }

            // Добавляем листья
            foreach (var item in
                items.Where(
                    x =>
                    x.MainId.HasValue && x.RegionId == regionId
                    && x.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId)
                    &&
                    x.IsPrivate
                    ==
                    (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private)))
            {
                var node = this.FindRootNode(item.MainId.ToString());
                var newNode = new TreeNode(item.ShortName, item.Id.ToString());
                newNode.ToolTip = item.FullName;
                node.ChildNodes.Add(newNode);
            }

            this.pnlData.Visible = true;
        }

        /// <summary>
        /// Производится ли поиск по ИНН
        /// </summary>
        /// <returns>
        /// The search by inn.
        /// </returns>
        private bool SearchByINN()
        {
            var innIsSet = !string.IsNullOrEmpty(this.Request.QueryString["INN"]);
            var innRadioIsSelected = this.Request.QueryString["RBSearchBy"] == "INN";
            return innRadioIsSelected && innIsSet;
        }

        private void ShowError()
        {
            this.pnlData.Visible = false;
            this.lblNoFilter.Visible = true;
        }

        private void ShowError(string errorText)
        {
            this.pnlData.Visible = false;
            this.lblNoFilter.Visible = true;
            this.lblNoFilter.Text = errorText;
        }

        /// <summary>
        /// The get current url.
        /// </summary>
        /// <param name="excludeParamName">
        /// The exclude param name.
        /// </param>
        /// <returns>
        /// The get current url.
        /// </returns>
        public string GetCurrentUrl(string excludeParamName)
        {
            string url = string.Empty;
            for (int i = 0; i < this.Page.Request.QueryString.Count; i++)
            {
                string key = this.Page.Request.QueryString.GetKey(i);
                if (!string.IsNullOrEmpty(key))
                {
                    string val = HttpUtility.UrlEncode(this.Page.Request.QueryString.Get(key));

                    if (key.ToLower() != excludeParamName.ToLower())
                    {
                        url += string.Format("{2}{0}={1}", key, val, url.Length == 0 ? "?" : "&");
                    }
                }
            }

            url = this.Page.Request.Path + url + string.Format("{1}{0}=", excludeParamName, url.Length == 0 ? "?" : "&");

            return url;
        }

        #endregion

        protected void BtnByDicClick(object sender, EventArgs e)
        {
            
        }

        protected void dlRegions_SelectedIndexChanged1(object sender, EventArgs e)
        {
            FindOrgTree();

        }
    }
}