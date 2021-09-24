using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GVUZ.ServiceModel.Import.Core.Operations.Conflicts
{
    public static class ConflictMessages
    {
        #region Проверки целостности

        /// <summary>
        /// В конкурсe указан уровень образования, недопустимый в данной приемной кампании.
        /// </summary>
        public const int CompetitiveGroupContainsNotAvailableEducationLevel = 1;

        /// <summary>
        /// В конкурсe указана форму обучения, недопустимая в данной приемной кампании.
        /// </summary>
        public const int CompetitiveGroupContainsNotAvailableEducationForm = 2;

        /// <summary>
        /// В пакете имеется конкурс с такими же параметрами и такой же программой обучения.
        /// </summary>
        public const int CompetitiveGroupWithProgramIsNotUniqueInPackage = 3;

        /// <summary>
        /// В системе уже имеется конкурс с такими же параметрами и такой же программой обучения.
        /// </summary>
        public const int CompetitiveGroupWithProgramIsNotUniqueInDb = 4;

        ///// <summary>
        ///// В пакете имеется конкурс с такими же параметрами.
        ///// </summary>
        //public const int CompetitiveGroupIsNotUniqueInPackage = 5;

        ///// <summary>
        ///// В системе уже имеется конкурс с такими же параметрами.
        ///// </summary>
        //public const int CompetitiveGroupIsNotUniqueInDb = 6;

        //      ///// <summary>
        //      ///// Родительский объект не проимпортирован.
        //      ///// </summary>
        //      ////public const int ParentObjectIsNotImported = 4;

        //      ///// <summary>
        //      ///// В конкурсe имеются направления, у которых не совпадает перечень вступительных испытаний или отсутствуют разрешённые вступительные испытания.
        //      ///// </summary>
        //      //public const int CompetitiveGroupContainsNotAvailableDirections = 5;

        /// <summary>
        /// В конкурсe недопустимы испытания профильной направленности.
        /// </summary>
        public const int CompetitiveGroupContainsNotAllowedExtraProfileSubjectInEntranceTest = 7;

        /// <summary>
        /// Превышена длина {1} символов для поля {0}
        /// </summary>
        public const int FieldLengthExceeded = 8;

        /// <summary>
        /// В конкуре недопустимы испытания творческой направленности.
        /// </summary>
        public const int CompetitiveGroupNotAllowedCreativeEntranceTests = 9;

        /// <summary>
        /// В Конкурсной группе указано направление, не входящее в список допустимых.
        /// </summary>
        //public const int CompetitiveGroupContainsDirectionNotDefinedInAllowed = 9;

        ///// <summary>
        ///// В конкурсе указаны предметы для основных вступительных испытаний, которые не допустимы для направлений.
        ///// </summary>
        //public const int CompetitiveGroupContainsNotAllowedEntranceTestSubjects = 10;

        /// <summary>
        /// В конкурсе нельзя одновременно задавать тэги CompetitiveGroupItem и TargetOrganizations
        /// </summary>
        public const int CompetitiveGroupCannotHaveCGIAndTarget = 11;

        ///// <summary>
        ///// В конкурсной группе не указаны допустимые предметы.
        ///// </summary>
        ///public const int CompetitiveGroupHasNotAllowedEntranceTestSubjects = 11;

        ///// <summary>
        ///// В конкурсной группе для направления {0} указаны формы обучения и источники финансирования, отсутствующие в объеме приема.
        ///// </summary>
        //public const int CompetitiveGroupHasDirectionOnlyWithTargetPlaces = 12;

        /// <summary>
        /// Объем приема для данного направления не импортирован.
        /// </summary>
        public const int AdmissionVolumeIsNotImportedForDirection = 13;

        /// <summary>
        /// Имеются связанные данные в целевой системе, которые необходимо удалить для выполнения импорта.
        /// </summary>
        public const int DependedObjectsExists = 14;

        /// <summary>
        /// В БД найден конкурс с тем же наименованием ({0}), и другим UID ({1})
        /// </summary>
        public const int CompetitiveGroupNameEsistsInDbWithOtherUID = 15;

        /// <summary>
        /// У другой ПК имеется объем приема с таким же UID
        /// </summary>
        public const int AdmissionVolumeHasSameUIDAnotherCampaign = 16;

        //// <summary>
        //// В конкурсной группе не найдено вступительного испытания с обязательным предметом "Русский язык".
        //// </summary>
        //public const int CompetitiveGroupMustHaveRussianLanguangeEntranceTest = 15;

        //// <summary>
        //// В конкурсной группе должно присутствовать одно профильное испытание.
        //// </summary>
        //public const int CompetitiveGroupMustHaveOneProfileEntranceTest = 16;

        /// <summary>
        /// Название конкурса ({0}) должно быть уникальным в рамках приёмной кампании.
        /// </summary>
        public const int CompetitiveGroupNameMustBeUnique = 17;

        /// <summary>
        /// UID для объектов одного типа в рамках одной коллекции должны быть уникальны.
        /// </summary>
        public const int UIDMustBeUniqueInCollection = 18;

        /// <summary>
        /// UID ({1}) должно быть уникальным в разрезе всех импортируемых коллекций объектов данного типа ({0}).
        /// </summary>
        public const int UIDMustBeUniqueForAllObjectInstancesOfType = 19;

        /// <summary>
        /// Объект содержит коллекцию элементов ({0}), у которых есть дубликаты по UID ({1}).
        /// </summary>
        public const int UIDMustBeUniqueForChildrenObjects = 20;

        /// <summary>
        /// Наименования объектов одного типа в рамках одной коллекции должны быть уникальны.
        /// </summary>
        public const int NameMustBeUniqueInCollection = 21;

        /// <summary>
        /// Наименование ({1}) должно быть уникальным в разрезе всех импортируемых коллекций объектов данного типа ({0}).
        /// </summary>
        public const int NameMustBeUniqueForAllObjectInstancesOfType = 22;

        /// <summary>
        /// Регион {0} отсутствует для указанного идентификатора.
        /// </summary>
        //public const int RegionIsNotFounded = 21;

        /// <summary>
        /// Страна {0} отсутствует для указанного идентификатора.
        /// </summary>
        //public const int CountryIsNotFounded = 22;

        /// <summary>
        /// Страна {0} не содержит указанный регион {1}.
        /// </summary>
        //public const int RegionIsNotFoundedForCountry = 23;

        /// <summary>
        /// Номер заявления не уникален.
        /// </summary>
        public const int ApplicationNumberIsNotUnique = 24;

        ///// <summary>
        ///// В БД существует конкурсная группа с таким же именем, которая на данный момент не может быть удалена.
        ///// </summary>
        //public const int CompetitiveGroupWithSameNameExists = 25;

        /// <summary>
        /// Предмет не найден для кода {0}.
        /// </summary>
        public const int SubjectIsNotFounded = 26;

        /// <summary>
        /// Результат испытания (значение {0}) должен принимать значения от 0 до {1}
        /// </summary>
        public const int ResultValueRange = 27;

        ///// <summary>
        ///// В системе уже существует данное направление у конкурсной группы.
        ///// </summary>
        //public const int CompetitiveGroupItemOnSameDirectionExists = 28;

        ///// <summary>
        ///// Источник вступительного испытания код {0} не найден.
        ///// </summary>
        //public const int EntranceTestSourceIDNotFound = 29;

        ///// <summary>
        ///// В конкурсной группе уже есть вступительное испытание {0} с такими же данными
        ///// </summary>
        //public const int CompetitiveGroupEntranceTestItemUniqueConstraint = 30;

        ///// <summary>
        ///// Не найдено конкурсной группы для заявления.
        ///// </summary>
        //public const int CompetitiveGroupNotFoundForApplication = 31;

        ///// <summary>
        ///// Конкурсная группа не указана для заявления.
        ///// </summary>
        //public const int CompetitiveGroupNotSpecifiedForApplication = 32;

        /// <summary>
        /// Недопустимый тип общей льготы: возможные значения "Без вступ. испытаний", "Вне Конкурса", "Преимущественное право на поступление"
        /// </summary>
        public const int NotAllowedCommonBenefitTypeForApplication = 33;

        ///// <summary>
        ///// Не указан документ основания для льготы РВИ
        ///// </summary>
        //public const int DocumentNotSpecifiedForBenefit = 34;

        /// <summary>
        /// В конкурсной группе не может быть более одного испытания профильной направленности
        /// </summary>
        //public const int CompetitiveGroupCantHaveMoreThanOneExtraProfileSubject = 35;

        /// <summary>
        /// У свойства {0} объекта указано некорректное справочное значение.
        /// </summary>
        public const int DictionaryItemAbsent = 36;

        /// <summary>
        /// Направление указанное для объема приема не разрешено для института.
        /// </summary>
        public const int AdmissionVolumeContainsNotAllowedDirections = 37;

        ///// <summary>
        ///// В конкурсной группе более одного раза указано направление в перечне направлений.
        ///// </summary>
        //public const int DirectionIDDuplicatedInCompetitiveGroup = 38;

        /// <summary>
        /// У объема приема дублирующиеся направления.
        /// </summary>
        public const int AdmissionVolumeNonUniqueDirections = 39;

        /// <summary>
        /// Количество мест ({4}) по направлению ({0}) в разрезе источника финансирования({1}) и формы обучения ({2}) {3}
        /// </summary>
        public const int CompetitiveGroupPlacesOnDirectionExceeded = 40;

        ///// <summary>
        ///// Количество мест ({4}) по направлению ({0}) в разрезе источника финансирования({1}) и формы обучения ({2}) {3}
        ///// </summary>
        //public const int CompetitiveGroupPlacesOnDirectionExceeded = 41;

        ///// <summary>
        ///// Конкурсная группа не может быть удалена (какая-то ошибка в логике удаления)
        ///// </summary>
        //public const int CompetitiveGroupCannotBeRemoved = 41;

        ///// <summary>
        ///// В Конкурсной группе имеется неразрешённая общая льгота.
        ///// </summary>
        //public const int CompetitiveGroupContainsNotAllowedCommonBenefit = 42;

        /// <summary>
        /// У свойства {0} объекта указаны повторяющиеся справочное значение.
        /// </summary>
        public const int DictionaryItemNonUnique = 43;

        /// <summary>
        /// Льгота содержит указание на все олимпиады и список олимпиад
        /// </summary>
        public const int BenefitContainsOlympicsAndAllOlympics = 44;

        /// <summary>
        /// Льгота не содержит указания на все олимпиады и сами олимпиады
        /// </summary>
        public const int BenefitContainsNoOlympicsAndNoAllOlympics = 45;

        /// <summary>
        /// Вступительное испытание имеет балл ниже, чем минимальный, указанный для данного предмета
        /// </summary>
        public const int EntranceTestContainsScoreLowerThanRequired = 46;

        /// <summary>
        /// В Конкурсной группе имеются направления не разрешенные для института.
        /// </summary>
        public const int CompetitiveGroupContainsNotAllowedByInstituteDirections = 47;

        /// <summary>
        /// У вступительного испытания указан некорректный данный тип льготы.
        /// </summary>
        public const int CompetitiveGroupContainsNotAllowedBenefitType = 48;

        ///// <summary>
        ///// Вступительное испытание содержит льготу, отсутствующую в пакете импорта.
        ///// </summary>
        //public const int EntranceTestBenefitItemNotExistsInPackage = 49;

        ///// <summary>
        ///// Дата отзыва заявления должна быть позднее даты регистрации.
        ///// </summary>
        //public const int ApplicationLastDenyDateShouldBeGreaterRegistrationDate = 50;

        ///// <summary>
        ///// Заявление содержит неразрешенные для конкурсной группы форму обучения и источник финансирования.
        ///// </summary>
        //public const int ApplicationContainsNotAllowedFinSourceEducationForm = 51;

        /// <summary>
        /// В заявлении указан несуществующий идентификатор конкурсной группы. UID: {0}
        /// </summary>
        public const int ApplicationContainsInvalidCompetitiveGroupID = 52;

        /// <summary>
        /// В качестве основания для льготы указан некорректный документ.
        /// </summary>
        public const int InvalidDocumentSpecifiedForBenefit = 53;

        /// <summary>
        /// Заявление содержит некорректный UID организации целевого приема.
        /// </summary>
        public const int ApplicationContainsIncorrectTargetOrganizationUID = 54;

        /// <summary>
        /// В БД существует заявление с тем же номером но другим UID'ом.
        /// </summary>
        public const int ApplicationNumberIsNotCorrelateWithUID = 55;


        ///// <summary>
        ///// Указанное в формах обучения и источниках финансирования направление (UID: {0}) не содержится в конкурсной группе (UID: {1})
        ///// </summary>
        //public const int ApplicationFinSourceCGIIsNotInCG = 56;



        /// <summary>
        /// В БД существует приемная кампания с таким же типом и годом начала.
        /// </summary>
        public const int CampaignWithSameTypeAndYearExistsDb = 58;

        /// <summary>
        /// В пакете уже имеется приемная кампания с таким же типом и годом начала.
        /// </summary>
        public const int CampaignWithSameTypeAndYearExistsPackage = 59;

        /// <summary>
        /// В БД существует приемная кампания с таким же именем.
        /// </summary>
        public const int CampaignWithSameNameExists = 60;

        /// <summary>
        /// Приемная кампания содержит некорректные данные.
        /// </summary>
        public const int CampaignWithInvalidData = 61;

        ///// <summary>
        ///// Приемная кампания содержит некорректные сроки проведения.
        ///// </summary>
        //public const int CampaignWithInvalidDate = 62;

        /// <summary>
        /// Объем приема указывает на отсутствующую кампанию.
        /// </summary>
        public const int AdmissionVolumeContainsNotAllowedCampaign = 63;

        /// <summary>
        /// Объем приема указывает на уровень образования, отсутствующий у приемной кампании.
        /// </summary>
        public const int AdmissionVolumeContainsNotAllowedCampaignEducationLevel = 64;

        /// <summary>
        /// Объем приема указывает на форму обучения, отсутствующую у приемной кампании.
        /// </summary>
        public const int AdmissionVolumeContainsNotAllowedCampaignEducationForm = 65;

        ///// <summary>
        ///// У приемной кампании для дополнительного набора не должно быть стадий
        ///// </summary>
        //public const int CampaignStageAndAdditionalExists = 65;

        /// <summary>
        /// Конкурсная группа указывает на отсутствующую кампанию.
        /// </summary>
        public const int CompetitiveGroupContainsNotAllowedCampaign = 66;

        ///// <summary>
        ///// Конкурсная группа указывает на неверный курс у приемной кампании.
        ///// </summary>
        //public const int CompetitiveGroupContainsNotAllowedCampaignCourse = 68;

        ///// <summary>
        ///// Приемная кампания содержит недопустимую комбинацию уровней образования и курсов ({0}).
        ///// </summary>
        //public const int CampaignWithInvalidEducationLevelsCombination = 67;

        /// <summary>
        /// Для указанного типа документа должно быть российское гражданство.
        /// </summary>
        public const int IdentityDocumentContainsInvalidNationality = 69;

        ///// <summary>
        ///// У свойства {0} документа с UID: {1} указано некорректное справочное значение.
        ///// </summary>
        //public const int DictionaryItemAbsentForApp = 70;

        ///// <summary>
        ///// Не найден элемент конкурсной группы для заявления.
        ///// </summary>
        //public const int CompetitiveGroupItemNotFoundForApplication = 71;

        ///// <summary>
        ///// Элемент конкурсной группы не указан для заявления.
        ///// </summary>
        //public const int CompetitiveGroupItemNotSpecifiedForApplication = 72;

        ///// <summary>
        ///// В заявлении указан несуществующий идентификатор элемента конкурсной группы. UID: {0}
        ///// </summary>
        //public const int ApplicationContainsInvalidCompetitiveGroupItemID = 73;

        ///// <summary>
        ///// Общая льгота указывает на некорректную конкурсную группу.
        ///// </summary>
        //public const int CommonBenefitContainsInvalidCompetitiveGroup = 74;

        /// <summary>
        /// Для данного предмета не разрешено указывать Свидетельство ЕГЭ в качестве основания для оценки.
        /// </summary>
        public const int NotAllowedSubjectForEgeDocument = 75;

        ///// <summary>
        ///// Для данной формы обучения и источника финансирования указан некорректный или недопустимый этап приёма.
        ///// </summary>
        //public const int CampaignDateWithInvalidStage = 76;

        ///// <summary>
        ///// В датах приемной кампании отсутствует необходимый этап приема.
        ///// </summary>
        //public const int CampaignDateWithMissingStage = 77;

        /// <summary>
        /// У абитуриента отсутствует UID.
        /// </summary>
        public const int EntrantUIDIsMissing = 78;

        ///// <summary>
        ///// Не заполнены сроки проведения по одной или нескольким формам обучения или источникам финансирования в рамках указанной приемной кампании
        ///// </summary>
        //public const int AdmissionVolumeContainsNotAllowedNumbers = 79;

        /// <summary>
        /// Приемная кампания содержит уровни образования, которые неразрешены для данного ОУ ({0}).
        /// </summary>
        public const int CampaignWithInvalidEducationLevelsByInstitution = 80;

        /// <summary>
        /// Документ с данным UID'ом ({0}) уже существует в базе или пакете, при этом имеет другой тип.
        /// </summary>
        public const int InvalidDocumentTypeRelatedToExisting = 81;

        ///// <summary>
        ///// Невозможно импортировать заявление со статусом "В приказе", следует изменить статус и отдельно передать список приказов.
        ///// </summary>
        //public const int ApplicationInOrderCannotBeImported = 82;

        /// <summary>
        /// В свидетельстве ЕГЭ одновременно указан год и дата свидетельства, при этом они не совпадают друг с другом.
        /// </summary>
        public const int EgeDocumentYearDateInvalid = 83;

        /// <summary>
        /// Заявление должно содержать один из следующих документов об образовании: {0}.
        /// </summary>
        public const int ApplicationDoesNotContainRequiredEduDocument = 84;

        /// <summary>
        /// В БД существует заявление с тем же UID'ом но другим номером.
        /// </summary>
        public const int ApplicationUIDIsNotCorrelateWithNumber = 85;

        /// <summary>
        /// В БД существует заявление с тем же UID'ом и номером, но с другой датой регистрации.
        /// </summary>
        public const int ApplicationUIDNumberIsNotCorrelateWithDate = 86;

        /// <summary>
        /// Допускается импортировать заявления только со статусами "Новое", "Принято" или "Отозвано".
        /// </summary>
        public const int ApplicationCannotBeImportedExceptAcceptedOrNewOrDenied = 87;

        ///// <summary>
        ///// Уже существует организация целевого приема с данным UID'ом, но другим наименованием.
        ///// </summary>
        //public const int TargetOrganizationNameIsNotUnique = 88;

        /// <summary>
        /// Для данного типа документа, удостоверяющего личность, необходимо указать серию.
        /// </summary>
        public const int IdentityDocumentRequireSeries = 89;

        /// <summary>
        /// Невозможно импортировать заявление для приёмной кампании со статусом отличным от "Идет набор".
        /// </summary>
        public const int ApplicationCannotImportedForCampaignStatus = 90;

        ///// <summary>
        ///// Данный диплом нельзя использовать в качестве источника для результата вступительного испытания.
        ///// </summary>
        //public const int OlympicDocumentCannotBeUsedAsSource = 91;

        /// <summary>
        /// Невозможно импортировать заявление, содержащее конкурсную группу без вступительных испытаний.
        /// </summary>
        public const int ApplicationCannotImportedForCGWithoutEntranceTests = 92;

        /// <summary>
        /// Для данного предмета не разрешено указывать Справку ГИА в качестве основания для оценки.
        /// </summary>
        public const int NotAllowedSubjectForGiaDocument = 93;

        /// <summary>
        /// Уже существует организация целевого приема с данным наименованием, но другим UID'ом.
        /// </summary>
        public const int TargetOrganizationSameNameDifferentUID = 94;

        /// <summary>
        /// В свидетельстве ЕГЭ необходимо передать год или дату.
        /// </summary>
        public const int EgeDocumentDateMissing = 95;

        ///// <summary>
        ///// В датах приемной кампании дублируются данные.
        ///// </summary>
        //public const int CampaignDateDuplicateData = 96;

        /// <summary>
        /// В БД уже существует КГ с целевым набором но с другим UID
        /// </summary>
        public const int CompetitiveGroupTargeExistsInDb = 97;

        /// <summary>
        /// В приказ можно включить только заявления, для которых предоставлены оригиналы документов или справка об обучении в другом ВУЗе
        /// </summary>
        public const int StudentDocumentRequired = 98;

        /// <summary>
        /// В приказ на бюджетные места можно включить только заявления, для которых предоставлены оригиналы документов
        /// </summary>
        public const int OriginalDocumentsRequired = 99;

        /// <summary>
        /// Конкурсная группа не может быть удалена, так как существуют связанные с ней заявления ({0})
        /// </summary>
        public const int CompetitiveGroupCannotBeRemovedWithApplications = 100;

        ///// <summary>
        ///// В БД найдена организация целевого приема с UID ({0}), принадлежащая другой конкурсной группе
        ///// </summary>
        //public const int CompetitiveGroupTargetExistsInDbInOtherCompetitiveGroup = 101;

        ///// <summary>
        ///// В БД найдены места для целевого приема с UID ({0}), принадлежащие другой организации целевого приема UID ({1})
        ///// </summary>
        //public const int CompetitiveGroupTargetItemExistsInDbInOtherCompetitiveGroupTarget = 102;

        ///// <summary>
        ///// В БД найдено направление конкурсной группы с UID ({0}), принадлежащее другой конкурсной группе UID ({1})
        ///// </summary>
        //public const int CompetitiveGroupItemExistsInDbInOtherCompetitiveGroup = 103;

        /// <summary>
        /// В БД найдены вступительные испытания конкурсной группы с UID ({0}), принадлежащие другой конкурсной группе UID ({1})
        /// </summary>
        public const int EntranceTestItemCExistsInDbInOtherCompetitiveGroup = 104;

        /// <summary>
        /// В БД найдена льгота с UID ({0}), принадлежащая другим вступительным испытаниям конкурсной группы UID ({1})
        /// </summary>
        public const int BenefitItemCExistsInDbInOtherEntranceTestItemC = 105;

        /// <summary>
        /// В БД найдена льгота с UID ({0}), принадлежащая другой конкурсной группе UID ({1})
        /// </summary>
        public const int BenefitItemCExistsInDbInOtherCompetitiveGroup = 106;

        ///// <summary>
        ///// Заявление ({0}) содержит льготу ({1}), уровень которой ниже разрешенного в конкурсной группе ({2})
        ///// </summary>
        //public const int BenefitItemCNotAllowedInCompetitiveGroup = 107;

        /// <summary>
        /// У льготы должен быть задан UID
        /// </summary>
        public const int BenefitMustHaveUID = 107;

        ///// <summary>
        ///// Заявление ({0}) содержит льготу ({1}) для призера, в конкурсной группе ({2}) разрешена льгота только для победителя
        ///// </summary>
        //public const int BenefitItemCAllowedToWinnerOnly = 108;

        //   /// <summary>
        //   /// В случае предоставления льготы призерам олимпиады должна быть предоставлена льгота того же или более высокого порядка также и победителям олимпиады
        //   /// </summary>
        //public const int BenefitNotContainsWinnerBenefitFlag = 109;

        //   /// <summary>
        //   /// Оригиналы документов предоставлены, но не указана дата или указана дата предоставления для документа без оригиналов
        //   /// </summary>
        //   public const int DocumentsFailRecievingCheck = 110;

        //   ///// <summary>
        //   ///// Олимпиада ({0}) не разрешена в конкурсной группе ({1})
        //   ///// </summary>
        //   //   public const int OlympicTypeIsNotAllowedInCompetitiveGroup = 111;

        //   //   /// <summary>
        //   //   /// Заявление ({0}) содержит льготу ({1}) с неопределенным уровнем для конкурсной группы({1}). В конкурсной группе ({1}) для олимпиады ({2}) разрешены уровни: ({3})
        //   //   /// </summary>
        //   //   public const int OlympicLevelIsNotAllowedInCompetitiveGroup = 112;

        /// <summary>
        /// Заявление ({0}) включено в приказ. Невозможно обновить заявление в приказе.
        /// </summary>
        public const int ApplicationsInOrderIsNotAllowedToUpdate = 113;

        ///// <summary>
        ///// Прием на обучение должен осуществляться раздельно по программам бакалавриата, специалитета, магистратуры, программам СПО и программам подготовки кадров высшей квалификации
        ///// </summary>
        //public const int CompetitiveGroupCannotContainVariousEdlevels = 114;

        /// <summary>
        /// Вступительное испытание (UID:{0}) относится к конкурсу, не указанному в данном заявлении
        /// </summary>
        public const int EntranceTestIsNotPartOfCompetitiveGroup = 115;

        /// <summary>
        /// Для вступительного испытания {0} указан некорректный приоритет
        /// </summary>
        public const int EntranceTestPriorityIncorrect = 116;

        /// <summary>
        /// Квота приёма лиц, имеющих особое право, может быть указана только для бакалавриата или специалитета
        /// </summary>
        public const int CompetitiveGroupItemQuotaIncorrect = 117;

        /// <summary>
        /// Для вступительных испытаний в конкурсах по кадрам высшей квалификации должен быть задан приоритет (не 0) хотя бы у одного испытания
        /// </summary>
        public const int CompetitiveGroupKVKMustHaveOneEntranceTestPriority = 118;

        ///// <summary>
        ///// Для индивидуального достижения не задано название - обязательный атрибут
        ///// </summary>
        //public const int IndividualAchivementNameIsEmpty = 119;

        /// <summary>
        /// Для индивидуального достижения не задана ссылка на подтверждающий документ
        /// </summary>
        public const int IndividualAchivementDocumentUIDIsEmpty = 120;

        /// <summary>
        /// Для индивидуального достижения не найден подтверждающий документ с типом \"иные документы\" (CustomDocuments)
        /// </summary>
        public const int IndividualAchivementDocumentNotFound = 121;

        //public const int IncorrectStageInRecommendedList = 122;
        //public const int IncorrectApplicationForRecommendedList = 123;
        //public const int IncorrectGroupFormSource = 124;
        //public const int IncorrectLevelDirection = 125;
        //public const int IncorrectRating = 126;

        /// <summary>
        /// Пара AdmissionVolumeUID и LevelBudget должны быть уникальны в коллекции объектов DistributedAdmissionVolume.
        /// </summary>
        public const int AdmissionVolumeUIDAndBudgetLevelMustBeUniqueInCollection = 127;

        /// <summary>
        /// Объем приема для данного распределенного объема приема не импортирован.
        /// </summary>
        public const int AdmissionVolumeIsNotImportedForDistributedAdmissionVolume = 128;

        ///// <summary>
        ///// Распределенный объем приема содержит некорректные числовые значения 
        ///// </summary>
        //public const int DistributedAdmissionVolumeInvalidNumbers = 129;

        /// <summary>
        /// Сумма распределенных объемов приема превышает исходный объем приема.
        /// </summary>
        public const int DistributedAdmissionVolumeNumbersSumBiggerAV = 130;


        /// <summary>
        /// Пара IAUID и CampaignUID должны быть уникальны в коллекции объектов InstitutionArchivements
        /// </summary>
        public const int IAUIDAndCampaignUIDMustBeUniqueInCollection = 131;

        ///// <summary>
        ///// CampaignUID должны быть уникальны в коллекции объектов InstitutionArchivements
        ///// </summary>
        //public const int CampaignUIDMustBeUniqueInCollection = 132;

        /// <summary>
        /// Недопустимое значение в поле {0}
        /// </summary>
        public const int IAIncorrentFieldValue = 133;

        ///// <summary>
        ///// Для места целевого приема с UID ({0}) не найдено соответствующего элемента конкурсной группы
        ///// </summary>
        //public const int CompetitiveGroupTargetItemHasNotCompetitiveGroupItem = 134;

        ///// <summary>
        ///// 
        ///// </summary>
        //public const int CompetitiveGroupTargetItemRefersCompetitiveGroupItemAlreadyInUse = 135;

        /// <summary>
        /// Заявление ({0}) включено в списки рекомендованных. Невозможно обновить заявление в списках.
        /// </summary>
        public const int ApplicationsInRecListIsNotAllowedToUpdate = 135;

        /// <summary>
        /// Невозможно импортировать заявление для нескольких приёмной кампаний.
        /// </summary>
        public const int ApplicationCannotImportedForDifferentCampaigns = 136;

        ///// <summary>
        ///// В заявлении указан элемент конкурса (UID: {0}), ссылающийся на несуществующий идентификатор конкурсной группы
        ///// </summary>
        //public const int ApplicationContainsCompetitiveGroupItemWithInvalidCGID = 137;

        /// <summary>
        /// В формах обучения и источниках финансирования указан несуществующий конкурс (UID: {0})
        /// </summary>
        public const int ApplicationContainsCompetitiveGroupNotSpecified = 138;

        ///// <summary>
        ///// В формах обучения и источниках финансирования указан элемент конкурсной группы (UID: {0}), который не указан в перечне элементов конкурсных групп заявления
        ///// </summary>
        //public const int ApplicationContainsCompetitiveGroupItemNotSpecified = 139;

        /// <summary>
        /// В перечне льгот указана конкурсная группа (UID: {0}), которая не указана в перечне конкурсных групп заявления
        /// </summary>
        public const int ApplicationBenefitsContainsCompetitiveGroupNotSpecified = 140;


        /// <summary>
        /// Для индивидуального достижения не задан идентификатор - обязательный атрибут
        /// </summary>
        public const int IndividualAchivementUIDIsEmpty = 141;

        /// <summary>
        /// У индивидуального достижения (IAUID: {0}) указан несуществующий идентификатор достижения приемной кампании
        /// </summary>
        public const int IndividualAchivementInstitutionAchievementUIDNotExists = 142;
        /// <summary>
        /// У индивидуального достижения (IAUID: {0}) указана приемная кампания, отличная от приемной кампании заявления
        /// </summary>
        public const int IndividualAchivementInstitutionAchievementUIDWrongCampaign = 143;
        /// <summary>
        /// У индивидуального достижения (IAUID: {0}) балл за достижение ({1}) превышает максимально возможный ({2})
        /// </summary>
        public const int IndividualAchivementMarkBiggerMaxValue = 144;

        /// <summary>
        /// Идентификаторы предметов дублируются
        /// </summary>
        public const int DublicateSubjects = 145;

        /// <summary>
        /// Идентификатор индивидуального достижения (IAUID: {0}) должен быть уникален в рамках ОО
        /// </summary>
        public const int IndividualAchivementUIDIsNotUniqueInDb = 146;
        /// <summary>
        /// Идентификатор индивидуального достижения (IAUID: {0}) должен быть уникален в пакете
        /// </summary>
        public const int IndividualAchivementUIDIsNotUniqueInPackage = 147;

        /// <summary>
        /// Для индивидуального достижения не задан идентификатор ИД ОО
        /// </summary>
        public const int InstitutionAchievementUIDNotSpecified = 148;


        /// <summary>
        /// Льгота для всех олимпиад должна содержать уровни олимпиад
        /// </summary>
        public const int BenefitForAllOlympicMustContainLevel = 149;
        /// <summary>
        /// Льгота для всех олимпиад должна содержать классы олимпиад
        /// </summary>
        public const int BenefitForAllOlympicMustContainClass = 150;
        /// <summary>
        /// Льгота для всех олимпиад должна содержать профили олимпиад
        /// </summary>
        public const int BenefitForAllOlympicMustContainProfile = 151;
        ///// <summary>
        ///// Все передаваемые в льготе олимпиады должны быть одного года
        ///// </summary>
        //public const int BenefitOlympicsMustBeTheSameYear = 152;

        /// <summary>
        /// Передаваемый профиль олимпиады ProfileID={0} не соответствует олимпиаде OlympicID={1}
        /// </summary>
        public const int OlympicProfileFromOtherOlympic = 153;

        /// <summary>
        /// Передаваемый профиль олимпиады ProfileID={0} не соответствует передаваемому уровню LevelID={1}
        /// </summary>
        public const int OlympicProfileHasOtherLevel = 154;


        /// <summary>
        /// Документ гражданина Крыма с UID {0} не найден ни в пакете, ни в базе данных
        /// </summary>
        public const int EntrantDocumentFromKrymUIDNotFound = 155;

        /// <summary>
        /// Не допускается импортировать заявления, которые уже имеются в системе со статусами "Редактируется" или "В приказе"
        /// </summary>
        public const int ApplicationStatus1Or8CannotBeImported = 156;

        ///// <summary>
        ///// Не допускается импортировать заявления, которые уже имеются в системе со статусами "Принято" или "Отозвано", изменяя статус на "Новое"
        ///// </summary>
        //public const int ApplicationStatus4Or6CannotBeImportedStatus2 = 157;

        /// <summary>
        /// Документ для ВИ с созданием специальных условий с UID {0} не найден ни в пакете, ни в базе данных
        /// </summary>
        public const int EntrantDocumentVIisDisabledUIDNotFound = 158;

        /// <summary>
        /// Заявление со статусом "Новое" не может содержать FinSourceEduForm.IsAgreedDate
        /// </summary>
        public const int ApplicationStatus2ContainsAgreedDate = 159;

        /// <summary>
        /// Абитуриент не может иметь более 1 согласия на зачисление при отстутствии отказа или более 2 согласий при наличии отказов по уровню обучения: бакалавриат, специалитет, форме: очная, очно-заочная
        /// </summary>
        public const int ApplicationContainsMoreThenOneAgreedDate = 160;

        /// <summary>
        /// У конкурса нет ВИ с IsForSPOandVO = 1
        /// </summary>
        public const int CompetitiveGroupMustHaveEntranceTestWithSPO = 161;

        /// <summary>
        /// Льгота типа 4 должна быть в конкурсе с источником \"Квота\" (SourceID = 20)
        /// </summary>
        public const int ApplicationBenefitsContainsBenefit4InCGNotSource20 = 162;

        /// <summary>
        /// Фамилия, Имя, Отчество и пол абитуриента должны совпадать с данными в Документах, удостоверяющих личность
        /// </summary>
        public const int IdentityDocumentHasOtherValues = 163;

        /// <summary>
        /// Дата рождения абитуриента должна совпадать с данными в Документах, удостоверяющих личность
        /// </summary>
        public const int IdentityDocumentHasOtherBirthDate = 164;

        /// <summary>
        /// Данные по олимпиаде, в результатах ВИ UID={0} не соответствуют льготам конкурса: {1}
        /// </summary>
        public const int BenefitOlympicHasError = 165;

        /// <summary>
        /// У абитуриента должен быть указан email и/или почтовый адрес
        /// </summary>
        public const int EntrantMustHaveEmailOrAddress = 166;

        /// <summary>
        /// В адресе абитуриента должно быть заполнено поле Address
        /// </summary>
        public const int EntrantMailAddressMustHaveAddress = 167;

        /// <summary>
        /// Общая льгота с UID = {0} должна либо содержать минимальный балл по предмету, либо иметь признак олимпиады творческой или в области спорта
        /// </summary>
        public const int CommonBenefitMustHaveSubjectOrCheckbox = 168;

        /// <summary>
        /// Нельзя одновременно указывать места на каждую целевую организацию и на весь конкурс.
        /// </summary>
        public const int CompetitiveGroupContainsNumberTargetXTwice = 169;

        /// <summary>
        /// Диплом победителя или призера олимпиады с UID = {0} не может быть использован дважды в одном заявлении в качестве льготы 
        /// </summary>
        public const int EntrantDocumentOlympicCannotBeUserTwiceInApplicaiton = 170;

        /// <summary>
        /// Льгота с UID = {0} указана в конкурсе с UID = {1}, однако конкурс не содержит такой льготы
        /// </summary>
        public const int ApplicationETResultHasWrongUID = 171;

        /// <summary>
        /// Eсли в OlympicDocument.OlympicSubjectID передан предмет, не являющийся общеобразовательным, то должен быть заполнен EgeSubjectID
        /// </summary>
        public const int OlympicDocumentSpecifiedSubjectMustBeEge = 172;

        /// <summary>
        /// OlympicDocument.OlympicSubjectID - должен соответствовать профилю олимпиады
        /// </summary>
        public const int OlympicDocumentOlympicSubjectMustMatchProfile = 173;

        /// <summary>
        /// Допускается проставлять согласие только если у заявления получены оригиналы документов
        /// </summary>
        public const int ApplicationHasAgreedDateButNoDocumentsWithReceicedDate = 174;

        /// <summary>
        /// Сумма баллов индивидуальных достижений по одному заявлению не должна превышать 10
        /// </summary>
        //public const int IndividualAchivementSumMarksMoreThen10 = 175;

        /// <summary>
        /// Для конкурсов позднее 2016 года запрещено передавать признак IsForKrym
        /// </summary>
        public const int CompetitiveGroupIsFromKrymWrongYear = 176;

        /// <summary>
        /// Минимальный балл (значение {0}) должен принимать значения от 0 до {1}
        /// </summary>
        public const int MinValueRange = 177;

        /// <summary>
        /// В качестве льгот выбраны олимпиады ({0}), срок действия которых истек
        /// </summary>
        public const int OlympicYearExpired = 178;

        /// <summary>
        /// Абитуриент не может иметь отказ от зачисления при отстутствии согласия на зачисление по тому же условию приема
        /// </summary>
        public const int ApplicationContainsAgreedDateWithoutDisagreed = 179;

        /// <summary>
        /// При импорте заявления со статусом "Отозвано"(6) необходимо передавать поля ReturnDocumentsTypeId, ReturnDocumentsDate
        /// </summary>
        public const int ApplicationNeedReturnDocumentsInfo = 180;

        /// <summary>
        /// Совокупность значений в полях "Код ОП" и "Наименование ОП" должна быть уникальной среди всех образовательных программ ОО
        /// </summary>
        public const int InstitutionProgramSameNameAndCodeDifferentUID = 181;

        #endregion

        #region РВИ

        /// <summary>
        /// Изменена конкурсная группа у заявления, у которого имеются результаты вступительных испытаний
        /// </summary>
        public const int EntranceTestResultsExistsForApplication = 1001;

        /// <summary>
        /// Отредактированы результаты ВИ. Связанные данные: приказ
        /// </summary>
        public const int EntranceTestResultsChangedForApplicationInOrder = 1002;

        /// <summary>
        /// Изменился РВИ.
        /// </summary>
        public const int EntranceTestResultChanged = 1003;

        /// <summary>
        /// Изменилось свидетельство ЕГЭ для РВИ.
        /// </summary>
        public const int EntranceTestResultEgeDocumentChanged = 1004;

        /// <summary>
        /// Изменился документ для олимпиады для РВИ.
        /// </summary>
        public const int EntranceTestResultOlympicDocumentChanged = 1005;

        /// <summary>
        /// В БД указан РВИ, который отсутствует в данных импорта
        /// </summary>
        public const int EntranceTestResultExistsInDbAndNotSpecifiedInImport = 1006;

        /// <summary>
        /// В списке документов заявления не найдено указанного ЕГЭ документа {0} для вступительного испытания {1}.
        /// </summary>
        public const int ReferencedEgeDocumentIsAbsent = 1007;

        /// <summary>
        /// Изменился документ-основание для общей льготы
        /// </summary>
        public const int ApplicationCommonBenefitDocumentChanged = 1008;

        /// <summary>
        /// Не найдено соответствующего вступительного испытания.
        /// </summary>
        public const int CompetitiveGroupEntranceTestNotFoundForEntranceTestResult = 1009;

        /// <summary>
        /// Для вступительного испытания {0} уже проимпортирован результат {1}. 
        /// </summary>
        public const int EntranceTestResultAlreadyImportedForEntranceTestItem = 1010;

        /// <summary>
        /// Не приложен документ - основание для оценки.
        /// </summary>
        public const int DocumentNotAttachedToEntranceTestResult = 1011;

        /// <summary>
        /// Повторяются предметы в свидетельстве ЕГЭ
        /// </summary>
        public const int SubjectsInEgeDocumentIsDupllicated = 1012;

        /// <summary>
        /// Вступительное испытание указано более одного раза.
        /// </summary>
        public const int CompetitiveGroupEntranceTestDuplicateInPackage = 1013;

        /// <summary>
        /// Повторяются предметы в справке ГИА.
        /// </summary>
        public const int SubjectsInGiaDocumentIsDupllicated = 1014;

        /// <summary>
        /// Приложено свидетельство ЕГЭ к результату ВИ, не позволяющего использовать свидетельство ЕГЭ в качестве основания для оценки
        /// </summary>
        public const int NotAllowedEgeDocumentForEntranceTestResult = 1015;

        /// <summary>
        /// "Для указанного года олимпиады не задан общий минимальный балл ЕГЭ"
        /// </summary>
        public const int NoMinSystemEGE = 1016;

        /// <summary>
        /// "Для олимпиад 2014 года и более поздних должен быть указан минимальный балл ЕГЭ"
        /// </summary>
        public const int NoMinEgeForOlympics = 1017;

        /// <summary>
        /// "Минимальный балл ЕГЭ для использования льготы ({0}) не может быть меньше общесистемного минимального балла ЕГЭ ({1})"
        /// </summary>
        public const int BenefitEGELessSystemMinEGE = 1018;

        /// <summary>
        /// "Для олимпиад 2014 года и более поздних должен быть задан хотя бы один предмет, минимально необходимый балл по которому даст право на использование льготы"
        /// </summary>
        public const int NoMinEGEMarks = 1019;

        /// <summary>
        /// Одна или несколько конкурсных групп содержат льготы с некорректными минимальными баллаим ЕГЭ
        /// </summary>
        public const int CgHasIncorrectBenefits = 1020;

        /// <summary>
        /// Баллы по предмету должны быть от 1 до 999. Указано значение {0}
        /// </summary>
        public const int SubjectHasIncorrectValue = 1021;

        /// <summary>
        /// Тип вступительного испытания {0} не найден.
        /// </summary>
        public const int EntranceTestTypeIDNotFound = 1022;

        /// <summary>
        /// Изменение предмета ВИ невозможно, поскольку имеются заявления с документами по ВИ c UID = {0}.
        /// </summary>
        public const int EntranceTestCannotChangeSubject = 1023;

        /// <summary>
        /// В конкурсе уже имеется ВИ с таким же типом {0} и предметом {1}
        /// </summary>
        public const int EntranceTestDublicateTypeAndSubject = 1024;

        /// <summary>
        /// Не найдено заменяемое ВИ с UID = {0}
        /// </summary>
        public const int EntranceTestReplacedItemNotFound = 1025;
        #endregion

        #region Приказ

        /// <summary>
        /// Не допускается в одном пакете осуществлять импорт заявления (UID: {0}) и его включение в приказ (UID: {1})
        /// </summary>
        public const int OrderOfAdmissionApplicationInImportedApplications = 3000;

        /// <summary>
        /// Заявление, включённое в приказ не найдено.
        /// </summary>
        public const int OrderOfAdmissionContainsRefOnNotImportedApplication = 3001;

        /// <summary>
        /// В приказ включено заявление c отсутствующей конкурсной группой
        /// </summary>
        public const int OrderOfAdmissionWithAbsentCompetitiveGroup = 3002;

        /// <summary>
        /// В приказе заявление с некорректным направлением
        /// </summary>
        public const int OrderOfAdmissionWithWrongDirection = 3003;

        /// <summary>
        /// В приказе заявление c некорректной формой обучения и источником финансирования
        /// </summary>
        public const int OrderOfAdmissionWithWrongFinSourceAndEduForm = 3004;

        /// <summary>
        /// В приказ включено заявление, у которого результаты экзамена не совпадают с соответствующими результатами в свидетельстве ЕГЭ.
        /// </summary>
        public const int OrderOfAdmissionWithInvalidAppExamData = 3005;

        /// <summary>
        /// В приказ включены дублирующиеся заявления.
        /// </summary>
        public const int OrderOfAdmissionWithDuplicateApp = 3006;

        /// <summary>
        /// В приёмной кампании не разрешены приказы данного типа.
        /// </summary>
        public const int OrderOfAdmissionWithInvalidType = 3007;

        /// <summary>
        /// Заявление, включаемое в приказ, не содержит ни одного подходящего условия приема в указанной конкурсной группе (уточните значения CompetitiveGroupUID, DirectionID, EducationFormID, FinanceSourceID, EducationLevelID)
        /// </summary>
        public const int OrderOfAdmissionNoneApplicationCGI = 3008;

        /// <summary>
        /// Заявление, включаемое в приказ, содержит несколько подходящих условий приема в указанной конкурсной группе. Невозможно определить требуемое условие приема (уточните значения CompetitiveGroupUID, DirectionID, EducationFormID, FinanceSourceID, EducationLevelID)
        /// </summary>
        public const int OrderOfAdmissionToManyApplicationCGI = 3108;

        /// <summary>
        /// Количество заявлений на данное направление превышает разрешённое количество.
        /// </summary>
        public const int OrderOfAdmissionWithIncorrectAppCountForPlaces = 3009;

        /// <summary>
        /// Заявление невозможно включить в льготный приказ.
        /// </summary>
        public const int OrderOfAdmissionBeneficiaryCannotInclude = 3010;

        /// <summary>
        /// Заявление не может быть включено в данный приказ, так как в заявлении не выбрана данная форма обучения.
        /// </summary>
        public const int OrderOfAdmissionWithNonSelectedForm = 3011;

        /// <summary>
        /// Невозможно включить в приказ заявление для приёмной кампании со статусом отличным от "Идет набор".
        /// </summary>
        public const int ApplicationCannotIncludeInOrderForCampaignStatus = 3012;

        /// <summary>
        /// Для заявления необходимо указать этап приемной кампании (1 или 2).
        /// </summary>
        public const int OrderOfAdmissionWithMissingStage = 3013;

        /// <summary>
        /// Этап приемной кампании для данного приказа недопустим.
        /// </summary>
        public const int OrderOfAdmissionWithIncorrectStage = 3014;

        /// <summary>
        /// Зачислять в приказ можно заявления только со статусом "Принято"
        /// </summary>
        public const int ApplicationCannotIncludeInOrderForAppStatus = 3015;

        /// <summary>
        /// В заявлении не указаны необходимые результаты вступительных испытаний. Необходимо добавить результаты вступительных испытаний или исключить группу из заявления.
        /// </summary>
        public const int ApplicationCannotIncludeInOrderMissingEntranceTestResult = 3016;
        /// <summary>
        /// Минимальный балл, указанный для вступительного испытания, не должен быть меньше {0} (минимального количества баллов  по результатам ЕГЭ по предмету).
        /// </summary>
        public const int ApplicationCannotIncludeInOrderLowResults = 3017;

        /// <summary>
        /// В заявлении не указаны конкурсные группы, на которые происходит включение в приказ. Необходимо добавить конкурсные группы в условия приема заявления.
        /// </summary>
	    public const int OrderOfAdmissionMismatchImplicitCompetitiveGroup = 3018;

        /// <summary>
        /// Не обнаружена явно указанная в приказе конкурсная группа (UID приказа = {0}, UID группы = {1})
        /// </summary>
	    public const int OrderOfAdmissionImplicitCompetitiveGroupUidNotFound = 3019;

        /// <summary>
        /// Заявление уже включено в приказ о зачислении
        /// </summary>
        public const int OrderOfAdmissionApplicationAlreadyIn = 3020;

        /// <summary>
        /// У приказа со статусом, отличным от "Нет заявлений", нельзя менять значение поля {0}
        /// </summary>
        public const int OrderOfAdmissionFieldCannotBeChanged = 3021;

        /// <summary>
        /// У приказа о со статусом "Опубликован" должно быть задано поле {0}
        /// </summary>
        public const int OrderOfAdmissionMustHaveValues = 3022;

        /// <summary>
        /// Для приказа с номером {0} должен быть передан UID {1}{2}
        /// </summary>
        public const int OrderOfAdmissionNumberMustBeUnique = 3023;

        /// <summary>
        /// Признак приказа по межправительственным соглашениям не соответствует типу документа, удостоверяющего личность абитуриента
        /// </summary>
        public const int OrderOfAdmissionForeignerWrongDocumentData = 3024;

        /// <summary>
        /// Не представлены оригиналы документов
        /// </summary>
        public const int OrderOfAdmissionNoOriginalDocumentsReceived = 3025;

        /// <summary>
        /// Одно из заявлений данного абитуриента уже включено в приказ о зачислении
        /// </summary>
        public const int OrderOfAdmissionOneOfApplicationsAlreadyInOrder = 3026;

        /// <summary>
        /// Нельзя добавлять заявления в приказ со статусом "Опубликован"
        /// </summary>
        public const int OrderOfAdmissionCantIncludeAppInPublishedOrder = 3027;

        /// <summary>
        /// Заявление не может быть включено в приказ в связи с ошибками при проверке результатов
        /// </summary>
        public const int OrderOfAdmissionCantIncludeAppCauseViolationErrors = 3028;

        /// <summary>
        /// У приказа не могут быть одновременно заданы платный источник финансирования и этап
        /// </summary>
        public const int OrderOfAdmissionFinanceSourcePaidAndStageSpecified = 3029;

        /// <summary>
        /// Нельзя изменить приемную кампанию у приказа
        /// </summary>
        public const int OrderOfAdmissionCampaignCantBeChanged = 3030;

        /// <summary>
        /// Нельзя опубликовать приказ, не содержащий заявлений
        /// </summary>
        public const int OrderOfAdmissionNoAppCantBePublished = 3031;

        /// <summary>
        /// UID Приказа должен быть задан
        /// </summary>
        public const int OrderOfAdmissionNoUID = 3032;


        /// <summary>
        /// Номер приказа {0} не должен встречаться в одном пакете у разных приказов (UID: {1})
        /// </summary>
        public const int OrderOfAdmissionNumberMustBeUniqueInPackage = 3033;

        /// <summary>
        /// Приказ с UID: {0} не найден
        /// </summary>
        public const int OrderOfAdmissionNotFound = 3034;

        /// <summary>
        /// Приказ с UID: {0} уже имеется в Системе с другим типом приказа
        /// </summary>
        public const int OrderOfAdmissionHasSameUIDAndWrongTypeInDB = 3035;
        /// <summary>
        /// Приказ с UID: {0} имеется в пакете с другим типом приказа
        /// </summary>
        public const int OrderOfAdmissionHasSameUIDAndWrongTypeInPackage = 3036;

        /// <summary>
        /// При включении заявления в приказ указан несуществующий конкурсе с UID: {0}
        /// </summary>
        public const int OrderOfAdmissionApplicationCompetitiveGroupNotFound = 3037;

        /// <summary>
        /// Заявление уже включено в приказ о зачислении по данному конкурсу
        /// </summary>
        public const int OrderOfAdmissionAcgiAlreadyIncluded = 3038;

        /// <summary>
        /// Заявление уже включено в 2 или более приказа о зачислении (UID: {0})
        /// </summary>
        public const int OrderOfAdmissionApplicationAlreadyHas2Orders = 3039;
        /// <summary>
        /// Заявление уже включено в приказ о зачислении (UID: {0}) и не исключено из него
        /// </summary>
        public const int OrderOfAdmissionApplicationAlreadyHasOrder = 3040;

        /// <summary>
        /// Заявление не может быть включено в приказ об исключении по данному конкурсу, поскольку оно не включено в приказ о зачислении по нему
        /// </summary>
        public const int OrderOfExceptionAcgiHasNoOOA = 3041;
        /// <summary>
        /// Заявление уже было включено в приказ об исключении по данному конкурсу
        /// </summary>
        public const int OrderOfExceptionAcgiAlreadyHasBeenExcluded = 3042;

        /// <summary>
        /// Заявление уже было включено в приказ(ы) об исключении (UID: {0})
        /// </summary>
        public const int OrderOfExceptionApplicationAlreadyHasExceptionOrder = 3043;

        /// <summary>
        /// При включении в приказ об исключении должна быть указана дата отказа от зачисления
        /// </summary>
        public const int OrderOfExceptionDisagreedDateMustBeSpecified = 3044;

        /// <summary>
        /// Не допускается включать в приказ заявление, если у него есть согласие и отказ по данному конкурсу 
        /// </summary>
        public const int OrderOfAdmissionAcgiHasDisagreedDate = 3045;
        #endregion

        #region Сервис удаления

        /// <summary>
        /// Не найдено заявление.
        /// </summary>
        public const int ApplicationIsNotFound = 4001;

        /// <summary>
        /// Не найдено направление.
        /// </summary>
        public const int CompetitiveGroupItemIsNotFound = 4002;
        /// <summary>
        /// Найдено более одного направления, неовзможно однозначно определить объект для удаления.
        /// </summary>
        public const int CompetitiveGroupItemMoreThenOne = 4102;

        /// <summary>
        /// Не найден результат вступительных испытаний в заявлении (Application\EntranceTestResults\EntranceTestResult\UID).
        /// </summary>
        public const int AppEntranceTestIsNotFound = 4003;

        /// <summary>
        /// Найдено более одного результата вступительных испытаний, невозможно однозначно определить объект для удаления.
        /// </summary>
        public const int AppEntranceTestMoreThenOne = 4103;

        /// <summary>
        /// Не найдена льгота, подтверждающая результат  вступительных испытаний в заявлении (Application\EntranceTestResults\EntranceTestResult\UID).
        /// </summary>
        public const int ApplicationBenefitIsNotFound = 4004;

        /// <summary>
        /// Найдено более одной льготы, подтверждающей результат вступительных испытаний, невозможно однозначно определить объект для удаления.
        /// </summary>
        public const int ApplicationBenefitIsMoreThenOne = 4104;


        /// <summary>
        /// Заявление не найдено или отcутствует в приказе
        /// </summary>
        public const int ApplicationIsNotFoundOrAbsentInOrder = 4005;

        /// <summary>
        /// Заявление включено в приказ. Для удаления заявления необходимо сначала исключить его из приказа.
        /// </summary>
        public const int ApplicationIsInOrder = 4006;

        /// <summary>
        /// Невозможно удалить направление, так как по нему есть заявления в приказе
        /// </summary>
        public const int CantDeleteDirectionLinkedWithAppInOrder = 4007;

        /// <summary>
        /// Невозможно удалить направление, так как это последнее направление в конкурсной группе и существуют заявления на эту группу
        /// </summary>
        public const int CantDeleteLastDirectionInCompetitiveGroupWithApplications = 4008;

        /// <summary>
        /// Невозможно удалить результат ВИ, потому что есть заявления в приказе с данным результатом
        /// </summary>
        public const int CantDeleteResultLinkedWithAppInOrder = 4009;

        /// <summary>
        /// Невозможно удалить общую льготу, потому что есть заявления в приказе с данной льготой
        /// </summary>
        public const int CantDeleteCommonBenefitUsedInApp = 4010;

        /// <summary>
        /// Не найдена приемная кампания.
        /// </summary>
        public const int CampaignIsNotFound = 4011;
        /// <summary>
        /// Найдено более одной кампании, неовзможно однозначно определить объект для удаления.
        /// </summary>
        public const int CampaignMoreThenOne = 4111;

        /// <summary>
        /// Существуют конкурсные группы, привязанные к данной приемной кампании
        /// </summary>
        public const int CampaignHasDependentCompetitiveGroups = 4012;



        /// <summary>
        /// Невозможно удалить направление, так как существуют заявления, поданные на данное направление.
        /// </summary>
        public const int CantDeleteDirectionInCompetitiveGroupWithApplications = 4013;

        /// <summary>
        /// Невозможно удалить группу, так как существуют заявления, относящиеся к данной группе.
        /// </summary>
        public const int CantDeleteCompetitiveGroupWithApplications = 4014;

        /// <summary>
        /// Невозможно удалить группу, так как существуют направления, относящиеся к данной группе.
        /// </summary>
        public const int CantDeleteCompetitiveGroupWithGroupItems = 4015;

        /// <summary>
        /// Группа не найдена
        /// </summary>
        public const int CompetitiveGroupIsNotFound = 4016;

        /// <summary>
        /// Найдено более одной группы, неовзможно однозначно определить объект для удаления.
        /// </summary>
        public const int CompetitiveGroupMoreThenOne = 4116;

        public const int DirectionLevelNotFound = 4017;
        public const int FinFormSourceNotFound = 4018;
        public const int FinFormSourceNotAllowedForConsidered = 4019;
        public const int ApplicationIsNotAcceptedForConsidered = 4020;
        public const int ApplicationIsNotFoundInConsidered = 4021;
        public const int FinFormSourceNotAllowedForRecommended = 4022;
        public const int EntrantRatingIsLessThanMinimalForConsidered = 4023;

        /// <summary>
        /// Кол-во рекомендуемых заявлений больше чем доступное кол-во мест
        /// </summary>
        public const int AdmissionVolumeOverflow = 4024;

        public const int EntranceTestResultsError = 4025;
        public const int DocumentsError = 4026;
        public const int OrderExists = 4027;


        /// <summary>
        /// Существуют индивидуальные достижения, учитываемые образовательной организацией, привязанные к данной приемной кампании
        /// </summary>
        public const int CampaignHasDependentInstitutionArchievements = 4028;

        /// <summary>
        /// Существуют списки рекомендованных к зачислению, привязанные к данной приемной кампании
        /// </summary>
        public const int CampaignHasDependentRecomendedLists = 4029;

        /// <summary>
        /// Существуют приказы о зачислении, привязанные к данной приемной кампании
        /// </summary>
        public const int CampaignHasDependentOrderOfAdmissions = 4031;

        /// <summary>
        /// Не найдены ИД, учитываемые ОО
        /// </summary>
        public const int InstitutionAchievementIsNotFound = 4030;

        /// <summary>
        /// ИД не может быть удалено, поскольку соответствующие индивидуальные достижения имеются в заявлениях с UID: ({0})
        /// </summary>
        public const int InstitutionAchievementCannotBeRemovedWithApplications = 4032;


        /// <summary>
        /// Заявление включено в cписки рекомендованных к зачислению. Для удаления заявления необходимо сначала исключить его из списков.
        /// </summary>
        public const int ApplicationIsInRecList = 4033;

        /// <summary>
        /// Объем приема не может быть удален, так как существуют связанные с ним заявления c UID: ({0})
        /// </summary>
        public const int AdmissionVolumeCannotBeRemovedWithApplications = 4040;

        /// <summary>
        /// В пакете имеются не все данные импорта по кампании с UID: {0}
        /// </summary>
        public const int AdmissionVolumeCannotBeChangedWithCampaign = 4041;

        /// <summary>
        /// В пакете имеются не все данные импорта по кампании с UID: {0}
        /// </summary>
        public const int CompetitiveGroupCannotBeRemovedWithCampaign = 4050;

        /// <summary>
        /// Невозможно импортировать группу, так как в пакет не включеные все ее данные, удаление которых невозмоно, так как с ними связаны заявления с UID: {0}
        /// </summary>
        public const int CantDeleteCompetitiveGroupWithReferencedApplications = 4051;

        /// <summary>
        /// Заявление не может быть исключено из приказа со статусом Опубликован
        /// </summary>
        public const int ApplicationCannotBeExcludedFromOrderStatusPublished = 4052;

        /// <summary>
        /// Приказ не найден
        /// </summary>
        public const int OrderIsNotFound = 4053;

        /// <summary>
        /// Удаление приказа, содержащего заявления, невозможно
        /// </summary>
        public const int OrderHasApplications = 4054;

        /// <summary>
        /// Заявление включено в приказ об исключении по данному конкурсу и не может быть удалено из приказа о зачислении
        /// </summary>
        public const int ApplicationOrderTryToDeleteOOAButHasOOE = 4055;

        /// <summary>
        /// Найдено более одной целевой организации с указанным UID
        /// </summary>
        public const int TargetOrganizationMoreThenOne = 4056;
        /// <summary>
        /// Целевая организация не найдена
        /// </summary>
        public const int TargetOrganizationIsNotFound = 4057;
        /// <summary>
        /// Имеются конкурсы, в которых указана данная целевая организация
        /// </summary>
        public const int CompetitiveGroupTargetHasDependentCompetitiveGroups = 4058;
        /// <summary>
        /// Имеются заявления, в которых указана данная целевая организация
        /// </summary>
        public const int CompetitiveGroupTargetHasDependentApplications = 4059;


        #endregion

        #region Иностранцы
        /// <summary>
        /// Указан некорректный или недопустимый этап приема
        /// </summary>
        public const int ApplicationInvalidStage = 5001;

        /// <summary>
        /// Заявление не может быть включено в льготный приказ
        /// </summary>
	    public const int ApplicationUnableToIncludeInBeneficiaryOrder = 5002;

        /// <summary>
        /// В приказ по межправительственным соглашениям могут быть включены только иностранные граждане, претендующие на бюджетные места
        /// </summary>
	    public const int ApplicationForeignEntrantUnacceptable = 5003;
        #endregion

        /// Для одного абитуриента указаны разные паспортные данные
        public const int TooManyIdentityDocumentsForEntrant = 9001;

        // ВИ состоит одновременно в двух группах
        public const int EntranceTestInTwoGroups = 9002;

        /// <summary>
        /// Для AdmissionVolume заданы DirectionID и ParentDirectionID
        /// </summary>
        public const int AdmissionVolumeContainsDirectionAndParentDirection = 9003;

        /// <summary>
        /// Такой AdmissionVolume уже есть в БД
        /// </summary>
        public const int AdmissionVolumeHasSameUID = 9004;

        /// <summary>
        /// Такой UID у CompetitiveGroup уже есть в БД
        /// </summary>
        public const int CompetitiveGroupHasSameUID = 9005;

        /// <summary>
        /// Неверная контрольная сумма ИНН 
        /// </summary>
        public const int CrcINNIsBroken = 9006;

        /// <summary>
        /// Допускается импортировать заявления со вступительными испытаниями только со статусами "Редактируется", "Новое" или "Не прошедшее проверку"
        /// </summary>
        public const int ApplicationCannotBeImportedExceptNewOrFailed = 9007;


        public static readonly ILog cm_logger = LogManager.GetLogger("ConflictMessages");


        public static string GetMessage(int messageCode)
        {
            switch (messageCode)
            {
                #region Проверки целостности
                case CrcINNIsBroken:
                    return "Введен некорректный ИНН";
                case CompetitiveGroupContainsNotAvailableEducationLevel:
                    return "В конкурсe указан уровень образования, недопустимый в данной приемной кампании";
                case CompetitiveGroupContainsNotAvailableEducationForm:
                    return "В конкурсe указана форму обучения, недопустимая в данной приемной кампании";
                case CompetitiveGroupWithProgramIsNotUniqueInPackage:
                    return "В пакете имеется конкурс с такими же параметрами и такой же программой обучения";
                case CompetitiveGroupWithProgramIsNotUniqueInDb:
                    return "В системе уже имеется конкурс с такими же параметрами и такой же программой обучения";
                //case CompetitiveGroupIsNotUniqueInPackage:
                //    return "В пакете имеется конкурс с такими же параметрами";
                //case CompetitiveGroupIsNotUniqueInDb:
                //    return "В системе уже имеется конкурс с такими же параметрами";


                //case PlacesOnDirectionExceeded:
                //	return "Количество мест по направлению в разрезе источника финансирования и формы обучения превышает контрольную сумму для института";
                //case AdmissionVolumeIsNotSpecifiedForDirectionInCompetitiveGroupItem:
                //    return "Для указанного в конкурсах направления не представлен объем приема";
                //case AdmissionVolumeIsNotSpecifiedForDirectionInCompetitiveGroupTargetItem:
                //    return "Для указанного направления в целевом приеме отсутствует объем приема";
                //case ParentObjectIsNotImported:
                //    return "Родительский объект не проимпортирован";
                //case CompetitiveGroupContainsNotAvailableDirections:
                //    return "В конкурсе имеются направления, у которых не совпадает перечень вступительных испытаний или отсутствуют разрешённые вступительные испытания.";
                case CompetitiveGroupContainsNotAllowedExtraProfileSubjectInEntranceTest:
                    return "В конкурсе недопустимы испытания профильной направленности";
                //case CompetitiveGroupAllowedExtraProfileSubjectEqualToProfileSubject: 
                //	return "В Конкурсной группе испытание профильной направленности должно совпадать с профильным предметом.";
                case FieldLengthExceeded:
                    return "Превышена длина {1} символов для поля {0}";
                case CompetitiveGroupNotAllowedCreativeEntranceTests:
                    return "В конкурсе недопустимы испытания творческой направленности";
                //case CompetitiveGroupContainsDirectionNotDefinedInAllowed: 
                //	return "В Конкурсной группе указано направление, не входящее в список допустимых.";
                //case CompetitiveGroupContainsNotAllowedEntranceTestSubjects:
                //    return "В конкурсе указаны предметы для основных вступительных испытаний, которые не допустимы для направлений";
                case CompetitiveGroupCannotHaveCGIAndTarget:
                    return "В конкурсе нельзя одновременно задавать тэги CompetitiveGroupItem и TargetOrganizations";

                //case CompetitiveGroupHasNotAllowedEntranceTestSubjects: 
                //	return "В конкурсной группе не указаны допустимые предметы.";
                //case CompetitiveGroupHasDirectionOnlyWithTargetPlaces:
                //    return "В конкурсе для направления {0} указаны формы обучения и источники финансирования, отсутствующие в объеме приема";
                case AdmissionVolumeIsNotImportedForDirection:
                    return "Объем приема для данного направления не импортирован.";
                case DependedObjectsExists:
                    return "Имеются связанные данные в целевой системе, которые необходимо удалить для выполнения импорта {0}";
                case CompetitiveGroupNameEsistsInDbWithOtherUID:
                    return "В БД найден конкурс с тем же наименованием ({0}), но другим UID ({1})";
                case AdmissionVolumeHasSameUIDAnotherCampaign:
                    return "У другой ПК имеется объем приема с таким же UID";

                case AdmissionVolumeHasSameUID:
                    return "В БД имеется объем приема с таким же UID, ПК: ({0})";

                //case CompetitiveGroupMustHaveRussianLanguangeEntranceTest: 
                //	return "В конкурсной группе не найдено вступительного испытания с обязательным предметом \"Русский язык\".";
                //case CompetitiveGroupMustHaveOneProfileEntranceTest:
                //	return "В конкурсной группе должно присутствовать одно профильное испытание.";
                case CompetitiveGroupNameMustBeUnique:
                    return "Название конкурса ({0}) должно быть уникальным в рамках приёмной кампании";
                case UIDMustBeUniqueInCollection:
                    return "UID для объектов одного типа в рамках одной коллекции должны быть уникальны.";
                case UIDMustBeUniqueForAllObjectInstancesOfType:
                    return "UID ({1}) должно быть уникальным в разрезе всех импортируемых коллекций объектов данного типа ({0}).";
                case UIDMustBeUniqueForChildrenObjects:
                    return "Объект содержит коллекцию элементов ({0}), у которых есть дубликаты по UID ({1}).";
                case NameMustBeUniqueInCollection:
                    return "Наименования объектов одного типа в рамках одной коллекции должны быть уникальны.";
                case NameMustBeUniqueForAllObjectInstancesOfType:
                    return "Наименование ({1}) должно быть уникальным в разрезе всех импортируемых коллекций объектов данного типа ({0}).";

                //case RegionIsNotFounded: 
                //	return "Регион отсутствует для указанного идентификатора.";
                //case CountryIsNotFounded: 
                //	return "Страна отсутствует для указанного идентификатора.";
                //case RegionIsNotFoundedForCountry: 
                //	return "Страна не содержит указанный регион.";
                case ApplicationNumberIsNotUnique:
                    return "Номер заявления не уникален.";
                //case CompetitiveGroupWithSameNameExists:
                //    return "В БД существует конкурс с таким же именем, которая на данный момент не может быть удалена";
                case SubjectIsNotFounded:
                    return "Предмет не найден для кода {0}.";
                case ResultValueRange:
                    return "Результат испытания (значение {0}) должен принимать значения от 0 до {1}";
                //case CompetitiveGroupItemOnSameDirectionExists:
                //    return "В системе уже существует данное направление у конкурса";
                //case EntranceTestSourceIDNotFound:
                //    return "Источник вступительного испытания код {0} не найден";
                //case CompetitiveGroupEntranceTestItemUniqueConstraint:
                //    return "В конкурсе уже есть вступительное испытание {0} с такими же данными";
                //case CompetitiveGroupNotFoundForApplication:
                //    return "Не найден конкурс для заявления";
                //case CompetitiveGroupNotSpecifiedForApplication:
                //    return "Конкурс не указана для заявления";
                case NotAllowedCommonBenefitTypeForApplication:
                    return "Недопустимый тип общей льготы: возможные значения \"Без вступ. испытаний\", \"Вне Конкурса\", \"Преимущественное право на поступление\"";
                //case DocumentNotSpecifiedForBenefit:
                //    return "Не указан документ основания для льготы РВИ";
                //case CompetitiveGroupCantHaveMoreThanOneExtraProfileSubject:
                //	return "В конкурсной группе не может быть более одного испытания профильной направленности";
                case DictionaryItemAbsent:
                    return "У свойства {0} объекта указано некорректное справочное значение";
                case AdmissionVolumeContainsNotAllowedDirections:
                    return "Направление указанное для объема приема не разрешено для института";
                //case DirectionIDDuplicatedInCompetitiveGroup:
                //    return "В конкурсе более одного раза указано направление в перечне направлений";
                case AdmissionVolumeNonUniqueDirections:
                    return "У объема приема дублирующиеся направления";
                case CompetitiveGroupPlacesOnDirectionExceeded:
                    return "Количество мест ({4}) по направлению ({0}) в разрезе источника финансирования ({1}) и формы обучения ({2}) {3}";

                case AdmissionVolumeContainsDirectionAndParentDirection:
                    return "У объема приема одновременно задано направление и укрупненная группа специальностей";
                //case CompetitiveGroupCannotBeRemoved:
                //    return "Конкурс не может быть удален.";
                //case CompetitiveGroupContainsNotAllowedCommonBenefit:
                //    return "В конкурсе имеется неразрешённая общая льгота.";
                case DictionaryItemNonUnique:
                    return "У свойства {0} объекта указаны повторяющиеся справочное значение.";
                case BenefitContainsOlympicsAndAllOlympics:
                    return "Льгота содержит указание на все олимпиады и список олимпиад.";
                case BenefitContainsNoOlympicsAndNoAllOlympics:
                    return "Льгота не содержит указания на все олимпиады и сами олимпиады.";
                case EntranceTestContainsScoreLowerThanRequired:
                    return "Вступительное испытание имеет балл ниже, чем минимальный, указанный для данного предмета.";
                case CompetitiveGroupContainsNotAllowedByInstituteDirections:
                    return "В конкурсе имеются направления не разрешенные для института.";
                case CompetitiveGroupContainsNotAllowedBenefitType:
                    return "У вступительного испытания указан некорректный данный тип льготы.";
                //case EntranceTestBenefitItemNotExistsInPackage:
                //    return "Вступительное испытание содержит льготу, отсутствующую в пакете импорта.";
                //case ApplicationLastDenyDateShouldBeGreaterRegistrationDate:
                //    return "Дата отзыва заявления должна быть позднее даты регистрации.";
                //case ApplicationContainsNotAllowedFinSourceEducationForm:
                //    return "Заявление содержит неразрешенные для конкурса форму обучения и источник финансирования.";
                case ApplicationContainsInvalidCompetitiveGroupID:
                    return "В заявлении указан несуществующий идентификатор конкурса. UID: {0}.";
                case InvalidDocumentSpecifiedForBenefit:
                    return "В качестве основания для льготы указан некорректный документ.";
                case ApplicationContainsIncorrectTargetOrganizationUID:
                    return "Заявление содержит некорректный UID организации целевого приема.";
                case ApplicationNumberIsNotCorrelateWithUID:
                    return "В БД существует заявление с тем же номером но другим UID'ом.";
                //case ApplicationFinSourceCGIIsNotInCG:
                //    return "Указанное в формах обучения и источниках финансирования направление (UID: {0}) не содержится в конкурсе (UID: {1})";

                case CampaignWithSameTypeAndYearExistsDb:
                    return "В БД существует приемная кампания с таким же типом и годом начала.";
                case CampaignWithSameTypeAndYearExistsPackage:
                    return "В пакете уже имеется приемная кампания с таким же типом и годом начала.";
                case CampaignWithSameNameExists:
                    return "В БД существует приемная кампания с таким же именем.";
                case CampaignWithInvalidData:
                    return "Приемная кампания содержит некорректные данные.";
                //case CampaignWithInvalidDate:
                //    return "Приемная кампания содержит некорректные сроки проведения.";
                case AdmissionVolumeContainsNotAllowedCampaign:
                    return "Объем приема указывает на отсутствующую кампанию.";
                case AdmissionVolumeContainsNotAllowedCampaignEducationLevel:
                    return "Объем приема указывает на уровень образования, отсутствующий у приемной кампании.";
                case AdmissionVolumeContainsNotAllowedCampaignEducationForm:
                    return "Объем приема указывает на форму обучения, отсутствующую у приемной кампании.";
                //case CampaignStageAndAdditionalExists:
                //    return "У приемной кампании для дополнительного набора не должно быть стадий";
                case CompetitiveGroupContainsNotAllowedCampaign:
                    return "Конкурс указывает на отсутствующую кампанию.";
                //case CompetitiveGroupContainsNotAllowedCampaignCourse:
                //    return "Конкурс указывает на неверный курс у приемной кампании.";
                //case CampaignWithInvalidEducationLevelsCombination:
                //    return "Приемная кампания содержит недопустимую комбинацию уровней образования и курсов ({0}).";
                case IdentityDocumentContainsInvalidNationality:
                    return "Для указанного типа документа должно быть российское гражданство.";
                //case DictionaryItemAbsentForApp:
                //    return "У свойства {0} документа с UID: {1} указано некорректное справочное значение.";
                //case CompetitiveGroupItemNotFoundForApplication:
                //    return "Не найден элемент конкурса для заявления.";
                //case CompetitiveGroupItemNotSpecifiedForApplication:
                //    return "Элемент конкурса не указан для заявления.";
                //case ApplicationContainsInvalidCompetitiveGroupItemID:
                //    return "В заявлении указан несуществующий идентификатор элемента конкурса. UID: {0}";
                //case CommonBenefitContainsInvalidCompetitiveGroup:
                //    return "Общая льгота указывает на некорректный конкурс.";
                case NotAllowedSubjectForEgeDocument:
                    return "Для данного предмета не разрешено указывать Свидетельство ЕГЭ в качестве основания для оценки.";
                //case CampaignDateWithInvalidStage:
                //    return "Для данной формы обучения и источника финансирования указан некорректный или недопустимый этап приёма.";
                //case CampaignDateWithMissingStage:
                //    return "В датах приемной кампании отсутствует необходимый этап приема.";
                case EntrantUIDIsMissing:
                    return "У абитуриента отсутствует UID.";
                //case AdmissionVolumeContainsNotAllowedNumbers:
                //    //return "Контрольные цифры элемента объема приема по одной или нескольким формам обучения или источникам финансирования не разрешены в рамках указанной приемной кампании.";
                //    return "Не заполнены сроки проведения по одной или нескольким формам обучения или источникам финансирования в рамках указанной приемной кампании";
                case CampaignWithInvalidEducationLevelsByInstitution:
                    return "Приемная кампания содержит уровни образования, которые неразрешены для данного ОУ ({0}).";
                case InvalidDocumentTypeRelatedToExisting:
                    return "Документ с данным UID'ом ({0}) уже существует в базе или пакете, при этом имеет другой тип.";
                //case ApplicationInOrderCannotBeImported:
                //    return "Невозможно импортировать заявление со статусом \"В приказе\", следует изменить статус и отдельно передать список приказов.";
                case EgeDocumentYearDateInvalid:
                    return "В свидетельстве ЕГЭ одновременно указан год и дата свидетельства, при этом они не совпадают друг с другом.";
                case ApplicationDoesNotContainRequiredEduDocument:
                    return "Заявление должно содержать один из следующих документов об образовании: {0}.";
                case ApplicationUIDIsNotCorrelateWithNumber:
                    return "В БД существует заявление с тем же UID'ом но другим номером.";
                case ApplicationUIDNumberIsNotCorrelateWithDate:
                    return "В БД существует заявление с тем же UID'ом и номером, но с другой датой регистрации.";
                case ApplicationCannotBeImportedExceptAcceptedOrNewOrDenied:
                    return "Допускается импортировать заявления только со статусами \"Новое\", \"Принято\" или \"Отозвано\".";
                //case TargetOrganizationNameIsNotUnique:
                //    return "Уже существует организация целевого приема с данным UID'ом, но другим наименованием.";
                case IdentityDocumentRequireSeries:
                    return "Для данного типа документа, удостоверяющего личность, необходимо указать серию.";
                case ApplicationCannotImportedForCampaignStatus:
                    return "Невозможно импортировать заявление для приёмной кампании со статусом отличным от \"Идет набор\".";
                //case OlympicDocumentCannotBeUsedAsSource:
                //    return "Данный диплом нельзя использовать в качестве источника для результата вступительного испытания.";
                case ApplicationCannotImportedForCGWithoutEntranceTests:
                    return "Невозможно импортировать заявление, содержащее конкурс без вступительных испытаний.";
                case NotAllowedSubjectForGiaDocument:
                    return "Для данного предмета не разрешено указывать Справку ГИА в качестве основания для оценки.";
                case TargetOrganizationSameNameDifferentUID:
                    return "Уже существует организация целевого приема с данным наименованием, но другим UID'ом.";
                case EgeDocumentDateMissing:
                    return "В свидетельстве ЕГЭ необходимо передать год или дату.";
                //case CampaignDateDuplicateData:
                //    return "В датах приемной кампании дублируются данные.";
                case CompetitiveGroupTargeExistsInDb:
                    return "В данном конкурсе ({2}) уже существует целевой набор, UID которого ({0}) отличается от UID'а, передаваемого в пакете ({1})";
                case StudentDocumentRequired:
                    return "В приказ можно включить только заявления, для которых предоставлены оригиналы документов или справка об обучении в другом ВУЗе";
                case OriginalDocumentsRequired:
                    return "В приказ на бюджетные можно включить только заявления, для которых предоставлены оригиналы документов об образовании";
                case DirectionLevelNotFound:
                    return "Не найдена привязка заявления к указанному направлению {0} или уровню образования {1}";
                case FinFormSourceNotFound:
                    return "Заявление c некорректной формой обучения {0} или источником финансирования {1}";
                case CompetitiveGroupCannotBeRemovedWithApplications:
                    return "Конкурс не может быть удален, так как существуют связанные с ним заявления UID ({0})";
                //case CompetitiveGroupTargetExistsInDbInOtherCompetitiveGroup:
                //    return "В БД найдена организация целевого приема с UID ({0}), принадлежащая другому конкурсу";
                //case CompetitiveGroupTargetItemExistsInDbInOtherCompetitiveGroupTarget:
                //    return "В БД найдены места для целевого приема с UID ({0}), принадлежащие другой организации целевого приема UID ({1})";
                //case CompetitiveGroupItemExistsInDbInOtherCompetitiveGroup:
                //    return "В БД найдено направление конкурса с UID ({0}), принадлежащее другому конкурсу UID ({1})";
                case EntranceTestItemCExistsInDbInOtherCompetitiveGroup:
                    return "В БД найдены вступительные испытания конкурса с UID ({0}), принадлежащие другому конкурсу UID ({1})";
                case BenefitItemCExistsInDbInOtherEntranceTestItemC:
                    return "В БД найдена льгота с UID ({0}), принадлежащая другим вступительным испытаниям конкурса UID ({1})";
                case BenefitItemCExistsInDbInOtherCompetitiveGroup:
                    return "В БД найдена льгота с UID ({0}), принадлежащая другому конкурсу UID ({1})";
                //case BenefitItemCNotAllowedInCompetitiveGroup:
                //    return "Заявление ({0}) содержит льготу ({1}), уровень которой ниже разрешенного в конкурсе ({2})";
                case BenefitMustHaveUID:
                    return "У льготы должен быть задан UID";
                //case BenefitItemCAllowedToWinnerOnly:
                //    return "Заявление ({0}) содержит льготу ({1}) для призера, в конкурсе ({2}) разрешена льгота только для победителя";
                //case DocumentsFailRecievingCheck:
                //    return "Оригиналы документов предоставлены, но не указана дата или указана дата предоставления для документа без оригиналов.";
                //case BenefitNotContainsWinnerBenefitFlag:
                //    return "В случае предоставления льготы призерам олимпиады должна быть предоставлена льгота того же или более высокого порядка также и победителям олимпиады";
                //case OlympicTypeIsNotAllowedInCompetitiveGroup:
                //    return "Олимпиада ({0}) не разрешена в конкурсе ({1})";
                //case OlympicLevelIsNotAllowedInCompetitiveGroup:
                //    return "Заявление ({0}) содержит льготу ({1}) с неопределенным уровнем для конкурса ({2}). В конкурсе ({2}) для олимпиады ({3}) разрешены уровни: ({4})";
                case EntranceTestIsNotPartOfCompetitiveGroup:
                    return "Вступительное испытание (UID:{0}) относится к конкурсу, не указанному в данном заявлении";
                case EntranceTestPriorityIncorrect:
                    return "Для вступительного испытания {0} указан некорректный приоритет";
                case CompetitiveGroupItemQuotaIncorrect:
                    return "Квота приёма лиц, имеющих особое право, может быть указана только для бакалавриата или специалитета";
                //case IndividualAchivementNameIsEmpty:
                //    return "Для индивидуального достижения не задано название - обязательный атрибут";
                case IndividualAchivementDocumentUIDIsEmpty:
                    return "Для индивидуального достижения не задана ссылка на подтверждающий документ";
                case IndividualAchivementDocumentNotFound:
                    return "Для индивидуального достижения не найден подтверждающий документ с типом \"иные документы\" (CustomDocuments)";
                //case IncorrectStageInRecommendedList:
                //    return "В списке лиц, рекомендованных к зачислению, указан этап приёма {0}. Должен быть 1 или 2";
                //case IncorrectApplicationForRecommendedList:
                //    return "Не найдено заявление № {0} для включения в список рекомендованых к зачислению";
                //case IncorrectGroupFormSource:
                //    return "Заявление № {0}, указанное для включения в список рекомендованных к зачислению, не содержит заданной комбинации конкурса, формы обучения и источника финансирования - бюджетные места";
                //case IncorrectLevelDirection:
                //    return "Заявление № {0}, указанное для включения в список рекомендованных не содержит указанного направления подготовки и/или уровня образования";
                //case IncorrectRating:
                //    return "Заявление № {0}, указанное для включения в список рекомендованных не содержит одного или нескольких результатов вступительных испытаний. Заявление без результатов вступительных испытаний не может быть включено в список рекомендованных";
                case AdmissionVolumeUIDAndBudgetLevelMustBeUniqueInCollection:
                    return "У распределенного объема приема дублируются AdmissionVolumeUID: {0}, LevelBudget: {1}, IsPlan: {2}";
                case AdmissionVolumeIsNotImportedForDistributedAdmissionVolume:
                    return "Для распределенного объема приема не импортирован объем приема UID: {0}, IsPlan {1}";
                //case DistributedAdmissionVolumeInvalidNumbers:
                //    return "Распределенный объем приема  UID: {0} содержит некорректные числовые значения";
                case DistributedAdmissionVolumeNumbersSumBiggerAV:
                    return "Сумма распределенных объемов приема превышает исходный объем приема ({1}) AdmissionVolumeUID: {0}, IsPlan: {2}";
                case IAUIDAndCampaignUIDMustBeUniqueInCollection:
                    return "У индивидуальных достижений, учитываемых образовательной организацией, дублируются IAUID: {0} и CampaignUID: {1}";
                //case CampaignUIDMustBeUniqueInCollection:
                //    return "У индивидуальных достижений, учитываемых образовательной организацией, дублируются CampaignUID: {0}";
                case IAIncorrentFieldValue:
                    return "Недопустимое значение в поле: {0}";
                //case CompetitiveGroupTargetItemHasNotCompetitiveGroupItem:
                //    return "Для места целевого приема с UID ({0}) не найдено соответствующего элемента конкурса";
                //case CompetitiveGroupTargetItemRefersCompetitiveGroupItemAlreadyInUse:
                //    return "";
                case ApplicationsInRecListIsNotAllowedToUpdate:
                    return "Заявление ({0}) включено в списки рекомендованных. Невозможно обновить заявление в списках";
                case ApplicationCannotImportedForDifferentCampaigns:
                    return "Невозможно импортировать заявление для нескольких приёмной кампаний";

                //case ApplicationContainsCompetitiveGroupItemWithInvalidCGID:
                //    return "В заявлении указан элемент конкурса (UID: {0}), ссылающийся на несуществующий идентификатор конкурса";

                case ApplicationContainsCompetitiveGroupNotSpecified:
                    return "В формах обучения и источниках финансирования указан несуществующий конкурс (UID: {0})";

                //case ApplicationContainsCompetitiveGroupItemNotSpecified:
                //    return "В формах обучения и источниках финансирования указан элемент конкурса (UID: {0}), который не указан в перечне элементов конкурсов заявления";

                case ApplicationBenefitsContainsCompetitiveGroupNotSpecified:
                    return "В перечне льгот указан конкурс (UID: {0}), который не указан в перечне конкурсов заявления";
                case IndividualAchivementUIDIsEmpty:
                    return "Для индивидуального достижения не задан идентификатор - обязательный атрибут";
                case IndividualAchivementInstitutionAchievementUIDNotExists:
                    return "У индивидуального достижения (IAUID: {0}) указан несуществующий идентификатор достижения приемной кампании";
                case IndividualAchivementInstitutionAchievementUIDWrongCampaign:
                    return "У индивидуального достижения (IAUID: {0}) указана приемная кампания, отличная от приемной кампании заявления";
                case IndividualAchivementMarkBiggerMaxValue:
                    return "У индивидуального достижения (IAUID: {0}) балл за достижение ({1}) превышает максимально возможный ({2})";
                case DublicateSubjects:
                    return "Идентификаторы предметов дублируются";
                case IndividualAchivementUIDIsNotUniqueInDb:
                    return "Идентификатор индивидуального достижения (IAUID: {0}) должен быть уникален в рамках ОО";
                case IndividualAchivementUIDIsNotUniqueInPackage:
                    return "Идентификатор индивидуального достижения (IAUID: {0}) должен быть уникален в пакете";
                case InstitutionAchievementUIDNotSpecified:
                    return "Для индивидуального достижения не задан идентификатор ИД ОО";

                case BenefitForAllOlympicMustContainLevel:
                    return "Льгота для всех олимпиад должна содержать уровни олимпиад";
                case BenefitForAllOlympicMustContainClass:
                    return "Льгота для всех олимпиад должна содержать классы олимпиад";
                case BenefitForAllOlympicMustContainProfile:
                    return "Льгота для всех олимпиад должна содержать профили олимпиад";
                //case BenefitOlympicsMustBeTheSameYear:
                //    return "Все передаваемые в льготе олимпиады должны быть одного года";

                case OlympicProfileFromOtherOlympic:
                    return "Передаваемый профиль олимпиады ProfileID={0} не соответствует олимпиаде OlympicID={1}";
                case OlympicProfileHasOtherLevel:
                    return "Передаваемый профиль олимпиады ProfileID={0} не соответствует передаваемому уровню LevelID={1}";

                case EntrantDocumentFromKrymUIDNotFound:
                    return "Документ гражданина Крыма с UID {0} не найден ни в пакете, ни в базе данных";

                case ApplicationStatus1Or8CannotBeImported:
                    return "Не допускается импортировать заявления, которые уже имеются в системе со статусами \"Редактируется\" или \"В приказе\"";
                //case ApplicationStatus4Or6CannotBeImportedStatus2:
                //    return "Не допускается импортировать заявления, которые уже имеются в системе со статусами \"Принято\" или \"Отозвано\", изменяя статус на \"Новое\"";

                case EntrantDocumentVIisDisabledUIDNotFound:
                    return "Документ для ВИ с созданием специальных условий с UID {0} не найден ни в пакете, ни в базе данных";
                case ApplicationStatus2ContainsAgreedDate:
                    return "Заявление со статусом \"Новое\" не может содержать FinSourceEduForm.IsAgreedDate";
                case ApplicationContainsMoreThenOneAgreedDate:
                    return "Абитуриент не может иметь более 1 согласия на зачисление при отстутствии отказа или более 2 согласий при наличии отказов по уровню обучения: бакалавриат, специалитет, форме: очная, очно-заочная";
                case CompetitiveGroupMustHaveEntranceTestWithSPO:
                    return "У конкурса нет ВИ с IsForSPOandVO = 1";
                case ApplicationBenefitsContainsBenefit4InCGNotSource20:
                    return "Льгота типа 4 должна быть в конкурсе с источником \"Квота\" (SourceID = 20)";

                case IdentityDocumentHasOtherValues:
                    return "Фамилия, Имя, Отчество и пол абитуриента должны совпадать с данными в Документах, удостоверяющих личность";
                case IdentityDocumentHasOtherBirthDate:
                    return "Дата рождения абитуриента должна совпадать с данными в Документах, удостоверяющих личность";


                case BenefitOlympicHasError:
                    return "Данные по олимпиаде, в результатах ВИ UID={0} не соответствуют льготам конкурса: {1}";

                case EntrantMustHaveEmailOrAddress:
                    return "У абитуриента должен быть указан email и/или почтовый адрес";
                case EntrantMailAddressMustHaveAddress:
                    return "В адресе абитуриента должно быть заполнено поле Address";

                case CommonBenefitMustHaveSubjectOrCheckbox:
                    return "Общая льгота с UID = {0} должна либо содержать минимальный балл по предмету, либо иметь признак олимпиады творческой или в области спорта";

                case CompetitiveGroupContainsNumberTargetXTwice:
                    return "Нельзя одновременно указывать места на каждую целевую организацию и на весь конкурс";

                case EntrantDocumentOlympicCannotBeUserTwiceInApplicaiton:
                    return "Диплом победителя или призера олимпиады с UID = {0} не может быть использован дважды в одном заявлении в качестве льготы ";

                case ApplicationETResultHasWrongUID:
                    return "Льгота с UID = {0} указана в конкурсе с UID = {1}, однако конкурс не содержит такой льготы";

                case OlympicDocumentSpecifiedSubjectMustBeEge:
                    return "Eсли в OlympicDocument.OlympicSubjectID передан предмет, не являющийся общеобразовательным, то должен быть заполнен EgeSubjectID";
                case OlympicDocumentOlympicSubjectMustMatchProfile:
                    return "OlympicDocument.OlympicSubjectID - должен соответствовать профилю олимпиады";

                case ApplicationHasAgreedDateButNoDocumentsWithReceicedDate:
                    return "Допускается проставлять согласие только если получены оригиналы документов / заявление с обязательством предоставления оригинала в течение первого учебного года";
                //case IndividualAchivementSumMarksMoreThen10:
                //    return "Сумма баллов индивидуальных достижений по одному заявлению не должна превышать 10";
                case CompetitiveGroupIsFromKrymWrongYear:
                    return "Для конкурсов позднее 2016 года запрещено передавать признак IsForKrym";
                case MinValueRange:
                    return "Минимальный балл (значение {0}) должен принимать значения от 0 до {1}";
                case OlympicYearExpired:
                    return "В качестве льгот выбраны олимпиады ({0}), срок действия которых истек";
                case ApplicationContainsAgreedDateWithoutDisagreed:
                    return "Абитуриент не может иметь отказ от зачисления при отстутствии согласия по тому же условию приема";
                case ApplicationNeedReturnDocumentsInfo:
                    return "При импорте заявления со статусом \"Отозвано\"(6) необходимо передать поле ReturnDocumentsDate";
                case InstitutionProgramSameNameAndCodeDifferentUID:
                    return "Совокупность значений в полях \"Код ОП\" и \"Наименование ОП\" должна быть уникальной среди всех образовательных программ ОО";

                #endregion

                #region РВИ

                case EntranceTestResultsExistsForApplication:
                    return "Изменен конкурс у заявления, у которого имеются результаты вступительных испытаний";
                case EntranceTestResultsChangedForApplicationInOrder:
                    return "Отредактированы результаты ВИ. Связанные данные: приказ";
                case EntranceTestResultChanged:
                    return "Изменился РВИ";
                case EntranceTestResultEgeDocumentChanged:
                    return "Изменилось свидетельство ЕГЭ для РВИ";
                case EntranceTestResultOlympicDocumentChanged:
                    return "Изменился документ для олимпиады для РВИ.";
                case EntranceTestResultExistsInDbAndNotSpecifiedInImport:
                    return "В БД указан РВИ, который отсутствует в данных импорта";
                case ReferencedEgeDocumentIsAbsent:
                    return "В списке документов заявления не найдено указанного ЕГЭ документа {0} для вступительного испытания {1}.";
                case ApplicationCommonBenefitDocumentChanged:
                    return "Изменился документ-основание для общей льготы";
                case EntranceTestResultAlreadyImportedForEntranceTestItem:
                    return "Для вступительного испытания {0} уже проимпортирован результат {1}. ";
                case EntranceTestInTwoGroups:
                    return "Вступительное испытание {0} состоит одновременно в двух группах. ";
                case DocumentNotAttachedToEntranceTestResult:
                    return "Не приложен документ - основание для оценки.";
                case SubjectsInEgeDocumentIsDupllicated:
                    return "Повторяются предметы в свидетельстве ЕГЭ.";
                case CompetitiveGroupEntranceTestDuplicateInPackage:
                    return "Вступительное испытание указано более одного раза.";
                case EntranceTestTypeIDNotFound:
                    return "Тип вступительного испытания {0} не найден.";
                case SubjectsInGiaDocumentIsDupllicated:
                    return "Повторяются предметы в справке ГИА.";
                case NotAllowedEgeDocumentForEntranceTestResult:
                    return "Приложено свидетельство ЕГЭ к результату ВИ, не позволяющего использовать свидетельство ЕГЭ в качестве основания для оценки.";
                case ApplicationsInOrderIsNotAllowedToUpdate:
                    return "Заявление ({0}) включено в приказ. Невозможно обновить заявление в приказе.";
                //case CompetitiveGroupCannotContainVariousEdlevels:
                //    return "Прием в высшие учебные заведения должен осуществляться отдельно по программам бакалавриата, программам подготовки специалиста, программам магистратуры и программам СПО.";
                case CompetitiveGroupKVKMustHaveOneEntranceTestPriority:
                    return "Для вступительных испытаний в конкурсах по кадрам высшей квалификации должен быть задан приоритет (не 0) хотя бы у одного испытания";
                case NoMinSystemEGE:
                    return "Для указанного года олимпиады не задан общий минимальный балл ЕГЭ";
                case NoMinEgeForOlympics:
                    return "Для олимпиад 2014 года и более поздних должен быть указан минимальный балл ЕГЭ";
                case BenefitEGELessSystemMinEGE:
                    return "Минимальный балл ЕГЭ для использования льготы ({0}) не может быть меньше общесистемного минимального балла ЕГЭ ({1})";
                case NoMinEGEMarks:
                    return "Для олимпиад 2014 года и более поздних должен быть задан хотя бы один предмет, минимально необходимый балл по которому даст право на использование льготы";
                case CgHasIncorrectBenefits:
                    return "Один или несколько конкурсов содержат льготы с некорректными минимальными баллаим ЕГЭ";
                case SubjectHasIncorrectValue:
                    return "Баллы по предмету должны быть от 1 до 999. Указано значение {0}";
                case EntranceTestCannotChangeSubject:
                    return "Изменение предмета ВИ невозможно, поскольку имеются заявления с документами по ВИ c UID = {0}.";
                case EntranceTestDublicateTypeAndSubject:
                    return "В конкурсе уже имеется ВИ с таким же типом {0} и предметом {1}";
                case EntranceTestReplacedItemNotFound:
                    return "Не найдено заменяемое ВИ с UID = {0}";
                #endregion

                #region Приказ
                case OrderOfAdmissionApplicationInImportedApplications:
                    return "Не допускается в одном пакете осуществлять импорт заявления (UID: {0}) и его включение в приказ (UID: {1})";
                case OrderOfAdmissionContainsRefOnNotImportedApplication:
                    return "Заявление, включённое в приказ не найдено";
                case OrderOfAdmissionWithAbsentCompetitiveGroup:
                    return "В приказ включено заявление c отсутствующим конкурсом";
                case OrderOfAdmissionWithWrongDirection:
                    return "В приказ включено заявление c некорректным направлением";
                case OrderOfAdmissionWithWrongFinSourceAndEduForm:
                    return "В приказе заявление c некорректной формой обучения и источником финансирования";
                case CompetitiveGroupEntranceTestNotFoundForEntranceTestResult:
                    return "Не найдено соответствующего вступительного испытания";
                case OrderOfAdmissionWithInvalidAppExamData:
                    return "В приказ включено заявление, у которого результаты экзамена не совпадают с соответствующими результатами в свидетельстве ЕГЭ";
                case OrderOfAdmissionWithDuplicateApp:
                    return "В приказ включены дублирующиеся заявления";
                case OrderOfAdmissionWithInvalidType:
                    return "В приёмной кампании не разрешены приказы данного типа";

                //case OrderOfAdmissionNotOneApplicationCGI:
                //    return "Невозможно однозначно определить конкурсную группу для заявления, включенного в приказ. Укажите значения CompetitiveGroupUID, DirectionID, EducationFormID, FinanceSourceID, EducationLevelID";
                //case OrderOfAdmissionWithDuplicateType:
                //	return "Заявления включены в приказ с повторяющимся типом.";
                case OrderOfAdmissionNoneApplicationCGI:
                    return "Заявление, включаемое в приказ, не содержит ни одного подходящего условия приема в указанном конкурсе (уточните значения CompetitiveGroupUID, DirectionID, EducationFormID, FinanceSourceID, EducationLevelID)";
                case OrderOfAdmissionToManyApplicationCGI:
                    return "Заявление, включаемое в приказ, содержит несколько подходящих условий приема в указанном конкурсе. Невозможно определить требуемое условие приема (уточните значения CompetitiveGroupUID, DirectionID, EducationFormID, FinanceSourceID, EducationLevelID)";

                case OrderOfAdmissionWithIncorrectAppCountForPlaces:
                    return "Количество заявлений на данное направление превышает разрешённое количество.";
                case OrderOfAdmissionBeneficiaryCannotInclude:
                    return "Заявление невозможно включить в льготный приказ.";
                case OrderOfAdmissionWithNonSelectedForm:
                    return "Заявление не может быть включено в данный приказ, так как в заявлении не выбрана данная форма обучения.";
                case ApplicationCannotIncludeInOrderForCampaignStatus:
                    return "Невозможно включить в приказ заявление для приёмной кампании со статусом отличным от \"Идет набор\".";
                case OrderOfAdmissionWithMissingStage:
                    return "Для заявления необходимо указать этап приемной кампании (1 или 2).";
                case OrderOfAdmissionWithIncorrectStage:
                    return "Этап приемной кампании для данного приказа недопустим.";
                case ApplicationCannotIncludeInOrderForAppStatus:
                    return "Зачислять в приказ можно заявления только со статусом \"Принято\"";
                case ApplicationCannotIncludeInOrderMissingEntranceTestResult:
                    return "В заявлении не указаны необходимые результаты вступительных испытаний. Необходимо добавить результаты вступительных испытаний или исключить группу из заявления.";
                case ApplicationCannotIncludeInOrderLowResults:
                    return "Минимальный балл, указанный для вступительного испытания, не должен быть меньше {0} (минимального количества баллов  по результатам ЕГЭ по предмету).";
                case OrderOfAdmissionMismatchImplicitCompetitiveGroup:
                    return "В заявлении не указаны конкурсы, на которые происходит включение в приказ. Необходимо добавить конкурсы в условия приема заявления.";
                case OrderOfAdmissionImplicitCompetitiveGroupUidNotFound:
                    return "Не обнаружен явно указанный в приказе конкурс (UID приказа = {0}, UID группы = {1})";
                case OrderOfAdmissionApplicationAlreadyIn:
                    return "Заявление уже включено в приказ о зачислении";
                case OrderOfAdmissionFieldCannotBeChanged:
                    return "У приказа со статусом, отличным от \"Нет заявлений\", нельзя менять значение поля {0}";
                case OrderOfAdmissionMustHaveValues:
                    return "У приказа о со статусом \"Опубликован\" должно быть задано поле {0}";
                case OrderOfAdmissionNumberMustBeUnique:
                    return "Для приказа с номером {0} должен быть передан UID {1}{2}";
                case OrderOfAdmissionForeignerWrongDocumentData:
                    return "Признак приказа по межправительственным соглашениям не соответствует типу документа, удостоверяющего личность абитуриента";
                case OrderOfAdmissionNoOriginalDocumentsReceived:
                    return "Не представлены оригиналы документов";
                case OrderOfAdmissionOneOfApplicationsAlreadyInOrder:
                    return "Одно из заявлений данного абитуриента уже включено в приказ о зачислении";
                case OrderOfAdmissionCantIncludeAppInPublishedOrder:
                    return "Нельзя добавлять заявления в приказ со статусом \"Опубликован\"";
                case OrderOfAdmissionCantIncludeAppCauseViolationErrors:
                    return "Заявление не может быть включено в приказ в связи с ошибками при проверке результатов";
                case OrderOfAdmissionFinanceSourcePaidAndStageSpecified:
                    return "У приказа не могут быть одновременно заданы платный источник финансирования и этап";
                case OrderOfAdmissionCampaignCantBeChanged:
                    return "Нельзя изменить приемную кампанию у приказа";
                case OrderOfAdmissionNoAppCantBePublished:
                    return "Нельзя опубликовать приказ, не содержащий заявлений";
                case OrderOfAdmissionNoUID:
                    return "UID приказа должен быть задан";
                case OrderOfAdmissionNumberMustBeUniqueInPackage:
                    return "Номер приказа {0} не должен встречаться в одном пакете у разных приказов (UID: {1})";
                case OrderOfAdmissionNotFound:
                    return "Приказ с UID: {0} не найден";
                case OrderOfAdmissionHasSameUIDAndWrongTypeInDB:
                    return "Приказ с UID: {0} уже имеется в Системе с другим типом приказа";
                case OrderOfAdmissionHasSameUIDAndWrongTypeInPackage:
                    return "Приказ с UID: {0} имеется в пакете с другим типом приказа";
                case OrderOfAdmissionApplicationCompetitiveGroupNotFound:
                    return "При включении заявления в приказ указан несуществующий конкурсе с UID: {0}";

                case OrderOfAdmissionAcgiAlreadyIncluded:
                    return "Заявление уже включено в приказ о зачислении по данному конкурсу";
                case OrderOfAdmissionApplicationAlreadyHas2Orders:
                    return "Заявление уже включено в 2 или более приказа о зачислении (UID: {0})";
                case OrderOfAdmissionApplicationAlreadyHasOrder:
                    return "Заявление уже включено в приказ о зачислении (UID: {0}) и не исключено из него";
                case OrderOfExceptionAcgiHasNoOOA:
                    return "Заявление не может быть включено в приказ об исключении по данному конкурсу, поскольку оно не включено в приказ о зачислении по нему";
                case OrderOfExceptionAcgiAlreadyHasBeenExcluded:
                    return "Заявление уже было включено в приказ об исключении по данному конкурсу";
                case OrderOfExceptionApplicationAlreadyHasExceptionOrder:
                    return "Заявление уже было включено в приказ(ы) об исключении (UID: {0})";
                case OrderOfExceptionDisagreedDateMustBeSpecified:
                    return "При включении в приказ об исключении должна быть указана дата отказа от зачисления";
                case OrderOfAdmissionAcgiHasDisagreedDate:
                    return "Не допускается включать в приказ заявление, если у него есть согласие и отказ по данному конкурсу";

                #endregion

                #region Сервис удаления

                case ApplicationIsNotFound:
                    return "Не найдено заявление";
                case CompetitiveGroupItemIsNotFound:
                    return "Не найдено направление";
                case CompetitiveGroupItemMoreThenOne:
                    return "Найдено более одного направления, неовзможно однозначно определить объект для удаления";
                case AppEntranceTestIsNotFound:
                    return "Не найден результат вступительных испытаний в заявлении (Application\\EntranceTestResults\\EntranceTestResult\\UID)";
                case AppEntranceTestMoreThenOne:
                    return "Найдено более одного результата вступительных испытаний, невозможно однозначно определить объект для удаления";
                case ApplicationBenefitIsNotFound:
                    return "Не найдена льгота, подтверждающая результат  вступительных испытаний в заявлении (Application\\EntranceTestResults\\EntranceTestResult\\UID)";
                case ApplicationBenefitIsMoreThenOne:
                    return "Найдено более одной льготы, подтверждающей результат вступительных испытаний, невозможно однозначно определить объект для удаления";
                case ApplicationIsNotFoundOrAbsentInOrder:
                    return "Заявление не найдено или отcутствует в приказе.";
                case ApplicationIsInOrder:
                    return "Заявление включено в приказ. Для удаления заявления необходимо сначала исключить его из приказа";
                case ApplicationIsInRecList:
                    return "Заявление включено в cписки рекомендованных к зачислению. Для удаления заявления необходимо сначала исключить его из списков";
                case CantDeleteDirectionLinkedWithAppInOrder:
                    return "Невозможно удалить направление, так как по нему есть заявления в приказе";
                case CantDeleteLastDirectionInCompetitiveGroupWithApplications:
                    return "Невозможно удалить направление, так как это последнее направление в конкурсе и существуют заявления на эту группу";
                case CantDeleteResultLinkedWithAppInOrder:
                    return "Невозможно удалить результат ВИ, потому что есть заявления в приказе с данным результатом";
                case CantDeleteCommonBenefitUsedInApp:
                    return "Невозможно удалить общую льготу, потому что есть заявления в приказе с данной льготой";
                case CampaignIsNotFound:
                    return "Не найдена приемная кампания";
                case CampaignMoreThenOne:
                    return "Найдено более одной кампании, неовзможно однозначно определить объект для удаления";
                case CampaignHasDependentCompetitiveGroups:
                    return "Существуют конкурсы, привязанные к данной приемной кампании";
                case CampaignHasDependentInstitutionArchievements:
                    return "Существуют индивидуальные достижения, учитываемые образовательной организацией, привязанные к данной приемной кампании";
                case CampaignHasDependentRecomendedLists:
                    return "Существуют списки рекомендованных к зачислению, привязанные к данной приемной кампании";
                case CampaignHasDependentOrderOfAdmissions:
                    return "Существуют приказы о зачислении, привязанные к данной приемной кампании";
                case CantDeleteDirectionInCompetitiveGroupWithApplications:
                    return "Невозможно удалить направление, так как существуют заявления, поданные на данное направление.";
                case CantDeleteCompetitiveGroupWithApplications:
                    return "Невозможно удалить группу, так как существуют заявления, относящиеся к данной группе";
                case CantDeleteCompetitiveGroupWithGroupItems:
                    return "Невозможно удалить группу, так как существуют направления, относящиеся к данной группе";
                case CompetitiveGroupIsNotFound:
                    return "Группа не найдена";
                case CompetitiveGroupMoreThenOne:
                    return "Найдено более одной группы, неовзможно однозначно определить объект для удаления";
                case FinFormSourceNotAllowedForConsidered:
                    return "Заявление для целевого приема не может быть добавлено в список рассматриваемых";
                case FinFormSourceNotAllowedForRecommended:
                    return "Абитуриент не может быть включен в список Рекомендованных, так как для мест целевого приема не формируется список рекомендованных к зачислению";
                case ApplicationIsNotAcceptedForConsidered:
                    return "Вы пытаетесь включить в список рассматриваемых абитуриента, у которого статус заявления не Принято";
                case ApplicationIsNotFoundInConsidered:
                    return "Заявление не найдено в расматриваемых";
                case EntrantRatingIsLessThanMinimalForConsidered:
                    return "Этот абитуриент не прошел успешно вступительные испытания";
                case AdmissionVolumeOverflow:
                    return "Кол-во рекомендуемых заявлений больше чем доступное кол-во мест";
                case EntranceTestResultsError:
                    return "Ошибка во вступительных испытаниях ({0})";
                case DocumentsError:
                    return "Ошибка в документах";
                case OrderExists:
                    return "Заявление не импортировано из-за наличия связанных данных в системе. Связанные данные: приказ";
                case InstitutionAchievementIsNotFound:
                    return "Не найдены индивидуальные достижения, учитываемые образовательной организацией";
                case InstitutionAchievementCannotBeRemovedWithApplications:
                    return "Индивидуальное достижение не может быть удалено, поскольку имеются в заявлениях"; //  с UID: ({0})
                case AdmissionVolumeCannotBeRemovedWithApplications:
                    return "Объем приема не может быть удален, так как существуют связанные с ним заявления c UID: ({0})";
                case AdmissionVolumeCannotBeChangedWithCampaign:
                    return "В пакете имеются не все данные импорта по кампании с UID: {0}";
                case CompetitiveGroupCannotBeRemovedWithCampaign:
                    return "В пакете имеются не все данные импорта по кампании с UID: {0}";
                case CantDeleteCompetitiveGroupWithReferencedApplications:
                    return "Невозможно импортировать группу, так как в пакет не включеные все ее данные, удаление которых невозмоно, так как с ними связаны заявления с UID: {0}";
                case ApplicationCannotBeExcludedFromOrderStatusPublished:
                    return "Заявление не может быть исключено из приказа со статусом \"Опубликован\"";
                case OrderIsNotFound:
                    return "Приказ не найден";
                case OrderHasApplications:
                    return "Удаление приказа, содержащего заявления, невозможно";
                case ApplicationOrderTryToDeleteOOAButHasOOE:
                    return "Заявление включено в приказ об исключении по данному конкурсу и не может быть удалено из приказа о зачислении";

                case TargetOrganizationMoreThenOne:
                    return "Найдено более одной целевой организации с указанным UID";
                case TargetOrganizationIsNotFound:
                    return "Целевая организация не найдена";
                case CompetitiveGroupTargetHasDependentCompetitiveGroups:
                    return "Имеются конкурсы, в которых указана данная целевая организация";
                case CompetitiveGroupTargetHasDependentApplications:
                    return "Имеются заявления, в которых указана данная целевая организация";
                case CompetitiveGroupHasSameUID:
                    return "В БД существует конкурс с таким же UID: ({0})";
                #endregion

                #region Иностранцы

                case ApplicationInvalidStage:
                    return "Указан некорректный или недопустимый этап приема";
                case ApplicationUnableToIncludeInBeneficiaryOrder:
                    return "Заявление не может быть включено в льготный приказ";
                case ApplicationForeignEntrantUnacceptable:
                    return "В приказ по межправительственным соглашениям могут быть включены только иностранные граждане, претендующие на бюджетные места";

                #endregion

                case TooManyIdentityDocumentsForEntrant:
                    return "Для одного абитуриента указаны разные паспортные данные";

                case ApplicationCannotBeImportedExceptNewOrFailed:
                    return "Допускается импортировать заявления со вступительными испытаниями только со статусами \"Редактируется\", \"Новое\" или \"Не прошедшее проверку\"";
                default:
                    cm_logger.Error($"Отсутсвует сообщение для указанного кода: {messageCode}");
                    return string.Empty;
            }
        }

        public static Dictionary<int, string> GetMessagesList()
        {
            return typeof(ConflictMessages)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.IsLiteral && !x.IsInitOnly)
                .Select(x => (int)x.GetValue(null)).ToDictionary(x => x, GetMessage);
        }
    }
}
