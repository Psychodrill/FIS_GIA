using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Core.Organizations;
using Esrp.Core;
using Esrp.Core.ApplicationFCT;
using FCTApplication = Esrp.Core.ApplicationFCT.ApplicationFCT;
using Esrp.Utility;
using System.IO;

namespace Esrp.Web.ApplicationFCT
{
    public partial class AppStep1 : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            if (org == null)
            {
                Response.Redirect("AppNoAccess.aspx");
                return;
            }

            this.EMail.Text = User.Identity.Name;            
            this.BriefName.Text = org.ShortName;
            this.FullName.Text = org.FullName;
            this.RealAddress.Text = org.FactAddress;
            this.OrgPhone.Text = org.Phone;

            FCTApplication app = ApplicationFCTDataAccessor.Get(org.Id);
            if (app != null)
            {
                BindData(app);
                Session.Add("ScanOrder", app.ScanOrder);
                Session.Add("OrderContentType", app.ScanOrderContentType);
            }
            else
            {
                rblIsThereAttestat.SelectedIndex = 1;
                ScanOrderText.Text = "Файл не сохранен";
            }            
        }





        protected void ShowOrder(object sender, EventArgs e)
        {            
            string fname, ctype, tmp;
            Stream stream;          

                
            if (!string.IsNullOrEmpty(this.Order4Person.FileName))
            {
                fname = Order4Person.FileName;
                stream = Order4Person.FileContent;
                ctype = Order4Person.PostedFile.ContentType;
            }
            else
            {
                if (ScanOrderText.Text == "Файл не сохранен")
                    return;

                if (Session["ScanOrder"] == null)
                    return;
                stream = new MemoryStream((byte[])Session["ScanOrder"]);
                tmp = (string)Session["OrderContentType"];
                string[] strs = tmp.Split('|');
                ctype = strs[0];
                fname = strs[1];
            }
            ResponseWriter.WriteStream(fname, ctype, stream);
        }


        #region data worker

        void BindData(FCTApplication app)
        {
            FIO.Text = app.PersonFullName;
            Position.Text = app.PersonPosition;
            WorkPhone.Text = app.PersonWorkPhone;
            MPhone.Text = app.PersonMobPhone;
            EMail4Person.Text = app.PersonEmail;
            if (app.IsThereAttestatK1More)
                rblIsThereAttestat.SelectedValue = "1";
            else
                rblIsThereAttestat.SelectedValue = "0";

            NumARMs.Text = app.NumARMs.ToString();
            NumPD.Text = app.NumPDNs.ToString();

            if ((app.ScanOrder != null) && (app.ScanOrderContentType != null))
            {
                string[] strs = app.ScanOrderContentType.Split('|');
                ScanOrderText.Text = "Сохраненный файл: " + strs[1];                
            }
            else
                ScanOrderText.Text = "Файл не сохранен";            
        }

        void GetData(FCTApplication app)
        {

            if (!string.IsNullOrEmpty(Order4Person.FileName) && this.Order4Person.FileBytes.Length > 0)
            {
                app.ScanOrderContentType = Order4Person.PostedFile.ContentType + "|" + Order4Person.FileName;
                app.ScanOrder = Order4Person.FileBytes;
            }

            app.PersonFullName = FIO.Text;
            app.PersonPosition = Position.Text;
            app.PersonWorkPhone = WorkPhone.Text;
            app.PersonMobPhone = this.MPhone.Text;
            app.PersonEmail = EMail4Person.Text;
            if (rblIsThereAttestat.SelectedValue == "1")
              app.IsThereAttestatK1More = true; 
            else
              app.IsThereAttestatK1More = false;
            app.NumARMs = Convert.ToInt32(NumARMs.Text);
            app.NumPDNs = Convert.ToInt32(NumPD.Text);
        }


        #endregion


        protected void ValidateStep1(object sender, EventArgs e)
        {           
            // проверяем валидность контролов страницы
            if (ScanOrderText.Text != "Файл не сохранен")
            {
                RequiredFieldValidator0.Enabled = false;
            }

            Validate();

            if (!Page.IsValid)         
                return;

            FCTApplication app = null;
            Organization org;

            org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            if (org != null)
            {
                app = ApplicationFCTDataAccessor.Get(org.Id);
                if (app == null)
                {
                    app = new FCTApplication();
                    app.OrganizationID = org.Id;
                    app.FillingStage = 1;
                }
            }

            GetData(app);
            ApplicationFCTDataAccessor.InsertOrUpdate(app);

            Session.Remove("ScanOrder");
            Session.Remove("OrderContentType");


            Response.Redirect("AppStep2.aspx");                                
        }


    }
}