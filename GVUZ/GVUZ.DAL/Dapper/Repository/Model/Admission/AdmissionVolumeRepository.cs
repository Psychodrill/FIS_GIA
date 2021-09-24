using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GVUZ.DAL.Dapper.ViewModel.Admission;
using GVUZ.DAL.Dapper.Model.Campaigns;
using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using System.Data;
using System.Data.SqlClient;
using GVUZ.Web.ViewModels.KcpDistribution;



namespace GVUZ.DAL.Dapper.Repository.Model.Admission
{
    public class AdmissionVolumeRepository : GvuzRepository, IAdmissionVolumeRepository
    {
        public AdmissionVolumeRepository() : base()
        {

        }
        public AdmissionVolumeViewModel FillAdmissionVolumeViewModel(AdmissionVolumeViewModel model, bool forEdit)
        {
            return DbConnection(db =>
            { 
                var allCampaigns = db.Query<Campaign>(sql: SQLQuery.AllowedCampaigns, param:
                    new
                    {
                        InstitutionID = model.InstitutionID
                    });
                model.AllowedCampaigns = allCampaigns.Select(x => new AdmissionVolumeViewModel.CampaignData
                    {
                        ID = x.CampaignID,
                        Name = x.Name
                    });
                if (model.SelectedCampaignID == 0)
                {
                    if (model.AllowedCampaigns.Any())
                    {
                        model.SelectedCampaignID = model.AllowedCampaigns.FirstOrDefault().ID;
                    }
                }

                model.HasPlan = db.Query<PlanAdmissionVolume>(sql: SQLQuery.PlanAdmissionVolumeByCampaign, param:
                   new { campaignId = model.SelectedCampaignID }).Any();

                var q1 = db.Query<AdmissionVolumeViewModel.AllowedDirectionView>(sql: SQLQuery.AllowedDirection, param: new
                {
                    InstitutionID = model.InstitutionID,
                    CampaignID = model.SelectedCampaignID
                }).ToArray();

                var campaign = allCampaigns.Where(t => t.CampaignID == model.SelectedCampaignID).FirstOrDefault();
                var campaignEducationFormFlag = campaign != null ? campaign.EducationFormFlag : 0;
                //var q1 = db.Query<AllowedDirection, AdmissionItemType, Direction, ParentDirection, AllowedDirection>(sql: SQLQuery.AllowedDirection,
                //    param: new
                //    {
                //        InstitutionID = model.InstitutionID,
                //        CampaignID = model.SelectedCampaignID
                //    },
                //    map: (allowedDirection, admissionItemType, direction, parentDirection) =>
                //    {
                //        allowedDirection.AdmissionItemType = admissionItemType;
                //        allowedDirection.Direction = direction;
                //        direction.AllowedDirections.Add(allowedDirection);
                //        direction.ParentDirection = parentDirection;
                //        parentDirection.Directions.Add(direction);
                //        return allowedDirection;
                //    },
                //    splitOn: "AdmissionItemTypeID, ItemTypeID, DirectionID, DirectionID, ParentID, ParentDirectionID").Select(x => new
                //    {
                //        x.AdmissionItemTypeID,
                //        AdmissionTypeName = x.AdmissionItemType.Name,
                //        x.DirectionID,
                //        x.Direction.Code,
                //        x.Direction.NewCode,
                //        DirectionName = x.Direction.Name,
                //        x.Direction.ParentDirection.ParentDirectionID,
                //        ParentDirectionName = x.Direction.ParentDirection.Name,
                //        ParentDirectionCode = x.Direction.ParentDirection.Code,
                //        QualificationCode = x.Direction.QUALIFICATIONCODE
                //    }).ToArray();


                //var q2 = db.Query<AdmissionVolume>(sql: "SELECT * FROM AdmissionVolume AS av WHERE av.InstitutionID = @InstitutionID", param: new { InstitutionID = model.InstitutionID }).ToList();
                var q2 = db.Query<AdmissionVolume>(sql: "SELECT * FROM AdmissionVolume AS av WHERE av.CampaignID = @CampaignID", param: new { CampaignID = model.SelectedCampaignID }).ToList();

                var distributed = q2.ToDictionary(v => v.AdmissionVolumeID, dv => db.Query<DistributedAdmissionVolume>(sql: "SELECT * FROM DistributedAdmissionVolume AS dav WHERE dav.AdmissionVolumeID = @AdmissionVolumeID",
                    param: new
                    {
                        AdmissionVolumeID = dv.AdmissionVolumeID
                    }).Sum(v => v.NumberBudgetO + v.NumberBudgetOZ + v.NumberBudgetZ + v.NumberQuotaO + v.NumberQuotaOZ + v.NumberQuotaZ + v.NumberTargetO + v.NumberTargetOZ + v.NumberTargetZ));

                model.Items = /*new List<AdmissionVolumeViewModel.RowData>(q1.Count());*/ new AdmissionVolumeViewModel.RowData[q1.Count()];
                int prevAdmID = 0;
                int? prevParDirID = 0;
                model.TreeItems = new List<List<List<AdmissionVolumeViewModel.RowData>>>();
                for (int i = 0; i < q1.Length; i++)
                {
                    AdmissionVolumeViewModel.RowData r = new AdmissionVolumeViewModel.RowData();
                    var item = q1[i];
                    if (item.AdmissionItemTypeID != prevAdmID)
                    {
                        model.TreeItems.Add(new List<List<AdmissionVolumeViewModel.RowData>>());
                        prevAdmID = item.AdmissionItemTypeID;
                        prevParDirID = 0;
                    }
                    var pdirs = model.TreeItems[model.TreeItems.Count - 1];
                    if (prevParDirID == 0 || prevParDirID != item.ParentDirectionID)
                    {
                        pdirs.Add(new List<AdmissionVolumeViewModel.RowData>());
                        prevParDirID = item.ParentDirectionID;
                    }
                    pdirs[pdirs.Count - 1].Add(r);
                    model.Items[i] = r;
                    //r.ParentDirectionID = q1[i].ParentDirectionID;

                    // Берем направление из разрешенных для ОО
                    r.DirectionID = q1[i].DirectionID;

                    r.AdmissionItemTypeID = q1[i].AdmissionItemTypeID;
                    r.AdmissionItemTypeName = q1[i].AdmissionTypeName;
                    r.DirectionName = q1[i].DirectionName;
                    r.ParentDirectionName = q1[i].ParentDirectionName;
                    r.ParentDirectionCode = q1[i].ParentDirectionCode;
                    r.DirectionCode = q1[i].Code;
                    r.DirectionNewCode = q1[i].NewCode;
                    r.QualificationCode = (q1[i].QualificationCode == null ? "" : q1[i].QualificationCode.Trim());
                    r.ParentID = q1[i].ParentDirectionID;

                    // Смотрим в AdmissionVolume, есть ли записи с таким направлением. Если нет, значит AdmissionVolume задан по УГС (ParentDirection), найдем такие записи:
                    AdmissionVolume volume = q2.Find(x => x.DirectionID == r.DirectionID && x.AdmissionItemTypeID == r.AdmissionItemTypeID) ??
                                             q2.Find(x => x.ParentDirectionID == q1[i].ParentDirectionID && x.AdmissionItemTypeID == r.AdmissionItemTypeID);
                   
                    r.ParentDirectionID = volume?.ParentDirectionID;

                    r.IsUGS = volume?.ParentDirectionID != null ? true : false;
                    r.IsForUGS = volume?.DirectionID == null ? true : false;

                    
                    r.DisableForEditing = false;
                    r.DisableFormO = (campaignEducationFormFlag & 1) == 0;
                    r.DisableFormOZ = (campaignEducationFormFlag & 2) == 0;
                    r.DisableFormZ = (campaignEducationFormFlag & 4) == 0;

                    if (volume != null)
                    {
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.FullTimeTuition))
                        {
                            r.NumberBudgetO = volume.NumberBudgetO;
                            r.NumberQuotaO =  volume.NumberQuotaO;
                        }
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.FullTimeTuition))
                            r.NumberPaidO = volume.NumberPaidO;
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.FullTimeTuition))
                            r.NumberTargetO = volume.NumberTargetO;

                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.MixedTuition))
                        {
                            r.NumberBudgetOZ = volume.NumberBudgetOZ;
                            r.NumberQuotaOZ = volume.NumberQuotaOZ;
                        }
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.MixedTuition))
                            r.NumberPaidOZ = volume.NumberPaidOZ;
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.MixedTuition))
                            r.NumberTargetOZ = volume.NumberTargetOZ;

                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.BudgetPlaces, AdmissionItemTypeConstants.PostalTuition))
                        {
                            r.NumberBudgetZ = volume.NumberBudgetZ;
                            r.NumberQuotaZ = volume.NumberQuotaZ;
                        }
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.PaidPlaces, AdmissionItemTypeConstants.PostalTuition))
                            r.NumberPaidZ = volume.NumberPaidZ;
                        if (forEdit || model.IsFormAvail(r.AdmissionItemTypeID, AdmissionItemTypeConstants.TargetReception, AdmissionItemTypeConstants.PostalTuition))
                            r.NumberTargetZ = volume.NumberTargetZ;
                        r.UID = volume.UID;
                        r.AdmissionVolumeId = volume.AdmissionVolumeID;
                        r.AvailableForDistribution = volume.NumberBudgetO + volume.NumberBudgetOZ + volume.NumberBudgetZ +
                                                              volume.NumberQuotaO.GetValueOrDefault() + volume.NumberQuotaOZ.GetValueOrDefault() +
                                                              volume.NumberQuotaZ.GetValueOrDefault() + volume.NumberTargetO +
                                                              volume.NumberTargetOZ + volume.NumberTargetZ;
                        r.TotalDistributed = distributed[volume.AdmissionVolumeID];
                    }
                 
                }

                //foreach (var treeItem in model.TreeItems)
                //{
                //    foreach (var items in treeItem)
                //    {
                //        if (items.Any(x => x.IsForUGS)) {

                //            items.Add(items[0]);
                //            for (int i = 1; i < items.Count(); i++)
                //            {
                //                items[i].NumberBudgetO = 0;
                //                items[i].NumberBudgetOZ = 0;
                //                items[i].NumberBudgetZ = 0;
                //                items[i].NumberPaidO = 0;
                //                items[i].NumberPaidOZ = 0;
                //                items[i].NumberPaidZ = 0;
                //                items[i].NumberQuotaO = 0;
                //                items[i].NumberQuotaOZ = 0;
                //                items[i].NumberQuotaZ = 0;
                //                items[i].NumberTargetO = 0;
                //                items[i].NumberTargetOZ = 0;
                //                items[i].NumberTargetZ = 0;
                //                items[i].UID = "";
                //            }
                //        }                       
                //    }
                //}


                foreach (var treeItem in model.TreeItems)
                {
                    var pDir = treeItem[treeItem.Count - 1];
                    pDir[pDir.Count - 1].GroupLast = true;
                }
                if (model.SelectedCampaignID == 0 || db.Query<Campaign>(sql: SQLQuery.AllowedCampaigns, param: new { InstitutionID = model.InstitutionID })
                    .Where(x => x.CampaignID == model.SelectedCampaignID && x.StatusID == 2).Any())
                { model.CanEdit = false; }
                else { model.CanEdit = true; }

                //Кнопка переброса мест видима и активна для всех приемных кампаний, для которых Campaign.YearStart >= 2018.
                if (model.SelectedCampaignID == 0 || db.Query<Campaign>(sql: SQLQuery.AllowedCampaigns, param: new { InstitutionID = model.InstitutionID })
                .Where(x => x.CampaignID == model.SelectedCampaignID && x.YearStart >= 2018).Any())
                { model.CanTransfer = true; }
                else { model.CanTransfer = false; }

                var DirectionsInfo = db.Query<Direction, ParentDirection, AdmissionItemType, Direction>(sql: SQLQuery.DirectionsInfo,
                    param: new
                    {
                        InstitutionID = model.InstitutionID,
                        CampaignID = model.SelectedCampaignID
                    },
                    map: (direction, parentDirection, admissionItemType) =>
                    {
                        direction.ParentDirection = parentDirection;
                        parentDirection.Directions.Add(direction);
                        direction.EDULEVEL = admissionItemType.Name;
                        return direction;
                    },
                    splitOn: "ParentDirectionID, ParentID, ItemTypeID").AsQueryable();

                model.CachedDirections = new Dictionary<string, AdmissionVolumeViewModel.DirectionInfo>();
                foreach(var x in GetDirectionsInfo(DirectionsInfo))
                {
                    if (!model.CachedDirections.ContainsKey(x.DirectionID.ToString()))
                        model.CachedDirections.Add(x.DirectionID.ToString(), x);
                }
                //model.CachedDirections = GetDirectionsInfo(DirectionsInfo).ToDictionary(x =>  x.DirectionID.ToString(), x => x);
                model.BudgetLevels = Dictionary.DictionaryContext.Dictionaries.GetLevelBudget();

                return model;
            });
        }

        public AdmissionVolumeViewModel.RowData CheckKCP(int campaignID, int educationLevelID, int? directionID, int? parentDirectionID)
        {
            return DbConnection(db =>
            {
                return db.Query<AdmissionVolumeViewModel.RowData>(sql: SQLQuery.AdmissionVolumeCheckKCP, param:
                    new
                    {
                        CampaignID = campaignID,
                        EducationLevelID = educationLevelID,
                        DirectionID = directionID,
                        ParentDirectionID = parentDirectionID
                    }).FirstOrDefault();
            });
        }

        public AdmissionVolumeViewModel.RowData CheckDistributionKCP(int AdmissionVolumeId, int BudgetLevelId)
        {
            return DbConnection(db =>
            {
                return db.Query<AdmissionVolumeViewModel.RowData>(sql: SQLQuery.DistributedAdmissionVolumeCheckKCP, param:
                    new
                    {
                        AdmissionVolumeId = AdmissionVolumeId,
                        BudgetLevelId = BudgetLevelId
                    }).FirstOrDefault();
            });
        }

        public AdmissionVolumeViewModel SaveAdmissionVolume(AdmissionVolumeViewModel model)
        {

            //AdmissionVolumeViewModel newModel = new AdmissionVolumeViewModel(model);

            model.BudgetLevels = Dictionary.DictionaryContext.Dictionaries.GetLevelBudget();
           // model.InstitutionID

            return 
            
             DbConnection<AdmissionVolumeViewModel>(db =>
            {
                IDbTransaction tran = null;
                try
                {
                    tran = db.BeginTransaction();

                    if (model != null)
                    {

                        DataTable volumeDT = new DataTable();
                        volumeDT.Columns.Add("ID", typeof(int));
                        volumeDT.Columns.Add("IdLevelBudget", typeof(int));
                        volumeDT.Columns.Add("DataType", typeof(short));
                        volumeDT.Columns.Add("UID", typeof(string));
                        volumeDT.Columns.Add("AdmissionVolumeGUID", typeof(Guid));
                        volumeDT.Columns.Add("InstitutionId", typeof(int));
                        volumeDT.Columns.Add("AdmissionItemTypeID", typeof(short));
                        volumeDT.Columns.Add("Course", typeof(int));
                        volumeDT.Columns.Add("CampaignId", typeof(int));
                        volumeDT.Columns.Add("ParentDirectionID", typeof(int));
                        volumeDT.Columns.Add("DirectionID", typeof(int));
                        volumeDT.Columns.Add("NumberBudgetO", typeof(int));
                        volumeDT.Columns.Add("NumberBudgetOZ", typeof(int));
                        volumeDT.Columns.Add("NumberBudgetZ", typeof(int));
                        volumeDT.Columns.Add("NumberPaidO", typeof(int));
                        volumeDT.Columns.Add("NumberPaidOZ", typeof(int));
                        volumeDT.Columns.Add("NumberPaidZ", typeof(int));
                        volumeDT.Columns.Add("NumberTargetO", typeof(int));
                        volumeDT.Columns.Add("NumberTargetOZ", typeof(int));
                        volumeDT.Columns.Add("NumberTargetZ", typeof(int));
                        volumeDT.Columns.Add("NumberQuotaO", typeof(int));
                        volumeDT.Columns.Add("NumberQuotaOZ", typeof(int));
                        volumeDT.Columns.Add("NumberQuotaZ", typeof(int));
                        volumeDT.Columns.Add("CreatedDate", typeof(DateTime));
                        volumeDT.Columns.Add("ModifiedDate", typeof(DateTime));



                        foreach (AdmissionVolumeViewModel.RowData row in model.Items)
                        {

                            volumeDT.Rows.Add(row.AdmissionVolumeId
                                             , model.BudgetLevels.Where(x => x.BudgetName == row.BudgetName).Select(x=>x.IdLevelBudget).FirstOrDefault()
                                             , 0
                                             , row.UID
                                             , null
                                             , model.InstitutionID
                                             , row.AdmissionItemTypeID
                                             , 1
                                             , model.SelectedCampaignID
                                             , row.ParentID
                                             , row.DirectionID
                                             , row.NumberBudgetO
                                             , row.NumberBudgetOZ
                                             , row.NumberBudgetZ
                                             , row.NumberPaidO
                                             , row.NumberPaidOZ
                                             , row.NumberPaidZ
                                             , row.NumberTargetO
                                             , row.NumberTargetOZ
                                             , row.NumberTargetZ
                                             , row.NumberQuotaO
                                             , row.NumberQuotaOZ
                                             , row.NumberQuotaZ
                                             , null
                                             , null
                                             );
                        }


                        var admissionVolume = new SqlParameter("@av_data", SqlDbType.Structured);
                        admissionVolume.TypeName = "[dbo].[ftct_AdmissionVolume]";
                        admissionVolume.Value = volumeDT;


                        var trans = tran as SqlTransaction;
                        var connection = db as SqlConnection;

                        SqlCommand cmd = new SqlCommand("SyncAdmissionVolume") { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.Add(admissionVolume);
                        cmd.Connection = connection;
                        cmd.Transaction = trans;

                        cmd.ExecuteNonQuery();


                    }
                    tran.Commit();

                    return model;
                }
                catch (Exception exc)
                {
                    if (tran != null)
                        tran.Rollback();
                    throw;
                }
            });

            //return DbConnection(db =>
            //{
            //    var campaign = db.Query<Campaign>(sql: "SELECT * FROM Campaign AS c WHERE c.InstitutionID=@InstitutionID AND c.CampaignID=@CampaignID", param: new { InstitutionID = model.InstitutionID, CampaignID = model.SelectedCampaignID });
            //    if (model.Items == null) { return false; }
            //    if (!campaign.Any())
            //    { return false; }
            //    if (!db.Query<CampaignEducationLevel>(sql: "SELECT * FROM CampaignEducationLevel AS cel WHERE cel.CampaignID = @CampaignID", param: new { CampaignID = model.SelectedCampaignID }).Any())
            //    { return false; }
            //    if (model.SelectedCampaignID == 0 || campaign.Where(x => x.StatusID == 2).Any())
            //    { return false; }// new AjaxResultModel("Невозможно редактировать объем приема у завершенной приемной кампании.");

            //    var itemsForUpdate = model.Items;
            //    Dictionary<string, string> errors = new Dictionary<string, string>();
            //    Dictionary<string, List<int>> errorIdxes = new Dictionary<string, List<int>>();

            //    var aVolume = db.Query<AdmissionVolume, DistributedAdmissionVolume, AdmissionVolume>(sql: SQLQuery.GetAdmissionVolume,
            //        param: new
            //        {
            //            InstitutionID = model.InstitutionID
            //        },
            //        map: (admissionVolume, distributedAdmissionVolume) =>
            //        {
            //            admissionVolume.DistributedAdmissionVolumes.Add(distributedAdmissionVolume);
            //            distributedAdmissionVolume.AdmissionVolume = admissionVolume;
            //            return admissionVolume;
            //        },
            //        splitOn: "AdmissionVolumeID, AdmissionVolumeID"
            //    );

            //    var allVolumes = aVolume.Where(x => x.CampaignID == model.SelectedCampaignID).ToList();

            //    var existingUids = aVolume.Select(x => new { x.DirectionID, x.CampaignID, x.UID }).ToArray()
            //    .Where(x => !itemsForUpdate
            //        .Any(y => y.DirectionID == x.DirectionID && model.SelectedCampaignID == x.CampaignID) && (x.UID != null && x.UID != string.Empty))
            //    .Select(x => x.UID).ToList();

            //    var deleteDistributed = new List<int>();
            //    var avList = new List<AdmissionVolume>();

            //    foreach (var r in itemsForUpdate)
            //    {
            //        var dbVolumes = aVolume.Where(x => x.DirectionID == r.DirectionID && x.AdmissionItemTypeID == r.AdmissionItemTypeID).ToList();
            //        AdmissionVolume dbVolume = dbVolumes.FirstOrDefault();
            //        AdmissionVolume volume = dbVolume ?? new AdmissionVolume();

            //        volume.AdmissionItemTypeID = r.AdmissionItemTypeID;
            //        volume.DirectionID = r.DirectionID;
            //        volume.InstitutionID = model.InstitutionID;

            //        if (volume.NumberBudgetO != r.NumberBudgetO ||
            //         volume.NumberBudgetOZ != r.NumberBudgetOZ ||
            //         volume.NumberBudgetZ != r.NumberBudgetZ ||
            //         volume.NumberQuotaO.GetValueOrDefault() != r.NumberQuotaO.GetValueOrDefault() ||
            //         volume.NumberQuotaOZ.GetValueOrDefault() != r.NumberQuotaOZ.GetValueOrDefault() ||
            //         volume.NumberQuotaZ.GetValueOrDefault() != r.NumberQuotaZ.GetValueOrDefault() ||
            //         volume.NumberTargetO != r.NumberTargetO ||
            //         volume.NumberTargetOZ != r.NumberTargetOZ ||
            //         volume.NumberTargetZ != r.NumberTargetZ)
            //        {
            //            deleteDistributed.Add(volume.AdmissionVolumeID);
            //        }
            //        volume.NumberBudgetO = r.NumberBudgetO;
            //        volume.NumberBudgetOZ = r.NumberBudgetOZ;
            //        volume.NumberBudgetZ = r.NumberBudgetZ;
            //        volume.NumberPaidO = r.NumberPaidO;
            //        volume.NumberPaidOZ = r.NumberPaidOZ;
            //        volume.NumberPaidZ = r.NumberPaidZ;
            //        volume.NumberTargetO = r.NumberTargetO;
            //        volume.NumberTargetOZ = r.NumberTargetOZ;
            //        volume.NumberTargetZ = r.NumberTargetZ;
            //        volume.NumberQuotaO = r.NumberQuotaO;
            //        volume.NumberQuotaOZ = r.NumberQuotaOZ;
            //        volume.NumberQuotaZ = r.NumberQuotaZ;

            //        volume.CampaignID = model.SelectedCampaignID;
            //        volume.Course = 1;
            //        volume.UID = r.UID;

            //        if (r.UID != null)
            //        {
            //            if (existingUids.Contains(r.UID))
            //            {
            //                errorIdxes.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, new List<int> { -1 });
            //                errors.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, "UID уже существует");
            //            }
            //            else
            //                existingUids.Add(r.UID);
            //        }
            //        if (dbVolume == null)
            //        {
            //            avList.Add(volume);
            //        }
            //        else
            //        {
            //            if (dbVolumes.Count > 1)
            //            {
            //                dbVolumes.Skip(1).ToList().ForEach(x => avList.Remove(x));
            //            }
            //        }
            //        List<int> errorIdx = new List<int>();
            //        //ValidateAvailableVolumeCounts(dbContext, model, volume, errorIdx);
            //        if (errorIdx.Count > 0)
            //        {
            //            errorIdxes.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, errorIdx);
            //            errors.Add(volume.AdmissionItemTypeID + "," + volume.DirectionID, "Недостаточно мест");
            //        }
            //    }
            //    if (errors.Count > 0)
            //    {
            //        var Data = errors.Select(x => new { DirectionID = x.Key.Split(',')[1], AdmID = x.Key.Split(',')[0], Error = x.Value, ErrorIdx = errorIdxes[x.Key].ToArray() }).ToArray();
            //    }
            //    if (deleteDistributed.Count > 0)
            //    {
            //        avList.ForEach(x => x.DistributedAdmissionVolumes.Remove(new DistributedAdmissionVolume { DistributedAdmissionVolumeID = deleteDistributed.FirstOrDefault() }));
            //    }
            //    foreach (var avl in avList)
            //    {
            //        //
            //        db.Query(sql: SQLQuery.UpdateAdmissionVolume, param: avl);
            //    }
            //    return true;
            //});
        }

        public AdmissionVolumeViewModel UpdateAdmissionVolume(KcpUpdateViewModel model)
        {

            //    //AdmissionVolumeViewModel newModel = new AdmissionVolumeViewModel(model);


            //    // model.InstitutionID

              return

            DbConnection<AdmissionVolumeViewModel>(db =>
            {
                IDbTransaction tran = null;
                try
                {
                    tran = db.BeginTransaction();

                    if (model != null)
                    {

                        DataTable volumeDT = new DataTable();
                        volumeDT.Columns.Add("ID", typeof(int));
                        volumeDT.Columns.Add("IdLevelBudget", typeof(int));
                        volumeDT.Columns.Add("DataType", typeof(short));
                        volumeDT.Columns.Add("UID", typeof(string));
                        volumeDT.Columns.Add("AdmissionVolumeGUID", typeof(Guid));
                        volumeDT.Columns.Add("InstitutionId", typeof(int));
                        volumeDT.Columns.Add("AdmissionItemTypeID", typeof(short));
                        volumeDT.Columns.Add("Course", typeof(int));
                        volumeDT.Columns.Add("CampaignId", typeof(int));
                        volumeDT.Columns.Add("ParentDirectionID", typeof(int));
                        volumeDT.Columns.Add("DirectionID", typeof(int));
                        volumeDT.Columns.Add("NumberBudgetO", typeof(int));
                        volumeDT.Columns.Add("NumberBudgetOZ", typeof(int));
                        volumeDT.Columns.Add("NumberBudgetZ", typeof(int));
                        volumeDT.Columns.Add("NumberPaidO", typeof(int));
                        volumeDT.Columns.Add("NumberPaidOZ", typeof(int));
                        volumeDT.Columns.Add("NumberPaidZ", typeof(int));
                        volumeDT.Columns.Add("NumberTargetO", typeof(int));
                        volumeDT.Columns.Add("NumberTargetOZ", typeof(int));
                        volumeDT.Columns.Add("NumberTargetZ", typeof(int));
                        volumeDT.Columns.Add("NumberQuotaO", typeof(int));
                        volumeDT.Columns.Add("NumberQuotaOZ", typeof(int));
                        volumeDT.Columns.Add("NumberQuotaZ", typeof(int));
                        volumeDT.Columns.Add("CreatedDate", typeof(DateTime));
                        volumeDT.Columns.Add("ModifiedDate", typeof(DateTime));



                        foreach (KcpBudgetLevelViewModel budgetLevel in model.BudgetLevels)
                        {

                            volumeDT.Rows.Add(model.AdmissionVolumeId
                                             ,budgetLevel.BudgetLevelId
                                             , 2
                                             , null
                                             , null
                                             , null
                                             , null
                                             , 1
                                             , null
                                             , null
                                             , null
                                             , budgetLevel.Budget.O.Value
                                             , budgetLevel.Budget.OZ.Value
                                             , budgetLevel.Budget.Z.Value
                                             , null
                                             , null
                                             , null
                                             , budgetLevel.Target.O.Value
                                             , budgetLevel.Target.OZ.Value
                                             , budgetLevel.Target.Z.Value
                                             , budgetLevel.Quota.O.Value
                                             , budgetLevel.Quota.OZ.Value
                                             , budgetLevel.Quota.Z.Value
                                             , DateTime.Now
                                             , DateTime.Now
                                             );
                        }


                        var admissionVolume = new SqlParameter("@av_data", SqlDbType.Structured);
                        admissionVolume.TypeName = "[dbo].[ftct_AdmissionVolume]";
                        admissionVolume.Value = volumeDT;


                        var trans = tran as SqlTransaction;
                        var connection = db as SqlConnection;

                        SqlCommand cmd = new SqlCommand("SyncAdmissionVolume") { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.Add(admissionVolume);
                        cmd.Connection = connection;
                        cmd.Transaction = trans;

                        cmd.ExecuteNonQuery();


                    }
                    tran.Commit();

                    return new AdmissionVolumeViewModel();//model;
                }
                catch (Exception exc)
                {
                    if (tran != null)
                        tran.Rollback();
                    throw;
                }
            });

        }

        public static AdmissionVolumeViewModel.DirectionInfo[] GetDirectionsInfo(IQueryable<Direction> query)
        {
            var modelArray = query
                .Select(x => new AdmissionVolumeViewModel.DirectionInfo
                {
                    DirectionCode = x.Code,
                    DirectionID = x.DirectionID,
                    DirectionName = x.Name,
                    QualificationCode = x.QUALIFICATIONCODE,
                    ParentCode = x.ParentDirection.Code,
                    ParentName = x.ParentDirection.Name,
                    NewCode = x.NewCode,
                    EducationLevelName = x.EDULEVEL,
                }).ToArray();
            //foreach (var model in modelArray)
            //{
            //    string qaCode = (model.QualificationCode ?? "").Trim();
            //    short edID = 0;
            //    if (qaCode == "62") edID = EDLevelConst.Bachelor;
            //    if (qaCode == "65") edID = EDLevelConst.Speciality;
            //    if (qaCode == "68") edID = EDLevelConst.Magistracy;
            //    if (qaCode == "51" || qaCode == "52") edID = EDLevelConst.SPO;
            //    if (qaCode == "70") edID = EDLevelConst.HighQualification;
            //    //if (edID == 0)
            //    //    model.EducationLevelName = qaCode;
            //    //else
            //    //    model.EducationLevelName = qaCode;//DictionaryCache.GetName(DictionaryCache.DictionaryTypeEnum.EducationLevel, edID);

            //}
            return modelArray;
        }
    }
}
