using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries
{
    public class DictionaryExporterSql
	{
        private static readonly Dictionary<int, string> Dictionaries = new Dictionary<int, string>
        {
            { 1, "Общеобразовательные предметы" },
            { 2, "Уровень образования" },
            { 3, "Уровень олимпиады" },
            { 4, "Статус заявления" },
            { 5, "Пол" },
            { 6, "Основание для оценки" },
            { 7, "Страна" },
            { 8, "Регион" }, // hidden
			{ 9, "Коды направлений подготовки" }, // hidden
			{ 10, "Направления подготовки" },
            { 11, "Тип вступительных испытаний" },
            { 12, "Статус проверки заявлений" },
            { 13, "Статус проверки документа" },
            { 14, "Форма обучения" },
            { 15, "Источник финансирования" },
            { 17, "Сообщения об ошибках" },
            { 18, "Тип диплома" },
            { 19, "Олимпиады" },
            { 21, "Гражданство" },
            { 22, "Тип документа, удостоверяющего личность" }, // hidden
			{ 23, "Группа инвалидности" }, // hidden
			{ 24, "Коды профессий" }, // empty
			{ 25, "Профессия" }, // empty
			{ 26, "Коды квалификаций" }, // hidden, empty
			{ 27, "Квалификация" }, // hidden, empty
			{ 28, "Коды специальностей" }, // hidden, empty
			{ 29, "Специальности" }, // hidden, empty
			{ 30, "Вид льготы" }, // hidden
			{ 31, "Тип документа" }, // hidden
			{ 32, "Иностранные языки" },
            { 33, "Тип документа для вступительного испытания ОУ" },
            { 34, "Статус приемной кампании" }, // hidden
            { 35, "Уровень бюджета" },
            { 36, "Категории индивидуальных достижений"},
            { 37, "Статус апелляции, перепроверки"},
            { 38, "Тип приемной кампании"},
            { 39, "Профили олимпиад" },
            { 40, "Классы олимпиад" },
            { 41, "Тип населенного пункта" },
            { 42, "Тип документа, подтверждающего сиротство" },
            { 43, "Тип диплома в области спорта" },
            { 44, "Тип документа, подтверждающего принадлежность к соотечественникам" },
            { 45, "Тип документа, подтверждающего принадлежность к ветеранам боевых действий" },
            { 46, "Тип документа, подтверждающего принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей" },
            { 47, "Тип документа, подтверждающего принадлежность к сотрудникам государственных органов Российской Федерации" },
            { 48, "Тип документа, подтверждающего участие в работах на радиационных объектах или воздействие радиации" },
            { 49, "Способ возврата документов абитуриенту" },
        };

        //private static readonly List<int> HiddenDictionaries = new List<int> { 8, 9, 22, 23, 26, 27, 28, 29, 30, 31, 34 };
		private static readonly List<int> HiddenDictionaries = new List<int> { };

        private static readonly ConcurrentDictionary<int, IDictionaryDataDto> Cache = new ConcurrentDictionary<int, IDictionaryDataDto>();
        private int _institutionId;

        public DictionaryExporterSql(int _institutionId)
        {
            this._institutionId = _institutionId;
        }

        /// <summary>
        /// Получение списка справочников
        /// </summary>
        public static string GetDictionariesListDtoString()
		{
			return PackageHelper.GenerateXmlPackageIntoString(GetDictionariesListDto(), "Dictionaries");
		}

		public string GetDictionaryDataString(int dictionaryCode)
		{
			var dictionaryDataDto = GetDictionaryData(dictionaryCode);

			if (dictionaryDataDto.GetType() == typeof(DirectionDictionaryDataDto))
				return PackageHelper.GenerateXmlPackageIntoString((DirectionDictionaryDataDto)dictionaryDataDto);

			if (dictionaryDataDto.GetType() == typeof(OlympicDictionaryDataDto))
				return PackageHelper.GenerateXmlPackageIntoString((OlympicDictionaryDataDto)dictionaryDataDto);

			return PackageHelper.GenerateXmlPackageIntoString((DictionaryDataDto)dictionaryDataDto);
		}

        public string GetTestDictionaryDataString(int dictionaryCode)
        {
            return GetDictionaryDataString(dictionaryCode);
        }

        public static Dictionary[] GetDictionariesListDto()
        {
            return Dictionaries
                .Where(x => !HiddenDictionaries.Contains(x.Key))
                .OrderBy(x => x.Key)
                .Select(x => new Dictionary { Code = x.Key.ToString(CultureInfo.InvariantCulture), Name = x.Value }).ToArray();
        }

        private static IDictionaryDataDto LoadDictionaryData<TLoader>(int code) where TLoader : IDictionaryDataLoader<DictionaryItemDto>, new()
        {
            return Cache.GetOrAdd(code, c => LoadDictionaryDataInternal(c, new TLoader()));
        }

        private static IDictionaryDataDto LoadDictionaryDataInternal(int code, IDictionaryDataLoader<DictionaryItemDto> loader)
        {
            using (loader)
            {
                return new DictionaryDataDto
                {
                    Code = code.ToString(CultureInfo.InvariantCulture),
                    Name = Dictionaries[code],
                    DictionaryItems = loader.Load()
                };    
            }
        }

        private static IDictionaryDataDto LoadOlympicDictionaryData()
        {
            return Cache.GetOrAdd(19, c => LoadOlympicDictionaryDataInternal());
        }

        private static IDictionaryDataDto LoadOlympicDictionaryDataInternal()
        {
            using (var loader = new OlympicDictionaryDataLoader())
            {
                return new OlympicDictionaryDataDto
                    {
                        Code = "19",
                        Name = Dictionaries[19],
                        DictionaryItems = loader.Load()
                    };
            }
        }

        private IDictionaryDataDto LoadDirectionDictionaryData()
        {
            //return Cache.GetOrAdd(10, c => LoadDirectionDictionaryDataInternal());
            // Теперь выдаются AllowedDirections по каждому ОО, поэтому нельзя кэшировать для всех!
            return LoadDirectionDictionaryDataInternal();
        }

        private IDictionaryDataDto LoadDirectionDictionaryDataInternal()
        {
            using (var loader = new DirectionDictionaryDataLoader(_institutionId))
            {
                return new DirectionDictionaryDataDto
                    {
                        Code = "10",
                        Name = Dictionaries[10],
                        DictionaryItems = loader.Load()
                        
                    };
            }
        }

        public IDictionaryDataDto GetDictionaryData(int dictionaryCode)
		{
			switch (dictionaryCode)
			{
				case 1:
                    return LoadDictionaryData<SubjectDictionaryDataLoader>(dictionaryCode);
				case 2:
			        return LoadDictionaryData<EducationLevelDictionaryDataLoader>(dictionaryCode);
				case 3:
			        return LoadDictionaryData<OlympicLevelDictionaryDataLoader>(dictionaryCode);
				case 4:
			        return LoadDictionaryData<ApplicationStatusDictionaryDataLoader>(dictionaryCode);
				case 5:
			        return LoadDictionaryData<GenderDictionaryDataLoader>(dictionaryCode);
				case 6:
			        return LoadDictionaryData<TestResultSourceDictionaryDataLoader>(dictionaryCode);
				case 7:
			        return LoadDictionaryData<CountryDictionaryDataLoader>(dictionaryCode);
				case 8:
			        return LoadDictionaryData<RegionDictionaryDataLoader>(dictionaryCode);
				case 9:
			        return LoadDictionaryData<DirectionCodeDictionaryDataLoader>(dictionaryCode);
				case 10:
			        return LoadDirectionDictionaryData();
				case 11:
			        return LoadDictionaryData<EntranceTestTypeDictionaryDataLoader>(dictionaryCode);
				case 12:
			        return LoadDictionaryData<ApplicationCheckStatusDictionaryDataLoader>(dictionaryCode);
				case 13:
			        return LoadDictionaryData<DocumentCheckStatusDictionaryDataLoader>(dictionaryCode);
				case 14:
			        return LoadDictionaryData<EducationFormDictionaryDataLoader>(dictionaryCode);
				case 15:
			        return LoadDictionaryData<EducationSourceDictionaryDataLoader>(dictionaryCode);
				case 17:
			        return LoadDictionaryData<ErrorMessagesDictionaryDataLoader>(dictionaryCode);
				case 18:
                    return LoadDictionaryData<OlympicDiplomTypeDictionaryDataLoader>(dictionaryCode);
				case 19:
			        return LoadOlympicDictionaryData();
				case 21:
			        return LoadDictionaryData<CitizenshipDictionaryDataLoader>(dictionaryCode);
				case 22:
			        return LoadDictionaryData<IdentityDocumentTypeDictionaryDataLoader>(dictionaryCode);
				case 23:
                    return LoadDictionaryData<DisabilityTypeDictionaryDataLoader>(dictionaryCode);
				case 24: // пустые справочники
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
			        return new DictionaryDataDto {Code = dictionaryCode.ToString(CultureInfo.InvariantCulture), Name = Dictionaries[dictionaryCode]};
				case 30:
                    return LoadDictionaryData<BenefitTypeDictionaryDataLoader>(dictionaryCode);
				case 31:
                    return LoadDictionaryData<DocumentTypeDictionaryDataLoader>(dictionaryCode);
				case 32:
                    return LoadDictionaryData<ForeignLanguagesDictionaryDataLoader>(dictionaryCode);
				case 33:
                    return LoadDictionaryData<InstitutionDocumentTypeDictionaryDataLoader>(dictionaryCode);
				case 34:
                    return LoadDictionaryData<CampaignStatusDictionaryDataLoader>(dictionaryCode);
                case 35:
                    return LoadDictionaryData<LevelBudgetDictionaryDataLoader>(dictionaryCode);
                case 36:
                    return LoadDictionaryData<IndividualAchievementsCategoryDictionaryDataLoader>(dictionaryCode);
                case 37:
                    return LoadDictionaryData<AppealStatusDictionaryDataLoader>(dictionaryCode);
                case 38:
                    return LoadDictionaryData<CampaignTypeDictionaryDataLoader>(dictionaryCode);
                case 39:
                    return LoadDictionaryData<OlympicProfileDictionaryDataLoader>(dictionaryCode);
                case 40:
                    return LoadDictionaryData<OlympicClassDictionaryDataLoader>(dictionaryCode);

                case 41:
                    return LoadDictionaryData<TownTypeDictionaryDataLoader>(dictionaryCode);
                case 42:
                    return LoadDictionaryData<OrphanCategoryDictionaryDataLoader>(dictionaryCode);
                case 43:
                    return LoadDictionaryData<SportCategoryDictionaryDataLoader>(dictionaryCode);
                case 44:
                    return LoadDictionaryData<CompatriotCategoryDictionaryDataLoader>(dictionaryCode);
                case 45:
                    return LoadDictionaryData<VeteranCategoryDictionaryDataLoader>(dictionaryCode);
                case 46:
                    return LoadDictionaryData<ParentsLostCategoryDictionaryDataLoader>(dictionaryCode);
                case 47:
                    return LoadDictionaryData <StateEmployeeCategoryDictionaryDataLoader>(dictionaryCode);
                case 48:
                    return LoadDictionaryData<RadiationWorkCategoryDictionaryDataLoader>(dictionaryCode);
                case 49:
                    return LoadDictionaryData<ReturnDocumentsTypeDictionaryDataLoader>(dictionaryCode);
            }

            throw new NotSupportedException("Неизвестный код справочника (" + dictionaryCode + ")");
		}
	}
}
