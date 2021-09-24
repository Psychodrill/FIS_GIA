using System;
using System.Linq;
using AutoMapper;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Results;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Infrastructure
{
    /// <summary>
    /// Маппинг dto объектов на объекты для bulk загрузки
    /// Дополнительная простановка параметров во время маппинга
    /// </summary>
    public class BulkEntitesMapper
    {
        public static void Initialize()
        {
            Mapper.CreateMap<ApplicationCommonBenefitDto, AppEntranceTestDocumentBulkEntity>()
                .ForMember(x => x.BenefitId, opt => opt.MapFrom(x => x.BenefitKindID))
                .ForMember(x => x.CompetitiveGroupUID, opt => opt.MapFrom(x => x.CompetitiveGroupID))
                .ForMember(x => x.BenefitEntrantDocumentId, opt => opt.MapFrom(x => x.GetEntranceTestBenefitDocumentId()));

            Mapper.CreateMap<EntranceTestAppItemDto, AppEntranceTestDocumentBulkEntity>()
                .ForMember(x => x.SourceId, opt => opt.MapFrom(x => x.ResultSourceTypeID.ToIntNullable()))
                .ForMember(x => x.SubjectId, opt => opt.MapFrom(x => x.With(c => c.EntranceTestSubject).Return(c => c.SubjectID.ToIntNullable(), null)))
                .ForMember(x => x.SubjectName, opt => opt.MapFrom(x => x.With(c => c.EntranceTestSubject).Return(c => c.SubjectName, null)))
                .ForMember(x => x.CompetitiveGroupUID, opt => opt.MapFrom(x => x.CompetitiveGroupID))
                .ForMember(x => x.EntranceTestTypeId, opt => opt.MapFrom(x => x.EntranceTestTypeID.ToIntNullable()))
                .ForMember(x => x.ResultValue, opt => opt.MapFrom(x => x.GetResultValue()))
                .AfterMap((a, b) =>
                              {
                                  /* Для документа OlympicDocument проставляется результат = 100 */
                                  if (b.SourceId.HasValue && b.SourceId.Value == (int)EntranceTestResultSourceEnum.InstitutionEntranceTest &&
                                      a.ResultDocument != null && a.ResultDocument.InstitutionDocument != null)
                                  {
                                      b.InstitutionDocumentTypeId = a.ResultDocument.InstitutionDocument.DocumentTypeID.ToIntNullable();
                                      b.InstitutionDocumentDate = a.ResultDocument.InstitutionDocument.DocumentDate.ToDateTimeNullable();
                                      b.InstitutionDocumentNumber = a.ResultDocument.InstitutionDocument.DocumentNumber;
                                  }
                                  else if (b.SourceId.HasValue && a.ResultDocument != null &&
                                      (b.SourceId.Value == (int)EntranceTestResultSourceEnum.EgeDocument ||
                                       b.SourceId.Value == (int)EntranceTestResultSourceEnum.GiaDocument))
                                  {
                                      b.EgeDocumentId = a.ResultDocument.EgeDocumentID;
                                  }
                              });

            Mapper.CreateMap<FinSourceEduFormDto, AppSelectedCompGroupTargetBulkEntity>()
                .ForMember(x => x.IsForZ, opt => opt.MapFrom(x => x.EducationFormIdInt == EDFormsConst.Z))
                .ForMember(x => x.IsForOZ, opt => opt.MapFrom(x => x.EducationFormIdInt == EDFormsConst.OZ))
                .ForMember(x => x.IsForO, opt => opt.MapFrom(x => x.EducationFormIdInt == EDFormsConst.O));

            Mapper.CreateMap<AcademicDiplomaDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<EduCustomDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<MilitaryCardDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<StudentDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<IncomplHighEduDiplomaDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<BasicDiplomaDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<MiddleEduDiplomaDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<HighEduDiplomaDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<SchoolCertificateBasicDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<SchoolCertificateDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<DisabilityDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<AllowEducationDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<OlympicDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<OlympicTotalDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<CustomDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();
            Mapper.CreateMap<PauperDocumentDto, EntrantDocumentBulkEntity>().InheritMappingFromSourceBaseType();

            Mapper.CreateMap<GiaDocumentWithSubjectsDto, EntrantDocumentBulkEntity>()
                .ForMember(x => x.DocumentSpecificData, opt => opt.MapFrom(x => x.GetDocumentSpecificData()))
                .InheritMappingFromSourceBaseType();

            Mapper.CreateMap<EgeDocumentWithSubjectsDto, EntrantDocumentBulkEntity>()
                .ForMember(x => x.DocumentSpecificData, opt => opt.MapFrom(x => x.GetDocumentSpecificData()))
                .InheritMappingFromSourceBaseType();

            Mapper.CreateMap<IdentityDocumentDto, EntrantDocumentBulkEntity>()
                .ForMember(x => x.IdentityDocumentTypeId, opt => opt.MapFrom(x => x.IdentityDocumentTypeID.ToIntNullable()))
                .ForMember(x => x.NationalityTypeId, opt => opt.MapFrom(x => x.NationalityTypeID.ToIntNullable()))
                .ForMember(x => x.BirthDate, opt => opt.MapFrom(x => x.BirthDate.ToDateTimeNullable()))
                .ForMember(x => x.DocumentSpecificData, opt => opt.MapFrom(x => x.GetDocumentSpecificData()))
                .InheritMappingFromSourceBaseType();

            Mapper.CreateMap<ApplicationDocumentDto, EntrantDocumentBulkEntity>()
                .ForMember(x => x.QualificationTypeId, opt => opt.MapFrom(x => x.QualificationTypeID.ToIntNullable()))
                .ForMember(x => x.GPA, opt => opt.MapFrom(x => x.GPA))
                .ForMember(x => x.ProfessionId, opt => opt.MapFrom(x => x.ProfessionID.ToIntNullable()))
                .ForMember(x => x.EndYear, opt => opt.MapFrom(x => x.EndYear.ToIntNullable()))
                .ForMember(x => x.SpecialityId, opt => opt.MapFrom(x => x.SpecialityID.ToIntNullable()))
                .ForMember(x => x.SpecializationId, opt => opt.MapFrom(x => x.SpecializationID.ToIntNullable()))
                .ForMember(x => x.DisabilityTypeId, opt => opt.MapFrom(x => x.DisabilityTypeID.ToIntNullable()))
                .ForMember(x => x.DiplomaTypeId, opt => opt.MapFrom(x => x.DiplomaTypeID.ToIntNullable()))
                .ForMember(x => x.OlympicId, opt => opt.MapFrom(x => x.OlympicID.ToIntNullable()))
                .ForMember(x => x.OlympicDate, opt => opt.MapFrom(x => x.OlympicDate.ToDateTimeNullable()))
                .ForMember(x => x.DocumentSpecificData, opt => opt.MapFrom(x => x.GetDocumentSpecificData()))
                .InheritMappingFromSourceBaseType();

            Mapper.CreateMap<BaseDocumentDto, EntrantDocumentBulkEntity>()
                .ForMember(x => x.DocumentTypeId, opt => opt.MapFrom(x => (int)x.EntrantDocumentType))
                .ForMember(x => x.OriginalReceived, opt => opt.MapFrom(x => x.OriginalReceived.ToBool(false)))
                .ForMember(x => x.DocumentDate, opt => opt.MapFrom(x => x.DocumentDate.ToDateTimeNullable()))
                .ForMember(x => x.DocumentOrganization, opt => opt.MapFrom(x => x.DocumentOrganization))
                .ForMember(x => x.DocumentNumber, opt => opt.MapFrom(x => x.DocumentNumber))
                .ForMember(x => x.OriginalReceivedDate, opt => opt.MapFrom(x => x.OriginalReceivedDate.ToDateTimeNullable()));

            Mapper.CreateMap<EntrantDto, EntrantBulkEntity>();

            Mapper.CreateMap<ApplicationDto, ApplicationBulkEntity>()
                .ForMember(x => x.EntrantUID, opt => opt.MapFrom(x => x.Entrant.UID))
                .ForMember(x => x.RegistrationDate, opt => opt.MapFrom(x => DateTime.Parse(x.RegistrationDateString)))
                .ForMember(x => x.StatusDecision, opt => opt.MapFrom(x => x.StatusComment))
                .ForMember(x => x.IsRequiresPaidO, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Paid &&
                    c.EducationFormIdInt == EDFormsConst.O)))
                .ForMember(x => x.IsRequiresPaidOZ, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Paid &&
                    c.EducationFormIdInt == EDFormsConst.OZ)))
                .ForMember(x => x.IsRequiresPaidZ, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Paid &&
                    c.EducationFormIdInt == EDFormsConst.Z)))
                .ForMember(x => x.IsRequiresBudgetO, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Budget &&
                    c.EducationFormIdInt == EDFormsConst.O)))
                .ForMember(x => x.IsRequiresBudgetOZ, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Budget &&
                    c.EducationFormIdInt == EDFormsConst.OZ)))
                .ForMember(x => x.IsRequiresBudgetZ, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Budget &&
                    c.EducationFormIdInt == EDFormsConst.Z)))
                .ForMember(x => x.IsRequiresTargetO, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Target &&
                    c.EducationFormIdInt == EDFormsConst.O)))
                .ForMember(x => x.IsRequiresTargetOZ, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Target &&
                    c.EducationFormIdInt == EDFormsConst.OZ)))
                .ForMember(x => x.IsRequiresTargetZ, opt => opt.MapFrom(x => x.FinSourceAndEduForms.Any(c =>
                    c.FinanceSourceIdInt == EDSourceConst.Target &&
                    c.EducationFormIdInt == EDFormsConst.Z)));

            Mapper.CreateMap<ConsideredApplicationDto, ConsideredApplicationBulkEntity>()
                .ForMember(x => x.ApplicationNumber, opt => opt.MapFrom(x => x.Application.ApplicationNumber))
                .ForMember(x => x.RegistrationDate, opt => opt.MapFrom(x => x.Application.RegistrationDateDate))
                .ForMember(x => x.IsRequiresPaidO, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Paid &&
                    x.EducationFormID == EDFormsConst.O))
                .ForMember(x => x.IsRequiresPaidOZ, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Paid &&
                    x.EducationFormID == EDFormsConst.OZ))
                .ForMember(x => x.IsRequiresPaidZ, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Paid &&
                    x.EducationFormID == EDFormsConst.Z))
                .ForMember(x => x.IsRequiresBudgetO, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Budget &&
                    x.EducationFormID == EDFormsConst.O))
                .ForMember(x => x.IsRequiresBudgetOZ, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Budget &&
                    x.EducationFormID == EDFormsConst.OZ))
                .ForMember(x => x.IsRequiresBudgetZ, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Budget &&
                    x.EducationFormID == EDFormsConst.Z))
                .ForMember(x => x.IsRequiresTargetO, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Target &&
                    x.EducationFormID == EDFormsConst.O))
                .ForMember(x => x.IsRequiresTargetOZ, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Target &&
                    x.EducationFormID == EDFormsConst.OZ))
                .ForMember(x => x.IsRequiresTargetZ, opt => opt.MapFrom(x =>
                    x.FinanceSourceID == EDSourceConst.Target &&
                    x.EducationFormID == EDFormsConst.Z));

            Mapper.CreateMap<RecommendedApplicationDto, RecommendedApplicationBulkEntity>().InheritMappingFromBothBaseTypes();

            /* Преобразование возвращаемых процедурой типов в обрабатываемые */
            Mapper.CreateMap<ApplicationShortRefResult, ConsideredApplicationDto>()
                .ForMember(x => x.Application, opt => opt.MapFrom(x => new ApplicationShortRef
                                                                           {
                                                                               ApplicationNumber = x.ApplicationNumber,
                                                                               RegistrationDateDate = x.RegistrationDate
                                                                           }));

            Mapper.CreateMap<ApplicationShortRefResult, RecommendedApplicationDto>().InheritMappingFromDestinationBaseType();
        }
    }
}
