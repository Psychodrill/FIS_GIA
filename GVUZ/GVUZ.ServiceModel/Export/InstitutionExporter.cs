using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.Model.Courses;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.ContextExtensions;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;
using Address = GVUZ.Model.Entrants.Address;
using AdmissionVolume = GVUZ.Model.Entrants.AdmissionVolume;
using Application = GVUZ.Model.Entrants.Application;
using ApplicationEntranceTestDocument = GVUZ.Model.Entrants.ApplicationEntranceTestDocument;
using ApplicationEntrantDocument = GVUZ.Model.Entrants.ApplicationEntrantDocument;
using CompetitiveGroup = GVUZ.Model.Entrants.CompetitiveGroup;
using CompetitiveGroupItem = GVUZ.Model.Entrants.CompetitiveGroupItem;
using Entrant = GVUZ.Model.Entrants.Entrant;
using EntrantDocument = GVUZ.Model.Entrants.EntrantDocument;
using Institution = GVUZ.Model.Institutions.Institution;
using System.Data.Objects.SqlClient;

namespace GVUZ.ServiceModel.Export
{
	public class InstitutionExporter
	{
		static InstitutionExporter()
		{
            Mapper.CreateMap<Institution, InstitutionDetailsDto>();
            Mapper.CreateMap<AdmissionVolume, AdmissionVolumeDto>()
                .ForMember(x => x.EducationLevelID, src => src.MapFrom(y => y.AdmissionItemTypeID))
                .ForMember(x => x.CampaignUID, src => src.Ignore())
                .ForMember(x => x.Course, src => src.MapFrom(y => y.Course));
            Mapper.CreateMap<CompetitiveGroup, CompetitiveGroupDto>();
            Mapper.CreateMap<CompetitiveGroupItem, CompetitiveGroupItemDto>();
            Mapper.CreateMap<Entrant, EntrantDto>();
            Mapper.CreateMap<Address, AddressDto>();
            Mapper.CreateMap<OlympicDocumentViewModel, OlympicDocumentDto>()
                .ForMember(x => x.Subjects, y => y.Ignore())
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<OlympicTotalDocumentViewModel, OlympicTotalDocumentDto>()
                .ForMember(x => x.Subjects, y => y.Ignore())
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<OlympicTotalDocumentDto, OlympicTotalDocumentViewModel>()
                .ForMember(x => x.Subjects, opt =>
                    opt.MapFrom(y => y.Subjects.Select(x => new OlympicTotalDocumentViewModel.SubjectBriefData
                    {
                        SubjectID = x.SubjectID.To(0, null, null, false)
                    }).ToArray()));
            Mapper.CreateMap<SubjectBriefDataDto, OlympicTotalDocumentViewModel.SubjectBriefData>();
            Mapper.CreateMap<OlympicTotalDocumentViewModel.SubjectBriefData, SubjectBriefDataDto>();

            Mapper.CreateMap<PsychoDocumentViewModel, ApplicationDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<PsychoDocumentViewModel, AllowEducationDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<MilitaryCardDocumentViewModel, MilitaryCardDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<StudentDocumentViewModel, StudentDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<CustomDocumentViewModel, CustomDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<DisabilityDocumentViewModel, DisabilityDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<BasicDiplomaDocumentViewModel, BasicDiplomaDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()))
                .ForMember(x => x.QualificationTypeID, y => y.Ignore())
                .ForMember(x => x.ProfessionID, y => y.Ignore());
            Mapper.CreateMap<DiplomaDocumentViewModel, HighEduDiplomaDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()))
                .ForMember(x => x.QualificationTypeID, y => y.Ignore())
                .ForMember(x => x.ProfessionID, y => y.Ignore())
                .ForMember(x => x.SpecialityID, y => y.Ignore())
                .ForMember(x => x.SpecializationID, y => y.Ignore());

            Mapper.CreateMap<DiplomaDocumentViewModel, MiddleEduDiplomaDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()))
                .ForMember(x => x.QualificationTypeID, y => y.Ignore())
                .ForMember(x => x.ProfessionID, y => y.Ignore())
                .ForMember(x => x.SpecialityID, y => y.Ignore())
                .ForMember(x => x.SpecializationID, y => y.Ignore());

            Mapper.CreateMap<DiplomaDocumentViewModel, IncomplHighEduDiplomaDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()))
                .ForMember(x => x.QualificationTypeID, y => y.Ignore())
                .ForMember(x => x.ProfessionID, y => y.Ignore())
                .ForMember(x => x.SpecialityID, y => y.Ignore())
                .ForMember(x => x.SpecializationID, y => y.Ignore());

            Mapper.CreateMap<DiplomaDocumentViewModel, AcademicDiplomaDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()))
                .ForMember(x => x.QualificationTypeID, y => y.Ignore())
                .ForMember(x => x.ProfessionID, y => y.Ignore())
                .ForMember(x => x.SpecialityID, y => y.Ignore())
                .ForMember(x => x.SpecializationID, y => y.Ignore());

            Mapper.CreateMap<EduCustomDocumentViewModel, EduCustomDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()))
                .ForMember(x => x.DocumentTypeNameText, y => y.Ignore());

            Mapper.CreateMap<SchoolCertificateDocumentViewModel, SchoolCertificateDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<IdentityDocumentViewModel, IdentityDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<EGEDocumentViewModel, EgeDocumentWithSubjectsDto>()
                .ForMember(x => x.Subjects, y => y.Ignore())
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            Mapper.CreateMap<GiaDocumentViewModel, GiaDocumentWithSubjectsDto>()
                .ForMember(x => x.Subjects, y => y.Ignore())
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));

            Mapper.CreateMap<SchoolCertificateDocumentViewModel, SchoolCertificateBasicDocumentDto>()
                .ForMember(x => x.DocumentDate, y => y.MapFrom(z => z.DocumentDate.GetNullableDateAsString()));
            
		}

        public static XElement GetInsitututionsData(int institutionID, bool bIncludeFilials)
        {
            return GetInsitututionsData(institutionID, new InstitutionInformationFilter(), bIncludeFilials);
        }

        // Формирование основного объекта InstitutionExportDto в рутовом списке InstitutionExports
        static InstitutionExportDto GenerateInstitutionExport(int institutionID, InstitutionInformationFilter filter)
        {
            var dto = new InstitutionExportDto();
            using (var dbContext = new InstitutionsEntities())
            {
                if (filter.IsEmpty || filter.InstitutionDetails != null)
                    dto.InstitutionDetails = GenerateInstitutionDetails(institutionID, dbContext);

                if (filter.IsEmpty || filter.Structure != null)
                    dto.Structure = GenerateInstitutionStructure(institutionID, dbContext);
            }

            using (var dbContext = new EntrantsEntities())
            {
                if (filter.IsEmpty || filter.AdmissionVolume != null)
                    dto.AdmissionVolume = GenerateAdmissionVolume(institutionID, dbContext, filter);

                if (filter.IsEmpty || filter.CompetitiveGroups != null)
                    dto.CompetitiveGroups = GenerateCompetitiveGroups(institutionID, dbContext, filter);

                if (filter.Applications != null)
                    dto.Applications = GenerateApplications(institutionID, dbContext, /*userLogin,*/ filter);

                if (!filter.IsEmpty && filter.AllowedDirections != null)
                    dto.AllowedDirections = GenerateAllowedDirections(institutionID, dbContext);
            }

            using (var dbContext = new CoursesEntities())
            {
                if (filter.IsEmpty)
                    dto.PreparatoryCourses = GeneratePreparatoryCourses(institutionID, dbContext);
            }

            using (var dbContext = new ImportEntities())
            {
                if (filter.IsEmpty || filter.Campaigns != null)
                    dto.Campaigns = GenerateCampaigns(institutionID, dbContext, filter);

                if (filter.IsEmpty || filter.OrdersOfAdmission != null)
                    dto.OrdersOfAdmission = GenerateOrderOfAdmissions(institutionID, dbContext, filter);

                if (filter.IsEmpty || filter.RecommendedLists != null)
                    dto.RecommendedLists = GenerateRecommendedLists(institutionID, dbContext);
            }

            return dto;
        }

        private static RecommendedListDto[] GenerateRecommendedLists(int institutionID, ImportEntities dbContext)
        {
            var result = new List<RecommendedListDto>();

            var dbData = dbContext.RecomendedLists.Where(x => x.InstitutionID == institutionID && x.RecomendedListsHistory.Any(y => !y.DateDelete.HasValue));
            var stage1DbData = dbData.Where(x => x.Stage == 1);
            var stage2DbData = dbData.Where(x => x.Stage == 2);

            RecommendedListDto stage1List = GenerateStageRecommendedListDto(stage1DbData, 1);
            RecommendedListDto stage2List = GenerateStageRecommendedListDto(stage2DbData, 2);

            result.Add(stage1List);
            result.Add(stage2List);

            return result.ToArray();
        }

        private static RecommendedListDto GenerateStageRecommendedListDto(IQueryable<GVUZ.ServiceModel.Import.RecomendedLists> stageData, int stage)
        {
            RecommendedListDto item = new RecommendedListDto() { Stage = stage };

            var apps = stageData.Select(x => new { ApplicationNumber = x.Application.ApplicationNumber, RegistrationDate = x.Application.RegistrationDate, ApplicationId = x.ApplicationID}).Distinct().ToArray();
            var itemRecLists = new List<RecListDto>();
            foreach (var app in apps)
            {
                var data = stageData.Where(x => x.ApplicationID == app.ApplicationId).ToArray();
                RecListDto listElement = new RecListDto();

                RecListApplicationDto application = new RecListApplicationDto()
                {
                    ApplicationNumber = app.ApplicationNumber,
                    RegistrationDate = app.RegistrationDate
                };

                listElement.Application = application;
                List<FinSourceAndEduFormDto> sources = new List<FinSourceAndEduFormDto>();

                foreach (var dataItem in data)
                {
                    FinSourceAndEduFormDto recFinSource = new FinSourceAndEduFormDto()
                    {
                        EducationLevelID = dataItem.EduLevelID,
                        EducationFormID = dataItem.EduFormID,
                        DirectionID = dataItem.DirectionID,
                        CompetitiveGroupID = dataItem.CompetitiveGroup.UID
                    };
                    sources.Add(recFinSource);
                }
                listElement.FinSourceAndEduForms = sources.ToArray();

                itemRecLists.Add(listElement);
            }
            item.RecLists = itemRecLists.ToArray();

            return item; 
        }

	    public static XElement GetInsitututionsData(int institutionID, InstitutionInformationFilter filter, bool bIncludeFilials)
		{            
            var iexport_list  = new List<InstitutionExportDto>();
            InstitutionExportDto filial;
            InstitutionExportDto mainInst = GenerateInstitutionExport(institutionID, filter);
            iexport_list.Add(mainInst);
            if (bIncludeFilials)
            {
                int[] fids = GetInstitutionFilials(institutionID);
                foreach (int fid in fids)
                {
                    filial = GenerateInstitutionExport(fid, filter);
                    iexport_list.Add(filial);
                }                
            }
         
            InstitutionExportDto[] arr = iexport_list.ToArray();

            var xmlSerializer = new XmlSerializer(typeof(InstitutionExportDto[]), new XmlRootAttribute("InstitutionExports"));
			var doc = new XDocument();
            using (var xw = doc.CreateWriter())
            {
                xmlSerializer.Serialize(xw, arr);
            }

            // удалить все Id из выходного xml 
            if (doc.Root != null)
                doc.Root.Descendants().Elements("Id").ToList().ForEach(x => x.Remove());                      

	        return doc.Root;
		}

        public static int[] GetInstitutionFilials(int institutionID)
        {
            int[] filials = new int[0];

            using (InstitutionsEntities context = new InstitutionsEntities())
            {
                int? esrpOrgID = context.Institution.Where(x => x.InstitutionID == institutionID).Select(y => y.EsrpOrgID).FirstOrDefault();
                if ((esrpOrgID != null) && (esrpOrgID != 0))
                {
                    filials = context.Institution.Where(x => x.MainEsrpOrgId == esrpOrgID).Select(x => x.InstitutionID).ToArray();
                }
            }
            return filials;
        }


		private static InstitutionDetailsDto GenerateInstitutionDetails(int institutionID, InstitutionsEntities dbContext)
		{
			var institution = dbContext.Institution.FirstOrDefault(x => x.InstitutionID == institutionID);

			if (institution == null)
				throw new ArgumentNullException("institutionID", "Invalid institution " + institutionID);

			var institutionLicense = dbContext.InstitutionLicense.FirstOrDefault(x => x.InstitutionID == institutionID);
			var institutionAccreditation = dbContext.InstitutionAccreditation.FirstOrDefault(x => x.InstitutionID == institutionID);
            

			InstitutionDetailsDto dto = Mapper.Map(institution, new InstitutionDetailsDto());

            if ((institution.MainEsrpOrgId != null) && (institution.MainEsrpOrgId != 0))
             dto.IsFilial = true;
            else
             dto.IsFilial = false;            
            
			if (institutionLicense != null)
			{
				dto.LicenseDate = institutionLicense.LicenseDate;
				dto.LicenseNumber = institutionLicense.LicenseNumber;
			}

			if (institutionAccreditation != null)
				dto.Accreditation = institutionAccreditation.Accreditation;
			return dto;
		}

		private static PreparatoryCourseDto[] GeneratePreparatoryCourses(int institutionID, CoursesEntities dbContext)
		{
			return dbContext.PreparatoryCourse
				.Include(x => x.CourseSubject)
				.Where(x => x.InstitutionID == institutionID)
				.ToArray()
				.Select(x => new PreparatoryCourseDto
				             {
				             	CourseName = x.CourseName,
				             	Information = x.Information,
				             	Subjects = x.CourseSubject.Select(y => new EntranceTestSubjectDto
				             	                                       {
				             	                                       	SubjectID =
				             	                                       		y.SubjectID != null ? y.SubjectID.ToString() : null,
				             	                                       	SubjectName = y.SubjectName
				             	                                       }).ToArray()
				             }).ToArray();
		}

		private static InstitutionStructureDto GenerateInstitutionStructure(int institutionID, InstitutionsEntities dbContext)
		{
			var itemsStructure = dbContext.InstitutionStructure
						.Include(x => x.InstitutionItem)
						.Include(x => x.Children)
						.Where(x => x.InstitutionItem.InstitutionID == institutionID)
						.OrderBy(x => x.InstitutionItem.Name)
						.ToArray();
			InstitutionStructureDto root = new InstitutionStructureDto();
			// Roman.N.Bukin отключена выгрузка сведений о кафедрах и факультетах решением от 01.04 по задаче FIS-38
            // GenerateSubStructureDto(root, itemsStructure);

            GenerateFilialStructure(root, institutionID, dbContext);

            root.ItemID = null;
			return root;
		}

        private static void GenerateFilialStructure(InstitutionStructureDto root, int? institutionID, InstitutionsEntities dbContext)
        {
            if (institutionID == null)
                return;

            var currentEsrpOrgID = dbContext.Institution.Where(i => i.InstitutionID == institutionID).Select(t=> t.EsrpOrgID).FirstOrDefault();
            if (currentEsrpOrgID == null)
                return;

            var filialStructure = dbContext.Institution.Where(i => i.MainEsrpOrgId == currentEsrpOrgID).Select(t => new InstitutionStructureDto
                                                                                        {
                                                                                            ItemID = t.InstitutionID,
                                                                                            InstitutionID = SqlFunctions.StringConvert((double)t.InstitutionID).Trim(),
                                                                                            Name = t.FullName //,
                                                                                            //BriefName = "", //t.BriefName,
                                                                                            //Site = "" //t.Site
                                                                                        });
            if (root.ChildStructure == null && filialStructure.Count() > 0)
                root.ChildStructure = new InstitutionStructureDto[]{};

            foreach (var filial in filialStructure)
            {
                GenerateFilialStructure(filial, filial.ItemID, dbContext);

                var list = root.ChildStructure.ToList();
                list.Add(filial);
                root.ChildStructure = list.ToArray();
            }
        }

		private static void GenerateSubStructureDto(InstitutionStructureDto dto, ICollection<InstitutionStructure> structure)
		{
			dto.ChildStructure = structure.Where(x => x.ParentID == dto.ItemID || (x.ParentID == null && dto.ItemID == 0)).Select(x => new InstitutionStructureDto
			                                                                            {
			                                                                            	ItemID = x.InstitutionStructureID,
                                                                                            Name = x.InstitutionItem.Name == "Нет" ? "" : x.InstitutionItem.Name,
                                                                                            BriefName = x.InstitutionItem.BriefName == "Нет" ? "" : x.InstitutionItem.BriefName,
																							Site = x.InstitutionItem.Site,
																							DirectionID = x.InstitutionItem.DirectionID != null ? x.InstitutionItem.DirectionID.ToString() : null,
			                                                                            }).ToArray();
			foreach (var institutionStructureDto in dto.ChildStructure)
			{
				GenerateSubStructureDto(institutionStructureDto, structure);
			}

			if (dto.ChildStructure.Length == 0)
				dto.ChildStructure = null;
		}



		private static AdmissionVolumeCollectionDto GenerateAdmissionVolume(int institutionID, EntrantsEntities dbContext, InstitutionInformationFilter filter)
		{
			var allowedDirs = dbContext.AllowedDirections.Where(x => x.InstitutionID == institutionID);
            
            var campaigns = dbContext.Campaign
                .Include(x => x.CampaignEducationLevel)
                .Where(x => x.InstitutionID == institutionID);
            // .Include(x => x.CampaignDate)

            if (!filter.IsEmpty && filter.AdmissionVolume != null)
            {
                var campaignUids = new HashSet<string>();
                campaignUids.UnionWith(filter.AdmissionVolume.Where(c => !string.IsNullOrEmpty(c)).Select(c => c));

                if (campaignUids.Count > 0)
                    campaigns = campaigns.WhereIn(c => c.UID, campaignUids, chunkSize: 500).AsQueryable();
            }

			return new AdmissionVolumeCollectionDto
			{
				Collection = dbContext.AdmissionVolume
					.Include(x => x.Campaign)
				    .Where(x => x.InstitutionID == institutionID)
				    .ToList()
					.Where(x => allowedDirs.Any(y => y.DirectionID == x.DirectionID && y.AdmissionItemTypeID == x.AdmissionItemTypeID))
                    .Where(x => campaigns.Select(y => y.CampaignID).Contains(x.Campaign.CampaignID))
				    .Select(x =>
				           	{
				           		var r = Mapper.Map(x, new AdmissionVolumeDto());
				           		r.CampaignUID = x.Campaign.UID;
				           		return r;
				           	})
				    .ToArray()
			};
		}

        private static AllowedDirectionsDto[] GenerateAllowedDirections(int institutionID, EntrantsEntities dbContext)
        {
            return (from c in dbContext.AllowedDirections
                    where c.InstitutionID == institutionID
                    select new AllowedDirectionsDto
                               {
                                   DirectionId = c.DirectionID,
                                   Name = c.Direction.Name,
                                   AdmissionItemTypeID = c.AdmissionItemTypeID
                               }).ToArray();
        }

		private static CompetitiveGroupDto[] GenerateCompetitiveGroups(int institutionID, EntrantsEntities dbContext1, InstitutionInformationFilter filter)
		{
			var cgCgroups = dbContext1.CompetitiveGroup
				.Include(x => x.CompetitiveGroupItem)
				.Where(x => x.InstitutionID == institutionID)
				.ToArray();
			var cgTargets = dbContext1.CompetitiveGroupTarget
				.Include(x => x.CompetitiveGroupTargetItem)
				.Include(x => x.CompetitiveGroupTargetItem.Select(y => y.CompetitiveGroup.CompetitiveGroupItem))
				.Where(x => x.InstitutionID == institutionID)
				.ToArray();
			var benefits = dbContext1.BenefitItemC.Include(x => x.BenefitItemCOlympicType)
				.Where(x => x.CompetitiveGroup.InstitutionID == institutionID)
				.ToArray();
			//var benefitItemCOlympicTypes = dbContext1.BenefitItemCOlympicType.ToArray();

			var entranceTestItems = dbContext1.EntranceTestItemC
				.Where(x => x.CompetitiveGroup.InstitutionID == institutionID)
				.ToArray();

            var campaigns = dbContext1.Campaign
                .Include(x => x.CampaignEducationLevel)
                .Where(x => x.InstitutionID == institutionID);
            //.Include(x => x.CampaignDate)

            if (!filter.IsEmpty && filter.CompetitiveGroups != null)
            {
                var campaignUids = new HashSet<string>();
                campaignUids.UnionWith(filter.CompetitiveGroups.Where(c => !string.IsNullOrEmpty(c)).Select(c => c));

                if (campaignUids.Count > 0)
                    campaigns = campaigns.WhereIn(c => c.UID, campaignUids, chunkSize: 500).AsQueryable();
            }

			List<CompetitiveGroupDto> cgGroupsDto = new List<CompetitiveGroupDto>(cgCgroups.Length);
			foreach (var cg in cgCgroups)
			{
				var cgDto = Mapper.Map(cg, new CompetitiveGroupDto());
				cgGroupsDto.Add(cgDto);

				cgDto.Items = cg.CompetitiveGroupItem
					.ToArray().Select(x => Mapper.Map(x, new CompetitiveGroupItemDto())).ToArray();

				var targets = cgTargets
					.Where(x => x.CompetitiveGroupTargetItem.Any(y => y.CompetitiveGroup.CompetitiveGroupID == cg.CompetitiveGroupID))
					.ToArray();
				List<CompetitiveGroupTargetDto> targetDtos = new List<CompetitiveGroupTargetDto>();
                for(int i = 0; i < targets.Length; i++)
                {
                    var target = targets[i];
                //foreach (var target in targets)
                //{
                    CompetitiveGroupTargetDto targetDto = new CompetitiveGroupTargetDto();
					targetDtos.Add(targetDto);

					targetDto.TargetOrganizationName = target.Name;
					targetDto.UID = target.UID;
					targetDto.ParentUID = cg.UID;
				    //targetDto.TargetUID = target.UID;

					targetDto.Items = target.CompetitiveGroupTargetItem
						.Select(x => new CompetitiveGroupTargetItemDto
						             {
						             	DirectionID = x.CompetitiveGroup.DirectionID.ToString(),
										EducationLevelID = (x.CompetitiveGroup.EducationLevelID).ToString(),
						             	NumberTargetO = x.NumberTargetO.ToString(),
										NumberTargetOZ = x.NumberTargetOZ.ToString(),
										NumberTargetZ = x.NumberTargetZ.ToString(),
						             	UID = x.CompetitiveGroup.UID,
						             	ParentUID = target.UID
						             }).ToArray();
					foreach (var item in targetDto.Items)
					{
						item.DirectionID = item.DirectionID.Trim();
						item.EducationLevelID = item.EducationLevelID.Trim();
						item.NumberTargetO = item.NumberTargetO.Trim();
						item.NumberTargetOZ = item.NumberTargetOZ.Trim();
						item.NumberTargetZ = item.NumberTargetZ.Trim();
					}
				}

				cgDto.TargetOrganizations = targetDtos.ToArray();
				cgDto.CommonBenefit = benefits.Where(x => x.CompetitiveGroupID == cg.CompetitiveGroupID && x.EntranceTestItemID == null)
						.ToArray()
						.Select(x => new BenefitItemDto
						             {
						             	BenefitKindID = x.BenefitID.ToString(),
						             	IsForAllOlympics = x.IsForAllOlympic ? "1" : "0",
										OlympicDiplomTypes = x.OlympicDiplomTypeID == 3 ? new[] { "1", "2" } : new[] { x.OlympicDiplomTypeID.ToString() },
                                        OlympicYear = x.OlympicYear,
						             	Olympics = 
                                            x.OlympicYear < 2013 ?
                                            x.BenefitItemCOlympicType.Select(y => y.ID).ToArray().Select(y => y.ToString()).ToArray() : null,
                                        OlympicsLevels = 
                                            /*x.OlympicYear > 2012 ?
                                            x.BenefitItemCOlympicType.Where(y => y.BenefitItemID == x.BenefitItemID)
                                            .Select(c => c.OlympicType.OlympicTypeSubjectLink).ToArray().Aggregate(
                                            new HashSet<OlympicDto>(), (t, u) =>
                                                {
                                                    t.UnionWith(u.Select(c =>
                                                             new OlympicDto
                                                                 {
                                                                     OlympicID = c.OlympicID.ToString(),
                                                                     LevelID = c.SubjectLevelID != null ? c.SubjectLevelID.ToString() : null
                                                                 }));
                                                    return t;
                                                }).Distinct().ToArray() :*/ null,
						             	UID = x.UID,
                            ParentUID = cg.UID
						             }).ToArray();

				var etItems = entranceTestItems.Where(x => x.CompetitiveGroupID == cg.CompetitiveGroupID).ToArray();
				List<EntranceTestItemDto> etItemsDto = new List<EntranceTestItemDto>();
				foreach (var et in etItems)
				{
					EntranceTestItemDto etDto = new EntranceTestItemDto
					                            {
					                            	//Form = et.Form,
													MinScore = et.MinScore.HasValue ? et.MinScore.Value.ToString("0.####", CultureInfo.InvariantCulture) : "",
					                            	UID = et.UID,
					                            	ParentUID = cg.UID,
                                                    EntranceTestPriority = et.EntranceTestPriority == null ? null : et.EntranceTestPriority.ToString(),
					                            	EntranceTestSubject =
					                            		new EntranceTestSubjectDto
					                            		{
					                            			SubjectID = et.SubjectID != null ? et.SubjectID.ToString() : null,
					                            			SubjectName = et.SubjectName
					                            		}
					                            };
					etDto.EntranceTestBenefits =
						benefits.Where(
							x => x.CompetitiveGroupID == cg.CompetitiveGroupID && x.EntranceTestItemID == et.EntranceTestItemID)
							.ToArray()
							.Select(x => new BenefitItemDto
							             {
							             	BenefitKindID = x.BenefitID.ToString(),
							             	IsForAllOlympics = x.IsForAllOlympic ? "1" : "0",
											OlympicDiplomTypes = x.OlympicDiplomTypeID == 3 ? new[] { "1", "2" } : new[] { x.OlympicDiplomTypeID.ToString() },
                                            OlympicYear = x.OlympicYear,
                                            Olympics =
                                                x.OlympicYear < 2013 ?
                                                x.BenefitItemCOlympicType.Select(y => y.ID).ToArray().Select(y => y.ToString()).ToArray() : null,
                                            OlympicsLevels = /*
                                                x.OlympicYear > 2012 ?
                                                x.BenefitItemCOlympicType.Where(y => y.BenefitItemID == x.BenefitItemID)
                                                .Select(c => c.OlympicType.OlympicTypeSubjectLink).ToArray().Aggregate(
                                                new HashSet<OlympicDto>(), (t, u) =>
                                                {
                                                    t.UnionWith(u.Select(c =>
                                                             new OlympicDto
                                                             {
                                                                 OlympicID = c.OlympicID.ToString(),
                                                                 LevelID = c.SubjectLevelID != null ? c.SubjectLevelID.ToString() : null
                                                             }));
                                                    return t;
                                                }).Distinct().ToArray() :*/ null,
							             	UID = x.UID,
							             	ParentUID = et.UID
							             }).ToArray();
					etItemsDto.Add(etDto);
				}

				cgDto.EntranceTestItems = etItemsDto.ToArray();
			}

			return cgGroupsDto.Where(x => campaigns.Select(y => y.UID).Contains(x.CampaignUID)).ToArray();
		}

        private static ApplicationDto[] GenerateApplications(int institutionID, EntrantsEntities dbContext, /*string userLogin,*/ InstitutionInformationFilter filter)
        {
#warning Тут очень сильно тормозит на большом кол-ве заявлений!!

            // MISSING: ApplicationSelectedCompetitiveGroup, ApplicationSelectedCompetitiveGroupItem - пустые таблицы!
            //List<Application> appslist;
            //var apps = dbContext.Application
            //    .Include(x => x.ApplicationSelectedCompetitiveGroup)
            //    .Include(x => x.ApplicationSelectedCompetitiveGroupItem)
            //    .Include(x => x.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroupID))
            //    .Include(x => x.ApplicationSelectedCompetitiveGroupItem.Select(y => y.CompetitiveGroupItem))
            //    .Include(x => x.ApplicationSelectedCompetitiveGroupTarget)
            //    .Include(x => x.ApplicationSelectedCompetitiveGroupTarget.Select(y => y.CompetitiveGroupTarget))
            //    .Include(x => x.Entrant)
            //    .Where(x => x.InstitutionID == institutionID);

            //            if (!filter.IsEmpty && filter.Applications != null)
            //            {
            //                var statusesIds = new HashSet<int>();
            //                statusesIds.UnionWith(filter.Applications.Where(c => c.StatusID.HasValue).Select(c => c.StatusID.Value));
            //                var uids = new HashSet<string>();
            //                uids.UnionWith(filter.Applications.Where(c => !string.IsNullOrEmpty(c.UID)).Select(c => c.UID));

            //                /* Может быть запрос большого кол-ва UID */
            //                if (uids.Count > 0)
            //                    apps = Queryable.AsQueryable<Application>(apps.WhereIn(c => c.UID, uids, chunkSize: 500));

            //                if (statusesIds.Count > 0)
            //                    apps = apps.Where(c => statusesIds.Contains(c.StatusID));
            //            }

            //#warning Дичайшие тормоза!!!
            //            appslist = apps.ToList();
            //            var applicationsIds = new HashSet<int>();
            //            applicationsIds.UnionWith(appslist.Select(c => c.ApplicationID));

            //            //выгружаем вначале все
            //            var appCommonBenefitsAll = Enumerable.ToDictionary(dbContext.ApplicationEntranceTestDocument
            //                                        .Include(x => x.EntrantDocument)
            //                                        .Include(x => x.EntranceTestItemC)
            //                                        .Include(x => x.CompetitiveGroup)
            //                                        .Where(x => x.Application.InstitutionID == institutionID)
            //                                        .WhereIn(c => c.ApplicationID, applicationsIds, chunkSize: 500)
            //                                        .ToArray()
            //                                        .GroupBy(x => x.ApplicationID), x => x.Key, x => Enumerable.ToArray(x));

            //            var appEntrantDocuments = Enumerable.ToDictionary(dbContext.ApplicationEntrantDocument
            //                                        .Include(x => x.EntrantDocument)
            //                                        .Where(x => x.Application.InstitutionID == institutionID)
            //                                        .WhereIn(c => c.ApplicationID, applicationsIds, chunkSize: 500)
            //                                        .ToArray()
            //                                        .GroupBy(x => x.ApplicationID), x => x.Key, x => Enumerable.ToArray(x));

            //            var sw = new Stopwatch();
            //            sw.Start();
            //            var appDtos = appslist
            //                .Select(app => GenerateApplication(dbContext, appEntrantDocuments, appCommonBenefitsAll, sw, app))
            //                .ToArray();

#warning Отключил arzyanin
            ////using (ImportEntities iContext = new ImportEntities())
            ////{
            ////    PDImportLoggerExtension.AddApplicationAccessToLog(PDImportLoggerExtension, iContext,
            ////        appslist.Select(x => new PersonalDataAccessLogger.AppData(x)).ToArray(), "InstitutionExport", institutionID, userLogin);
            ////});

            //return appDtos;
            return new ApplicationDto[] { };
        }

        private static ApplicationDto GenerateApplication(EntrantsEntities dbContext, Dictionary<int, ApplicationEntrantDocument[]> appEntrantDocuments,
            Dictionary<int, ApplicationEntranceTestDocument[]> appCommonBenefitsAll, Stopwatch sw, Application app)
        {
            ApplicationDto dto = new ApplicationDto();
            dto.RegistrationDateDate = app.RegistrationDate;
            dto.ApplicationNumber = app.ApplicationNumber;
            dto.UID = app.UID;
            dto.StatusID = app.StatusID.ToString();
            dto.StatusComment = app.StatusDecision;
            dto.LastDenyDate = app.LastDenyDate == null ? null : app.LastDenyDate.Value.GetDateAsString();
            dto.NeedHostel = app.NeedHostel != null && app.NeedHostel.Value ? "1" : "0";
            //dto.SelectedCompetitiveGroups = app.ApplicationSelectedCompetitiveGroup.Select(y => y.CompetitiveGroup.UID).ToArray();
            dto.SelectedCompetitiveGroups = new[] { app.CompetitiveGroup.UID };
            dto.SelectedCompetitiveGroupItems = new[] { app.ApplicationSelectedCompetitiveGroupItem.ToString() };

			//dto.OriginalDocumentsReceived = app.OriginalDocumentsReceived ? "1" : "0";
			//dto.OriginalDocumentsReceivedDate = app.OriginalDocumentsReceivedDate.GetNullableDateAsString();

			dto.Entrant = Mapper.Map(app.Entrant, new EntrantDto());
			dto.Entrant.LastName = app.Entrant.LastName;
			dto.Entrant.FirstName = app.Entrant.FirstName;
			dto.Entrant.MiddleName = app.Entrant.MiddleName;
			dto.Entrant.GenderID = app.Entrant.GenderID.ToString();

    		//dto.Entrant.FatherData = bindParents(app.Entrant.PersonParentFather);
			//dto.Entrant.MotherData = bindParents(app.Entrant.PersonParentMother);
			//dto.Entrant.FactAddress = Mapper.Map(app.Entrant.AddressFact, new AddressDto());
			//dto.Entrant.RegistrationAddress = Mapper.Map(app.Entrant.AddressReg, new AddressDto());

			//dto.Entrant.ForeignLanguages =
			//	app.Entrant.EntrantLanguage.Select(x => x.LanguageID).ToArray().Select(x => x.ToString()).ToArray();

			List<FinSourceEduFormDto> finForms = new List<FinSourceEduFormDto>();
			if (app.IsRequiresBudgetO) finForms.Add(new FinSourceEduFormDto { EducationFormID = "11", FinanceSourceID = "14" });
			if (app.IsRequiresBudgetOZ) finForms.Add(new FinSourceEduFormDto { EducationFormID = "12", FinanceSourceID = "14" });
			if (app.IsRequiresBudgetZ) finForms.Add(new FinSourceEduFormDto { EducationFormID = "10", FinanceSourceID = "14" });
			if (app.IsRequiresPaidO) finForms.Add(new FinSourceEduFormDto { EducationFormID = "11", FinanceSourceID = "15" });
			if (app.IsRequiresPaidOZ) finForms.Add(new FinSourceEduFormDto { EducationFormID = "12", FinanceSourceID = "15" });
			if (app.IsRequiresPaidZ) finForms.Add(new FinSourceEduFormDto { EducationFormID = "10", FinanceSourceID = "15" });

			if (app.IsRequiresTargetO)
				finForms.Add(new FinSourceEduFormDto
				{
					EducationFormID = "11",
					FinanceSourceID = "16",
					TargetOrganizationUID =
						app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForO).Select(x => x.CompetitiveGroupTarget.UID).FirstOrDefault()
				});
			if (app.IsRequiresTargetOZ)
				finForms.Add(new FinSourceEduFormDto
				{
					EducationFormID = "12",
					FinanceSourceID = "16",
					TargetOrganizationUID =
						app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForOZ).Select(x => x.CompetitiveGroupTarget.UID).FirstOrDefault()
				});
			if (app.IsRequiresTargetZ)
				finForms.Add(new FinSourceEduFormDto
				{
					EducationFormID = "10",
					FinanceSourceID = "16",
					TargetOrganizationUID =
						app.ApplicationSelectedCompetitiveGroupTarget.Where(x => x.IsForZ).Select(x => x.CompetitiveGroupTarget.UID).FirstOrDefault()
				});

			dto.FinSourceAndEduForms = finForms.ToList();

			ApplicationEntranceTestDocument[] benefitsByApplication;
			if (!appCommonBenefitsAll.TryGetValue(app.ApplicationID, out benefitsByApplication))
				benefitsByApplication = new ApplicationEntranceTestDocument[0];
			var appCommonBenefits = benefitsByApplication
#warning https://redmine.armd.ru/issues/18642
                //.Where(x => x.EntranceTestItemID == null)
                .ToArray();

            ApplicationEntrantDocument[] appDocuments;
            if (!appEntrantDocuments.TryGetValue(app.ApplicationID, out appDocuments))
                appDocuments = new ApplicationEntrantDocument[0];

		    dto.ApplicationCommonBenefits = appCommonBenefits.Select(appCommonBenefit => new ApplicationCommonBenefitDto
			{
				ApplicationCommonBenefitID = appCommonBenefit.ID.ToString(),
                CompetitiveGroupID = appCommonBenefit.CompetitiveGroup != null ? appCommonBenefit.CompetitiveGroup.UID : null,
                BenefitKindID = appCommonBenefit.BenefitID != null ? appCommonBenefit.BenefitID.ToString() : null,
				UID = appCommonBenefit.UID,
				ParentUID = app.UID,
				DocumentTypeID = appCommonBenefit.EntrantDocument != null ? appCommonBenefit.EntrantDocument.DocumentTypeID.ToString() : null,
				EntrantDocumentID = appCommonBenefit.EntrantDocumentID,
#warning https://redmine.armd.ru/issues/18642
                DocumentReason = GetBenefitDocumentDto(appCommonBenefit.EntrantDocument, dbContext, appDocuments)
			}).ToArray();

			var etResults = benefitsByApplication
				.Where(x => x.EntranceTestItemID != null)
				.ToArray();

			var etResultDtos = new List<EntranceTestAppItemDto>();
			foreach (var etResult in etResults)
			{
				EntranceTestAppItemDto etResultDto = new EntranceTestAppItemDto();
				etResultDtos.Add(etResultDto);
				etResultDto.EntranceTestSubject = new EntranceTestSubjectDto
				{
					SubjectID = etResult.EntranceTestItemC.SubjectID != null
						? etResult.EntranceTestItemC.SubjectID.ToString()
						: null,
					SubjectName = etResult.EntranceTestItemC.SubjectName
				};
				etResultDto.EntranceTestTypeID = etResult.EntranceTestItemC != null
					? etResult.EntranceTestItemC.EntranceTestTypeID.ToString() : null;
				etResultDto.ResultSourceTypeID = etResult.SourceID != null ? etResult.SourceID.ToString() : null;
				etResultDto.ResultValue = etResult.ResultValue != null ? etResult.ResultValue.Value.ToString("0.####") : null;
				etResultDto.UID = etResult.UID;
				etResultDto.ParentUID = app.UID;
				etResultDto.EntranceTestsResultID = etResult.ID.ToString();

				if (etResult.SourceID == (int)EntranceTestResultSourceEnum.InstitutionEntranceTest)
				{
					etResultDto.ResultDocument = new EntranceTestResultDocumentsDto
					{
						InstitutionDocument = new EntranceTestInstitutionDocumentDto
						{
							DocumentDate = etResult.InstitutionDocumentDate.GetNullableDateAsString(),
                            DocumentTypeID = etResult.InstitutionDocumentTypeID != null ? etResult.InstitutionDocumentTypeID.ToString() : null,
							DocumentNumber = etResult.InstitutionDocumentNumber
						}
					};
				}
				else
                    etResultDto.ResultDocument = GetEntranceTestDocumentDto(etResult.EntrantDocument, dbContext);
			}

			dto.EntranceTestResults = etResultDtos.ToArray();
			dto.ApplicationDocuments = GetApplicationDocumentDto(dbContext, appDocuments);

			//Debug.WriteLine(sw.Elapsed.TotalSeconds + " " + (sw.Elapsed.TotalSeconds));
			return dto;
		}

        private static ApplicationCommonBenefitDocumentsDto GetBenefitDocumentDto(EntrantDocument document, 
            EntrantsEntities dbContext, ApplicationEntrantDocument[] documents)
		{
			if (document == null)
				return null;
			var dto = new ApplicationCommonBenefitDocumentsDto();
			var docModel = EntrantDocumentExtensions.GetEntrantDocumentViewModel(document, false);
            if (docModel is CustomDocumentViewModel)
            {
                dto.CustomDocument = Mapper.Map((CustomDocumentViewModel) docModel, new CustomDocumentDto());
                /* обнуляем Subjects */
                dto.CustomDocument.Subjects = null;
            }
		    if (docModel is OlympicDocumentViewModel)
			{
				var olDoc = docModel as OlympicDocumentViewModel;
				lock (dbContext)
                    olDoc.FillData(dbContext, true, null, null);
                dto.OlympicDocument = Mapper.Map((OlympicDocumentViewModel)docModel, new OlympicDocumentDto());
				//dto.OlympicDocument.Subjects = olDoc.OlympicDetails.SubjectIDs.Select(x => new SubjectBriefDataDto {SubjectID = x.ToString()}).ToArray();
				dto.OlympicDocument.OlympicID = olDoc.OlympicID.ToString();
                /* обнуляем Subjects */
                dto.OlympicDocument.Subjects = null;
			}

			if (docModel is OlympicTotalDocumentViewModel)
			{
				var olDoc = docModel as OlympicTotalDocumentViewModel;
                dto.OlympicTotalDocument = Mapper.Map((OlympicTotalDocumentViewModel)docModel, new OlympicTotalDocumentDto());
				dto.OlympicTotalDocument.Subjects = olDoc.Subjects.Select(x => new SubjectBriefDataDto { SubjectID = x.SubjectID.ToString() }).ToArray();
				dto.OlympicTotalDocument.OlympicDate = olDoc.OlympicDate.GetNullableDateAsString();
				dto.OlympicTotalDocument.OlympicPlace = olDoc.OlympicPlace;
			}

            if (docModel is PsychoDocumentViewModel)
            {
                dto.MedicalDocuments = new MedicalDocumentsDto { AllowEducationDocument = Mapper.Map((PsychoDocumentViewModel)docModel, new AllowEducationDocumentDto()) };
                /* обнуляем Subjects */
                dto.MedicalDocuments.AllowEducationDocument.Subjects = null;
            }
            if (docModel is DisabilityDocumentViewModel)
            {
                dto.MedicalDocuments = new MedicalDocumentsDto { BenefitDocument = new BenefitDocumentsDto { DisabilityDocument = Mapper.Map((DisabilityDocumentViewModel)docModel, new DisabilityDocumentDto()) } };
                /* обнуляем Subjects */
                dto.MedicalDocuments.BenefitDocument.DisabilityDocument.Subjects = null;
                /* + ищем AllowEducationDocument */
                var allowEducationDocument = documents.SingleOrDefault(c => c.EntrantDocument.DocumentTypeID == (int)EntrantDocumentType.AllowEducationDocument);
                if (allowEducationDocument != null)
                {
                    var allowEducationDocumentModel = EntrantDocumentExtensions.GetEntrantDocumentViewModel(allowEducationDocument.EntrantDocument, false);
                    dto.MedicalDocuments.AllowEducationDocument = Mapper.Map((PsychoDocumentViewModel)allowEducationDocumentModel, new AllowEducationDocumentDto());
                    /* обнуляем Subjects */
                    dto.MedicalDocuments.AllowEducationDocument.Subjects = null;
                }
            }
			return dto;
		}

        private static EntranceTestResultDocumentsDto GetEntranceTestDocumentDto(EntrantDocument document, EntrantsEntities dbContext)
		{
			if (document == null)
				return null;
			EntranceTestResultDocumentsDto dto = new EntranceTestResultDocumentsDto();
			var docModel = EntrantDocumentExtensions.GetEntrantDocumentViewModel(document, false);
			if (docModel is OlympicDocumentViewModel)
			{
				var olDoc = docModel as OlympicDocumentViewModel;
				lock (dbContext)
                    olDoc.FillData(dbContext, true, null, null);
                dto.OlympicDocument = Mapper.Map((OlympicDocumentViewModel)docModel, new OlympicDocumentDto());
				//dto.OlympicDocument.Subjects = olDoc.OlympicDetails.SubjectIDs.Select(x => new SubjectBriefDataDto {SubjectID = x.ToString()}).ToArray();
				dto.OlympicDocument.OlympicID = olDoc.OlympicID.ToString();
			}

			if (docModel is OlympicTotalDocumentViewModel)
			{
				var olDoc = docModel as OlympicTotalDocumentViewModel;
                dto.OlympicTotalDocument = Mapper.Map((OlympicTotalDocumentViewModel)docModel, new OlympicTotalDocumentDto());
				dto.OlympicTotalDocument.Subjects = olDoc.Subjects.Select(x => new SubjectBriefDataDto { SubjectID = x.SubjectID.ToString() }).ToArray();
				dto.OlympicTotalDocument.OlympicDate = olDoc.OlympicDate.GetNullableDateAsString();
				dto.OlympicTotalDocument.OlympicPlace = olDoc.OlympicPlace;
			}

			if (docModel is EGEDocumentViewModel)
				dto.EgeDocumentID = docModel.UID;
			if (docModel is GiaDocumentViewModel)
				dto.EgeDocumentID = docModel.UID;
			return dto;
		}

        private static ApplicationDocumentsDto GetApplicationDocumentDto(EntrantsEntities dbContext, ApplicationEntrantDocument[] documents)
		{
            var dto = new ApplicationDocumentsDto();
			//dto.MandatoryEduDocument = new EduDocumentsDto();
			foreach (var documentData in documents)
			{
				var docModel = EntrantDocumentExtensions.GetEntrantDocumentViewModel(documentData.EntrantDocument, false);
				lock (dbContext)
				 docModel.FillDataImportLoadSave(dbContext);
                
                //if (docModel is CustomDocumentViewModel)
                //{
                //    dto.CustomDocuments = (dto.CustomDocuments ?? new CustomDocumentDto[0])
                //        .Concat(new[] { SetReceived(Mapper.Map((CustomDocumentViewModel)docModel, new CustomDocumentDto()), documentData.OriginalReceivedDate) })
                //        .ToArray();
                //    foreach (var doc in dto.CustomDocuments)
                //        doc.Subjects = null;
                //}

                if (docModel is IdentityDocumentViewModel)
                {
                    dto.IdentityDocument = SetReceived(Mapper.Map((IdentityDocumentViewModel)docModel, new IdentityDocumentDto()), documentData.OriginalReceivedDate);
                }
				if (docModel is EGEDocumentViewModel)
				{
					var egeModel = (EGEDocumentViewModel)docModel;
					var egeDto = SetReceived(Mapper.Map(egeModel, new EgeDocumentWithSubjectsDto()), documentData.OriginalReceivedDate);
					egeDto.Subjects =
						egeModel.Subjects.Select(x => new SubjectDataDto { SubjectID = x.SubjectID.ToString(), Value = x.Value.ToString() })
							.ToArray();
					dto.EgeDocuments = (dto.EgeDocuments ?? new EgeDocumentWithSubjectsDto[0])
						.Concat(new[] { egeDto })
						.ToArray();
				}

				if (docModel is GiaDocumentViewModel)
				{
					var giaModel = (GiaDocumentViewModel)docModel;
					var giaDto = SetReceived(Mapper.Map(giaModel, new GiaDocumentWithSubjectsDto()), documentData.OriginalReceivedDate);
					giaDto.Subjects =
						giaModel.Subjects.Select(x => new SubjectDataDto { SubjectID = x.SubjectID.ToString(), Value = x.Value.ToString() })
							.ToArray();
					dto.GiaDocuments = (dto.GiaDocuments ?? new GiaDocumentWithSubjectsDto[0])
						.Concat(new[] { giaDto })
						.ToArray();
				}

                if (docModel is MilitaryCardDocumentViewModel)
                {
                    dto.MilitaryCardDocument = Mapper.Map((MilitaryCardDocumentViewModel)docModel, new MilitaryCardDocumentDto());
                    dto.MilitaryCardDocument.Subjects = null;
                }

                if (docModel is StudentDocumentViewModel)
                {
                    dto.StudentDocument = Mapper.Map((StudentDocumentViewModel)docModel, new StudentDocumentDto());
                    dto.StudentDocument.Subjects = null;
                }

				var edDocDto = new EduDocumentsDto();
				bool isDocumentFound = true;
                if (docModel.DocumentTypeID == 3)
                {
                    edDocDto.SchoolCertificateDocument =
                        SetReceived(Mapper.Map((SchoolCertificateDocumentViewModel)docModel, new SchoolCertificateDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.SchoolCertificateDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 16)
                {
                    edDocDto.SchoolCertificateBasicDocument =
                        SetReceived(Mapper.Map((SchoolCertificateDocumentViewModel)docModel, new SchoolCertificateBasicDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.SchoolCertificateBasicDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 4)
                {
                    edDocDto.HighEduDiplomaDocument =
                        SetReceived(Mapper.Map((DiplomaDocumentViewModel)docModel, new HighEduDiplomaDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.HighEduDiplomaDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 5)
                {
                    edDocDto.MiddleEduDiplomaDocument =
                        SetReceived(Mapper.Map((DiplomaDocumentViewModel)docModel, new MiddleEduDiplomaDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.MiddleEduDiplomaDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 6)
                {
                    edDocDto.BasicDiplomaDocument =
                        SetReceived(Mapper.Map((BasicDiplomaDocumentViewModel)docModel, new BasicDiplomaDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.BasicDiplomaDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 7)
                {
                    edDocDto.IncomplHighEduDiplomaDocument =
                        SetReceived(Mapper.Map((DiplomaDocumentViewModel)docModel, new IncomplHighEduDiplomaDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.IncomplHighEduDiplomaDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 8)
                {
                    edDocDto.AcademicDiplomaDocument =
                        SetReceived(Mapper.Map((DiplomaDocumentViewModel)docModel, new AcademicDiplomaDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.AcademicDiplomaDocument.Subjects = null;
                }
                else if (docModel.DocumentTypeID == 19)
                {
                    edDocDto.EduCustomDocument =
                        SetReceived(Mapper.Map((EduCustomDocumentViewModel)docModel, new EduCustomDocumentDto()), documentData.OriginalReceivedDate);
                    edDocDto.EduCustomDocument.Subjects = null;
                }
                else isDocumentFound = false; //это не образовательный документ
				if (isDocumentFound)
				{
					if (dto.EduDocuments == null)
						dto.EduDocuments = new[] { edDocDto };
					else
						dto.EduDocuments = dto.EduDocuments.Concat(new[] { edDocDto }).ToArray();
				}
			}
			
			return dto;
		}

		private static T SetReceived<T>(T dto, DateTime? receivedDate) where T : BaseDocumentDto
		{
			if (receivedDate.HasValue)
			{
				dto.OriginalReceived = "1";
				dto.OriginalReceivedDate = receivedDate.Value.GetDateAsString();
			}
			else
			{
				dto.OriginalReceived = "0";
			}

			return dto;
		}

        private static CampaignDto[] GenerateCampaigns(int institutionID, ImportEntities dbContext, InstitutionInformationFilter filter)
		{
			var campaigns = dbContext.Campaign
				.Include(x => x.CampaignEducationLevel)
				.Include(x => x.CampaignDate)
				.Where(x => x.InstitutionID == institutionID);

            if (!filter.IsEmpty && filter.Campaigns != null)
            {
                var campaignUids = new HashSet<string>();
                campaignUids.UnionWith(filter.Campaigns.Where(c => !string.IsNullOrEmpty(c)).Select(c => c));

                if (campaignUids.Count > 0)
                    campaigns = campaigns.WhereIn(c => c.UID, campaignUids, chunkSize: 500).AsQueryable();
            }

            var dtos = new List<CampaignDto>();
            var campaignsCollection = campaigns.ToList();

            foreach (var dbCampaign in campaignsCollection)
			{
				var dto = new CampaignDto();
			    dto.CampaignID = dbCampaign.CampaignID;
				dto.UID = dbCampaign.UID;
				dto.Name = dbCampaign.Name;
				dto.YearStart = dbCampaign.YearStart.ToString();
				dto.YearEnd = dbCampaign.YearEnd.ToString();
				dto.StatusID = dbCampaign.StatusID;

				var educationForms = new List<int>();
				if ((dbCampaign.EducationFormFlag & 1) != 0) educationForms.Add(AdmissionItemTypeConstants.FullTimeTuition);
				if ((dbCampaign.EducationFormFlag & 2) != 0) educationForms.Add(AdmissionItemTypeConstants.MixedTuition);
				if ((dbCampaign.EducationFormFlag & 4) != 0) educationForms.Add(AdmissionItemTypeConstants.PostalTuition);
				dto.EducationForms = educationForms.Select(x => x.ToString()).ToArray();

				dto.EducationLevels = dbCampaign.CampaignEducationLevel
					.Select(x => new CampaignEducationLevelDto { Course = x.Course.ToString(), EducationLevelID = x.EducationLevelID.ToString() }).ToArray();

				dto.CampaignDates = dbCampaign.CampaignDate.Where(x => x.IsActive)
					.Select(x => new CampaignDateDto
					             {
					             	Course = x.Course.ToString(),
									EducationLevelID = x.EducationLevelID.ToString(),
									EducationFormID = x.EducationFormID.ToString(),
									EducationSourceID = x.EducationSourceID.ToString(),
									Stage = x.Stage != 0 ? x.Stage.ToString() : null,
									UID = x.UID,
									DateStart = x.DateStart.GetNullableDateAsString(),
									DateEnd = x.DateEnd.GetNullableDateAsString(),
									DateOrder = x.DateOrder.GetNullableDateAsString()
					             }).ToArray();

				dtos.Add(dto);
			}

			return dtos.ToArray();
		}

		private static OrderOfAdmissionItemDto[] GenerateOrderOfAdmissions(int institutionID, ImportEntities dbContext, InstitutionInformationFilter filter)
		{
            var campaigns = dbContext.Campaign
                .Include(x => x.CampaignEducationLevel)
                .Include(x => x.CampaignDate)
                .Where(x => x.InstitutionID == institutionID);

            if (!filter.IsEmpty && filter.OrdersOfAdmission != null)
            {
                var campaignUids = new HashSet<string>();
                campaignUids.UnionWith(filter.OrdersOfAdmission.Where(c => !string.IsNullOrEmpty(c)).Select(c => c));

                if (campaignUids.Count > 0)
                    campaigns = campaigns.WhereIn(c => c.UID, campaignUids, chunkSize: 500).AsQueryable();
            }

			List<OrderOfAdmissionItemDto> items = new List<OrderOfAdmissionItemDto>();
			var apps = dbContext.Application
				.Include(x => x.Entrant)
				.Include(x => x.CompetitiveGroupItem)
				.Include(x => x.CompetitiveGroupItem.Direction)
				.Include(x => x.CompetitiveGroup)
				.Include(x => x.OrderOfAdmission)
				.Where(x => x.InstitutionID == institutionID && x.StatusID == 8)
                .ToArray();

		    var camps = campaigns.Select(y => y.CampaignID);

            apps = apps.Where(x => camps.Contains(x.CompetitiveGroup.CampaignID ?? 0)).ToArray();

			foreach (var dbApp in apps)
			{
				OrderOfAdmissionItemDto item = new OrderOfAdmissionItemDto
				                               {
				                               	Application = new ApplicationRef
				                               	              {
				                               	              	ApplicationNumber = dbApp.ApplicationNumber,
                                                                FirstName = dbApp.Entrant.FirstName,
                                                                LastName = dbApp.Entrant.LastName,
                                                                MiddleName = dbApp.Entrant.MiddleName,
																RegistrationDateDate = dbApp.RegistrationDate
				                               	              },
												DirectionID = dbApp.CompetitiveGroupItem != null ? dbApp.CompetitiveGroupItem.DirectionID.ToString() : null,
												DirectionName = dbApp.CompetitiveGroupItem != null && dbApp.CompetitiveGroupItem.Direction != null ? dbApp.CompetitiveGroupItem.Direction.Name : null,
												EducationFormID = dbApp.OrderEducationFormID.ToString(),
												FinanceSourceID = dbApp.OrderEducationSourceID.ToString(),
												EducationLevelID = dbApp.CompetitiveGroupItem != null ? dbApp.CompetitiveGroupItem.EducationLevelID.ToString() : null,
												Stage = dbApp.OrderOfAdmission.Stage > 0 ? dbApp.OrderOfAdmission.Stage.ToString() : null,
												IsBeneficiary = dbApp.OrderOfAdmission.IsForBeneficiary ? "1" : "0",
												UID = dbApp.OrderOfAdmission.UID
				                               };
				items.Add(item);
			}

			return items.ToArray();
		}
	}
}
