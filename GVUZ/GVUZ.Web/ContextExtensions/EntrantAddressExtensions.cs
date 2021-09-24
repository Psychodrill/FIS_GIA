using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Helpers;
using GVUZ.Web.Portlets.Entrants;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Адреса абитуриента. Сейчас не вводятся.
	/// </summary>
	public static class EntrantAddressExtensions
	{
		/// <summary>
		/// Загружаем адрес
		/// </summary>
		public static PersonalRecordsAddressViewModel FillPersonalAddress(this EntrantsEntities dbContext, PersonalRecordsAddressViewModel model, bool isView, EntrantKey key)
		{
//			int entrantID = key.GetEntrantID(dbContext, !isView);
//			if (entrantID == 0 && key.ApplicationID > 0)
//				model.ShowDenyMessage = true;
//			Entrant entrant = dbContext.Entrant
//			                           //.Include(x => x.AddressFact.CountryType)
//			                           //.Include(x => x.AddressFact.RegionType)
//			                           //.Include(x => x.AddressReg)
//			                           //.Include(x => x.AddressReg.CountryType)
//			                           //.Include(x => x.AddressReg.RegionType)
//                                       .FirstOrDefault(x => x.EntrantID == entrantID);

//			if (isView && entrant == null)
//				throw new ArgumentException("EntrantID is required");

//			if (entrant != null)
//			{
//				model.Mobile = entrant.MobilePhone;
//				model.Email = entrant.Email;
//				model.EntrantID = entrant.EntrantID;
//				if (entrant.RegistrationAddressID != null)
//					//SetPersonAddressFieldsToModel(model.RegistrationAddress, entrant.AddressReg);
//				if (entrant.FactAddressID != null)
//				{
//					model.IsOnlyRegistration = false;
//					//SetPersonAddressFieldsToModel(model.FactAddress, entrant.AddressFact);
//				}
//				else
//				{
//					model.IsOnlyRegistration = true;
//					//if (entrant.AddressReg != null)
//					//	SetPersonAddressFieldsToModel(model.FactAddress, entrant.AddressReg);
//				}

//				if (!isView)
//				{
//					List<DictionaryItem> countryList = dbContext.GetCountries();
//					countryList.Insert(0, new DictionaryItem { ID = 0, Name = "[Не указано]" });
//					model.CountryList = countryList;
//					int countryID = countryList[1].ID;
//					bool hasRegion = countryList[1].HasChild;
//					if (entrant.AddressReg != null)
//					{
//						if (entrant.AddressReg.CountryType != null)
//						{
//							countryID = entrant.AddressReg.CountryType.CountryID;
//							hasRegion = entrant.AddressReg.CountryType.HasRegions;
//						}
//						else
//						{
//							countryID = 0;
//							hasRegion = false;
//						}
//					}
//					else
//					{
//						//для новых делаем россию
//						model.RegistrationAddress.CountryID = countryID;
//					}

//					model.RegistrationAddress.CountryHasRegions = hasRegion ? 1 : 0;
//					model.RegistrationRegionsList = dbContext.GetRegions(countryID);
					
//					if (!model.IsOnlyRegistration)
//					{
//						countryID = countryList[0].ID;
//						hasRegion = countryList[0].HasChild; 
//						if (entrant.AddressFact != null)
//						{
//							if (entrant.AddressFact.CountryType != null)
//							{
//								countryID = entrant.AddressFact.CountryType.CountryID;
//								hasRegion = entrant.AddressFact.CountryType.HasRegions;
//							}
//							else
//							{
//								countryID = 0;
//								hasRegion = false;
//							}
//						}

//						model.FactAddress.CountryHasRegions = hasRegion ? 1 : 0;
//						model.FactRegionsList = dbContext.GetRegions(countryID);
//					}
//					else
//					{
//						model.FactRegionsList = model.RegistrationRegionsList;
//						model.FactAddress.CountryHasRegions = hasRegion ? 1 : 0;
//						model.FactAddress.CountryID = countryID;
//					}
//				}

//				if (model.ApplicationID > 0) //подаём заявление, нужно проставить общежитие
//				{
//					bool? needHostel =
//						dbContext.Application.Where(x => x.ApplicationID == model.ApplicationID).Select(x => x.NeedHostel).Single();
//					if (needHostel.HasValue)
//						model.HostelRequired = needHostel.Value;
//				}
//			}

			return model;
		}

//		/// <summary>
//		/// Заполняем модель из базы
//		/// </summary>
//		private static void SetPersonAddressFieldsToModel(PersonalRecordsAddressViewModel.Address model, Address address)
//		{
//			model.AddressID = address.AddressID;
//			model.PostalCode = address.PostalCode;
//			model.CountryID = address.CountryID ?? 0;
//			model.CountryName = address.CountryName;
//			model.CountryHasRegions = address.CountryType != null && address.CountryType.HasRegions ? 1 : 0;
//			model.RegionID = address.RegionID;
			
//#warning Отключил arzyanin            
//            //model.RegionName = model.RegionID != null ? address.RegionType.Name : address.RegionName;
//            model.RegionName = address.RegionName;
//			model.CityName = address.CityName;
//			model.Street = address.Street;
//			model.Building = address.Building;
//			model.BuildingPart = address.BuildingPart;
//			model.Room = address.Room;
//			model.Phone = address.Phone;
//		}

//		/// <summary>
//		/// Заполняем базу из модели
//		/// </summary>
//		private static void SetPersonAddressModelToFields(EntrantsEntities dbContext, 
//			PersonalRecordsAddressViewModel.Address model, Address address)
//		{
//			address.PostalCode = model.PostalCode;
//			if (model.CountryID > 0)
//			{
//				if (address.CountryID != model.CountryID)
//                    address.CountryName = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType).First(x => x.Key == model.CountryID).Value;
//				address.CountryID = model.CountryID;
//			}
//			else
//			{
//				address.CountryName = null;
//				address.CountryID = null;
//			}

//			address.RegionID = model.RegionID == 0 ? null : model.RegionID;
//			address.RegionName = model.RegionName;
//			address.CityName = model.CityName;
//			address.Street = model.Street;
//			address.Building = model.Building;
//			address.BuildingPart = model.BuildingPart;
//			address.Room = model.Room;
//			address.Phone = model.Phone;
//		}

//		/// <summary>
//		/// Сохраняем адрес
//		/// </summary>
//		public static AjaxResultModel SavePersonalAddress(this EntrantsEntities dbContext, PersonalRecordsAddressViewModel model, UserInfo userInfo)
//		{
//			var currentData = SavePersonalAddressInternal(dbContext, model, model.EntrantID);
//			if (currentData.IsError) return currentData;
//			if (model.ApplicationID > 0)
//			{
//				int origEntrantID =
//					dbContext.Application.Where(x => x.ApplicationID == model.ApplicationID).Select(x => x.EntrantID).FirstOrDefault();
//				//ставим те же данные, что и у текущего #30337
//				if (origEntrantID > 0 && origEntrantID != model.EntrantID)
//					SavePersonalAddressInternal(dbContext, model, origEntrantID);
//			}

//			return currentData;
//		}

//		/// <summary>
//		/// Сохраняем адрес
//		/// </summary>
		private static AjaxResultModel SavePersonalAddressInternal(EntrantsEntities dbContext, PersonalRecordsAddressViewModel model, int entrantID)
		{
//			if (entrantID <= 0)
//				throw new ArgumentException("EntrantID is required");

//			//bool isEdit = model.EntrantID > 0;

//			Entrant entrant = dbContext.Entrant
//				.Include(x => x.AddressFact)
//				.Include(x => x.AddressReg)
//				.Where(x => x.EntrantID == entrantID).FirstOrDefault();
//			if (entrant == null)
//				return new AjaxResultModel("Не найдена запись об абитуриенте");

//			if (entrant.RegistrationAddressID == null)
//			{
//				entrant.AddressReg = new Address();
//				dbContext.Address.AddObject(entrant.AddressReg);
//			}

//			SetPersonAddressModelToFields(dbContext, model.RegistrationAddress, entrant.AddressReg);

//			if (!model.IsOnlyRegistration)
//			{
//				if (entrant.FactAddressID == null)
//				{
//					entrant.AddressFact = new Address();
//					dbContext.Address.AddObject(entrant.AddressFact);
//				}

//				SetPersonAddressModelToFields(dbContext, model.FactAddress, entrant.AddressFact);
//			}
//			else
//			{
//				if (entrant.FactAddressID != null)
//				{
//					dbContext.Address.DeleteObject(entrant.AddressFact);
//					entrant.AddressFact = null;
//				}
//			}

//			entrant.MobilePhone = model.Mobile;
//			entrant.Email = model.Email;

//			if (model.ApplicationID > 0) //подаём заявление, нужно проставить общежитие
//			{
//				Application application = dbContext.GetApplication(model.ApplicationID);
//				application.NeedHostel = model.HostelRequired;
//			}

//			dbContext.SaveChanges();

			return new AjaxResultModel();
		}
	}
}