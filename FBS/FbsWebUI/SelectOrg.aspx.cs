using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic ;
using Fbs.Web.Administration.Organizations;
using Fbs.Web.Administration.SqlConstructor.Organizations;
using Fbs.Core.CatalogElements;
using Fbs.Core.Organizations;
using Fbs.Core.Common;
using System.Web.UI.WebControls;
using System.Linq;
using Fbs.Utility;

namespace Fbs.Web
{
    public partial class Registration_st1 : System.Web.UI.Page
    {
        private SqlConstructor_GetOrganizations m_SqlConstructorGetOrganizations;

        public string BackUrl = "";
        public string BackUrl_forUse = "";
        public string ReturnPrmName = "OrgID";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BackUrl=HttpUtility.UrlDecode(Request.QueryString["BackUrl"]);
            
            BackUrl_forUse = Request.QueryString["BackUrl"];
            if (BackUrl_forUse == null)
                return;
            if (!BackUrl_forUse.Contains("?"))
                BackUrl_forUse = BackUrl_forUse + "?";
            else
                BackUrl_forUse = BackUrl_forUse += "&";
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ReturnPrmName"]))
            {
                ReturnPrmName = Request.QueryString["ReturnPrmName"];
            }

            DataTable regionDT = RegionDataAcessor.GetAllInEtalon(null);

            RepeaterRegions.DataSource = regionDT;
            RepeaterRegions.DataBind();

            // Заполняем выпадающие списки на вкладке "Выбор из справочника"
            ScriptManager1.RegisterAsyncPostBackControl(dlRegions);
            ScriptManager1.RegisterAsyncPostBackControl(dlSchoolType);

            if (!Page.IsPostBack)
            {
                dlRegions.DataSource = RegionDataAcessor.GetAllInEtalon("<Выберите регион>");
                dlRegions.DataBind();

                dlSchoolType.DataSource = OrganizationDataAccessor.GetTypes("<Выберите тип>");
                dlSchoolType.DataBind();

                dlRegions_SelectedIndexChanged(null, null);
            }

            // -------------------------------------------------------------

            m_SqlConstructorGetOrganizations = new SqlConstructor_GetOrganizations_Registration(Request.QueryString);

            tablePager_top.ItemCount = GetOrgsCount();
            tablePager_bottom.ItemCount = tablePager_top.ItemCount;
            m_SqlConstructorGetOrganizations.defaultPageSize = tablePager_top.PageSize;
            txtInfo.Text = tablePager_bottom.ItemCount.ToString();


            if (SearchByINN())
            {
                System.Text.RegularExpressions.Regex Regex = new System.Text.RegularExpressions.Regex("[0-9]{10}");
                if (!Regex.Match(Request.QueryString["INN"]).Success) //Не 10 цифр
                {
                    ShowError("ИНН должен состоять только из цифр и содержать ровно 10 знаков.");
                    return;
                }
            }

            if (m_SqlConstructorGetOrganizations.HasFilter()) // (QueryParameters.Count>0)
            {
                dgOrgList.DataSource = GetOrgs();
                if (dgOrgList.DataSource != null)
                    dgOrgList.DataBind();
                
                pnlData.Visible = true;
                lblNoFilter.Visible = false;
            }
            else
            {
               if(Request.QueryString.Count>1)//Т.е. зашли не первый раз и нажали "искать"
                    ShowError();
            }
        }

        /// <summary>
        /// Производится ли поиск по ИНН
        /// </summary>
        /// <returns></returns>
        private bool SearchByINN()
        {
            bool INNIsSet = !string.IsNullOrEmpty(Request.QueryString["INN"]);
            bool INNRadioIsSelected = Request.QueryString["RBSearchBy"] == "INN";
            return INNRadioIsSelected && INNIsSet;
        }

        private void ShowError()
        {
            pnlData.Visible = false;
            lblNoFilter.Visible = true;
        }

        private void ShowError(string errorText)
        {
            pnlData.Visible = false;
            lblNoFilter.Visible = true;
            lblNoFilter.Text = errorText;
        }

        List<ParameterDefinition> QueryParameters_;
        private List<ParameterDefinition> QueryParameters
        {
            get
            {
                if (QueryParameters_ == null)
                {
                    QueryParameters_ =new List<ParameterDefinition>();
                    if ((!String.IsNullOrEmpty(Request.QueryString["OldTypeId"]))&&(Request.QueryString["OldTypeId"]!="0"))
                    {
                        int OldTypeId;
                        if (int.TryParse(Request.QueryString["OldTypeId"], out OldTypeId))
                        {
                            OrganizationDataAccessor.OPF OPF = OrganizationDataAccessor.GetIsPrivate(OldTypeId);
                            if (OPF != OrganizationDataAccessor.OPF.Undefinded) //Для РЦОИ, ОУО непонятно, какие они должны быть - поэтому не включаем в запрос
                            {
                                QueryParameters_.Add(new ParameterDefinition("IsPrivate", ((int)OPF).ToString(), ComparsionType.Equals));
                            }

                            int NewTypeId = OrganizationDataAccessor.GetNewTypeId(OldTypeId);
                            QueryParameters_.Add(new ParameterDefinition("TypeId", NewTypeId.ToString(), ComparsionType.Equals));
                        }
                    }
                    if ((!String.IsNullOrEmpty(Request.QueryString["RegID"]))&&(Request.QueryString["RegID"]!="0"))
                    {
                        QueryParameters_.Add(new ParameterDefinition("RegionId", Request.QueryString["RegId"], ComparsionType.Equals));
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["OrgName"]))
                    {
                        QueryParameters_.Add(new ParameterDefinition("FullName", Request.QueryString["OrgName"], ComparsionType.Like));
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["INN"]))
                    {
                        QueryParameters_.Add(new ParameterDefinition("INN", Request.QueryString["INN"], ComparsionType.Equals));
                    }
                }
                return QueryParameters_;
            }
        }

        private DataTable GetOrgs()
        {

          //  return OrganizationDataAccessor.GetAsTable(QueryParameters );




            DataTable table = null;

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = m_SqlConstructorGetOrganizations.GetSQL();
                cmd.Connection = connection;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }
            return table;
        }

        private int GetOrgsCount()
        {
          //  return OrganizationDataAccessor.GetCount(QueryParameters);
            int orgCount = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = m_SqlConstructorGetOrganizations.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                orgCount = (int)cmd.Parameters["rowCount"].Value;
            }
            return orgCount;
        }

        public string IsElementSelected(int  id, string name)
        {
            switch (name.ToLower())
            {
                case "regid":
                case "kindid":
                case "oldtypeid":
                    return (id == GetVal_int(name)) ? " selected " : "";
                default: return "";
            }
        }

        public string GetVal_Str(string name)
        {
            if (Page.Request.QueryString[name] != null)
                return Page.Request.QueryString[name];

            return "";
        }
        public int GetVal_int(string name)
        {
            int val = 0;

            if (Page.Request.QueryString[name] != null)
            {
                int.TryParse(Page.Request.QueryString[name], out val);
            }
            else
                val = 0;
            return val;
        }


        public  string IsRadioButtonSelected(string buttonValue)
        {
            if ((Request.QueryString["RBSearchBy"] != null) && (Request.QueryString["RBSearchBy"] == "INN"))
            {
                if (buttonValue == "INN")
                {
                    //pnlData.Visible = false;
                    return "checked=\"checked\"";
                }
                else if (buttonValue == "OrgName")
                {
                    //pnlData.Visible = false;
                    return "";
                }
            }
            else
            {
                if (buttonValue == "INN")
                {
                   // pnlData.Visible = false;
                    return "";
                }
                else if (buttonValue == "OrgName")
                {
                    //pnlData.Visible = false;
                    return "checked=\"checked\"";
                }
            }
            //pnlData.Visible = false;
            return "";
        }

        public string IsRowVisible(string rowId)
        {
            string queryString = Request.QueryString["RBSearchBy"];

            // Поиск по номеру ИНН
            if (queryString == "INN")
            {
                if (rowId == "INNRow")
                    return string.Empty;
                else
                    return "style=\"display:none;\"";
            }

            // Выбор из справочника
            if (queryString == "Dic")
            {
                if (rowId == "Dic")
                    return string.Empty;
                else
                    return "style=\"display:none;\"";
            }

            // Поиск по названию, региону и типу
            if (rowId == "RegionRow" || rowId == "OrgTypeRow" || rowId == "OrgNameRow")
                return string.Empty;
            else
                return "style=\"display:none;\"";
        }

        protected void dlRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int regionId = -1;
            int typeId = -1;

            phDicTree.Visible = false;
            phEmpty.Visible = false;
            phNote.Visible = true;

            if (!int.TryParse(dlRegions.SelectedValue, out regionId) || regionId == 0)
            {
                return;
            }

            if (!int.TryParse(dlSchoolType.SelectedValue, out typeId) || typeId == 0)
            {
                return;
            }

            LoadDicTree(regionId, typeId);
        }

        private void LoadDicTree(int regionId, int typeId)
        {
            List<OrgWithParent> items = OrganizationDataAccessor.GetWithParent();
            TreeView1.Nodes.Clear();

            if (items.Where(x => 
                x.RegionId == regionId && 
                x.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId) && 
                x.IsPrivate == (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private)).Count() > 0)
            {
                phDicTree.Visible = true;
                phEmpty.Visible = false;
                phNote.Visible = false;
            }
            else
            {
                phDicTree.Visible = false;
                phEmpty.Visible = true;
                phNote.Visible = false;
            }

            // Добавляем корни
            foreach (OrgWithParent item in items.Where(x => !x.MainId.HasValue))
            {
                if (HasChildren(item.Id.ToString(), items, regionId, typeId) || (
                    item.RegionId == regionId &&
                    item.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId) &&
                    item.IsPrivate == (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private ? true : false)
                    ))
                {
                    TreeNode newNode; 
                    if (item.ShortName == null || item.ShortName == string.Empty)
                    {
                        newNode = new TreeNode(item.FullName, item.Id.ToString());
                    }
                    else
                    {
                        newNode = new TreeNode(item.ShortName, item.Id.ToString());
                    }
                    newNode.ToolTip = item.FullName;
                    TreeView1.Nodes.Add(newNode);
                }
            }

            // Добавляем листья
            foreach (OrgWithParent item in items.Where(x => 
                x.MainId.HasValue && 
                x.RegionId == regionId &&
                x.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId) &&
                x.IsPrivate == (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private ? true : false)
                ))
            {
                TreeNode node = FindRootNode(item.MainId.ToString());
                TreeNode newNode = new TreeNode(item.ShortName, item.Id.ToString());
                newNode.ToolTip = item.FullName;
                node.ChildNodes.Add(newNode);
            }
            pnlData.Visible = true;
        }

        private bool HasChildren(string id, List<OrgWithParent> items, int regionId, int typeId)
        {
            foreach (OrgWithParent item in items.Where(x => 
                x.RegionId == regionId &&
                x.TypeId == OrganizationDataAccessor.GetNewTypeId(typeId) &&
                x.IsPrivate == (OrganizationDataAccessor.GetIsPrivate(typeId) == OrganizationDataAccessor.OPF.Private ? true : false)))
            {
                if (item.MainId.ToString() == id)
                {
                    return true;
                }
            }

            return false;
        }

        private TreeNode FindRootNode(string itemId)
        {
            foreach (TreeNode node in TreeView1.Nodes)
            {
                if (node.Value == itemId)
                {
                    return node;
                }
            }

            return null;
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            Response.Redirect(BackUrl_forUse + ReturnPrmName + "=" + TreeView1.SelectedValue.ToString());
        }
    }
}
