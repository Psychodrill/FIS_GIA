using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Administration;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using Application = GVUZ.Model.Entrants.Application;

namespace GVUZ.Model.Applications
{
	/// <summary>
	/// Проверка заявлений и свидетельств ЕГЭ
	/// </summary>
	public class ApplicationValidator
	{
		public const int EgeDocumentTypeId = 2;

		public List<ApplicationValidationErrorInfo> ValidateApplication(int applicationId, bool withEge = true)
		{
			List<EgeSubjectValidateErrorList> raw;
			Dictionary<int, ApplicationValidationErrorInfo> infos;
			return ValidateApplication(applicationId, withEge, null, out raw, out infos);
		}

		/// <summary>
		/// 	Проверяет заявление (правило 5/3, паспорт, свидетельство о ЕГЭ). Возвращает пустой список, если все проверки прошли успешно или строки с ошибками.
		/// </summary>
		public List<ApplicationValidationErrorInfo> ValidateApplication(int applicationId, bool withEge, string userName, out List<EgeSubjectValidateErrorList> egeValidationErrorsRaw, out Dictionary<int, ApplicationValidationErrorInfo> egeCheckResults)
		{
            var errors = new HashSet<ApplicationValidationErrorInfo>();
			egeValidationErrorsRaw = null;
			egeCheckResults = null;
            
            try
            {
                Application application;
                using (var entities = new EntrantsEntities())
                {
                    application = entities.Application.Single(x => x.ApplicationID == applicationId);
                }

                // проверяем правило 5/3
                errors.UnionWith(ApplicationCountValidator.ValidateOtherApplicationsCount(application));

                //проверяем льготу
                string benefitErrorMessage;
                if (ApplicationCountValidator.IsCommonBenefitMoreThanOnce(application.ApplicationID,application.RegistrationDate, out benefitErrorMessage))
                    errors.Add(new ApplicationValidationErrorInfo(6, benefitErrorMessage));

                // Проверяем льготы "Без всупительных испытаний" и "Приравнены к макс. баллу ЕГЭ"
                // на соответствие требованиям о минимально необходимых баллах.

                using (var entities = new EntrantsEntities())
                {
                    var globalsCheckResult = entities.CheckGlobalBenefitsEGEMinValues(application.ApplicationID);
                    var subjectCkeckResult = entities.CheckSubjectEGEMinValues(application.ApplicationID);

                    foreach (string s in globalsCheckResult)
                        errors.Add(new ApplicationValidationErrorInfo(7, s));

                    foreach (string s in subjectCkeckResult)
                        errors.Add(new ApplicationValidationErrorInfo(7, s));
                }

                using (var entities = new EntrantsEntities())
                {
                    application = entities.Application
                        .Include(x => x.Entrant)
                        .Include(x => x.Entrant.EntrantDocument_Identity)
                        .Include(x => x.Institution)
                        .Single(x => x.ApplicationID == applicationId);

                    // проверяем паспортные данные
                    errors.UnionWith(entities.ValidatePassport(application).Select(x => new ApplicationValidationErrorInfo(2, x)));

                    // проверяем свидетельства о ЕГЭ
                    if (withEge)
                    {
                        egeCheckResults = new ApplicationEgeValidator(entities, application).ValidateEgeDocuments(userName, out egeValidationErrorsRaw, false, null);
                        errors.UnionWith(egeCheckResults.Values.Where(error => !string.IsNullOrEmpty(error.Message)).ToList());
                    }

                    application.LastCheckDate = DateTime.Now;
                    entities.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                throw;
            }

			return errors.ToList();
		}
    }

#warning Юсупов: оставил для GVUZ.Web - чтобы интерфейс обращений к методам не менять
    public static class ApplicationValidatorExtensions
    {
#warning Юсупов: Какой-то непонятный метод для веб-сервиса InformationService.asmx
        /// <summary>
		/// Возвращает XML, содержащий ответ от сервиса проверки ФБС.
		/// </summary>
		public static EgeResultAndStatus GetEgeInformation(EgeQuery query, string pinCode)
		{
            return  ApplicationEgeInfoProvider.GetEgeInformation(query, pinCode);
		}

		/// <summary>
		/// Проверка свидетельств ЕГЭ
		/// </summary>
        public static Dictionary<int, ApplicationValidationErrorInfo> ValidateEgeDocuments(this EntrantsEntities dbContext,
            Application application, bool findCertificatesOfCurrentYearOnly, int? etId)
		{
			List<EgeSubjectValidateErrorList> list;
            return new ApplicationEgeValidator(dbContext, application).ValidateEgeDocuments(null, out list, findCertificatesOfCurrentYearOnly, etId);
		}
         
		/// <summary>
		/// Получение документов ЕГЭ по ФИО абитуриента и номеру сертификата
		/// </summary>
        public static EGEDocumentViewModel GetEgeDocumentByFIO(this EntrantsEntities dbContext, Application application, string certificateNumber, string userName, out List<ApplicationValidationErrorInfo> errors)
        {
            EgeCertificate cert;
            return new ApplicationEgeValidator(dbContext,application).GetEgeDocumentByFIO(null,  certificateNumber, userName, out errors, out cert);
        } 
	}
}