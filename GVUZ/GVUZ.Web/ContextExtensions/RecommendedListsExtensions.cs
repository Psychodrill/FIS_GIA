using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GVUZ.Web.ViewModels;
using GVUZ.Model.RecommendedLists;
using GVUZ.Web.ViewModels.RecommendedLists;
using GVUZ.Helper;
using FogSoft.Helpers;
using GVUZ.Model.Institutions;
using System.Data.Objects;

namespace GVUZ.Web.ContextExtensions
{
    public static class RecommendedListsExtensions
    {
        public static ListOfRecommendedListViewModel FillList(this RecommendedListsEntities context, int institutionId, byte? stage, RecommendedListsShowParametersViewModel showParams)
        {
            ListOfRecommendedListViewModel result = new ListOfRecommendedListViewModel();
            
            var query = context.RecomendedLists.Where(x => x.InstitutionID == institutionId);
            if (stage.HasValue)
                query = query.Where(x => x.Stage == stage.Value);

            query = query.Where(x => x.RecomendedListsHistory.Any(y => !y.DateDelete.HasValue));

            #region Фильтрация
            if (showParams.Filter != null)
            {
                if (showParams.Filter.CampaignId > 0)
                    query = query.Where(x => x.CampaignID == showParams.Filter.CampaignId);

                if (showParams.Filter.Stage > 0)
                    query = query.Where(x => x.Stage == showParams.Filter.Stage);

                if (showParams.Filter.CompetitiveGroup > 0)
                    query = query.Where(x => x.CompetitiveGroupID == showParams.Filter.CompetitiveGroup);

                if (showParams.Filter.Direction > 0)
                    query = query.Where(x => x.DirectionID== showParams.Filter.Direction);

                if (showParams.Filter.EduForm > 0)
                    query = query.Where(x => x.EduFormID == showParams.Filter.EduForm);

                if (showParams.Filter.Edulevel > 0)
                    query = query.Where(x => x.EduLevelID == showParams.Filter.Edulevel);

                if (showParams.Filter.OriginalsReceived >= 0)
                    query = query.Where(x => showParams.Filter.OriginalsReceived == 1 ? x.Application.OriginalDocumentsReceived : !x.Application.OriginalDocumentsReceived);

                if (!string.IsNullOrEmpty(showParams.Filter.ApplicationNumber))
                    query = query.Where(x => x.Application.ApplicationNumber.StartsWith(showParams.Filter.ApplicationNumber.Trim()));

                if (!string.IsNullOrEmpty(showParams.Filter.LastName))
                    query = query.Where(x => x.Application.Entrant.LastName.StartsWith(showParams.Filter.LastName.Trim()));

                if (!string.IsNullOrEmpty(showParams.Filter.MiddleName))
                    query = query.Where(x => x.Application.Entrant.MiddleName.StartsWith(showParams.Filter.MiddleName.Trim()));

                if (!string.IsNullOrEmpty(showParams.Filter.FirstName))
                    query = query.Where(x => x.Application.Entrant.FirstName.StartsWith(showParams.Filter.FirstName.Trim()));
            }
            #endregion

            var resData = query.Select(x => new RecommendedListViewModel()
            {
                ApplicationId = x.ApplicationID,
                CampaignId = x.CampaignID,
                CompetitiveGroupId = x.CompetitiveGroupID,
                DirectionId = x.DirectionID,
                EduFormId = x.EduFormID,
                EduLevelId = x.EduLevelID,
                InstitutionId = institutionId,
                Rating = x.Rating.HasValue ? x.Rating.Value : 0,
                RecListId = x.RecListID,
                Stage = x.Stage,
                ApplicationStatus = x.Application.StatusID,

                ApplicationNumber = x.Application.ApplicationNumber,
                CampaignName = x.Campaign.Name,
                CompetitiveGroupName = x.CompetitiveGroup.Name,
                DirectionName = x.Direction.Name,
                EduForm = context.AdmissionItemType.FirstOrDefault(y => y.ItemTypeID == (short)x.EduFormID).Name,
                EduLevel = context.AdmissionItemType.FirstOrDefault(y => y.ItemTypeID == (short)x.EduLevelID).Name,
                EntrantName = x.Application.Entrant.LastName + " " + x.Application.Entrant.FirstName + (x.Application.Entrant.MiddleName != null ? (" " + x.Application.Entrant.MiddleName) : ""),
                OriginalsReceived = x.Application.OriginalDocumentsReceived ? "Да" : "Нет",

                IsCampaignFinished = (x.Campaign.StatusID == 2),
                AppRegistrationDate = x.Application.RegistrationDate,
                CompetitiveGroupUID = x.CompetitiveGroup.UID
            });

            int totalCount = resData.Count();
            result.PageCount = totalCount % ListOfRecommendedListViewModel.PAGESIZE == 0 ? totalCount / ListOfRecommendedListViewModel.PAGESIZE : totalCount / ListOfRecommendedListViewModel.PAGESIZE + 1;

            int pageSize = showParams.PageToShow >= 0 ? ListOfRecommendedListViewModel.PAGESIZE : totalCount;
            if (showParams.PageToShow < 0) showParams.PageToShow = 0;

            switch (showParams.SortDirection)
            {
                case 1:
                case -1:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.CampaignName) : resData.OrderByDescending(x => x.CampaignName)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;

                case 2:
                case -2:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.Stage) : resData.OrderByDescending(x => x.Stage)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 3:
                case -3:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.ApplicationNumber) : resData.OrderByDescending(x => x.ApplicationNumber)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 4:
                case -4:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.EntrantName) : resData.OrderByDescending(x => x.EntrantName)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 5:
                case -5:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.EduLevel) : resData.OrderByDescending(x => x.EduLevel)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 6:
                case -6:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.EduForm) : resData.OrderByDescending(x => x.EduForm)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 7:
                case -7:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.CompetitiveGroupName) : resData.OrderByDescending(x => x.CompetitiveGroupName)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 8:
                case -8:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.DirectionName) : resData.OrderByDescending(x => x.DirectionName)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 9:
                case -9:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.OriginalsReceived) : resData.OrderByDescending(x => x.OriginalsReceived)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;
                
                case 10:
                case -10:
                    result.RecommendedLists = (showParams.SortDirection > 0 ? resData.OrderBy(x => x.Rating) : resData.OrderByDescending(x => x.Rating)).Skip((showParams.PageToShow) * ListOfRecommendedListViewModel.PAGESIZE).Take(pageSize).ToArray();
                    break;

                case 0:
                    result.RecommendedLists = resData.ToArray();
                    break;
            }

            result.FilterData = context.BuildDataForFilters(institutionId);

/*            result.RecommendedLists.All(x =>
            {
                ObjectParameter rating = new ObjectParameter("rating", typeof(decimal));
                context.CalculateAppRating(x.ApplicationId, x.CompetitiveGroupId, rating);
                x.Rating = rating.Value == null ? 0 : (decimal)rating.Value;
                return true;
            });*/

            return result;
        }

        public static FilterDataViewModel BuildDataForFilters(this RecommendedListsEntities context, int institutionId)
        {
            FilterDataViewModel result = new FilterDataViewModel();

            DropDownData[] AnyM = new DropDownData[1] { new DropDownData() { Id = -1, Name = "Любой" } };
            DropDownData[] AnyF = new DropDownData[1] { new DropDownData() { Id = -1, Name = "Любая" } };
            DropDownData[] AnyMD = new DropDownData[1] { new DropDownData() { Id = -1, Name = "Любое" } };

            result.Campaigns = context.Campaign
                .Where(x => x.InstitutionID == institutionId)
                .Select(x => new DropDownData() { Id = x.CampaignID, Name = x.Name })
                .ToArray()
                .Concat(AnyF)
                .OrderBy(x => x.Id)
                .ToArray();


            result.EduLevels = context.AdmissionItemType
                .Where(x => x.ItemLevel == 2)
                .Select(x => new DropDownData() { Id = x.ItemTypeID, Name = x.Name })
                .ToArray()
                .Concat(AnyM)
                .OrderBy(x => x.Id)
                .ToArray();

            result.EduForms = context.AdmissionItemType
                .Where(x => x.ItemLevel == 7)
                .Select(x => new DropDownData() { Id = x.ItemTypeID, Name = x.Name })
                .ToArray()
                .Concat(AnyF)
                .OrderBy(x => x.Id)
                .ToArray();

            var campaignIds = result.Campaigns.Select(y => y.Id).ToArray();
            result.CompetitiveGroups = context.CompetitiveGroup
                .Where(x => x.CampaignID.HasValue && campaignIds.Contains(x.CampaignID.Value))
                .Select(x => new DropDownData() { Id = x.CompetitiveGroupID, Name = x.Name })
                .ToArray()
                .Concat(AnyF)
                .OrderBy(x => x.Id)
                .ToArray();

            var competitiveGroupIds = result.CompetitiveGroups.Select(x => x.Id).ToArray();
            result.Directions = context.CompetitiveGroupItem
                .Where(x => competitiveGroupIds.Contains(x.CompetitiveGroupID))
                .Select(x => new DropDownData() { Id = x.Direction.DirectionID, Name = x.Direction.Name })
                .Distinct()
                .ToArray()
                .Concat(AnyMD)
                .OrderBy(x => x.Id)
                .ToArray();

            return result;
        }

        public static AjaxResultModel DeleteRecommendedListElement(this RecommendedListsEntities context, int recListId)
        {
            var recListObject = context.RecomendedLists.FirstOrDefault(x => x.RecListID == recListId);

            if (recListObject == null) // Почему-то не нашли такой элемент. Чтобы избежать Exception, это надо проверить и обработать
                return new AjaxResultModel("Элемент для удаления не найден. Обновите список рекомендованных к зачислению");

            // Проверим, не включили ли заявление в приказ между отображением списка и попыткой удалить
            if (recListObject.Application.StatusID == 8)
                return new AjaxResultModel("Заявление включено в приказ и не может быть удалено из списка рекомендованных");

            // Найдём текущий элемент таблицы истории
            var historyCurrentElement = recListObject.RecomendedListsHistory.FirstOrDefault(x => !x.DateDelete.HasValue);

            // Если его нет - это ошибка
            if (historyCurrentElement == null)
                return new AjaxResultModel("Выбранное заявление уже удалено из списка рекомендованных по выбранным критериям");

            try
            {
                historyCurrentElement.DateDelete = DateTime.Now;
                context.SaveChanges();
                return new AjaxResultModel();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex);
                return new AjaxResultModel("При удалении заявления из списка рекомендованных возникла ошибка. Обратитесть к администратору");
            }
        }

        public static GVUZ.Model.RecommendedLists.RecomendedLists GetListElement(this RecommendedListsEntities context, int recListId)
        {
            return context.RecomendedLists.FirstOrDefault(x => x.RecListID == recListId);
        }

        public static ApplicationIncludeInRecListViewModel FillIncludeApplicationModel(this RecommendedListsEntities context, int appId)
        {
            ApplicationIncludeInRecListViewModel result = new ApplicationIncludeInRecListViewModel();
            result.ApplicationId = appId;

            var app = context.Application.FirstOrDefault(x => x.ApplicationID == appId);
            if (app == null)
            {
                result.ApplicationNumber = "Заявление не найдено";
                return result;
            }

            result.ApplicationNumber = app.ApplicationNumber;
            result.EntrantName = app.Entrant.LastName + " " + app.Entrant.FirstName + (string.IsNullOrEmpty(app.Entrant.MiddleName) ? string.Empty : " " + app.Entrant.MiddleName);

            result.DocumentData = 
                app.Entrant.EntrantIdentityDocument.EntrantDocumentIdentity.IdentityDocumentType.Name + // Вид документа
                " " + 
                app.Entrant.EntrantIdentityDocument.DocumentSeries + // Серия документа
                " " + 
                app.Entrant.EntrantIdentityDocument.DocumentNumber; // Номер документа

            result.Stage1List = context.GetParametersList(appId, 1);
            result.Stage2List = context.GetParametersList(appId, 2);
            
            return result;
        }

        private static StageListIncludeViewModel[] GetParametersList(this RecommendedListsEntities context, int appId, int stage)
        {
            int[] availiableLevels = new int[4] { 2, 3, 5, 19 };
            var data = context.ApplicationCompetitiveGroupItem
                .Where(
                    x => x.ApplicationId == appId &&
                    x.EducationSourceId == EDSourceConst.Budget &&
                    (x.EducationFormId == EDFormsConst.O || x.EducationFormId == EDFormsConst.OZ) && 
                    x.Priority.HasValue &&
                    availiableLevels.Contains(x.CompetitiveGroupItem.EducationLevelID))
                .Select(x => new StageListIncludeViewModel()
                            {
                                CompetitiveGroupId = x.CompetitiveGroupId,
                                CompetitveGroupName = x.CompetitiveGroup.Name,
                                DirectionId = x.CompetitiveGroupItem.DirectionID,
                                DirectionName = x.CompetitiveGroupItem.Direction.Name,
                                EduFormId = (short)x.EducationFormId,
                                EduLevelId = x.CompetitiveGroupItem.EducationLevelID,
                                CampaignId = x.CompetitiveGroup.CampaignID.HasValue ? x.CompetitiveGroup.CampaignID.Value : 0,
                                Priority = (short)x.Priority,
                                Recommended = false,
                            })
                .OrderBy(x => x.Priority);

            if (!context.RecomendedLists.Any(x => x.ApplicationID == appId))
                return data.ToArray();

            Func<StageListIncludeViewModel, bool> SetRecommended = delegate( StageListIncludeViewModel item)
            {
                var listItem = context.RecomendedLists.FirstOrDefault(x => x.ApplicationID == appId &&
                    x.CompetitiveGroupID == item.CompetitiveGroupId &&
                    x.DirectionID == item.DirectionId &&
                    x.EduFormID == item.EduFormId &&
                    x.EduLevelID == item.EduLevelId &&
                    x.CampaignID == item.CampaignId &&
                    x.RecomendedListsHistory.Any(y => !y.DateDelete.HasValue) &&
                    x.Stage == stage);

                item.Recommended = (listItem != null);
                return true;
            };

            var result =  data.ToArray();
            result.All(x => SetRecommended(x));

            return result.OrderBy(x => x.Priority).ToArray();
        }


        public static void IncludeApplicationInRecommendedLists(this RecommendedListsEntities context, ApplicationIncludeInRecListViewModel data)
        {
            // Обработка этапа 1
            foreach (var item in data.Stage1List)
            {
                var existingItem = context.RecomendedLists.FirstOrDefault(x =>
                    x.ApplicationID == data.ApplicationId &&
                    x.CompetitiveGroupID == item.CompetitiveGroupId &&
                    x.DirectionID == item.DirectionId &&
                    x.EduFormID == item.EduFormId &&
                    x.EduLevelID == item.EduLevelId &&
                    x.Stage == 1);

                if (item.Recommended)
                {
                    if (existingItem == null)
                    {
                        // Создать новый элемент списка рекомендованных и проставить дату включения
                        context.FillItemData(item, data.ApplicationId, 1);
                    }
                    else
                    {
                        // Установить дату включения, если её ещё не было
                        var newHistoryElement = context.RecomendedListsHistory.CreateObject();
                        newHistoryElement.RecListID = existingItem.RecListID;
                        newHistoryElement.DateAdd = DateTime.Now;
                        newHistoryElement.DateDelete = null;
                        context.RecomendedListsHistory.AddObject(newHistoryElement);

                        // Пересчитаем рейтинг
                        ObjectParameter rating = new ObjectParameter("rating", typeof(decimal));
                        context.CalculateAppRating(existingItem.ApplicationID, existingItem.CompetitiveGroupID, rating);

                        if (rating.Value != DBNull.Value)
                        {
                            existingItem.Rating = (decimal)rating.Value;
                            var ratingItem = context.ApplicationSelectedCompetitiveGroup.FirstOrDefault(x => x.ApplicationID == existingItem.ApplicationID && x.CompetitiveGroupID == existingItem.CompetitiveGroupID);
                            if (ratingItem != null) ratingItem.CalculatedRating = existingItem.Rating;
                        }
                    }
                }
                else
                {
                    if (existingItem == null)
                        continue;
                    else
                    {
                        // Установим дату удаления из списка рекомендованных
                        var historyActiveElement = existingItem.RecomendedListsHistory.FirstOrDefault(x => !x.DateDelete.HasValue);
                        if (historyActiveElement != null)
                            historyActiveElement.DateDelete = DateTime.Now;
                    }
                }
            }

            // Обработка этапа 2
            foreach (var item in data.Stage2List)
            {
                var existingItem = context.RecomendedLists.FirstOrDefault(x =>
                    x.ApplicationID == data.ApplicationId &&
                    x.CompetitiveGroupID == item.CompetitiveGroupId &&
                    x.DirectionID == item.DirectionId &&
                    x.EduFormID == item.EduFormId &&
                    x.EduLevelID == item.EduLevelId &&
                    x.Stage == 2);

                if (item.Recommended)
                {
                    if (existingItem == null)
                    {
                        // Создать новый элемент списка рекомендованных и проставить дату включения
                        context.FillItemData(item, data.ApplicationId, 2);
                    }
                    else
                    {
                        // Установить дату включения, если её ещё не было
                        var newHistoryElement = context.RecomendedListsHistory.CreateObject();
                        newHistoryElement.RecListID = existingItem.RecListID;
                        newHistoryElement.DateAdd = DateTime.Now;
                        newHistoryElement.DateDelete = null;
                        context.RecomendedListsHistory.AddObject(newHistoryElement);

                        // Пересчитаем рейтинг
                        ObjectParameter rating = new ObjectParameter("rating", typeof(decimal));
                        context.CalculateAppRating(existingItem.ApplicationID, existingItem.CompetitiveGroupID, rating);

                        if (rating.Value != DBNull.Value)
                        {
                            existingItem.Rating = (decimal)rating.Value;
                            var ratingItem = context.ApplicationSelectedCompetitiveGroup.FirstOrDefault(x => x.ApplicationID == existingItem.ApplicationID && x.CompetitiveGroupID == existingItem.CompetitiveGroupID);
                            if (ratingItem != null) ratingItem.CalculatedRating = existingItem.Rating;
                        }
                    }
                }
                else
                {
                    if (existingItem == null)
                        continue;
                    else
                    {
                        // Установим дату удаления из списка рекомендованных
                        var historyActiveElement = existingItem.RecomendedListsHistory.FirstOrDefault(x => !x.DateDelete.HasValue);
                        if (historyActiveElement != null)
                            historyActiveElement.DateDelete = DateTime.Now;
                    }
                }
            }

            context.SaveChanges();
        }

        private static void FillItemData(this RecommendedListsEntities context, StageListIncludeViewModel data, int appId, byte stage)
        {
            var item = context.RecomendedLists.CreateObject();
            var campaign = context.CompetitiveGroup.First(x => x.CompetitiveGroupID == data.CompetitiveGroupId).Campaign;
            var app = context.Application.First(x => x.ApplicationID == appId);

            item.ApplicationID = appId;
            item.CampaignID = campaign.CampaignID;
            item.InstitutionID = campaign.InstitutionID;
            item.CompetitiveGroupID = data.CompetitiveGroupId;
            item.DirectionID = data.DirectionId;
            item.EduFormID = data.EduFormId;
            item.EduLevelID = data.EduLevelId;
            item.Stage = stage;
            ObjectParameter rating = new ObjectParameter("rating", typeof(decimal));
            context.CalculateAppRating(appId, item.CompetitiveGroupID, rating);

            if (rating.Value == DBNull.Value)
                throw new ArgumentNullException("В заявлении не указаны необходимые результаты вступительных испытаний. В список рекомендованных можно внести только заявления, для которых указаны результаты ВИ. Необходимо добавить результаты вступительных испытаний", new Exception());

            item.Rating = (decimal)rating.Value;

            RecomendedListsHistory newItem = context.RecomendedListsHistory.CreateObject();
            newItem.DateAdd = DateTime.Now;
            newItem.DateDelete = null;
            item.RecomendedListsHistory.Add(newItem);
            context.RecomendedLists.AddObject(item);
        }
    }
};