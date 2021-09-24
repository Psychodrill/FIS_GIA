using System;
using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing.Applications
{
    //Импорт одного заявления
    public class ApplicationImporter : ObjectImporter
    {
        private readonly Application _applicationDb;
        private readonly ApplicationDto _applicationDto;

        private readonly Dictionary<int, string> _entrTestWithAppEntrTestIds = new Dictionary<int, string>();
        private readonly List<EntranceTestAppItemDto> _entranceTestItemIds = new List<EntranceTestAppItemDto>();

        // РВИ для заявления (ВИ по ЕГЭ, обычное или призер олимпиады)
        private ApplicationEntranceTestDocument[] _entranceTestResultsDb;

        public ApplicationImporter(StorageManager storageManager, ApplicationDto application)
            : base(storageManager)
        {
            _applicationDto = application;
            _applicationDb = DbObjectRepository.FindApplicationByUID(_applicationDto.UID, true);
            //_applicationDb = DbObjectRepository.GetObject<Application>(_applicationDto.UID);
            ProcessedDtoStorage.AddApplication(_applicationDto);
        }

        protected override void FindExcludedObjectsInDbForDelete()
        {
            // дочерние объекты для заявления
            // проверки для РВИ и льгот делаются в IsChangedCommonBenefitOrEntranceTests
        }

        protected override void FindInsertAndUpdate()
        {
            _entranceTestResultsDb = ObjectLinkManager.ApplicationLinkWithEntranceTestResults(_applicationDb);

            // например, помещено в конфликт при импорте структуры приема
            //if(ConflictStorage.CheckApplicationInConflicts(_applicationDto)) return;
            if (ConflictStorage.HasConflictOrNotImported(_applicationDto)) return;

            //если нет в базе, проверяем, можем ли вставить
            if (_applicationDb == null)
            {
                if (CanInsert())
                    InsertStorage.AddApplication(_applicationDto);
            }
            else
            {
                if (CanUpdate())
                {
                    // обновление равносильно удалению со вставкой
                    // информация о заявлении импортируется целиком.					
                    DeleteStorage.AddApplication(_applicationDb);
                    // если заявление включено в приказ, то сохраняем эту информацию
                    _applicationDto.OrderOfAdmissionID = _applicationDb.OrderOfAdmissionID;
                    _applicationDto.PreviousStatusID = _applicationDb.StatusID;
                    InsertStorage.AddApplication(_applicationDto);
                }
                // если не добавили заявление, то имеются конфликты
            }
        }

        protected override bool CanUpdate()
        {
            if (_applicationDb.OrderOfAdmissionID.HasValue)
            {
                ConflictStorage.AddNotImportedDto(_applicationDto, ConflictMessages.OrderExists);
                return false;
            }

            bool isChangedCommonBenefitOrEntranceTests = IsChangedCommonBenefitOrEntranceTests(false);

            //по идее теперь запрещено импортировать заявления в приказе, и проверка не должна сработать
            //но на всякий случай оставлено, вдруг логика изменится
            bool isOrderConflictExists =
                IsExistsOrderOfAdmissionAndEntranceResultsChanged(isChangedCommonBenefitOrEntranceTests);
            //#38402 теперь разрешено менять
            bool checkCanUpdate = /*CompetitiveGroupChangedCheckForUpdate() && */ !isOrderConflictExists /* && 
				!isChangedCommonBenefitOrEntranceTests*/;

            return checkCanUpdate;
        }

        //#28443 тут проверяем на валидность ВИ
        protected bool CanInsert()
        {
            return true;
        }

        protected override void ProcessChildren(bool isParentConflict)
        {
            //заполняем детей и проставляем им нужные уиды
            foreach (ApplicationCommonBenefitDto applicationCommonBenefitDto in _applicationDto.GetCommonBenefits())
            {
                applicationCommonBenefitDto.ParentUID = _applicationDto.UID;
            }

            if (_applicationDto.EntranceTestResults != null)
                foreach (EntranceTestAppItemDto entranceTestAppItemDto in _applicationDto.EntranceTestResults)
                    entranceTestAppItemDto.ParentUID = _applicationDto.UID;
            if (_applicationDto.Entrant != null)
                _applicationDto.Entrant.ParentUID = _applicationDto.UID;

            if (isParentConflict)
            {
                foreach (ApplicationCommonBenefitDto applicationCommonBenefitDto in _applicationDto.GetCommonBenefits())
                    ConflictStorage.AddNotImportedDto(applicationCommonBenefitDto,
                                                      ConflictMessages.ParentObjectIsNotImported);

                ConflictStorage.AddNotImportedDto(_applicationDto.Entrant, ConflictMessages.ParentObjectIsNotImported);
                if (_applicationDto.EntranceTestResults != null)
                    foreach (EntranceTestAppItemDto entranceTestAppItemDto in _applicationDto.EntranceTestResults)
                        ConflictStorage.AddNotImportedDto(entranceTestAppItemDto,
                                                          ConflictMessages.ParentObjectIsNotImported);
            }
        }

        protected override BaseDto GetDtoObject()
        {
            return _applicationDto;
        }

        /// <summary>
        ///     Изменились ли льготы или РВИ относительно существующего
        /// </summary>
        private bool IsChangedCommonBenefitOrEntranceTests(bool addToConflicts)
        {
            bool result = false;

            // смотрим изменения в общих льготах
            foreach (ApplicationCommonBenefitDto applicationCommonBenefitDto in _applicationDto.GetCommonBenefits())
            {
                ApplicationEntranceTestDocument applicationEntranceTestDocument;
                int changeResult = this.IsChanged(applicationCommonBenefitDto, _applicationDb, out applicationEntranceTestDocument);
                if (changeResult > 0)
                {
                    /*ConflictStorage.AddEntranceTestResults(_applicationDto, 
						applicationEntranceTestDocument == null ? null : (int?) applicationEntranceTestDocument.ID, changeResult);*/
                    result = true;
                }

                if (applicationCommonBenefitDto.DocumentReason != null &&
                    applicationCommonBenefitDto.DocumentReason.OlympicDocument != null)
                    /* если передается льгота уровня ниже, чем разрешена в КГ */
                    CheckBenefitIsValid(DbObjectRepository.CompetitiveGroupCommonBenefitItems,
                                        applicationCommonBenefitDto.DocumentReason.OlympicDocument,
                                        applicationCommonBenefitDto.CompetitiveGroupID,
                                        applicationCommonBenefitDto);
            }

            // ищем отредактированные РВИ. Связанные данные: приказ			
            if (_applicationDto.EntranceTestResults == null)
            {
                return _entranceTestResultsDb != null && _entranceTestResultsDb.Length != 0;
            }

            foreach (EntranceTestAppItemDto etResultDto in _applicationDto.EntranceTestResults)
            {
                /* если передается льгота уровня ниже, чем разрешена в КГ */
                if (etResultDto.ResultDocument != null &&
                    etResultDto.ResultDocument.OlympicDocument != null)
                {
                    if (!CheckBenefitIsValid(DbObjectRepository.CompetitiveGroupBenefitItemsForEntranceTest,
                                        etResultDto.ResultDocument.OlympicDocument,
                                        etResultDto.CompetitiveGroupID,
                                        etResultDto))
                        continue;
                }


                //если создалось поддельное РВИ, добавляем его
                if (CreateEntranceTestDbObjectFromDto(etResultDto, null) != null)
                    _entranceTestItemIds.Add(etResultDto);
            }

            if (_entranceTestItemIds.Count != _entranceTestResultsDb.Length) result = true;

            // сравниваем РВИ на обновление
            foreach (EntranceTestAppItemDto entranceTestResultDto in _entranceTestItemIds)
            {
                ApplicationEntranceTestDocument appEntranceTestDb =
                    _entranceTestResultsDb.SingleOrDefault(x => x.UID == entranceTestResultDto.UID);

                // добавляется новое РВИ, не равны
                if (appEntranceTestDb == null)
                {
                    result = true;
                    continue;
                }

                int changeResult = this.IsChanged(entranceTestResultDto, appEntranceTestDb,
                                                  _applicationDto.ApplicationDocuments);
                if (changeResult > 0)
                {
                    if (addToConflicts)
                        ConflictStorage.AddEntranceTestResults(_applicationDto, appEntranceTestDb.ID, changeResult);
                    result = true;
                }
            }

            // добавляем в конфликты РВИ, которые отсутствуют в импорте для заявления
            foreach (ApplicationEntranceTestDocument entranceTestDocument in _entranceTestResultsDb)
            {
                string uid = entranceTestDocument.UID;
                if (_applicationDto.EntranceTestResults.Any(x => x.UID == uid))
                    continue;
                if (addToConflicts)
                    ConflictStorage.AddEntranceTestResults(_applicationDto, entranceTestDocument.ID,
                        ConflictMessages.EntranceTestResultExistsInDbAndNotSpecifiedInImport);
            }

            return result;
        }

        private bool CheckBenefitIsValid(IEnumerable<BenefitItemC> benefitItems,
            OlympicDocumentDto olympicDocument, string competitiveGroupUID, BaseDto benefitDto)
        {
            var olympic = DbObjectRepository.GetOlympicType(olympicDocument.OlympicID.To(0));
            var cgroup = DbObjectRepository.CompetitiveGroups.FirstOrDefault(c => c.UID == competitiveGroupUID);
            if (cgroup != null)
            {
                var benefitsWitOlympic = benefitItems.Where(c => c.CompetitiveGroupID == cgroup.CompetitiveGroupID &&
                    (c.BenefitItemCOlympicType.Select(x => x.OlympicTypeID).Contains(olympic.OlympicID) || c.IsForAllOlympic)).ToList();

                /* Проверка того, что олимпиада не разрешена в КГ */
                if (benefitsWitOlympic.Count == 0)
                {
                    ConflictStorage.AddNotImportedDto(benefitDto,
                        ConflictMessages.OlympicTypeIsNotAllowedInCompetitiveGroup, olympic.OlympicID.ToString(), cgroup.UID);
                    return false;
                }

                /* общие льготы */
                bool levelsExistsValidValues = false;
                bool winnersExistsValidValues = false;

                /* Если не найдено никаких уровней - берем минимальный уровень олимпиады и смотрим разрешени ли он в КГ */
                if (olympic.OlympicLevelID == null && string.IsNullOrEmpty(olympicDocument.LevelID))
                {
                    var subjects = DbObjectRepository.OlympicTypeSubjectLinks.Where(c => c.OlympicID == olympic.OlympicID && c.SubjectLevelID.HasValue).ToList();
                    var maxOlympicLevel = subjects.Max(c => c.SubjectLevelID.Value);
                    /* Если нет льготы которая содержит выбранную олимпиаду с разрешенными уровнями */
                    if (benefitsWitOlympic.All(c => !IsOlympicLevelFlagsHasValidValues(maxOlympicLevel, c.OlympicLevelFlags)))
                    {
                        ConflictStorage.AddNotImportedDto(benefitDto,
                            ConflictMessages.OlympicLevelIsNotAllowedInCompetitiveGroup, 
                            _applicationDto.UID,
                            benefitDto.UID,
                            competitiveGroupUID,
                            olympic.OlympicID.ToString(),
                            string.Join(", ", 
                                benefitsWitOlympic.Where(c => c.OlympicLevelFlags != 255)
                                .Aggregate(new HashSet<string>(), (t, u) =>
                                    {
                                        t.UnionWith(u.OlympicLevelFlags.GetLevelNames());
                                        return t;
                                    }).Distinct().OrderBy(c => c).ToArray()));
                        return false;
                    }

                    levelsExistsValidValues = true;
                }

                foreach (var benefit in benefitsWitOlympic)
                {
                    var allFlagsChecked = benefit.OlympicLevelFlags == 255 || (benefit.OlympicLevelFlags.IsFlagSet(1) &&
                                                                               benefit.OlympicLevelFlags.IsFlagSet(3) &&
                                                                               benefit.OlympicLevelFlags.IsFlagSet(4));
                    if (allFlagsChecked)
                        levelsExistsValidValues = true;
                    else
                    {
                        if (!levelsExistsValidValues && olympic.OlympicLevelID != null)
                        {
                            if (IsOlympicLevelFlagsHasValidValues(olympic.OlympicLevelID.Value, benefit.OlympicLevelFlags))
                                levelsExistsValidValues = true;
                        }
                        else if (!levelsExistsValidValues && olympicDocument.LevelID != null)
                        {
                            if (IsOlympicLevelFlagsHasValidValues(olympicDocument.LevelID.To<short>(0), benefit.OlympicLevelFlags))
                                levelsExistsValidValues = true;
                        }
                    }

                    /* В случае предоставления льготы призерам олимпиады должна быть предоставлена 
                        * льгота того же или более высокого порядка также и победителям олимпиады */
                    /* разрешен только победитель - мы шлем призера */
                    if (!winnersExistsValidValues &&
                        !(benefit.OlympicDiplomTypeID.IsFlagSet(1) &&
                        !benefit.OlympicDiplomTypeID.IsFlagSet(2) && olympicDocument.DiplomaTypeID.To(0) == 2))
                        winnersExistsValidValues = true;
                }

                if (!levelsExistsValidValues)
                {
                    ConflictStorage.AddNotImportedDto(benefitDto,
                        ConflictMessages.BenefitItemCNotAllowedInCompetitiveGroup, _applicationDto.UID, benefitDto.UID, cgroup.UID);
                    return false;
                }

                if (!winnersExistsValidValues)
                {
                    ConflictStorage.AddNotImportedDto(benefitDto, ConflictMessages.BenefitItemCAllowedToWinnerOnly,
                        _applicationDto.UID, benefitDto.UID, cgroup.UID);
                    return false;
                }
            }

            return true;
        }

        private bool IsOlympicLevelFlagsHasValidValues(short level, int olympicLevelFlags)
        {
            if (olympicLevelFlags == 255)
                return true;

            /* Если грузим I уровня - должен быть проставлен I */
            if (level == 2 && (olympicLevelFlags.IsFlagSet(1) || olympicLevelFlags.IsFlagSet(3) || olympicLevelFlags.IsFlagSet(4)))
                return true;

            /* Если грузим II уровня - должен быть проставлен II или III */
            if (level == 3 && (olympicLevelFlags.IsFlagSet(3) || olympicLevelFlags.IsFlagSet(4)))
                return true;

            /* Если грузим III уровня - должен быть проставлен I или II или III */
            if (level == 4 && olympicLevelFlags.IsFlagSet(4))
                return true;

            return false;
        }

        /// <summary>
        ///     Если заявление включено в приказ и импортируемые ВИ отличаются, то добавляем в конфликты приказ.
        ///     Нельзя применить изменения, если есть приказ.
        /// </summary>
        private bool IsExistsOrderOfAdmissionAndEntranceResultsChanged(bool isChangedCommonBenefitOrEntranceTests)
        {
            // если заявление включено в приказ, то делаем дополнительные проверки
            if (_applicationDb.OrderOfAdmissionID.HasValue && isChangedCommonBenefitOrEntranceTests)
            {
                ConflictStorage.AddOrdersOfAdmission(_applicationDto, _applicationDto.GetApplicationShortRef(),
                                                     ConflictMessages.EntranceTestResultsChangedForApplicationInOrder);
                return true;
            }
            return false;
        }

        protected override void CheckIntegrity()
        {
            ObjectIntegrityManager.CheckIntegrity(_applicationDto);
            
            /* Проверка справочных значений для заяалений */
            foreach (ApplicationCommonBenefitDto applicationCommonBenefitDto in _applicationDto.GetCommonBenefits())
            {
                if (applicationCommonBenefitDto.DocumentReason != null &&
                    applicationCommonBenefitDto.DocumentReason.OlympicDocument != null)
                {
                    ObjectIntegrityManager.CheckDictionaryValues(applicationCommonBenefitDto.DocumentReason.OlympicDocument.OlympicID, 
                        "OlympicID", _applicationDto, DbObjectRepository.GetOlympicType);
                }
            }

            if (_applicationDto.EntranceTestResults != null)
            foreach (EntranceTestAppItemDto etResultDto in _applicationDto.EntranceTestResults)
            {
                if (etResultDto.ResultDocument != null &&
                    etResultDto.ResultDocument.OlympicDocument != null)
                {
                    ObjectIntegrityManager.CheckDictionaryValues(etResultDto.ResultDocument.OlympicDocument.OlympicID,
                        "OlympicID", _applicationDto, DbObjectRepository.GetOlympicType);
                }
            }
        }

        private ApplicationEntranceTestDocument CreateEntranceTestDbObjectFromDto(EntranceTestAppItemDto etDto, Application app)
        {
            //все проверки уже должны быть в ObjectIntegrityManager.CheckApplicationEntranceTestResultUniqueness
            //поэтому в конфликты их не пишем
            if (_applicationDto.EntranceTestResults == null) return null;

            var doc = new ApplicationEntranceTestDocument();

            doc.UID = etDto.UID;
            doc.SubjectID = etDto.EntranceTestSubject.SubjectID.To((int?)null);
            doc.SourceID = etDto.ResultSourceTypeID.To((int?)null);
            int entranceTestTypeID = etDto.EntranceTestTypeID.To(0);
            EntranceTestItemC entranceTestItemC =
                DbObjectRepository.CompetitiveGroupEntranceTestItems.SingleOrDefault(x =>
                    x.CompetitiveGroup.UID == etDto.CompetitiveGroupID &&
                    doc.SourceID.HasValue && x.EntranceTestTypeID == entranceTestTypeID &&
                    ((!doc.SubjectID.HasValue && (x.SubjectName ?? "").ToLower() == etDto.EntranceTestSubject.SubjectName.ToLower()) ||
                    (doc.SubjectID.HasValue && x.SubjectID == doc.SubjectID)));

            if (entranceTestItemC == null)
                return null;

            if (_entrTestWithAppEntrTestIds.ContainsKey(entranceTestItemC.EntranceTestItemID))
                return null;

            if (!doc.SourceID.HasValue)
                return null;

            var sourceID = (EntranceTestResultSourceEnum)doc.SourceID;
            switch (sourceID)
            {
                case EntranceTestResultSourceEnum.EgeDocument:
                    if (etDto.ResultDocument == null || String.IsNullOrWhiteSpace(etDto.ResultDocument.EgeDocumentID))
                    {
                        bool isInNotOrderDb = _applicationDb == null || _applicationDb.OrderOfAdmissionID == null;
                        if (isInNotOrderDb) //если не включено в приказ, тогда разрешено не иметь источника. #22904
                            break;

                        return null;
                    }

                    string egeDocumentID = etDto.ResultDocument.EgeDocumentID;

                    if (app != null)
                    {
                        EntrantDocument egeDocument = DbObjectRepository.EntrantDocuments.SingleOrDefault(x =>
                                                                                                          x.UID ==
                                                                                                          egeDocumentID &&
                                                                                                          x.EntrantID ==
                                                                                                          app.EntrantID);
                        if (egeDocument == null)
                        {
                            // данный случай исключает проверка на целостность в ObjectIntegrityManager
                            LogHelper.Log.ErrorFormat("Отсутствует ЕГЭ Свидетельство с UID: {0} для РВИ с UID: {1}",
                                                      egeDocumentID, etDto.UID);
                            return null;
                        }
                    }

                    break;

                case EntranceTestResultSourceEnum.GiaDocument:
                    if (etDto.ResultDocument == null || String.IsNullOrWhiteSpace(etDto.ResultDocument.EgeDocumentID))
                    {
                        bool isInNotOrderDb = _applicationDb != null && _applicationDb.OrderOfAdmissionID == null;
                        if (isInNotOrderDb) //если не включено в приказ, тогда разрешено не иметь источника. #22904
                            break;

                        return null;
                    }

                    break;

                case EntranceTestResultSourceEnum.OlympicDocument:
                    if (etDto.ResultDocument == null ||
                        (etDto.ResultDocument.OlympicDocument == null &&
                         etDto.ResultDocument.OlympicTotalDocument == null))
                        return null;

                    // для заполнения объекта в БД
                    if (app != null)
                    {
                        EntrantDocument olympicDocument =
                            new DbDocumentInsertManager(ConflictStorage, app).CreateOlympicDocument(
                                etDto.ResultDocument, null);
                        if (olympicDocument == null)
                            return null;
                    }

                    break;
            }

            _entrTestWithAppEntrTestIds.Add(entranceTestItemC.EntranceTestItemID, etDto.UID);

            return doc;
        }

        /// <summary>
        /// Метод проверяющий необходимость автоматического проставления дат
        /// </summary>
        /// <param name="eduDocument">Документ об образовании</param>
        /// <param name="receivedDate">Дата предоставления документа</param>
        public static string CheckDate(ApplicationDocumentDto eduDocument, DateTime? receivedDate)
        {
            if (eduDocument != null)
            {
                //смотрим есть ли оригиналы
                bool isReceived = !(eduDocument.OriginalReceived == null ||
                                    eduDocument.OriginalReceived.Trim() == "0" ||
                                    eduDocument.OriginalReceived.Trim() == "" ||
                                    eduDocument.OriginalReceived.Trim() == "false");

                if (isReceived)
                {   
                    //если да, но дата ещё не проставлена, берём сегодняшнюю
                    if (eduDocument.OriginalReceivedDate == null &&
                    !receivedDate.HasValue)
                        return DateTime.Now.ToString();

                    //если дата проставлена, то её и оставляем
                    if (receivedDate.HasValue)
                        return receivedDate.ToString();
                }
                else
                {
                    //если нет, ставим null
                    return "";
                }
            }
            //если документа нет или дата уже проставлена, а новая не передана, не меняем
            return null;
        }
    }



    public static partial class Extensions
    {
        public static string GetLevelName(this short? level)
        {
            if (!level.HasValue) return "";

            if (level == 1) return "Все";
            if (level == 2) return "I";
            if (level == 3) return "II";
            if (level == 4) return "III";
         
            return "";
        }

        public static List<string> GetLevelNames(this short flag)
        {
            var results = new List<string>();
            if (flag == 255)
                results.Add("Все");
            else 
            {
                if (flag.IsFlagSet(1))
                    results.Add("I");
                if (flag.IsFlagSet(3))
                    results.Add("II");
                if (flag.IsFlagSet(4))
                    results.Add("III");
            }

            return results;
        }
    }
}