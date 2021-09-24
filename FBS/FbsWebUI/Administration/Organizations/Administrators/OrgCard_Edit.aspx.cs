namespace Fbs.Web.Administration.Organizations
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.UI;

    using Fbs.Core;
    using Fbs.Core.Organizations;

    /// <summary>
    /// The org card.
    /// </summary>
    public partial class OrgCard : Page
    {
        #region Properties

        private Organization organization { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get user status.
        /// </summary>
        /// <param name="sysStatus">
        /// The sys status.
        /// </param>
        /// <returns>
        /// The get user status.
        /// </returns>
        public static string GetUserStatus(string sysStatus)
        {
            return UserAccountExtentions.GetNewStatusName(sysStatus);
        }

        /// <summary>
        /// The get param_ int.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The get param_ int.
        /// </returns>
        public int GetParam_Int(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                int returnVal;
                if (int.TryParse(this.Page.Request.QueryString[name], out returnVal))
                {
                    return returnVal;
                }
            }

            return 0;
        }

        /// <summary>
        /// The get param_ str.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The get param_ str.
        /// </returns>
        public string GetParam_Str(string name)
        {
            if (this.Page.Request.QueryString[name] != null)
            {
                return this.Page.Request.QueryString[name];
            }

            return string.Empty;
        }

        /// <summary>
        /// The validate.
        /// </summary>
        public override void Validate()
        {
            base.Validate();

            int fbFullTime = 0;
            if (!int.TryParse(this.txtBxFBFullTime.Text, out fbFullTime) || fbFullTime < 0)
            {
                this.lblFederalBudgetError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            int fbEvening = 0;
            if (!int.TryParse(this.txtBxFBEvening.Text, out fbEvening) || fbEvening < 0)
            {
                this.lblTargetedError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            int fbPostal = 0;
            if (!int.TryParse(this.txtBxFBPostal.Text, out fbPostal) || fbPostal < 0)
            {
                this.lblLocalBudgetError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            int payFullTime = 0;
            if (!int.TryParse(this.txtBxPayFullTime.Text, out payFullTime) || payFullTime < 0)
            {
                this.lblPayingError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            int payEvening = 0;
            if (!int.TryParse(this.txtBxPayEvening.Text, out payEvening) || payEvening < 0)
            {
                this.lblFullTimeError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }

            int payPostal = 0;
            if (!int.TryParse(this.txtBxPayPostal.Text, out payPostal) || payPostal < 0)
            {
                this.lblEveningError.Text = "Должно быть введено целое положительное число, либо 0<br />";
                this.vlStruct.IsValid = false;
            }
        }

        #endregion

        #region Methods

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
            int orgID = this.GetParam_Int("OrgID");
            int newOrgId = this.GetParam_Int("NewOrgID");

            if (this.User.IsInRole("EditSelfOrganization") && !this.OrganizationAvaliableForEdit(orgID))
            {
                Organization org = OrganizationDataAccessor.GetByLogin(this.User.Identity.Name);
                if (org != null)
                {
                    if (orgID != org.Id)
                    {
                        this.Response.Redirect("OrgCard_Edit.aspx?OrgID=" + org.Id.ToString());
                    }
                }
                else
                {
                    this.Response.Redirect("/Profile/View.aspx");
                }
            }

            if (!this.Page.IsPostBack)
            {
                if (orgID > 0)
                {
                    // Редактирование организации
                    Organization org = OrganizationDataAccessor.Get(orgID);
                    this.organization = org;

                    // CheckBox (журналирование проверок)
                    this.cbIsLogCheckEvent.Checked = org.DisableLog;

                    // Заполняем простые текстовые поля
                    this.LFullName.Text = org.FullName;
                    this.LShortName.Text = org.ShortName;
                    this.lblOrgStatus.Text = org.Status.Name;

                    this.LINN.Text = org.INN;

                    this.LOwnerDepartment.Text = org.DepartmentFullName;

                    this.LDirectorName.Text = org.DirectorFullName;
                    this.LDirectorPosition.Text = org.DirectorPosition;

                    this.LAccredCert.Text = org.AccreditationSertificate;

                    this.LFactAddress.Text = org.FactAddress;
                    this.LJurAddress.Text = org.LawAddress;

                    this.LCityCode.Text = org.PhoneCityCode;
                    this.LPhone.Text = org.Phone;
                    this.LFax.Text = org.Fax;
                    this.LEMail.Text = org.EMail;
                    this.LSite.Text = org.Site;

                    // Модель приемной кампании
                    string modelName = org.RCModelId != 999 ? org.RCModelName : org.RCDescription;
                    this.lblModelName.Text = string.IsNullOrEmpty(modelName) ? "—" : modelName;

                    // Сведения об объеме и структуре приема
                    this.txtBxFBFullTime.Text = org.CNFBFullTime.ToString();
                    this.txtBxFBEvening.Text = org.CNFBEvening.ToString();
                    this.txtBxFBPostal.Text = org.CNFBPostal.ToString();
                    this.txtBxPayFullTime.Text = org.CNPayFullTime.ToString();
                    this.txtBxPayEvening.Text = org.CNPayEvening.ToString();
                    this.txtBxPayPostal.Text = org.CNPayPostal.ToString();

                    // Заполняем списки и чекбокс
                    this.LRegion.Text = org.Region.Name;
                    this.LOrgLevel.Text = org.OrgType.Name;

                    this.LOrgKind.Text = org.Kind.Name;

                    this.LOPF.Text = org.IsPrivate ? "Негосударственный" : "Государственный";

                    this.LIsFilial.Text = org.IsFilial ? "Да" : "Нет";
                    this.phMainOrgName.Visible = org.IsFilial;
                    this.lblMainOrgName.Text = org.MainFullName;

                    // Запоминаем идентификатор типа организации для того, 
                    // что бы скрыть ненужные поля для учредителя
                    this.hiddenKindIdEdit.Value = org.OrgType.Id.ToString();

                    if (org.Status.Id == (int)OrganizationStatus.Reorganized)
                    {
                        this.lblNewOrgName.Text =
                            string.Format(
                                "Новая организация: <a href=\"/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgID={1}\">{0}</a>", 
                                org.NewOrgFullName, 
                                org.NewOrgId);
                    }
                    else
                    {
                        this.lblNewOrgName.Visible = false;
                    }
                }
                else
                {
                    // Создание организации
                    this.phUpdate.Visible = orgID > 0;
                    this.btnUpdate.Text = "Создать";

                    // Сведения об объеме и структуре приема
                    this.txtBxFBFullTime.Text = "0";
                    this.txtBxFBEvening.Text = "0";
                    this.txtBxFBPostal.Text = "0";
                    this.txtBxPayFullTime.Text = "0";
                    this.txtBxPayEvening.Text = "0";
                    this.txtBxPayPostal.Text = "0";
                }
            }
        }

        /// <summary>
        /// The btn filial_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnFilial_Click(object sender, EventArgs e)
        {
            int orgID = this.GetParam_Int("OrgID");

            this.Response.Redirect(
                string.Format(
                    "/SelectOrg.aspx?BackUrl={0}&ReturnPrmName=FilID", 
                    HttpUtility.UrlEncode(
                        "./Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=" + orgID)));
        }

        /// <summary>
        /// выбрать организацию, в которую была реорганизована текущая
        /// </summary>
        /// <param name="sender">
        /// кнопка выбора новой организации
        /// </param>
        /// <param name="e">
        /// e
        /// </param>
        protected void btnReorganizedTo_Click(object sender, EventArgs e)
        {
            int orgId = this.GetParam_Int("OrgID");

            this.Response.Redirect(
                string.Format(
                    "/SelectOrg.aspx?BackUrl={0}&ReturnPrmName=NewOrgID", 
                    HttpUtility.UrlEncode(
                        "./Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID=" + orgId)));
        }

        /// <summary>
        /// The btn update_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                int orgId = this.GetParam_Int("OrgID");

                try
                {
                    if (orgId > 0)
                    {
                        this.btnUpdate.Enabled = true;

                        Organization org = OrganizationDataAccessor.Get(orgId);

                        // Сведения об объеме и структуре приема
                        org.CNFBFullTime = org.OrgType.Id != 6 ? int.Parse(this.txtBxFBFullTime.Text) : 0;
                        org.CNFBEvening = int.Parse(this.txtBxFBEvening.Text);
                        org.CNFBPostal = int.Parse(this.txtBxFBPostal.Text);
                        org.CNPayFullTime = int.Parse(this.txtBxPayFullTime.Text);
                        org.CNPayEvening = int.Parse(this.txtBxPayEvening.Text);
                        org.CNPayPostal = int.Parse(this.txtBxPayPostal.Text);
                        org.DisableLog = this.cbIsLogCheckEvent.Checked;
                        OrganizationDataAccessor.UpdateOrCreate(org, false, this.User.Identity.Name);

                        if (this.User.IsInRole("EditSelfOrganization"))
                        {
                            this.Response.Redirect("/Profile/View.aspx");
                        }
                        else
                        {
                            this.Response.Redirect(
                                string.Format("OrgCard_Edit_Success.aspx?IsNew={1}&OrgID={0}", org.Id, orgId != org.Id), 
                                true);
                        }

                        this.lblError.Text = string.Empty;
                        this.lblError.Visible = false;
                    }
                    else
                    {
                        this.btnUpdate.Enabled = false;
                    }
                }
                catch (Exception exp)
                {
                    if (orgId > 0)
                    {
                        this.lblError.Text = "Во время обновления организации произошла ошибка: " + exp.Message;
                    }
                    else
                    {
                        this.lblError.Text = "Во время создания организации произошла ошибка: " + exp.Message;
                    }

                    this.lblError.Visible = true;
                }
            }
        }

        private bool OrganizationAvaliableForEdit(int orgId)
        {
            Organization userOrg = OrganizationDataAccessor.GetByLogin(this.User.Identity.Name);

            if (userOrg == null)
            {
                return false;
            }

            string sqlString =
                string.Format(
                    "SELECT  [Id] FROM [dbo].[Organization2010] WHERE [DepartmentId] = {0} OR [MainId] = {0}", 
                    userOrg.Id);

            var table = new DataTable();
            using (var connection = new SqlConnection(DBSettings.ConnectionString))
            {
                var cmd = new SqlCommand(sqlString, connection);
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table.Load(reader);
                }
            }

            bool contains = false;
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToInt32(row["Id"]) == orgId)
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }

        #endregion
    }
}