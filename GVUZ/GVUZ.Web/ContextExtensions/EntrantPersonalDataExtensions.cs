using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Cache;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Web.Models;
using GVUZ.Web.Portlets.Entrants;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с личными данными абитуриентов
	/// </summary>
	public static class EntrantPersonalDataExtensions
	{
		/// <summary>
		/// Загружаем модель с персональными данными
		/// </summary>
		public static PersonalRecordsDataViewModel FillPersonalData(this EntrantsEntities dbContext, PersonalRecordsDataViewModel model, bool isView, EntrantKey key)
		{
			// для редактирования ещё и справочники
			if (!isView)
			{
                model.GenderList = new[] {new
			                            {
			                                ID = GenderType.Male,
                                            Name = GenderType.GetName(GenderType.Male)
			                            },
                                        new
			                            {
			                                ID = GenderType.Female,
                                            Name = GenderType.GetName(GenderType.Female)
			                            }
                };

                model.NationalityList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType).Select(x => new { ID = x.Key, Name = x.Value });
                model.IdentityDocumentList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.IdentityDocumentType).Select(x => new { ID = x.Key, Name = x.Value });
					 model.RussianDocs = dbContext.IdentityDocumentType.Where(x => x.IsRussianNationality).Select(x => x.IdentityDocumentTypeID).ToArray();
			}

            Entrant entrant = key.GetEntrant(dbContext, !isView); //теперь можно и исторических мучать
            if (entrant == null && key.ApplicationID > 0)
				model.ShowDenyMessage = true;
			if (key.ApplicationID > 0)
				model.ApplicationID = key.ApplicationID;
			
            if (isView && entrant == null)
				throw new ArgumentException("EntrantID is required");

			if (entrant != null)
			{
				model.EntrantID = entrant.EntrantID;
				model.Entrant.LastName = entrant.LastName;
				model.Entrant.FirstName = entrant.FirstName;
				model.Entrant.MiddleName = entrant.MiddleName;
				model.Entrant.GenderID = entrant.GenderID;
                
				if (entrant.EntrantDocument_Identity != null)
				{
					IdentityDocumentViewModel model2 =
						new JavaScriptSerializer().Deserialize<IdentityDocumentViewModel>(
							entrant.EntrantDocument_Identity.DocumentSpecificData);
					EntrantDocumentExtensions.ConvertDatesToLocal(model2);

					if (entrant.IdentityDocumentID.HasValue)
						model.IdentityDocumentID = entrant.IdentityDocumentID.Value;

					model.BirthDate = model2.BirthDate;
					//#29011 Дата рождения абитуриента
					if (model.BirthDate.Date == DateTime.MinValue.Date) model.BirthDate = new DateTime(DateTime.Now.Year - 17, 1, 1);
					bool canEditDocument = !dbContext.ApplicationEntrantDocument.Any(x => x.EntrantDocumentID == model.IdentityDocumentID && model.ApplicationID != x.ApplicationID);
					if (canEditDocument) //документ ни к чему ещё не прицеплен, можно изменить
					{
						model.ForceAddData = true;
						//а почему бы и не поменять
						//model.DisableDocumentDataEditing = true; //серию и номер не позволяем менять
					}

					model.DocumentTypeID = model2.IdentityDocumentTypeID;
					model.DocumentSeries = model2.DocumentSeries;
					model.DocumentNumber = model2.DocumentNumber;
					model.DocumentOrganization = model2.DocumentOrganization;
					model.DocumentDate = model2.DocumentDate;
					model.SubdivisionCode = model2.SubdivisionCode;
					if (entrant.EntrantDocument_Identity.Attachment != null)
					{
						model.DocumentAttachmentID = entrant.EntrantDocument_Identity.Attachment.FileID.Value;
						model.DocumentAttachmentName = entrant.EntrantDocument_Identity.Attachment.Name;
					}

					model.NationalityID = model2.NationalityTypeID;
					model.BirthPlace = model2.BirthPlace;
					model.Entrant.GenderID = model2.GenderTypeID;
					if (isView)
					{
                        model.NationalityName = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType).Where(x => x.Key == model2.NationalityTypeID).Select(x => x.Value).FirstOrDefault();

                        model.IdentityDocumentName = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.IdentityDocumentType)
							.Where(x => x.Key == model2.IdentityDocumentTypeID).Select(x => x.Value).First();
					}
				}
				else
				{
					model.ForceAddData = true;
				}

				model.CustomInformation = entrant.CustomInformation;
				// если есть папа/мама загружаем их тоже
				if (isView)
				{
				    model.GenderName = GenderType.GetName(model.Entrant.GenderID);
				}

                var app = EntrantCacheManager.GetFirst<Application>(x => x.EntrantID == entrant.EntrantID);
                if (app == null)
                {
                    app = dbContext.Application.FirstOrDefault(x => x.EntrantID == entrant.EntrantID);
                    EntrantCacheManager.Add(app.ApplicationID, app);
                }

			    model.NeedHostel = app == null ? false : (app.NeedHostel?? false);


				dbContext.AddEntrantAccessToLog(entrant, "PersonalData");
			}
			else
			{
				// если с ПГУ, то у нас есть такие данные
				model.Entrant.LastName = key.UserInfo.SURNAME;
				model.Entrant.FirstName = key.UserInfo.NAME;
				model.Entrant.MiddleName = key.UserInfo.PATRONYMIC;
			}

			return model;
		}

		/// <summary>
		/// Сохраняем персональные данные абитуриента
		/// </summary>
		public static AjaxResultModel SavePersonalData(this EntrantsEntities dbContext, PersonalRecordsDataViewModel model, EntrantKey key)
		{
			var currentData = SavePersonalDataInternal(dbContext, model, model.EntrantID);
			if (currentData.IsError) return currentData;
			if (key.ApplicationID > 0)
			{
                int origEntrantID = EntrantCacheManager.Get<Application>(key.ApplicationID).EntrantID;
					
				//ставим те же данные, что и у текущего #30337
				if (origEntrantID > 0 && origEntrantID != model.EntrantID)
					SavePersonalDataInternal(dbContext, model, origEntrantID);
			}

			return currentData;
		}

		/// <summary>
		/// Внутренее сохранение данных абитуриента
		/// </summary>
		private static AjaxResultModel SavePersonalDataInternal(EntrantsEntities dbContext, PersonalRecordsDataViewModel model, int entrantID)
		{
			bool isEdit = entrantID > 0;

			Entrant entrant;
			//если есть абитуриент, то загружаем его, иначе создаём нового
			if (isEdit)
			{
			    entrant = dbContext.Entrant.FirstOrDefault(x => x.EntrantID == entrantID);
                    //EntrantCacheManager.Get<Entrant>(entrantID);
                if (entrant == null)
                {
                //    entrant = dbContext.Entrant.FirstOrDefault(x => x.EntrantID == entrantID);
             //       if (entrant == null)
                        return new AjaxResultModel("Не найдена запись об абитуриенте");

                    //EntrantCacheManager.Add(entrantID, entrant); // WTF ?
                }
			}
			else
			{
                entrant = new Entrant();
				dbContext.Entrant.AddObject(entrant);
			}

			if (model.ApplicationStep != ApplicationStepType.ParentData) //нам данные не пришли на шаге родителей, мы их не меняем
			{
				var oldPersonData = dbContext.PrepareEntrantOldData(entrant);

				entrant.LastName = (model.Entrant.LastName ?? "").Trim();
				entrant.FirstName = (model.Entrant.FirstName ?? "").Trim();
				entrant.MiddleName = model.Entrant.MiddleName != null ? model.Entrant.MiddleName.Trim() : null;
				entrant.GenderID = model.Entrant.GenderID;
				entrant.CustomInformation = model.CustomInformation;
                dbContext.AddEntrantAccessToLog(oldPersonData, entrant, "SavePersonalData", false);
			}

			IdentityDocumentViewModel id;
			bool isReadonlyDoc = false;
			//загружаем ДУЛ, если надо
			if (isEdit && entrant.IdentityDocumentID.HasValue)
			{
				id = (IdentityDocumentViewModel)dbContext.LoadEntrantDocument(entrant.IdentityDocumentID.Value);
				//если к чему-то прицеплен, запрещаем редактировать
				if (
                    dbContext.ApplicationEntrantDocument.Any(
						x => x.EntrantDocumentID == id.EntrantDocumentID && model.ApplicationID != x.ApplicationID)
					/*|| model.Entrant.GenderID == 0*/)
					isReadonlyDoc = true;
			}
			else
			{
                id = new IdentityDocumentViewModel
                {
                    DocumentNumber = model.DocumentNumber,
                    DocumentSeries = model.DocumentSeries,
                    IdentityDocumentTypeID = model.DocumentTypeID,
                    EntrantID = entrant.EntrantID
                };
                entrant.EntrantDocument_Identity = dbContext.CreateEntrantDocument(id, entrant);
			}
			//на других шагах документ не вводится и не редактируется
			if (model.ApplicationStep != ApplicationStepType.PersonalData)
				isReadonlyDoc = true;

			if (isEdit && entrant.IdentityDocumentID.HasValue)
				id.EntrantDocumentID = entrant.IdentityDocumentID.Value;

            entrant.EntrantDocument_Identity.DocumentDate = id.DocumentDate = model.DocumentDate;
            id.SubdivisionCode = model.SubdivisionCode;
            entrant.EntrantDocument_Identity.DocumentNumber = id.DocumentNumber = model.DocumentNumber;
            entrant.EntrantDocument_Identity.DocumentSeries = id.DocumentSeries = model.DocumentSeries;
            id.IdentityDocumentTypeID = model.DocumentTypeID;

            entrant.EntrantDocument_Identity.DocumentOrganization = id.DocumentOrganization = model.DocumentOrganization;
            id.NationalityTypeID = model.NationalityID;
            id.DocumentAttachmentID = model.DocumentAttachmentID;
            id.BirthDate = model.BirthDate;
            id.BirthPlace = model.BirthPlace;
            id.NationalityTypeID = model.NationalityID;
            id.GenderTypeID = model.Entrant.GenderID;

			if (!isReadonlyDoc)
			{
				var msd = new ModelStateDictionary();
				id.Validate(msd, entrant.InstitutionID ?? 0);
				if (!msd.IsValid)
					return new AjaxResultModel(msd);
			}

			if (!isEdit) entrant.SNILS = null;

			//создаём родителей, если на этом шаге
			if (model.ApplicationStep != ApplicationStepType.PersonalData)
			{
				//CreatePersonParents(dbContext, model, isEdit, entrant);
			}

			dbContext.SaveChanges();
			if (!isEdit || !entrant.IdentityDocumentID.HasValue)
				//иначе у нас уже есть документ и смысла в установке данных по абитуриенту нет
			{
				id.EntrantID = entrant.EntrantID;
				dbContext.SaveEntrantDocument(id, null);
				entrant.EntrantDocument_Identity = dbContext.EntrantDocument.First(x => x.EntrantID == entrant.EntrantID);
				dbContext.SaveChanges();
			}
			else
			{
				if (!isReadonlyDoc)
				{
                    entrant.EntrantDocument_Identity.DocumentSpecificData = new JavaScriptSerializer().Serialize(id);
                    dbContext.SaveEntrantDocument(id, null);
				}
			}

            EntrantCacheManager.Add(entrant.EntrantID, entrant);

			return new AjaxResultModel();
		}
	}
}