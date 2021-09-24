namespace Fbs.Web.Controls
{
    using System;
    using System.Web.UI;

    using Fbs.Core.Organizations;

    /// <summary>
    /// Просмотр организации или ее версии
    /// </summary>
    public partial class OrganizationView : UserControl
    {
        #region Public Properties

        /// <summary>
        /// Сообщение для вывода в заголовке
        /// </summary>
        public string Message
        {
            get
            {
                return (string)this.ViewState["Message"];
            }

            set
            {
                this.ViewState["Message"] = value;
            }
        }

        /// <summary>
        /// id организации
        /// </summary>
        public int? OrganizationId
        {
            get
            {
                object o = this.ViewState["OrganizationId"];
                if (o == null)
                {
                    return null;
                }

                return int.Parse(o.ToString());
            }

            set
            {
                this.ViewState["OrganizationId"] = value;
            }
        }

        /// <summary>
        /// версия организации
        /// </summary>
        public int? Version
        {
            get
            {
                object o = this.ViewState["Version"];
                if (o == null)
                {
                    return null;
                }

                return int.Parse(o.ToString());
            }

            set
            {
                this.ViewState["Version"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// установить поля организации
        /// </summary>
        /// <param name="e">
        /// e
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            if (Page.IsPostBack || (this.OrganizationId ?? 0) == 0)
            {
                return;
            }

            Organization org;
            if ((this.Version ?? 0) > 0)
            {
                org = OrganizationDataAccessor.Get(this.OrganizationId.Value, this.Version.Value);
            }
            else
            {
                org = OrganizationDataAccessor.Get(this.OrganizationId.Value);
            }
            
            // Заполняем простые текстовые поля
            this.lblIsLogCheckEvent.Text = org.DisableLog ? "Да" : "Нет";
            LFullName.Text = org.FullName;
            LShortName.Text = org.ShortName;

            LINN.Text = org.INN;
            LOGRN.Text = org.OGRN;

            LOwnerDepartment.Text = org.DepartmentShortName;
            LDirectorName.Text = org.DirectorFullName;
            LDirectorPosition.Text = org.DirectorPosition;

            LAccredCert.Text = org.AccreditationSertificate;

            LFactAddress.Text = org.FactAddress;
            LJurAddress.Text = org.LawAddress;

            LCityCode.Text = org.PhoneCityCode;
            LPhone.Text = org.Phone;
            LFax.Text = org.Fax;
            LEMail.Text = org.EMail;
            LSite.Text = org.Site;

            // Модель приемной кампании
            string modelName = org.RCModelId != 999 ? org.RCModelName : org.RCDescription;
            lblModelName.Text = string.IsNullOrEmpty(modelName) ? "—" : modelName;

            // Сведения об объеме и структуре приема
            lblFBFullTime.Text = org.CNFBFullTime.ToString();
            lblFBEvening.Text = org.CNFBEvening.ToString();
            lblFBPostal.Text = org.CNFBPostal.ToString();
            lblPayFullTime.Text = org.CNPayFullTime.ToString();
            lblPayEvening.Text = org.CNPayEvening.ToString();
            lblPayPostal.Text = org.CNPayPostal.ToString();

            //Заполняем списки и чекбокс
            LRegion.Text = org.Region.Name;
            LOrgLevel.Text = org.OrgType.Name;
            LOrgKind.Text = org.Kind.Name;

            LOPF.Text = org.IsPrivate ? "Негосударственный" : "Государственный";
            LIsFilial.Text = org.IsFilial ? "Да" : "Нет";
            phMainOrgName.Visible = org.IsFilial;
            lblMainOrgName.Text = org.MainFullName;

            // Запоминаем идентификатор типа организации для того, 
            // что бы скрыть ненужные поля для учредителя
            hiddenKindId.Value = org.OrgType.Id.ToString();

            this.OrgStatusLabel.Text = org.Status.Name;
            if (org.Status.Id == (int)OrganizationStatus.Reorganized)
            {
                this.NewOrgNameLabel.Text = string.Format(
                    "Новая организация: <a href=\"/Administration/Organizations/UserDepartments/OrgCardInfo.aspx?OrgID={1}\">{0}</a>",
                    org.NewOrgFullName, org.NewOrgId);
            }
            else
            {
                this.NewOrgNameLabel.Visible = false;
            }

            this.EditLink.NavigateUrl =
               string.Format("/Administration/Organizations/Administrators/OrgCard_Edit.aspx?OrgID={0}", this.OrganizationId);
        }

        #endregion
    }
}