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
	/// �������� ��������� � ������������ ���
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
		/// 	��������� ��������� (������� 5/3, �������, ������������� � ���). ���������� ������ ������, ���� ��� �������� ������ ������� ��� ������ � ��������.
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

                // ��������� ������� 5/3
                errors.UnionWith(ApplicationCountValidator.ValidateOtherApplicationsCount(application));

                //��������� ������
                string benefitErrorMessage;
                if (ApplicationCountValidator.IsCommonBenefitMoreThanOnce(application.ApplicationID,application.RegistrationDate, out benefitErrorMessage))
                    errors.Add(new ApplicationValidationErrorInfo(6, benefitErrorMessage));

                // ��������� ������ "��� ������������ ���������" � "���������� � ����. ����� ���"
                // �� ������������ ����������� � ���������� ����������� ������.

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

                    // ��������� ���������� ������
                    errors.UnionWith(entities.ValidatePassport(application).Select(x => new ApplicationValidationErrorInfo(2, x)));

                    // ��������� ������������� � ���
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

#warning ������: ������� ��� GVUZ.Web - ����� ��������� ��������� � ������� �� ������
    public static class ApplicationValidatorExtensions
    {
#warning ������: �����-�� ���������� ����� ��� ���-������� InformationService.asmx
        /// <summary>
		/// ���������� XML, ���������� ����� �� ������� �������� ���.
		/// </summary>
		public static EgeResultAndStatus GetEgeInformation(EgeQuery query, string pinCode)
		{
            return  ApplicationEgeInfoProvider.GetEgeInformation(query, pinCode);
		}

		/// <summary>
		/// �������� ������������ ���
		/// </summary>
        public static Dictionary<int, ApplicationValidationErrorInfo> ValidateEgeDocuments(this EntrantsEntities dbContext,
            Application application, bool findCertificatesOfCurrentYearOnly, int? etId)
		{
			List<EgeSubjectValidateErrorList> list;
            return new ApplicationEgeValidator(dbContext, application).ValidateEgeDocuments(null, out list, findCertificatesOfCurrentYearOnly, etId);
		}
         
		/// <summary>
		/// ��������� ���������� ��� �� ��� ����������� � ������ �����������
		/// </summary>
        public static EGEDocumentViewModel GetEgeDocumentByFIO(this EntrantsEntities dbContext, Application application, string certificateNumber, string userName, out List<ApplicationValidationErrorInfo> errors)
        {
            EgeCertificate cert;
            return new ApplicationEgeValidator(dbContext,application).GetEgeDocumentByFIO(null,  certificateNumber, userName, out errors, out cert);
        } 
	}
}