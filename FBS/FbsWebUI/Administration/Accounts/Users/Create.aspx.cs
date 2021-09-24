using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Core.Users ;
using Fbs.Utility;
using Fbs.Core.Organizations;

namespace Fbs.Web.Administration.Accounts.Users
{
    public partial class Create : System.Web.UI.Page
    {
        private const string SuccessUri = "/Administration/Accounts/Users/CreateSuccess.aspx?login={0}";

        private Organization FoundedOrg_;
        private Organization FoundedOrg
        {
            get
            {
                if (FoundedOrg_ == null)
                {
                    FoundedOrg_ = OrganizationDataAccessor.Get(OrgID);
                }
                return FoundedOrg_;
            }
        }

        private int OrgID
        {
            get
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["OrgID"]))
                {
                    int orgId;
                    if (int.TryParse(Page.Request.QueryString["OrgID"], out orgId))
                    {
                        return orgId;
                    }
                }
                return 0;
            }
        }

        private bool UserCreatesNewOrg()
        {
            return OrgID <= 0;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                return;
            }
            else
            {
                if (string.IsNullOrEmpty(Request.QueryString["OrgID"]))
                {
                    // Пользователь еще не выбирал организацию
                }
                else
                {
                    if (!UserCreatesNewOrg()) // если организация выбрана
                    {
                        ShowOrgFounded();

                    }
                }
            }

            // Нажатие на кнопку не должно инициировать проверку изменений на форме
            btnUpdate.Attributes.Add("handleClick", "false");
            //lbSearchSameUserAccounts.Attributes.Add("handleClick", "false");

            // Костыль: не удается выпонить javascript код в ответе сервера. Поэтому в hidden input 
            // jsCheck скриптом записываю значение.
            // Если значение есть, то javascript включен и кнопку пратать не надо.
            //if (Request.Form["jsCheck"] == string.Empty)
                //lbSearchSameUserAccounts.Style.Add("display", "none");
        }

        private void ShowOrgFounded()
        {

            txtOrganizationName.Text = FoundedOrg.FullName;
            txtOrganizationName.Enabled = false;
            txtOrganizationName.ToolTip = "Название организации зафиксировано.";

            ddlOrganizationRegion.SelectedValue = FoundedOrg.Region.Id.ToString();
            ddlOrganizationRegion.Enabled = false;
            ddlOrganizationRegion.ToolTip = "Регион организации зафиксирован.";

            txtOrganizationFounderName.Text = FoundedOrg.DepartmentShortName.ToString();
            txtOrganizationFounderName.Enabled = false;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // проверю валидность контролов страницы
            if (!Page.IsValid)
                return;

         //   UserAccount user;
            OrgUser user;
            string password;

            // регистрирую нового пользователя
            UpdateUser(out user, out password);

            // выполню действия после успешной регистрации
            ProcessSuccess(user, password);
        }

        protected void lbSearchSameUserAccounts_Click(object sender, EventArgs e)
        {
            /*
            //System.Threading.Thread.Sleep(2000);
            // Получу информацию повторах
            UserAccount[] userList = UserAccount.SearchSameUserAccount(txtOrganizationName.Text.Trim());

            if (userList.Length > 0)
            {
                // Сформирую строки с данными пользователей
                StringBuilder description = new StringBuilder();
                foreach (UserAccount user in userList)
                {
                    description.Append("<tr>");
                    description.AppendFormat("<td><a href=\"/Administration/Accounts/Users/Edit.aspx?login={0}\">{1}</a></td>",
                        user.Login, user.OrganizationName);
                    description.AppendFormat("<td>{0}</td><td style=\"white-space:nowrap;\">{1}</td>",
                        user.LastName, UserAccountExtentions.GetUserAccountStatusName(user.Status));
                    description.Append("</tr>");
                }

                //LiteralControl litTemplate = (LiteralControl)SameUserAccountsResultsTemplate.Controls[0];

                //string resultInfo = litTemplate.Text;
                //resultInfo = resultInfo.Replace("#USERSCOUNT#", userList.Length.ToString());
                //resultInfo = resultInfo.Replace("#USERSDESCRIPTION#", description.ToString());

                //SameUserAccountsResultPanel.Controls.Add(new LiteralControl(resultInfo));
            }
            else
            {
                // Отображу сообщение о том, что повторы не найдены
                LiteralControl litTemplate = (LiteralControl)SameUserAccountsNoResultTemplate.Controls[0];
                SameUserAccountsResultPanel.Controls.Add(new LiteralControl(litTemplate.Text));
            }
             */
        }

        private void UpdateUser(out UserAccount user, out string password)
        {
            // сгенерирую пароль               
            password = Utility.GeneratePassword();

            user = new UserAccount();
            user.Login = null; // уникальный логин вернется после выполнения процедуры
            user.Password = password;
            user.SetFullName(txtFullName.Text.Trim());
            user.Email = txtEmail.Text.Trim();
            user.OrganizationAddress = txtOrganizationAddress.Text.Trim();
            user.OrganizationChiefName = txtOrganizationChiefName.Text.Trim();
            user.OrganizationFax = txtOrganizationFax.Text.Trim();
            user.OrganizationFounderName = txtOrganizationFounderName.Text.Trim();
            user.OrganizationName = txtOrganizationName.Text.Trim();
            user.OrganizationPhone = txtOrganizationPhone.Text.Trim();
            user.OrganizationRegionId = Convert.ToInt32(ddlOrganizationRegion.SelectedItem.Value);
            user.Phone = txtPhone.Text.Trim();
            // если задан документ регистрации, то добавлю его
            if (!String.IsNullOrEmpty(fuRegistrationDocument.FileName) && fuRegistrationDocument.FileBytes.Length > 0)
            {
                user.RegistrationDocument.ContentType = fuRegistrationDocument.PostedFile.ContentType;
                user.RegistrationDocument.Extension = Path.GetExtension(fuRegistrationDocument.FileName);
                user.RegistrationDocument.Document = fuRegistrationDocument.FileBytes;
            }

            user.Update();


            //А тут надо будет заливать новые данные в организации2010 (создать ее)
        }

        private void UpdateUser(out OrgUser user, out string password)
        {
            // сгенерирую пароль               
            password = Utility.GeneratePassword();


            //     Fbs.Core.Users.OrgUserDataAccessor.UpdateOrCreate()
            user = new OrgUser();

            user.password  = password;
            user.lastName=txtFullName.Text.Trim();
            user.email = txtEmail.Text.Trim();
            user.RequestedOrganization.OrgType = new Fbs.Core.CatalogElements.CatalogElement((int)FoundedOrg.OrgType.Id); //5);//"Другое"
            user.RequestedOrganization.LawAddress = txtOrganizationAddress.Text.Trim();
            user.RequestedOrganization.DirectorFullName = txtOrganizationChiefName.Text.Trim();
            user.RequestedOrganization.Fax = txtOrganizationFax.Text.Trim();
            user.RequestedOrganization.OwnerDepartment = txtOrganizationFounderName.Text.Trim();
            user.RequestedOrganization.FullName = txtOrganizationName.Text.Trim();
            user.RequestedOrganization.Phone = txtOrganizationPhone.Text.Trim();
            user.RequestedOrganization.Region = new Fbs.Core.CatalogElements.CatalogElement(Convert.ToInt32(ddlOrganizationRegion.SelectedItem.Value));
            user.phone = txtPhone.Text.Trim();
            // если задан документ регистрации, то добавлю его
            if (!String.IsNullOrEmpty(fuRegistrationDocument.FileName) && fuRegistrationDocument.FileBytes.Length > 0)
            {
                user.registrationDocumentContentType = fuRegistrationDocument.PostedFile.ContentType;
                // user.RegistrationDocument.Extension = Path.GetExtension(fuRegistrationDocument.FileName);
                user.registrationDocument = fuRegistrationDocument.FileBytes;
            }
            //Проставляем организацию у пользователя
            user.RequestedOrganization.OrganizationId = FoundedOrg.Id;

            OrgUserDataAccessor.UpdateOrCreate(user);
        }

        private void ProcessSuccess(UserAccount user, string password)
        {
            // подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.Registration);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.Email;
            message.Params = Utility.CollectEmailMetaVariables(user, password, Utility.GetSeverPath(Request));
            // отправлю email сообщение
            TaskManager.SendEmail(message);

            // перейду на страницу успеха
            Response.Redirect(String.Format(SuccessUri, user.Login), true);
        }

        private void ProcessSuccess(OrgUser  user, string password)
        {
            // подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.Registration);
            EmailMessage message = template.ToEmailMessage();
            message.To = user.email;
            message.Params = Utility.CollectEmailMetaVariables(user, password, Utility.GetSeverPath(Request));
            // отправлю email сообщение
            TaskManager.SendEmail(message);

            // перейду на страницу успеха
            Response.Redirect(String.Format(SuccessUri, user.login), true);
        }
    }
}
