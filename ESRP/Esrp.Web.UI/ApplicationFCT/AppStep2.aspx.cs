using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Core.CatalogElements;
using Esrp.Core.Organizations;
using FCTApplication = Esrp.Core.ApplicationFCT.ApplicationFCT;
using Esrp.Core.ApplicationFCT;
using System.Web.UI.HtmlControls;

namespace Esrp.Web.ApplicationFCT
{
    public partial class AppStep2 : System.Web.UI.Page
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

            InitDicts();

            FCTApplication app = ApplicationFCTDataAccessor.Get(org.Id);
            if (app == null)
            {
                Response.Redirect("AppEnter.aspx");
                return;
            }

            // Устанавливаем видимость рядов для АРМ IP вне зависимости от типа адреса (здесь оно не меняется)
            // app.NumARMs устанавливается на предыдущей странице

            SetIPRows(app.NumARMs);

            if (app.DictOperationSystemID != null)
                BindData(app);
            else
            {
                ddlIPType.SelectedValue = "1";
            }

        }


        void SetIPRows(int nARMs)
        {
            if (nARMs < 9)
                trIP9.Visible = false;
            if (nARMs < 8)
                trIP8.Visible = false;
            if (nARMs < 7)
                trIP7.Visible = false;
            if (nARMs < 6)
                trIP6.Visible = false;
            if (nARMs < 5)
                trIP5.Visible = false;
            if (nARMs < 4)
                trIP4.Visible = false;
            if (nARMs < 3)
                trIP3.Visible = false;
            if (nARMs < 2)
                trIP2.Visible = false;
            trIP1.Visible = true;
        }


        void InitDicts ()
        {
                this.ddlOperationSystem.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.DictOperationSystem);
                this.ddlOperationSystem.DataBind();

                this.ddlAntivirus.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.DictAntivirus);
                this.ddlAntivirus.DataBind();

                this.ddlUnauthAccessProtect.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.DictUnauthAccessProtect);
                this.ddlUnauthAccessProtect.DataBind();

                this.ddlElectronicLock.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.DictElectronicLock);
                this.ddlElectronicLock.DataBind();

                this.ddlTNScreen.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.DictTNScreen);
                this.ddlTNScreen.DataBind();

                this.ddlVipNetCrypto.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.DictVipNetCrypto);
                this.ddlVipNetCrypto.DataBind();

                FISLogin.Text = User.Identity.Name;         
        }


        void HideShowArmIP (string[] currentIPs, int nIndex, TextBox tbIP, int nNumArms)
        {
            if (nNumArms <= nIndex)
            {                
                return;
            }            

            if (currentIPs.Length > nIndex)
              tbIP.Text = currentIPs[nIndex];
            else
              tbIP.Text = string.Empty;
        }

        void BindData(FCTApplication app)
        {
            ddlOperationSystem.SelectedValue = Convert.ToString(app.DictOperationSystemID);
            if ((bool)app.IsIPStatic)
              ddlIPType.SelectedValue = "1";
            else
              ddlIPType.SelectedValue = "0";
            
            if ((bool)app.IsIPStatic)
            {
                string[] IPs = app.IPAddress.Split('|');                
                IP1.Text = IPs[0];
                HideShowArmIP(IPs, 1, IP2, app.NumARMs);
                HideShowArmIP(IPs, 2, IP3, app.NumARMs);
                HideShowArmIP(IPs, 3, IP4, app.NumARMs);
                HideShowArmIP(IPs, 4, IP5, app.NumARMs);
                HideShowArmIP(IPs, 5, IP6, app.NumARMs);
                HideShowArmIP(IPs, 6, IP7, app.NumARMs);
                HideShowArmIP(IPs, 7, IP8, app.NumARMs);
                HideShowArmIP(IPs, 8, IP9, app.NumARMs);                  
                //IPMask4ARMs.Text = app.IPMask4ARMs;
            }
            
            FISLogin.Text = app.FISLogin;
            ddlAntivirus.SelectedValue = Convert.ToString(app.DictAntivirusID);
            ddlUnauthAccessProtect.SelectedValue = Convert.ToString(app.DictUnauthAccessProtectID);
            ddlElectronicLock.SelectedValue = Convert.ToString(app.DictElectronicLockID);
            ddlTNScreen.SelectedValue = Convert.ToString(app.DictTNScreenID);
            IP4TNS.Text = app.IP4TNS;
            ddlVipNetCrypto.SelectedValue = Convert.ToString(app.DictVipNetCryptoID);
        }

        string GetIPsText(int nNumArms)
        {
            string strRes = IP1.Text;
            if (!trIP2.Visible || (nNumArms < 2))
               return strRes;
            strRes += "|" + IP2.Text;
            if (!trIP3.Visible || (nNumArms < 3))
                 return strRes;
             strRes += "|" + IP3.Text;
             if (!trIP4.Visible || (nNumArms < 4))
                 return strRes;
             strRes += "|" + IP4.Text;
             if (!trIP5.Visible || (nNumArms < 5))
                 return strRes;
             strRes += "|" + IP5.Text;
             if (!trIP6.Visible || (nNumArms < 6))
                 return strRes;
             strRes += "|" + IP6.Text;
             if (!trIP7.Visible || (nNumArms < 7))
                 return strRes;
             strRes += "|" + IP7.Text;
             if (!trIP8.Visible || (nNumArms < 8))
                 return strRes;
             strRes += "|" + IP8.Text;
             if (!trIP9.Visible || (nNumArms < 9))
                 return strRes;
             strRes += "|" + IP9.Text;
             return strRes;
        }



        FCTApplication GetData(int OrgID)
        {

            FCTApplication app = ApplicationFCTDataAccessor.Get(OrgID);
            if (app == null)
                return null;

            app.DictOperationSystemID = (int?)Convert.ToInt32(ddlOperationSystem.SelectedValue);
            if (ddlIPType.SelectedValue == "1")
                app.IsIPStatic = true;
            else
                app.IsIPStatic = false;

            if ((bool)app.IsIPStatic)
                app.IPAddress = GetIPsText(app.NumARMs);
            else
                app.IPAddress = null;

            app.FISLogin = FISLogin.Text;

            if (ddlOperationSystem.Items[ddlOperationSystem.SelectedIndex].Text == "Linux в составе VipNet Terminal")
            {
                app.DictAntivirusID = null;
                app.DictElectronicLockID = null;
                app.DictTNScreenID = null;
                app.DictUnauthAccessProtectID = null;
                app.DictVipNetCryptoID = null;                
            }
            else
            {
                app.DictAntivirusID = (int?)Convert.ToInt32(ddlAntivirus.SelectedValue);
                app.DictUnauthAccessProtectID = (int?)Convert.ToInt32(ddlUnauthAccessProtect.SelectedValue);
                app.DictElectronicLockID = (int?)Convert.ToInt32(ddlElectronicLock.SelectedValue);
                app.DictTNScreenID = (int?)Convert.ToInt32(ddlTNScreen.SelectedValue);                
                app.DictVipNetCryptoID = (int?)Convert.ToInt32(ddlVipNetCrypto.SelectedValue);
            }
            app.IP4TNS = IP4TNS.Text;
            app.FillingStage = 2;

            return app;
        }

        void HideAllIPRows()
        {
            if (trIP1.Visible)
                trIP1.Visible = false;
            if (trIP2.Visible)
                trIP2.Visible = false;
            if (trIP3.Visible)
                trIP3.Visible = false;
            if (trIP4.Visible)
                trIP4.Visible = false;
            if (trIP5.Visible)
                trIP5.Visible = false;
            if (trIP6.Visible)
                trIP6.Visible = false;
            if (trIP7.Visible)
                trIP7.Visible = false;
            if (trIP8.Visible)
                trIP8.Visible = false;
            if (trIP9.Visible)
                trIP9.Visible = false;
        }



        protected void ValidateStep2(object sender, EventArgs e)
        {
            // в случае если выбран динамический адрес - нужно скрыть все адреса АРМов чтобы запретить валидаторы
            if (ddlIPType.SelectedValue == "0")
                HideAllIPRows();

            Validate();

            if (!Page.IsValid)
                return;


            Organization org = OrganizationDataAccessor.GetByLogin(User.Identity.Name);
            if (org != null)
            {
                FCTApplication app = GetData(org.Id);
                if (app != null)
                    ApplicationFCTDataAccessor.InsertOrUpdate(app);
            }
            Response.Redirect("AppSuccess.aspx");
        }
    }
}