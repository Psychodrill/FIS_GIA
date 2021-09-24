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
    internal class ApplicationEgeValidator
    {
        internal const int EgeDocumentTypeId = 2;
        internal const string EgeNotFoundText = "Свидетельство о ЕГЭ не найдено в АИС ФБС.";
        internal const string EgeCancelledText = "Свидетельство о ЕГЭ аннулировано, новый номер свидетельства {0}.";
        internal static readonly CultureInfo MarkFormatter = CultureInfo.GetCultureInfo("ru-RU");

        internal ApplicationEgeValidator(EntrantsEntities dbContext, Application application)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            if (application == null)
                throw new ArgumentNullException("application");

            _dbContext = dbContext;
            _application = application;
        }

        private EntrantsEntities _dbContext;
        private Application _application;

        /// <summary>
        /// 	Возвращает массив с результатами проверки ЕГЭ EntranceTestItemID - ошибка
        /// </summary>
        internal Dictionary<int, ApplicationValidationErrorInfo> ValidateEgeDocuments(string userName,
                    out List<EgeSubjectValidateErrorList> checkErrorList, bool findCertificatesOfCurrentYearOnly, int? entranceTestItemId)
        {
            // MISSING: в таблице 
            var validationResult = new Dictionary<int, ApplicationValidationErrorInfo>();
            checkErrorList = new List<EgeSubjectValidateErrorList>();

            //Уровни, для которых проводим проверку ЕГЭ: бакалавриат, сокращенный бакалавриат, прикладной бакалавриат, специалитет
            IEnumerable<int> eduLevelsRequiringCheck = new List<int>() { 2, 3, 5, 19 };
            // TODO: if (!_application.ApplicationCompetitiveGroupItem.Any(x => eduLevelsRequiringCheck.Contains(x.CompetitiveGroupItem.EducationLevelID)))
#warning Это .... короче все остальной код в функции лесом
            if (true)
            {
                _application.LastEgeDocumentsCheckDate = DateTime.Now;
                _dbContext.SaveChanges();
                return validationResult;
            }

            var cachedContext = new ApplicationEgeContext(_dbContext, _application);
            if (!cachedContext.HasAnyEntrantTestDocuments)
                return validationResult;

            int? findCertificateOfSubjectId = null;
            if (entranceTestItemId.HasValue)
            {
                var entranceTestItem = _dbContext.EntranceTestItemC.SingleOrDefault(c => c.EntranceTestItemID == entranceTestItemId.Value);
                if (entranceTestItem != null)
                {
                    findCertificateOfSubjectId = entranceTestItem.SubjectID;
                }
            }

            // получаем все связанные св-ва о ЕГЭ из ФБС (по ФИО + паспорт, без номеров св-в), если ошибка - выходим.
            EgeResult egeResult = GetEgeInformation(_application, userName ?? UserHelper.GetAuthenticatedUserName());
            if (ValidateAndAppendError(egeResult, validationResult))
            {
                //ставим дату проверки и выходим
                _application.LastEgeDocumentsCheckDate = DateTime.Now;
                _dbContext.SaveChanges();
                return validationResult;
            }

            //добавляем документы как полные модели, чтобы не создавать новые
            foreach (var nonAttachedDocument in cachedContext.NotAttachedDocuments)
            {
                var model = (EGEDocumentViewModel)EntrantDocumentExtensions.GetEntrantDocumentViewModel(nonAttachedDocument);
                if (!model.DocumentDate.HasValue)
                    continue;
                model.FillData(_dbContext, cachedContext.AllSubjects, true, null, null);
                ValidateEgeResults(model, 0, null, egeResult, cachedContext.FullModels);
                // непонятно что делать с результатом ValidateEgeResults, потому что дальше могут быть найдены действительные результаты
            }

            //РВИ с приложенными документами
            foreach (var document in cachedContext.DocumentsWithCertificate)
            {
                if (findCertificateOfSubjectId.HasValue && document.SubjectID != findCertificateOfSubjectId)
                    continue;

                var model = (EGEDocumentViewModel)EntrantDocumentExtensions.GetEntrantDocumentViewModel(document.EntrantDocument);
                //нет даты - ошибка
                if (!model.DocumentDate.HasValue)
                {
                    AddSingleError(validationResult, new ApplicationValidationErrorInfo(2, Messages.ApplicationValidator_NoEgeDate), model, document);
                    continue;
                }

                model.FillData(_dbContext, cachedContext.AllSubjects, true, null, null);
                EgeSubjectValidateErrorList checkErr = ValidateEgeResults(model, document.SubjectID ?? 0, document.Subject != null ? document.Subject.Name : null, egeResult, cachedContext.FullModels);

                //не нашли полностью, попробуем поискать по фио и номеру
                if (checkErr.MainError == ApplicationEgeValidator.EgeNotFoundText)
                {
                    List<ApplicationValidationErrorInfo> oneCheckErrorList;
                    EgeCertificate certificate;
                    GetEgeDocumentByFIO(cachedContext.AllSubjects, model.DocumentNumber, userName, out oneCheckErrorList, out certificate);
                    //если нашли сертификат по ФИО, добавляем его в текущий контекст и проверяем заново. Должно всё быть уже хорошо
                    if ((certificate != null) && findCertificatesOfCurrentYearOnly && (certificate.Year != DateTime.Now.Year.ToString()))
                    {
                        certificate = null;
                    }

                    if (certificate != null)
                    {
                        egeResult.Certificates.Add(certificate);
                        checkErr = ValidateEgeResults(model, document.SubjectID ?? 0, document.Subject != null ? document.Subject.Name : null, egeResult, cachedContext.FullModels);
                    }
                }

#warning Ну это полнейшая тупизна!!! Error без сообщения об ошибке - это все ОК по этому объекту
                checkErrorList.Add(checkErr);
                if (checkErr.MainError != null)
                {
                    AddSingleError(validationResult, new ApplicationValidationErrorInfo(2, checkErr.MainError), model, document);
                }
                else
                {
                    AddMultipleErrors(validationResult, checkErr.ErrorsFiltered.Select(x => new ApplicationValidationErrorInfo(2, x.Error)).ToArray(), model, document);
                }
            }
            _dbContext.SaveChanges();

            //РВИ без документов 
            foreach (var testItem in cachedContext.TestItemsWithoutDocument)
            {
                if (findCertificateOfSubjectId.HasValue && testItem.SubjectID != findCertificateOfSubjectId)
                    continue;

                // не указан балл => выбираем лучший результат
                decimal mark;
                int? currentYear = null;
                if (findCertificatesOfCurrentYearOnly)
                {
                    currentYear = DateTime.Now.Year;
                }
                EgeCertificate certificate = egeResult.FindBestMark(testItem.Subject.Name, currentYear, out mark);

                if (certificate == null || mark == 0)
                    continue;

                bool isAnyBenefitDocument = testItem.ApplicationEntranceTestDocument.Any(x => x.BenefitID == 3);

                ApplicationEntranceTestDocument appDoc = new ApplicationEntranceTestDocument
                {
                    ApplicationID = _application.ApplicationID,
                    SubjectID = testItem.SubjectID,
                    SourceID = 1,
                    EntranceTestItemID = testItem.EntranceTestItemID,
                    ResultValue = Convert.ToInt32(mark)
                };

                // проверяем и проставляем нужный балл
                if (ValidateAndPossibleSaveSingleMark(cachedContext, certificate, validationResult, appDoc))
                {
                    if (appDoc.EntrantDocumentID != null)
                    {
                        checkErrorList.Add(new EgeSubjectValidateErrorList(appDoc.EntrantDocumentID.Value, true));
                    }
                    _dbContext.ApplicationEntranceTestDocument.AddObject(appDoc);
                }
            }
            _dbContext.SaveChanges();

            //есть балл, но нет сертификата
            foreach (var document in cachedContext.NoCertificateDocuments)
            {
                if (findCertificateOfSubjectId.HasValue && document.SubjectID != findCertificateOfSubjectId)
                    continue;

                if (document.ResultValue == null)
                {
                    AddSingleError(validationResult, new ApplicationValidationErrorInfo(4, ApplicationEgeValidatorMessages.NoResultValueForSubject(document.Subject.Name)), doc: document);
                    continue;
                }

                if (document.Subject == null)
                    continue;

                int? currentYear = null;
                if (findCertificatesOfCurrentYearOnly)
                {
                    currentYear = DateTime.Now.Year;
                }
                var certificate = egeResult.FindByMark(document.Subject.Name, currentYear, document.ResultValue.Value);

                if (certificate == null)
                {
                    AddSingleError(validationResult, new ApplicationValidationErrorInfo(4, ApplicationEgeValidatorMessages.CertNotFoundForSubject(document.ResultValue.Value, document.Subject.Name)), doc: document);
                }
                else
                {
                    ValidateAndPossibleSaveSingleMark(cachedContext, certificate, validationResult, document);
                    if (document.EntrantDocumentID != null)
                    {
                        checkErrorList.Add(new EgeSubjectValidateErrorList(document.EntrantDocumentID.Value, true));
                    }
                }
            }
            _dbContext.SaveChanges();

            //ставим дату последней проверки
            _application.LastEgeDocumentsCheckDate = DateTime.Now;
            //перевычисляем рейтинг (мы же могли проставить баллы)
            ApplicationRatingCalculator.CalculateApplicationRating(_application.ApplicationID);
            _dbContext.SaveChanges();

            return validationResult;
        }

        /// <summary>
        /// Получение документов ЕГЭ по ФИО абитуриента и номеру сертификата
        /// </summary>
        internal EGEDocumentViewModel GetEgeDocumentByFIO(IEnumerable<Subject> allSubjects, string certificateNumber, string userName, out List<ApplicationValidationErrorInfo> errors, out EgeCertificate cert)
        {
            errors = new List<ApplicationValidationErrorInfo>();
            cert = null;
            //некорректная ситуация
            if (_application.Entrant == null)
            {
                ValidateAndAppendError(EgeResult.CreateError(Messages.ApplicationValidator_InvalidApplicationData), errors);
                return null;
            }

            EgeQuery query = new EgeQuery(_application.Entrant.LastName, _application.Entrant.FirstName, _application.Entrant.MiddleName, certificateNumber, "", "");
            //получаем информацию по ЕГЭ
            EgePacket packet = EgePacketHelper.GetEgePacket(userName ?? UserHelper.GetAuthenticatedUserName(), query);
            EgeResultAndStatus resultAndStatus = EgeInformationProvider.GetEgeInfo(packet);
            //если есть ошибки, создаём их

            if (ValidateAndAppendError(resultAndStatus.Result, errors))
                return null;

            // если не нашлось указанного сертификата
            if (resultAndStatus.Result.Certificates.Count == 0 || resultAndStatus.Result.Certificates.All(x => x.CertificateNumber != certificateNumber))
            {
                ValidateAndAppendError(EgeResult.CreateError(Messages.ApplicationValidator_NoEgeCertificates), errors);
                return null;
            }
            else
            {
                //сертификат от ЕГЭ
                cert = resultAndStatus.Result.Certificates.First(x => x.CertificateNumber == certificateNumber);

                if (allSubjects == null)
                {
                    allSubjects = _dbContext.Subject;
                }
                allSubjects = allSubjects.OrderBy(x => x.Name).ToArray();

                //заполняем модель из полученного ответа с учётом иностранных языков
                var model = new EGEDocumentViewModel();
                model.Subjects = cert.Marks
                    .Where(x => FindSubject(allSubjects, x.SubjectName) != null)
                    .Select(x => new EGEDocumentViewModel.SubjectData
                    {
                        SubjectID = FindSubject(allSubjects, x.SubjectName).SubjectID,
                        SubjectName = FindSubject(allSubjects, x.SubjectName).Name,
                        Value = x.Value
                    }).ToArray();

                model.TypographicNumber = cert.TypographicNumber;
                model.DocumentYear = cert.Year.To(0);
                return model;
            }
        }

        private Subject FindSubject(IEnumerable<Subject> subjects, string name)
        {
            Subject result = subjects.FirstOrDefault(y => String.Equals(y.Name, name, StringComparison.OrdinalIgnoreCase)) ??
                              subjects.FirstOrDefault(y => String.Equals(LanguageSubjects.ForeignLanguagePrefix + y.Name, name, StringComparison.OrdinalIgnoreCase));
            return result;
        }

        /// <summary>
        /// Заполнение модели с ошибочными данными
        /// </summary>
        /// <param name="egeResult"></param>
        /// <param name="validationResult"></param>
        /// <returns>false, если ошибок не было и модель не изменилась</returns>
        private bool ValidateAndAppendError(EgeResult egeResult, List<ApplicationValidationErrorInfo> validationResult)
        {
            if (egeResult.Errors.Count != 0)
            {
                ApplicationValidationErrorInfo errorInfo = GenerateValidationError(egeResult);
                if (validationResult.Count == 0)
                {
                    validationResult.Add(errorInfo);
                }
                else
                {
                    validationResult[0] = errorInfo;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Заполнение модели с ошибочными данными
        /// </summary>
        /// <param name="egeResult"></param>
        /// <param name="validationResult"></param>
        /// <returns>false, если ошибок не было и модель не изменилась</returns>
        private bool ValidateAndAppendError(EgeResult egeResult, Dictionary<int, ApplicationValidationErrorInfo> validationResult)
        {
            if (egeResult.Errors.Count != 0)
            {
                ApplicationValidationErrorInfo errorInfo = GenerateValidationError(egeResult);
                if (!validationResult.ContainsKey(0))
                {
                    validationResult.Add(0, errorInfo);
                }
                else
                {
                    validationResult[0] = errorInfo;
                }
                return true;
            }
            return false;
        }

        private ApplicationValidationErrorInfo GenerateValidationError(EgeResult egeResult)
        {
            ApplicationValidationErrorInfo result = new ApplicationValidationErrorInfo(2, "");
            result.Message = ApplicationEgeValidatorMessages.EgeErrorPrefix() + string.Join(",", egeResult.Errors.ToArray());
            if (egeResult.Certificates != null && egeResult.Certificates.Any(x => x.Status == EgeResult.CancelledCertificate))
            {
                result.Code = 5;
            }
            return result;
        }

        /// <summary>
        /// Проверяет найденный сертификат на правильность. Если нужно - сохраняет документ.</summary>
        /// <returns>Возвращает true, если нужно далее сохранить документ.</returns>
        private bool ValidateAndPossibleSaveSingleMark(ApplicationEgeContext context, EgeCertificate certificate,
                                                              Dictionary<int, ApplicationValidationErrorInfo> validationResult,
                                                              ApplicationEntranceTestDocument document)
        {
            //для неактуального сертификата ставим ошибку
            if (certificate.Status != EgeResult.ActualCertificate)
            {
                if (String.IsNullOrEmpty(certificate.CertificateNewNumber))
                {
                    string subjectName = context.AllSubjects.First(x => x.SubjectID == document.SubjectID).Name;

                    AddSingleError(validationResult, new ApplicationValidationErrorInfo(certificate.Status == EgeResult.CancelledCertificate ? 3 : 4
                , ApplicationEgeValidatorMessages.StatusForSubject((document.ResultValue ?? 0), subjectName, certificate.Status))
                        , doc: document);

                    //если не аннулирован, то пробуем ещё и поставить документ
                    if (certificate.Status != EgeResult.CancelledCertificate)
                    {
                        if (AssignAndPossibleCreateEntrantDocument(context, validationResult, document, certificate))
                            return true;
                    }
                }
            }
            else
            {
                //ставим документ, если неуспешно - добавляем ошибку
                if (AssignAndPossibleCreateEntrantDocument(context, validationResult, document, certificate))
                {
                    AddSingleError(validationResult, null, doc: document);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Присвоение документа РВИ (возможно и создание документа)
        /// </summary>
        private bool AssignAndPossibleCreateEntrantDocument(ApplicationEgeContext context,
                                                                   Dictionary<int, ApplicationValidationErrorInfo> resDoc,
                                                                   ApplicationEntranceTestDocument document,
                                                                   EgeCertificate certificate)
        {
            var model = FindFullModel(context.FullModels, certificate);
            bool isError = false;
            if (model == null)
            {
                // если не нашли подходящую "полную" по баллам модель - создаем новую
                model = CreateFullModel(context, certificate);
                var resultModel = _dbContext.SaveEntrantDocument(model, context.Application.Entrant.SNILS);
                if (resultModel.IsError)
                {
                    AddSingleError(resDoc, new ApplicationValidationErrorInfo(2, resultModel.Message), model, document);
                    isError = true;
                }

                context.FullModels.Add(model);
            }

            document.EntrantDocumentID = model.EntrantDocumentID;
            return !isError;
        }

        /// <summary>
        /// Добавление ошибки проверки
        /// </summary>
        private void AddMultipleErrors(Dictionary<int, ApplicationValidationErrorInfo> to, ApplicationValidationErrorInfo[] errors,
                                     EGEDocumentViewModel viewModel = null, ApplicationEntranceTestDocument doc = null)
        {
            int key = doc == null ? 0 : doc.EntranceTestItemID ?? 0;

            if (errors.IsNullOrEmpty())
            {
                if (!to.ContainsKey(key))
                {
                    to.Add(key, new ApplicationValidationErrorInfo(0, ""));
                }
                return;
            }

            var errorCode = errors.Select(x => x.Code).FirstOrDefault();
            to[key] = new ApplicationValidationErrorInfo(errorCode > 0 ? errorCode : 2,
                (to.ContainsKey(key) ? to[key] + " " : "") + ApplicationEgeValidatorMessages.EgeErrorPrefix(viewModel) + string.Join(",", errors.Select(x => x.Message)));
        }

        /// <summary>
        /// Добавление ошибки проверки
        /// </summary>
        private void AddSingleError(Dictionary<int, ApplicationValidationErrorInfo> to, ApplicationValidationErrorInfo error,
                                     EGEDocumentViewModel viewModel = null, ApplicationEntranceTestDocument doc = null)
        {
            AddMultipleErrors(to, error == null ? null : new[] { error }, viewModel, doc);
        }

        /// <summary>
        /// Создание "полной" модели сертификата ЕГЭ
        /// </summary>
        private EGEDocumentViewModel CreateFullModel(ApplicationEgeContext context, EgeCertificate certificate)
        {
            int year;
            if (string.IsNullOrEmpty(certificate.Year) || !int.TryParse(certificate.Year, NumberStyles.Integer, null, out year))
                throw new InvalidOperationException("Некорректный сертификат");
            EGEDocumentViewModel model = new EGEDocumentViewModel
            {
                EntrantID = context.Application.EntrantID,
                DocumentOrganization = "",
                DocumentTypeID = ApplicationEgeValidator.EgeDocumentTypeId,
                DocumentDate = new DateTime(year, 1, 1),
                DocumentNumber = certificate.CertificateNumber
            };

            CopyMarksToViewModel(certificate, model);

            return model;
        }

        /// <summary>
        /// Поиск полной модели сертификатов по параметрам
        /// </summary>
        private EGEDocumentViewModel FindFullModel(IEnumerable<EGEDocumentViewModel> fullModels,
                                                          EgeCertificate certificate)
        {
            return fullModels.FirstOrDefault(x => x.DocumentNumber == certificate.CertificateNumber
                                                  && x.DocumentDate.HasValue &&
                                                  x.DocumentDate.Value.Year.ToString() == certificate.Year);
        }

        /// <summary>
        /// 	Возвращает пустой список, если результат ЕГЭ из нашей базы соответствует возвращенному из ФБС или строки с ошибками.
        /// </summary>
        private EgeSubjectValidateErrorList ValidateEgeResults(EGEDocumentViewModel viewModel, int checkSubjectId,
            string subjectName,
             EgeResult actualEgeResult, List<EGEDocumentViewModel> fullModels)
        {
            var errList = new EgeSubjectValidateErrorList(viewModel.EntrantDocumentID, false);

            if (!viewModel.DocumentDate.HasValue)
            {
                errList.MainError = Messages.ApplicationValidator_NoEgeDate;
                return errList;
            }

            var certificate = GeValidEgeCertificateHeader(viewModel.DocumentDate.Value.Year);

            //добавляем предметы в сертификат
            foreach (var subject in viewModel.Subjects)
            {
                certificate.Marks.Add(new Mark
                {
                    SubjectName = subject.SubjectName,
                    SubjectMark = subject.Value.ToString(ApplicationEgeValidator.MarkFormatter)
                });
            }

            //находим действительный сертификатEgeResult.CancelledCertificate
            EgeCertificate actual = actualEgeResult.Certificates.FirstOrDefault(x =>
                x.Status == EgeResult.ActualCertificate &&
                x.CertificateNumber == viewModel.DocumentNumber &&
                (string.IsNullOrEmpty(viewModel.TypographicNumber) || viewModel.TypographicNumber == x.TypographicNumber));

            if (actual == null)
            {
                errList.MainError = ApplicationEgeValidator.EgeNotFoundText;
                return errList;
            }

            // теперь нужно проверить, не ануллировано ли это св-во (тогда ошибка)
            if (actual.СertificateDeny.To(false) && !string.IsNullOrEmpty(actual.CertificateNewNumber))
            {
                errList.MainError = ApplicationEgeValidator.EgeCancelledText.FormatWith(actual.CertificateNewNumber);
                if (!string.IsNullOrEmpty(actual.CertificateDenyComment))
                    errList.MainError += " Комментарий:" + actual.CertificateDenyComment;
                return errList;
            }

            bool isStatusError;
            errList.Errors = certificate.Validate(actual, out isStatusError);
            errList.ErrorsFiltered = errList.Errors;

            // если баллы совпали полностью - можно использовать как "полную" модель
            if (!errList.Errors.Any() || (isStatusError && errList.Errors.Count() == 1))
            {
                // если есть и просто иностранный язык и конкретный, то нужно вычесть 1 из количества слева
                int foreignLanguageShift =
                    certificate.Marks.Any(x => x.SubjectName == LanguageSubjects.ForeignLanguage) &&
                    certificate.Marks.Any(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix)) ? 1 : 0;
                if (certificate.Marks.Count - foreignLanguageShift == actual.Marks.Count
                    || certificate.Marks.Count - foreignLanguageShift == actual.Marks.Count(x => x.Value > 0))
                {
                    fullModels.Add(viewModel);
                }
                return errList;
            }

            if (certificate.Marks.Count > 1 && checkSubjectId > 0)
            {
                // урезаем сертификат до проверяемого предмета и проводим проверку заново (чтобы не дублировать ошибки)
                certificate.Marks.Clear();
                var documentSubject = viewModel.Subjects.Where(x => x.SubjectID == checkSubjectId).ToArray();
                if (documentSubject.Length == 0)
                {
                    if (subjectName == LanguageSubjects.ForeignLanguage)
                        documentSubject = viewModel.Subjects.Where(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix)).ToArray();
                }
                //не нашли предмета. Некорректная ситуация, непонятно что валидировать и что делать
                if (documentSubject.Length == 0)
                {
                    errList.MainError = "Не найден документ для результата по предмету SubjectID = " + checkSubjectId;
                    return errList;
                }

                certificate.Marks.AddRange(documentSubject.Select(x => new Mark
                {
                    SubjectName = x.SubjectName,
                    SubjectMark = x.Value.ToString(ApplicationEgeValidator.MarkFormatter)
                }));
                errList.ErrorsFiltered = certificate.Validate(actual, out isStatusError);
            }

            return errList;
        }

        /// <summary>
        /// Корректный заголовок сертификата
        /// </summary>
        private EgeCertificate GeValidEgeCertificateHeader(int year)
        {
            return new EgeCertificate { Status = "Действующий", Year = year.ToString() };
        }

        /// <summary>
        /// Копировение оценок из сертификата в модель
        /// </summary>
        private void CopyMarksToViewModel(EgeCertificate certificate, EGEDocumentViewModel model)
        {
            List<Mark> marks = certificate.Marks.Where(x => x.Value > 0).ToList();
            List<EGEDocumentViewModel.SubjectData> subjects = new List<EGEDocumentViewModel.SubjectData>(marks.Count);

            EGEDocumentViewModel.SubjectData subjectData;
            for (int index = 0; index < marks.Count; index++)
            {
                var mark = marks[index];

                subjectData = new EGEDocumentViewModel.SubjectData();
                string subjectName;
                if (!LanguageSubjects.EntranceTestBySubject.TryGetValue(mark.SubjectName, out subjectName))
                    subjectName = mark.SubjectName;

                subjectData.SubjectName = subjectName;
                subjectData.Value = mark.Value;

                subjects.Add(subjectData);
            }

            // если есть конкретные иностранные, добавляем ещё и общий
            if (subjects.Any(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix)))
            {
                subjectData = new EGEDocumentViewModel.SubjectData
                {
                    SubjectName = LanguageSubjects.ForeignLanguage,
                    Value = subjects.Where(x => x.SubjectName.StartsWith(LanguageSubjects.ForeignLanguagePrefix))
                        .OrderByDescending(x => x.Value).Take(1).First().Value
                };
                subjects.Add(subjectData);
            }

            model.Subjects = subjects.ToArray();
        }

        /// <summary>
        /// Получение информации о ЕГЭ из ФБС
        /// </summary>
        private EgeResult GetEgeInformation(Application application, string userName)
        {
            if (application.Entrant == null)
                return EgeResult.CreateError(Messages.ApplicationValidator_InvalidApplicationData);
            EntrantDocument identity = application.Entrant.EntrantDocument_Identity;

            if (identity == null)
                return EgeResult.CreateError(Messages.ApplicationValidator_InvalidApplicationData);

            EgeQuery query = new EgeQuery(application.Entrant.LastName, application.Entrant.FirstName, application.Entrant.MiddleName, "",
                                          identity.DocumentSeries, identity.DocumentNumber);

            EgePacket packet = EgePacketHelper.GetEgePacket(userName, query);
            EgeResultAndStatus resultAndStatus = EgeInformationProvider.GetEgeInfo(packet);
            return resultAndStatus.Result;
        }
    }
}
