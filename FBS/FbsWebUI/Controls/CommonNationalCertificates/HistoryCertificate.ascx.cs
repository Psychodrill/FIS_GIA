namespace Fbs.Web.Controls.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Fbs.Core.CNEChecks;

    using FbsWebViewModel.CNEC;

    /// <summary>
    /// Контрол для отображения таблицы вида:
    /// ссылка на сертификат|год|статус
    /// </summary>
    public partial class HistoryCertificate : UserControl
    {
        #region Constants and Fields


        /// <summary>
        /// Настройка web.config 
        /// </summary>
        private bool IsOpenFbs
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableOpenedFbs"]);
            }
        }


        /// <summary>
        /// Номер текущего свидетельства
        /// </summary>
        public string CurrentCertificateNumber;

        #endregion

        #region Public Properties

        /// <summary>
        /// имя сертифицируемого
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// фамилия сертифицируемого
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// номер документа сертифицируемого (паспорта)	
        /// </summary>
        public string PassportNumber { get; set; }

        /// <summary>
        /// серия документа сертифицируемого (паспорта)
        /// </summary>
        public string PassportSeria { get; set; }

        /// <summary>
        /// отчетсво сертифицируемого
        /// </summary>
        public string PatronymicName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик события, которое вызывается после методом Select
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void OnSelectedCertificateList(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.ReturnValue != null && ((List<HistoryCertificateView>)e.ReturnValue).Count > 0)
            {
                this.pHistoryCertificate.Visible = true;
                if (this.IsOpenFbs)
                {
                    foreach (var cert in (List<HistoryCertificateView>)e.ReturnValue)
                    {
                        cert.Certificate.Url = cert.Certificate.Url.Replace("CheckResult", "CheckResultForOpenedFbs");
                        var hash = CheckUtil.GetCheckHash(this.Page.User.Identity.Name, cert.Certificate.Text);
                        cert.Certificate.Url = string.Format("{0}&check={1}", cert.Certificate.Url, hash);
                    }
                }
            }
            else
            {
                this.pHistoryCertificate.Visible = false;
            }
        }

        /// <summary>
        /// Обработчик события, которое вызывается перед методом Select
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void OnSelectingCertificateList(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["lastName"] = this.LastName;
            e.InputParameters["firstName"] = this.FirstName;
            e.InputParameters["patronymicName"] = this.PatronymicName;
            e.InputParameters["passportNumber"] = this.PassportNumber;
            e.InputParameters["passportSeria"] = this.PassportSeria;
            e.InputParameters["currentCertificateNumber"] = this.CurrentCertificateNumber;
        }

        #endregion
    }
}