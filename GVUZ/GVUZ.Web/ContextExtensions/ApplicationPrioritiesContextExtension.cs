using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GVUZ.Web.ViewModels;
using GVUZ.Model.ApplicationPriorities;
using System.Collections;

namespace GVUZ.Web.ContextExtensions
{
    public static class ApplicationPrioritiesContextExtension
    {
        public static void FillExistingPriorities(this ApplicationPrioritiesEntities context, InstitutionApplicationListViewModel model)
        {
            var applicationsIds = model.Applications.Select(c => c.ApplicationID).ToArray();
            
            var results = context.ApplicationCompetitiveGroupItem.Where(c => applicationsIds.Contains(c.ApplicationId)).Select(x =>
                new ApplicationPriorityViewModel
                {
                    ApplicationId = x.ApplicationId,
                    CompetitiveGroupId = x.CompetitiveGroupId,
                    CompetitiveGroupItemId = x.CompetitiveGroupItemId,
                    CompetitiveGroupItemName = x.CompetitiveGroupItem.Direction.Name,
                    CompetitiveGroupName = x.CompetitiveGroup.Name,
                    CompetitiveGroupTargetId = x.CompetitiveGroupTargetId,
                    TargetOrganizationName = x.CompetitiveGroupTargetId != null ? context.CompetitiveGroupTarget.FirstOrDefault(y => y.CompetitiveGroupTargetID == x.CompetitiveGroupTargetId).Name : string.Empty,
                    EducationFormId = x.EducationFormId,
                    EducationFormName = context.AdmissionItemType.FirstOrDefault(adm => adm.ItemTypeID == x.EducationFormId).Name,
                    EducationSourceId = x.EducationSourceId,
                    EducationSourceName = context.AdmissionItemType.FirstOrDefault(adm => adm.ItemTypeID == x.EducationSourceId).Name,
                    Id = x.id,
                    Priority = x.Priority
                }).ToArray();

            foreach (var application in model.Applications)
            {
                application.Priorities = new ApplicationPrioritiesViewModel();
                application.Priorities.ApplicationId = application.ApplicationID;

                var priorities = results.Where(c => c.ApplicationId == application.ApplicationID).OrderBy(c => c.Priority);
                application.Priorities.ApplicationPriorities.AddRange(priorities);
            }
        }

        /// <summary>
        /// Загрузка приоритетов заявления из БД
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="applicationId">Идентификатор заявления</param>
        /// <returns>Модель приоритетов для отображения</returns>
        public static ApplicationPrioritiesViewModel FillExistingPriorities(this ApplicationPrioritiesEntities context, int applicationId)
        {
            ApplicationPrioritiesViewModel result = new ApplicationPrioritiesViewModel();

            var query = context.ApplicationCompetitiveGroupItem.Where(x => x.ApplicationId == applicationId);

            var items = query.Select(x => new ApplicationPriorityViewModel()
                                            {
                                                CompetitiveGroupId = x.CompetitiveGroupId,
                                                CompetitiveGroupItemId = x.CompetitiveGroupItemId,
                                                CompetitiveGroupItemName = x.CompetitiveGroupItem.Direction.Name,
                                                CompetitiveGroupName = x.CompetitiveGroup.Name,
                                                CompetitiveGroupTargetId = x.CompetitiveGroupTargetId,
                                                TargetOrganizationName = x.CompetitiveGroupTargetId != null ? context.CompetitiveGroupTarget.FirstOrDefault(y => y.CompetitiveGroupTargetID == x.CompetitiveGroupTargetId).Name : string.Empty,
                                                EducationFormId = x.EducationFormId,
                                                EducationFormName = context.AdmissionItemType.Where(adm => adm.ItemTypeID == x.EducationFormId).FirstOrDefault().Name,
                                                EducationSourceId = x.EducationSourceId,
                                                EducationSourceName = context.AdmissionItemType.Where(adm => adm.ItemTypeID == x.EducationSourceId).FirstOrDefault().Name,
                                                Id = x.id,
                                                Priority = x.Priority
                                            }
            ).OrderBy(x => x.Priority);

            result.ApplicationId = applicationId;
            result.ApplicationPriorities.AddRange(items);

            return result;
        }

        /// <summary>
        /// Сохранение приоритетов в БД
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="data">Данные для сохранения - модель от UI</param>
        public static void SavePriorities(this ApplicationPrioritiesEntities context, ApplicationPrioritiesViewModel data)
        {
            // Сначала убедимся, что имеющиеся в БД приоритеты снова пришли

            var existingPriorities = context.ApplicationCompetitiveGroupItem.Where(x => x.ApplicationId == data.ApplicationId).ToList();

            foreach (var item in existingPriorities)
            {
                if (data.ApplicationPriorities.FirstOrDefault(x =>
                    x.CompetitiveGroupId == item.CompetitiveGroupId &&
                    x.CompetitiveGroupItemId == item.CompetitiveGroupItemId &&
                    x.EducationFormId == item.EducationFormId &&
                    x.EducationSourceId == item.EducationSourceId) == null)
                        context.ApplicationCompetitiveGroupItem.DeleteObject(item);
            }

            foreach (var dataItem in data.ApplicationPriorities)
            {
                var priorityItem = context.ApplicationCompetitiveGroupItem.FirstOrDefault(x =>
                    x.ApplicationId == data.ApplicationId &&
                    x.CompetitiveGroupId == dataItem.CompetitiveGroupId &&
                    x.CompetitiveGroupItemId == dataItem.CompetitiveGroupItemId &&
                    x.EducationFormId == dataItem.EducationFormId &&
                    x.EducationSourceId == dataItem.EducationSourceId);

                if (priorityItem == null)
                {
                    priorityItem = new ApplicationCompetitiveGroupItem();
                    context.ApplicationCompetitiveGroupItem.AddObject(priorityItem);
                }

                SetItemValues(data.ApplicationId, dataItem, priorityItem);
            }
            context.SaveChanges();

            context.UpdateSelectedData(data.ApplicationId);
        }

        /// <summary>
        /// Получение списка доступных для заявления форм обучения в порядке убывания приоритетов
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="applicationId">Идентификатор заявления</param>
        /// <param name="directionId">Идентификатор направления подготовки</param>
        /// <returns>Доступные формы обучения, упорядоченные по приоритету в виде списка пар {ID, Name}</returns>
        public static IList GetEduFormsForDirectionInApplication(this ApplicationPrioritiesEntities context, int applicationId, int directionId)
        {
            var result = context.ApplicationCompetitiveGroupItem.Where(x => x.ApplicationId == applicationId &&
                                                                            x.CompetitiveGroupItemId == directionId &&
                                                                            x.Priority != null)
                                                                .OrderBy(x => x.Priority)
                                                                .Select(x => new
                                                                {
                                                                    ID = x.EducationFormId,
                                                                    Name = context.AdmissionItemType.FirstOrDefault(y => y.ItemTypeID == x.EducationFormId).Name
                                                                })
                                                                .Distinct()
                                                                .ToList();
            return result;
        }

        /// <summary>
        /// Получение списка доступных источников финансирования для заявления, выбранного направления и формы обучения
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="applicationId">Идентификатор заявления</param>
        /// <param name="directionId">Идентификатор направления подготовки</param>
        /// <param name="eduFormId">Идентификатор формы обучения</param>
        /// <returns>Доступные источники финансирования, упорядоченные по приоритету в виде списка пар {ID, Name}</returns>
        public static IList GetSourcesForApplication(this ApplicationPrioritiesEntities context, int applicationId, int directionId, int eduFormId)
        {
            var result = context.ApplicationCompetitiveGroupItem.Where(x => x.ApplicationId == applicationId &&
                                                                            x.CompetitiveGroupItemId == directionId &&
                                                                            x.EducationFormId == eduFormId &&
                                                                            x.Priority != null)
                                                                .OrderBy(x => x.Priority);

             var res = result.Select(x => new
                                                {
                                                    ID = x.EducationSourceId,
                                                    Name = context.AdmissionItemType.FirstOrDefault(y => y.ItemTypeID == x.EducationSourceId).Name,
                                                    TargetOrganizationName = x.CompetitiveGroupTargetId == null ? string.Empty : context.CompetitiveGroupTarget.FirstOrDefault(y => y.CompetitiveGroupTargetID == x.CompetitiveGroupTargetId).Name,
                                                    Priority = x.Priority
                                                })
                             .Distinct()
                             .OrderBy(x => x.Priority)
                             .ToList();
            return res;
        }

        /// <summary>
        /// Вспомогательный метод - копирование данных модели в объектную модель БД
        /// </summary>
        /// <param name="appId">Идентификатор приложения. 
        /// Передаётся отдельно - он один на всё и в каждой модели собственно приоритетов его нет</param>
        /// <param name="src">Источник данных - модель</param>
        /// <param name="dest">Приёмник данных - объект модели БД, существующий или новый</param>
        private static void SetItemValues(int appId, ApplicationPriorityViewModel src, ApplicationCompetitiveGroupItem dest)
        {
            dest.ApplicationId = appId;
            dest.CompetitiveGroupId = src.CompetitiveGroupId;
            dest.CompetitiveGroupItemId = src.CompetitiveGroupItemId;
            dest.CompetitiveGroupTargetId = src.CompetitiveGroupTargetId;
            dest.EducationFormId = src.EducationFormId;
            dest.EducationSourceId = src.EducationSourceId;
            dest.Priority = src.Priority;
        }
    }
}