using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.AppCheckProcessor;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Core.Packages.Repositories;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace GVUZ.ServiceModel.Import.Core.Packages
{
	/// <summary>
	/// Менеджер пактов импорта
	/// </summary>
	public class PackageManager
	{
		private static IPackageRepository _packageRepository;
		public static IPackageRepository PackageRepository
		{
			get { return _packageRepository ?? (_packageRepository = new DbPackageRepository()); }
		}

		/// <summary>
		/// Инициализация (маппинги)
		/// </summary>
		public static void Initialize()
		{
			// маппинги для импорта: формируются объекты из dto
			Mapper.CreateMap<string, short>().ConstructUsing(ConvertExtensions.ParseShort);
			Mapper.CreateMap<string, int>().ConstructUsing(ConvertExtensions.ParseInt32);
			Mapper.CreateMap<string, decimal>().ConstructUsing(ConvertExtensions.ParseDecimal);
			Mapper.CreateMap<string, bool>().ConstructUsing(ConvertExtensions.ParseBool);
			Mapper.CreateMap<string, DateTime>().ConstructUsing(DateTimeExtensions.GetStringAsDate);
			Mapper.CreateMap<string, DateTime?>().ConstructUsing(DateTimeExtensions.GetStringOrEmptyAsDate);

            Mapper.CreateMap<CompetitiveGroupDto, CompetitiveGroup>()
                .ForMember(d => d.CompetitiveGroupID, opt => opt.Ignore());

			Mapper.CreateMap<AdmissionVolumeDto, AdmissionVolume>()
				.ForMember(d => d.AdmissionItemTypeID, opt => opt.MapFrom(src => src.EducationLevelID))
				.ForMember(d => d.NumberBudgetO, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberBudgetO) ? "0" : src.NumberBudgetO))
				.ForMember(d => d.NumberBudgetOZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberBudgetOZ) ? "0" : src.NumberBudgetOZ))
				.ForMember(d => d.NumberBudgetZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberBudgetZ) ? "0" : src.NumberBudgetZ))
				.ForMember(d => d.NumberPaidO, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberPaidO) ? "0" : src.NumberPaidO))
				.ForMember(d => d.NumberPaidOZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberPaidOZ) ? "0" : src.NumberPaidOZ))
				.ForMember(d => d.NumberPaidZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberPaidZ) ? "0" : src.NumberPaidZ))
				.ForMember(d => d.NumberTargetO, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberTargetO) ? "0" : src.NumberTargetO))
				.ForMember(d => d.NumberTargetOZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberTargetOZ) ? "0" : src.NumberTargetOZ))
				.ForMember(d => d.NumberTargetZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberTargetZ) ? "0" : src.NumberTargetZ))
                .ForMember(d => d.ParentDirectionID, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.ParentDirectionID) ? null : src.ParentDirectionID));
			
            Mapper.CreateMap<CompetitiveGroupItemDto, CompetitiveGroupItem>()
				.ForMember(d => d.NumberBudgetO, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberBudgetO) ? "0" : src.NumberBudgetO))
				.ForMember(d => d.NumberBudgetOZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberBudgetOZ) ? "0" : src.NumberBudgetOZ))
				.ForMember(d => d.NumberBudgetZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberBudgetZ) ? "0" : src.NumberBudgetZ))
				.ForMember(d => d.NumberPaidO, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberPaidO) ? "0" : src.NumberPaidO))
				.ForMember(d => d.NumberPaidOZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberPaidOZ) ? "0" : src.NumberPaidOZ))
				.ForMember(d => d.NumberPaidZ, opt => opt.MapFrom(src => String.IsNullOrEmpty(src.NumberPaidZ) ? "0" : src.NumberPaidZ))
				.ForMember(d => d.CompetitiveGroupItemID, opt => opt.Ignore());
			
            Mapper.CreateMap<CompetitiveGroupTargetDto, CompetitiveGroupTarget>()
				.ForMember(d => d.Name, opt => opt.MapFrom(src => src.TargetOrganizationName));

		    Mapper.CreateMap<CompetitiveGroupTargetItemDto, CompetitiveGroupTargetItem>()
		          .ForMember(d => d.UID, opt => opt.MapFrom(src => src.UID))
		          .ForMember(d => d.NumberTargetO, opt => opt.MapFrom(src => Convert.ToInt32(src.NumberTargetO)))
                  .ForMember(d => d.NumberTargetOZ, opt => opt.MapFrom(src => Convert.ToInt32(src.NumberTargetOZ)))
                  .ForMember(d => d.NumberTargetZ, opt => opt.MapFrom(src => Convert.ToInt32(src.NumberTargetZ)));

			
			Mapper.CreateMap<BenefitItemDto, BenefitItemC>()
				.ForMember(x => x.OlympicDiplomTypeID, opt => opt.MapFrom(src => src.OlympicDiplomTypesParsed))
				.ForMember(x => x.BenefitID, opt => opt.MapFrom(src => src.BenefitKindID))
                .ForMember(x => x.OlympicYear, opt => opt.MapFrom(src => src.OlympicYear))
				.ForMember(x => x.IsForAllOlympic, opt => opt.MapFrom(src => src.IsForAllOlympics))
                .ForMember(x => x.EgeMinValue, opt => opt.MapFrom(src => src.MinEgeMark));
			
            //Mapper.CreateMap<AdmissionVolumeDto, AdmissionVolume>()
            //    .ForMember(x => x.AdmissionItemTypeID, opt => opt.MapFrom(src => src.EducationLevelID));
			
            Mapper.CreateMap<EntranceTestItemDto, EntranceTestItemC>()
				.ForMember(x => x.SubjectID, opt => opt.MapFrom(x =>
					String.IsNullOrEmpty(x.EntranceTestSubject.SubjectID) ? null : x.EntranceTestSubject.SubjectID))
				.ForMember(x => x.SubjectName, opt => opt.MapFrom(x => x.EntranceTestSubject.SubjectName))
				.ForMember(x => x.MinScore, opt => opt.MapFrom(x => Convert.ToDecimal(x.MinScore, CultureInfo.InvariantCulture)))
                .ForMember(x => x.EntranceTestPriority, opt => opt.MapFrom(x => string.IsNullOrEmpty(x.EntranceTestPriority) ? (int?)null : Convert.ToInt32(x.EntranceTestPriority)))
                .ForMember(x => x.IsFirst, opt => opt.MapFrom(x => x.IsFirst));

			Mapper.CreateMap<ApplicationDto, Application>();
			Mapper.CreateMap<EntrantDto, Entrant>();
			Mapper.CreateMap<AddressDto, Address>()
				.ForMember(x => x.RegionID, x => x.Ignore())
				.ForMember(x => x.CountryID, x => x.Ignore());

			Mapper.CreateMap<ApplicationDocumentDto, EntrantDocument>();
            Mapper.CreateMap<OlympicTotalDocumentDto, OlympicTotalDocumentViewModel>()
                .ForMember(x => x.Subjects, opt => 
                    opt.MapFrom(y => y.Subjects.Select(x => 
                    new OlympicTotalDocumentViewModel.SubjectBriefData { SubjectID = x.SubjectID.To(0, null, null, false) }).ToArray()));
			
            Mapper.CreateMap<OlympicDocumentDto, OlympicDocumentViewModel>();
            Mapper.CreateMap<SubjectBriefDataDto, OlympicTotalDocumentViewModel.SubjectBriefData>();
            Mapper.CreateMap<EduCustomDocumentDto, EduCustomDocumentViewModel>();

            Mapper.CreateMap<ApplicationDocumentDto, DiplomaDocumentViewModel>()
				.ForMember(x => x.SpecialityTypeID, opt => opt.MapFrom(x => x.SpecialityID ?? "0"))
				.ForMember(x => x.SpecializationTypeID, opt => opt.MapFrom(x => x.SpecializationID ?? "0"))
				.ForMember(x => x.QualificationTypeID, opt => opt.MapFrom(x => (x.QualificationTypeID ?? "0")));
			
            Mapper.CreateMap<BasicDiplomaDocumentDto, BasicDiplomaDocumentViewModel>()
				.ForMember(x => x.ProfessionTypeID, opt => opt.MapFrom(x => x.ProfessionID ?? "0"))
				.ForMember(x => x.QualificationTypeID, opt => opt.MapFrom(x => (x.QualificationTypeID ?? "0")));
            
            Mapper.CreateMap<MilitaryCardDocumentDto, MilitaryCardDocumentViewModel>();
            Mapper.CreateMap<StudentDocumentDto, StudentDocumentViewModel>();

            Mapper.CreateMap<AllowEducationDocumentDto, PsychoDocumentViewModel>();
			Mapper.CreateMap<ApplicationDocumentDto, PsychoDocumentViewModel>();
            Mapper.CreateMap<CustomDocumentDto, CustomDocumentViewModel>();
			Mapper.CreateMap<EgeDocumentWithSubjectsDto, EntrantDocument>();
			Mapper.CreateMap<EgeDocumentWithSubjectsDto, EGEDocumentViewModel>();
			Mapper.CreateMap<SubjectDataDto, EGEDocumentViewModel.SubjectData>();
			Mapper.CreateMap<GiaDocumentWithSubjectsDto, EntrantDocument>();
			Mapper.CreateMap<GiaDocumentWithSubjectsDto, GiaDocumentViewModel>();
			Mapper.CreateMap<SubjectDataDto, GiaDocumentViewModel.SubjectData>();
			
            Mapper.CreateMap<IdentityDocumentDto, IdentityDocumentViewModel>()
                .ForMember(x => x.GenderTypeID, opt => opt.MapFrom(x => x.GenderTypeID ?? "0"));
			
            Mapper.CreateMap<IdentityDocumentDto, EntrantDocument>();
            Mapper.CreateMap<DisabilityDocumentDto, DisabilityDocumentViewModel>();
            Mapper.CreateMap<SchoolCertificateDocumentDto, SchoolCertificateDocumentViewModel>();
            Mapper.CreateMap<ApplicationDocumentDto, OlympicDocumentViewModel>();
            
            Mapper.CreateMap<ApplicationDocumentDto, BasicDiplomaDocumentViewModel>()
                .ForMember(x => x.ProfessionTypeID, opt => opt.MapFrom(x => x.ProfessionID ?? "0"))
                .ForMember(x => x.QualificationTypeID, opt => opt.MapFrom(x => (x.QualificationTypeID ?? "0")));
            
            Mapper.CreateMap<ApplicationDocumentDto, CustomDocumentViewModel>();
            Mapper.CreateMap<ApplicationDocumentDto, MilitaryCardDocumentViewModel>();
            Mapper.CreateMap<ApplicationDocumentDto, DisabilityDocumentViewModel>();
            Mapper.CreateMap<ApplicationDocumentDto, SchoolCertificateDocumentViewModel>();
            Mapper.CreateMap<PostGraduateDiplomaDocumentDto, DiplomaDocumentViewModel>()
                .ForMember(x => x.SpecialityTypeID, opt => opt.MapFrom(x => x.SpecialityID ?? "0"))
                .ForMember(x => x.SpecializationTypeID, opt => opt.MapFrom(x => x.SpecializationID ?? "0"))
                .ForMember(x => x.QualificationTypeID, opt => opt.MapFrom(x => (x.QualificationTypeID ?? "0")));
            Mapper.CreateMap<PhDDiplomaDocumentDto, DiplomaDocumentViewModel>()
                .ForMember(x => x.SpecialityTypeID, opt => opt.MapFrom(x => x.SpecialityID ?? "0"))
                .ForMember(x => x.SpecializationTypeID, opt => opt.MapFrom(x => x.SpecializationID ?? "0"))
                .ForMember(x => x.QualificationTypeID, opt => opt.MapFrom(x => (x.QualificationTypeID ?? "0")));
        }


        /// <summary>
        /// Берём обработчик пакета по типу пакета
        /// </summary>
        public static PackageHandler GetPackageProcessor(ImportPackage package)
		{
			switch ((PackageType)package.TypeID)
			{
                case PackageType.Import:
                case PackageType.ImportApplicationSingle:
                    return new ImportPackageHandler(package);
                case PackageType.Delete:
                    return new DeletePackageHandler(package);
                default:
					throw new Exception("Не найден пакет указанного типа TypeID");
			}
		}

		/// <summary>
		/// Создание нового пакета
		/// </summary>
		public static int CreatePackage(string packageXml, int institutionID, PackageType packageType, string login)
		{
            return CreatePackage(packageXml, institutionID, packageType, login, null, PackageStatus.PlacedInQueue);
		}

        public static bool IsPackageProcessed(int packageId)
	    {
	        return PackageRepository.IsPackageProcessed(packageId);
	    }

	    /// <summary>
		/// Создание нового пакета
		/// </summary>
		public static int CreatePackage(string packageXml, int institutionId, PackageType packageType, string login, string comment, PackageStatus status)
		{
            return InsertImportPackage(packageXml, institutionId, packageType, login, comment, status, null);

            //ImportPackage importPackage = new ImportPackage
            //{
            //    PackageData = packageXml,
            //    TypeID = (int)packageType,
            //    StatusID = (int)status,
            //    Comment = comment,
            //    InstitutionID = institutionId,
            //    CreateDate = DateTime.Now,
            //    UserLogin = login
            //};
            //// Roman.N.Bukin  Оптимизация текущего импорта: для записей с типом импорт (type=1) CheckStatusId ставится = 1
            //if (packageType == PackageType.Import || packageType == PackageType.ImportApplicationSingle)
            //{
            //    importPackage.CheckStatusID = (int)PackageStatus.PlacedInQueue;
            //}

            //PackageRepository.SavePackage(importPackage);
            //return importPackage.PackageID;
		}

        public static int InsertImportPackage(string packageXml, int institutionId, PackageType packageType, string login, string comment, PackageStatus status, string packageResult)
        {
            var content = GVUZ.ServiceModel.Import.Core.Packages.Repositories.DbPackageRepository.GetContent((int)packageType, packageXml);

            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
  INSERT INTO [dbo].[ImportPackage]
           ([InstitutionID]
           ,[TypeID]
           ,[CreateDate]
           ,[LastDateChanged]
           ,[StatusID]
           ,[Comment]
           ,[PackageData]
           ,[ProcessResultInfo]
           ,[CheckStatusID]
           ,[CheckResultInfo]
           ,[ImportedAppIDs]
           ,[UserLogin]
           ,[Content])
     VALUES
           (@institutionId
           ,@typeId
           ,GETDATE()
           ,GETDATE()
           ,@statusId
           ,@comment
           ,@packageData
           ,@processResultInfo
           ,null
           ,null
           ,null
           ,@login
           ,@content);
SELECT CAST(scope_identity() AS int);";        

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 60000; // минуты хватит?

                    cmd.Parameters.Add(new SqlParameter("@institutionId", institutionId));
                    cmd.Parameters.Add(new SqlParameter("@typeId", (int)packageType));
                    cmd.Parameters.Add(new SqlParameter("@statusId", (int)status));
                    cmd.Parameters.Add(new SqlParameter("@comment", string.IsNullOrEmpty(comment) ? (object)DBNull.Value : comment));
                    cmd.Parameters.Add(new SqlParameter("@packageData", packageXml));
                    cmd.Parameters.Add(new SqlParameter("@login", login));
                    cmd.Parameters.Add(new SqlParameter("@processResultInfo", string.IsNullOrEmpty(packageResult) ? (object)DBNull.Value : packageResult));
                    cmd.Parameters.Add(new SqlParameter("@content", content));

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

		/// <summary>
		/// Создание нового валидационного пакета
		/// </summary>
		public static ImportPackage CreateValidationPackage(string packageXml)
		{
			int typeID;
			XElement rootElement = XElement.Parse(packageXml);
			switch (rootElement.Name.LocalName.ToLower())
			{
				case "packagedata":
					typeID = (int)PackageType.Import;
					break;
				case "datafordelete":
					typeID = (int)PackageType.Delete;
					break;
				case "getresultcheckapplication":
					typeID = (int)PackageType.ApplicationCheckResult;
					break;
				default:
					return null;
			}
			
			ImportPackage importPackage = new ImportPackage
			{
				PackageData = packageXml,
				TypeID = typeID,
				StatusID = (int)PackageStatus.None
			};
			
			return importPackage;
		}

		/// <summary>
		/// Создание нового информационного пакета (для логов)
		/// </summary>
		public static void CreateInfoPackage(string packageXml, int institutionId, PackageType packageType, string login, string comment, string packageResult)
		{
            if (institutionId == 0 && string.IsNullOrEmpty(packageXml)) return; //нет института, не можем создавать
            InsertImportPackage(packageXml, institutionId, packageType, login, comment, PackageStatus.Processed, packageResult);
            
            //var importPackage = new ImportPackage
            //{
            //    PackageData = packageXml,
            //    TypeID = (int)packageType,
            //    StatusID = (int)PackageStatus.Processed,
            //    Comment = comment,
            //    InstitutionID = institutionID,
            //    ProcessResultInfo = packageResult,
            //    CreateDate = DateTime.Now,
            //    UserLogin = login
            //};
            //PackageRepository.SavePackage(importPackage);
		}
	}
}
