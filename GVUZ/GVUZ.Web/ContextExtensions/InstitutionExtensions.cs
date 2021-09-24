using System;
using System.Data.Entity;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels;
using System.Collections.Generic;
using GVUZ.Model.Entrants;

namespace GVUZ.Web.ContextExtensions
{
    /// <summary>
    /// Методы для работы с институтом
    /// </summary>
    public static class InstitutionExtensions
    {
        /// <summary>
        /// Общая информация об институте
        /// </summary>
        public static InstituteCommonInfoViewModel LoadInstitutionCommonInfoVM(this InstitutionsEntities context,
            int institutionID, bool isEdit)
        {
            return new InstituteCommonInfoViewModel(context.LoadInstitution(institutionID), isEdit);
        }

        /// <summary>
        /// Начальная загрузка фильтров для списка институтов
        /// </summary>
        public static InstitutionListViewModel InitialFillInstitutionListViewModel(this InstitutionsEntities dbContext, int currentInstitutionID)
        {
            InstitutionListViewModel model = new InstitutionListViewModel();
            model.CurrentInstitutionID = currentInstitutionID;
            var regions = dbContext.RegionType.Select(x => new { ID = x.RegionId, Name = x.Name }).ToList();
            //regions.Insert(0, new {ID = 0, Name = "[Все]"});
            model.Regions = regions.ToArray();
            var types = dbContext.FormOfLaw.Select(x => new { ID = x.FormOfLawID, Name = x.Name }).ToList();
            types.Insert(0, new { ID = 0, Name = "[Все]" });
            model.FormOfLaws = types.ToArray();
            model.InstitututionTypes = new[] { new { ID = 0, Name = "[Все]" }, new { ID = 1, Name = "ВУЗ" }, new { ID = 2, Name = "ССУЗ" } };
            model.OwnerDepartments = dbContext.Institution
                .Where(x => x.OwnerDepartment != null)
                .Select(x => x.OwnerDepartment).ToArray();
            return model;
        }

        public static RequestListViewModel InitialFillRequestListViewModel(this InstitutionsEntities dbContext, int currentInstitutionID)
        {
            RequestListViewModel model = new RequestListViewModel();
            model.CurrentInstitutionID = currentInstitutionID;
            return model;
        }

        public static readonly int InstitutionListPageSize = AppSettings.Get("Search.PageSize", 25);
        public static AjaxResultModel FillRequestList(this InstitutionsEntities dbContext, int InstID)
        {
            var query = dbContext.RequestDirection.Where(x => x.Request_ID == InstID && x.Activity == "W").Select(x => x.Direction_ID);

            var current = dbContext.Direction.Where(x => query.Contains(x.DirectionID)).OrderBy(x => x.DirectionID)
					.Select(x => new { ID = x.DirectionID, Code = x.Code, Name = x.Name, Period = x.PERIOD, x.QUALIFICATIONCODE, NewCode = x.NewCode}).ToArray()
					.Select(x => new { ID = x.ID, Code = x.Code, NewCode = x.NewCode, QualificationCode = x.QUALIFICATIONCODE, Name = x.Name, Period = (x.Period ?? "").Trim() }).ToArray();;

            var query1 = dbContext.RequestComments.Include(x => x.RequestDirection).Where(x => x.InstitutionID == InstID && x.Commentor == "U").OrderByDescending(x => x.Date);

            List<GVUZ.Model.Institutions.RequestComments> rlist = new List<GVUZ.Model.Institutions.RequestComments>();

            foreach (GVUZ.Model.Institutions.RequestComments rq in query1)
            {
                if (!rlist.Select(x => x.DirectionID).Contains(rq.DirectionID))
                {
                    rlist.Add(rq);
                }
            }

            foreach (var rd in dbContext.RequestDirection.Where(x => x.Request_ID == InstID && x.Activity == "W"))
            {
                rd.Activity = "A";
            }
            dbContext.SaveChanges();
            new RequestThread(InstID);
            return new AjaxResultModel { Data = new { Request = current, Comment = rlist.OrderBy(x => x.DirectionID).Select(x => 
                new { DirectionID = x.DirectionID, Comment = x.Comment, Action = x.RequestDirection.Action, admissionType = x.RequestDirection.AdmissionItemType}) } };
        }


        /// <summary>
        /// Возвращаем институты по фильтру
        /// </summary>
        public static AjaxResultModel GetInstitutionList(this InstitutionsEntities dbContext, InstitutionListViewModel model)
        {
            var query = dbContext.Institution
                .Include(x => x.RegionType)
                .Include(x => x.FormOfLaw)
                .Include(x => x.UserPolicy);

            //sorting
            if ((model.SortID ?? 0) == 0)
                model.SortID = 1;

            if (model.SortID == 1) query = query.OrderBy(x => x.FullName);
            if (model.SortID == -1) query = query.OrderByDescending(x => x.FullName);
            if (model.SortID == 2) query = query.OrderBy(x => x.InstitutionTypeID);
            if (model.SortID == -2) query = query.OrderByDescending(x => x.InstitutionTypeID);
            if (model.SortID == 3) query = query.OrderBy(x => x.RegionType.Name);
            if (model.SortID == -3) query = query.OrderByDescending(x => x.RegionType.Name);
            if (model.SortID == 4) query = query.OrderBy(x => x.OwnerDepartment);
            if (model.SortID == -4) query = query.OrderByDescending(x => x.OwnerDepartment);
            if (model.SortID == 5) query = query.OrderBy(x => x.UserPolicy.Count());
            if (model.SortID == -5) query = query.OrderByDescending(x => x.UserPolicy.Count());

            model.TotalItemCount = query.Count();

            if (model.Filter != null)
            {
                if (!String.IsNullOrEmpty(model.Filter.ShortName))
                    query = query.Where(x => x.BriefName.Contains(model.Filter.ShortName));
                if (!String.IsNullOrEmpty(model.Filter.FullName))
                    query = query.Where(x => x.FullName.Contains(model.Filter.FullName));
                if (!String.IsNullOrEmpty(model.Filter.Owner))
                    query = query.Where(x => x.OwnerDepartment.Contains(model.Filter.Owner));
                if (model.Filter.InstitutionTypeID > 0)
                    query = query.Where(x => x.InstitutionTypeID == model.Filter.InstitutionTypeID);
                if (model.Filter.FormOfLawID > 0)
                    query = query.Where(x => x.FormOfLawID == model.Filter.FormOfLawID);
                if (model.Filter.RegionID > 0)
                    query = query.Where(x => x.RegionID == model.Filter.RegionID);
                if (!String.IsNullOrEmpty(model.Filter.OGRN))
                    query = query.Where(x => x.OGRN == model.Filter.OGRN);
                if (!String.IsNullOrEmpty(model.Filter.INN))
                    query = query.Where(x => x.OGRN == model.Filter.INN);
            }

            query = query.Where(x => x.StatusId != null && x.StatusId == 1);


            var pageNumber = model.PageNumber;
            if (!pageNumber.HasValue || pageNumber < 0) pageNumber = 0;

            int totalCount = query.Count();
            model.TotalItemFilteredCount = totalCount;
            model.TotalPageCount = ((Math.Max(totalCount, 1) - 1) / InstitutionListPageSize) + 1;

            var filtQuery = query
                .Skip(pageNumber.Value * InstitutionListPageSize)
                .Take(InstitutionListPageSize);

            model.Institutions = filtQuery.ToArray().Select(x => new InstitutionListViewModel.InstitutionData
                                        {
                                            InstitutionID = x.InstitutionID,
                                            ShortName = x.BriefName ?? x.FullName ?? "",
                                            FullName = x.FullName ?? "",
                                            InstitutionTypeName = x.InstitutionTypeID == 1 ? "ВУЗ" : (x.InstitutionTypeID == 2 ? "ССУЗ" : ""),
                                            FormOfLawName = x.FormOfLaw != null ? x.FormOfLaw.Name : "",
                                            Owner = x.OwnerDepartment ?? "",
                                            RegionName = x.RegionType != null ? x.RegionType.Name : "",
                                            UserCount = x.UserPolicy.Count(),
                                        }).ToArray();
            return new AjaxResultModel { Data = model };
        }

        /// <summary>
        /// Возвращаем интституты и их заявки
        /// </summary>
        public static AjaxResultModel GetRequestList(this InstitutionsEntities dbContext, RequestListViewModel model)
        {
            //var query = from request in dbContext.RequestDirection
            //            join institute in dbContext.Institution on request.Request_ID equals institute.InstitutionID
            //            select new { InstId = institute.InstitutionID, /*Date = request.ChangeDate,*/ name = institute.FullName, /*DirId = request.Direction_ID */};
        
            //query = query.Distinct();

            var query2 = dbContext.RequestDirection.Where(x => x.Activity == "W").Include(x => x.Institution);
            List<int> query1 = query2.Select(x => x.Institution.InstitutionID).Distinct().ToList();
            List<GVUZ.Model.Institutions.RequestDirection> queryL = new List<GVUZ.Model.Institutions.RequestDirection>();
            foreach (GVUZ.Model.Institutions.RequestDirection q in query2)
            {
                if (query1.Contains(q.Request_ID))
                {
                    queryL.Add(q);
                    query1.Remove(q.Institution.InstitutionID);
                }
            }
            var query = queryL.AsEnumerable();
            
            //sorting
            if ((model.SortID ?? 0) == 0)
                model.SortID = 1;

            if (model.SortID == 1) query = query.OrderBy(x => x.Institution.FullName);
            if (model.SortID == -1) query = query.OrderByDescending(x => x.Institution.FullName);
            if (model.SortID == 3) query = query.OrderBy(x => x.ChangeDate);
            if (model.SortID == -3) query = query.OrderByDescending(x => x.ChangeDate);
            if (model.SortID == 2) query = query.OrderBy(x => dbContext.RequestDirection.Where(z => z.Institution.InstitutionID == x.Request_ID && z.Activity == "W").Count());
            if (model.SortID == -2) query = query.OrderByDescending(x => dbContext.RequestDirection.Where(z => z.Institution.InstitutionID == x.Request_ID && z.Activity == "W").Count());


            model.TotalItemCount = query.Count();

            /*
            if (model.Filter != null)
            {
                if (!String.IsNullOrEmpty(model.Filter.ShortName))
                    query = query.Where(x => x.BriefName.Contains(model.Filter.ShortName));
                if (!String.IsNullOrEmpty(model.Filter.FullName))
                    query = query.Where(x => x.FullName.Contains(model.Filter.FullName));
                if (!String.IsNullOrEmpty(model.Filter.Owner))
                    query = query.Where(x => x.OwnerDepartment.Contains(model.Filter.Owner));
                if (model.Filter.InstitutionTypeID > 0)
                    query = query.Where(x => x.InstitutionTypeID == model.Filter.InstitutionTypeID);
                if (model.Filter.FormOfLawID > 0)
                    query = query.Where(x => x.FormOfLawID == model.Filter.FormOfLawID);
                if (model.Filter.RegionID > 0)
                    query = query.Where(x => x.RegionID == model.Filter.RegionID);
                if (!String.IsNullOrEmpty(model.Filter.OGRN))
                    query = query.Where(x => x.OGRN == model.Filter.OGRN);
                if (!String.IsNullOrEmpty(model.Filter.INN))
                    query = query.Where(x => x.OGRN == model.Filter.INN);
            }
            */

            var pageNumber = model.PageNumber;
            if (!pageNumber.HasValue || pageNumber < 0) pageNumber = 0;

            int totalCount = query.Count();
            model.TotalItemFilteredCount = totalCount;
            model.TotalPageCount = ((Math.Max(totalCount, 1) - 1) / InstitutionListPageSize) + 1;

            var filtQuery = query
                .Skip(pageNumber.Value * InstitutionListPageSize)
                .Take(InstitutionListPageSize);
            
            model.Institutions = filtQuery.ToArray().Select(x => new RequestListViewModel.InstitutionData
            {
                InstitutionID = x.Request_ID,
                FullName = x.Institution.FullName,
                RequestNumber = dbContext.RequestDirection.Where(z => z.Institution.InstitutionID == x.Request_ID && z.Activity == "W").Count(),
                Date = x.ChangeDate.ToString()

            }).ToArray();
            return new AjaxResultModel { Data = model };
        }

        /// <summary>
        /// Добавляем историю изменений вуза
        /// </summary>
        public static void AddChangesToHistory(this InstitutionsEntities dbContext, int institutionID)
        {
            var inst = dbContext.LoadInstitution(institutionID);
            if (inst != null)
            {
                InstitutionHistory hist = new InstitutionHistory
                                          {
                                              InstitutionTypeID = inst.InstitutionTypeID,
                                              InstitutionID = inst.InstitutionID,
                                              FullName = inst.FullName,
                                              BriefName = inst.BriefName,
                                              FormOfLawID = inst.FormOfLawID,
                                              RegionID = inst.RegionID,
                                              Site = inst.Site,
                                              Address = inst.Address,
                                              Phone = inst.Phone,
                                              Fax = inst.Fax,
                                              HasMilitaryDepartment = inst.HasMilitaryDepartment,
                                              HasHostel = inst.HasHostel,
                                              HostelCapacity = inst.HostelCapacity,
                                              HasHostelForEntrants = inst.HasHostelForEntrants,
                                              HostelAttachmentID = inst.HostelAttachmentID,
                                              INN = inst.INN,
                                              OGRN = inst.OGRN,
                                              AdmissionStructurePublishDate = inst.AdmissionStructurePublishDate,
                                              ReceivingApplicationDate = inst.ReceivingApplicationDate,
                                              EsrpOrgID = inst.EsrpOrgID,
                                              OwnerDepartment = inst.OwnerDepartment
                                          };
                if (inst.InstitutionAccreditation.Count > 0)
                {
                    hist.Accreditation = inst.InstitutionAccreditation.First().Accreditation;
                    hist.AccreditationAttachmentID = inst.InstitutionAccreditation.First().AttachmentID;
                }

                if (inst.InstitutionLicense.Count > 0)
                {
                    hist.LicenseNumber = inst.InstitutionLicense.First().LicenseNumber;
                    hist.LicenseDate = inst.InstitutionLicense.First().LicenseDate;
                    hist.LicenseAttachmentID = inst.InstitutionLicense.First().AttachmentID;
                }

                hist.CreatedDate = DateTime.Now;
                dbContext.InstitutionHistory.AddObject(hist);
                dbContext.SaveChanges();
            }
        }

        public class HistoryChangeInfo
        {
            public int HistoryID { get; set; }
            public DateTime DateChanged { get; set; }
        }


        /// <summary>
        /// Загружаем историю изменений вуза
        /// </summary>
        public static HistoryChangeInfo[] GetHistoriesForInstitution(this InstitutionsEntities dbContext, int institutionID)
        {
            return dbContext.InstitutionHistory.Where(x => x.InstitutionID == institutionID)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new HistoryChangeInfo { HistoryID = x.InstitutionHistoryID, DateChanged = x.CreatedDate })
                .ToArray();
        }


        public static AjaxResultModel DenyRequestAdmin(this InstitutionsEntities dbContext, int DirectionID, int InstitutionID, string Comment)
        {
            if (DirectionID * InstitutionID == 0) return new AjaxResultModel("Data sending error. Try again later.");
            GVUZ.Model.Institutions.RequestDirection rq = dbContext.RequestDirection.Where(x => x.Direction_ID == DirectionID && x.Request_ID == InstitutionID).Single();
            rq.Activity = "D";

            GVUZ.Model.Institutions.RequestComments rc = Model.Institutions.RequestComments.CreateRequestComments(dbContext.RequestComments.Count() + 1, "A", DirectionID, InstitutionID);
            rc.Comment = Comment;
            dbContext.AddToRequestComments(rc);

            dbContext.SaveChanges();

            return new AjaxResultModel();
        }


        public static AjaxResultModel AcceptRequest(this InstitutionsEntities dbContext, int DirID, int InstId, int adType)
        {
            var rd = dbContext.RequestDirection.SingleOrDefault(x => x.Direction_ID == DirID && x.Request_ID == InstId && x.AdmissionItemType == adType);
            if (rd == null)
                return new AjaxResultModel();

            using (var dbContextE = new EntrantsEntities())
            {
                if (rd.Action == "Add")
                {
                    var ad = new Model.Entrants.AllowedDirections();
                    ad.Institution = dbContextE.Institution.SingleOrDefault(x => x.InstitutionID == InstId);
                    ad.Direction = dbContextE.Direction.SingleOrDefault(x => x.DirectionID == DirID);
                    ad.AdmissionItemTypeID = (short)adType;
                    if (!dbContextE.AllowedDirections.Any(x => x.AdmissionItemTypeID == ad.AdmissionItemTypeID
                                                            && x.DirectionID == ad.Direction.DirectionID 
                                                            && x.InstitutionID == ad.Institution.InstitutionID))
                        dbContextE.AllowedDirections.AddObject(ad);
                }

                if (rd.Action == "Delete")
                {
                    var ad = dbContextE.AllowedDirections.FirstOrDefault(x => x.InstitutionID == InstId && 
                        x.AdmissionItemTypeID == (short)adType && x.DirectionID == DirID) ?? new GVUZ.Model.Entrants.AllowedDirections();
                    dbContextE.AllowedDirections.DeleteObject(ad);
                }
                dbContextE.SaveChanges();
            }
            dbContext.RequestDirection.DeleteObject(rd);
            dbContext.SaveChanges();

            return new AjaxResultModel();
        }

        public static AjaxResultModel AtoU(this InstitutionsEntities dbContext, int instid)
        {
            foreach(var rd in dbContext.RequestDirection.Where(x => x.Activity == "A" && x.Request_ID == instid))
            {
                rd.Activity = "W";
            }
            dbContext.SaveChanges();
            return new AjaxResultModel();
        }

        class RequestThread
        {
            System.Threading.Thread thread;
            InstitutionsEntities dbContext;

            public RequestThread(int instID) 
            {
                thread = new System.Threading.Thread(this.func);
                thread.Start(instID);
                dbContext = new InstitutionsEntities();
            }

            void func(object instID)
            {
                int id = Convert.ToInt32(instID);
                System.Threading.Thread.Sleep(900000);
                foreach (var rd in dbContext.RequestDirection.Where(x => x.Activity == "A" && x.Request_ID == id))
                {
                    rd.Activity = "W";
                }
                dbContext.SaveChanges();
            }
        }
    }
}