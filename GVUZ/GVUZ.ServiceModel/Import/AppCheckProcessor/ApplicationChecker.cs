using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.ExternalValidation;
using GVUZ.Model.Applications;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.Package;

namespace GVUZ.ServiceModel.Import.AppCheckProcessor
{
	/// <summary>
	/// Проверка заявлений из импорта
	/// </summary>
	public class ApplicationChecker
    {
#warning Дичайшие тормоза!!!
        public void CheckApplications(ImportPackage package, ImportedAppCheckResultPackage resPackage)
		{
			//берём все импортированные заявления из пакета
			string[] appIDPairs = (package.ImportedAppIDs ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Trim()).ToArray();
			var resList1 = new List<EgeDocumentCheckResultDto>();
			var resList2 = new List<GetEgeDocumentDto>();

		    var sw = new Stopwatch();
            if (appIDPairs.Length > 0)
            {
                sw.Start();
                LogHelper.Log.InfoFormat("=== Пакет {0}. Начало проверки {1} заявлений в ФБС", package.PackageID, appIDPairs.Length);
            }

		    //через подчёркивание предыдущий статус заявления. Нужен для определения необходимости проверки
			foreach (string appIDPair in appIDPairs)
			{
				EgeDocumentCheckResultDto crDto;
				GetEgeDocumentDto edDto;
				int appID;
				int statusID = 0;
				if (appIDPair.IndexOf('_') > 0)
				{
					var splittedAppID = appIDPair.Split('_');
					appID = splittedAppID[0].To(0);
					statusID = splittedAppID[1].To(0);
				}
				else
				{
					appID = appIDPair.To(0);
				}
                
			    CheckApplication(appID, statusID, package.UserLogin, out crDto, out edDto, false);
			    // формируем результирующий список
			    if (crDto != null) resList1.Add(crDto);
			    if (edDto != null) resList2.Add(edDto);
			}

            if (appIDPairs.Length > 0)
            {
                sw.Stop();
                LogHelper.Log.InfoFormat("=== Пакет {0}. Конец проверки {1} заявлений в ФБС. Время = {2} сек.", 
                    package.PackageID, appIDPairs.Length, sw.Elapsed.TotalSeconds);
            }

			// генерируем пакет проверки
			resPackage.EgeDocumentCheckResults = resList1.Count == 0 ? null : resList1.ToArray();
			resPackage.GetEgeDocuments = resList2.Count == 0 ? null : resList2.ToArray();
			if (resList1.Count == 0 && resList2.Count == 0)
			{
				resPackage.StatusCheckCode = "1";
				resPackage.StatusCheckMessage = "Не найдены заявления для проверки";
			}
			else
			{
                resPackage.StatusCheckCode = "0";
                resPackage.StatusCheckMessage = "Найдены заявления для проверки";			    
			}
		}

		/// <summary>
		/// Проверка одного заявления
		/// </summary>
		public static void CheckApplication(int appID, int prevStatusID, string userName, 
            out EgeDocumentCheckResultDto crDto, out GetEgeDocumentDto edDto, bool forced)
		{
            crDto = new EgeDocumentCheckResultDto();
			edDto = new GetEgeDocumentDto();
			using (var dbContext = new EntrantsEntities())
			{
                var appData = dbContext.Application.Where(x => x.ApplicationID == appID).Select(x =>
                    new { Application = x, x.ApplicationID, x.ApplicationNumber, x.RegistrationDate , x.StatusID}).FirstOrDefault();
                if (appData == null)
                {
                    crDto = null;
                    edDto = null;
                    return;
                }
                //В статусе отозвано не надо проверять
                if(appData.StatusID == Model.Entrants.ApplicationStatusType.Denied)
                {
                    crDto = null;
                    edDto = null;
                    return;
                }
                //2.	Проверка должна осуществляться только при передаче «Принятых» заявлений
                //5.	Когда «Принятое» заявление включается в приказ, проверка должна отсутствовать (включение в приказ пойдёт раньше проверки, и статус изменится)
                if (!forced && appData.Application.StatusID != Model.Entrants.ApplicationStatusType.Accepted)
                {
                    crDto = null;
                    edDto = null;
                    return;
                }
                //4.	Система должна проверить заявление на предыдущий статус.
                // Если он был «Не прошедшее проверку», то загрузить заявление в «Принятые» (проверка при этом не осуществляется). 
                if (!forced && prevStatusID == Model.Entrants.ApplicationStatusType.Failed)
                {
                    crDto = null;
                    edDto = null;
                    return;
                }

                // начальное заполнение ответа
                crDto.Application = new ApplicationShortRef { ApplicationNumber = appData.ApplicationNumber, RegistrationDateString = appData.RegistrationDate.ToUniversalTime().ToString("o") };
                edDto.Application = new ApplicationShortRef { ApplicationNumber = appData.ApplicationNumber, RegistrationDateString = appData.RegistrationDate.ToUniversalTime().ToString("o") };

                List<EgeSubjectValidateErrorList> egeDocumentsValidateList;
                Dictionary<int, ApplicationValidationErrorInfo> checkResults;

                // формируем строковый ответ от валидации
                var validateResult = new ApplicationValidator().ValidateApplication(appID, true, userName, out egeDocumentsValidateList, out checkResults);
                string validateApplicationResult = String.Join(", ", validateResult.Select(x => x.Message));

                var egeDocsList = new List<EgeDocumentDto>();
                var checkDocsList = new List<EgeDocumentCheckItemDto>();

                //смотрим общие ошибки от проверки
                string globalDocError = checkResults.ContainsKey(0) ? checkResults[0].Message : null;
                if (!String.IsNullOrEmpty(validateApplicationResult))
                    globalDocError = (globalDocError == null ? "" : globalDocError + ", ") + validateApplicationResult;
                edDto.Error = globalDocError;

                //берём документы заявления
                var dbData = dbContext.ApplicationEntranceTestDocument
                    .Where(x => x.ApplicationID == appID && x.SourceID == EntranceTestSource.EgeDocumentSourceId)
                    .Select(x => new { x.EntrantDocumentID, x.EntrantDocument.DocumentDate, x.EntrantDocument.DocumentNumber })
                    .Distinct().ToArray();

                //по каждому документу формируем ответ
                bool isProcessed = true;
                foreach (var dt in dbData)
                {
                    if (dt.EntrantDocumentID.HasValue)
                    {
                        var egeDocumentViewModel = dbContext.LoadEntrantDocument(dt.EntrantDocumentID.Value) as EGEDocumentViewModel;
                        if (egeDocumentViewModel != null)
                        {
                            //docs
                            egeDocumentViewModel.FillData(dbContext, true, null, null);
                            var dto = new EgeDocumentDto
                                                    {
                                                        CertificateNumber = egeDocumentViewModel.DocumentNumber,
                                                        TypographicNumber = egeDocumentViewModel.TypographicNumber,
                                                        Year = ((egeDocumentViewModel.DocumentDate ?? DateTime.MinValue).Year).ToString(),
                                                        Marks = egeDocumentViewModel.Subjects.Select(
                                                        x => new SubjectMarkDto
                                                            {
                                                                SubjectMark = x.Value.ToString(CultureInfo.InvariantCulture),
                                                                SubjectName = x.SubjectName,
                                                                SubjectID = x.SubjectID.ToString()
                                                            })
                                                            .ToArray(),
                                                        Status = "0"
                                                    };
                            egeDocsList.Add(dto);

                            //check
                            var dto2 = new EgeDocumentCheckItemDto
                                    {
                                        DocumentDate = (egeDocumentViewModel.DocumentDate ?? DateTime.MinValue).ToUniversalTime().ToString("o"),
                                        DocumentNumber = egeDocumentViewModel.DocumentNumber,
                                        StatusCode = "0",
                                        StatusMessage = "Cвидетельство найдено и проверено"
                                    };
                            var dt1 = dt;
                            
                            if (!string.IsNullOrEmpty(globalDocError))
                            {
                                isProcessed = true;
                                dto2.StatusCode = "1";
                                dto2.StatusMessage = globalDocError;
                            }

                            EgeSubjectValidateErrorList errorDetails = egeDocumentsValidateList.FirstOrDefault(x => 
                                x.EntrantDocumentID == dt1.EntrantDocumentID.Value);
                            if (errorDetails == null)
                            {
                                isProcessed = true;
                                dto2.StatusCode = "1";
                                dto2.StatusMessage = "Свидетельство не найдено в ФБС";
                                ////LogHelper.Log.ErrorFormat("Свидетельство не найдено в ФБС: {0}, EntrantDocumentID: {1}",
                                ////    dt1.DocumentNumber, dt1.EntrantDocumentID.Value);
                            }
                            else if (!errorDetails.IsLoaded || (errorDetails.IsLoaded && errorDetails.HasErrors))
                            {
                                isProcessed = true;
                                //заменяем сообщение на более детальное, если есть
                                if (!String.IsNullOrEmpty(errorDetails.MainError))
                                {
                                    dto2.StatusCode = "1";
                                    dto2.StatusMessage = errorDetails.MainError;
                                }
                                else
                                {
                                    //успешные/неуспешные оценки
                                    dto2.CorrectResults = egeDocumentViewModel.Subjects
                                        .Where(x => !errorDetails.Errors.Any(y => x.SubjectName.Equals(y.SubjectName, StringComparison.CurrentCultureIgnoreCase)))
                                        .Select(x => new CorrectResultItemDto
                                                        {
                                                            Score = x.Value.ToString(CultureInfo.InvariantCulture),
                                                            SubjectName = x.SubjectName
                                                        }).ToArray();

                                    dto2.IncorrectResults = egeDocumentViewModel.Subjects
                                        .Where(x => errorDetails.Errors.Any(y => x.SubjectName.Equals(y.SubjectName, StringComparison.CurrentCultureIgnoreCase)))
                                        .Select(x => new IncorrectResultItemDto
                                        {
                                            Score = x.Value.ToString(CultureInfo.InvariantCulture),
                                            Comment = errorDetails.Errors.First(y => x.SubjectName.Equals(y.SubjectName, StringComparison.CurrentCultureIgnoreCase)).Error,
                                            SubjectName = x.SubjectName
                                        }).ToArray();

                                    if (dto2.IncorrectResults.Any())
                                    {
                                        dto2.StatusCode = "2";
                                        dto2.StatusMessage = "Есть ошибки в оценках";
                                    }
                                }
                            }

                            if (isProcessed)
                                checkDocsList.Add(dto2);
                        }
                    }
                }

                edDto.EgeDocuments = egeDocsList.ToArray();

			    if (isProcessed)
			        crDto.EgeDocuments = checkDocsList.ToArray();
			    else crDto = null;

                //3.	В результате проверки заявления остаются «Принятыми», либо меняют статус на «Не прошедшие проверку».
                if (validateResult.Count == 0)
                {
                    appData.Application.StatusID = Model.Entrants.ApplicationStatusType.Accepted;
                    appData.Application.ViolationID = 0;
                    appData.Application.ViolationErrors = null;
                    appData.Application.StatusDecision = "";
                }
                else
                {
                    appData.Application.StatusID = Model.Entrants.ApplicationStatusType.Failed;
                    var anyErrorCode = validateResult.Select(y => y.Code).First();
                    appData.Application.ViolationID = anyErrorCode;
                    appData.Application.ViolationErrors = validateApplicationResult;
                }

                dbContext.SaveChanges();
			}
		}


        /// <summary>
        /// Проверка одного заявления
        /// </summary>
        public static void CheckSingleApplication(int _institutionId, CheckApp applicationInfo, string userName,
            out EgeDocumentCheckResultDto crDto, out GetEgeDocumentDto edDto)
        {
            crDto = new EgeDocumentCheckResultDto();
            edDto = new GetEgeDocumentDto();
            using (var dbContext = new EntrantsEntities())
            {
                /* Ищем заявление в БД по UID + номер или по UID + дата */

                var applications = dbContext.Application.Where(c => c.InstitutionID == _institutionId &&
                    c.ApplicationNumber == applicationInfo.ApplicationNumber &&
                    c.RegistrationDate == applicationInfo.RegistrationDateDate);

                switch (applications.Count())
                {
                    case 0:
                        throw new DataException("Заявление не найдено");
                    case 1:
                        break;
                    default:
                        throw new DataException("Найдено несколько заявлений с такими параметрами");
                }

                var appData = applications.Select(x =>
                        new { Application = x, x.ApplicationID, x.ApplicationNumber, x.RegistrationDate, x.StatusID, x.ViolationErrors }).FirstOrDefault();

                if (!string.IsNullOrEmpty(appData.ViolationErrors))
                {
                    throw new DataException(appData.ViolationErrors);
                }

                if (appData.StatusID == Model.Entrants.ApplicationStatusType.Denied/*//В статусе отозвано не надо проверять*/)
                {
                    throw new DataException("Не найдено заявление для проверки");
                }
     
                // начальное заполнение ответа
                crDto.Application = new ApplicationShortRef { ApplicationNumber = appData.ApplicationNumber, RegistrationDateString = appData.RegistrationDate.ToUniversalTime().ToString("o") };
                edDto.Application = new ApplicationShortRef { ApplicationNumber = appData.ApplicationNumber, RegistrationDateString = appData.RegistrationDate.ToUniversalTime().ToString("o") };

                List<EgeSubjectValidateErrorList> egeDocumentsValidateList;
                Dictionary<int, ApplicationValidationErrorInfo> checkResults;

                // формируем строковый ответ от валидации
                var validateResult = new ApplicationValidator().ValidateApplication(appData.ApplicationID, true, userName, out egeDocumentsValidateList, out checkResults);
                string validateApplicationResult = String.Join(", ", validateResult.Select(x => x.Message));

                var egeDocsList = new List<EgeDocumentDto>();
                var checkDocsList = new List<EgeDocumentCheckItemDto>();

                //смотрим общие ошибки от проверки
                string globalDocError = checkResults.ContainsKey(0) ? checkResults[0].Message : null;
                if (!String.IsNullOrEmpty(validateApplicationResult))
                    globalDocError = (globalDocError == null ? "" : globalDocError + ", ") + validateApplicationResult;
                edDto.Error = globalDocError;

                //берём документы заявления
                var dbData = dbContext.ApplicationEntranceTestDocument
                    .Where(x => x.ApplicationID == appData.ApplicationID && x.SourceID == EntranceTestSource.EgeDocumentSourceId)
                    .Select(x => new { x.EntrantDocumentID, x.EntrantDocument.DocumentDate, x.EntrantDocument.DocumentNumber })
                    .Distinct().ToArray();

                //по каждому документу формируем ответ
                bool isProcessed = true;
                foreach (var dt in dbData)
                {
                    if (dt.EntrantDocumentID.HasValue)
                    {
                        var egeDocumentViewModel = dbContext.LoadEntrantDocument(dt.EntrantDocumentID.Value) as EGEDocumentViewModel;
                        if (egeDocumentViewModel != null)
                        {
                            //docs
                            egeDocumentViewModel.FillData(dbContext, true, null, null);
                            var dto = new EgeDocumentDto
                            {
                                CertificateNumber = egeDocumentViewModel.DocumentNumber,
                                TypographicNumber = egeDocumentViewModel.TypographicNumber,
                                Year = ((egeDocumentViewModel.DocumentDate ?? DateTime.MinValue).Year).ToString(),
                                Marks = egeDocumentViewModel.Subjects.Select(
                                x => new SubjectMarkDto
                                {
                                    SubjectMark = x.Value.ToString(CultureInfo.InvariantCulture),
                                    SubjectName = x.SubjectName,
                                    SubjectID = x.SubjectID.ToString()
                                })
                                    .ToArray(),
                                Status = "0"
                            };
                            egeDocsList.Add(dto);

                            //check
                            var dto2 = new EgeDocumentCheckItemDto
                            {
                                DocumentDate = (egeDocumentViewModel.DocumentDate ?? DateTime.MinValue).ToUniversalTime().ToString("o"),
                                DocumentNumber = egeDocumentViewModel.DocumentNumber,
                                StatusCode = "0",
                                StatusMessage = "Cвидетельство найдено и проверено"
                            };
                            var dt1 = dt;

                            if (!string.IsNullOrEmpty(globalDocError))
                            {
                                isProcessed = true;
                                dto2.StatusCode = "1";
                                dto2.StatusMessage = globalDocError;
                            }

                            EgeSubjectValidateErrorList errorDetails = egeDocumentsValidateList.FirstOrDefault(x =>
                                x.EntrantDocumentID == dt1.EntrantDocumentID.Value);
                            if (errorDetails == null)
                            {
                                isProcessed = true;
                                dto2.StatusCode = "1";
                                dto2.StatusMessage = "Свидетельство не найдено в ФБС";
                                ////LogHelper.Log.ErrorFormat("Свидетельство не найдено в ФБС: {0}, EntrantDocumentID: {1}",
                                ////    dt1.DocumentNumber, dt1.EntrantDocumentID.Value);
                            }
                            else if (!errorDetails.IsLoaded || (errorDetails.IsLoaded && errorDetails.HasErrors))
                            {
                                isProcessed = true;
                                //заменяем сообщение на более детальное, если есть
                                if (!String.IsNullOrEmpty(errorDetails.MainError))
                                {
                                    dto2.StatusCode = "1";
                                    dto2.StatusMessage = errorDetails.MainError;
                                }
                                else
                                {
                                    //успешные/неуспешные оценки
                                    dto2.CorrectResults = egeDocumentViewModel.Subjects
                                        .Where(x => !errorDetails.Errors.Any(y => x.SubjectName.Equals(y.SubjectName, StringComparison.CurrentCultureIgnoreCase)))
                                        .Select(x => new CorrectResultItemDto
                                        {
                                            Score = x.Value.ToString(CultureInfo.InvariantCulture),
                                            SubjectName = x.SubjectName
                                        }).ToArray();

                                    dto2.IncorrectResults = egeDocumentViewModel.Subjects
                                        .Where(x => errorDetails.Errors.Any(y => x.SubjectName.Equals(y.SubjectName, StringComparison.CurrentCultureIgnoreCase)))
                                        .Select(x => new IncorrectResultItemDto
                                        {
                                            Score = x.Value.ToString(CultureInfo.InvariantCulture),
                                            Comment = errorDetails.Errors.First(y => x.SubjectName.Equals(y.SubjectName, StringComparison.CurrentCultureIgnoreCase)).Error,
                                            SubjectName = x.SubjectName
                                        }).ToArray();

                                    if (dto2.IncorrectResults.Any())
                                    {
                                        dto2.StatusCode = "2";
                                        dto2.StatusMessage = "Есть ошибки в оценках";
                                    }
                                }
                            }

                            if (isProcessed)
                                checkDocsList.Add(dto2);
                        }
                    }
                }

                edDto.EgeDocuments = egeDocsList.ToArray();

                if (isProcessed)
                    crDto.EgeDocuments = checkDocsList.ToArray();
                else crDto = null;

                //3.	В результате проверки заявления остаются «Принятыми», либо меняют статус на «Не прошедшие проверку».
                if (validateResult.Count == 0)
                {
                    appData.Application.StatusID = Model.Entrants.ApplicationStatusType.Accepted;
                    appData.Application.ViolationID = 0;
                    appData.Application.ViolationErrors = null;
                    appData.Application.StatusDecision = "";
                }
                else
                {
                    appData.Application.StatusID = Model.Entrants.ApplicationStatusType.Failed;
                    var anyErrorCode = validateResult.Select(y => y.Code).First();
                    appData.Application.ViolationID = anyErrorCode;
                    appData.Application.ViolationErrors = validateApplicationResult;
                }

                dbContext.SaveChanges();
            }
        }
	}
}
