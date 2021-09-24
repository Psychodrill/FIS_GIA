using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Applications;
using GVUZ.Model.Cache;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.Model.RecommendedLists;
using GVUZ.Web.Portlets.Applications;
using GVUZ.Web.Portlets.Entrants;
using GVUZ.Web.ViewModels;
using System.Data.Objects;

using Application=GVUZ.Model.Entrants.Application;
using GVUZ.Model.ApplicationPriorities;
using Microsoft.Practices.ServiceLocation;
using Entrant=GVUZ.Model.Entrants.Entrant;

namespace GVUZ.Web.ContextExtensions {
	/// <summary>
	/// Методы работы с заявлениями
	/// </summary>
	public static class EntrantApplicationExtensions {
		/// <summary>
		/// Загружаем заявеление на просмотр
		/// </summary>
		public static ApplicationViewModel FillApplicationView(this EntrantsEntities dbContext,ApplicationViewModel model,UserInfo userInfo,int applicationID) {
			model.CanView=true;

			if(userInfo.SNILS!=null) {
				string snils=dbContext.ApplicationWhere(applicationID).Select(x => x.Entrant.SNILS).Single();
				if(!snils.Equals(userInfo.SNILS)) {
					model.CanView=false;
					model.DenyMessage="Доступ запрещен: чужое заявление";
				}
			}

			int statusID=dbContext.ApplicationWhere(applicationID).Select(x => x.StatusID).Single();
			if(statusID==ApplicationStatusType.Draft) {
				model.CanView=false;
				model.DenyMessage="Доступ запрещен: заявление еще не отправлено";
			}

			return model;
		}

		/// <summary>
		/// Загружаем общую информацию по заявлению на просмотр
		/// </summary>
		public static ApplicationCommonInfoViewModel FillApplicationCommonInfo(this EntrantsEntities dbContext,ApplicationCommonInfoViewModel model,UserInfo userInfo,int applicationID) {
			Application application=dbContext.ApplicationWhere(applicationID)
					 .Include("Institution")
					 .Include("ApplicationStatusType")
					 .Include("ViolationType")
					 .Include("CompetitiveGroup")
					 .Include("CompetitiveGroupItem")
					 .Include("ApplicationCompetitiveGroupItem")
					 .Include("ApplicationSelectedCompetitiveGroup")
					 .Include("ApplicationSelectedCompetitiveGroupItem")
					 .Include("ApplicationSelectedCompetitiveGroup.CompetitiveGroup")
					 .Include("ApplicationSelectedCompetitiveGroupItem.CompetitiveGroupItem")
					 .Include("ApplicationSelectedCompetitiveGroupItem.CompetitiveGroupItem.Direction")
				.FirstOrDefault();

			if(application==null)
				throw new ArgumentException("Application is required");
            
			model.Status=application.ApplicationStatusType.Name;
			model.Violation=application.ViolationType.Name;
			// если есть решение, добавляем его
			if(!String.IsNullOrEmpty(application.StatusDecision))
				model.Violation+=" ("+application.StatusDecision+")";

			model.IsVUZ=application.Institution.InstitutionTypeID==(short)InstitutionType.VUZ;
			model.Institution=application.Institution.FullName;

			var course=application.CompetitiveGroup.Course;

			model.Course=CompetitiveGroupExtensions.GetCourseName(course);

			FillBaseModelFromCompetitiveGroup(dbContext,application,model);

			//для приказа одна КГ
			if(application.StatusID==ApplicationStatusType.InOrder) {
				var groupInfo=new ApplicationCommonInfoViewModel.CompetitiveGroupInfo();
				int competitiveGroupID=application.CompetitiveGroup.CompetitiveGroupID;
				groupInfo.CompetitiveGroupName=application.CompetitiveGroup.Name;
				groupInfo.CompetitiveGroupID=competitiveGroupID;

				// GVUZ-564 заполняем общее кол-во мест в КГ и вычисляем Конкурс в КГ				
				groupInfo.Places=dbContext.vCompetitiveGroup
					.Where(x => x.CompetitiveGroupID==competitiveGroupID)
					.Select(CountPlacesInCompetitiveGroup)
					.Sum();
				int? appCount=dbContext
					.Application
					.Include(x => x.ApplicationStatusType)
					.Count(x => x.OrderCompetitiveGroupID==competitiveGroupID&&x.ApplicationStatusType.IsActiveApp);
				groupInfo.Requests=appCount.GetValueOrDefault();
				groupInfo.Competition=groupInfo.Places>0?Math.Round((decimal)groupInfo.Requests/groupInfo.Places,2):0;

				// вывести количество баллов, когда будет готов ввод результатов вступительных испытаний (http://qa.fogsoft.ru/browse/GVUZ-276)
				//вот эти баллы 
				groupInfo.Points=application.OrderCalculatedRating.ToString();
				model.CompetitiveGroups=new[] { groupInfo };
			} else //для не приказов может быть несколько
			{
				List<ApplicationCommonInfoViewModel.CompetitiveGroupInfo> resList=new List<ApplicationCommonInfoViewModel.CompetitiveGroupInfo>();

				foreach(var ascg in application.ApplicationCompetitiveGroupItem) {
					var groupInfo=new ApplicationCommonInfoViewModel.CompetitiveGroupInfo();

					int competitiveGroupID=ascg.CompetitiveGroup.CompetitiveGroupID;
					groupInfo.CompetitiveGroupName=ascg.CompetitiveGroup.Name;
					groupInfo.CompetitiveGroupID=competitiveGroupID;

					// GVUZ-564 заполняем общее кол-во мест в КГ и вычисляем Конкурс в КГ				
					groupInfo.Places=dbContext.vCompetitiveGroup
						.Where(x => x.CompetitiveGroupID==competitiveGroupID)
						.Select(CountPlacesInCompetitiveGroup)
						.Sum();

					int? appCount=dbContext
				 .Application
				 .Include(x => x.ApplicationStatusType)
                 .Include(x => x.CompetitiveGroup)
                 .Count(x => x.OrderCompetitiveGroupID==competitiveGroupID && x.ApplicationStatusType.IsActiveApp);

					groupInfo.Requests=appCount.GetValueOrDefault();
					groupInfo.Competition=groupInfo.Places>0?Math.Round((decimal)groupInfo.Requests/groupInfo.Places,2):0;
					groupInfo.Points=ascg.CalculatedRating.ToString();

					resList.Add(groupInfo);
				}

				model.CompetitiveGroups=resList.ToArray();
			}

			return model;
		}

		/// <summary>
		/// Загружаем данные из КГ для заявления
		/// </summary>
		private static void FillBaseModelFromCompetitiveGroup(EntrantsEntities dbContext,Application application,ApplicationInfoViewModelBase model) {
			if(string.IsNullOrEmpty(model.Course)) {
				//model.Course=CompetitiveGroupExtensions.GetCourseName(dbContext.ApplicationSelectedCompetitiveGroup
				//		 .Where(x => x.ApplicationID==application.ApplicationID).Select(x => x.CompetitiveGroup.Course).FirstOrDefault());
			}

			Model.Entrants.CompetitiveGroupItem[] competitiveGroupItems=null;
			if(application.ApplicationSelectedCompetitiveGroupItem==null) {
				competitiveGroupItems=dbContext.ApplicationSelectedCompetitiveGroupItem.Where(x => x.ApplicationID==application.ApplicationID)
					 .Select(x => x.CompetitiveGroupItem).ToArray();
			} else {
				competitiveGroupItems=application.ApplicationSelectedCompetitiveGroupItem.Select(c => c.CompetitiveGroupItem).ToArray();
			}


			var edLevels=competitiveGroupItems.Select(x => x.CompetitiveGroup.EducationLevelID).ToArray().Distinct()
			.Select(x => DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel,x.Value));
			model.EducationLevel=String.Join(", ",edLevels);

			// если в приказе - берем конкретные значения для направлений и условий обучения				
			if(application.StatusID==ApplicationStatusType.InOrder) {
				if(application.CompetitiveGroup.CompetitiveGroupItem==null)
					throw new InvalidOperationException("Не задан конкурс.");
				if(application.OrderEducationFormID==null)
					throw new InvalidOperationException("Не задана форма обучения.");
				if(application.OrderEducationSourceID==null)
					throw new InvalidOperationException("Не задан источник финансирования.");

				model.Direction=application.CompetitiveGroup.Direction.Name;
				if(application.OrderEducationSourceID.Value!=16)
					model.EducationalFormList=DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.Study,application.OrderEducationFormID.Value)
							+" - "+
							DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.AdmissionType,application.OrderEducationSourceID.Value);
				else {
					string orgName=dbContext.CompetitiveGroupTarget
						.Where(x => x.CompetitiveGroupTargetID==application.OrderCompetitiveGroupTargetID).Select(x => x.Name)
						.FirstOrDefault();
                    if (orgName == string.Empty || orgName.IsDbNullOrNull())
                    {
                         orgName = dbContext.CompetitiveGroupTarget
						.Where(x => x.CompetitiveGroupTargetID == application.OrderCompetitiveGroupTargetID).Select(x => x.ContractOrganizationName)
						.FirstOrDefault();

                    }


                    if (orgName!=null) orgName=" ("+orgName+")";
					else orgName="";
					model.EducationalFormList=DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.AdmissionType,
																						 application.OrderEducationSourceID.Value)+orgName;
				}
			} else {
				ApplicationPrioritiesViewModel priorities=null;
				using(var prioritiesContext=new ApplicationPrioritiesEntities()) {
					priorities=prioritiesContext.FillExistingPriorities(application.ApplicationID);
				}

				model.EducationalFormList=string.Join("; ",priorities.ApplicationPriorities.Where(x => x.Priority.HasValue).Select(x => x.ToString()));
				//GetEducationalFormList(dbContext, application);

				if(application.ApplicationSelectedCompetitiveGroupItem==null) {
					model.Direction=string.Join(", ",from p in dbContext.ApplicationSelectedCompetitiveGroupItem
																join c in dbContext.CompetitiveGroupItem on p.CompetitiveGroupItemID equals c.CompetitiveGroupItemID
																join d in dbContext.Direction on c.CompetitiveGroup.DirectionID equals d.DirectionID
																where p.ApplicationID==application.ApplicationID
																orderby d.Name
																select d.Name);
				} else {
					model.Direction=string.Join(", ",from c in application.ApplicationSelectedCompetitiveGroupItem
																let cgi=c.CompetitiveGroupItem
																where cgi!=null
																let d=cgi.CompetitiveGroup.Direction
																where d!=null
																select d.Name);
				}
			}
		}

		/// <summary>
		/// Формируем текстовые формы обучения для заявления
		/// </summary>
		public static string GetEducationalFormList(this EntrantsEntities dbContext,Application app) {
			var l=new List<string>();
			Type t=typeof(ApplicationSendingViewModel.EducationForms);
			Action<string> addName=x =>
											l.Add(((DisplayNameAttribute)
											Attribute.GetCustomAttribute(t.GetProperty(x),typeof(DisplayNameAttribute)))
											.DisplayName);

			if(app.IsRequiresBudgetO) addName("BudgetO");
			if(app.IsRequiresBudgetOZ) addName("BudgetOZ");
			if(app.IsRequiresBudgetZ) addName("BudgetZ");
			if(app.IsRequiresPaidO) addName("PaidO");
			if(app.IsRequiresPaidOZ) addName("PaidOZ");
			if(app.IsRequiresPaidZ) addName("PaidZ");
			if(app.IsRequiresTargetO) {
				var orgName=
					dbContext.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.ApplicationID==app.ApplicationID&&x.IsForO)
						.Select(x => x.CompetitiveGroupTarget.Name).FirstOrDefault();
				if(orgName!=null) orgName=" ("+orgName+")";
				else orgName="";
				addName("TargetO");
				l[l.Count-1]=l[l.Count-1]+orgName;
			}

			if(app.IsRequiresTargetOZ) {
				var orgName=
					dbContext.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.ApplicationID==app.ApplicationID&&x.IsForOZ)
						.Select(x => x.CompetitiveGroupTarget.Name).FirstOrDefault();
				if(orgName!=null) orgName=" ("+orgName+")";
				else orgName="";
				addName("TargetOZ");
				l[l.Count-1]=l[l.Count-1]+orgName;
			}

			if(app.IsRequiresTargetZ) {
				var orgName=
					dbContext.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.ApplicationID==app.ApplicationID&&x.IsForZ)
						.Select(x => x.CompetitiveGroupTarget.Name).FirstOrDefault();
				if(orgName!=null) orgName=" ("+orgName+")";
				else orgName="";
				addName("TargetZ");
				l[l.Count-1]=l[l.Count-1]+orgName;
			}

			return String.Join(", ",l.ToArray());
		}

		/// <summary>
		/// Считаем общее количество мест в КГ
		/// </summary>
		private static int CountPlacesInCompetitiveGroup(vCompetitiveGroup cg) {
			return cg.NumberBudgetO.GetValueOrDefault()+cg.NumberBudgetOZ.GetValueOrDefault()+cg.NumberBudgetZ.GetValueOrDefault()+
				cg.NumberPaidO.GetValueOrDefault()+cg.NumberPaidOZ.GetValueOrDefault()+cg.NumberPaidZ.GetValueOrDefault()+
				cg.NumberTargetO.GetValueOrDefault()+cg.NumberTargetOZ.GetValueOrDefault()+cg.NumberTargetZ.GetValueOrDefault();
		}

		private class CompetitiveGroupItem {
			public int CompetitiveGroupItemID { get; set; }
			public int CompetitiveGroupID { get; set; }
			public int? CampaignID { get; set; }
			public short? EducationLevelID { get; set; }
			public int? DirectionID { get; set; }

			public short Course { get; set; }

			public short? OrderEducationSourceID { get; set; }

			public int StatusID { get; set; }
		}

		static CompetitiveGroupItem[] GetCurrentCompetitiveGroupItem(EntrantsEntities dbContext,int applicationId) {
			return (from app in dbContext.Application
					  join ac in dbContext.ApplicationSelectedCompetitiveGroup on app.ApplicationID equals
							ac.ApplicationID
					  join cg in dbContext.CompetitiveGroup on ac.CompetitiveGroupID equals
							cg.CompetitiveGroupID

					  join left in dbContext.CompetitiveGroupItem on cg.CompetitiveGroupID equals left.CompetitiveGroupID

					  join ag in dbContext.ApplicationSelectedCompetitiveGroupItem on
									new { ApplicationID=app.ApplicationID,CompetitiveGroupItemID=left.CompetitiveGroupItemID } equals new { ApplicationID=ag.ApplicationID,CompetitiveGroupItemID=ag.CompetitiveGroupItemID }

					  where app.ApplicationID==applicationId
					  select new CompetitiveGroupItem {
						  StatusID=app.StatusID,
						  OrderEducationSourceID=app.OrderEducationSourceID,
						  CompetitiveGroupID=cg.CompetitiveGroupID,
						  CampaignID=cg.CampaignID,
						  EducationLevelID=(short?)left.CompetitiveGroup.EducationLevelID,
						  DirectionID=(int?)left.CompetitiveGroup.DirectionID,
						  Course=cg.Course,
						  CompetitiveGroupItemID=left.CompetitiveGroupItemID
					  }
											).ToArray();
		}

		/// <summary>
		/// Заполняем модель проверки сведений у заявления
		/// </summary>
		public static ApplicationSendingViewModel FillApplicationSending(this EntrantsEntities dbContext,ApplicationSendingViewModel model,PersonalRecordsDataViewModel dataModel,UserInfo userInfo,int applicationID) {
			EntrantKey key=new EntrantKey(applicationID,userInfo);

			int entrantID=key.GetEntrantID(dbContext,false);
			if(entrantID==0&&key.ApplicationID>0)
				model.ShowDenyMessage=true;

			dbContext.FillPersonalData(dataModel,true,key);
			model.FIO=string.Format("{0} {1} {2}",dataModel.Entrant.LastName,dataModel.Entrant.FirstName,dataModel.Entrant.MiddleName);

			model.DOB=dataModel.BirthDate.ToString("d MMMM yyyy 'г.'");

			model.IdentityDocument=dataModel.IdentityDocumentName+": "+dataModel.DocumentSeries+" "
				 +dataModel.DocumentNumber
				 +(!String.IsNullOrWhiteSpace(dataModel.DocumentOrganization)?(" выдан: "+dataModel.DocumentOrganization):"")
				 +" "+
				 (dataModel.DocumentDate.HasValue?dataModel.DocumentDate.Value.ToString("d.MM.yyyy 'г.'"):"");
			model.Gender=dataModel.GenderName;
			model.Citizen=dataModel.NationalityName;
			model.POB=dataModel.BirthPlace;

			var applicationSelectedCompetitiveGroupTarget=dbContext.ApplicationSelectedCompetitiveGroupTarget
				 .Where(x => x.ApplicationID==applicationID)
				 .Select(x => new { x.IsForO,x.IsForOZ,x.IsForZ,x.CompetitiveGroupTargetID })
				 .ToArray();

			var competitiveGroup=GetCurrentCompetitiveGroupItem(dbContext,applicationID);
			var application=(from app in dbContext.Application
								  join ins in dbContext.Institution on app.InstitutionID equals ins.InstitutionID
								  where app.ApplicationID==applicationID
								  select new {
									  app,
									  app.ApplicationID,
									  app.UID,
									  app.RegistrationDate,
									  app.StatusID,
									  app.ApproveInstitutionCount,
									  app.NeedHostel,
									  app.FirstHigherEducation,
									  app.ApprovePersonalData,
									  app.InstitutionID,
									  app.FamiliarWithLicenseAndRules,
									  app.FamiliarWithAdmissionType,
									  app.FamiliarWithOriginalDocumentDeliveryDate,
									  app.IsRequiresBudgetO,
									  app.IsRequiresBudgetOZ,
									  app.IsRequiresBudgetZ,
									  app.IsRequiresPaidO,
									  app.IsRequiresPaidOZ,
									  app.IsRequiresPaidZ,
									  app.IsRequiresTargetO,
									  app.IsRequiresTargetOZ,
									  app.IsRequiresTargetZ,
									  InstitutionFullName=ins.FullName,
									  ins.InstitutionTypeID,
									  app.Priority
								  }
									).FirstOrDefault();


			model.Uid=application.UID;
			model.RegistrationDate=application.RegistrationDate;
			//если есть данные по приказу, то грузим сведения по КГ
			if(competitiveGroup.Any())
				FillBaseModelFromCompetitiveGroup(dbContext,application.app,model);

			model.CustomInformation=dataModel.CustomInformation;

			//#30129
			// берём дату первого попавшегося документа, поскольку нужно одну только выводить, да и по факту почти всегда один бывает
			var edDocuments=new[] { 3,4,5,6,7,8,16,19 };

			var firstDoc=(from doc in dbContext.ApplicationEntrantDocument
							  join ed in dbContext.EntrantDocument on doc.EntrantDocumentID equals ed.EntrantDocumentID
							  where doc.ApplicationID==application.ApplicationID&&doc.OriginalReceivedDate!=null
									  &&edDocuments.Contains(ed.DocumentTypeID)
							  select new { doc.OriginalReceivedDate }).FirstOrDefault();

			model.EducationDocumentDate=firstDoc!=null
				 ?firstDoc.OriginalReceivedDate.Value.ToString("d.MM.yyyy")
				 :"";

			model.ApproveInstitutionCount=application.ApproveInstitutionCount??false;
			model.NeedHostel=application.NeedHostel??false;
			model.FirstHigherEducation=application.FirstHigherEducation??false;
			model.ApprovePersonalData=application.ApprovePersonalData??false;
			model.FamiliarWithLicenseAndRules=application.FamiliarWithLicenseAndRules??false;
			model.FamiliarWithAdmissionType=application.FamiliarWithAdmissionType??false;
			model.FamiliarWithOriginalDocumentDeliveryDate=application.FamiliarWithOriginalDocumentDeliveryDate??false;

			if(competitiveGroup.Any()) {
				model.CampaignID=competitiveGroup.First().CampaignID??0;
			}

			model.SelectedCompetitiveGroupIDs=
				 competitiveGroup.Select(x => x.CompetitiveGroupID).Distinct().ToArray();

			model.SelectedDirectionIDs=competitiveGroup.Where(x => x.DirectionID.HasValue)
													  .Select(x => new { x.EducationLevelID,DirectionID=x.DirectionID.Value,x.CompetitiveGroupItemID })
													  .Distinct()
													  .Select(x => string.Format("{0}@{1}@{2}",x.EducationLevelID,x.DirectionID,x.CompetitiveGroupItemID)).ToArray();

			model.EducationFormsSelected=new ApplicationSendingViewModel.EducationForms();

			//вытягиваем доступные КГ (с количеством мест где-нибудь)
			var competitiveGroupBaseQuery=dbContext.CompetitiveGroup.Where(x => x.InstitutionID==application.InstitutionID
				 &&x.CompetitiveGroupItem.Sum(y =>
					  y.NumberBudgetO
							+y.NumberBudgetOZ
							+y.NumberBudgetZ
							+y.NumberPaidO
							+y.NumberPaidOZ
							+y.NumberPaidZ
							+(y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)(z.NumberTargetO+z.NumberTargetOZ+z.NumberTargetZ))??0))>0);

#warning Исправление по выводу КГ по СПО
			competitiveGroupBaseQuery=competitiveGroupBaseQuery.Where(x => x.EntranceTestItemC.Any()||
				 x.CompetitiveGroupItem.Any(c => c.CompetitiveGroup.EducationLevelID==EDLevelConst.SPO));

			var competitiveGroups=competitiveGroupBaseQuery
					  .OrderBy(x => x.Name)
					  .Select(x => new {
						  x.CampaignID,
						  x.CompetitiveGroupID,
						  x.Name,
						  x.Course,
						  HasO=x.CompetitiveGroupItem.Sum(y => y.NumberBudgetO+y.NumberPaidO+(y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)z.NumberTargetO)??0))>0,
						  HasOZ=x.CompetitiveGroupItem.Sum(y => y.NumberBudgetOZ+y.NumberPaidOZ+(y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)z.NumberTargetOZ)??0))>0,
						  HasZ=x.CompetitiveGroupItem.Sum(y => y.NumberBudgetZ+y.NumberPaidZ+(y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)z.NumberTargetZ)??0))>0
					  });

			model.CompetitiveGroupNamesByCampaign=competitiveGroups
				 .GroupBy(x => x.CampaignID)
				 .ToDictionary(x => x.Key??0,
					  x => (IEnumerable)x.Select(y => new { ID=y.CompetitiveGroupID,y.Name,y.Course }).ToArray());

			var res=dbContext.GetEducationFormsForCompetitiveGroups(model.SelectedCompetitiveGroupIDs,
				 model.SelectedDirectionIDs,application.InstitutionID);

			dbContext.Connection.Close();

			using(ApplicationPrioritiesEntities prioritiesContext=new ApplicationPrioritiesEntities()) {
				var Priorities=prioritiesContext.FillExistingPriorities(application.ApplicationID);
				model.Priorities=Priorities;
			}

			dbContext.Connection.Open();

			var edForms=(ApplicationSendingViewModel.EducationForms)res.Data;
			model.EducationFormsAvailable=edForms;

			model.TargetOrganizations=edForms.TargetOrganizationsO;

			model.Campaigns=
				 dbContext.Campaign.Where(x => x.InstitutionID==application.InstitutionID).Select(x => new { ID=x.CampaignID,Name=x.Name })
					  .ToArray().Where(x => model.CompetitiveGroupNamesByCampaign.ContainsKey(x.ID)).ToArray();

			model.SelectedTargetOrganizationIDO=
				 applicationSelectedCompetitiveGroupTarget.Where(x => x.IsForO).Select(x => x.CompetitiveGroupTargetID).
					  FirstOrDefault();
			model.SelectedTargetOrganizationIDOZ=
				 applicationSelectedCompetitiveGroupTarget.Where(x => x.IsForOZ).Select(x => x.CompetitiveGroupTargetID).
					  FirstOrDefault();
			model.SelectedTargetOrganizationIDZ=
				 applicationSelectedCompetitiveGroupTarget.Where(x => x.IsForZ).Select(x => x.CompetitiveGroupTargetID).
					  FirstOrDefault();
			model.ApplicationID=application.ApplicationID;
			model.IsVUZ=application.InstitutionTypeID==(short)InstitutionType.VUZ;
			model.Institution=application.InstitutionFullName;
			model.IsDraft=application.StatusID==ApplicationStatusType.Draft;
			model.Priority=application.Priority;

			//ставим допустимые флажки по формам
			model.EducationFormsSelected.BudgetO=application.IsRequiresBudgetO&&model.EducationFormsAvailable.BudgetO;
			model.EducationFormsSelected.BudgetOZ=application.IsRequiresBudgetOZ&&model.EducationFormsAvailable.BudgetOZ;
			model.EducationFormsSelected.BudgetZ=application.IsRequiresBudgetZ&&model.EducationFormsAvailable.BudgetZ;
			model.EducationFormsSelected.PaidO=application.IsRequiresPaidO&&model.EducationFormsAvailable.PaidO;
			model.EducationFormsSelected.PaidOZ=application.IsRequiresPaidOZ&&model.EducationFormsAvailable.PaidOZ;
			model.EducationFormsSelected.PaidZ=application.IsRequiresPaidZ&&model.EducationFormsAvailable.PaidZ;
			model.EducationFormsSelected.TargetO=application.IsRequiresTargetO&&model.EducationFormsAvailable.TargetO;
			model.EducationFormsSelected.TargetOZ=application.IsRequiresTargetOZ&&model.EducationFormsAvailable.TargetOZ;
			model.EducationFormsSelected.TargetZ=application.IsRequiresTargetZ&&model.EducationFormsAvailable.TargetZ;
			dbContext.AddApplicationAccessToLog(application.app,"ApplicationSending");
			return model;
		}

		////public static ApplicationEntrantDocumentsViewModel FillApplicationDocumentList(this EntrantsEntities dbContext, ApplicationEntrantDocumentsViewModel model, bool isView, int applicationID)
		////{
		////    var sw = new Stopwatch();
		////    sw.Start();

		////    var app = dbContext.GetApplication(applicationID);

		////    var entrantsData = dbContext.Application.Where(x => (x.Entrant.EntrantDocument_Identity.DocumentSeries.Equals(
		////        app.Entrant.EntrantDocument_Identity.DocumentSeries) || String.IsNullOrEmpty(x.Entrant.EntrantDocument_Identity.DocumentSeries)) &&
		////        x.Entrant.EntrantDocument_Identity.DocumentNumber.Equals(
		////        app.Entrant.EntrantDocument_Identity.DocumentNumber)
		////        )
		////        .Select(x => new
		////        {
		////            x.EntrantID,
		////            x.ApplicationStatusType.StatusID,
		////            x.OriginalDocumentsReceived,
		////            x.InstitutionID,
		////            ApplicationID = x.ApplicationID,
		////            Application = x
		////        }).Distinct().ToArray();

		////    if (entrantsData.Length == 0)
		////    {
		////        model.ShowDenyMessage = true;
		////        return model;
		////    }

		////    List<DocumentShortInfoViewModel> allEntrantDocuments = new List<DocumentShortInfoViewModel>();

		////    foreach (var entrantData in entrantsData)
		////    {
		////        int entrantID = entrantData.EntrantID;
		////        var q = dbContext.EntrantDocument
		////                         .Include(x => x.DocumentType)
		////                         .Include(x => x.Attachment)
		////                         .Where(x => x.EntrantID == entrantID)
		////                         .OrderBy(x => x.DocumentTypeID)
		////                         .Select(x => new DocumentShortInfoViewModel
		////                         {
		////                             DocumentAttachmentID = x.AttachmentID != null ? x.Attachment.FileID : Guid.Empty,
		////                             DocumentAttachmentName = x.AttachmentID != null ? x.Attachment.Name : null,
		////                             DocDate = x.DocumentDate,
		////                             DocNumber = x.DocumentNumber,
		////                             DocTypeID = x.DocumentTypeID,
		////                             DocSpecificData = x.DocumentSpecificData,
		////                             DocumentTypeName = x.DocumentType.Name,
		////                             DocumentOrganization = x.DocumentOrganization,
		////                             DocSeries = x.DocumentSeries,
		////                             EntrantDocumentID = x.EntrantDocumentID,
		////                             //можно открепить, если не используется в данном заявлении где-нибудь еще
		////                             CanBeDetached = !(x.Entrant_IdentityDocument.Any(u => u.EntrantID == app.EntrantID)
		////                                               ||
		////                                               x.ApplicationEntranceTestDocument.Any(
		////                                                   y => y.ApplicationID == app.ApplicationID)),
		////                             //можно редактировать, если не используется в ВИ данного или другого заявления
		////                             CanBeModified = !x.ApplicationEntranceTestDocument.Any(),
		////                             //если используется в другом заявлении, показываем ворнинг
		////                             ShowWarnBeforeModifying =
		////                                 x.ApplicationEntrantDocument.Any(y => y.ApplicationID != entrantData.ApplicationID)
		////                         }).ToArray();

		////        foreach (var doc in q)
		////            doc.FillData();
		////        allEntrantDocuments.AddRange(q.Where(x => allEntrantDocuments.All(y => y.EntrantDocumentID != x.EntrantDocumentID)));

		////        var idDocsToLog = q.Where(x => x.DocTypeID == 1)
		////                           .Select(x => new
		////                           {
		////                               x.DocDate,
		////                               x.DocNumber,
		////                               x.DocTypeID,
		////                               x.DocumentTypeName,
		////                               x.DocumentOrganization,
		////                               x.DocSeries,
		////                           })
		////                           .ToArray();
		////        if (idDocsToLog.Length > 0)
		////            dbContext.AddApplicationAccessToLog(entrantData.Application, "ViewDocuments");
		////    }

		////    var attachedDocs = dbContext.ApplicationEntrantDocument.Where(x =>
		////            x.Application.ApplicationID == app.ApplicationID)
		////                                    .ToArray();
		////    //прикреплённые - которые прикреплены или нельзя открепить
		////    model.AttachedDocuments =
		////        allEntrantDocuments.Where(x => attachedDocs.Any(y => y.EntrantDocumentID == x.EntrantDocumentID) ||
		////            !x.CanBeDetached).ToList();
		////    //откреплённые все остальные
		////    model.ExistingDocuments =
		////        allEntrantDocuments.Where(x => attachedDocs.All(y => y.EntrantDocumentID != x.EntrantDocumentID) &&
		////            x.CanBeDetached && x.DocTypeID != 1).ToList();

		////    //ставим флажки об оригиналах у прикреплённых документов
		////    foreach (var docModel in model.AttachedDocuments)
		////    {
		////        var fd = attachedDocs.Where(x => x.EntrantDocumentID == docModel.EntrantDocumentID).FirstOrDefault();
		////        if (fd != null)
		////        {
		////            docModel.OriginalReceivedDate = fd.OriginalReceivedDate.HasValue
		////                                                ? fd.OriginalReceivedDate.Value.ToString("dd.MM.yyyy")
		////                                                : null;
		////            if (fd.OriginalReceivedDate.HasValue)
		////                docModel.OriginalReceived = true;
		////        }

		////        if (docModel.DocTypeID == 1) //ДУЛ
		////            docModel.CanNotSetReceived = true;
		////    }
		////    model.EntrantID = app.EntrantID;
		////    model.DocumentTypes = dbContext.DocumentType
		////                                       .OrderBy(x => x.Name)
		////                                       .Select(x => new ApplicationEntrantDocumentsViewModel.DocumentType
		////                                       {
		////                                           TypeID = x.DocumentID,
		////                                           Name = x.Name
		////                                       }).ToArray();
		////    model.ApplicationIncludedInOrder = app.StatusID == ApplicationStatusType.InOrder;
		////    model.ApplicationID = applicationID;
		////    model.ApplicationStatus = app.StatusID;

		////    sw.Stop();
		////    var el = sw.Elapsed.TotalMilliseconds;

		////    return model;
		////}

		public static ApplicationEntrantDocumentsViewModel FillApplicationDocumentList(this EntrantsEntities dbContext,ApplicationEntrantDocumentsViewModel model,bool isView,int applicationId) {
			//var sw = new Stopwatch();
			//sw.Start();

			var app=dbContext.GetApplication(applicationId);
			var entrantsIds=dbContext.Application
				 .Where(x => x.InstitutionID==app.InstitutionID)   //Юсупов: убрал по просьбе Назара // Вернул обратно
				 .Where(x => (
							x.Entrant.EntrantDocument_Identity.DocumentSeries.Equals(app.Entrant.EntrantDocument_Identity.DocumentSeries)||String.IsNullOrEmpty(x.Entrant.EntrantDocument_Identity.DocumentSeries))&&
							x.Entrant.EntrantDocument_Identity.DocumentNumber.Equals(app.Entrant.EntrantDocument_Identity.DocumentNumber)).Select(c => c.EntrantID).Distinct().ToArray();

			var alldocuments=dbContext.EntrantDocument
				 .Include("Entrant_IdentityDocument")
				 .Include("ApplicationEntrantDocument")
				 .Include("ApplicationEntranceTestDocument")
				 .Include("DocumentType")
				 .Include("Attachment")
				 .Where(x => entrantsIds.Contains(x.EntrantID.Value)).ToArray();

			if(entrantsIds.Length==0) {
				model.ShowDenyMessage=true;
				return model;
			}

			/* Документы используемые в нескольких заявлениях */
			var crossedDocumentsIds=(from c in alldocuments
											 group c by c.EntrantDocumentID into g
											 let items=new {
												 DocumentId=g.Key,
												 ApplicationsCount=g.Count()
											 }
											 where items.ApplicationsCount>1
											 select items.DocumentId).ToArray();

			var allEntrantDocuments=new List<DocumentShortInfoViewModel>();
			var documentsByEntrants=alldocuments.GroupBy(x => x.EntrantID).ToArray();

			foreach(var documents in documentsByEntrants) {
				var q=documents.OrderBy(c => c.DocumentTypeID).Select(x => new DocumentShortInfoViewModel {
					DocumentAttachmentID=x.AttachmentID!=null?x.Attachment.FileID.Value:Guid.Empty,
					DocumentAttachmentName=x.AttachmentID!=null?x.Attachment.Name:null,
					DocDate=x.DocumentDate,
					DocNumber=x.DocumentNumber,
					DocTypeID=x.DocumentTypeID,
					DocSpecificData=x.DocumentSpecificData,
					DocumentTypeName=x.DocumentType.Name,
					DocumentOrganization=x.DocumentOrganization,
					DocSeries=x.DocumentSeries,
					EntrantDocumentID=x.EntrantDocumentID,

					//можно открепить, если не используется в данном заявлении где-нибудь еще
					CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),

					//можно редактировать, если не используется в ВИ данного или другого заявления
					CanBeModified=!x.ApplicationEntranceTestDocument.Any(),

					//если используется в другом заявлении, показываем ворнинг
					ShowWarnBeforeModifying=crossedDocumentsIds.Contains(x.EntrantDocumentID),

                    //(Реестр требований 72) можно удалить если не используется в данном заявлении или в других заявлениях
                    //CanBeDeleted = false,
                    //Походу не используется

                }).ToArray();

				foreach(var doc in q)
					doc.FillData();
				allEntrantDocuments.AddRange(q.Where(x => allEntrantDocuments.All(y => y.EntrantDocumentID!=x.EntrantDocumentID)));

				var idDocsToLog=q.Where(x => x.DocTypeID==1)
										 .Select(x => new {
											 x.DocDate,
											 x.DocNumber,
											 x.DocTypeID,
											 x.DocumentTypeName,
											 x.DocumentOrganization,
											 x.DocSeries,
										 })
										 .ToArray();
				if(idDocsToLog.Length>0)
					dbContext.AddApplicationAccessToLog(app,"ViewDocuments");
			}

			var attachedDocs=new List<ApplicationEntrantDocument>();
			attachedDocs.AddRange(alldocuments.Where(c => c.ApplicationEntrantDocument.Any(x => x.ApplicationID==app.ApplicationID))
				 .Aggregate(new List<ApplicationEntrantDocument>(),(n,x) => {
					 n.AddRange(x.ApplicationEntrantDocument);
					 return n;
				 }));

			//прикреплённые - которые прикреплены или нельзя открепить
			model.AttachedDocuments=allEntrantDocuments.Where(x => attachedDocs.Any(y => y.EntrantDocumentID==x.EntrantDocumentID)||!x.CanBeDetached).ToList();
			//откреплённые все остальные
			model.ExistingDocuments=
				 allEntrantDocuments.Where(x => attachedDocs.All(y => y.EntrantDocumentID!=x.EntrantDocumentID)&&
					  x.CanBeDetached&&x.DocTypeID!=1).ToList();

			//ставим флажки об оригиналах у прикреплённых документов
			foreach(var docModel in model.AttachedDocuments) {
				var fd=attachedDocs.FirstOrDefault(x => x.EntrantDocumentID==docModel.EntrantDocumentID&&x.OriginalReceivedDate.HasValue);

				if(fd!=null) {
					docModel.OriginalReceivedDate=fd.OriginalReceivedDate.HasValue?fd.OriginalReceivedDate.Value.ToString("dd.MM.yyyy"):null;
					if(fd.OriginalReceivedDate.HasValue)
						docModel.OriginalReceived=true;
				}

				if(docModel.DocTypeID==1) //ДУЛ
					docModel.CanNotSetReceived=true;
			}
			model.EntrantID=app.EntrantID;
			model.DocumentTypes=DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType).Select(c =>
												 new ApplicationEntrantDocumentsViewModel.DocumentType {
													 TypeID=c.Key,
													 Name=c.Value
												 }).OrderBy(c => c.Name);

			model.ApplicationIncludedInOrder=app.StatusID==ApplicationStatusType.InOrder;
			model.ApplicationID=applicationId;
			model.ApplicationStatus=app.StatusID;

			//sw.Stop();
			//var el = sw.Elapsed.TotalMilliseconds;

			return model;
		}


#warning Похоже, это не используется
#warning added Решение по доработке добавления уровня образования - часть доделать здесь.
        /// <summary>
		/// Проверка на необходимые документы у заявления
		/// </summary>
		// при изменениии логики следить за ObjectIntergiryManager
		public static string CheckRequiredDocuments(this EntrantsEntities dbContext,int applicationID,int[] loadedTypes) {
			var res=dbContext.ApplicationSelectedCompetitiveGroupItem.Where(x => x.ApplicationID==applicationID)
				.Select(x => new { x.CompetitiveGroupItem.CompetitiveGroup.Course,x.CompetitiveGroupItem.CompetitiveGroup.EducationLevelID }).ToArray();

			bool hasSpo=res.Any(x => x.EducationLevelID==EDLevelConst.SPO); // наличие СПО
			bool hasVpo1Course=
				res.Any(x => x.Course==1&&
					(x.EducationLevelID==EDLevelConst.Bachelor || 
					 x.EducationLevelID==EDLevelConst.Speciality));
			bool hasVpo2Course=res.Any(x => x.Course>1&&
					(x.EducationLevelID==EDLevelConst.Bachelor || 
					 x.EducationLevelID==EDLevelConst.Speciality));
			bool hasMag=res.Any(x => x.EducationLevelID==EDLevelConst.Magistracy);
			bool hasHiQual=res.Any(x => x.EducationLevelID==EDLevelConst.HighQualification);
             
			Func<int[],string> checkDoc2=arr => {
				if(!loadedTypes.Intersect(arr).Any()) {
					string docTypes=String.Join("\r\n ",
												 DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType).Where(x => arr.Contains(x.Key)).OrderBy(x => x.Value)
															.Select(x => x.Value).ToArray());
					return docTypes;
				}

				return null;
			};

			var errors=new List<string>();
			// в зависимости от того, где абитуриент проверяем один из списка документов
			if(hasSpo)
				errors.Add(checkDoc2(new[] { 16,3,4,5,6,19 }));
			if(hasVpo1Course)
				errors.Add(checkDoc2(new[] { 3,4,5,6,7,19 }));
			if(hasVpo2Course)
				errors.Add(checkDoc2(new[] { 4,7,8,19 }));
			if(hasMag)
				errors.Add(checkDoc2(new[] { 4,19 }));
			if(hasHiQual)
				errors.Add(checkDoc2(new[] { 25,26,4 }));
			errors=errors.Where(x => x!=null).ToList();
			if(errors.Count==0) return null;
			return "Заявление должно содержать один из следующих документов об образовании:\r\n "
					 +String.Join("\r\nА также один из следующих\r\n",errors);
		}

		/// <summary>
		/// Проверяем количество ДУЛов
		/// </summary>
		public static string CheckIdentityDocuments(this EntrantsEntities dbContext,int applicationID,int[] identityDocumentIDs) {
			if(identityDocumentIDs.Length>1) {
				var docs=dbContext.EntrantDocument.Where(x => identityDocumentIDs.Contains(x.EntrantDocumentID)).ToArray();
				var modelDocs=docs.Select(
						doc => (IdentityDocumentViewModel)new JavaScriptSerializer().Deserialize(doc.DocumentSpecificData,typeof(IdentityDocumentViewModel)));
				if(modelDocs.GroupBy(x => x.IdentityDocumentTypeID).Any(x => x.Count()>1))
					return "Нельзя прикреплять к заявлению несколько удостоверяющих личность документов одного вида";
			}

			return null;
		}

		/// <summary>
		/// Ставим флаг про оригиналы документов
		/// </summary>
		public static AjaxResultModel SetDocumentReceived(this EntrantsEntities dbContext,int institutionID,int applicationID,int entrantDocumentID,bool documentsReceived,DateTime? receivedDate) {
			var ed=dbContext.ApplicationEntrantDocument.FirstOrDefault(x => x.ApplicationID==applicationID
																									&&x.Application.InstitutionID==institutionID
																									&&x.EntrantDocumentID==entrantDocumentID);
			if(ed!=null) {
				if(documentsReceived)
					ed.OriginalReceivedDate=receivedDate??DateTime.Today;
				else
					ed.OriginalReceivedDate=null;
				dbContext.SaveChanges();
				UpdateAppDocumentReceived(dbContext,applicationID);
			}

			return new AjaxResultModel();
		}

		/// <summary>
		/// Обновляем дату оригиналов и флаг у заявления
		/// </summary>
		internal static void UpdateAppDocumentReceived(this EntrantsEntities dbContext,int applicationID) {
			Application app=dbContext.GetApplication(applicationID);
			if(app==null) return;
			int[] receivedCheckDocumentTypes=new[] { 3,4,5,6,7,8,16,19,25,26 }; /* DEBUG, 18 Временно для иных документов об образовании*/
			var receivedDate=
				dbContext.ApplicationEntrantDocument.Where(
					x => x.ApplicationID==applicationID&&x.OriginalReceivedDate!=null&&receivedCheckDocumentTypes.Contains(x.EntrantDocument.DocumentTypeID))
				.Select(x => x.OriginalReceivedDate).FirstOrDefault();
			if(receivedDate==null) {
				app.OriginalDocumentsReceived=false;
				app.OriginalDocumentsReceivedDate=null;
			} else {
				app.OriginalDocumentsReceived=true;
				app.OriginalDocumentsReceivedDate=receivedDate.Value;
			}
			dbContext.SaveChanges();
		}

#warning Похоже, это не используется
		/// <summary>
		/// Сохраняем список документов у заявления
		/// </summary>
		public static ActionResult SaveApplicationDocumentList(this EntrantsEntities dbContext,ApplicationEntrantDocumentsViewModel model,UserInfo userInfo,int applicationID,int institutionID) {
			// поддельные данные. Ничего не делаем
			if(!dbContext.Application.Any(x => x.ApplicationID==applicationID&&x.InstitutionID==institutionID))
				return new AjaxResultModel("");

			if(model.AttachedDocumentIDs==null)
				model.AttachedDocumentIDs=new int[0];
			var loadedTypesAndDocs=dbContext.EntrantDocument
				.Where(x => model.AttachedDocumentIDs.Contains(x.EntrantDocumentID))
				.Select(x => new { x.DocumentTypeID,x.EntrantDocumentID }).ToArray();

			string errorRequiredDocuments=CheckRequiredDocuments(dbContext,applicationID,loadedTypesAndDocs.Select(x => x.DocumentTypeID).Distinct().ToArray());

			string errorIdentityDocuments=CheckIdentityDocuments(dbContext,applicationID,
																					 loadedTypesAndDocs
																	.Where(x => x.DocumentTypeID==1)
																	.Select(x => x.EntrantDocumentID).ToArray());
			string combinedError=(!String.IsNullOrEmpty(errorRequiredDocuments)?errorRequiredDocuments+"\r\n":"")+
										  (errorIdentityDocuments??"");
			if(String.IsNullOrEmpty(combinedError)) combinedError=null;

			//разрешаем уходить назад, даже если не все документы загружены
			//только для редактируемых заявлений
			Application application=dbContext.GetApplication(applicationID);
			if(application.StatusID==ApplicationStatusType.Draft) {
				if(model.StepDirection=="back"||model.StepDirection=="save"||model.StepDirection=="refresh"||model.StepDirection=="norefresh")
					combinedError=null;
			}

			if(combinedError!=null)
				return new AjaxResultModel(combinedError);

			var array=dbContext.ApplicationEntrantDocument.Where(x => x.ApplicationID==applicationID).ToArray();
			//удаляем удалённые из базы
			foreach(var doc in array.Where(x => !model.AttachedDocumentIDs.Contains(x.EntrantDocumentID)).ToArray()) {
				dbContext.ApplicationEntrantDocument.DeleteObject(doc);

			}
			//записываем новые объекты в базу
			foreach(var documentID in model.AttachedDocumentIDs) {
				if(array.All(x => x.EntrantDocumentID!=documentID))
					dbContext.ApplicationEntrantDocument.AddObject(new ApplicationEntrantDocument {
						EntrantDocumentID=documentID,
						ApplicationID=applicationID
					});
			}

			dbContext.SaveChanges();
			return new AjaxResultModel("");
		}

		/// <summary>
		/// Список заявлений по абитуриенту
		/// </summary>
		public static ApplicationListViewModel FillApplicationList(this EntrantsEntities dbContext,ApplicationListViewModel model,UserInfo userInfo) {
			model.Applications=dbContext.Application
				.Where(x => x.Entrant.SNILS==userInfo.SNILS)
				.Select(x => new ApplicationListViewModel.ApplicationData {
					ApplicationID=x.ApplicationID,
					InstitutionName=x.Institution.FullName,
					RegistrationDate=x.RegistrationDate,
					StatusID=x.StatusID,
					StatusName=x.ApplicationStatusType.Name,
				})
				.ToArray();
			return model;
		}

		/// <summary>
		/// Сохраняем подачу заявления (галочки, выбранные КГ)
		/// </summary>
		public static ActionResult SaveApplicationsChecks(this EntrantsEntities dbContext, ApplicationSendingViewModel model,UserInfo userInfo,int applicationID,bool saveInconsistentData) {
			Application application=dbContext.Application
				.Include(x => x.OrderCompetitiveGroupID)
				.Include(x => x.ApplicationSelectedCompetitiveGroupItem)
				.Include(x => x.ApplicationSelectedCompetitiveGroupTarget)
				.Include(x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
				.Single(x => x.ApplicationID==applicationID);

			application.UID=model.Uid;

			if(model.Uid!=null&&dbContext.Application.Any(x =>
					 x.InstitutionID==application.InstitutionID&&x.UID==model.Uid&&x.ApplicationID!=model.ApplicationID))
				return new AjaxResultModel().SetIsError("Uid","Уже существует заявление с данным UID'ом.");

			if(application.RegistrationDate.ToString("HH:mm:ss")=="00:00:00")
				application.RegistrationDate=model.RegistrationDate;

			application.NeedHostel=model.NeedHostel;

			application.ApproveInstitutionCount=true;
			application.FirstHigherEducation=true;
			application.ApprovePersonalData=true;
			application.FamiliarWithLicenseAndRules=true;
			application.FamiliarWithAdmissionType=true;
			application.FamiliarWithOriginalDocumentDeliveryDate=true;
			application.Priority=model.Priority;
			AjaxResultModel error=null;

			// Старая проверка и сохранение форм обучения и источников финансирования
			// Исключаем эти проверки - все они делаются при редактировании и сохранении приоритетов
			// К моменту сохранения заявления всё хорошо - либо правки сохранены, либо изменения отменены
			#region Устаревший код
			/*
            if (model.EducationFormsSelected == null || !model.EducationFormsSelected.HasAny)
			{
				error = new AjaxResultModel().SetIsError("EducationFormsSelected_NotSelected", "Не выбраны форма обучения и источник финансирования");
			}

			application.IsRequiresBudgetO = model.EducationFormsSelected.BudgetO;
			application.IsRequiresBudgetOZ = model.EducationFormsSelected.BudgetOZ;
			application.IsRequiresBudgetZ = model.EducationFormsSelected.BudgetZ;
			application.IsRequiresPaidO = model.EducationFormsSelected.PaidO;
			application.IsRequiresPaidOZ = model.EducationFormsSelected.PaidOZ;
			application.IsRequiresPaidZ = model.EducationFormsSelected.PaidZ;
			application.IsRequiresTargetO = model.EducationFormsSelected.TargetO;
			application.IsRequiresTargetOZ = model.EducationFormsSelected.TargetOZ;
			application.IsRequiresTargetZ = model.EducationFormsSelected.TargetZ;

			//удаляем все организации ЦП у заявления и добавляем выбранные
			application.ApplicationSelectedCompetitiveGroupTarget.ToList().ForEach(x => dbContext.ApplicationSelectedCompetitiveGroupTarget.DeleteObject(x));
			if (application.IsRequiresTargetO)
			{
				if (model.SelectedTargetOrganizationIDO == 0)
					return new AjaxResultModel().SetIsError("SelectedTargetOrganizationIDO", "Не выбрана организация целевого приема");
				dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new ApplicationSelectedCompetitiveGroupTarget
				{
					Application = application,
					CompetitiveGroupTargetID = model.SelectedTargetOrganizationIDO,
					IsForO = true
				});
			}

			if (application.IsRequiresTargetOZ)
			{
				if (model.SelectedTargetOrganizationIDOZ == 0)
					return new AjaxResultModel().SetIsError("SelectedTargetOrganizationIDOZ", "Не выбрана организация целевого приема");
				dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new ApplicationSelectedCompetitiveGroupTarget
				{
					Application = application,
					CompetitiveGroupTargetID = model.SelectedTargetOrganizationIDOZ,
					IsForOZ = true
				});
			}

			if (application.IsRequiresTargetZ)
			{
				if (model.SelectedTargetOrganizationIDZ == 0)
					return new AjaxResultModel().SetIsError("SelectedTargetOrganizationIDZ", "Не выбрана организация целевого приема");
				dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new ApplicationSelectedCompetitiveGroupTarget
				{
					Application = application,
					CompetitiveGroupTargetID = model.SelectedTargetOrganizationIDZ,
					IsForZ = true
				});
            }
            */
			#endregion

			//удаляем все организации ЦП у заявления и добавляем выбранные
			application.ApplicationSelectedCompetitiveGroupTarget.ToList().ForEach(x => dbContext.ApplicationSelectedCompetitiveGroupTarget.DeleteObject(x));

			model.Priorities.ApplicationPriorities.ForEach(x => {
				if(x.CompetitiveGroupTargetId!=null) {
					dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroupTarget {
						Application=application,
						CompetitiveGroupTargetID=x.CompetitiveGroupTargetId.Value,
						IsForO=x.EducationFormId==11,
						IsForOZ=x.EducationFormId==12,
						IsForZ=x.EducationFormId==10
					});
				}
			});

			model.SelectedCompetitiveGroupIDs=model.SelectedCompetitiveGroupIDs??new int[0];

			//var groupChanged=!model.SelectedCompetitiveGroupIDs
			//	.OrderBy(x => x)
			//	.SequenceEqual(application.ApplicationSelectedCompetitiveGroup.Select(x => x.CompetitiveGroupID).OrderBy(x => x));
			//если сменили группу - чистим РВИ
			//if(groupChanged&&error==null) {
			//	//удаляем отсутствующие
			//	//вначале испытания
			//	dbContext.ApplicationEntranceTestDocument
			//		.Where(x => x.ApplicationID==applicationID
			//			&&(
			//				(x.CompetitiveGroupID==null&&!model.SelectedCompetitiveGroupIDs.Contains(x.EntranceTestItemC.CompetitiveGroupID))
			//				||(x.CompetitiveGroupID>0&&!model.SelectedCompetitiveGroupIDs.Contains(x.CompetitiveGroupID??0))))
			//		.ToList().ForEach(dbContext.ApplicationEntranceTestDocument.DeleteObject);
			//	//а теперь и сами сведения о КГ
			//	application.ApplicationSelectedCompetitiveGroup.Where(x => !model.SelectedCompetitiveGroupIDs.Contains(x.CompetitiveGroupID))
			//		.ToList().ForEach(dbContext.ApplicationSelectedCompetitiveGroup.DeleteObject);

			//	//добавляем новые
			//	model.SelectedCompetitiveGroupIDs.Where(x => !application.ApplicationSelectedCompetitiveGroup.Any(y => y.CompetitiveGroupID==x))
			//		.ToList().ForEach(x => dbContext.ApplicationSelectedCompetitiveGroup.AddObject(new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroup {
			//			Application=application,
			//			CompetitiveGroupID=x
			//		}));
			//}

			//а теперь направления
			{
				model.SelectedDirectionIDs=model.SelectedDirectionIDs??new string[0];

				var applicationSelectedCompetitiveGroupItems=application.ApplicationSelectedCompetitiveGroupItem;
				//удаляем направления, которые есть в базе, но которых нет в выбранных
				applicationSelectedCompetitiveGroupItems
					.Where(x => !model.SelectedDirectionIDs.Contains(x.CompetitiveGroupItem.CompetitiveGroup.EducationLevelID+
                    "@"+x.CompetitiveGroupItem.CompetitiveGroup.DirectionID+
								"@"+x.CompetitiveGroupItemID)
						||!model.SelectedCompetitiveGroupIDs.Contains(x.CompetitiveGroupItem.CompetitiveGroupID))
					.ToList()
					.ForEach(dbContext.ApplicationSelectedCompetitiveGroupItem.DeleteObject);

				//добавляем те направления, которых нет в базе, но есть в выбранных
				if(applicationSelectedCompetitiveGroupItems!=null&&model.SelectedDirectionIDs!=null&&model.SelectedCompetitiveGroupIDs!=null)
					foreach(string selectedDirectionID in model.SelectedDirectionIDs) {
						short edLevel=selectedDirectionID.Split('@')[0].To<short>();
						int dirID=selectedDirectionID.Split('@')[1].To<int>();
						int cgItemId=selectedDirectionID.Split('@')[2].To<int>();
						foreach(int groupID in model.SelectedCompetitiveGroupIDs) {
							if(!applicationSelectedCompetitiveGroupItems.Any(
									  x => x.CompetitiveGroupItemID==cgItemId)) {
								var competitiveGroupItem=dbContext.CompetitiveGroupItem.FirstOrDefault(
									 y => y.CompetitiveGroupItemID==cgItemId);
								if(competitiveGroupItem!=null) {
									if(!model.SelectedDirectionIDs.Contains(competitiveGroupItem.CompetitiveGroup.EducationLevelID+"@"+
																						  competitiveGroupItem.CompetitiveGroup.DirectionID+"@"+
																								competitiveGroupItem.CompetitiveGroupItemID))
										return new AjaxResultModel().SetIsError("dirComplete","Не выбрано направление подготовки");
									var ascgi=new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroupItem {
										Application=application,
										CompetitiveGroupItem=competitiveGroupItem
									};
									dbContext.ApplicationSelectedCompetitiveGroupItem.AddObject(ascgi);
								}
							}
						}
					}
			}

			if(error==null||saveInconsistentData)
				dbContext.SaveChanges();
			return error??new AjaxResultModel("");
		}

		//for pgu, not working now
		public static int CreateApplication(this EntrantsEntities dbContext,UserInfo userInfo,int structureItemID) {
			Application app=new Application();
			var entrant=dbContext.Entrant.Single(x => x.SNILS==userInfo.SNILS);
			app.Entrant=entrant;
			app.InstitutionID=0; //publ.InstitutionID;
			app.StatusID=ApplicationStatusType.Draft;
			app.WizardStepID=2;
			app.SourceID=1;
			var registrationDate=DateTime.Now;
			registrationDate=registrationDate.AddMilliseconds(-registrationDate.Millisecond);
			app.RegistrationDate=registrationDate;
			dbContext.Application.AddObject(app);
			dbContext.SaveChanges();
			return app.ApplicationID;
		}

		/// <summary>
		/// Проверяем, есть ли уже абитуриент с введёнными данными
		/// </summary>
		public static Entrant CheckExistingEntrant(this EntrantsEntities dbContext,InstitutionPrepareApplicationViewModel model,int insitutionID) {
			return (from entrant in dbContext.Entrant
					  join i in dbContext.EntrantDocument on entrant.EntrantID equals i.EntrantID
					  where entrant.InstitutionID==insitutionID
							  &&(i.DocumentSeries==model.DocumentSeries
									||(i.DocumentSeries==null&&model.DocumentSeries==null))
							  &&i.DocumentNumber==model.DocumentNumber
							  &&i.DocumentTypeID==model.IdentityDocumentTypeID
					  select entrant).FirstOrDefault();
		}

		/// <summary>
		/// Создаём абитуриента и заявление
		/// </summary>
		public static int? CreateAndGetApplicationAndEntrant(this EntrantsEntities dbContext,int institutionID,	InstitutionPrepareApplicationViewModel model,out int? entrantExist) {
			entrantExist=null;

			int[] compGroupIDs=dbContext.CompetitiveGroup
				.Where( x => x.CampaignID==model.CampaignID && model.SelectedCompetitiveGroupIDs.Contains(x.CompetitiveGroupID) && x.InstitutionID==institutionID)
				.Select(x => x.CompetitiveGroupID).ToArray();
			if(compGroupIDs.Length==0) return null;

			Entrant entrant;

			using(var transaction=new TransactionScope(TransactionScopeOption.RequiresNew,  new TransactionOptions { IsolationLevel=IsolationLevel.ReadCommitted })) {
				entrant=CheckExistingEntrant(dbContext, model, institutionID);
				if(entrant!=null) {
					entrantExist=entrant.EntrantID;
				} else {
					entrant=new Entrant {
						SNILS=null,
						InstitutionID=institutionID,
						LastName="",
						FirstName="",
						MiddleName="",
						GenderID=GenderType.Male
					};
					dbContext.Entrant.AddObject(entrant);
					dbContext.SaveChanges();
					var id=new IdentityDocumentViewModel {
						DocumentNumber=model.DocumentNumber,
						DocumentSeries=model.DocumentSeries,
						IdentityDocumentTypeID=model.IdentityDocumentTypeID,
						EntrantID=entrant.EntrantID
					};

					entrant.EntrantDocument_Identity=dbContext.CreateEntrantDocument(id,entrant);

#warning Предположительно, это сохранение тут нужно
					dbContext.SaveChanges();
				}
				transaction.Complete();
			}



			Application app=new Application();
			app.OrderCalculatedRating=0;
			app.Entrant=entrant;
			app.InstitutionID=institutionID;
			// создаём всегда черновик
			app.StatusID=ApplicationStatusType.Draft;
			app.WizardStepID=2;
			app.SourceID=2;
			app.RegistrationDate=model.RegistrationDate;
			app.ApproveInstitutionCount=true;
			app.FirstHigherEducation=true;
			app.ApprovePersonalData=true;
			app.FamiliarWithLicenseAndRules=true;
			app.FamiliarWithAdmissionType=true;
			app.FamiliarWithOriginalDocumentDeliveryDate=true;

			//Пока оставляем этот кусок - для совместимости. Впоследствии его можно будет удалить
			if(model.EducationForms==null) model.EducationForms=new ApplicationSendingViewModel.EducationForms();
			app.IsRequiresBudgetO=model.EducationForms.BudgetO;
			app.IsRequiresPaidO=model.EducationForms.PaidO;
			app.IsRequiresTargetO=model.EducationForms.TargetO;

			app.IsRequiresBudgetOZ=model.EducationForms.BudgetOZ;
			app.IsRequiresPaidOZ=model.EducationForms.PaidOZ;
			app.IsRequiresTargetOZ=model.EducationForms.TargetOZ;

			app.IsRequiresBudgetZ=model.EducationForms.BudgetZ;
			app.IsRequiresPaidZ=model.EducationForms.PaidZ;
			app.IsRequiresTargetZ=model.EducationForms.TargetZ;

			if(app.IsRequiresTargetO)
				dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroupTarget { Application=app,CompetitiveGroupTargetID=model.SelectedTargetOrganizationIDO,IsForO=true });
			if(app.IsRequiresTargetOZ)
				dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroupTarget { Application=app,CompetitiveGroupTargetID=model.SelectedTargetOrganizationIDOZ,IsForOZ=true });
			if(app.IsRequiresTargetZ)
				dbContext.ApplicationSelectedCompetitiveGroupTarget.AddObject(new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroupTarget { Application=app,CompetitiveGroupTargetID=model.SelectedTargetOrganizationIDZ,IsForZ=true });

			//bool isUnique = dbContext.CheckApplicationNumberIsUnique(institutionID, model.ApplicationNumber);
			//if (!isUnique)
			//    return null;

			app.ApplicationNumber=model.ApplicationNumber;
			app.Priority=model.Priority;
			//app.CompetitiveGroupID = compGroupID;
			foreach(var compGroupID in compGroupIDs) {
				//var ascg=new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroup {	Application=app,	CompetitiveGroupID=compGroupID};
				//dbContext.ApplicationSelectedCompetitiveGroup.AddObject(ascg);
			}

			if(model.SelectedDirectionIDs==null) model.SelectedDirectionIDs=new string[0];
			foreach(var dirKey in model.SelectedDirectionIDs) {
				short edLevel=dirKey.Split('@')[0].To<short>();
				int dirID=dirKey.Split('@')[1].To<int>();
				var cgi=dbContext.CompetitiveGroupItem.Where(x => x.CompetitiveGroup.DirectionID==dirID && x.CompetitiveGroup.EducationLevelID==edLevel && compGroupIDs.Contains(x.CompetitiveGroupID)).ToArray();
				
				foreach(var competitiveGroupItem in cgi) {
					var ascgi=new GVUZ.Model.Entrants.ApplicationSelectedCompetitiveGroupItem {
						Application=app,
						CompetitiveGroupItem=competitiveGroupItem
					};
					dbContext.ApplicationSelectedCompetitiveGroupItem.AddObject(ascgi);
				}
			}

			dbContext.Application.AddObject(app);
			dbContext.SaveChanges();
			dbContext.AddApplicationAccessToLog(null,new PersonalDataAccessLogger.AppData(app),"CreateApplication", institutionID, app.ApplicationID);

			EntrantCacheManager.Add(app.ApplicationID, app);

			return app.ApplicationID;
		}

		public static void ClearFilterDataCache(int institutionID) {
			var cache=ServiceLocator.Current.GetInstance<ICache>();
			cache.RemoveAllWithPrefix(
				 "ApplicationList_FilterData_"+institutionID,
				 "ApplicationList_AllowedCompGroups_"+institutionID,
				 "ApplicationList_AllowedCompGroupsByCampaignName_"+institutionID);
		}

		/// <summary>
		/// Загрузка списка заявлений
		/// </summary>
		/// <param name="loadFilterData">грузить ли фильтр</param>
		public static InstitutionApplicationListViewModel FillInstitutionApplicationList(this EntrantsEntities dbContext,InstitutionApplicationListViewModel model,bool loadFilterData,int institutionID,int applicationID=0,bool ignoreSkipping=false) {
			var cache=ServiceLocator.Current.GetInstance<ICache>();

			//сейчас создавать можно всегда
			model.CanCreateNewApplication=true;
			#region Кеширование фильтров
			if(loadFilterData) {
				var filter=cache.Get<InstitutionApplicationListViewModel>("ApplicationList_FilterData_"+institutionID,null);
				if(filter!=null)
					return filter;

				var violationTypes=dbContext.ViolationType.Where(x => x.ViolationID!=0).OrderBy(x => x.Name).Select(x => new { ID=x.ViolationID,x.Name }).ToList();
				violationTypes.Insert(0,new { ID=-1,Name=InstitutionApplicationListViewModel.EmptyText });
				model.ViolationTypes=violationTypes;
				var benefits=dbContext.Benefit.OrderBy(x => x.ShortName).Select(x => new { ID=x.BenefitID,Name=x.ShortName }).ToList();
				benefits.Insert(0,new { ID=(short)0,Name="Без льгот" });
				benefits.Insert(0,new { ID=(short)-1,Name=InstitutionApplicationListViewModel.EmptyText });
				model.Benefits=benefits;
				model.Campaigns=dbContext.Campaign.Where(x => x.InstitutionID==institutionID).OrderBy(x => x.Name).Select(x => x.Name).ToArray();
				model.CompetitiveGroups=dbContext.CompetitiveGroup.Where(x => x.InstitutionID==institutionID).OrderBy(x => x.Name).Select(x => x.Name).ToArray();

				//много где хардкод ID-шников, так что тут тоже не стоит изменять логике
				var formNames=EducationFormNames;
				var forms=new[] { new { ID=0,Name="[Любая]" } }.ToList();
				for(int i=0;i<formNames.Length;i++) { forms.Add(new { ID=i+1,Name=formNames[i] }); }
				model.EducationForms=forms;

				cache.Insert("ApplicationList_FilterData_"+institutionID,model);
				return model;
			}

			var allowedCompGroups=cache.Get<int[]>("ApplicationList_AllowedCompGroups_"+institutionID,null);
			if(allowedCompGroups==null) {
				allowedCompGroups=dbContext.CompetitiveGroup.Where(x => x.InstitutionID==institutionID).Select(x => x.CompetitiveGroupID).ToArray();
				cache.Insert("ApplicationList_AllowedCompGroups_"+institutionID,allowedCompGroups);
			}

			int oneCompGroupID=0;
			if(model.Filter!=null&&!String.IsNullOrWhiteSpace(model.Filter.CompetitiveGroupName)&&model.TabID==4) { //только для принятых применяем фильтр
				allowedCompGroups=cache.Get<int[]>(string.Format("ApplicationList_AllowedCompGroupsByCampaignName_{0}_{1}",institutionID,model.Filter.CampaignName),null);
				if(allowedCompGroups==null) {
					allowedCompGroups=dbContext.CompetitiveGroup
						 .Where(x => x.Name==model.Filter.CompetitiveGroupName&&x.InstitutionID==institutionID&&x.Campaign.Name==model.Filter.CampaignName)
						 .Select(x => x.CompetitiveGroupID).ToArray();
					cache.Insert(string.Format("ApplicationList_AllowedCompGroupsByCampaignName_{0}_{1}",institutionID,model.Filter.CampaignName),allowedCompGroups);
				}

				if(allowedCompGroups.Length==1) { oneCompGroupID=allowedCompGroups[0]; }
			}
			#endregion
			// Формирование запроса
			IQueryable<Application> tq=dbContext.Application;
			bool sortByRating=false;
			bool sortByInRecList=false;
			//если нет конкретного заявления, грузим все по фильтру и сортировке
			if(applicationID==0) {
				#region Сортировка
				int? sortID=model.SortID;
				if(!sortID.HasValue) { sortID=1; }
				if(true) {
					if(sortID.Value==1) tq=tq.OrderBy(x => x.ApplicationNumber);
					if(sortID.Value==-1) tq=tq.OrderByDescending(x => x.ApplicationNumber);

					if(sortID.Value==2) tq=tq.OrderBy(x => x.Entrant.LastName).ThenBy(x => x.Entrant.FirstName).ThenBy(x => x.Entrant.MiddleName);
					if(sortID.Value==-2) tq=tq.OrderByDescending(x => x.Entrant.LastName).ThenByDescending(x => x.Entrant.FirstName).ThenByDescending(x => x.Entrant.MiddleName);

					if(sortID.Value==3) tq=tq.OrderBy(x => x.Entrant.EntrantDocument_Identity.DocumentSeries).ThenBy(x => x.Entrant.EntrantDocument_Identity.DocumentNumber);
					if(sortID.Value==-3) tq=tq.OrderByDescending(x => x.Entrant.EntrantDocument_Identity.DocumentSeries).ThenByDescending(x => x.Entrant.EntrantDocument_Identity.DocumentNumber);

					if(sortID.Value==4) tq=tq.OrderBy(x => x.RegistrationDate);
					if(sortID.Value==-4) tq=tq.OrderByDescending(x => x.RegistrationDate);

					//if(sortID.Value==6) tq=tq.OrderBy(x => x.ViolationType.Name).ThenBy(x => x.ViolationErrors); // ViolationErrors ДИКО ТОРМОЗИТ!!!!
					//if(sortID.Value==-6) tq=tq.OrderByDescending(x => x.ViolationType.Name).ThenByDescending(x => x.ViolationErrors); // ViolationErrors ДИКО ТОРМОЗИТ!!!!
					if(sortID.Value==6) tq=tq.OrderBy(x => x.ViolationType.Name);
					if(sortID.Value==-6) tq=tq.OrderByDescending(x => x.ViolationType.Name);

					if(sortID.Value==7) tq=tq.OrderBy(x => x.ApplicationStatusType.Name);
					if(sortID.Value==-7) tq=tq.OrderByDescending(x => x.ApplicationStatusType.Name);
					if(sortID.Value==8) tq=tq.OrderBy(x => x.LastCheckDate);
					if(sortID.Value==-8) tq=tq.OrderByDescending(x => x.LastCheckDate);
					//если нет Order, то и так нормально не отсортируем, так что пусть клиентская часть разбирается, когда можно сортировать
					if(sortID.Value==9) tq=tq.OrderBy(x => x.CompetitiveGroup.Name);
					if(sortID.Value==-9) tq=tq.OrderByDescending(x => x.CompetitiveGroup.Name);
					if(sortID.Value==10) tq=tq.OrderBy(x => x.SourceID);
					if(sortID.Value==-10) tq=tq.OrderBy(x => -x.StatusID);
					//сортировка по рейтингу уже материализованная в связи со сложностью
					if(sortID.Value==11||sortID.Value==-11) sortByRating=true;
					if(sortID.Value==12) tq=tq.OrderBy(x => x.OriginalDocumentsReceived);
					if(sortID.Value==-12) tq=tq.OrderByDescending(x => x.OriginalDocumentsReceived);

					sortByInRecList=Math.Abs(sortID.Value)==13;
				}
				#endregion
				#region Фильтр по TabID;
				int? tabID=model.TabID;
				if(!tabID.HasValue) { tabID=1; }
				if(tabID==1) { tq=tq.Where(x => x.StatusID==ApplicationStatusType.New||(x.StatusID==ApplicationStatusType.Draft&&x.SourceID==2)); }
				if(tabID==2) tq=tq.Where(x => x.StatusID==ApplicationStatusType.Failed||x.StatusID==ApplicationStatusType.Removed);
				if(tabID==3) tq=tq.Where(x => x.StatusID==ApplicationStatusType.Denied);
				if(tabID==4) tq=tq.Where(x => x.StatusID==ApplicationStatusType.Accepted /*|| x.StatusID == ApplicationStatusType.InOrder*/ &&x.PublishDate==null);
				if(tabID==5) tq=tq.Where(x => x.StatusID==ApplicationStatusType.InOrder /*&& x.PublishDate == null*/);
				if(tabID==-1) {
					tq=tq.Where(x => x.StatusID==ApplicationStatusType.Draft&&x.SourceID==2).Where(x => x.InstitutionID==institutionID).OrderBy(x => x.RegistrationDate);
					model.CanCreateNewApplication=true;
				} else {
					tq=tq.Where(x => x.InstitutionID==institutionID);
				}
				#endregion

				model.TotalItemCount=tq.Count();	// Количество Без фильтров
				#region ФИльтры
				if(model.Filter!=null) {
					if(model.Filter.DateBegin.HasValue) tq=tq.Where(x => x.RegistrationDate>=model.Filter.DateBegin);
					if(model.Filter.DateEnd.HasValue) {
						DateTime endDateAdj=model.Filter.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
						tq=tq.Where(x => x.RegistrationDate<=endDateAdj);
					}
					//if (model.Filter.Place != -1)
					//	tq = tq.Where(x => x.SourceID == model.Filter.Place);
					if(model.Filter.DocumentsReceived!=-1) {
						if(model.Filter.DocumentsReceived==1) {
							tq=tq.Where(x => x.OriginalDocumentsReceived);
						} else {
							tq=tq.Where(x => !x.OriginalDocumentsReceived);
						}
					}

					if(tabID==2&&model.Filter.ViolationTypeID>=0)
						tq=tq.Where(x => x.ViolationID==model.Filter.ViolationTypeID);

					if(!String.IsNullOrWhiteSpace(model.Filter.CompetitiveGroupName)&&(model.TabID>=1&&model.TabID<=4)) //применяем фильтр для всех
					{
						var allowedItems1=dbContext.CompetitiveGroup.Where(x => x.Name==model.Filter.CompetitiveGroupName&&x.InstitutionID==institutionID);
						if(!String.IsNullOrWhiteSpace(model.Filter.CampaignName)) allowedItems1=allowedItems1.Where(x => x.Campaign.Name==model.Filter.CampaignName);
						int[] allowedItems=allowedItems1.Select(x => x.CompetitiveGroupID).ToArray();
						tq=tq.Where(y => allowedItems.Contains(y.CompetitiveGroup.CompetitiveGroupID));
					}

					if(String.IsNullOrWhiteSpace(model.Filter.CompetitiveGroupName)&&!String.IsNullOrWhiteSpace(model.Filter.CampaignName)) {
						//var alItems = dbContext.Campaign.Where(x => x.InstitutionID == institutionID).ToArray();
						var arr=new List<int>();
						//foreach(var t in tq.Select(x => x.ApplicationSelectedCompetitiveGroup)) {
						//	foreach(var tt in t.Distinct()) {
						//		if(tt.CompetitiveGroup.Campaign.Name==model.Filter.CampaignName) arr.Add(tt.CompetitiveGroupID);
						//	}
						//}
						//tq=tq.Where(x => x.ApplicationSelectedCompetitiveGroup.Any(y => arr.Contains(y.CompetitiveGroupID)));
					}

					//if(model.Filter.Benefit!=-1) {	// Что это такое?
					//	if(model.Filter.Benefit==0)
					//		tq=tq.Where(x => x.ApplicationSelectedCompetitiveGroup.Where(y => allowedCompGroups.Contains(y.CompetitiveGroupID)).All(y => y.CalculatedBenefitID==null));
					//	else
					//		tq=tq.Where(x => x.ApplicationSelectedCompetitiveGroup.Where(y => allowedCompGroups.Contains(y.CompetitiveGroupID)).All(y => y.CalculatedBenefitID==model.Filter.Benefit));
					//}
					#region model.Filter.EducationForm
					if(model.Filter.EducationForm>0&&model.TabID==4) //только для принятых применяем фильтр
					{
						if(model.Filter.EducationForm==1)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.O&&(y.EducationSourceId==EDSourceConst.Budget||y.EducationSourceId==EDSourceConst.Quota) /*&& y.Priority.HasValue && y.Priority.Value > 0 */));

						if(model.Filter.EducationForm==2)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.OZ&&(y.EducationSourceId==EDSourceConst.Budget||y.EducationSourceId==EDSourceConst.Quota) /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==3)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.OZ&&(y.EducationSourceId==EDSourceConst.Budget||y.EducationSourceId==EDSourceConst.Quota) /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==4)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.O&&y.EducationSourceId==EDSourceConst.Paid /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==5)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.OZ&&y.EducationSourceId==EDSourceConst.Paid /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==6)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.Z&&y.EducationSourceId==EDSourceConst.Paid /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==7)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.O&&y.EducationSourceId==EDSourceConst.Target /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==8)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.OZ&&y.EducationSourceId==EDSourceConst.Target /*&& y.Priority.HasValue && y.Priority.Value > 0*/));

						if(model.Filter.EducationForm==9)
							tq=tq.Where(x => x.ApplicationCompetitiveGroupItem.Any(y => y.EducationFormId==EDFormsConst.Z&&y.EducationSourceId==EDSourceConst.Target /*&& y.Priority.HasValue && y.Priority.Value > 0*/));
					}
					#endregion
					#region Фильтры по текстовым полям
					if(!String.IsNullOrEmpty(model.Filter.ApplicationNumber)) tq=tq.Where(x => x.ApplicationNumber.Contains(model.Filter.ApplicationNumber));
					if(!String.IsNullOrEmpty(model.Filter.EntrantLastName)) tq=tq.Where(x => x.Entrant.LastName.Contains(model.Filter.EntrantLastName));
					if(!String.IsNullOrEmpty(model.Filter.EntrantFirstName)) tq=tq.Where(x => x.Entrant.FirstName.Contains(model.Filter.EntrantFirstName));
					if(!String.IsNullOrEmpty(model.Filter.EntrantMiddleName)) tq=tq.Where(x => x.Entrant.MiddleName.Contains(model.Filter.EntrantMiddleName));
					if(!String.IsNullOrEmpty(model.Filter.EntrantDocSeries)) tq=tq.Where(x => x.Entrant.EntrantDocument_Identity.DocumentSeries.Contains(model.Filter.EntrantDocSeries));
					if(!String.IsNullOrEmpty(model.Filter.EntrantDocNumber)) tq=tq.Where(x => x.Entrant.EntrantDocument_Identity.DocumentNumber.Contains(model.Filter.EntrantDocNumber));
					#endregion
				}
				#endregion
			} else { //нужно вернуть только одно заявление			
				tq=tq.Where(x => x.ApplicationID==applicationID).OrderBy(x => x.ApplicationID);
			}

			model.TotalItemFilteredCount=tq.Count();

			#region Пейджинг
			// если не игнорить пропуск, то достаём только нужную страницу
			// игнорится только для выгрузки списка
			if(!ignoreSkipping) {
				if(!model.PageNumber.HasValue||model.PageNumber<0)
					model.PageNumber=0;

				//var tmp = tq.Select(x => new {ApplicationDate = x.RegistrationDate, x.ApplicationID, x.ApplicationNumber, x.UID});

				model.TotalPageCount=((Math.Max(model.TotalItemFilteredCount,1)-1)/IncludeInOrderPageSize)+1;
				if(!sortByRating&&!sortByInRecList)
					tq=tq.Skip(model.PageNumber.Value*IncludeInOrderPageSize).Take(IncludeInOrderPageSize);
			}
			#endregion

			short[] levelsForRecList=new short[] { EDLevelConst.Bachelor, EDLevelConst.Speciality };

#warning Дичайшие тормоза!!!
			#region Выполнение запроса SQL
			var q=tq.Select(x => new {
				ApplicationDate=x.RegistrationDate, x.ApplicationID, x.ApplicationNumber, x.UID,
				DocSeries=x.Entrant.EntrantDocument_Identity.DocumentSeries,
				DocNumber=x.Entrant.EntrantDocument_Identity.DocumentNumber,
				F=x.Entrant.LastName,
				I=x.Entrant.FirstName,
				O=x.Entrant.MiddleName,
				ViolationName=x.ViolationType.BriefName, x.ViolationErrors, x.ViolationID, x.StatusDecision, x.StatusID,
				StatusName=x.ApplicationStatusType.Name, x.LastCheckDate, x.LastDenyDate,
				Source=x.SourceID, x.PublishDate,
				OrderCompetitiveGroupName=x.CompetitiveGroup.Name,
				CompetitiveGroupNames=x.CompetitiveGroup.Name,
				BenefitID=x.OrderCalculatedBenefitID.Value,
				Benefit=x.Benefit,
				Rating=x.OrderCalculatedRating,
				DocumentsReceived=x.OriginalDocumentsReceived,
				IsRequiresBudgetO=x.IsRequiresBudgetO,
				IsRequiresBudgetOZ=x.IsRequiresBudgetOZ,
				IsRequiresBudgetZ=x.IsRequiresBudgetZ,
				IsRequiresPaidO=x.IsRequiresPaidO,
				IsRequiresPaidOZ=x.IsRequiresPaidOZ,
				IsRequiresPaidZ=x.IsRequiresPaidZ,
				IsRequiresTargetO=x.IsRequiresTargetO,
				IsRequiresTargetOZ=x.IsRequiresTargetOZ,
				IsRequiresTargetZ=x.IsRequiresTargetZ,
				IncludeInRecListAvailiable=x.ApplicationCompetitiveGroupItem
                .Any(y => (y.EducationFormId==EDFormsConst.O||y.EducationFormId==EDFormsConst.OZ)&&(y.EducationSourceId==EDSourceConst.Budget)
                &&(levelsForRecList.Contains(y.CompetitiveGroupItem.CompetitiveGroup.EducationLevelID.Value)&&(y.Priority.HasValue))),
				//IsInRecList=x.RecomendedLists.Any(y => y.ApplicationID==x.ApplicationID&&y.RecomendedListsHistory.Any(z => z.RecListID==y.RecListID&&!z.DateDelete.HasValue)),
				// вызывать extension method тут нельзя иначе LINQ валится
				EntrantDocumentIdentityId=(x.Entrant.EntrantDocument_Identity==null)?0:x.Entrant.EntrantDocument_Identity.EntrantDocumentID
				//x.Entrant.EntrantDocument_Identity != null && 
				//                   x.Entrant.EntrantDocument_Identity.EntrantDocumentIdentity != null && 
				//                   x.Entrant.EntrantDocument_Identity.EntrantDocumentIdentity.CountryType != null && 
				//                   x.Entrant.EntrantDocument_Identity.EntrantDocumentIdentity.CountryType.DigitCode != ApplicationExtensions.RussiaDigitCode
			}).ToArray();
			//если сортировка по рейтингу, то сортируем с учётом льгот
			#endregion

			if(sortByRating) {
				if(model.SortID.Value==11) q=q.OrderByDescending(x => x.BenefitID==1||x.BenefitID==4||x.BenefitID==5?x.BenefitID:100).ThenBy(x => x.Rating).ToArray();
				if(model.SortID.Value==-11) q=q.OrderBy(x => x.BenefitID==1||x.BenefitID==4||x.BenefitID==5?x.BenefitID:100).ThenByDescending(x => x.Rating).ToArray();
				q=q.Skip(model.PageNumber.Value*IncludeInOrderPageSize).Take(IncludeInOrderPageSize).ToArray();
			}
			#region Обработка резултатов запроса
			var data=q.Select(x => new InstitutionApplicationListViewModel.ApplicationData {
				ApplicationDate=x.ApplicationDate.ToString("dd.MM.yyyy"),
				ApplicationDateDate=x.ApplicationDate,
				ApplicationUID=x.UID,
				LastCheckDate=model.TabID==3?(x.LastDenyDate.HasValue?x.LastDenyDate.Value.ToString("dd.MM.yyyy"):"")
								:(x.LastCheckDate.HasValue?x.LastCheckDate.Value.ToString("dd.MM.yyyy"):""),
				ApplicationID=x.ApplicationID,
				ApplicationNumber=String.IsNullOrWhiteSpace(x.ApplicationNumber)?"не задан":x.ApplicationNumber,
				EntrantDocData=x.DocSeries+" "+x.DocNumber,
				EntrantFIO=x.F+" "+x.I+" "+x.O,
				Source=x.Source==1?"ПГУ":"ОО",
				StatusID=x.StatusID,
				CompetitiveGroupName=x.StatusID==ApplicationStatusType.InOrder?(x.OrderCompetitiveGroupName??""):String.Join(", ",x.CompetitiveGroupNames),
				StatusName=x.StatusName+(x.PublishDate.HasValue?x.PublishDate.Value.ToString(" '(дата публикации:' dd.MM.yyyy')'"):""),
				ViolationName=x.StatusID==ApplicationStatusType.Failed||x.StatusID==ApplicationStatusType.Denied
						?x.ViolationName+((x.ViolationID==3&&!String.IsNullOrWhiteSpace(x.StatusDecision))?" ("+x.StatusDecision+")":"")
					//реплейс для фикса отображения некорректных данных, находящихся в базе
							+((/*(x.ViolationID == 2) &&*/ !String.IsNullOrWhiteSpace((x.ViolationErrors??"").Replace("GVUZ.Model.Applications.ApplicationValidationErrorInfo","")))
								?" ("+(x.ViolationErrors??"").Replace("GVUZ.Model.Applications.ApplicationValidationErrorInfo","")+")"
								:"")
						:"",
				Benefit=x.Benefit!=null?x.Benefit.Name:"",
				BenefitShort=x.Benefit!=null?x.Benefit.ShortName:"",
				BenefitID=x.BenefitID,
				NumberRating=oneCompGroupID>0?x.Rating??0:-1,
				OriginalDocumentsRecieved=x.DocumentsReceived,
				IsRequiresBudgetO=x.IsRequiresBudgetO,
				IsRequiresBudgetOZ=x.IsRequiresBudgetOZ,
				IsRequiresBudgetZ=x.IsRequiresBudgetZ,
				IsRequiresPaidO=x.IsRequiresPaidO,
				IsRequiresPaidOZ=x.IsRequiresPaidOZ,
				IsRequiresPaidZ=x.IsRequiresPaidZ,
				IsRequiresTargetO=x.IsRequiresTargetO,
				IsRequiresTargetOZ=x.IsRequiresTargetOZ,
				IsRequiresTargetZ=x.IsRequiresTargetZ,
				IncludeInRecListAvailiable=x.IncludeInRecListAvailiable,
				//IsInRecList=x.IsInRecList,
				EntrantDocumentIdentityId=x.EntrantDocumentIdentityId
			}).ToArray();
			#endregion

			int[] selectedIdentityDocumentId=data.Select(x => x.EntrantDocumentIdentityId).ToArray();

			int[] foreignIdentityDocumentId=dbContext.EntrantDocumentIdentity
				 .Where(x => selectedIdentityDocumentId.Contains(x.EntrantDocumentID)&&x.CountryType.DigitCode!=ApplicationExtensions.RussiaDigitCode)
				 .Select(x => x.EntrantDocumentID)
				 .ToArray();

			// ReSharper disable ForCanBeConvertedToForeach
			for(int ix=0;ix<foreignIdentityDocumentId.Length;ix++) {// ReSharper restore ForCanBeConvertedToForeach
				int foreignDocId=foreignIdentityDocumentId[ix];
                foreach (var item in data.Where(x => x.EntrantDocumentIdentityId == foreignDocId)) ; //{ item.IsForeignCitizen=true; }
			}

			if(sortByInRecList) {
				if(model.SortID==13)
					data=data.OrderBy(x => x.IsInRecList).ToArray();// .Skip(model.PageNumber.Value * IncludeInOrderPageSize).Take(IncludeInOrderPageSize).ToArray();
				else if(model.SortID==-13) data=data.OrderByDescending(x => x.IsInRecList).ToArray(); // .Skip(model.PageNumber.Value * IncludeInOrderPageSize).Take(IncludeInOrderPageSize).ToArray();
			}

			if(model.Filter!=null) {
				switch(model.Filter.IsInRecList) {
					case 0: data=data.Where(x => !x.IsInRecList).ToArray(); break;
					case 1: data=data.Where(x => x.IsInRecList).ToArray(); break;
					case -1: break;
				}
			}

			if(sortByInRecList) {
				model.Applications=data.Skip(model.PageNumber.Value*IncludeInOrderPageSize).Take(IncludeInOrderPageSize).ToArray();
			} else {
				model.Applications=data;
			}

			dbContext.Connection.Close();

			using(var prioritiesContext=new ApplicationPrioritiesEntities()) {
				prioritiesContext.FillExistingPriorities(model);
			}
			dbContext.Connection.Open();

			dbContext.AddApplicationAccessToLog(model.Applications.Select(x => new PersonalDataAccessLogger.AppData {
				ApplicationNumber=x.ApplicationNumber,
				ApplicationUID=x.ApplicationUID,
				ApplicationID=x.ApplicationID,
				ApplicationDate=x.ApplicationDateDate,
			}).ToArray(),"ApplicationList",institutionID);

			return model;
		}
/*
		public static InstitutionApplication2ListModel FillInstitutionApplication2List(this EntrantsEntities dbContext,InstitutionApplication2ListReqModel req,bool loadFilterData,int institutionID,int applicationID=0,bool ignoreSkipping=false) {
			//сейчас создавать можно всегда
			//model.CanCreateNewApplication=true;
			var model=new InstitutionApplication2ListModel();

			//dbContext.CreateQuery  .sql .Database.SqlQuery  .DbExtensions
			string sql=@"SELECT TOP 10 app.ApplicationID, COALESCE(app.ApplicationNumber,'не задан') as ApplicationNumber, ent.LastName as EntrantLastName, ent.FirstName as EntrantFirstName, ent.MiddleName as EntrantMiddleName
FROM [dbo].[Application] app with (NOLOCK)
 INNER JOIN Entrant ent with (NOLOCK) on app.EntrantID=ent.EntrantID INNER JOIN EntrantDocument ed on ent.EntrantID=ed.EntrantID
 INNER JOIN [ViolationType] vt with (NOLOCK) on app.ViolationID=vt.ViolationID
 INNER JOIN ApplicationStatusType ast with (NOLOCK) on app.StatusID=ast.StatusID
 INNER JOIN CompetitiveGroup cg with (NOLOCK) on app.OrderCompetitiveGroupID=cg.CompetitiveGroupID
 LEFT OUTER JOIN ApplicationSelectedCompetitiveGroup ascg with (NOLOCK) on ascg.ApplicationID=app.ApplicationID and cg.CompetitiveGroupID=ascg.CompetitiveGroupID
WHERE app.InstitutionID=@InstitutionID
AND ent.LastName is not null and ent.FirstName is not null and ent.MiddleName is not null
";
			//dbContext.Connection.ConnectionString
			System.Data.Objects.ObjectParameter[] parameters=new System.Data.Objects.ObjectParameter[1];
			parameters[0]=new System.Data.Objects.ObjectParameter("InstitutionID",587);
			//var q=dbContext.CreateQuery<Application2Data>(sql,parameters);
			//var q=dbContext.CreateQuery<Application2Data>(sql,new System.Data.Objects.ObjectParameter("InstitutionID",587));		
			model.Applications=q.ToArray<Application2Data>();
			return model;
		}
*/
		internal static string[] EducationFormNames {
			get {
				var formNames=new[]{
				                	"Очная форма - Бюджетные места", 
										"Очно-заочная (вечерняя) - Бюджетные места",
				                	"Заочная форма - Бюджетные места",
				                	"Очная форма - Платные места", 
										"Очно-заочная (вечерняя) - Платные места",
				                	"Заочная форма - Платные места",
										"Очная форма - Целевой прием", 
										"Очно-заочная (вечерняя) - Целевой прием",
				                	"Заочная форма - Целевой прием",
				                };
				return formNames;
			}
		}

		/// <summary>
		/// Ищем место, где находится нужное заявление
		/// </summary>
		public static void FindApplicationInList(this EntrantsEntities dbContext,int applicationID,int institutionID,out int tabID,out int pageNumber,out int orderID) {
			var appRes=dbContext.Application.Where(x => x.ApplicationID==applicationID)
				.Select(x => new { x.StatusID,OrderID=x.OrderOfAdmissionID??0 })
				.FirstOrDefault();

			int statusID=ApplicationStatusType.Draft;
			orderID=0;
			if(appRes!=null) {
				statusID=appRes.StatusID;
				orderID=appRes.OrderID;
			}

			IQueryable<Application> tq=dbContext.Application;

			tq=tq.OrderBy(x => x.ApplicationNumber);
			tabID=1;
			if(statusID==ApplicationStatusType.New||(statusID==ApplicationStatusType.Draft))
				tabID=1;
			if(statusID==ApplicationStatusType.Failed||(statusID==ApplicationStatusType.Removed))
				tabID=2;
			if(statusID==ApplicationStatusType.Denied)
				tabID=3;
			if(statusID==ApplicationStatusType.Accepted)
				tabID=4;
			if(statusID==ApplicationStatusType.InOrder)
				tabID=5;

			if(tabID==1)
				tq=tq.Where(x => x.StatusID==ApplicationStatusType.New||(x.StatusID==ApplicationStatusType.Draft&&x.SourceID==2));
			if(tabID==2) tq=tq.Where(x => x.StatusID==ApplicationStatusType.Failed||x.StatusID==ApplicationStatusType.Removed);
			if(tabID==3) tq=tq.Where(x => x.StatusID==ApplicationStatusType.Denied);
			if(tabID==4)
				tq=tq.Where(x => x.StatusID==ApplicationStatusType.Accepted&&x.PublishDate==null);
			if(tabID==5) {
				tq=tq.Where(x => x.StatusID==ApplicationStatusType.InOrder&&x.OrderOfAdmissionID==appRes.OrderID);
			}

			tq=tq.Where(x => x.InstitutionID==institutionID);

			var q=tq.Select(x => x.ApplicationID).ToArray();
			int idx=Array.IndexOf(q,applicationID);
			if(idx<0) idx=0;
			pageNumber=Math.Max(idx,1)/IncludeInOrderPageSize;
		}

		private static readonly int IncludeInOrderPageSize=AppSettings.Get("Search.PageSize",25);

		/// <summary>
		/// Принятие заявления
		/// </summary>
		public static AjaxResultModel ApproveApplication(this EntrantsEntities dbContext,InstitutionApplicationListViewModel model,int institutionID) {
			Application app=dbContext.GetApplication(model.ApplicationID);
			if(app==null)
				return new AjaxResultModel("Не найдено заявление");
			if(app.InstitutionID!=institutionID)
				return new AjaxResultModel("Невозможно принять данное заявление");
			//подрезаем длину поля (textarea на клиенте плохо лимитируется)
			if(model.StatusDecision!=null&&model.StatusDecision.Length>4000)
				model.StatusDecision=model.StatusDecision.Remove(4000);
			// результирующий статус в зависимости от исходного
			switch(app.StatusID) {
				case ApplicationStatusType.New:
				case ApplicationStatusType.Failed:
				case ApplicationStatusType.Removed:
				app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Accepted);
				app.StatusDecision=model.StatusDecision;
				break;
				case ApplicationStatusType.Denied:
				app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.New);
				app.StatusDecision="";
				break;
				default:
				return new AjaxResultModel("Неверный статус");
			}

			dbContext.SaveChanges();
			return new AjaxResultModel { Data=dbContext.FillInstitutionApplicationList(model,false,0,app.ApplicationID).Applications[0] };
		}

		/// <summary>
		/// Отзыв заявления
		/// </summary>
		public static AjaxResultModel DenyApplication(this EntrantsEntities dbContext,InstitutionApplicationListViewModel model,int institutionID) {
			Application app=dbContext.GetApplication(model.ApplicationID);
			if(app==null)
				return new AjaxResultModel("Не найдено заявление");
			if(app.InstitutionID!=institutionID)
				return new AjaxResultModel("Невозможно отклонить данное заявление");

			//итоговый статус в зависимости от текущего
			switch(app.StatusID) {
				case ApplicationStatusType.New:
				app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Removed);
				app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==3);
				app.StatusDecision=model.StatusDecision;
				break;
				case ApplicationStatusType.Failed:
				case ApplicationStatusType.Removed:
				case ApplicationStatusType.Accepted:
				app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Denied);
				app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==3);
				app.StatusDecision=model.StatusDecision;
				app.LastDenyDate=DateTime.Now;
				break;
				default:
				return new AjaxResultModel("Неверный статус");
			}

			dbContext.SaveChanges();
			return new AjaxResultModel { Data=dbContext.FillInstitutionApplicationList(model,false,0,app.ApplicationID).Applications[0] };
		}

		/// <summary>
		/// Публикуем заявления в приказе
		/// </summary>
		public static AjaxResultModel PublishApplications(this EntrantsEntities dbContext,int institutionID) {
			Application[] applications=dbContext.Application.Where(x => x.StatusID==ApplicationStatusType.InOrder&&x.PublishDate==null).ToArray();
			foreach(Application application in applications) {
				application.PublishDate=DateTime.Now;
			}

			dbContext.SaveChanges();
			return new AjaxResultModel { Data=applications.Length };
		}

		/// <summary>
		/// Проверяем заявление на ЕГЭ и количество
		/// </summary>
		public static AjaxResultModel CheckApplication(this EntrantsEntities dbContext,int applicationID) {
			List<ApplicationValidationErrorInfo> result=new ApplicationValidator().ValidateApplication(applicationID);
			int prevStatusID;
			int newStatusID;
			//нет ошибок - успешное
			if(result.Count==0) {
				Application app=dbContext.GetApplication(applicationID);
				prevStatusID=app.StatusID;
				app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Accepted);
				app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==0);
				app.ViolationErrors=null;
				app.StatusDecision="";
				app.LastCheckDate=DateTime.Now;
				dbContext.SaveChanges();
				newStatusID=app.StatusID;
			} else //иначе ставим ошибки
            {
				Application app=dbContext.GetApplication(applicationID);
				prevStatusID=app.StatusID;
				app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Failed);
				var anyErrorCode=result.Select(y => y.Code).First();
				app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==anyErrorCode);
				app.ViolationErrors=String.Join("; ",result.Select(x => x.Message).ToArray());
				app.LastCheckDate=DateTime.Now;
				dbContext.SaveChanges();
				newStatusID=app.StatusID;
			}

			// возвращаем результат клиенту
			string errorString=result.Count==0?"Нет нарушений":String.Join("<br/>",result.Select(x => x.Message).ToArray());
			return new AjaxResultModel(errorString) {
				Data=prevStatusID==newStatusID||(prevStatusID==ApplicationStatusType.Removed&&newStatusID==ApplicationStatusType.Failed)
					 ?dbContext.FillInstitutionApplicationList(new InstitutionApplicationListViewModel(),false,0,applicationID).Applications[0]:null,
				Extra=result.Count==0
			};
		}

		//====================================================================================================================================================================================================
		//                                                    Массовые операции с заявлениями   
		//====================================================================================================================================================================================================

		/// <summary>
		/// Массовое принятие заявлений
		/// </summary>
		public static AjaxResultModel ApproveAllApplications(this EntrantsEntities dbContext,InstitutionApplicationListViewModel model,int institutionID) {
			if(model.applicationIds!=null) {

				for(int i=0;i<model.applicationIds.Length;i++) {
					Application app=dbContext.GetApplication(model.applicationIds[i]);
					if(app==null)
						continue;

					if(app.InstitutionID!=institutionID)
						continue;

					if((model.decisionReasons[i]!=null)&&(model.decisionReasons[i].Length>4000))
						model.decisionReasons[i]=model.decisionReasons[i].Substring(0,4000);

					// меняем статус заявления в зависимости от исходного
					switch(app.StatusID) {
						case ApplicationStatusType.New:
						case ApplicationStatusType.Failed:
						case ApplicationStatusType.Removed:
						app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Accepted);
						app.StatusDecision=model.decisionReasons[i];
						break;

						case ApplicationStatusType.Denied:
						app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.New);
						app.StatusDecision="";
						break;
					}
				}

				dbContext.SaveChanges();
			}

			return new AjaxResultModel("");
		}

		/// <summary>
		/// Массовый отзыв заявлений
		/// </summary>
		public static AjaxResultModel DenyAllApplications(this EntrantsEntities dbContext,InstitutionApplicationListViewModel model,int institutionID) {
			if(model.applicationIds!=null) {
				for(int i=0;i<model.applicationIds.Length;i++) {
					Application app=dbContext.GetApplication(model.applicationIds[i]);
					if(app==null)
						continue;

					if(app.InstitutionID!=institutionID)
						continue;

					if((model.decisionReasons[i]!=null)&&(model.decisionReasons[i].Length>4000))
						model.decisionReasons[i]=model.decisionReasons[i].Substring(0,4000);

					//итоговый статус в зависимости от исходного
					switch(app.StatusID) {
						case ApplicationStatusType.New:
						app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Removed);
						app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==3);

						app.StatusDecision=model.decisionReasons[i];
						break;
						case ApplicationStatusType.Failed:
						case ApplicationStatusType.Removed:
						case ApplicationStatusType.Accepted:
						app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Denied);
						app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==3);
						app.StatusDecision=model.decisionReasons[i];
						app.LastDenyDate=DateTime.Now;
						break;
					}
				}

				dbContext.SaveChanges();
			}
			return new AjaxResultModel("");
		}

		/// <summary>
		/// Массовая проверка заявлений на ЕГЭ и количество
		/// </summary>
		public static AjaxResultModel CheckAllApplications(this EntrantsEntities dbContext,InstitutionApplicationListViewModel model,int institutionID) {
			LogHelper.Log.WarnFormat(">>> Массовая проверка заявлений на ЕГЭ и количество");

			string resultString=string.Empty;
			if(model.applicationIds!=null) {
				for(int i=0;i<model.applicationIds.Length;i++) {
					var result=new ApplicationValidator().ValidateApplication(model.applicationIds[i]);
					if(result.Count==0) {
						Application app=dbContext.GetApplication(model.applicationIds[i]);
						app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Accepted);
						app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==0);
						app.ViolationErrors=null;
						app.StatusDecision="";
						app.LastCheckDate=DateTime.Now;
					} else //иначе ставим ошибки
                    {
						Application app=dbContext.GetApplication(model.applicationIds[i]);
						app.ApplicationStatusType=dbContext.ApplicationStatusType.Single(x => x.StatusID==ApplicationStatusType.Failed);
						var anyErrorCode=result.Select(y => y.Code).First();
						app.ViolationType=dbContext.ViolationType.Single(x => x.ViolationID==anyErrorCode);
						app.ViolationErrors=String.Join("; ",result.Select(x => x.Message).ToArray());
						app.LastCheckDate=DateTime.Now;
					}
					resultString+=(result.Count==0?"Нет нарушений":String.Join("\n",result.Select(x => x.Message).ToArray()))+"#";
					dbContext.SaveChanges();
				}
			}

			return new AjaxResultModel(resultString) { Data=null };
		}
	}
}