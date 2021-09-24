using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations
{
	/// <summary>
	/// Связанные объекты, которые анализируются при обновлении / удалении.
	/// </summary>
	public class ObjectLinkManager
	{
		private readonly DbObjectRepositoryBase _dbObjectRepo;		

		public ObjectLinkManager(StorageManager storageManager)
		{	
			_dbObjectRepo = storageManager.DbObjectRepository;			
		}

		/// <summary>
		/// К ОП привязаны направления в КГ
		/// </summary>
		public CompetitiveGroupItem[] AdmissionVolumeLinkWithCompetitiveGroupItem(AdmissionVolume admissionVolume)
		{
			return _dbObjectRepo.CompetitiveGroupItems.Where(x => 
                x.CompetitiveGroup.CampaignID == admissionVolume.CampaignID &&
                x.DirectionID == admissionVolume.DirectionID).ToArray();
		}

        public ApplicationShortRef[] OrderOfAdmissionApplicationsExists(AdmissionVolume admissionVolume)
        {
            return _dbObjectRepo.Applications.Where(x => x.OrderOfAdmissionId == admissionVolume.AdmissionVolumeID).ToArray();
        }

		/// <summary>
		/// К ПК привязаны КГ
		/// </summary>
		public CompetitiveGroup[] CampaignLinkWithCompetitiveGroups(Campaign campaign)
		{
			return _dbObjectRepo.CompetitiveGroups.Where(x => x.CampaignID == campaign.CampaignID).ToArray();
		}

		/// <summary>
		/// К ПК привязаны Приказы
		/// </summary>
		public IQueryable<Application> CampaignLinkWithOrderOfAdmissions(Campaign campaign)
		{
            return _dbObjectRepo.FindApplicationsByCampaign(campaign.CampaignID);
		}

		/// <summary>
		/// К КГ привязаны Заявления
		/// </summary>
		public IQueryable<Application> CompetitiveGroupLinkWithApplications(CompetitiveGroup cg)
		{
		    return _dbObjectRepo.FindApplicationsBySelectedCompetitiveGroup(cg.CompetitiveGroupID);
		}

	    public Application[] FindApplicationsByOrderCompetitiveGroup(CompetitiveGroup cg)
	    {
	        return _dbObjectRepo.FindApplicationsByOrderCompetitiveGroup(cg.CompetitiveGroupID).ToArray();
	    }

	    /// <summary>
		/// К НП (= 1 в КГ ) все заявления в КГ
		/// </summary>
		public Application[] LastCompetitiveGroupItemLinkWithApplicationsOnCompetitiveGroup(CompetitiveGroupItem cgItem)
		{
            CompetitiveGroup cg = _dbObjectRepo.GetCompetitiveGroupDictById(cgItem.CompetitiveGroupID);
			if (cg == null) return new Application[0];
            return _dbObjectRepo.FindApplicationsBySelectedCompetitiveGroup(cg.CompetitiveGroupID).ToArray();
		}

		/// <summary>
		/// К НП привязаны Заявления в Приказе.
		/// </summary>
		public Application[] CompetitiveGroupItemLinkWithAppsInOrder(CompetitiveGroupItem cgItem)
		{
            return _dbObjectRepo.FindApplicationsBySelectedCompetitiveGroupItem(cgItem.CompetitiveGroupItemID)
                .Where(c => c.OrderOfAdmissionID.HasValue).ToArray();
		}

		/// <summary>
		/// К НП целевого приема привязаны Заявления в Приказе
		/// </summary>
		public Application[] CompetitiveGroupTargetItemLinkWithAppsInOrder(CompetitiveGroupTargetItem cgTargetItem)
		{
            return _dbObjectRepo.FindApplicationsByOrderCompetitiveGroupItem(cgTargetItem.CompetitiveGroupItemID)
                .Where(c => c.OrderOfAdmissionID.HasValue).ToArray();
		}

		/// <summary>
		/// К ЦП привязаны заявления (в приказе и при подаче)
		/// </summary>
        public Application[] TargetOrganizationItemLinkWithAppsInOrder(CompetitiveGroupTarget cgt)
		{
			// 1. заявления, не включенные в приказ, но на которые абитуриент изъявил желание поступить
			// 2. заявления, включенные в приказ на целевой прием.
            return _dbObjectRepo.TargetOrganizationItemLinkWithAppsInOrder(cgt.CompetitiveGroupTargetID);
		}

		//К ВИ в КГ привязаны РВИ	
		public ApplicationEntranceTestDocument[] CompetitiveGroupEntranceTestLinkWithAppEntranceTest(
			EntranceTestItemC entranceTestItem)
		{
            return _dbObjectRepo.FindApplicationEntranceTestResultsByEntranceTestItem(entranceTestItem.EntranceTestItemID).ToArray();
		}

		/// <summary>
		/// К общей льготе в КГ привязаны РВИ (используется общая льгота в заявлении).
		/// К льготе для ВИ в КГ привязаны РВИ (используется льгота в заявлении)
		/// выводятся РВИ как для общей льготы, так и льготы для испытания.
		/// </summary>
		public List<ApplicationEntranceTestDocument> CompetitiveGroupCommonOrEntranceTestBenefitLinkWithAppEntranceTest(
			BenefitItemC benefitItem)
		{
			if (_dbObjectRepo.ApplicationEntranceTestBenefits == null) return new List<ApplicationEntranceTestDocument>();

			// это льгота для ВИ
			if (benefitItem.EntranceTestItemID.HasValue)
			{
                return _dbObjectRepo.FindApplicationEntranceTestResultsByEntranceTestItem(benefitItem.EntranceTestItemID.Value)
                    .Where(x => x.Application.ApplicationSelectedCompetitiveGroup.Any(y => y.CompetitiveGroupID == benefitItem.CompetitiveGroupID)).ToList();
			}

			// общая льгота
			return _dbObjectRepo.ApplicationEntranceTestBenefits.Where(x => 
                x.CompetitiveGroupID == benefitItem.CompetitiveGroupID && x.BenefitID.HasValue).ToList();
		}

        public List<BenefitItemSubject> CommonBenefitLinkSubjects(BenefitItemC benefitItem)
        {
            if (benefitItem.BenefitItemSubject.Count == 0) return new List<BenefitItemSubject>();

            return benefitItem.BenefitItemSubject.ToList();
        }

		//К Заявлениям привязаны Приказы
		public ApplicationShortRef ApplicationLinkWithOrder(
			Application application)
		{
			if (application == null) return null;
			if (application.OrderOfAdmissionID.HasValue)
				return new ApplicationShortRef
				       	{
				       		ApplicationNumber = application.ApplicationNumber,
							RegistrationDateDate = application.RegistrationDate
				       	};

			return null;
		}

		//К Заявлениям привязаны РВИ
		public ApplicationEntranceTestDocument[] ApplicationLinkWithEntranceTestResults(Application application)
		{
			if (application == null) return new ApplicationEntranceTestDocument[0];
		    return _dbObjectRepo.FindApplicationEntranceTestResultsByApplication(application.ApplicationID).ToArray();
		}

		//К Заявлениям привязаны общие льготы для РВИ
		public ApplicationEntranceTestDocument[] ApplicationLinkWithEntranceTestResultsCommonBenefit(Application application)
		{
			if (application == null) return new ApplicationEntranceTestDocument[0];
			if (_dbObjectRepo.ApplicationEntranceTestBenefits == null) return new ApplicationEntranceTestDocument[0];
			return _dbObjectRepo.ApplicationEntranceTestBenefits
				.Where(x => x.ApplicationID == application.ApplicationID &&
					!x.EntranceTestItemID.HasValue && !x.SourceID.HasValue).ToArray();
		}

		//К Диплому ОШ привязаны РВИ
		//К Док-ту основанию привязаны РВИ
		//К Свед. о предост. льготах привязаны РВИ
		//К РВИ привязаны Приказы
		public ApplicationShortRef ApplicationEntranceTestDocumentLinkWithOrder(
			ApplicationEntranceTestDocument applicationEntranceTestDocument)
		{
			return ApplicationLinkWithOrder(applicationEntranceTestDocument.Application);
		}
	}
}
