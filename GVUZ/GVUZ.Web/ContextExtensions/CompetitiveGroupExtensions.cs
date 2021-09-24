using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Institutions;
using GVUZ.Web.Portlets.Applications;
using GVUZ.Web.ViewModels;
using Microsoft.Practices.ServiceLocation;
using AdmissionVolume = GVUZ.Model.Entrants.AdmissionVolume;
using CompetitiveGroup = GVUZ.Model.Entrants.CompetitiveGroup;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с КГ
	/// </summary>
	public static class CompetitiveGroupExtensions
	{
		private static readonly int CompetitiveGroupPageSize = AppSettings.Get("Search.PageSize", 25);

		/// <summary>
		/// Создание заявления. Метод лучше сдвинуть в заявления, но тут много работы с КГ, так что пусть тут будет
		/// </summary>
		public static InstitutionPrepareApplicationViewModel FillPrepareApplicationViewModel(this EntrantsEntities context, int institutionID)
		{
			InstitutionPrepareApplicationViewModel model = new InstitutionPrepareApplicationViewModel();
			model.InstitutionID = institutionID;
			//вытягиваем доступные КГ (с количеством мест где-нибудь)
			var competitiveGroupBaseQuery = context.CompetitiveGroup.Where(x => x.InstitutionID == institutionID
				&& x.CompetitiveGroupItem.Sum(y =>
					y.NumberBudgetO
						+ y.NumberBudgetOZ
						+ y.NumberBudgetZ
						+ y.NumberPaidO
						+ y.NumberPaidOZ
						+ y.NumberPaidZ
						+ (y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)(z.NumberTargetO + z.NumberTargetOZ + z.NumberTargetZ)) ?? 0)) > 0);

                competitiveGroupBaseQuery = competitiveGroupBaseQuery.Where(x => x.EntranceTestItemC.Any() ||
                x.CompetitiveGroupItem.Any(c => c.CompetitiveGroup.EducationLevelID == EDLevelConst.SPO));

                var competitiveGroups = competitiveGroupBaseQuery
                        //.Where(x => x.EntranceTestItemC.Any())
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            x.CampaignID,
                            x.CompetitiveGroupID,
                            x.Name,
                            x.Course,
                            HasO = x.CompetitiveGroupItem.Sum(y => y.NumberBudgetO + y.NumberPaidO + (y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)z.NumberTargetO) ?? 0)) > 0,
                            HasOZ = x.CompetitiveGroupItem.Sum(y => y.NumberBudgetOZ + y.NumberPaidOZ + (y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)z.NumberTargetOZ) ?? 0)) > 0,
                            HasZ = x.CompetitiveGroupItem.Sum(y => y.NumberBudgetZ + y.NumberPaidZ + (y.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(z => (int?)z.NumberTargetZ) ?? 0)) > 0
                        });
            
			model.CompetitiveGroupNamesByCampaign = competitiveGroups
				.GroupBy(x => x.CampaignID)
				.ToDictionary(x => x.Key ?? 0, x => (IEnumerable)x.Select(y => new { ID = y.CompetitiveGroupID, y.Name, y.Course }).OrderBy(z => z.Name).ToArray());
            
            var b = model.CompetitiveGroupNamesByCampaign.Select(x=>x.Key);
            
            if (model.CompetitiveGroupNamesByCampaign.Count == 0 && competitiveGroupBaseQuery.Any())
				model.ExistsGroupsWithoutEntranceTests = true;

			//общие дропдауны
			model.Campaigns = 
                
                context.Campaign
					.Where(x => x.InstitutionID == institutionID && x.StatusID != CampaignStatusType.Finished)
                    .Select(x => new { ID = x.CampaignID, Name = x.Name })
					.Where(x => b.Contains(x.ID))
					.ToArray();

            model.RegistrationDate = DateTime.Today;
			model.CompetitiveGroupEducationForms = competitiveGroups.ToDictionary(x => x.CompetitiveGroupID.ToString(),
			                                                                      y =>
			                                                                      (y.HasO ? 1 : 0) | (y.HasOZ ? 2 : 0) |
			                                                                      (y.HasZ ? 4 : 0));
			model.IdentityDocumentList =
					context.IdentityDocumentType.OrderBy(x => x.IdentityDocumentTypeID).Select(x => new { ID = x.IdentityDocumentTypeID, x.Name }).ToArray();
			return model;
		}

		/// <summary>
		/// Список доступных КГ по имени или идентификатору
		/// </summary>
		public static CompetitiveGroup[] LoadCompetitiveGroups(this EntrantsEntities context,	int institutionID, int? groupID, string filter)	{
			// should be objects with label/value properties
			var groups = (from cg in context.CompetitiveGroupItem
			        where cg.CompetitiveGroup.InstitutionID == institutionID &&
			              (cg.CompetitiveGroup.CompetitiveGroupID == groupID || groupID == null)
						  && cg.CompetitiveGroup.Name.Contains(filter) 
						  && (cg.NumberBudgetO > 0 || cg.NumberBudgetOZ > 0 || cg.NumberBudgetZ > 0 ||
						  cg.NumberPaidO > 0 || cg.NumberPaidOZ > 0 || cg.NumberPaidZ > 0 ||
                          cg.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(x => x.NumberTargetO + x.NumberTargetOZ + x.NumberTargetZ) > 0)
			        orderby cg.CompetitiveGroup.Name
					group cg.CompetitiveGroup by cg.CompetitiveGroupID into gk
					select gk).ToArray().Select(x => x.First()).ToArray();
			return groups;
		}

		//private class CGItemNumbers
		//{
		//	public int ItemID; //не используется, нужен для корректной материализации данных (Хотя кто мешает пользовать временные обекты с иниц по полям)
		//	public int? NumberBudgetO;
		//	public int? NumberPaidO;
		//	public int? NumberTargetO;
		//	public int? NumberBudgetOZ;
		//	public int? NumberPaidOZ;
		//	public int? NumberTargetOZ;
		//	public int? NumberBudgetZ;
		//	public int? NumberPaidZ;
		//	public int? NumberTargetZ;
  //          public int? NumberQuotaO;
  //          public int? NumberQuotaOZ;
  //          public int? NumberQuotaZ;
  //      }

		///// <summary>
		///// Загружкаем данные для редактирования КГ
		///// </summary>
		//public static CompetitiveGroupViewModel FillCompetitiveGroupViewModel(
		//	this EntrantsEntities context, CompetitiveGroupListViewModel model, int insitutionID)
		//{
		//	if (context == null) throw new ArgumentNullException("context");
		//	if (model == null) throw new ArgumentNullException("model");
		//	if (insitutionID <= 0) throw new ArgumentOutOfRangeException("insitutionID");

		//	//этот код делает вид что что-то делает, чтобы компилятор не ругался на неиспользуемое поле
		//	var dummyItem = new CGItemNumbers();
		//	if (dummyItem.ItemID != 0)
		//		throw new Exception("Fatal exception");

  //          //загрузка фильтров
  //          var fCampaigns = context.Campaign
  //              .Where(x => x.InstitutionID == insitutionID).Select(x => new { ID = x.CampaignID, Name = x.Name })
  //              .OrderBy(x => x.Name)
  //              .ToList();
  //          fCampaigns.Insert(0, new { ID = 0, Name = "[Любая]" });
  //          model.Campaigns = fCampaigns;

  //          var fCourses = context.CampaignEducationLevel.Select(x => x.Course)
  //              .Distinct()
  //              .ToList()
  //              .Select(x => new { ID = x, Name = x.ToString() })
  //              .OrderBy(x => x.ID)
  //              .ToList();
  //          fCourses.Insert(0, new { ID = 0, Name = "[Любой]" });
  //          model.Courses = fCourses;

  //          var fEducationLevels = context.CampaignEducationLevel.Select(x => new { ID = (int)x.EducationLevelID, Name = x.AdmissionItemType.Name })
  //                  .OrderBy(x => x.ID)
  //                  .Distinct()
  //                  .ToArray()
  //                  .ToList();
  //          fEducationLevels.Insert(0, new { ID = 0, Name = "[Любой]" });
  //          model.EducationLevels = fEducationLevels;

  //          model.HasCompaigns = context.Campaign.Any(x => x.InstitutionID == insitutionID);

		//	var groupQuery = context.CompetitiveGroup.Where(x => x.InstitutionID == insitutionID);

		//	model.TotalItemCount = groupQuery.Count();
		//	//фильтруем
		//	if (model.Filter != null)
		//	{
		//		if (!String.IsNullOrEmpty(model.Filter.Name))
		//			groupQuery = groupQuery.Where(x => x.Name.Contains(model.Filter.Name));
		//		if (model.Filter.CampaignID > 0)
		//			groupQuery = groupQuery.Where(x => x.CampaignID == model.Filter.CampaignID);
		//		if (model.Filter.Course > 0)
		//			groupQuery = groupQuery.Where(x => x.Course == model.Filter.Course);
		//		if (model.Filter.EducationLevelID > 0)
		//			groupQuery = groupQuery.Where(x => x.CompetitiveGroupItem.Any(y => y.EducationLevelID == model.Filter.EducationLevelID));
  //              if (!String.IsNullOrEmpty(model.Filter.UID))
  //                  groupQuery = groupQuery.Where(x => x.UID == model.Filter.UID);
  //          }

		//	// сортируем
		//	if (!model.SortID.HasValue)
		//		model.SortID = 1;
		//	if (model.SortID == 1) groupQuery = groupQuery.OrderBy(x => x.Name);
		//	if (model.SortID == -1) groupQuery = groupQuery.OrderByDescending(x => x.Name);
		//	if (model.SortID == 2) groupQuery = groupQuery.OrderBy(x => x.Course);
		//	if (model.SortID == -2) groupQuery = groupQuery.OrderByDescending(x => x.Course);

		//	if (model.SortID == 5) groupQuery = groupQuery.OrderBy(x => x.Campaign.Name);
		//	if (model.SortID == -5) groupQuery = groupQuery.OrderByDescending(x => x.Campaign.Name);

		//	if (!model.PageNumber.HasValue || model.PageNumber < 0)
		//		model.PageNumber = 0;
		//	model.TotalFilteredCount = groupQuery.Count();
		//	model.TotalPageCount = ((Math.Max(model.TotalFilteredCount, 1) - 1) / CompetitiveGroupPageSize) + 1;
		//	groupQuery = groupQuery.Skip(model.PageNumber.Value * CompetitiveGroupPageSize).Take(CompetitiveGroupPageSize);
		//	var groupQueryRes = groupQuery.Select(x => x.CompetitiveGroupID);
		//	var groupIDs = groupQueryRes.ToArray();
		//	var groupIDNames = groupQuery.Select(x => new 
		//		{
		//			ID = x.CompetitiveGroupID,
		//			Name = x.Name,
		//			Course = x.Course,
		//			CampaignID = x.CampaignID,
		//			CampaignName = x.Campaign != null ? x.Campaign.Name : null,
		//			CanEdit = x.Campaign != null && x.Campaign.StatusID != CampaignStatusType.Finished
		//	}).ToArray();

		//	var tempQuery = from vcg in context.CompetitiveGroupItem
		//					where groupQueryRes.Contains(vcg.CompetitiveGroupID)
		//					select new { vcg, CompetitiveGroup = vcg.CompetitiveGroup, Campaign = vcg.CompetitiveGroup.Campaign };

		//	//var itemsList = tempQuery.Select(x => new
		//	//								  {
		//	//									  GroupID = x.vcg.CompetitiveGroupID,
		//	//									  GroupName = x.vcg.CompetitiveGroup.Name,
		//	//									  GroupCourse = x.vcg.CompetitiveGroup.Course,
		//	//									  Item = x.vcg,
		//	//									  TargetOCount = x.vcg.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetO),
		//	//									  TargetOZCount = x.vcg.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetOZ),
		//	//									  TargetZCount = x.vcg.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetZ),
		//	//									  CGItemNumbers = new CGItemNumbers { ItemID = x.vcg.CompetitiveGroupItemID },
		//	//									  DirectionName = "", 
		//	//									  CampaignName = x.Campaign != null ? x.Campaign.Name : null,
		//	//									  CanEdit = x.Campaign != null && x.Campaign.StatusID != CampaignStatusType.Finished
		//	//								  }).ToArray().ToList();
  // //         // TODO: x.vcg.Direction == null ? "" : x.vcg.Direction.Name,

  //          Func<AdmissionVolumeViewModel.AvailFormsInfo[], short, short, short, bool> isFormAvail =
		//		(availForms, levelID, sourceID, formID) =>
		//		availForms.Any(x => x.FormID == formID && x.LevelID == levelID && x.SourceID == sourceID);

  //          Func<short, bool> isQuotaAvail = (levelId) => (levelId == 2 || levelId == 3 || levelId == 5 || levelId == 19);

		//	//вместе с КГ грузим ещё и направления
		//	foreach (var groupIdName in groupIDNames)
		//	{
		//		//добавляем элементы к пустым группам
		//		//if (itemsList.All(x => x.GroupID != groupIdName.ID))
		//		//{
		//		//	itemsList.Add(new
		//		//	{
		//		//		GroupID = groupIdName.ID,
		//		//		GroupName = groupIdName.Name,
		//		//		GroupCourse = groupIdName.Course,
		//		//		Item = new GVUZ.Model.Entrants.CompetitiveGroupItem(),
		//		//		TargetOCount = (int?)null,
		//		//		TargetOZCount = (int?)null,
		//		//		TargetZCount = (int?)null,
		//		//		CGItemNumbers = new CGItemNumbers(),
		//		//		DirectionName = "",
		//		//		CampaignName = groupIdName.CampaignName,
		//		//		CanEdit = groupIdName.CanEdit
		//		//	});
		//		//}
		//		//прячем запрещённые формы
  //              //var availForms = context.CampaignDate.Where(
  //              //    x => x.CampaignID == groupIdName.CampaignID && x.Course == groupIdName.Course && x.IsActive)
  //              //    .Select(x => new { x.EducationFormID, x.EducationSourceID, x.EducationLevelID }).Distinct().ToArray()
  //              //    .Select(x => new AdmissionVolumeViewModel.AvailFormsInfo
  //              //    {
  //              //        FormID = x.EducationFormID,
  //              //        LevelID = x.EducationLevelID,
  //              //        SourceID = x.EducationSourceID
  //              //    }).ToArray();

		//		//проставляем числа, если разрешены (иначе будет выводиться пустое поле)
  //              //foreach (var r in itemsList.Where(x => x.GroupID == groupIdName.ID).ToArray())
  //              //{
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Budget, EDFormsConst.O))
  //              //        r.CGItemNumbers.NumberBudgetO = r.Item.NumberBudgetO;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Budget, EDFormsConst.O) && isQuotaAvail(r.Item.EducationLevelID))
  //              //        r.CGItemNumbers.NumberQuotaO = r.Item.NumberQuotaO;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Paid, EDFormsConst.O))
  //              //        r.CGItemNumbers.NumberPaidO = r.Item.NumberPaidO;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Target, EDFormsConst.O)) 
  //              //        r.CGItemNumbers.NumberTargetO = r.TargetOCount;

  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Budget, EDFormsConst.OZ))
  //              //        r.CGItemNumbers.NumberBudgetOZ = r.Item.NumberBudgetOZ;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Budget, EDFormsConst.OZ) && isQuotaAvail(r.Item.EducationLevelID))
  //              //        r.CGItemNumbers.NumberQuotaOZ = r.Item.NumberQuotaOZ;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Paid, EDFormsConst.OZ))
  //              //        r.CGItemNumbers.NumberPaidOZ = r.Item.NumberPaidOZ;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Target, EDFormsConst.OZ))
  //              //        r.CGItemNumbers.NumberTargetOZ = r.TargetOZCount;

  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Budget, EDFormsConst.Z))
  //              //        r.CGItemNumbers.NumberBudgetZ = r.Item.NumberBudgetZ;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Budget, EDFormsConst.Z) && isQuotaAvail(r.Item.EducationLevelID))
  //              //        r.CGItemNumbers.NumberQuotaZ = r.Item.NumberQuotaZ;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Paid, EDFormsConst.Z))
  //              //        r.CGItemNumbers.NumberPaidZ = r.Item.NumberPaidZ;
  //              //    if (isFormAvail(availForms, r.Item.EducationLevelID, EDSourceConst.Target, EDFormsConst.Z))
  //              //        r.CGItemNumbers.NumberTargetZ = r.TargetZCount;
  //              //}
		//	}

		//	//var items = itemsList.ToArray();
		//	//Array.Sort(items, (x, y) => Array.IndexOf(groupIDs, x.GroupID)
		//	//	.CompareTo(Array.IndexOf(groupIDs, y.GroupID)));

		//	//Dictionary<int, int> entranceTestCount = context.GetEntranceTestCount(items.Select(x => x.GroupID).ToList());

		//	int prevGroupID = 0;
		//	model.TreeItems = new List<List<CompetitiveGroupViewModel>>();
		//	// формируем результирующий список
		//	//foreach (var item in items)
		//	//{
		//	//	CompetitiveGroupViewModel row = new CompetitiveGroupViewModel();
		//	//	//var item = items[i];
		//	//	if (item.GroupID != prevGroupID)
		//	//	{
		//	//		model.TreeItems.Add(new List<CompetitiveGroupViewModel>());
		//	//		prevGroupID = item.GroupID;
		//	//	}

		//	//	var directions = model.TreeItems[model.TreeItems.Count - 1];
		//	//	directions.Add(row);
				
		//	//	row.GroupID = item.GroupID;
		//	//	int etCount;
		//	//	if (entranceTestCount.TryGetValue(row.GroupID, out etCount)) row.EntranceTestCount = etCount;
		//	//	row.GroupName = item.GroupName;

		//	//	row.EducationalLevelID = item.Item.EducationLevelID;
		//	//	row.EducationalLevelName = item.Item.EducationLevelID > 0 ?
		//	//		DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, item.Item.EducationLevelID) : "";
		//	//	row.DirectionID = item.Item.DirectionID;
		//	//	row.DirectionName = item.DirectionName;
		//	//	row.CourseName = GetCourseName(item.GroupCourse);
		//	//	row.CampaignName = item.CampaignName;

		//	//	row.NumberBudgetO = item.CGItemNumbers.NumberBudgetO;
		//	//	row.NumberBudgetOZ = item.CGItemNumbers.NumberBudgetOZ;
		//	//	row.NumberBudgetZ = item.CGItemNumbers.NumberBudgetZ;
		//	//	row.NumberPaidO = item.CGItemNumbers.NumberPaidO;
		//	//	row.NumberPaidOZ = item.CGItemNumbers.NumberPaidOZ;
		//	//	row.NumberPaidZ = item.CGItemNumbers.NumberPaidZ;
		//	//	row.NumberTargetO = item.CGItemNumbers.NumberTargetO;
		//	//	row.NumberTargetOZ = item.CGItemNumbers.NumberTargetOZ;
		//	//	row.NumberTargetZ = item.CGItemNumbers.NumberTargetZ;
  // //             row.NumberQuotaO = item.CGItemNumbers.NumberQuotaO;
  // //             row.NumberQuotaOZ = item.CGItemNumbers.NumberQuotaOZ;
  // //             row.NumberQuotaZ = item.CGItemNumbers.NumberQuotaZ;
  // //             row.CanEdit = item.CanEdit;
		//	//}

		//	return model;
		//}

        /// <summary>
        /// Получем текстовое имя курса
        /// </summary>
        public static string GetCourseName(int course)
        {
            //if (course == 0 || course > 10)
            //    throw new ArgumentOutOfRangeException("course");
            return course <= 0 ? string.Empty : course + " курс";
        }

		/// <summary>
		/// Тип фильтров в КГ
		/// </summary>
		public enum AvailableDirectionsFilter
		{
			NotExact = 0,
			Exact = 1,
			Any = 2
		}

		/// <summary>
		/// Получаем оставшиеся доступные направления для выбранной КГ
		/// </summary>
		public static AjaxResultModel GetRemainedAvailableDirections(this EntrantsEntities context, int groupID, int institutionID, int educationLevelID, int[] selectedDirections, AvailableDirectionsFilter filterType)
		{
			if (selectedDirections == null) selectedDirections = new int[0];
			var directions = context.AllowedDirections
				.Where(x => x.InstitutionID == institutionID && x.AdmissionItemTypeID == educationLevelID)
				.Select(x => new { ID = x.DirectionID, x.Direction.Name, x.Direction.Code, QualificationCode = x.Direction.QUALIFICATIONCODE, NewCode = x.Direction.NewCode  })
				.Distinct().ToArray().Where(x => Array.IndexOf(selectedDirections, x.ID) < 0)
				.OrderBy(x => x.Code)
				.ThenBy(x => x.Name)
				.ToArray();
			if (selectedDirections.Length > 0) //если выбрали хотя бы одно направление, то фильруем
			{
				if (filterType == AvailableDirectionsFilter.Exact || filterType == AvailableDirectionsFilter.NotExact)
				{
					int[] availableByEntranceTests = context.GetDirectionsWithSameEntranceTests(selectedDirections, filterType == AvailableDirectionsFilter.Exact);
					directions = directions.Where(x => Array.IndexOf(availableByEntranceTests, x.ID) >= 0).ToArray();
				}
			}

			return new AjaxResultModel { Data = directions };
		}

		/// <summary>
		/// Возвращаем направления с теми же ВИ
		/// </summary>
		/// <param name="isExact">false - 505 приказ, совпадение по трём предметам</param>
		public static int[] GetDirectionsWithSameEntranceTests(this EntrantsEntities context, int[] directionIDs, bool isExact)
		{
			var correctLinks = GetAvailableLinksWithSameEntranceTests(context, directionIDs, isExact);
			return context.DirectionSubjectLinkDirection.Where(x => correctLinks.Contains(x.LinkID)).Select(x => x.DirectionID).ToArray();
		}

		/// <summary>
		/// Возвращаем доступные линки по направлениям
		/// </summary>
		/// <param name="isExact">false - 505 приказ, совпадение по трём предметам</param>
		public static int[] GetAvailableLinksWithSameEntranceTests(this EntrantsEntities context, int[] directionIDs, bool isExact)
		{
			int[] linkIDs = context.DirectionSubjectLinkDirection.Where(x => directionIDs.Contains(x.DirectionID)).Select(x => x.LinkID).Distinct().ToArray();
			if (linkIDs.Length == 0)
				return new int[0];

			////профильные должны быть у всех одинаковыми
			//var firstLinkID = linkIDs[0];
			//int profileSubjectID = context.DirectionSubjectLink.Where(x => x.ID == firstLinkID).Select(x => x.ProfileSubjectID).First();
			//теперь профильные могут быть разными, поэтому дополнительно фильтруем
			var profileSubjects = context.DirectionSubjectLink.Where(x => linkIDs.Contains(x.ID)).Select(x => x.ProfileSubjectID).ToArray();
			if (profileSubjects.Distinct().Count() > 1) //разные профильные, ничего не сделать, не возвращаем ничего
				return new int[0];
			int? profileSubjectID = profileSubjects[0];
			var subjects = context.DirectionSubjectLinkSubject.Where(x => linkIDs.Contains(x.LinkID)).Select(x => new { x.SubjectID, x.LinkID }).ToArray();
			
			//берём общие для всех испытания
			List<int> intersectedSubjects = subjects.Select(x => x.SubjectID).Distinct().ToList();
			intersectedSubjects = subjects.GroupBy(x => x.LinkID)
				.Aggregate(intersectedSubjects, (current, sub) => current.Intersect(sub.Select(x => x.SubjectID)).ToList());

			//если не точное совпадение то требуем, чтобы у остальных испытаний было совпадание по трём (или меньше)
			int subLength = intersectedSubjects.Count;
			if (!isExact)
				subLength = Math.Min(subLength, 3);

			var possibleLinks = context.DirectionSubjectLinkSubject
				.Where(x => x.DirectionSubjectLink.ProfileSubjectID == profileSubjectID && intersectedSubjects.Contains(x.SubjectID))
				.Select(x => new { x.LinkID, x.SubjectID }).ToArray().GroupBy(x => x.LinkID);
			List<int> correctLinks = (from possibleLink in possibleLinks
									  where possibleLink.Select(x => x.SubjectID).Intersect(intersectedSubjects).Count() >= subLength
									  select possibleLink.Key).ToList();

			return correctLinks.ToArray();
		}

		/// <summary>
		/// Загружаем модель для редактирования КГ
		/// </summary>
		public static CompetitiveGroupEditViewModel FillCompetitiveGroupEditViewModel(
			this EntrantsEntities context, CompetitiveGroupEditViewModel model, int institutionID, int groupID)
		{
			var group = context.CompetitiveGroup
				.Include(x => x.Campaign).SingleOrDefault(x => x.InstitutionID == institutionID && x.CompetitiveGroupID == groupID);
		    if (group == null)
		        return new CompetitiveGroupEditViewModel();

			model.Name = group.Name;
			model.CourseID = group.Course;
			model.Uid = group.UID;
			model.GroupID = group.CompetitiveGroupID;
			model.DirectionFilterType = group.DirectionFilterType;
			//model.AllowAnyDirectionsFilterType = !context.EntranceTestItemC
			//	.Where(x => x.CompetitiveGroupID == model.GroupID && x.EntranceTestTypeID == EntranceTestType.ProfileType).Any();
			model.AllowAnyDirectionsFilterType = true;

			// разрешённые формы из дат ПК
            //var availForms = context.CampaignDate
            //    .Where(x => x.CampaignID == group.CampaignID && x.Course == group.Course && x.IsActive)
            //    .Select(x => new { x.EducationSourceID, x.EducationFormID, x.EducationLevelID })
            //    .Distinct()
            //    .ToArray();

			//функция выбора разрешённых
            //Func<int, int, CompetitiveGroupEditViewModel.OOZZInfo> getForms =
            //    (source, edLevel) => new CompetitiveGroupEditViewModel.OOZZInfo
            //    {
            //        HasO =
            //            availForms.Any(
            //                x => x.EducationSourceID == source && x.EducationFormID == EDFormsConst.O && x.EducationLevelID == edLevel),
            //        HasOZ =
            //            availForms.Any(
            //                x => x.EducationSourceID == source && x.EducationFormID == EDFormsConst.OZ && x.EducationLevelID == edLevel),
            //        HasZ =
            //            availForms.Any(
            //                x => x.EducationSourceID == source && x.EducationFormID == EDFormsConst.Z && x.EducationLevelID == edLevel),
            //    };

			// по идее всегда есть. Оставлено для на всякий случай (вдруг логика изменится)
			if (group.Campaign != null)
			{
				model.CampaignID = group.CampaignID ?? 0;
				model.CampaignName = group.Campaign.Name;
				model.CanEdit = group.Campaign.StatusID != CampaignStatusType.Finished;
			}
			// разрешённые направления
			model.AllowedDirections = context.AllowedDirections
				.Where(x => x.InstitutionID == institutionID)
				.OrderBy(x => x.Direction.Name)
				.Select(x => new { ID = x.DirectionID, x.Direction.Name, x.Direction.Code, x.Direction.NewCode }).Distinct().ToArray();

			// кеш направлений для попапов
			model.CachedDirections = AdmissionVolumeExtensions.GetDirectionsInfo(
				context.Direction
					.Where(x => x.AllowedDirections.Any(y => y.DirectionID == x.DirectionID && y.InstitutionID == institutionID
						&& context.CampaignEducationLevel
							.Where(z => z.CampaignID == model.CampaignID && z.Course == model.CourseID)
							.Select(z => z.EducationLevelID).Contains(y.AdmissionItemTypeID.Value))))
				.ToDictionary(x => x.DirectionID.ToString(), x => x);

			model.HasBudget = new Dictionary<int, CompetitiveGroupEditViewModel.OOZZInfo>();
			model.HasPaid = new Dictionary<int, CompetitiveGroupEditViewModel.OOZZInfo>();
			model.HasTarget = new Dictionary<int, CompetitiveGroupEditViewModel.OOZZInfo>();

			// есть всегда
			if (group.Campaign != null)
			{
				var allowedLevels =
					context.CampaignEducationLevel.Where(x => x.CampaignID == group.CampaignID && x.Course == group.Course)
						.Select(x => new { ID = x.AdmissionItemType.ItemTypeID, Name = x.AdmissionItemType.Name })
						.Distinct()
						.ToArray();

                //foreach (var allowedLevel in allowedLevels)
                //{
                //    model.HasBudget[allowedLevel.ID] = getForms(EDSourceConst.Budget, allowedLevel.ID);
                //    model.HasPaid[allowedLevel.ID] = getForms(EDSourceConst.Paid, allowedLevel.ID);
                //    model.HasTarget[allowedLevel.ID] = getForms(EDSourceConst.Target, allowedLevel.ID);
                //}

                model.AllowedEdLevels = allowedLevels;
			}
			else
			{
				model.AllowedEdLevels = context.AdmissionItemType
					.Where(x => x.ItemLevel == 2)
					.Select(x => new { ID = x.ItemTypeID, Name = x.Name }).ToArray();
			}

			// выгружаем целевой приём для КГ
			var targets =
				context.CompetitiveGroupTarget
					.Where(x => x.CompetitiveGroupTargetItem.Any(y => y.CompetitiveGroup.CompetitiveGroupID == groupID))
					.OrderBy(x => x.Name)
					.Select(x => new
					{
						x.CompetitiveGroupTargetID,
						x.Name,
						x.UID,
						//нельзя удалять если есть заявления КГ у которых есть данная организация и данная КГ
						CanDelete = !context.Application.Any(y => y.ApplicationSelectedCompetitiveGroupTarget.Any(z => z.CompetitiveGroupTargetID == x.CompetitiveGroupTargetID)
						                                          && y.OrderCompetitiveGroupID == groupID)
					})
					.ToArray();

			model.Organizations = targets.Select(x => new CompetitiveGroupEditViewModel.OrganizationData
			{
				Name = x.Name,
				UID = x.UID,
				ID = x.CompetitiveGroupTargetID,
				CanDelete = x.CanDelete
			}).ToArray();

			//направления ЦП
			var targetItems = context.CompetitiveGroupTargetItem
				.Where(x => x.CompetitiveGroupID == groupID)
				.Select(
					x =>
						new
						{
                            x.CompetitiveGroupID,
							x.CompetitiveGroupTargetItemID,
							x.CompetitiveGroupTargetID,
							x.NumberTargetO,
							x.NumberTargetOZ,
							x.NumberTargetZ,
                            x.CompetitiveGroup,                       
							x.CompetitiveGroupTarget.UID
						}).ToArray();

			var items =
				context.CompetitiveGroupItem.Include(x => x.CompetitiveGroup.Direction).Where(x => x.CompetitiveGroupID == groupID).ToArray();
			List<CompetitiveGroupEditViewModel.DirectionInfo> rows = new List<CompetitiveGroupEditViewModel.DirectionInfo>();
			// строим результирующую таблицу
			foreach (var item in items)
			{
				CompetitiveGroupEditViewModel.DirectionInfo info = new CompetitiveGroupEditViewModel.DirectionInfo
				{
					DirectionID = item.CompetitiveGroup.DirectionID.Value,
					EducationLevelID = item.CompetitiveGroup.EducationLevelID.Value,
					DirectionName = item.CompetitiveGroup.Direction.Name,
					Data = new int[9 + (model.Organizations.Length * 3)],
					DataTargetUIDs = new string[model.Organizations.Length],
				};
				info.Data[0] = item.NumberBudgetO.Value;
				info.Data[1] = item.NumberBudgetOZ.Value;
				info.Data[2] = item.NumberBudgetZ.Value;
				info.Data[6] = item.NumberPaidO.Value;
				info.Data[7] = item.NumberPaidOZ.Value;
				info.Data[8] = item.NumberPaidZ.Value;
                info.Data[3] = item.NumberQuotaO.HasValue ? item.NumberQuotaO.Value : 0;
                info.Data[4] = item.NumberQuotaOZ.HasValue ? item.NumberQuotaOZ.Value : 0;
                info.Data[5] = item.NumberQuotaZ.HasValue ? item.NumberQuotaZ.Value : 0;
                //info.UID = item.UID;
				for (int iList = 0; iList < model.Organizations.Length; iList++)
				{
					var i = iList;
					CompetitiveGroupItem item1 = item;
					info.Data[9 + (i * 3)] =
						targetItems
							.Where(x => x.CompetitiveGroupTargetItemID == item1.CompetitiveGroupItemID && x.CompetitiveGroupTargetID == model.Organizations[i].ID)
							.Select(x => x.NumberTargetO.Value).FirstOrDefault();
					info.Data[9 + (i * 3) + 1] =
						targetItems
							.Where(x => x.CompetitiveGroupTargetItemID == item1.CompetitiveGroupItemID && x.CompetitiveGroupTargetID == model.Organizations[i].ID)
							.Select(x => x.NumberTargetOZ.Value).FirstOrDefault();
					info.Data[9 + (i * 3) + 2] =
						targetItems
							.Where(x => x.CompetitiveGroupTargetItemID == item1.CompetitiveGroupItemID && x.CompetitiveGroupTargetID == model.Organizations[i].ID)
							.Select(x => x.NumberTargetZ.Value).FirstOrDefault();
					info.DataTargetUIDs[i] =
						targetItems
							.Where(x => x.CompetitiveGroupTargetItemID == item1.CompetitiveGroupItemID && x.CompetitiveGroupTargetID == model.Organizations[i].ID)
							.Select(x => x.UID).FirstOrDefault();
				}

				rows.Add(info);
			}

			//сортируем складываем в модель
			model.Rows = rows.OrderBy(x => x.EducationLevelID).ThenBy(x => x.DirectionName).ToArray();

			// количество ВИ (отображается в заголовке таба
			model.EntranceTestCount = context.GetEntranceTestCount(groupID);

			return model;
		}

		/// <summary>
		/// Загружает только часть модели, для редактирования ВИ (оставляет только то, что требуется для отображения)
		/// </summary>
		public static CompetitiveGroupEditViewModel FillCompetitiveGroupEditViewModelForEntranceTestInfo(
			this EntrantsEntities context, CompetitiveGroupEditViewModel model, int institutionID, int groupID)
		{
			var group = context.CompetitiveGroup
				.Include(x => x.Campaign)
				.Single(x => x.InstitutionID == institutionID && x.CompetitiveGroupID == groupID);
			model.Name = group.Name;
			model.CourseID = group.Course;
			//model.EducationalLevelID = group.EducationalLevelID;
			model.Uid = group.UID;
			model.GroupID = group.CompetitiveGroupID;
			if (group.Campaign != null)
			{
				model.CampaignID = group.CampaignID ?? 0;
				model.CampaignName = group.Campaign.Name;
			}
			//model.EducationLevelName = context.AdmissionItemType.Where(x => x.ItemTypeID == group.EducationalLevelID).Select(x => x.Name).FirstOrDefault() ?? "";

			model.EntranceTestCount = context.GetEntranceTestCount(groupID);
			return model;
		}

		/// <summary>
		/// Загружаем модель для добавления КГ
		/// </summary>
		public static CompetitiveGroupAddViewModel FillCompetitiveGroupAddViewModel(
			this EntrantsEntities context, CompetitiveGroupAddViewModel model, int institutionID)
		{
			model.Campaigns = context.Campaign.Where(x => x.InstitutionID == institutionID && x.StatusID != CampaignStatusType.Finished)
				.OrderBy(x => x.Name)
				.Select(x => new
				             {
				             	CampaignID = x.CampaignID,
				             	Name = x.Name,
				             	Courses = x.CampaignEducationLevel.Select(y => y.Course).OrderBy(y => y).Distinct(),
				             	EducationLevels =
				             	x.CampaignEducationLevel.Select(y => y.AdmissionItemType).OrderBy(y => y.DisplayOrder).Distinct(),
								Pairs = x.CampaignEducationLevel.Select(y => new { y.EducationLevelID, y.Course })
				             }).ToArray()
				.Select(x => new CompetitiveGroupAddViewModel.CampaignData
				             {
				             	Campaign = new CompetitiveGroupAddViewModel.BaseData { ID = x.CampaignID, Name = x.Name },
								Courses = x.Courses.Select(y => new CompetitiveGroupAddViewModel.BaseData { ID = y, Name = GetCourseName(y) }).ToArray(),
								Campaign_Courses = x.Pairs.Select(y => x.CampaignID + "_" + y.Course).ToArray()
				             }).ToArray();

			return model;
		}

		/// <summary>
		/// Валидация модели добавления КГ
		/// </summary>
		public static bool IsValidGroupAddViewModel(
			this EntrantsEntities context, CompetitiveGroupAddViewModel model, int institutionID,
			ref Dictionary<string, string> validationMessages)
		{
			if (CompetitiveGroup.IsCompetitiveGroupExists(context, model.Name, model.CampaignID, institutionID))
			{
				validationMessages.Add("Name", "Конкурс с таким именем уже существует. Название конкурса должно быть уникальным в рамках одной приемной кампании.");
				return false;
			}

			if (!context.Campaign.Where(x => x.CampaignID == model.CampaignID && x.InstitutionID == institutionID && x.StatusID != CampaignStatusType.Finished).Any())
			{
				validationMessages.Add("CampaignID", "Неверная приемная кампания");
				return false;
			}

			if (!context.CampaignEducationLevel.Where(x => x.CampaignID == model.CampaignID && x.Course == model.CourseID /*&& x.EducationLevelID == model.EducationLevelID*/).Any())
			{
				validationMessages.Add("CampaignID", "Неверная приемная кампания");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Проверка на существование КГ
		/// </summary>
		public static bool CheckCompetitiveGroupOnExists(this EntrantsEntities context, int groupID, int campaignID, int institutionID, ref Dictionary<string, string> validationMessages){
			if (!CompetitiveGroup.IsCompetitiveGroupExists(context, groupID, institutionID))	{
				validationMessages.Add("CompetitionGroup", "Указанный конкурс отсутствует");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Проверка на корректность модели создания нового заявления
		/// </summary>
		public static bool IsValidInstitutionPrepareApplicationViewModel(this EntrantsEntities context, InstitutionPrepareApplicationViewModel model, int institutionID, ref Dictionary<string, string> validationMessages, out bool isGroupAbsent){
			isGroupAbsent = false;
			//ситуация уже ошибочная, так что без разницы, что ошибки по одной будем выдавать
			model.SelectedCompetitiveGroupIDs = model.SelectedCompetitiveGroupIDs ?? new int[0];
			foreach (var selectedCompetitiveGroupID in model.SelectedCompetitiveGroupIDs){
				if (!CheckCompetitiveGroupOnExists(context, selectedCompetitiveGroupID, model.CampaignID, institutionID, ref validationMessages))
				{
					isGroupAbsent = true;
					return false;
				}
				//проверяем на пустые группы
				if (context.LoadCompetitiveGroups(institutionID, selectedCompetitiveGroupID, "").Length == 0)
				{
					validationMessages["CompetitionGroup"] = "Некорректный конкурс";
					return false;
				}
         }

            #region Устаревшая проверка выбора форм и источников финансирования
            //if (model.EducationForms == null || !model.EducationForms.HasAny)
            //{
            //    validationMessages["EducationForms"] = "Необходимо выбрать форму обучения";
            //    return false;
            //}

            //if (model.EducationForms.TargetO && model.SelectedTargetOrganizationIDO == 0)
            //{
            //    validationMessages["SelectedTargetOrganizationIDO"] = "Необходимо выбрать организацию целевого приема";
            //    return false;
            //}

            //if (model.EducationForms.TargetOZ && model.SelectedTargetOrganizationIDOZ == 0)
            //{
            //    validationMessages["SelectedTargetOrganizationIDOZ"] = "Необходимо выбрать организацию целевого приема";
            //    return false;
            //}

            //if (model.EducationForms.TargetZ && model.SelectedTargetOrganizationIDZ == 0)
            //{
            //    validationMessages["SelectedTargetOrganizationIDZ"] = "Необходимо выбрать организацию целевого приема";
            //    return false;
            //}
            #endregion

            #region Проверка на то, что для каждого направления выбрана хотя бы одна комбинация источника финансирования и формы обучения
            var directionIds = model.Priorities.ApplicationPriorities
                .Select(x => new { x.CompetitiveGroupItemId })
                .Distinct()
                .ToArray();
            
            bool error = false;

            foreach (var directionId in directionIds)
            {
                var selectedItems = model.Priorities.ApplicationPriorities
                    .Where(x => x.CompetitiveGroupItemId == directionId.CompetitiveGroupItemId && x.Priority.HasValue)
                    .Count();

                if (selectedItems == 0)
                {
                    validationMessages[directionId.CompetitiveGroupItemId.ToString()] = "Для специальности на выбрано ни одной комбинации формы обучения и источника финансирования";
                    error = true;
                }
            }

            if (error)
            {
                validationMessages["Fake"] = "Fake";
                return false;
            }

            #endregion

            #region Проверка на то, что все приоритеты различны
            if (model.CheckUniqueBeforeCreate)
            {
                int totalValues = model.Priorities.ApplicationPriorities.Count(x => x.Priority.HasValue && x.Priority.Value != 0);
                int distinctValues = model.Priorities.ApplicationPriorities
                    .Select(x => x.Priority)
                    .Distinct()
                    .Count(x => x.HasValue && x.Value != 0);

                if (totalValues > distinctValues)
                {
                    validationMessages["NonUniquePriorities"] = "Для нескольких условий приема указаны одинаковые приоритеты. Вы уверены, что хотите продолжить?";
                    return false;
                }
            }
            #endregion

            #region Проверка на то, что все приоритеты - нули
            if (model.CheckZerozBeforeCreate)
            {
                var zeroCount = model.Priorities.ApplicationPriorities.Count(x => x.Priority.HasValue && x.Priority.Value == 0);

                if (zeroCount > 0 && zeroCount != model.Priorities.ApplicationPriorities.Count)
                {
                    validationMessages["zeroMessage"] = "Нули могут быть указаны только в случае приема без учета приоритетов. Вы уверены, что хотите продолжить?";
                    return false;
                }
            }
            #endregion

            if (IdentityDocumentViewModel.IsSeriesRequired(model.IdentityDocumentTypeID) && String.IsNullOrEmpty(model.DocumentSeries))
			{
				validationMessages["DocumentSeries"] = "";
				validationMessages["DocumentNumber"] = "Неверная серия у документа";
				return false;
			}

			return true; 
		}

		/// <summary>
		/// Создаём новую КГ
		/// </summary>
		public static int SaveCompetitiveGroupAddViewModel(this EntrantsEntities context, UrlHelper urlHelper, CompetitiveGroupAddViewModel model, int institutionID){
			CompetitiveGroup group = new CompetitiveGroup();
			group.Name = model.Name;
            group.CreatedDate = DateTime.Now;
			group.CampaignID = model.CampaignID;
			group.Course = model.CourseID;
			group.InstitutionID = institutionID;
			group.DirectionFilterType = (int)AvailableDirectionsFilter.NotExact;
			context.CompetitiveGroup.AddObject(group);
			context.SaveChanges();
			return group.CompetitiveGroupID;
		}

		/// <summary>
		/// Сохраняем КГ. Вся логика в <see cref="CompetitiveGroupModelSaver"/>
		/// </summary>
		public static AjaxResultModel SaveCompetitiveGroupEditViewModel(this EntrantsEntities context, CompetitiveGroupEditViewModel model, int institutionID){
			return new CompetitiveGroupModelSaver(context, model, institutionID).SaveCompetitiveGroupEditViewModel();
		}

		/// <summary>
		/// Проверка на уникальность организации ЦП
		/// </summary>
		public static AjaxResultModel CompetitiveGroupCheckTargetOrganizationsUnique(this EntrantsEntities dbContext, int groupID, CompetitiveGroupEditViewModel.OrganizationData org, int instiutionID)
		{
			if (org != null && !String.IsNullOrEmpty(org.UID))
			{
				if (dbContext.CompetitiveGroupTarget.Any(x => x.InstitutionID == instiutionID && x.Name != org.Name && x.CompetitiveGroupTargetID != org.ID && x.UID == org.UID))
					return new AjaxResultModel().SetIsError("tbNewOrgUID", "Существует организация с данным UID но другим названием.");
			}

			return new AjaxResultModel();
		}

		/// <summary>
		/// Возвращаем количество допустимых мест для КГ
		/// </summary>
		public static AjaxResultModel GetCompetitiveGroupAvailableDirectionCount(
			this EntrantsEntities dbContext, int institutionID, int groupID, int directionID, int educationLevelID)
		{
			return new AjaxResultModel
			{
				Data = GetCompetitiveGroupAvailableDirectionCountInternal(dbContext, institutionID, groupID, directionID, educationLevelID)
			};
		}

		/// <summary>
		/// Возвращаем количество допустимых мест для КГ
		/// </summary>
		internal static int[] GetCompetitiveGroupAvailableDirectionCountInternal(
			this EntrantsEntities dbContext, int institutionID, int groupID, int directionID, int educationLevelID)
		{
			var gr = dbContext.CompetitiveGroup.FirstOrDefault(x => x.InstitutionID == institutionID && x.CompetitiveGroupID == groupID);
			// направления в КГ
			var groupItems = (from x in dbContext.CompetitiveGroupItem
			        where x.CompetitiveGroupID != groupID
			              && x.CompetitiveGroup.InstitutionID == institutionID
						  && x.CompetitiveGroup.EducationLevelID == educationLevelID
						  && x.CompetitiveGroup.Course == gr.Course
						  && x.CompetitiveGroup.CampaignID == gr.CampaignID
			              && x.CompetitiveGroup.DirectionID == directionID
			        select new
			               {
			               		x.NumberBudgetO,
								x.NumberBudgetOZ,
								x.NumberBudgetZ,
                                x.NumberQuotaO,
                                x.NumberQuotaOZ,
                                x.NumberQuotaZ,
								x.NumberPaidO,
								x.NumberPaidOZ,
								x.NumberPaidZ
			               }).ToArray();
			//целевые направления
			var target = (from x in dbContext.CompetitiveGroupTargetItem
							  where x.CompetitiveGroup.CompetitiveGroupID != groupID
									&& x.CompetitiveGroup.InstitutionID == institutionID
									&& x.CompetitiveGroup.EducationLevelID == educationLevelID
									&& x.CompetitiveGroup.DirectionID == directionID
									&& x.CompetitiveGroup.Course == gr.Course
									&& x.CompetitiveGroup.CampaignID == gr.CampaignID
							  select new
							  {
								  x.NumberTargetO,
								  x.NumberTargetOZ,
								  x.NumberTargetZ,
							  }).ToArray();
			//объём приёма
			AdmissionVolume volumeItems = (from x in dbContext.AdmissionVolume
				                   where x.InstitutionID == institutionID
				                         && x.AdmissionItemTypeID == educationLevelID
				                         && x.DirectionID == directionID
				                         && x.Course == gr.Course
				                         && x.CampaignID == gr.CampaignID
				                   select x).FirstOrDefault();
			//нет ообъёма - нули
			if (volumeItems == null)
				return new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // возвращаем количество мест, но не меньше ноля
            return new[]
            {
                Math.Max(0, volumeItems.NumberBudgetO - groupItems.Sum(x => x.NumberBudgetO).Value),
				Math.Max(0, volumeItems.NumberBudgetOZ - groupItems.Sum(x => x.NumberBudgetOZ).Value),
				Math.Max(0, volumeItems.NumberBudgetZ - groupItems.Sum(x => x.NumberBudgetZ).Value),
				Math.Max(0, volumeItems.NumberQuotaO.HasValue ? (volumeItems.NumberQuotaO.Value - groupItems.Sum(x => x.NumberQuotaO.HasValue ? x.NumberQuotaO.Value : 0)) : 0),
				Math.Max(0, volumeItems.NumberQuotaOZ.HasValue ? (volumeItems.NumberQuotaOZ.Value - groupItems.Sum(x => x.NumberQuotaOZ.HasValue ? x.NumberQuotaOZ.Value : 0)) : 0),
				Math.Max(0, volumeItems.NumberQuotaZ.HasValue ? (volumeItems.NumberQuotaZ.Value - groupItems.Sum(x => x.NumberQuotaZ.HasValue ? x.NumberQuotaZ.Value : 0)) : 0),
				Math.Max(0, volumeItems.NumberPaidO - groupItems.Sum(x => x.NumberPaidO).Value),
				Math.Max(0, volumeItems.NumberPaidOZ - groupItems.Sum(x => x.NumberPaidOZ).Value),
				Math.Max(0, volumeItems.NumberPaidZ - groupItems.Sum(x => x.NumberPaidZ).Value),
				Math.Max(0, volumeItems.NumberTargetO - target.Sum(x => x.NumberTargetO).Value),
				Math.Max(0, volumeItems.NumberTargetOZ - target.Sum(x => x.NumberTargetOZ.Value)),
				Math.Max(0, volumeItems.NumberTargetZ - target.Sum(x => x.NumberTargetZ).Value),
			};
		}

		/// <summary>
		/// Возвращаем количество заявлений в приказе в данной КГ
		/// </summary>
		public static AjaxResultModel GetCompetitiveGroupApplicationInOrderCount(
			this EntrantsEntities dbContext, int institutionID, int groupID, int competitiveGroupItemID)
		{
			return new AjaxResultModel
			{
				Data = GetCompetitiveGroupApplicationInOrderCountInternal(dbContext, institutionID, groupID, competitiveGroupItemID)
			};
		}

		/// <summary>
		/// Возвращаем количество заявлений в приказе в данной КГ
		/// </summary>
		internal static int[] GetCompetitiveGroupApplicationInOrderCountInternal(
			this EntrantsEntities dbContext, int institutionID, int groupID, int competitiveGroupItemID)
		{
			var apps = dbContext.Application.Where(x => x.StatusID == ApplicationStatusType.InOrder && x.OrderCompetitiveGroupID == groupID
				&& x.OrderCompetitiveGroupItemID == competitiveGroupItemID)
				.Select(x => new { x.OrderEducationFormID, x.OrderEducationSourceID }).ToArray();
			
			return new[]
            {
	            apps.Count(x => x.OrderEducationFormID == EDFormsConst.O && x.OrderEducationSourceID == EDSourceConst.Budget),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.OZ && x.OrderEducationSourceID == EDSourceConst.Budget),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.Z && x.OrderEducationSourceID == EDSourceConst.Budget),
	            apps.Count(x => x.OrderEducationFormID == EDFormsConst.O && x.OrderEducationSourceID == EDSourceConst.Quota),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.OZ && x.OrderEducationSourceID == EDSourceConst.Quota),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.Z && x.OrderEducationSourceID == EDSourceConst.Quota),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.O && x.OrderEducationSourceID == EDSourceConst.Paid),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.OZ && x.OrderEducationSourceID == EDSourceConst.Paid),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.Z && x.OrderEducationSourceID == EDSourceConst.Paid),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.O && x.OrderEducationSourceID == EDSourceConst.Target),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.OZ && x.OrderEducationSourceID == EDSourceConst.Target),
				apps.Count(x => x.OrderEducationFormID == EDFormsConst.Z && x.OrderEducationSourceID == EDSourceConst.Target),
			};
		}

		/// <summary>
		/// Можем ли удалить направление из КГ
		/// </summary>
		public static AjaxResultModel CanDeleteCompetitiveGroupDirection(
			this EntrantsEntities dbContext, int institutionID, int groupID, int directionID, int edLevelID, int remainedDirections)
		{
			int cnt = dbContext.Application.Count(x => x.OrderCompetitiveGroupID == groupID
			                                           && x.InstitutionID == institutionID
			                                           && x.CompetitiveGroup.DirectionID == directionID);
			if (cnt > 0)
				return new AjaxResultModel("Невозможно удалить направление, так как по нему есть заявления в приказе");
			int cnt2 = dbContext.ApplicationSelectedCompetitiveGroupItem.Count(x => x.CompetitiveGroupItem.CompetitiveGroup.DirectionID == directionID
														&& x.CompetitiveGroupItem.CompetitiveGroup.EducationLevelID == edLevelID
													   && x.CompetitiveGroupItem.CompetitiveGroup.InstitutionID == institutionID
													   && x.CompetitiveGroupItem.CompetitiveGroupID == groupID);
			if (cnt2 > 0)
				return new AjaxResultModel("Невозможно удалить направление, так как по нему есть заявления с данным направлением");
			if (remainedDirections == 1)
			{
				int entranceTests = dbContext.EntranceTestItemC.Count(x => x.CompetitiveGroupID == groupID);
				if (entranceTests > 0)
					return new AjaxResultModel("Невозможно удалить последнее направление, так как для данной группы существуют вступительные испытания");
			}

			return new AjaxResultModel();
		}

		/// <summary>
		/// Возвращаем группированные допустимые направления для выбранных КГ
		/// </summary>
		public static AjaxResultModel GetCompetitiveGroupItemsForSelectedCompetitiveGroup(this EntrantsEntities dbContext, int[] groupIDs, int institutionID)
		{
            groupIDs = groupIDs ?? new int[0];

		    var key = string.Format("GetCompetitiveGroupItemsForSelectedCompetitiveGroup_{0}_{1}", institutionID, string.Join(";", groupIDs.Distinct()));
            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var model = cache.Get<AjaxResultModel>(key, null);
            if (model != null)
                return model;

            var res = dbContext.CompetitiveGroupItem.Where(x => groupIDs.Contains(x.CompetitiveGroupID) && x.CompetitiveGroup.InstitutionID == institutionID)
				.Select(x => new
							 {
								 ID = x.CompetitiveGroupItemID,
								 Code = x.CompetitiveGroup.Direction.Code == null ? "" : x.CompetitiveGroup.Direction.Code.Trim(),
                                 NewCode = x.CompetitiveGroup.Direction.NewCode,
								 QualificationCode = x.CompetitiveGroup.Direction.QUALIFICATIONCODE,
								 DirectionID = x.CompetitiveGroup.DirectionID,
								 Name = x.CompetitiveGroup.Direction.Name,
								 EducationLevelID = x.CompetitiveGroup.EducationLevelID,
                                 cgName = x.CompetitiveGroup.AdmissionItemType.Name
							 }).ToArray()
				.Select(x => new 
				             {
				             	ID = x.EducationLevelID + "@" + x.DirectionID + "@" + x.ID,
								Code = (string.IsNullOrEmpty(x.Code) ? "" : (x.Code.Trim() + "." + x.QualificationCode.Trim())) + "/" + (x.NewCode == null ? "" : x.NewCode.Trim()),
								Name = x.Name + ", (" + x.cgName + ")"
				             }).Distinct().OrderBy(x => x.Name).ToArray();

            model = new AjaxResultModel { Data = res };
            cache.Insert(key, model);
		    return model;
		}

		/// <summary>
		/// Получаем допустимые формы образования для выбранных КГ
		/// </summary>
		public static AjaxResultModel GetEducationFormsForCompetitiveGroups(this EntrantsEntities dbContext, int[] groupIDs, string[] directionKeys, int institutionID)
		{
            groupIDs = groupIDs ?? new int[0];
            directionKeys = directionKeys ?? new string[0];

            var key = string.Format("GetEducationFormsForCompetitiveGroups_{0}_{1}_{2}", institutionID, 
                string.Join(";", groupIDs.Distinct()), 
                string.Join(";", directionKeys.Distinct()));

            var cache = ServiceLocator.Current.GetInstance<ICache>();
            var model = cache.Get<AjaxResultModel>(key, null);
            if (model != null)
                return model;

			var res =
				dbContext.CompetitiveGroupItem.Where(
					x => groupIDs.Contains(x.CompetitiveGroupID) && x.CompetitiveGroup.InstitutionID == institutionID)
					.Select(x => new
					             {
                                    x.CompetitiveGroupItemID,
									x.CompetitiveGroup.EducationLevelID,
									x.CompetitiveGroup.DirectionID,
					             	x.NumberBudgetO,
					             	x.NumberBudgetOZ,
					             	x.NumberBudgetZ,
					             	x.NumberPaidO,
					             	x.NumberPaidOZ,
					             	x.NumberPaidZ,
					             	NumberTargetO = x.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetO) ?? 0,
					             	NumberTargetOZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetOZ) ?? 0,
					             	NumberTargetZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Sum(y => (int?)y.NumberTargetZ) ?? 0,
									TargetOrganizationsO = x.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.NumberTargetO > 0).Select(y => y.CompetitiveGroupTarget),
									TargetOrganizationsOZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.NumberTargetOZ > 0).Select(y => y.CompetitiveGroupTarget),
									TargetOrganizationsZ = x.CompetitiveGroup.CompetitiveGroupTargetItem.Where(y => y.NumberTargetZ > 0).Select(y => y.CompetitiveGroupTarget),
					             }).ToArray()
								 .Where(x => directionKeys.Contains(x.EducationLevelID + "@" + x.DirectionID + "@" + x.CompetitiveGroupItemID))
								 .ToArray();

            // Блок устарел. Вместо этого - приоритеты
			// возвращаем формы по допустимым местам
				var edForms = new ApplicationSendingViewModel.EducationForms
				{
					BudgetO = res.Sum(x => x.NumberBudgetO) > 0,
					BudgetOZ = res.Sum(x => x.NumberBudgetOZ) > 0,
					BudgetZ = res.Sum(x => x.NumberBudgetZ) > 0,
					PaidO = res.Sum(x => x.NumberPaidO) > 0,
					PaidOZ = res.Sum(x => x.NumberPaidOZ) > 0,
					PaidZ = res.Sum(x => x.NumberPaidZ) > 0,
					TargetO = res.Sum(x => x.NumberTargetO) > 0,
					TargetOZ = res.Sum(x => x.NumberTargetOZ) > 0,
					TargetZ = res.Sum(x => x.NumberTargetZ) > 0,
					TargetOrganizationsO = res.SelectMany(x => x.TargetOrganizationsO).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name })
						.Distinct().OrderBy(y => y.Name).ToArray(),
					TargetOrganizationsOZ = res.SelectMany(x => x.TargetOrganizationsOZ).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name })
						.Distinct().OrderBy(y => y.Name).ToArray(),
					TargetOrganizationsZ = res.SelectMany(x => x.TargetOrganizationsZ).Select(x => new { ID = x.CompetitiveGroupTargetID, Name = x.Name })
						.Distinct().OrderBy(y => y.Name).ToArray(),
				};
			model = new AjaxResultModel { Data = edForms };
            cache.Insert(key, model);
		    return model;
		}
	}
}