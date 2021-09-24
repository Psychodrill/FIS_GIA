using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Data;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;
using AdmissionVolume = GVUZ.Model.Entrants.AdmissionVolume;
using AllowedDirections = GVUZ.Model.Entrants.AllowedDirections;
using Direction = GVUZ.Model.Entrants.Direction;

namespace GVUZ.Web.ContextExtensions {
    /// <summary>
    /// Методы для работы с объёмом приёма
    /// </summary>
    public static class AdmissionVolumeExtensions {



        /// <summary>
        /// Загружаем объём приёма из базы
        /// </summary>
        public static AdmissionVolumeViewModel FillAdmissionVolumeViewModel(this EntrantsEntities dbContext, AdmissionVolumeViewModel model, int insitutionID, bool forEdit) {
            //model.AllowedCampaigns = dbContext.Campaign
            //    .Where(x => x.InstitutionID == insitutionID)
            //    .Select(x => new AdmissionVolumeViewModel.CampaignData { ID = x.CampaignID, Name = x.Name }).ToArray();
            //model.AllowedCoursesByCampaign = new AdmissionVolumeViewModel.CampaignData[0];

            //// если не выбрана ПК, ставим первую
            //if(model.SelectedCampaignID == 0) {
            //    if(model.AllowedCampaigns.Length > 0) {
            //        model.SelectedCampaignID = model.AllowedCampaigns[0].ID;
            //    }
            //}

            //// если выбрана ПК, выбираем для неё курсы
            //if(model.SelectedCampaignID > 0) {
            //    model.AllowedCoursesByCampaign = dbContext.CampaignEducationLevel.Where(x => x.CampaignID == model.SelectedCampaignID)
            //        .OrderBy(x => x.Course)
            //        .Select(x => x.Course).Distinct().Select(x => new AdmissionVolumeViewModel.CampaignData {
            //            ID = x,
            //            Name = SqlFunctions.StringConvert((double)x)
            //        }).ToArray();
            //}

            //if(model.SelectedCourse == 0 || !model.AllowedCoursesByCampaign.Any(x => x.ID == model.SelectedCourse)) {
            //    if(model.AllowedCoursesByCampaign.Length > 0)
            //        model.SelectedCourse = model.AllowedCoursesByCampaign[0].ID;
            //}

            ////var availForms = dbContext.CampaignDate.Where(
            ////    x => x.CampaignID == model.SelectedCampaignID && x.Course == model.SelectedCourse && x.IsActive)
            ////    .Select(x => new { x.EducationFormID, x.EducationSourceID, x.EducationLevelID }).Distinct().ToArray();
            ////model.AvailForms = availForms.Select(x => new AdmissionVolumeViewModel.AvailFormsInfo {
            ////    FormID = x.EducationFormID,
            ////    LevelID = x.EducationLevelID,
            ////    SourceID = x.EducationSourceID
            ////}).ToArray();
            


            ////вытаскиваем разрешённые цифры по AllowedDirection и правилах в ПК
            //var q1 = (from x in dbContext.AllowedDirections
            //          where x.InstitutionID == insitutionID
            //             && dbContext.CampaignEducationLevel
            //             .Where(y => y.CampaignID == model.SelectedCampaignID && y.Course == model.SelectedCourse)
            //             .Select(y => y.EducationLevelID).Contains(x.AdmissionItemTypeID.Value)
            //          orderby x.AdmissionItemType.DisplayOrder, x.Direction.ParentDirection.Name, x.Direction.Name
            //          select new {
            //              x.AdmissionItemTypeID,
            //              AdmissionTypeName = x.AdmissionItemType.Name,
            //              x.DirectionID,
            //              x.Direction.Code,
            //              x.Direction.NewCode,
            //              DirectionName = x.Direction.Name,
            //              x.Direction.ParentDirection.ParentDirectionID,
            //              ParentDirectionName = x.Direction.ParentDirection.Name,
            //              ParentDirectionCode = x.Direction.ParentDirection.Code,
            //              QualificationCode = x.Direction.QUALIFICATIONCODE
            //          }).ToArray();


            ////вытаскиваем существующий объём приёма
            //var q2 = dbContext.AdmissionVolume.Where(x => x.InstitutionID == insitutionID
            //    && x.CampaignID == 46
            //    ).ToList();  

            //var distributed = q2.ToDictionary(x => x.AdmissionVolumeID,
            //                                             v =>
            //                                             v.DistributedAdmissionVolume.Sum(
            //                                                  x =>
            //                                                  x.NumberBudgetO + x.NumberBudgetOZ + x.NumberBudgetZ + x.NumberQuotaO +
            //                                                  x.NumberQuotaOZ +
            //                                                  x.NumberQuotaZ + x.NumberTargetO + x.NumberTargetOZ + x.NumberTargetZ));

            ////заполняем весь, существующими данными (если нет данных, выводим ноли)
            //model.Items = new AdmissionVolumeViewModel.RowData[q1.Length];
            //int prevAdmID = 0;
            //int prevParDirID = 0;
            //model.TreeItems = new List<List<List<AdmissionVolumeViewModel.RowData>>>();
            //for(int i = 0; i < q1.Length; i++) {
            //    AdmissionVolumeViewModel.RowData r = new AdmissionVolumeViewModel.RowData();
            //    var item = q1[i];
            //    if(item.AdmissionItemTypeID != prevAdmID) {
            //        model.TreeItems.Add(new List<List<AdmissionVolumeViewModel.RowData>>());
            //        prevAdmID = item.AdmissionItemTypeID.Value;
            //        prevParDirID = 0;
            //    }

            //    var pdirs = model.TreeItems[model.TreeItems.Count - 1];
            //    if(prevParDirID == 0 || prevParDirID != item.ParentDirectionID) {
            //        pdirs.Add(new List<AdmissionVolumeViewModel.RowData>());
            //        prevParDirID = item.ParentDirectionID;
            //    }

            //    pdirs[pdirs.Count - 1].Add(r);
            //    model.Items[i] = r;

            //    r.AdmissionItemTypeID = q1[i].AdmissionItemTypeID.Value;
            //    r.AdmissionItemTypeName = q1[i].AdmissionTypeName;
            //    r.DirectionID = q1[i].DirectionID;
            //    r.DirectionName = q1[i].DirectionName;
            //    r.ParentDirectionName = q1[i].ParentDirectionName;
            //    r.ParentDirectionCode = q1[i].ParentDirectionCode;
            //    r.DirectionCode = q1[i].Code;
            //    r.DirectionNewCode = q1[i].NewCode;
            //    r.QualificationCode = (q1[i].QualificationCode == null ? "" : q1[i].QualificationCode.Trim());

            //    AdmissionVolume volume = q2.Find(x => x.DirectionID == r.DirectionID && x.AdmissionItemTypeID == r.AdmissionItemTypeID);
            //    //r.DisableForEditing = compGroupItems
            //    //		.Where(x => x.DirectionID == r.DirectionID && x.EducationalLevelID == r.AdmissionItemTypeID)
            //    //		.FirstOrDefault() != null;

            //    //сейчас можно редактировать всё
            //    r.DisableForEditing = false;
            //    if(volume != null) {
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.FullTimeTuition)) {
            //            r.NumberBudgetO = volume.NumberBudgetO;
            //            r.NumberQuotaO = volume.NumberQuotaO;
            //        }
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.FullTimeTuition))
            //            r.NumberPaidO = volume.NumberPaidO;
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.FullTimeTuition))
            //            r.NumberTargetO = volume.NumberTargetO;

            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.MixedTuition)) {
            //            r.NumberBudgetOZ = volume.NumberBudgetOZ;
            //            r.NumberQuotaOZ = volume.NumberQuotaOZ;
            //        }
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.MixedTuition))
            //            r.NumberPaidOZ = volume.NumberPaidOZ;
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.MixedTuition))
            //            r.NumberTargetOZ = volume.NumberTargetOZ;

            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.PostalTuition)) {
            //            r.NumberBudgetZ = volume.NumberBudgetZ;
            //            r.NumberQuotaZ = volume.NumberQuotaZ;
            //        }
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.PostalTuition))
            //            r.NumberPaidZ = volume.NumberPaidZ;
            //        if(forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.PostalTuition))
            //            r.NumberTargetZ = volume.NumberTargetZ;
            //        r.UID = volume.UID;
            //        r.AdmissionVolumeId = volume.AdmissionVolumeID;
            //        r.AvailableForDistribution = volume.NumberBudgetO + volume.NumberBudgetOZ + volume.NumberBudgetZ +
            //                                              volume.NumberQuotaO.GetValueOrDefault() + volume.NumberQuotaOZ.GetValueOrDefault() +
            //                                              volume.NumberQuotaZ.GetValueOrDefault() + volume.NumberTargetO +
            //                                              volume.NumberTargetOZ + volume.NumberTargetZ;
            //        r.TotalDistributed = distributed[volume.AdmissionVolumeID];
            //    }
            //}

            //// заполняем дерево, родительскими направлениями, чтобы в интерфейсе смогли их выровнять
            //foreach(var treeItem in model.TreeItems) {
            //    var pDir = treeItem[treeItem.Count - 1];
            //    pDir[pDir.Count - 1].GroupLast = true;
            //}
            ////не даём редактировать при завершённой кампании
            //if(model.SelectedCampaignID == 0 || dbContext.Campaign.Where(x => x.CampaignID == model.SelectedCampaignID && x.StatusID == 2).Any())
            //    model.CanEdit = false;
            //else model.CanEdit = true;

            //var DirectionsInfo = from x in dbContext.Direction.AsQueryable()
            //                     join pd in dbContext.ParentDirection on x.ParentID equals pd.ParentDirectionID
            //                     where x.AllowedDirections.Any(y => y.DirectionID == x.DirectionID && y.InstitutionID == insitutionID
            //                                                        && dbContext.CampaignEducationLevel
            //                                                            .Where(
            //                                                                z =>
            //                                                                    z.CampaignID == model.SelectedCampaignID &&
            //                                                                    z.Course == model.SelectedCourse)
            //                                                            .Select(z => z.EducationLevelID).Contains(y.AdmissionItemTypeID.Value))
            //                     select x;

            //model.CachedDirections = GetDirectionsInfo(DirectionsInfo).ToDictionary(x => x.DirectionID.ToString(), x => x);

            //// выгружаем отдельно все направления, чтобы детальная информация в попапах была доступна локально, и не надо было бегать на сервер
            ////model.CachedDirections = GetDirectionsInfo(
            ////    dbContext.Direction
            ////    .Where(x => x.AllowedDirections.Any(y => y.DirectionID == x.DirectionID && y.InstitutionID == insitutionID
            ////        && dbContext.CampaignEducationLevel
            ////            .Where(z => z.CampaignID == model.SelectedCampaignID && z.Course == model.SelectedCourse)
            ////            .Select(z => z.EducationLevelID).Contains(y.AdmissionItemTypeID))))
            ////    .ToDictionary(x => x.DirectionID.ToString(), x => x);

            //model.BudgetLevels = dbContext.LevelBudget.OrderBy(x => x.IdLevelBudget).Select(x => x.BudgetName).ToArray();

            return model;
        }

        /// <summary>
        /// Получить количество мест во всех конкурсных группах по данным параметрам
        /// </summary>
        private static AjaxResultModel GetCompetitiveGroupDirectionCount(
            this EntrantsEntities dbContext, int institutionID, int educationalLevelID, int? directionID, int campaignID)
        { //int course
            var groupItems = (from x in dbContext.CompetitiveGroupItem
                              where x.CompetitiveGroup.InstitutionID == institutionID
                                   && x.CompetitiveGroup.CampaignID == campaignID
                                   //&& x.CompetitiveGroup.Course == course
                              select new {
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

            //&& x.CompetitiveGroup.EducationLevelID == educationalLevelID
            //&& x.CompetitiveGroup.DirectionID == directionID
            //CompetitiveGroupItemID
            //var target = (from x in dbContext.CompetitiveGroupTargetItem
            //              where x.CompetitiveGroupItem.CompetitiveGroup.InstitutionID == institutionID
            //                   && x.CompetitiveGroupItem.CompetitiveGroup.CampaignID == campaignID
            //              //&& x.CompetitiveGroupItem.CompetitiveGroup.Course == course
            //              select new
            //              {
            //                  x.NumberTargetO,
            //                  x.NumberTargetOZ,
            //                  x.NumberTargetZ
            //              }).ToArray();

            //&& x.CompetitiveGroupItem.EducationLevelID == educationalLevelID
            //&& x.CompetitiveGroupItem.DirectionID == directionID
            return new AjaxResultModel {
                Data = new[]
			            {
			            Math.Max(0, groupItems.Sum(x => x.NumberBudgetO).Value),
						Math.Max(0, groupItems.Sum(x => x.NumberBudgetOZ).Value),
						Math.Max(0, groupItems.Sum(x => x.NumberBudgetZ).Value),
                        Math.Max(0, groupItems.Sum(x => x.NumberQuotaO.GetValueOrDefault())),
						Math.Max(0, groupItems.Sum(x => x.NumberQuotaOZ.GetValueOrDefault())),
						Math.Max(0, groupItems.Sum(x => x.NumberQuotaZ.GetValueOrDefault())),
						Math.Max(0, groupItems.Sum(x => x.NumberPaidO).Value),
						Math.Max(0, groupItems.Sum(x => x.NumberPaidOZ).Value),
						Math.Max(0, groupItems.Sum(x => x.NumberPaidZ).Value),
						Math.Max(0, 0),//target.Sum(x => x.NumberTargetO)),
						Math.Max(0, 0),//target.Sum(x => x.NumberTargetOZ)),
						Math.Max(0, 0),//target.Sum(x => x.NumberTargetZ)),
			            }
            };
        }

        /// <summary>
        /// Проверить, что числа в конкретном объёме приёма соответствуют конкурсным группам (не меньше чем уже есть)
        /// </summary>
        private static void ValidateAvailableVolumeCounts(this EntrantsEntities dbContext, AdmissionVolumeViewModel model, AdmissionVolume volume, List<int> errorIdx) {
            int[] data = (int[])GetCompetitiveGroupDirectionCount(dbContext, volume.InstitutionID, volume.AdmissionItemTypeID, volume.DirectionID,
                volume.CampaignID ?? 0).Data;
            if(volume.NumberBudgetO < data[0] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.FullTimeTuition))
                errorIdx.Add(0);
            if(volume.NumberBudgetOZ < data[1] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.MixedTuition))
                errorIdx.Add(1);
            if(volume.NumberBudgetZ < data[2] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.PostalTuition))
                errorIdx.Add(2);
            if(volume.NumberQuotaO.GetValueOrDefault() < data[3] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.FullTimeTuition) && model.IsQuotaEnabled(volume.AdmissionItemTypeID))
                errorIdx.Add(3);
            if(volume.NumberQuotaOZ.GetValueOrDefault() < data[4] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.MixedTuition) && model.IsQuotaEnabled(volume.AdmissionItemTypeID))
                errorIdx.Add(4);
            if(volume.NumberQuotaZ.GetValueOrDefault() < data[5] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.PostalTuition) && model.IsQuotaEnabled(volume.AdmissionItemTypeID))
                errorIdx.Add(5);
            if(volume.NumberPaidO < data[6] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.FullTimeTuition))
                errorIdx.Add(6);
            if(volume.NumberPaidOZ < data[7] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.MixedTuition))
                errorIdx.Add(7);
            if(volume.NumberPaidZ < data[8] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.PostalTuition))
                errorIdx.Add(8);
            if(volume.NumberTargetO < data[9] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.FullTimeTuition))
                errorIdx.Add(9);
            if(volume.NumberTargetOZ < data[10] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.MixedTuition))
                errorIdx.Add(10);
            if(volume.NumberTargetZ < data[11] && model.IsFormAvail(volume.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.PostalTuition))
                errorIdx.Add(11);
        }

        /// <summary>
        /// Сохранить объём приёма
        /// </summary>
        public static AjaxResultModel SaveAdmissionVolumeViewModel(this EntrantsEntities dbContext, GVUZ.DAL.Dapper.ViewModel.Admission.AdmissionVolumeViewModel model, int insitutionID) {
            if(model.Items == null)
                return new AjaxResultModel(AjaxResultModel.DataError);

            if(!dbContext.Campaign.Where(x => x.InstitutionID == insitutionID && x.CampaignID == model.SelectedCampaignID).Any())
                return new AjaxResultModel(AjaxResultModel.DataError);
            if(!dbContext.CampaignEducationLevel.Where(x => x.CampaignID == model.SelectedCampaignID).Any()) //&& x.Course == model.SelectedCourse
                return new AjaxResultModel(AjaxResultModel.DataError);

            //не даём редактировать при завершённой кампании
            if(model.SelectedCampaignID == 0 || dbContext.Campaign.Where(x => x.CampaignID == model.SelectedCampaignID && x.StatusID == 2).Any())
                return new AjaxResultModel("Невозможно редактировать объем приема завершенной приемной кампании.");

            var itemsForUpdate = model.Items;

            //var availForms = dbContext.CampaignDate.Where(
            //     x => x.CampaignID == model.SelectedCampaignID && x.Course == model.SelectedCourse && x.IsActive)
            //     .Select(x => new { x.EducationFormID, x.EducationSourceID, x.EducationLevelID }).Distinct().ToArray();

            //model.AvailForms = availForms.Select(x => new AdmissionVolumeViewModel.AvailFormsInfo {
            //    FormID = x.EducationFormID,
            //    LevelID = x.EducationLevelID,
            //    SourceID = x.EducationSourceID
            //}).ToArray();

            Dictionary<string, string> errors = new Dictionary<string, string>();
            //Dictionary<string, List<int>> errorIdxes = new Dictionary<string, List<int>>();
            Dictionary<string, List<Tuple<int, int>>> errorIdxes = new Dictionary<string, List<Tuple<int, int>>>();

            // существующие в базе данные
            var allVolumes = dbContext.AdmissionVolume.Where(x => x.InstitutionID == insitutionID
                && x.CampaignID == model.SelectedCampaignID).ToList(); //&& x.Course == model.SelectedCourse

            // существующие уиды          
            var existingUids = dbContext.AdmissionVolume.Where(x => x.InstitutionID == insitutionID)
                .Select(x => new { x.UID, x.AdmissionVolumeID }).ToArray()
                .Where(x => itemsForUpdate.Any(y =>  x.AdmissionVolumeID != y.AdmissionVolumeId && x.UID == y.UID && !String.IsNullOrWhiteSpace(y.UID)) && x.UID != null)
                .Select(x => x.UID).ToList();

            var deleteDistributed = new List<int>();

            //вставляем в базу новые записи/обновлённые записи
            foreach(var r in itemsForUpdate) {

                r.ParentDirectionID = r.ParentDirectionID == r.ParentID ? r.ParentDirectionID : r.ParentID;
                r.ParentDirectionID = r.ParentDirectionID == 0 ? null : r.ParentDirectionID;
                r.DirectionID = r.DirectionID == 0 ? null : r.DirectionID;


                var allowedDirections = dbContext.AllowedDirections
                    .Where(x => x.InstitutionID == insitutionID && x.AdmissionItemTypeID == r.AdmissionItemTypeID)
                    .Select(x => new {x.DirectionID, x.Direction.ParentID }).ToList();

                //var linkedCompetitiveGroups = dbContext.CompetitiveGroup
                //   .Where(x => x.DirectionID == r.DirectionID || x.ParentDirectionID == r.ParentDirectionID
                //   && x.CampaignID == model.SelectedCampaignID).Any();

                // TODO : linkedCompetitiveGroups
                //if (linkedCompetitiveGroups)
                //    return new AjaxResultModel("Невозможно отредактировать объем приема.");

                //Если новый объем приема по УГС, нужно удалить записи с направлениями 
                if (r.IsForUGS)
                {
                    var dirsForParentDirs = allowedDirections.Where(x => x.ParentID == r.ParentID).Select(x => x.DirectionID).ToList();

                    foreach (var dir in dirsForParentDirs)
                    {
                        var volumeForDelete = allVolumes.Where(x => x.DirectionID == dir).ToList();

                        if (volumeForDelete != null)
                        {
                            foreach (var item in volumeForDelete)
                            {
                                dbContext.AdmissionVolume.DeleteObject(item);
                            }
                        }   
                    }
                }
                //Удаление записей по УГС из объема 
                else if (!r.IsForUGS)
                {
                    var parentDirection =  allowedDirections.Where(x => x.DirectionID == r.DirectionID).Select(x => x.ParentID).FirstOrDefault();

                    var volumeForDelete = allVolumes.Where(x => x.ParentDirectionID == parentDirection).ToList();

                    if (volumeForDelete != null)
                    {
                        foreach (var item in volumeForDelete)
                        {
                            dbContext.AdmissionVolume.DeleteObject(item);
                        }
                    }                       
                }


                var dbVolumes = allVolumes
                        .Where(x => x.DirectionID == r.DirectionID && x.ParentDirectionID == r.ParentDirectionID
                        && x.AdmissionItemTypeID == r.AdmissionItemTypeID)
                        .ToList();


                AdmissionVolume dbVolume = dbVolumes.FirstOrDefault();
                AdmissionVolume volume = dbVolume ?? new AdmissionVolume();
                volume.AdmissionItemTypeID = r.AdmissionItemTypeID;
                volume.DirectionID = r.DirectionID;
                volume.InstitutionID = insitutionID;
                volume.ModifiedDate = DateTime.Now;
                volume.CreatedDate = volume.CreatedDate ?? DateTime.Now;
                volume.ParentDirectionID = r.ParentDirectionID;

                if (volume.NumberBudgetO != r.NumberBudgetO ||
                     volume.NumberBudgetOZ != r.NumberBudgetOZ ||
                     volume.NumberBudgetZ != r.NumberBudgetZ ||
                     volume.NumberQuotaO.GetValueOrDefault() != r.NumberQuotaO.GetValueOrDefault() ||
                     volume.NumberQuotaOZ.GetValueOrDefault() != r.NumberQuotaOZ.GetValueOrDefault() ||
                     volume.NumberQuotaZ.GetValueOrDefault() != r.NumberQuotaZ.GetValueOrDefault() ||
                     volume.NumberTargetO != r.NumberTargetO ||
                     volume.NumberTargetOZ != r.NumberTargetOZ ||
                     volume.NumberTargetZ != r.NumberTargetZ) {
                    deleteDistributed.Add(volume.AdmissionVolumeID);
                }

                volume.NumberBudgetO = r.NumberBudgetO;
                volume.NumberBudgetOZ = r.NumberBudgetOZ;
                volume.NumberBudgetZ = r.NumberBudgetZ;
                volume.NumberPaidO = r.NumberPaidO;
                volume.NumberPaidOZ = r.NumberPaidOZ;
                volume.NumberPaidZ = r.NumberPaidZ;
                volume.NumberTargetO = r.NumberTargetO;
                volume.NumberTargetOZ = r.NumberTargetOZ;
                volume.NumberTargetZ = r.NumberTargetZ;
                volume.NumberQuotaO = r.NumberQuotaO;
                volume.NumberQuotaOZ = r.NumberQuotaOZ;
                volume.NumberQuotaZ = r.NumberQuotaZ;

                volume.CampaignID = model.SelectedCampaignID;
                volume.Course = 1;//model.SelectedCourse;
                volume.UID = r.UID;

                if (r.UID != null && !String.IsNullOrWhiteSpace(r.UID)) {
                    if(existingUids.Contains(r.UID)) {
                        errorIdxes.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, new List<Tuple<int, int>> { new Tuple<int, int>( -1, -1) });
                        errors.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, "UID уже существует");
                    } else
                        existingUids.Add(r.UID);
                }

                if(dbVolume == null)
                    dbContext.AdmissionVolume.AddObject(volume);
                else {
                    //фикс предыдущих проблем, удаление лишних записей
                    if(dbVolumes.Count > 1)
                        dbVolumes.Skip(1).ToList().ForEach(dbContext.AdmissionVolume.DeleteObject);
                }

                //List<int> errorIdx = new List<int>();
                //ValidateAvailableVolumeCounts(dbContext, model, volume, errorIdx);
                //if(errorIdx.Count > 0) {
                //    errorIdxes.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, errorIdx);
                //    errors.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, "Недостаточно мест");
                //}
            }

            if(errors.Count > 0)
                return new AjaxResultModel("df") { Data = errors.Select(x => new { DirectionID = x.Key.Split(',')[1], AdmID = x.Key.Split(',')[0], Error = x.Value, ErrorIdx = errorIdxes[x.Key].ToArray() }).ToArray() };

            if(deleteDistributed.Count > 0) {
                dbContext.AdmissionVolume.Where(x => deleteDistributed.Contains(x.AdmissionVolumeID)).SelectMany(x => x.DistributedAdmissionVolume).ToList().ForEach(dbContext.DeleteObject);
            }
            //dbContext.SaveChanges(); теперь только проверяет
            return new AjaxResultModel();
        }

        //public static AdmissionVolumeViewModel SaveAdmissionVolume(AdmissionVolumeViewModel model, int institutionId)
        //{
            
        //    return DbConnection(db =>
        //    {
        //        IDbTransaction tran = null;
        //        try
        //        {
        //            tran = db.BeginTransaction();

        //            if (model != null)
        //            {

        //                DataTable volumeDT = new DataTable();
        //                volumeDT.Columns.Add("ID", typeof(int));
        //                volumeDT.Columns.Add("IdLevelBudget", typeof(int));
        //                volumeDT.Columns.Add("DataType", typeof(short));
        //                volumeDT.Columns.Add("UID", typeof(string));
        //                volumeDT.Columns.Add("AdmissionVolumeGUID", typeof(Guid));
        //                volumeDT.Columns.Add("InstitutionId", typeof(int));
        //                volumeDT.Columns.Add("AdmissionItemTypeID", typeof(short));
        //                volumeDT.Columns.Add("Course", typeof(int));
        //                volumeDT.Columns.Add("CampaignId", typeof(int));
        //                volumeDT.Columns.Add("ParentDirectionID", typeof(int));
        //                volumeDT.Columns.Add("DirectionID", typeof(int));
        //                volumeDT.Columns.Add("NumberBudgetO", typeof(int));
        //                volumeDT.Columns.Add("NumberBudgetOZ", typeof(int));
        //                volumeDT.Columns.Add("NumberBudgetZ", typeof(int));
        //                volumeDT.Columns.Add("NumberPaidO", typeof(int));
        //                volumeDT.Columns.Add("NumberPaidOZ", typeof(int));
        //                volumeDT.Columns.Add("NumberPaidZ", typeof(int));
        //                volumeDT.Columns.Add("NumberTargetO", typeof(int));
        //                volumeDT.Columns.Add("NumberTargetOZ", typeof(int));
        //                volumeDT.Columns.Add("NumberTargetZ", typeof(int));
        //                volumeDT.Columns.Add("NumberQuotaO", typeof(int));
        //                volumeDT.Columns.Add("NumberQuotaOZ", typeof(int));
        //                volumeDT.Columns.Add("NumberQuotaZ", typeof(int));
        //                volumeDT.Columns.Add("CreatedDate", typeof(DateTime));
        //                volumeDT.Columns.Add("ModifiedDate", typeof(DateTime));



        //                foreach (AdmissionVolumeViewModel.RowData row in model.Items)
        //                {

        //                    volumeDT.Rows.Add(row.AdmissionVolumeId
        //                                     , model.BudgetLevels.Where(x => x.BudgetName == row.BudgetName)
        //                                     , 0
        //                                     , row.UID
        //                                     , null
        //                                     , model.InstitutionID
        //                                     , row.AdmissionItemTypeID
        //                                     , 1
        //                                     , model.SelectedCampaignID
        //                                     , row.ParentDirectionID
        //                                     , row.DirectionID
        //                                     , row.NumberBudgetO
        //                                     , row.NumberBudgetOZ
        //                                     , row.NumberBudgetZ
        //                                     , row.NumberPaidO
        //                                     , row.NumberPaidOZ
        //                                     , row.NumberPaidZ
        //                                     , row.NumberTargetO
        //                                     , row.NumberTargetOZ
        //                                     , row.NumberTargetZ
        //                                     , row.NumberQuotaO
        //                                     , row.NumberQuotaOZ
        //                                     , row.NumberQuotaZ
        //                                     , DateTime.Now
        //                                     , DateTime.Now
        //                                     );
        //                }


        //                var admissionVolume = new SqlParameter("@av_data", SqlDbType.Structured);
        //                admissionVolume.TypeName = "[dbo].[ftct_AdmissionVolume]";
        //                admissionVolume.Value = volumeDT;


        //                var trans = tran as SqlTransaction;
        //                var connection = db as SqlConnection;

        //                SqlCommand cmd = new SqlCommand("SyncAdmissionVolume") { CommandType = CommandType.StoredProcedure };
        //                cmd.Parameters.Add(admissionVolume);
        //                //cmd.Parameters.AddWithValue("@institutionId", model.CompetitiveGroupEdit.InstitutionID);
        //                //cmd.Parameters.AddWithValue("@campaignId", model.CompetitiveGroupEdit.CampaignID);
        //                cmd.Connection = connection;
        //                cmd.Transaction = trans;

        //                cmd.ExecuteNonQuery();


        //            }
        //            tran.Commit();
        //            return model;
        //        }
        //        catch (Exception exc)
        //        {
        //            if (tran != null)
        //                tran.Rollback();
        //            throw;
        //        }
        //    });

        //}

        /// <summary>
        /// Заполняем модель для выбора нового разрешённого направления в ОО
        /// </summary>
        public static AllowedDirectionAddViewModel FillAllowedDirectionAddModel(this InstitutionsEntities dbContext)
        {
            AllowedDirectionAddViewModel model = new AllowedDirectionAddViewModel();
            var levels =
                dbContext.AdmissionItemType.Where(x => x.ItemLevel == 2)
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new { ID = (int)x.ItemTypeID, Name = x.Name }).ToList();
            levels.Insert(0, new { ID = 0, Name = "[Не выбран]" });
            model.EducationLevels = levels;
            var parDirections = dbContext.ParentDirection
                .OrderBy(x => x.Name)
                .Select(x => new { ID = x.ParentDirectionID, Name = x.Name })
                .ToList();
            parDirections.Insert(0, new { ID = 0, Name = "[Не выбран]" });
            model.ParentDirections = parDirections;
            return model;
        }

        /// <summary>
        /// Возвращаем оставшиеся доступные направления (исключая уже существующие)
        /// </summary>
        public static AjaxResultModel GetRemainedAvailableAllowedDirections(this InstitutionsEntities dbContext, AllowedDirectionAddViewModel model, int institutionID)
        {
            var existing = dbContext.AllowedDirections
                .Where(x => x.InstitutionID == institutionID && x.AdmissionItemTypeID == model.EducationLevelID && x.Direction.ParentID == model.ParentDirectionID)
                .Select(x => x.DirectionID);
            List<string> qualCodes = new List<string>();
            // выбираем по коду квалификации, который зависит от уровня образования
            if(model.EducationLevelID == EDLevelConst.Bachelor)
                qualCodes.Add("62");
            if(model.EducationLevelID == EDLevelConst.Magistracy)
                qualCodes.Add("68");
            if(model.EducationLevelID == EDLevelConst.SPO) {
                qualCodes.Add("51");
                qualCodes.Add("52");
            }

            if(model.EducationLevelID == EDLevelConst.Speciality)
                qualCodes.Add("65");
            if(model.EducationLevelID == EDLevelConst.HighQualification)
                qualCodes.Add("70");

            var avail =
                dbContext.Direction.Where(x => x.ParentID == model.ParentDirectionID && !existing.Contains(x.DirectionID))
                    .OrderBy(x => x.Code)
                    .Select(x => new { ID = x.DirectionID, Code = x.Code, Name = x.Name, Period = x.PERIOD, x.QUALIFICATIONCODE, x.NewCode }).ToArray()
                    .Where(x => qualCodes.Contains((x.QUALIFICATIONCODE ?? "").Trim()))
                    .Select(x => new { ID = x.ID, Code = x.Code, QualificationCode = x.QUALIFICATIONCODE, Name = x.Name, Period = (x.Period ?? "").Trim(), NewCode = x.NewCode }).ToArray();
            return new AjaxResultModel { Data = avail };
        }

        public static AjaxResultModel GetAllowedDirections(this InstitutionsEntities dbContext, AllowedDirectionAddViewModel model, int institutionID)
        {

            var existing = dbContext.AllowedDirections
                 .Where(x => x.InstitutionID == institutionID && x.AdmissionItemTypeID == model.EducationLevelID && x.Direction.ParentID == model.ParentDirectionID)
                 .Select(x => x.DirectionID);
            List<string> qualCodes = new List<string>();
            // выбираем по коду квалификации, который зависит от уровня образования
            if(model.EducationLevelID == EDLevelConst.Bachelor)
                qualCodes.Add("62");
            if(model.EducationLevelID == EDLevelConst.Magistracy)
                qualCodes.Add("68");
            if(model.EducationLevelID == EDLevelConst.SPO) {
                qualCodes.Add("51");
                qualCodes.Add("52");
            }

            if(model.EducationLevelID == EDLevelConst.Speciality)
                qualCodes.Add("65");
            if(model.EducationLevelID == EDLevelConst.HighQualification)
                qualCodes.Add("70");
            var avail =
                 dbContext.Direction.Where(x => x.ParentID == model.ParentDirectionID && existing.Contains(x.DirectionID))
                      .OrderBy(x => x.Code)
                      .Select(x => new { ID = x.DirectionID, Code = x.Code, Name = x.Name, Period = x.PERIOD, x.QUALIFICATIONCODE, x.NewCode }).ToArray()
                      .Where(x => qualCodes.Contains((x.QUALIFICATIONCODE ?? "").Trim()))
                      .Select(x => new { ID = x.ID, Code = x.Code, QualificationCode = x.QUALIFICATIONCODE, Name = x.Name, Period = (x.Period ?? "").Trim(), NewCode = x.NewCode }).ToArray();
            return new AjaxResultModel { Data = avail };
        }

        /// <summary>
        /// Список направлений отражённых в заявке на добавление
        /// </summary>
        public static AjaxResultModel GetRequestedToAddDirections(this InstitutionsEntities dbContext, int institutionID) {
            List<int> temp1;
            temp1 = dbContext.RequestDirection.Where(y => y.Activity == "W" && y.Request_ID == institutionID && y.Action == "Add").Select(n => n.Direction_ID).ToList();

            List<int> temp2 = new List<int>();
            foreach(int intro in temp1) {
                temp2.Add(Convert.ToInt32(intro));
            }

            var avail =
                 dbContext.Direction.Where(x => x.DirectionID > 0 && temp2.Contains(x.DirectionID))
                      .OrderBy(x => x.Code)
                      .Select(x => new { ID = x.DirectionID, Code = x.Code, Name = x.Name, Period = x.PERIOD, x.QUALIFICATIONCODE, NewCode = x.NewCode }).ToArray()
                //.Where(x => qualCodes.Contains((x.QUALIFICATIONCODE ?? "").Trim()))
                      .Select(x => new { ID = x.ID, Code = x.Code, QualificationCode = x.QUALIFICATIONCODE, Name = x.Name, Period = (x.Period ?? "").Trim(), NewCode = x.NewCode }).ToArray();
            return new AjaxResultModel { Data = avail };
        }

        /// <summary>
        /// Список направлений отражённых в заявке на udalenie
        /// </summary>
        public static AjaxResultModel GetRequestedToDeleteDirections(this InstitutionsEntities dbContext, int institutionID) {
            List<int> temp1;
            temp1 = dbContext.RequestDirection.Where(y => y.Activity == "W" && y.Request_ID == institutionID && y.Action == "Delete").Select(n => n.Direction_ID).ToList();

            List<int> temp2 = new List<int>();
            foreach(int intro in temp1) {
                temp2.Add(Convert.ToInt32(intro));
            }

            var avail =
                 dbContext.Direction.Where(x => x.DirectionID > 0 && temp2.Contains(x.DirectionID))
                      .OrderBy(x => x.Code)
                      .Select(x => new { ID = x.DirectionID, Code = x.Code, Name = x.Name, Period = x.PERIOD, x.QUALIFICATIONCODE, x.NewCode }).ToArray()
                //.Where(x => qualCodes.Contains((x.QUALIFICATIONCODE ?? "").Trim()))
                      .Select(x => new { ID = x.ID, Code = x.Code, QualificationCode = x.QUALIFICATIONCODE, Name = x.Name, Period = (x.Period ?? "").Trim(), NewCode = x.NewCode }).ToArray();
            return new AjaxResultModel { Data = avail };
        }

        public static AjaxResultModel GetDeniedDirections(this InstitutionsEntities dbContext, int institutionID) {
            List<int> temp1;
            temp1 = dbContext.RequestDirection.Where(y => y.Activity == "D" && y.Request_ID == institutionID).Select(n => n.Direction_ID).ToList();

            List<int> temp2 = new List<int>();
            foreach(int intro in temp1) {
                temp2.Add(Convert.ToInt32(intro));
            }

            var avail =
                 dbContext.Direction.Where(x => x.DirectionID > 0 && temp2.Contains(x.DirectionID))
                      .OrderBy(x => x.Code)
                      .Select(x => new { ID = x.DirectionID, Code = x.Code, Name = x.Name, Period = x.PERIOD, x.QUALIFICATIONCODE, x.NewCode }).ToArray()
                //.Where(x => qualCodes.Contains((x.QUALIFICATIONCODE ?? "").Trim()))
                      .Select(x => new { ID = x.ID, Code = x.Code, QualificationCode = x.QUALIFICATIONCODE, Name = x.Name, Period = (x.Period ?? "").Trim(), NewCode = x.NewCode }).ToArray();

            var query1 = dbContext.RequestComments
                 .Where(x => temp1.Contains(x.RequestDirection.Direction_ID)
                      && x.InstitutionID == institutionID
                      && x.Commentor == "A")
                      .OrderByDescending(x => x.Date);

            List<GVUZ.Model.Institutions.RequestComments> rlist = new List<GVUZ.Model.Institutions.RequestComments>();

            foreach(GVUZ.Model.Institutions.RequestComments rq in query1) {
                if(!rlist.Select(x => x.DirectionID).Contains(rq.DirectionID)) {
                    rlist.Add(rq);
                }
            }

            var comment = rlist
                 .Select(x => new { ID = x.DirectionID, Comment = x.Comment });

            return new AjaxResultModel { Data = new { Direction = avail, Comment = comment } };
        }


        /// <summary>
        /// Удаляем отклонённые направления в заявке
        /// </summary>
        public static AjaxResultModel DeleteDenied(this InstitutionsEntities dbContext, int instid) {
            var db = dbContext.RequestDirection.Where(x => x.Request_ID == instid && x.Activity == "D");

            foreach(GVUZ.Model.Institutions.RequestDirection rd in db) {
                dbContext.RequestDirection.DeleteObject(rd);
            }

            dbContext.SaveChanges();
            return new AjaxResultModel();
        }

        /// <summary>
        /// Добавляем новое разрешённое направление в базу
        /// </summary>
        public static AjaxResultModel AddAllowedDirection(this EntrantsEntities dbContext, AllowedDirectionAddViewModel model, int institutionID) {
            //пришли ошибочные данные, ничего не делаем. Ситуация в нормальной жизни не должна быть
            if(!dbContext.AdmissionItemType.Any(x => x.ItemTypeID == model.EducationLevelID && x.ItemLevel == 2))
                return new AjaxResultModel();
            foreach(var directionID in (model.DirectionIDs ?? new int[0])) {
                // если нет ещё, то добавляем
                if(!dbContext.AllowedDirections.Any(x => x.DirectionID == directionID && x.AdmissionItemTypeID == model.EducationLevelID && x.InstitutionID == institutionID)) {
                    AllowedDirections ad = new AllowedDirections();
                    ad.InstitutionID = institutionID;
                    ad.AdmissionItemTypeID = (short)model.EducationLevelID;
                    ad.DirectionID = directionID;
                    dbContext.AllowedDirections.AddObject(ad);
                }

                dbContext.SaveChanges();
            }

            return new AjaxResultModel();
        }

        /// <summary>
        /// Удаляем разрешённое направление из базы. Проверям что это допустимо.
        /// </summary>
        public static AjaxResultModel DeleteAllowedDirection(this EntrantsEntities dbContext, int educationLevelID, int directionID, int institutionID)
        {
            //пришли ошибочные данные, ничего не делаем. Ситуация в нормальной жизни не должна быть
            if(!dbContext.AdmissionItemType.Any(x => x.ItemTypeID == educationLevelID && x.ItemLevel == 2))
                return new AjaxResultModel();
            if(dbContext.CompetitiveGroupItem.Any(x => x.CompetitiveGroup.InstitutionID == institutionID && x.CompetitiveGroup.DirectionID == directionID
                                                                     && x.CompetitiveGroup.EducationLevelID == educationLevelID))
                return new AjaxResultModel("Невозможно удалить направление, так как оно используется в конкурсах.");

            var ad = dbContext.AllowedDirections.FirstOrDefault(x => x.DirectionID == directionID && x.AdmissionItemTypeID == educationLevelID && x.InstitutionID == institutionID);
            if(ad != null) {
                // до кучи удаляем объём приёма, он уже больше не нужен (КГ проверили, нет)
                dbContext.AdmissionVolume
                    .Where(x => x.DirectionID == directionID && x.InstitutionID == institutionID && x.AdmissionItemTypeID == educationLevelID)
                    .ToList().ForEach(dbContext.AdmissionVolume.DeleteObject);
                dbContext.AllowedDirections.DeleteObject(ad);
                dbContext.SaveChanges();
            }

            return new AjaxResultModel();
        }

        /// <summary>
        /// Получаем информацию по одному направлению (для попапов)
        /// </summary>
        public static AjaxResultModel GetDirectionInfo(this EntrantsEntities dbContext, int directionID) {
            var model = GetDirectionsInfo(dbContext.Direction.Where(x => x.DirectionID == directionID));
            return new AjaxResultModel { Data = model.Length > 0 ? model[0] : null };
        }

        /// <summary>
        /// Получаем список направлений (для попапов)
        /// </summary>
        public static AdmissionVolumeViewModel.DirectionInfo[] GetDirectionsInfo(IQueryable<Direction> query)
        { 
            var modelArray = query
                .Select(x => new AdmissionVolumeViewModel.DirectionInfo {
                    DirectionCode = x.Code,
                    DirectionID = x.DirectionID,
                    DirectionName = x.Name,
                    QualificationCode = x.QUALIFICATIONCODE,
                    ParentCode = x.ParentDirection.Code,
                    ParentName = x.ParentDirection.Name,
                    NewCode = x.NewCode
                }).ToArray();
            foreach(var model in modelArray) {
                string qaCode = (model.QualificationCode ?? "").Trim();
                short edID = 0;
                if(qaCode == "62") edID = EDLevelConst.Bachelor;
                if(qaCode == "65") edID = EDLevelConst.Speciality;
                if(qaCode == "68") edID = EDLevelConst.Magistracy;
                if(qaCode == "51" || qaCode == "52") edID = EDLevelConst.SPO;
                if(qaCode == "70") edID = EDLevelConst.HighQualification;
                if(edID == 0)
                    model.EducationLevelName = qaCode;
                else
                    model.EducationLevelName = DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, edID);
            }

            return modelArray;
        }

        public static bool IsInCompetitiveGroup(this EntrantsEntities dbContext, int institution, int direction, int admtype)
        {
            return (dbContext.CompetitiveGroupItem.Any(x => x.CompetitiveGroup.InstitutionID == institution && x.CompetitiveGroup.DirectionID == direction && x.CompetitiveGroup.EducationLevelID == (short)admtype));
        }


    }
}