namespace Esrp.Web.Personal.Profile
{
    using System;
    using System.Configuration;
    using System.IO;

    using Esrp.Core;
    using Esrp.Core.RegistrationTemplates;
    using Esrp.Utility;

    using Esrp.Services;

    public partial class DocumentForConfirmation : BasePage
    {
    	const string FileContentType = "application/msword";

        protected void Page_Load(object sender, EventArgs e)
        {
        	const string FileName = @"Документ регистрации.doc";

            try
            {
                var virPath = string.Format(
                    "{0}{1}", Server.MapPath("~/"), ConfigurationManager.AppSettings["PathSystemsRegistrationTemplate"]);
                var docName = RegistrationTemplateFactory.GetWordDocument(Account.ClientLogin, virPath);

                // Отдам документ
                ResponseWriter.WriteFile(FileName, FileContentType, docName);

                File.Delete(docName);
            }
            catch (Exception ex)
            {
                throw new BllException(ex.Message, ex.InnerException);
            }
        }
    }
}
