using System.Collections.Generic;
using System.Linq;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService;

namespace GVUZ.ServiceModel.Import.Dictionaries
{
	public static class DictionaryExporter
	{
		private static readonly Dictionary<int, string> _dictionaries = new Dictionary<int, string>
		{
			{ 1, "Общеобразовательные предметы" },
			{ 2, "Уровень образования" },
			{ 3, "Уровень олимпиады" },
			{ 4, "Статус заявления" },
			{ 5, "Пол" },
			{ 6, "Основание для оценки" },
			{ 7, "Страна" },
			{ 8, "Регион" },
			{ 9, "Коды направлений подготовки" },
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
			{ 22, "Тип документа, удостоверяющего личность" },
			{ 23, "Группа инвалидности" },
			{ 24, "Коды профессий" },
			{ 25, "Профессия" },
			{ 26, "Коды квалификаций" },
			{ 27, "Квалификация" },
			{ 28, "Коды специальностей" },
			{ 29, "Специальности" },
			{ 30, "Вид льготы" },
			{ 31, "Тип документа" },
			{ 32, "Иностранные языки" },
			{ 33, "Тип документа для вступительного испытания ОУ" },
			{ 34, "Статус приемной кампании" },
		};

		private static readonly List<int> _hiddenDictionaries = new List<int> { 8, 9, 20, 21, 24, 25, 26, 27, 28, 29, 32 };

		/// <summary>
		/// Получение списка справочников
		/// </summary>
		public static string GetDictionariesListDtoString()
		{
			return PackageHelper.GenerateXmlPackageIntoString(GetDictionariesListDto(), "Dictionaries");
		}

		public static Dictionary[] GetDictionariesListDto()
		{
			return _dictionaries
				.Where(x => !_hiddenDictionaries.Contains(x.Key))
				.OrderBy(x => x.Key)
				.Select(x => new Dictionary { Code = x.Key.ToString(), Name = x.Value }).ToArray();
		}

        //public static string GetDictionaryDataString(int dictionaryCode)
        //{
        //	var dictionaryDataDto = GetDictionaryData(dictionaryCode);
        //	if (dictionaryDataDto == null)
        //		return null;

        //          //подобная логика для корректной типизации и красивой конечной сериализации
        //	if (dictionaryDataDto.GetType() == typeof(DirectionDictionaryDataDto))
        //		return PackageHelper.GenerateXmlPackageIntoString((DirectionDictionaryDataDto)dictionaryDataDto);

        //          //подобная логика для корректной типизации и красивой конечной сериализации
        //	if (dictionaryDataDto.GetType() == typeof(OlympicDictionaryDataDto))
        //		return PackageHelper.GenerateXmlPackageIntoString((OlympicDictionaryDataDto)dictionaryDataDto);

        //	return PackageHelper.GenerateXmlPackageIntoString((DictionaryDataDto)dictionaryDataDto);
        //}

        //public static IDictionaryDataDto GetDictionaryData(int dictionaryCode)
        //{
        //	var dto = new DictionaryDataDto();
        //	if (!_dictionaries.ContainsKey(dictionaryCode))
        //		return null;

        //	dto.Code = dictionaryCode.ToString();
        //	dto.Name = _dictionaries[dictionaryCode];
        //	switch (dictionaryCode)
        //	{
        //		case 1:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.ID).ToArray();
        //			break;
        //		case 2:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.EducationLevel)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).ToArray();
        //			break;
        //		case 3:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.OlympicLevel)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).ToArray();
        //			break;
        //		case 4:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.ApplicationStatusType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).ToArray();
        //			break;
        //		case 5:
        //	        dto.DictionaryItems = new DictionaryItemDto[]
        //	                                  {
        //	                                      new DictionaryItemDto()
        //	                                          {
        //	                                              ID = GenderType.Male.ToString(),
        //	                                              Name = GenderType.GetName(GenderType.Male)
        //	                                          },
        //	                                      new DictionaryItemDto()
        //	                                          {
        //	                                              ID = GenderType.Female.ToString(),
        //	                                              Name = GenderType.GetName(GenderType.Female)
        //	                                          }
        //	                                  };
        //			break;
        //		case 6:
        //                  using (var dbContext = new EntrantsEntities())
        //			{
        //				dto.DictionaryItems = dbContext.EntranceTestResultSource
        //					.OrderBy(x => x.Description)
        //					.Select(x => new { ID = x.SourceID, Name = x.Description })
        //					.ToArray()
        //					.Select(x => new DictionaryItemDto { ID = x.ID.ToString(), Name = x.Name })
        //					.ToArray();
        //			}

        //			break;
        //		case 7:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.Name).ToArray();
        //			break;
        //		case 8:
        //                  using (var dbContext = new EntrantsEntities())
        //			{
        //				dto.DictionaryItems = dbContext.RegionType
        //					.OrderBy(x => x.Name)
        //					.Select(x => new { ID = x.RegionId, Name = x.Name })
        //					.ToArray()
        //					.Select(x => new DictionaryItemDto { ID = x.ID.ToString(), Name = x.Name })
        //					.ToArray();
        //			}

        //			break;
        //		case 9:
        //                  using (var dbContext = new EntrantsEntities())
        //			{
        //				dto.DictionaryItems = dbContext.Direction
        //					.OrderBy(x => x.Name)
        //					.Select(x => new { ID = x.DirectionID, Name = x.Code })
        //					.ToArray()
        //					.Select(x => new DictionaryItemDto { ID = x.ID.ToString(), Name = x.Name })
        //					.ToArray();
        //			}

        //			break;
        //		case 10:
        //                  using (var dbContext = new ImportEntities())
        //			{
        //				//новые постановки
        //                      var ddto = new DirectionDictionaryDataDto();
        //				ddto.Code = dto.Code;
        //				ddto.Name = dto.Name;
        //				ddto.DictionaryItems = dbContext.Direction
        //					.Select(x => new { x, x.ParentDirection }).ToArray()
        //					.Select(x => new DirectionDictionaryItemDto
        //					{
        //						ID = x.x.DirectionID.ToString(),
        //						Name = x.x.Name,
        //						Code = x.x.Code == null ? "" : x.x.Code.Trim(),
        //                              NewCode = x.x.NewCode ?? "",
        //						QualificationCode = (x.x.QUALIFICATIONCODE ?? "").Trim(),
        //						Period = (x.x.PERIOD ?? ""),
        //						UGSCode = x.ParentDirection != null ? x.ParentDirection.Code : null,
        //						UGSName = x.ParentDirection != null ? x.ParentDirection.Name : null
        //					}).ToArray();
        //				return ddto;
        //			}

        //		case 11:
        //                  using (var dbContext = new EntrantsEntities())
        //			{
        //				dto.DictionaryItems = dbContext.EntranceTestType
        //					.Where(x => x.EntranceTestTypeID == 1 || x.EntranceTestTypeID == 2 || x.EntranceTestTypeID == 3)
        //					.OrderBy(x => x.Name)
        //					.Select(x => new { ID = x.EntranceTestTypeID, Name = x.Name })
        //					.ToArray()
        //					.Select(x => new DictionaryItemDto { ID = x.ID.ToString(), Name = x.Name })
        //					.ToArray();
        //			}

        //			break;
        //		case 12:
        //			dto.DictionaryItems = new[]
        //			{
        //				new DictionaryItemDto { ID = "-1", Name = "Не проверен" },
        //				new DictionaryItemDto { ID = "0", Name = "Найдены заявления для проверки" },
        //				new DictionaryItemDto { ID = "1", Name = "Не найдены заявления для проверки" },
        //			};
        //			break;
        //		case 13:
        //			dto.DictionaryItems = new[]
        //			{
        //				new DictionaryItemDto { ID = "0", Name = "Проверен" },
        //				new DictionaryItemDto { ID = "1", Name = "Общая ошибка" },
        //				new DictionaryItemDto { ID = "2", Name = "Ошибка в предметах" },
        //			};
        //			break;
        //		case 14:
        //                  dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Study)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.Name).ToArray();
        //			break;
        //		case 15:
        //                  dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.AdmissionType)
        //                      .Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.Name).ToArray();
        //			break;
        //		case 17:
        //			dto.DictionaryItems = Enumerable.ToArray(ConflictMessages.GetMessagesList()
        //				    .Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }));
        //			break;
        //		case 18:
        //			using (var dbContext = new EntrantsEntities())
        //			{
        //				dto.DictionaryItems = dbContext.OlympicDiplomType
        //					.Where(x => x.OlympicDiplomTypeID != 3)
        //					.OrderBy(x => x.Name)
        //					.Select(x => new { ID = x.OlympicDiplomTypeID, Name = x.Name })
        //					.ToArray()
        //					.Select(x => new DictionaryItemDto { ID = x.ID.ToString(), Name = x.Name })
        //					.ToArray();
        //			}

        //			break;
        //		case 19:
        //                  using (var dbContext = new EntrantsEntities())
        //			{
        //				OlympicDictionaryDataDto ddto = new OlympicDictionaryDataDto();
        //				ddto.Code = dto.Code;
        //				ddto.Name = dto.Name;

        //				//ddto.DictionaryItems = dbContext.OlympicType
        //				//	.OrderBy(x => x.OlympicNumber)
        //				//	.Select(x => new { ID = x.OlympicID, x.Name, x.OlympicLevelID, x.OlympicNumber, 
        //    //                          Subjects = x.OlympicTypeSubjectLink, x.OlympicYear })
        //				//	.ToArray()
        //				//	.Select(x => new OlympicDictionaryItemDto
        //				//	{
        //				//		OlympicID = x.ID.ToString(),
        //    //                          OlympicNumber = x.OlympicNumber.ToString(),
        //    //                          OlympicName = x.Name.ToString(),
        //    //                          //OlympicLevelID = x.OlympicLevelID != null ? x.OlympicLevelID.ToString() : null,
        //    //                          Year = x.OlympicYear.ToString(),
        //    //                          Profiles = x.Pro
        //				//		Subjects = x.Subjects.Select(y => 
        //    //                              new SubjectLevel
        //    //                                  {
        //    //                                      SubjectID = y .SubjectID,
        //    //                                      LevelID = y.SubjectLevelID != null ? y.SubjectLevelID.Value.ToString() : null
        //    //                                  }).ToArray()
        //				//	})
        //				//	.ToArray();
        //				return ddto;
        //			}
        //		case 21:
        //                  dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.Name).ToArray();
        //			break;
        //		case 22:
        //				dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.IdentityDocumentType)
        //					.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.ID).ToArray();
        //			break;
        //		case 23:
        //                  dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DisabilityType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.ID).ToArray();
        //			break;
        //		case 24:
        //  				break;
        //		case 25:
        //			break;
        //		case 26:
        //			break;
        //		case 27:
        //			break;
        //		case 28:
        //			break;
        //		case 29:
        //			break;
        //		case 30:
        //                  using (var dbContext = new EntrantsEntities())
        //			{
        //				dto.DictionaryItems = dbContext.Benefit
        //					.OrderBy(x => x.BenefitID)
        //					.Select(x => new { ID = x.BenefitID, Name = x.Name })
        //					.ToArray()
        //					.Select(x => new DictionaryItemDto { ID = x.ID.ToString(), Name = x.Name })
        //					.ToArray();
        //			}

        //			break;
        //		case 31:
        //                  dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(x => x.ID).ToArray();
        //			break;
        //		case 32:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.ForeignLanguageType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.ID).ToArray();
        //			break;
        //		case 33:
        //			dto.DictionaryItems = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.InstitutionDocumentType)
        //				.Select(x => new DictionaryItemDto { ID = x.Key.ToString(), Name = x.Value }).OrderBy(c => c.ID).ToArray();
        //			break;
        //		case 34:
        //			dto.DictionaryItems = new[]
        //			{
        //				new DictionaryItemDto { ID = "0", Name = "Набор не начался" },
        //				new DictionaryItemDto { ID = "1", Name = "Идет набор" },
        //				new DictionaryItemDto { ID = "2", Name = "Завершена" }
        //			};
        //			break;
        //	}

        //	return dto;
        //}

        public static string GetTestDictionaryDataString(int dictionaryCode)
        {
            return ""; // GetDictionaryDataString(dictionaryCode);
        }
    }
}
