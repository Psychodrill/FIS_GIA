using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;

namespace GVUZ.Model.Applications
{
	public static class ApplicationCountValidator
	{
		/// <summary>
		/// 	Возвращает пустой список, если выполняется правило 5/3 или строки с ошибками.
		/// </summary>
        internal static IEnumerable<ApplicationValidationErrorInfo> ValidateOtherApplicationsCount(Application application)
        {
#warning Заходим и бросам исключение.... зачем тогда нужна эта финкция?
            throw new InvalidOperationException("Некорректный вызов gvuz_ValidateOtherApplicationsCount!");
            int year = application.RegistrationDate.Year;

            using (var entities = new EntrantsEntities())
            {
                var cmd = @"exec gvuz_ValidateOtherApplicationsCount @appilationId={0}, @dateFrom={1}, @dateTO={2}";

                var res = entities.ExecuteStoreQuery<int>(cmd, application.ApplicationID, new DateTime(year, 1, 1), new DateTime(year, 12, 31, 23, 59, 59)).ToArray();


                int totalLimit = AppSettings.Get("Application.TotalLimit", 5);
                //#40120 - считаем только разные ВУЗы
                if (res.Distinct().Count() > totalLimit)
                {
                    return new[]
					       	    {
					       		    new ApplicationValidationErrorInfo(1,
									    Messages.ApplicationValidator_TotalLimitExceeded.FormatWith(totalLimit, 
                                        res.Distinct().Count()))
					       	    };
                }

                return new ApplicationValidationErrorInfo[0];
            }
        }
		/// <summary>
		/// Проверка на использование льготы более одного раза
		/// </summary>
        public static bool IsCommonBenefitMoreThanOnce(int ApplicationID, DateTime RegistrationDate, out string errorMessage)
		{
            return IsCommonBenefitMoreThanOnce(ApplicationID, RegistrationDate, new short[] { 1, 4, 5 }, out errorMessage); //Зачисление без вступительных испытаний
		}
       
        public static bool IsCommonBenefitMoreThanOnce(int ApplicationID, DateTime RegistrationDate, int benefitID, out string errorMessage)
        {
            return IsCommonBenefitMoreThanOnce(ApplicationID, RegistrationDate, new [] { (short)benefitID }, out errorMessage);
        }

        public static bool IsCommonBenefitMoreThanOnce(int ApplicationID, DateTime RegistrationDate, short[] benefitID, out string errorMessage)
        {
            
            using (var entities = new EntrantsEntities())
            {
                Application app = entities.GetApplication(ApplicationID);
                ApplicationEntranceTestDocument existedDocument =
                    app.ApplicationEntranceTestDocument.FirstOrDefault(x => benefitID.ToList().Contains(x.BenefitID ?? 0));
                if (existedDocument != null)
                {
                    //using (new TransactionScope(TransactionScopeOption.RequiresNew,
                    //                            new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted}))
                    //{
                        int year = RegistrationDate.Year;
                        var query = (from ed in entities.EntrantDocument
                                     join aetd in entities.ApplicationEntranceTestDocument
                                         on ed.EntrantDocumentID equals aetd.EntrantDocumentID
                                     join a in entities.Application
                                         on aetd.ApplicationID equals a.ApplicationID
                                     join i in entities.Institution
                                         on a.InstitutionID equals i.InstitutionID
                                     where a.RegistrationDate >= new DateTime(year, 1, 1) &&
                                           a.RegistrationDate <= new DateTime(year, 12, 31) &&
                                           a.RegistrationDate <= RegistrationDate &&
                                           benefitID.Contains(aetd.BenefitID.Value) &&
                                           aetd.BenefitID.Value != 5 && //Преимущественное право на поступление - не проверяется
                                           existedDocument.EntrantDocumentID == ed.EntrantDocumentID &&
                                           a.ApplicationID != ApplicationID
                                     select
                                         new
                                             {
                                                 ExistedApplicationInstitutionID = a.InstitutionID,
                                                 a.ApplicationNumber,
                                                 i.BriefName,
                                                 ExistedApplicationStatusID = a.StatusID,
                                                 ExistedApplicationRegistrationDate = a.RegistrationDate
                                             });
                        var applications = query.ToArray();
                        //если больше одного заявления - ошибка
                        if (applications.Any())
                        {
                            foreach (var otherApp in applications)
                            {
                                if (otherApp.ExistedApplicationStatusID == 4)
                                {
                                    string messageTemplate = app.InstitutionID ==
                                                             otherApp.ExistedApplicationInstitutionID
                                                                 ? Messages
                                                                       .ApplicationValidator_CommonBenefitAlreadyUsedThisInstitution
                                                                 : Messages
                                                                       .ApplicationValidator_CommonBenefitAlreadyUsedAnotherInstitution;
                                    errorMessage = String.Format(messageTemplate, otherApp.ApplicationNumber,
                                                                 otherApp.BriefName);
                                    return true;
                                }
                            }
                        }
                    //}
                }
                errorMessage = null;
                return false;
            }
        }
	}
}