namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Web.UI.WebControls;

    using FbsServices;

    /// <summary>
    /// Страница уникальных проверок сертификатов университетами
    /// </summary>
    public partial class CheckHistoryForOpenedFbs : BasePage
    {
        #region Constants and Fields

        private readonly CNECService cnecService = new CNECService();

        private Guid certificateId;

        #endregion

        #region Properties

        /// <summary>
        /// id сертификата
        /// </summary>
        protected Guid CertificateId
        {
            get
            {
                if (this.certificateId == Guid.Empty)
                {
                    if (string.IsNullOrEmpty(this.Request.QueryString["certificateId"]))
                    {
                        throw new ArgumentException("certificateId");
                    }

                    this.certificateId = new Guid(Request.QueryString["certificateId"]);
                }

                return this.certificateId;
            }
        }

        /// <summary>
        /// номер сертификата
        /// </summary>
        protected string CertificateNumber
        {
            get
            {
                return this.Request.QueryString["certificateNumber"];
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Добавить инфу о сертификате в название страницы
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = string.Format(
                "История проверок свидетельства № {0} на {1}", this.CertificateNumber, DateTime.Now.ToShortDateString());
        }

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.orgPager.ItemCount = this.GetOrgCount();
        }

        /// <summary>
        /// добавить параметры в селект датасорса
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected void historyDataSource_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["certificateId"] = this.CertificateId;
            e.InputParameters["startRow"] = this.orgPager.PageNumber * this.orgPager.PageSize;
            e.InputParameters["maxRow"] = this.orgPager.PageSize;
        }

        /// <summary>
        /// добавить номера в строки в листвью
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void historyListView_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var lblOrgName = (Literal)e.Item.FindControl("lblOrgName");
            var firstRowAllIndex = this.orgPager.PageNumber * this.orgPager.PageSize;
            var firstRowPageIndex = ((ListViewDataItem)e.Item).DisplayIndex;
            lblOrgName.Text = string.Format("{0}. {1}", firstRowAllIndex + 1 + firstRowPageIndex, lblOrgName.Text);
        }

        private int GetOrgCount()
        {
            return this.cnecService.SelectCNECCheckHystoryCount(this.CertificateId);
        }

        #endregion
    }
}