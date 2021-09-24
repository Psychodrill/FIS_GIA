using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.Web.Controllers;
using GVUZ.Web.Helpers;
using GVUZ.Web.ViewModels;
using GVUZ.ServiceModel.SQL;
using GVUZ.Web.Controllers.Admission;
using GVUZ.ServiceModel.SQL.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;

namespace GVUZ.Web.ContextExtensions
{
    public class GetInstitution : BaseController
    {
        public int GetInstitutionID()
        {
            int x = InstitutionID;
            return x;
        }
    }
    /// <summary>
    /// Методы для информации о импорте в интерфейсе
    /// </summary>
    public static class ImportExtensions
	{
		private static readonly int ListPageSize = AppSettings.Get("Search.PageSize", 25);

		/// <summary>
		/// Загружаем фильтры для страницы списка пакетов
		/// </summary>
		public static ImportListViewModel InitialFillImportListViewModell(this ImportEntities dbContext, int isAdminOrInst = 0)
		{
			var model = new ImportListViewModel();
            if (isAdminOrInst==0)//Администратор
            {
                var inst = dbContext.Institution.Select(x => new { ID = x.InstitutionID, Name = x.BriefName }).ToList();
                model.Institutions = inst.ToArray();
            }
            else
            {
                model.Institutions = new List<Institution>();
            }

			var types = dbContext.ImportPackageType.Select(x => new { ID = x.TypeID, Name = x.Name })
				.ToList();
			types.Insert(0, new { ID = 0, Name = "[По всем типам]" });  
			model.Types = types.ToArray();

            if (isAdminOrInst != 0)
                model.Logins = dbContext.ImportPackage.Where(x => 
                    x.InstitutionID == isAdminOrInst).Where(x => x.UserLogin != null).Select(x => x.UserLogin).Distinct().ToArray();

			return model;
		}

		/// <summary>
		/// Список пакетов по фильтру
		/// </summary>
		/// <param name="usePaging">Использовать пейджинг (не использовать для выгрузки)</param>
        public static AjaxResultModel GetImportListModel(this ImportEntities dbContext, ImportListViewModel model, bool usePaging = true, int isAdminOrInst = 0)
		{
            IQueryable<ImportPackage> tq = dbContext.ImportPackage;
            if (isAdminOrInst != 0) tq = tq.Where(x => x.InstitutionID == isAdminOrInst);
			model.TotalItemCount = tq.Count();

            if (isAdminOrInst == 0)
            {
                if (!String.IsNullOrEmpty(model.SelectedInstitution))
                    tq = tq.Where(x => x.Institution.BriefName.Contains(model.SelectedInstitution));
            }
			if (!String.IsNullOrEmpty(model.SelectedLogin))
            
				tq = tq.Where(x => x.UserLogin.Contains(model.SelectedLogin));
			if (model.SelectedType != 0)
				tq = tq.Where(x => x.TypeID == model.SelectedType);
			if (model.DateBegin.HasValue)
				tq = tq.Where(x => x.CreateDate >= model.DateBegin);
			if (model.DateEnd.HasValue)
			{
				var endDateAdj = model.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
				tq = tq.Where(x => x.CreateDate <= endDateAdj);
			}

			model.TotalItemFilteredCount = tq.Count();
			model.TotalPageCount = ((Math.Max(model.TotalItemFilteredCount, 1) - 1) / ListPageSize) + 1;

			if (!model.SortID.HasValue)
				model.SortID = -3;

			if (model.SortID == 1) tq = tq.OrderBy(x => x.UserLogin);
			if (model.SortID == -1) tq = tq.OrderByDescending(x => x.UserLogin);
			if (model.SortID == 2) tq = tq.OrderBy(x => x.InstitutionID);
			if (model.SortID == -2) tq = tq.OrderByDescending(x => x.InstitutionID);
			if (model.SortID == 3) tq = tq.OrderBy(x => x.CreateDate);
			if (model.SortID == -3) tq = tq.OrderByDescending(x => x.CreateDate);
			if (model.SortID == 4) tq = tq.OrderByDescending(x => x.StatusID).ThenBy(x => x.LastDateChanged);
			if (model.SortID == -4) tq = tq.OrderByDescending(x => x.StatusID).ThenByDescending(x => x.LastDateChanged);
			
			if (model.SortID == 5) tq = tq.OrderBy(x => x.ImportPackageType.Name);
			if (model.SortID == -5) tq = tq.OrderByDescending(x => x.ImportPackageType.Name);

			if (model.SortID == 7) tq = tq.OrderBy(x => x.ImportPackageStatus.Name);
			if (model.SortID == -7) tq = tq.OrderByDescending(x => x.ImportPackageStatus.Name);

			if (model.SortID == 8) tq = tq.OrderBy(x => x.UserLogin);
			if (model.SortID == -8) tq = tq.OrderByDescending(x => x.UserLogin);

			if (usePaging)
				tq = tq.Skip((model.PageNumber ?? 0) * ListPageSize).Take(ListPageSize);
			var res = tq.Select(x => new
			                         	{
			                         		x.PackageID,
											x.InstitutionID,
			                         		InstitutionName = x.Institution.BriefName,
			                         		x.CreateDate,
			                         		x.LastDateChanged,
			                         		TypeName = x.ImportPackageType.Name,
			                         		StatusName = x.ImportPackageStatus.Name,
			                         		x.StatusID,
			                         		x.TypeID,
			                         		x.Content,
											x.UserLogin
			                         	}).ToArray();

			model.ImportPackages = res.Select(x => new ImportListViewModel.ImportPackageData
			                                       	{
			                                       		ID = x.PackageID,
			                                       		PackageID = x.PackageID.ToString(),
														InstitutionID = x.InstitutionID,
			                                       		InstitutionName = x.InstitutionName,
			                                       		DateSent = x.CreateDate.ToString("dd.MM.yyy HH:mm:ss"),
														DateProcessing = x.StatusID == 3 ? x.LastDateChanged.ToString("dd.MM.yyy HH:mm:ss") : "",
			                                       		Type = x.TypeName,
			                                       		Status = x.StatusName,
                                                        Content = x.Content,
														Login = x.UserLogin
			                                       	}).ToArray();
			return new AjaxResultModel { Data = model };
		}

		/// <summary>
		/// Загружаем XML пакета
		/// </summary>
        public static string GetPackageXml(this ImportEntities dbContext, int packageID)
		{
		    int institutionId = InstitutionHelper.GetInstitutionID();

            string result = dbContext.ImportPackage.Where(x => x.PackageID == packageID && x.InstitutionID == institutionId)
                    .Select(x => x.PackageData).FirstOrDefault();

		    if (!string.IsNullOrEmpty(result))
		    {
		        result = XmlPreparations.Prepare(result);
		    }

		    return result;
        }

		/// <summary>
		/// Детальная информация о пакете
		/// </summary>
        public static ImportPackageInfoViewModel GetImportInfo(this ImportEntities dbContext, int packageID, int institutionID)
		{
			var queryable = from ip in dbContext.ImportPackage
                            where ip.PackageID == packageID
			                select ip;

            if (institutionID > 0)
            {
                queryable = queryable.Where(x => x.InstitutionID == institutionID);
            }

			var e = queryable.FirstOrDefault();
			if (e == null)
				return new ImportPackageInfoViewModel();

			string resultError = "";
			int countProcessed = 0;
			int countNotProcessed = 0;
			var errors = new List<ImportPackageInfoViewModel.ErrorObjectsData>();
			var success = new List<ImportPackageInfoViewModel.SuccessObjectsData>();

			//если есть результат, формируем его
			if (!String.IsNullOrEmpty(e.ProcessResultInfo))
			{
			    try
			    {
			        var elem = XElement.Parse(e.ProcessResultInfo);
                    if (elem.Name == "Error")
                    {
                        if (elem.Element("ErrorText") != null)
                            resultError = elem.Element("ErrorText").Value;
                        else
                            resultError = elem.Value;
                    }
                    else
                    {
                        if (e.TypeID == 1 || e.TypeID == 11) //import
                        {
                            ImportResultPackage resultPackage =
                                new Serializer().Deserialize<ImportResultPackage>(e.ProcessResultInfo);
                            countProcessed = GetSuccessCount(resultPackage.Log);
                            countNotProcessed = GetFailCount(resultPackage.Log);

                            GetFailDetails(errors, resultPackage, null);
                            //PackageData importPackage = new Serializer().Deserialize<PackageData>(XmlPreparations.Prepare(e.PackageData));
                            GVUZ.ImportService2016.Core.Dto.Import.PackageData importPackage = new Serializer().Deserialize<GVUZ.ImportService2016.Core.Dto.Import.PackageData>(XmlPreparations.Prepare(e.PackageData));
                            GetSuccessDetails(dbContext, success, importPackage, resultPackage);

                            if (e.CheckResultInfo != null)
                            {
                                var resultCheckPackage =
                                    new Serializer().Deserialize<ImportedAppCheckResultPackage>(e.CheckResultInfo);
                                GetCheckPackage(resultCheckPackage, success, "Проверка заявлений");
                            }
                        }

                        if (e.TypeID == 2) //delete
                        {
                            var resultPackage = new Serializer().Deserialize<DeleteResultPackage>(e.ProcessResultInfo);
                            countProcessed = GetSuccessCount(resultPackage.Log);
                            countNotProcessed = GetFailCount(resultPackage.Log);

                            GetFailDetails(errors, null, resultPackage);
                        }

                        if (e.TypeID == 3) //сведения о проверках
                        {
                            var resultPackage = new Serializer().Deserialize<ImportedAppCheckResultPackage>(e.ProcessResultInfo);
                            GetCheckPackage(resultPackage, success, "Проверка заявлений");
                        }
                    }
			    }
			    catch (Exception)
			    {
			        resultError = e.ProcessResultInfo;
			    }
			}

			if (e.TypeID == 4) //валидация
			{
				//do nothing
			}

			if (e.TypeID == 5) //список справочников
			{
				var dList = DictionaryExporterSql.GetDictionariesListDto();
				success.AddRange(dList.Select(dictionary => new ImportPackageInfoViewModel.SuccessObjectsData
				{
					ObjectType = "Справочник",
					ObjectDetails = dictionary.Code + " " + dictionary.Name
				}));
			}

			if (e.TypeID == 6) //список элементов справочника
			{
				XElement dCode = XElement.Parse(e.PackageData).Elements().Where(x => x.Name == "DictionaryCode").FirstOrDefault();
				if (dCode != null)
				{
					string dictID = dCode.Value.To(0).ToString();
					var dList = DictionaryExporterSql.GetDictionariesListDto().Where(x => x.Code == dictID).FirstOrDefault();
					if (dList != null)
					{
						success.Add(new ImportPackageInfoViewModel.SuccessObjectsData { ObjectType = "Справочник", ObjectDetails = dList.Code + " " + dList.Name });
                        var exporter = new DictionaryExporterSql(institutionID);
                        var iDictionaryDataDto = exporter.GetDictionaryData(dictID.To(0));
						var dictionaryDataDto = iDictionaryDataDto as DictionaryDataDto;
						if (dictionaryDataDto != null)
							success.AddRange(dictionaryDataDto.DictionaryItems.Select(diDto => new ImportPackageInfoViewModel.SuccessObjectsData
							{
								ObjectType = "Элемент справочника", ObjectDetails = "" + diDto.ID + " " + diDto.Name
							}));
						var dictionaryDataDtoDir = iDictionaryDataDto as DirectionDictionaryDataDto;
						if (dictionaryDataDtoDir != null)
							success.AddRange(dictionaryDataDtoDir.DictionaryItems.Select(diDto => new ImportPackageInfoViewModel.SuccessObjectsData { ObjectType = "Элемент справочника", ObjectDetails = "" + diDto.DirectionID + " " + diDto.Name }));
					}
				}
			}

			if (e.TypeID == 7) //тестовые ответы
			{
				//do nothing
			}

			if (e.TypeID == 8) //информация по ОО
			{
				//do nothing
			}

#warning arzyanin added
		    success.ForEach(x => x.InstitutionID = e.InstitutionID);
            
            return new ImportPackageInfoViewModel
			       	{
			       		PackageID = e.PackageID,
						InstitutionName = e.Institution.BriefName,
						DateSent = e.CreateDate.ToString(),
						DateProcessing = e.StatusID == 3 ? e.LastDateChanged.ToString("dd.MM.yyyy HH:mm:ss") : "",
						Type = e.ImportPackageType.Name,
						Status = e.ImportPackageStatus.Name,
						ResultError = resultError,
						CountProcessed = countProcessed,
						CountNotProcessed = countNotProcessed,
						Comment = e.Comment,
						ErrorObjects = errors.ToArray(),
                        SuccessObjects = success.ToArray(),
                        Login = dbContext.ImportPackage.Where(x => x.PackageID == packageID).Select(x => x.UserLogin).SingleOrDefault(),
			       	};
		}

        /// <summary>
		/// Для фильтра по пакету
		/// </summary>
        public static AjaxResultModel GetImportInfoList(this ImportEntities dbContext, int packageID, int? SortID, int institutionID)
        {
            var queryable = from ip in dbContext.ImportPackage
                            where ip.PackageID == packageID
                            select ip;

            if (institutionID > 0)
            {
                queryable = queryable.Where(x => x.InstitutionID == institutionID);
            }

            var e = queryable.FirstOrDefault();
            if (e == null)
                return new AjaxResultModel();

            string resultError = "";
            int countProcessed = 0;
            int countNotProcessed = 0;
            var errors = new List<ImportPackageInfoViewModel.ErrorObjectsData>();
            var success = new List<ImportPackageInfoViewModel.SuccessObjectsData>();

            //если есть результат, формируем его
            if (!String.IsNullOrEmpty(e.ProcessResultInfo))
            {
                try
                {
                    var elem = XElement.Parse(e.ProcessResultInfo);
                    if (elem.Name == "Error")
                    {
                        if (elem.Element("ErrorText") != null)
                            resultError = elem.Element("ErrorText").Value;
                        else
                            resultError = elem.Value;
                    }
                    else
                    {
                        if (e.TypeID == 1 || e.TypeID == 11) //import
                        {
                            ImportResultPackage resultPackage =
                                new Serializer().Deserialize<ImportResultPackage>(e.ProcessResultInfo);
                            countProcessed = GetSuccessCount(resultPackage.Log);
                            countNotProcessed = GetFailCount(resultPackage.Log);

                            GetFailDetails(errors, resultPackage, null);
                            //PackageData importPackage = new Serializer().Deserialize<PackageData>(XmlPreparations.Prepare(e.PackageData));
                            GVUZ.ImportService2016.Core.Dto.Import.PackageData importPackage = new Serializer().Deserialize<GVUZ.ImportService2016.Core.Dto.Import.PackageData>(XmlPreparations.Prepare(e.PackageData));
                            GetSuccessDetails(dbContext, success, importPackage, resultPackage);

                            if (e.CheckResultInfo != null)
                            {
                                var resultCheckPackage =
                                    new Serializer().Deserialize<ImportedAppCheckResultPackage>(e.CheckResultInfo);
                                GetCheckPackage(resultCheckPackage, success, "Проверка заявлений");
                            }
                        }

                        if (e.TypeID == 2) //delete
                        {
                            var resultPackage = new Serializer().Deserialize<DeleteResultPackage>(e.ProcessResultInfo);
                            countProcessed = GetSuccessCount(resultPackage.Log);
                            countNotProcessed = GetFailCount(resultPackage.Log);

                            GetFailDetails(errors, null, resultPackage);
                        }

                        if (e.TypeID == 3) //сведения о проверках
                        {
                            var resultPackage = new Serializer().Deserialize<ImportedAppCheckResultPackage>(e.ProcessResultInfo);
                            GetCheckPackage(resultPackage, success, "Проверка заявлений");
                        }
                    }
                }
                catch (Exception)
                {
                    resultError = e.ProcessResultInfo;
                }
            }

            if (e.TypeID == 4) //валидация
            {
                //do nothing
            }

            if (e.TypeID == 5) //список справочников
            {
                var dList = DictionaryExporterSql.GetDictionariesListDto();
                success.AddRange(dList.Select(dictionary => new ImportPackageInfoViewModel.SuccessObjectsData
                {
                    ObjectType = "Справочник",
                    ObjectDetails = dictionary.Code + " " + dictionary.Name
                }));
            }

            if (e.TypeID == 6) //список элементов справочника
            {
                XElement dCode = XElement.Parse(e.PackageData).Elements().Where(x => x.Name == "DictionaryCode").FirstOrDefault();
                if (dCode != null)
                {
                    string dictID = dCode.Value.To(0).ToString();
                    var dList = DictionaryExporterSql.GetDictionariesListDto().Where(x => x.Code == dictID).FirstOrDefault();
                    if (dList != null)
                    {
                        success.Add(new ImportPackageInfoViewModel.SuccessObjectsData { ObjectType = "Справочник", ObjectDetails = dList.Code + " " + dList.Name });
                        var exporter = new DictionaryExporterSql(institutionID);
                        var iDictionaryDataDto = exporter.GetDictionaryData(dictID.To(0));
                        var dictionaryDataDto = iDictionaryDataDto as DictionaryDataDto;
                        if (dictionaryDataDto != null)
                            success.AddRange(dictionaryDataDto.DictionaryItems.Select(diDto => new ImportPackageInfoViewModel.SuccessObjectsData
                            {
                                ObjectType = "Элемент справочника",
                                ObjectDetails = "" + diDto.ID + " " + diDto.Name
                            }));
                        var dictionaryDataDtoDir = iDictionaryDataDto as DirectionDictionaryDataDto;
                        if (dictionaryDataDtoDir != null)
                            success.AddRange(dictionaryDataDtoDir.DictionaryItems.Select(diDto => new ImportPackageInfoViewModel.SuccessObjectsData { ObjectType = "Элемент справочника", ObjectDetails = "" + diDto.DirectionID + " " + diDto.Name }));
                    }
                }
            }

            if (e.TypeID == 7) //тестовые ответы
            {
                //do nothing
            }

            if (e.TypeID == 8) //информация по ОО
            {
                //do nothing
            }

            success.ForEach(x => x.InstitutionID = e.InstitutionID);

            if (!SortID.HasValue)
                SortID = -3;

            if (SortID == 1) errors = errors.OrderBy(x => x.ObjectType).ToList();
            if (SortID == -1) errors = errors.OrderByDescending(x => x.ObjectType).ToList();
            if (SortID == 2) errors = errors.OrderBy(x => x.ObjectDetails).ToList();
            if (SortID == -2) errors = errors.OrderByDescending(x => x.ObjectDetails).ToList();
            if (SortID == 3) errors = errors.OrderBy(x => x.ErrorCode).ToList();
            if (SortID == -3) errors = errors.OrderByDescending(x => x.ErrorCode).ToList();
            if (SortID == 4) errors = errors.OrderBy(x => x.ErrorText).ToList();
            if (SortID == -4) errors = errors.OrderByDescending(x => x.ErrorText).ToList();

            if (SortID == 5) success = success.OrderBy(x => x.ObjectType).ToList();
            if (SortID == -5) success = success.OrderByDescending(x => x.ObjectType).ToList();
            if (SortID == 6) success = success.OrderBy(x => x.ObjectDetails).ToList();
            if (SortID == -6) success = success.OrderByDescending(x => x.ObjectDetails).ToList();
            if (SortID == 7) success = success.OrderBy(x => x.LinkString).ToList();
            if (SortID == -7) success = success.OrderByDescending(x => x.LinkString).ToList();

            var model = new ImportPackageInfoViewModel
            {
                PackageID = e.PackageID,
                InstitutionName = e.Institution.BriefName,
                DateSent = e.CreateDate.ToString(),
                DateProcessing = e.StatusID == 3 ? e.LastDateChanged.ToString("dd.MM.yyyy HH:mm:ss") : "",
                Type = e.ImportPackageType.Name,
                Status = e.ImportPackageStatus.Name,
                ResultError = resultError,
                CountProcessed = countProcessed,
                CountNotProcessed = countNotProcessed,
                Comment = e.Comment,
                ErrorObjects = errors.ToArray(),
                SuccessObjects = success.ToArray(),
                Login = dbContext.ImportPackage.Where(x => x.PackageID == packageID).Select(x => x.UserLogin).SingleOrDefault(),
            };

            return new AjaxResultModel { Data = model };
        }

        /// <summary>
        /// Формируем результат проверки
        /// </summary>
        private static void GetCheckPackage(ImportedAppCheckResultPackage resultPackage, List<ImportPackageInfoViewModel.SuccessObjectsData> success, string prefix)
		{
			success.Add(new ImportPackageInfoViewModel.SuccessObjectsData
			            {
							ObjectType = prefix,
			            	ObjectDetails = "Cтатус проверки: " + resultPackage.StatusCheckCode + " " + resultPackage.StatusCheckMessage
			            });
			if (resultPackage.EgeDocumentCheckResults != null && resultPackage.GetEgeDocuments != null)
			{
				foreach (var resultDto in resultPackage.GetEgeDocuments)
				{
					if (resultDto.Application == null) continue;

					var suc = new ImportPackageInfoViewModel.SuccessObjectsData
					          {
								  ObjectType = prefix,
					          		ObjectDetails =
					          		"Заявление №" + resultDto.Application.ApplicationNumber + ", " +
					          		(resultDto.Error == null ? "Нет ошибок" : "Ошибка: " + resultDto.Error)
					          };
					GetEgeDocumentDto dto = resultDto;

                    //
                    int[] arrayInts = SQL.GetApplicationId(dto.Application.ApplicationNumber);

                    if(arrayInts.Any()) {
                        if(arrayInts[1] != 8) {
                            suc.SetLink<InstitutionApplicationController>(x => x.NavigateToList(arrayInts[0]));
                        }
                    }
					//suc.SetLink<InstitutionApplicationController>(x => x.ExtendedApplicationList(dto.Application.ApplicationNumber));
					success.Add(suc);
				}
			}
		}

		/// <summary>
		/// Детали о ошибках
		/// </summary>
		private static void GetFailDetails(List<ImportPackageInfoViewModel.ErrorObjectsData> errors, ImportResultPackage importResultPackage, DeleteResultPackage deleteResultPackage)
		{
			FailedImportInfoDto failDto;
            if (importResultPackage != null)
                failDto = importResultPackage.Log.Failed;
            else
                failDto = deleteResultPackage.Log.Failed;
			if (failDto.AdmissionVolumes != null)
				foreach (var item in failDto.AdmissionVolumes)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Объём приема";
					obj.ObjectDetails = item.EducationLevelName + ", " + item.DirectionName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.ApplicationCommonBenefits != null)
				foreach (var item in failDto.ApplicationCommonBenefits)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Общая льгота";
					obj.ObjectDetails = item.BenefitKindName + " для заявления №" + item.ApplicationNumber;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.Applications != null)
				foreach (var item in failDto.Applications)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Заявление";
					obj.ObjectDetails = "№" + item.ApplicationNumber;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

            if (failDto.ConsideredApplications != null)
                foreach (var item in failDto.ConsideredApplications)
                {
                    var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
                    obj.ObjectType = item.ConsideredApplication.GetDescription();
                    obj.ObjectDetails = item.ConsideredApplication.ToString();
                    obj.ErrorCode = item.ErrorInfo.ErrorCode;
                    obj.ErrorText = item.ErrorInfo.Message;
                    errors.Add(obj);
                }

            if (failDto.RecommendedApplications != null)
                foreach (var item in failDto.RecommendedApplications)
                {
                    var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
                    obj.ObjectType = item.RecommendedApplication.GetDescription();
                    obj.ObjectDetails = item.RecommendedApplication.ToString();
                    obj.ErrorCode = item.ErrorInfo.ErrorCode;
                    obj.ErrorText = item.ErrorInfo.Message;
                    errors.Add(obj);
                }

			if (failDto.CampaignDates != null)
				foreach (var item in failDto.CampaignDates)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Дата приемной кампании";
					obj.ObjectDetails = "UID " + item.UID;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.Campaigns != null)
				foreach (var item in failDto.Campaigns)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Приемная кампания";
					obj.ObjectDetails = item.Name;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.CommonBenefit != null)
				foreach (var item in failDto.CommonBenefit)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Общая льгота";
					obj.ObjectDetails = item.BenefitKindName + " для конкурса " + item.CompetitiveGroupName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.CompetitiveGroupItems != null)
				foreach (var item in failDto.CompetitiveGroupItems)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Направление конкурса";
					obj.ObjectDetails = "Код направления " + item.DirectionCode + ", конкурс " + item.CompetitiveGroupName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.CompetitiveGroups != null)
				foreach (var item in failDto.CompetitiveGroups)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Конкурс";
					obj.ObjectDetails = item.CompetitiveGroupName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.EntranceTestBenefits != null)
				foreach (var item in failDto.EntranceTestBenefits)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Льгота";
					obj.ObjectDetails = "Конкурс " + item.CompetitiveGroupName + ", тип льготы " + item.BenefitKindName + ", предмет " + item.SubjectName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.EntranceTestItems != null)
				foreach (var item in failDto.EntranceTestItems)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Вступительное испытание";
					obj.ObjectDetails = "Предмет " + item.SubjectName + ", конкурс " + item.CompetitiveGroupName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.EntranceTestResults != null)
				foreach (var item in failDto.EntranceTestResults)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Результат вступительного испытания";
					obj.ObjectDetails = "Предмет " + item.SubjectName + ", заявления № " + item.ApplicationNumber;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.OrdersOfAdmissions != null)
				foreach (var item in failDto.OrdersOfAdmissions)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Приказ";
                    obj.ObjectDetails = string.Format("UID: {0}", string.IsNullOrWhiteSpace(item.OrderUID) ? string.Empty : item.OrderUID);
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}
            if (failDto.ApplicationsInOrders != null)
                foreach (var item in failDto.ApplicationsInOrders)
                {
                    var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
                    obj.ObjectType = "Заявление";
                    obj.ObjectDetails = string.Format("UID: {0}", item.UID);
                    obj.ErrorCode = item.ErrorInfo.ErrorCode;
                    obj.ErrorText = item.ErrorInfo.Message;
                    errors.Add(obj);
                }

            if (failDto.TargetOrganizationDirections != null)
				foreach (var item in failDto.TargetOrganizationDirections)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Направление целевого приема";
					obj.ObjectDetails = item.DirectionName + ", организация " + item.TargetOrganizationName + ", конкурс " + item.CompetitiveGroupName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

			if (failDto.TargetOrganizations != null)
				foreach (var item in failDto.TargetOrganizations)
				{
					var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
					obj.ObjectType = "Организация целевого приема";
                    obj.ObjectDetails = item.TargetOrganizationName; //+ ", конкурс " + item.CompetitiveGroupName;
					obj.ErrorCode = item.ErrorInfo.ErrorCode;
					obj.ErrorText = item.ErrorInfo.Message;
					errors.Add(obj);
				}

            if (failDto.RecommendedLists != null)
                foreach (var item in failDto.RecommendedLists)
                {
                    var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
                    obj.ObjectType = "Список лиц, рекомендованных к зачислению";
                    obj.ObjectDetails = string.Format("Этап зачисления: {0}", item.Stage);
                    obj.ErrorCode = item.ErrorInfo.ErrorCode;
                    obj.ErrorText = item.ErrorInfo.Message;
                    errors.Add(obj);
                }

            if (failDto.DistributedAdmissionVolumes != null)
                foreach (var item in failDto.DistributedAdmissionVolumes)
                {
                    var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
                    obj.ObjectType = "Распределенный объем приема";
                    obj.ObjectDetails = item.AdmissionVolumeUID + ", " + item.LevelBudget;
                    obj.ErrorCode = item.ErrorInfo.ErrorCode;
                    obj.ErrorText = item.ErrorInfo.Message;
                    errors.Add(obj);
                }
            if (failDto.InstitutionAchievements != null)
                foreach (var item in failDto.InstitutionAchievements)
                {
                    var obj = new ImportPackageInfoViewModel.ErrorObjectsData();
                    obj.ObjectType = "Объём приема";
                    obj.ObjectDetails = item.IAUID + ", " + item.Name;
                    obj.ErrorCode = item.ErrorInfo.ErrorCode;
                    obj.ErrorText = item.ErrorInfo.Message;
                    errors.Add(obj);
                }
		}

		/// <summary>
		/// Общее количество ошибочных данных
		/// </summary>
		private static int GetFailCount(LogDto log)
		{
            var result = log.Failed.AdmissionVolumes.GetNullableArray().Select(t => new { t.DirectionName, t.EducationLevelName }).Distinct().Count();
			result += log.Failed.ApplicationCommonBenefits.GetNullableArray().Select(t=>  new {t.RegistrationDate, t.ApplicationNumber, t.BenefitKindName} ).Distinct().Count();
			result += log.Failed.Applications.GetNullableArray().Select(t=>  new {t.RegistrationDate, t.ApplicationNumber} ).Distinct().Count();
            result += log.Failed.ConsideredApplications.GetNullableArray().Select(t=>  new {t.ConsideredApplication} ).Distinct().Count();
            result += log.Failed.RecommendedApplications.GetNullableArray().Select(t=>  new {t.RecommendedApplication} ).Distinct().Count();
			result += log.Failed.CampaignDates.GetNullableArray().Select(t=>  new {t.UID} ).Distinct().Count();
			result += log.Failed.Campaigns.GetNullableArray().Select(t=>  new {t.Name} ).Distinct().Count();
			result += log.Failed.CommonBenefit.GetNullableArray().Select(t=>  new {t.BenefitKindName, t.CompetitiveGroupName} ).Distinct().Count();
			result += log.Failed.CompetitiveGroupItems.GetNullableArray().Select(t=>  new {t.CompetitiveGroupName, t.DirectionCode} ).Distinct().Count();
			result += log.Failed.CompetitiveGroups.GetNullableArray().Select(t=>  new {t.CompetitiveGroupName} ).Distinct().Count();
			result += log.Failed.EntranceTestBenefits.GetNullableArray().Select(t=>  new {t.BenefitKindName, t.CompetitiveGroupName, t.EntranceTestType, t.SubjectName} ).Distinct().Count();
			result += log.Failed.EntranceTestItems.GetNullableArray().Select(t=>  new {t.CompetitiveGroupName, t.EntranceTestType, t.SubjectName} ).Distinct().Count();
			result += log.Failed.EntranceTestResults.GetNullableArray().Select(t=>  new {t.RegistrationDate, t.ApplicationNumber, t.ResultSourceType, t.ResultValue, t.SubjectName} ).Distinct().Count();
			result += log.Failed.OrdersOfAdmissions.GetNullableArray().Select(t=>  new {t.OrderUID} ).Distinct().Count();
            result += log.Failed.ApplicationsInOrders.GetNullableArray().Select(t => new { t.UID }).Distinct().Count();
            result += log.Failed.TargetOrganizationDirections.GetNullableArray().Select(t=>  new {t.CompetitiveGroupName, t.DirectionName, t.EducationLevelName, t.TargetOrganizationName} ).Distinct().Count();
			result += log.Failed.TargetOrganizations.GetNullableArray().Select(t=>  new {t.TargetOrganizationName} ).Distinct().Count();
            result += log.Failed.RecommendedLists.GetNullableArray().Select(t=>  new {t.Stage} ).Distinct().Count();
            result += log.Failed.DistributedAdmissionVolumes.GetNullableArray().Select(t=>  new {t.AdmissionVolumeUID, t.LevelBudget} ).Distinct().Count();
            result += log.Failed.InstitutionAchievements.GetNullableArray().Select(t=>  new {t.IAUID, t.Name} ).Distinct().Count();
			return result;
		}

        /// <summary>
        /// Формируем результаты об успехе
        /// </summary>
        private static void GetSuccessDetails(ImportEntities dbContext,
            List<ImportPackageInfoViewModel.SuccessObjectsData> successes,
            //PackageData importPackage,
            GVUZ.ImportService2016.Core.Dto.Import.PackageData importPackage,
            ImportResultPackage resultPackage)
        {
            GetInstitution instId = new GetInstitution();
            if (importPackage.Applications != null)
            {
                foreach (var applicationDto in importPackage.Applications)
                {
                    if (resultPackage.Log.Failed.Applications != null)
                    {

                        if (resultPackage.Log.Failed.Applications.Any(y =>
                                (y.ApplicationNumber == applicationDto.ApplicationNumber && y.RegistrationDate == applicationDto.RegistrationDate.ToShortDateString())
                                || (y.UID == applicationDto.UID)
                                )
                            )
                            continue;
                    }

                    ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                    {
                        ObjectType = "Заявление",
                        ObjectDetails = "№" + applicationDto.ApplicationNumber
                    };

                    //ApplicationDto dto = applicationDto;
                    PackageDataApplication dto = applicationDto;

                    int[] arrayInts = SQL.GetApplicationId(dto.ApplicationNumber);

                    if (arrayInts.Any())
                    {
                        if (arrayInts[1] != 8)
                        {
                            sobj.SetLink<InstitutionApplicationController>(x => x.NavigateToList(arrayInts[0]));
                        }
                    }
                    //sobj.SetLink<InstitutionApplicationController>(x => x.ExtendedApplicationList(dto.ApplicationNumber));
                    successes.Add(sobj);
                }
            }


            if (importPackage.AdmissionInfo != null && importPackage.AdmissionInfo.AdmissionVolume != null)
            {
                var directions = dbContext.Direction.ToDictionary(x => x.DirectionID, x => x.Name);
                Func<string, string> getDirName = (dirID) => directions.ContainsKey(dirID.To(0)) ? directions[dirID.To(0)] : dirID;

                var admTypes = dbContext.AdmissionItemType.ToDictionary(x => x.ItemTypeID, x => x.Name);
                Func<string, string> getAdmType = (at) => admTypes.ContainsKey((short)at.To(0)) ? admTypes[(short)at.To(0)] : at;

                foreach (var avDto in importPackage.AdmissionInfo.AdmissionVolume)
                {
                    if (resultPackage.Log.Failed.AdmissionVolumes != null)
                    {
                        if (resultPackage.Log.Failed.AdmissionVolumes.Any(y => y.DirectionName == getDirName(avDto.DirectionID.ToString())
                            && y.EducationLevelName == getAdmType(avDto.EducationLevelID.ToString())))
                            continue;
                    }

                    ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                    {
                        ObjectType = "Объем приема",
                        ObjectDetails = getAdmType(avDto.EducationLevelID.ToString()) + ", " + getDirName(avDto.DirectionID.ToString())
                    };
                    sobj.SetLink<AdmissionController>(x => x.VolumeView(null, null));
                    successes.Add(sobj);
                }
            }
            int i = 0;
            if (importPackage.AdmissionInfo != null && importPackage.AdmissionInfo.CompetitiveGroups != null)
            {
                i = importPackage.AdmissionInfo.CompetitiveGroups.Count();
                string[] arrayCgSQL = new string[i];
                i = 0;
                foreach (var cgDtoSQL in importPackage.AdmissionInfo.CompetitiveGroups)
                {
                    arrayCgSQL[i] = cgDtoSQL.UID;
                    i++;
                }
                int[] arrayCg = SQL.GetIDFromUIDandInstId(arrayCgSQL, instId.GetInstitutionID(), i, "CompetitiveGroup", "CompetitiveGroupID");
                i = 0;
                foreach (var cgDto in importPackage.AdmissionInfo.CompetitiveGroups)
                {
                    if (resultPackage.Log.Failed.CompetitiveGroups != null)
                    {
                        if (resultPackage.Log.Failed.CompetitiveGroups.Any(y => y.CompetitiveGroupName == cgDto.Name))
                            continue;
                    }

                    ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                    {
                        ObjectType = "Конкурс",
                        ObjectDetails = cgDto.Name
                    };
                    if (arrayCg.Any())
                    {
                        if (arrayCg[i] != 0)
                        {
                            int IdForLink = arrayCg[i];
                            sobj.SetLink<CompetitiveGroupController>(x => x.CompetitiveGroupEdit(IdForLink));
                            i++;
                        }
                    }
                    successes.Add(sobj);
                }
            }

            i = 0;
            if (importPackage.CampaignInfo != null && importPackage.CampaignInfo.Campaigns != null)
            {
                i = importPackage.CampaignInfo.Campaigns.Count();
                string[] arrayCSQL = new string[i];
                i = 0;
                foreach (var CDtoSQL in importPackage.CampaignInfo.Campaigns)
                {
                    arrayCSQL[i] = CDtoSQL.UID;
                    i++;
                }
                int[] arrayC = SQL.GetIDFromUIDandInstId(arrayCSQL, instId.GetInstitutionID(), i, "Campaign", "CampaignId");
                i = 0;
                foreach (var cDto in importPackage.CampaignInfo.Campaigns)
                {
                    if (resultPackage.Log.Failed.Campaigns != null)
                    {
                        if (resultPackage.Log.Failed.Campaigns.Any(y => y.Name == cDto.Name))
                            continue;
                    }

                    ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                    {
                        ObjectType = "Приемная кампания",
                        ObjectDetails = cDto.Name,
                    };
                    if (arrayC.Any())
                    {
                        if (arrayC[i] != 0)
                        {
                            int IdForLink = arrayC[i];
                            sobj.SetLink<CampaignController>(x => x.CampaignEdit(IdForLink));
                            i++;
                        }

                    }
                    successes.Add(sobj);
                }
            }


            if (importPackage.Orders != null)
            {
                if (importPackage.Orders.OrdersOfAdmission != null)
                {
                    i = importPackage.Orders.OrdersOfAdmission.Count();
                    string[] arrayOASQL = new string[i];
                    i = 0;
                    foreach (var CDtoSQL in importPackage.Orders.OrdersOfAdmission)
                    {
                        arrayOASQL[i] = CDtoSQL.UID;
                        i++;
                    }
                    int[] arrayOA = SQL.GetIDFromUIDandInstId(arrayOASQL, instId.GetInstitutionID(), i, "OrderOfAdmission", "OrderId");
                    i = 0;
                    foreach (var aDto in importPackage.Orders.OrdersOfAdmission)
                    {
                        if (resultPackage.Log.Failed.OrdersOfAdmissions != null && resultPackage.Log.Failed.OrdersOfAdmissions.Any(y => y.OrderUID == aDto.UID)) continue;

                        ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                        {
                            ObjectType = "Приказ о зачислении",
                            ObjectDetails = string.Format("UID: {0}", aDto.OrderOfAdmissionUID)
                        };
                        if (arrayOA.Any())
                        {
                            if (arrayOA[i] != 0)
                            {
                                int IdForLink = arrayOA[i];
                                sobj.SetLink<OrderOfAdmissionController>(x => x.ViewOrder(IdForLink));
                                i++;
                            }
                        }
                        successes.Add(sobj);
                    }
                }
                if (importPackage.Orders.OrdersOfException != null)
                {
                    i = importPackage.Orders.OrdersOfException.Count();
                    string[] arrayOESQL = new string[i];
                    i = 0;
                    foreach (var AEtoSQL in importPackage.Orders.OrdersOfAdmission)
                    {
                        arrayOESQL[i] = AEtoSQL.UID;
                        i++;
                    }
                    int[] arrayOE = SQL.GetIDFromUIDandInstId(arrayOESQL, instId.GetInstitutionID(), i, "OrderOfAdmission", "OrderId");
                    i = 0;
                    foreach (var aDto in importPackage.Orders.OrdersOfException)
                    {
                        if (resultPackage.Log.Failed.OrdersOfAdmissions != null && resultPackage.Log.Failed.OrdersOfAdmissions.Any(y => y.OrderUID == aDto.UID)) continue;
                        ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                        {
                            ObjectType = "Приказ об исключении",
                            ObjectDetails = string.Format("UID: {0}", aDto.OrderOfExceptionUID)
                        };
                        if (arrayOE.Any())
                        {
                            if (arrayOE[i] != 0)
                            {
                                int IdForLink = arrayOE[i];
                                sobj.SetLink<OrderOfAdmissionController>(x => x.ViewOrder(IdForLink));
                                i++;
                            }
                        }
                        successes.Add(sobj);
                    }
                }
                if (importPackage.Orders.Applications != null)
                {
                    i = importPackage.Orders.Applications.Count();
                    string[] arrayOAppSQL = new string[i];
                    i = 0;
                    foreach (var OAPPtoSQL in importPackage.Orders.OrdersOfAdmission)
                    {
                        arrayOAppSQL[i] = OAPPtoSQL.UID;
                        i++;
                    }
                    int[] arrayOAPP = SQL.GetIDFromUIDandInstId(arrayOAppSQL, instId.GetInstitutionID(), i, "OrderOfAdmission", "OrderId");
                    i = 0;
                    foreach (var aDto in importPackage.Orders.Applications)
                    {
                        if (resultPackage.Log.Failed.ApplicationsInOrders != null && resultPackage.Log.Failed.ApplicationsInOrders.Any(y => y.UID == aDto.UID)) continue;
                        ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                        {
                            ObjectType = "Включение заявления в приказ",
                            ObjectDetails = string.Format("Application UID: {0}, Order UID: {1}", aDto.ApplicationUID, aDto.OrderUID)
                        };
                        if (arrayOAPP.Any())
                        {
                            if (arrayOAPP[i] != 0)
                            {
                                int IdForLink = arrayOAPP[i];
                                sobj.SetLink<OrderOfAdmissionController>(x => x.ViewOrder(IdForLink));
                                i++;
                            }
                        }
                        successes.Add(sobj);
                    }
                }
            }


            if (importPackage.AdmissionInfo != null && importPackage.AdmissionInfo.DistributedAdmissionVolume != null)
                foreach (var dav in importPackage.AdmissionInfo.DistributedAdmissionVolume)
                {
                    if (resultPackage.Log.Failed.DistributedAdmissionVolumes != null)
                    {
                        if (resultPackage.Log.Failed.DistributedAdmissionVolumes.Any(y => y.AdmissionVolumeUID == dav.AdmissionVolumeUID && y.LevelBudget == dav.LevelBudget.ToString()))
                            continue;
                    }

                    ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                    {
                        ObjectType = "Распределенный объем приема",
                        ObjectDetails = "AdmissionVolumeUID: " + dav.AdmissionVolumeUID + " Уровень бюджета:" + dav.LevelBudget
                    };
                    //sobj.SetLink<AdmissionController>(x => x.CompetitiveGroupList(null, null, null, null, null, null, null));
                    successes.Add(sobj);
                }

            if (importPackage.InstitutionAchievements != null)
                foreach (var ia in importPackage.InstitutionAchievements)
                {
                    if (resultPackage.Log.Failed.InstitutionAchievements != null)
                    {
                        if (resultPackage.Log.Failed.InstitutionAchievements.Any(y => y.IAUID == ia.InstitutionAchievementUID && y.Name == ia.Name))
                            continue;
                    }

                    ImportPackageInfoViewModel.SuccessObjectsData sobj = new ImportPackageInfoViewModel.SuccessObjectsData
                    {
                        ObjectType = "Индивидуальные достижения, учитываемые образовательной организацией",
                        ObjectDetails = "UID: " + ia.InstitutionAchievementUID + " наименование:" + ia.Name,
                    };
                    sobj.SetLink<InstitutionAchievementsController>(x => x.Index());
                    successes.Add(sobj);
                }

        }

        /// <summary>
        /// Общее количество успешных данных
        /// </summary>
        private static int GetSuccessCount(LogDto log)
        {
            var result = log.Successful.AdmissionVolumes.To(0);
            result += log.Successful.Applications.To(0);
            //result += log.Successful.ConsideredApplications.To(0);
            //result += log.Successful.RecommendedApplications.To(0);
            result += log.Successful.Campaigns.To(0);
            result += log.Successful.CompetitiveGroups.To(0);
            result += log.Successful.CompetitiveGroupItems.To(0);
            result += log.Successful.Orders.To(0);
            result += log.Successful.ApplicationsInOrders.To(0);
            result += log.Successful.DistributedAdmissionVolumes.To(0);
            result += log.Successful.InstitutionAchievements.To(0);
            return result;
        }

        private static int GetNullableLength<T>(this T[] arr)
        {
            return arr == null ? 0 : arr.Length;
        }

        private static List<T> GetNullableArray<T>(this T[] arr)
        {
            return arr == null ? new List<T>() : arr.ToList();
        }
    }
}