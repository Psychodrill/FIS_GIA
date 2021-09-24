using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model;
using System.Data.Entity;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
    public class DbObjectRepository : DbObjectRepositoryBase
    {
        public DbObjectRepository(int institutionID)
            : base(institutionID)
        {
        }

        public override IQueryable<AdmissionVolume> AdmissionVolumes
        {
            get
            {
                return ImportEntities.AdmissionVolume
                    .Include(x => x.Campaign)
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<CompetitiveGroup> CompetitiveGroups
        {
            get
            {
                return ImportEntities.CompetitiveGroup
                    .Include(x => x.Campaign)
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<CompetitiveGroupItem> CompetitiveGroupItems
        {
            get
            {
                return ImportEntities.CompetitiveGroupItem
                    .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<CompetitiveGroupTarget> CompetitiveGroupTargets
        {
            get
            {
                return ImportEntities.CompetitiveGroupTarget
                    .Include(x => x.CompetitiveGroupTargetItem)
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<CompetitiveGroupTargetItem> CompetitiveGroupTargetItems
        {
            get
            {
                return ImportEntities.CompetitiveGroupTargetItem
                    .Where(x => x.CompetitiveGroupTarget.InstitutionID == InstitutionId);
            }
        }

        public override IEnumerable<Campaign> Campaigns
        {
            get
            {
                return ImportEntities.Campaign
                    .Include(x => x.CampaignDate)
                    .Include(x => x.CampaignEducationLevel)
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<BenefitItemC> CompetitiveGroupBenefitItemsForEntranceTest
        {
            get
            {
                return ImportEntities.BenefitItemC
                    .Include(x => x.BenefitItemCOlympicType)
                    .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId && x.EntranceTestItemID.HasValue);
            }
        }

        public override IQueryable<BenefitItemC> CompetitiveGroupCommonBenefitItems
        {
            get
            {
                return ImportEntities.BenefitItemC
                    .Include(x => x.BenefitItemCOlympicType)
                    .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId && !x.EntranceTestItemID.HasValue);
            }
        }

        public override IEnumerable<EntranceTestItemC> CompetitiveGroupEntranceTestItems
        {
            get
            {
                return ImportEntities.EntranceTestItemC
                    .Include(x => x.CompetitiveGroup)
                    .Where(x => x.CompetitiveGroup.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<ApplicationShortRef> Applications
        {
            get
            {
                return ImportEntities.Application
                    .Where(c => c.InstitutionID == InstitutionId)
                    .Select(c => new ApplicationShortRef
                        {
                            ApplicationNumber = c.ApplicationNumber,
                            RegistrationDateDate = c.RegistrationDate,
                            OrderOfAdmissionId = c.OrderOfAdmissionID,
                            OriginalDocumentsReceivedDate = c.OriginalDocumentsReceivedDate,
                            UID = c.UID
                        });
            }
        }

        public override IQueryable<ApplicationEntranceTestDocumentShortRef> ApplicationEntranceTestResults
        {
            get
            {
                return ImportEntities.ApplicationEntranceTestDocument
                    .Where(x =>
                            x.Application.InstitutionID == InstitutionId &&
                            x.EntranceTestItemID != null &&
                            x.Application.UID != null &&
                            x.UID != null)
                    .Select(c => new ApplicationEntranceTestDocumentShortRef
                        {
                            ApplicationID = c.ApplicationID,
                            ApplicationUID = c.Application.UID,
                            UID = c.UID
                        });
            }
        }

        public override IQueryable<ApplicationEntranceTestDocument> ApplicationEntranceTestBenefits
        {
            get
            {
                return ImportEntities.ApplicationEntranceTestDocument
                    .Include(x => x.Application) //ради application uid
                    .Include(x => x.CompetitiveGroup) //ради competitiveGroup uid
                    .Where(x => x.Application.InstitutionID == InstitutionId && x.EntranceTestItemID == null);
            }
        }

        public override IQueryable<EntrantDocument> EntrantDocuments
        {
            get
            {
                return ImportEntities.EntrantDocument
                    .Where(x => x.Entrant.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<OrderOfAdmission> OrdersOfAdmission
        {
            get
            {
                return ImportEntities.OrderOfAdmission
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<AllowedDirections> AllowedDirections
        {
            get
            {
                return ImportEntities.AllowedDirections
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IQueryable<EntranceTestProfileDirection> EntranceTestProfileDirections
        {
            get
            {
                return ImportEntities.EntranceTestProfileDirection
                    .Where(x => x.InstitutionID == InstitutionId);
            }
        }

        public override IEnumerable<DirectionSubjectLinkDirection> DirectionSubjectLinkDirections { get { return DbStableObjectRepository.DirectionSubjectLinkDirections; } }

        public override IEnumerable<DirectionSubjectLink> DirectionSubjectLinks { get { return DbStableObjectRepository.DirectionSubjectLinks; } }

        public override IEnumerable<DirectionSubjectLinkSubject> DirectionSubjectLinkSubjects { get { return DbStableObjectRepository.DirectionSubjectLinkSubjects; } }

        public override IEnumerable<EntranceTestCreativeDirection> EntranceTestCreativeDirections { get { return DbStableObjectRepository.EntranceTestCreativeDirections; } }

        public override IEnumerable<OlympicTypeSubjectLink> OlympicTypeSubjectLinks { get { return DbStableObjectRepository.OlympicTypeSubjectLinks; } }

        public override CompetitiveGroup GetCompetitiveGroupDictById(int id)
        {
            return CompetitiveGroups.FirstOrDefault(obj => obj.CompetitiveGroupID == id);
        }

        public override T GetObject<T>(string uid)
        {
            T result = null;
            Type type = typeof(T);
            if (type == typeof(AdmissionVolume))
            {
                AdmissionVolume typedResult = AdmissionVolumes.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(Campaign))
            {
                Campaign typedResult = Campaigns.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(CompetitiveGroup))
            {
                CompetitiveGroup typedResult = CompetitiveGroups.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(CompetitiveGroupItem))
            {
                CompetitiveGroupItem typedResult = CompetitiveGroupItems.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(CompetitiveGroupTarget))
            {
                CompetitiveGroupTarget typedResult = CompetitiveGroupTargets.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(CompetitiveGroupTargetItem))
            {
                CompetitiveGroupTargetItem typedResult = CompetitiveGroupTargetItems.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(BenefitItemC))
            {
                BenefitItemC typedResult = CompetitiveGroupBenefitItemsForEntranceTest.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(EntranceTestItemC))
            {
                EntranceTestItemC typedResult = CompetitiveGroupEntranceTestItems.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else if (type == typeof(OrderOfAdmission))
            {
                OrderOfAdmission typedResult = OrdersOfAdmission.FirstOrDefault(obj => obj.UID == uid);
                if (typedResult != null)
                {
                    result = (T)(IObjectWithUID)typedResult;
                }
            }
            else
                throw new Exception("Неизвестный тип: " + type.Name);


            if (result == null)
                return default(T);
            return result;
        }

        public override string GetParentUID<T>(string uid)
        {
            string result = null;
            Type type = typeof(T);

            if (type == typeof(CompetitiveGroupTargetItem))
            {
                var typedResult = CompetitiveGroupTargetItems.Where(obj => obj.UID == uid).Select(obj => new { ParentUID = obj.CompetitiveGroupTarget.UID }).FirstOrDefault();
                if (typedResult != null)
                {
                    result = typedResult.ParentUID;
                }
            }
            else if (type == typeof(CompetitiveGroupItem))
            {
                var typedResult = CompetitiveGroupItems.Where(obj => obj.UID == uid).Select(obj => new { ParentUID = obj.CompetitiveGroup.UID }).FirstOrDefault();
                if (typedResult != null)
                {
                    result = typedResult.ParentUID;
                }
            }
            else if (type == typeof(BenefitItemC))
            {
                var typedResult = CompetitiveGroupBenefitItemsForEntranceTest.Where(obj => obj.UID == uid).Select(obj => new { ParentUID = obj.EntranceTestItemC.UID }).FirstOrDefault();
                if (typedResult != null)
                {
                    result = typedResult.ParentUID;
                }
            }
            else if (type == typeof(EntranceTestItemC))
            {
                var typedResult = CompetitiveGroupEntranceTestItems.Where(obj => obj.UID == uid).Select(obj => new { ParentUID = obj.CompetitiveGroup.UID }).FirstOrDefault();
                if (typedResult != null)
                {
                    result = typedResult.ParentUID;
                }
            }
            else if (type == typeof(ApplicationEntranceTestResult))
            {
                var typedResult = ApplicationEntranceTestResults.Where(obj => obj.UID == uid).Select(obj => new { ParentUID = obj.ApplicationUID }).FirstOrDefault();
                if (typedResult != null)
                {
                    result = typedResult.ParentUID;
                }
            }
            else if (type == typeof(ApplicationEntranceTestBenefit))
            {
                var typedResult = ApplicationEntranceTestBenefits.Where(obj => obj.UID == uid).Select(obj => new { ParentUID = obj.Application.UID }).FirstOrDefault();
                if (typedResult != null)
                {
                    result = typedResult.ParentUID;
                }
            }
            else
                throw new Exception("Неизвестный тип: " + type.Name);

            return result;
        }

        public override void LoadData()
        {
            DbStableObjectRepository.Load(ImportEntities);
        }
    }
}
