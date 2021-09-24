namespace Esrp.Web.Administration.Organizations
{
    using System;

    using Esrp.Utility;

    using Esrp.Services;

    /// <summary>
    /// Письмо о переносе сроков
    /// </summary>
    public partial class LetterToReschedule : BasePage
    {
        private const string LetterQueryKeyOrgId = "orgId";
        private const string LetterQueryKeyVersionNumber = "versionNumber";
        private const string ErrorLetterNotFound = "Письмо не найдено";
        private readonly OrganizationService organizationService = new OrganizationService();

        private int OrgId
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[LetterQueryKeyOrgId]))
                {
                    throw new NullReferenceException(ErrorLetterNotFound);
                }

                int result;
                if (!int.TryParse(Request.QueryString[LetterQueryKeyOrgId], out result))
                {
                    throw new NullReferenceException(ErrorLetterNotFound);
                }

                return result;
            }
        }

        private int VersionNumber
        {
            get
            {
                int result;
                if (!int.TryParse(Request.QueryString[LetterQueryKeyVersionNumber], out result))
                {
                    return -1;
                }

                return result;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Загружу документ из БД
            var doc = this.VersionNumber > 0 ? this.organizationService.GetLetter(this.OrgId, this.VersionNumber) : this.organizationService.GetLetter(this.OrgId);

            // Проверю существование документа. Т.к. это административный раздел, то неактивные 
            // документы также доступны для просмотра.
            if (doc == null)
            {
                throw new NullReferenceException(ErrorLetterNotFound);
            }

            // Отдам документ в response
            ResponseWriter.WriteFile(doc.Name, doc.ContentType, doc.Content);
        }
    }
}