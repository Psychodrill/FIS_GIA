using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Esrp.Core.CatalogElements;
using Esrp.Core.Deliveries;

namespace Esrp.Web.Administration.Deliveries
{
    public partial class Create : System.Web.UI.Page
    {
        private const string SuccessUri = "/Administration/Deliveries/List.aspx";
        private string PrevPage;
        private Delivery mCurrentDelivery;
        private Delivery CurrentDelivery
        {
            get
            {
                if (mCurrentDelivery == null)
                {
                    mCurrentDelivery = new Delivery();
                    DataBindCurrentDelivery();
                }
                return mCurrentDelivery;
            }
        }


        private bool IsOrgDelivery
        {
            get
            {
                return (Request.QueryString["Type"] == "Organizations");
            }
        }

        public new string Title
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath != null)
                {
                    if (!Request.UrlReferrer.LocalPath.Contains(Request.Url.LocalPath))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    PrevPage = (string)Session["BackLink.HRef"];
                    BackLink.HRef = PrevPage;
                }
                if (IsOrgDelivery)
                {

                    ChBRecipients.Items.Add(new ListItem(" Не зарегистрировавшиеся организации", DeliveryTypes.NotRegistredOrgs.ToString()));
                }
                else
                {

                    ChBRecipients.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.UserGroups);
                    ChBRecipients.DataBind();
                }
            }

            Title = "Новая рассылка по организациям";

        }

        private void DataBindCurrentDelivery()
        {
            CurrentDelivery.Title = TbTitle.Text;
            CurrentDelivery.Message = TbMessage.Text;
            CurrentDelivery.DeliveryDate = TbDate.Value;
            if (IsOrgDelivery)
            {
                CurrentDelivery.DeliveryType = DeliveryDataAccessor.TypeFromString(ChBRecipients.SelectedValue);
            }
            else
            {
                CurrentDelivery.DeliveryType = DeliveryTypes.Users;
                foreach (ListItem Item in ChBRecipients.Items)
                {
                    if (Item.Selected)
                    {
                        CurrentDelivery.RecipientCodes.Add(int.Parse(Item.Value));
                    }
                }
            }
        }

        private void ProcessSuccess()
        {
            Response.Redirect(BackLink.HRef, true);
        }

        protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ChBRecipients.SelectedItem == null)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Создам новую рассылку
            DeliveryDataAccessor.UpdateOrCreate(CurrentDelivery);

            // Выполню действия после успешного создания документа
            ProcessSuccess();
        }

        protected void validateDate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = this.TbDate.Value.HasValue;
        }
    }
}
