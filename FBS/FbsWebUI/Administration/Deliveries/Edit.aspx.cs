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
using Fbs.Core.CatalogElements;
using Fbs.Core.Deliveries;

namespace Fbs.Web.Administration.Deliveries
{
    public partial class Edit : BasePage
    {
        private const string NewsQueryKey = "id";
        private const string DeliveryNotFound = "Рассылка не найдена";
        private const string SuccessUri = "/Administration/Deliveries/List.aspx";
        private Fbs.Core.Deliveries.Delivery mCurrentDelivery;

        private long DeliveryId
        {
            get
            {
                long result;
                if (!long.TryParse(Request.QueryString[NewsQueryKey], out result))
                    throw new NullReferenceException(DeliveryNotFound);
                return result;
            }
        }

        public Fbs.Core.Deliveries.Delivery CurrentDelivery
        {
            get
            {
                if (mCurrentDelivery == null)
                {
                    mCurrentDelivery = DeliveryDataAccessor.Get(DeliveryId);
                    if (mCurrentDelivery == null)
                        throw new NullReferenceException(DeliveryNotFound);
                }
                return mCurrentDelivery;
            }
        }

        private void DataBindCurrentDelivery()
        {
            CurrentDelivery.Title = TbTitle.Text;
            CurrentDelivery.Message = TbMessage.Text;
            CurrentDelivery.DeliveryDate = Convert.ToDateTime(TbDate.Text);
            if (CurrentDelivery.DeliveryType == DeliveryTypes.Users)
            {
                CurrentDelivery.RecipientCodes.Clear();//Не забыть очистить

                foreach (ListItem Item in ChBRecipients.Items)
                {
                    if (Item.Selected)
                    {
                        CurrentDelivery.RecipientCodes.Add(int.Parse(Item.Value));
                    }
                }
            }
            else
            {
                CurrentDelivery.DeliveryType =DeliveryDataAccessor.TypeFromString ( ChBRecipients.SelectedValue);
            }
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

            //Сохраню данные с формы
            DataBindCurrentDelivery();

            // Обновлю рассылку в БД
            DeliveryDataAccessor.UpdateOrCreate(CurrentDelivery);

            // Выполню действия после успешного редактирования рассылки
            ProcessSuccess();
        }


        private void ProcessSuccess()
        {
            Response.Redirect(SuccessUri, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (CurrentDelivery.DeliveryType == DeliveryTypes.Users)
                {
                    ChBRecipients.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.UserGroups);
                    ChBRecipients.DataBind();
                }
                else
                {

                    ChBRecipients.Items.Add(new ListItem(" Не зарегистрировавшиеся организации", DeliveryTypes.NotRegistredOrgs.ToString()));
                }
                TbTitle.Text = CurrentDelivery.Title;
                TbMessage.Text = CurrentDelivery.Message;
                TbDate.Text = CurrentDelivery.DeliveryDateString;
                SelectRecipientByDelivery();
            }

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование рассылки “{0}”", CurrentDelivery.Title);
        }

        private void SelectRecipientByDelivery()
        {
            if (CurrentDelivery.DeliveryType == DeliveryTypes.Users)
            {
                foreach (ListItem Item in ChBRecipients.Items)
                {
                    if (CurrentDelivery.RecipientCodes.Contains(int.Parse(Item.Value)))
                    {
                        Item.Selected = true;
                    }
                }
            }
            else
            {
                ChBRecipients.SelectedValue = CurrentDelivery.DeliveryType.ToString();
            }
        }
    }
}
