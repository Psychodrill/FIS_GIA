using System;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Utility;
using Fbs.Web.Administration.Organizations;
using Fbs.Core.Users ;
using Fbs.Core.Organizations;
using Fbs.Core.CatalogElements;

namespace Fbs.Web.Controls
{
    public partial class UserProfileEdit : System.Web.UI.UserControl
    {
        public string BackUrl = "/Profile/Edit.aspx";
        private const string SuccessUri = "/Profile/View.aspx";


        ////До 2010
        //UserAccount mCurrentUser;

        //public UserAccount CurrentUser
        //{
        //    get
        //    {
        //        if (mCurrentUser == null)
        //        {
        //            mCurrentUser = UserAccount.GetUserAccount(Account.ClientLogin);

        //            if (mCurrentUser == null)
        //                throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
        //                    Account.ClientLogin));
        //        }
        //        return mCurrentUser;
        //    }
        //}

       OrgUser  mCurrentUser;

        public OrgUser CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                {
                    mCurrentUser = OrgUserDataAccessor.Get(Account.ClientLogin);

                    if (mCurrentUser == null)
                        throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                            Account.ClientLogin));
                }
                return mCurrentUser;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // Выйду если постбэк
            if (!Page.IsPostBack)
            {
                // Заполню контролы
                litUserName.Text = CurrentUser.login;


                litStatus.Text = UserAccountExtentions.GetUserAccountStatusName(CurrentUser.status);
                litNewStatus.Text = UserAccountExtentions.GetUserAccountNewStatusName(CurrentUser.status);
                litStatusDescription.Text =  UserAccountExtentions.GetEditStatusDescription (CurrentUser );


                txtOrganizationChiefName.Text = CurrentUser.RequestedOrganization.DirectorFullName;
                txtEmail.Text = CurrentUser.email;
                txtPhone.Text = CurrentUser.phone;
                
                txtFullName.Text = CurrentUser.GetFullName();
                txtOrganizationAddress.Text = CurrentUser.RequestedOrganization.LawAddress;
                txtOrganizationFax.Text = CurrentUser.RequestedOrganization.Fax;
                txtOrganizationFounderName.Text = CurrentUser.RequestedOrganization.OwnerDepartment;
                txtOrganizationName.Text = CurrentUser.RequestedOrganization.FullName;
                txtOrganizationPhone.Text = CurrentUser.RequestedOrganization.Phone;
                txtOrganizationFax.Text = CurrentUser.RequestedOrganization.Fax;
                hfEtalonOrgID.Value = CurrentUser.RequestedOrganization.OrganizationId != null ? CurrentUser.RequestedOrganization.OrganizationId.ToString() : "";

                ddlOrganizationRegion.DataSource = RegionDataAcessor.GetAllInEtalon(null);
                ddlOrganizationRegion.DataBind();
                if (CurrentUser.RequestedOrganization.Region.Id == null)
                    ddlOrganizationRegion.SelectedValue = "0";
                else
                    ddlOrganizationRegion.SelectedValue = CurrentUser.RequestedOrganization.Region.Id.ToString();

                ddlOrgType.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);// Org.GetOrgTypes();
                ddlOrgType.DataBind();
                ddlOrgType.SelectedValue = CurrentUser.RequestedOrganization.OrgType.Id.ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["OrgID"]))
                {
                    int orgID;
                    if (int.TryParse(Request.QueryString["OrgID"], out orgID))
                    {
                        //throw new Exception("Непонятный блок");
                    Organization    NewOrg =OrganizationDataAccessor.Get(orgID);
                    if (NewOrg!=null )
                        {
                            txtOrganizationName.Text = NewOrg.FullName;
                            ddlOrganizationRegion.SelectedValue = NewOrg.Region.Id.ToString();
                            ddlOrgType.SelectedValue = NewOrg.OrgType.Id .ToString();
                            hfEtalonOrgID.Value = NewOrg.Id.ToString();
                        }
                        else hfEtalonOrgID.Value = "";
                    }
                }

                // если есть связь с Эталонной организацией, то блокируем поля входа
                if (hfEtalonOrgID.Value != "")
                {
                    txtOrganizationName.ReadOnly = true;
                    ddlOrganizationRegion.Enabled = false;
                    ddlOrgType.Enabled = false;
                }

                btnUpdate.Attributes.Add("handleClick", "false");

                btnChangeOrg.PostBackUrl =
                    string.Format("/SelectOrg.aspx?BackUrl={0}&RegID={1}&OrgTypeID={2}&OrgName={3}",
                                  BackUrl, ddlOrganizationRegion.SelectedValue,
                                  ddlOrgType.SelectedValue, txtOrganizationName.Text);


                //// Заполню контролы
                //litUserName.Text = CurrentUser.Login;
                //litStatus.Text = CurrentUser.GetStatusName();
                //litNewStatus.Text = CurrentUser.GetNewStatusName();
                //litStatusDescription.Text = CurrentUser.GetEditStatusDescription();
                //txtOrganizationChiefName.Text = CurrentUser.OrganizationChiefName;
                //txtEmail.Text = CurrentUser.Email;
                //txtPhone.Text = CurrentUser.Phone;
                //txtFullName.Text = CurrentUser.GetFullName();
                //txtOrganizationAddress.Text = CurrentUser.OrganizationAddress;
                //txtOrganizationFax.Text = CurrentUser.OrganizationFax;
                //txtOrganizationFounderName.Text = CurrentUser.OrganizationFounderName;
                //txtOrganizationName.Text = CurrentUser.OrganizationName;
                //txtOrganizationPhone.Text = CurrentUser.OrganizationPhone;
                //hfEtalonOrgID.Value = CurrentUser.EtalonOrgId.HasValue ? CurrentUser.EtalonOrgId.Value.ToString() : "";

                //ddlOrganizationRegion.DataSource = Region.GetRegions();
                //ddlOrganizationRegion.DataBind();
                //if (CurrentUser.OrganizationRegionId.ToString() == "")
                //    ddlOrganizationRegion.SelectedValue = "0";
                //else
                //    ddlOrganizationRegion.SelectedValue = CurrentUser.OrganizationRegionId.ToString();

                //ddlOrgType.DataSource = Org.GetOrgTypes();
                //ddlOrgType.DataBind();
                //ddlOrgType.SelectedValue = CurrentUser.EducationInstitutionTypeId.Value.ToString();

                //if (!string.IsNullOrEmpty(Request.QueryString["OrgID"]))
                //{
                //    int orgID;
                //    if (int.TryParse(Request.QueryString["OrgID"], out orgID))
                //    {
                //        Org tecOrg = Org.Get(orgID);
                //        if (tecOrg.OrgID > 0)
                //        {
                //            txtOrganizationName.Text = tecOrg.Name;
                //            ddlOrganizationRegion.SelectedValue = tecOrg.RegionId.ToString();
                //            ddlOrgType.SelectedValue = tecOrg.OrgTypeID.ToString();
                //            hfEtalonOrgID.Value = tecOrg.OrgID.ToString();
                //        }
                //        else hfEtalonOrgID.Value = "";
                //    }
                //}

                //// если есть связь с Эталонной организацией, то блокируем поля входа
                //if (hfEtalonOrgID.Value != "")
                //{
                //    txtOrganizationName.ReadOnly = true;
                //    ddlOrganizationRegion.Enabled = false;
                //    ddlOrgType.Enabled = false;
                //}

                //btnUpdate.Attributes.Add("handleClick", "false");

                //btnChangeOrg.PostBackUrl =
                //    string.Format("/SelectOrg.aspx?BackUrl={0}&RegID={1}&OrgTypeID={2}&OrgName={3}",
                //                  BackUrl, ddlOrganizationRegion.SelectedValue,
                //                  ddlOrgType.SelectedValue, txtOrganizationName.Text);
            }
        }

        public void vldIsEmailUniq_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string email = args.Value.Trim();
        //    args.IsValid = UserAccount.CheckUserAccountEmail(CurrentUser.Login, email);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

            // Получу текуший статус пользователя
            UserAccount.UserAccountStatusEnum status = CurrentUser.status;
            UpdateUser();
            // Если статус пользователя изменился на "на доработке" - отправлю уведомление
            if (CurrentUser.status == UserAccount.UserAccountStatusEnum.Consideration && CurrentUser.status != status)
                SendNotification();

            // Выполню действия после успешной регистрации
            ProcessSuccess();
        }

        private void UpdateUser()
        {
            CurrentUser.lastName = txtFullName.Text.Trim();
            CurrentUser.phone = txtPhone.Text.Trim();
            CurrentUser.email = txtEmail.Text.Trim();
            CurrentUser.RequestedOrganization.LawAddress  = txtOrganizationAddress.Text.Trim();
            CurrentUser.RequestedOrganization.DirectorFullName  = txtOrganizationChiefName.Text.Trim();
            CurrentUser.RequestedOrganization.Fax  = txtOrganizationFax.Text.Trim();
            CurrentUser.RequestedOrganization.OwnerDepartment = txtOrganizationFounderName.Text.Trim();
            CurrentUser.RequestedOrganization.FullName  = txtOrganizationName.Text.Trim();
            CurrentUser.RequestedOrganization.Phone  = txtOrganizationPhone.Text.Trim();
            CurrentUser.RequestedOrganization.Region=new CatalogElement( Convert.ToInt32(ddlOrganizationRegion.SelectedItem.Value));
            CurrentUser.hasCrocEgeIntegration = null;
            if (hfEtalonOrgID.Value == "")
                CurrentUser.RequestedOrganization.OrganizationId = null;
            else CurrentUser.RequestedOrganization.OrganizationId = int.Parse(hfEtalonOrgID.Value);


            OrgUserDataAccessor.UpdateOrCreate(CurrentUser);


           // UserAccount u;
            //u.SetFullName(
            //CurrentUser.SetFullName(txtFullName.Text.Trim());
            //CurrentUser.Phone = txtPhone.Text.Trim();
            //CurrentUser.Email = txtEmail.Text.Trim();
            //CurrentUser.OrganizationAddress = txtOrganizationAddress.Text.Trim();
            //CurrentUser.OrganizationChiefName = txtOrganizationChiefName.Text.Trim();
            //CurrentUser.OrganizationFax = txtOrganizationFax.Text.Trim();
            //CurrentUser.OrganizationFounderName = txtOrganizationFounderName.Text.Trim();
            //CurrentUser.OrganizationName = txtOrganizationName.Text.Trim();
            //CurrentUser.OrganizationPhone = txtOrganizationPhone.Text.Trim();
            //CurrentUser.OrganizationRegionId = Convert.ToInt32(ddlOrganizationRegion.SelectedItem.Value);
            //CurrentUser.HasCrocEgeIntegration = null;
            //if (hfEtalonOrgID.Value == "")
            //    CurrentUser.EtalonOrgId = null;
            //else CurrentUser.EtalonOrgId = int.Parse(hfEtalonOrgID.Value);
            
            //CurrentUser.Update();
        }

        private void ProcessSuccess()
        {
            // Перейду на страницу успеха
            Response.Redirect(SuccessUri, true);
        }

        private void SendNotification()
        {
            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.Consideration);
            EmailMessage message = template.ToEmailMessage();
            message.To = CurrentUser.email;
            message.Params = Utility.CollectEmailMetaVariables(CurrentUser);

            // Отправлю уведомление
            TaskManager.SendEmail(message);
        }
    }
}