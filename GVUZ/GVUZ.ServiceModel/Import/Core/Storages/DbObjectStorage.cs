using System;
using System.Collections.Generic;
using GVUZ.Model;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;

namespace GVUZ.ServiceModel.Import.Core.Storages
{
	public class DbObjectStorage
	{
		private readonly ImportEntities _importEntities;

		public List<AdmissionVolume> AdmissionVolumes = new List<AdmissionVolume>();
		public List<Campaign> Campaigns = new List<Campaign>();
		public List<CompetitiveGroup> CompetitiveGroups = new List<CompetitiveGroup>();
		public List<CompetitiveGroupItem> CompetitiveGroupItems = new List<CompetitiveGroupItem>();
		public List<CompetitiveGroupTarget> CompetitiveGroupTargets = new List<CompetitiveGroupTarget>();
		public List<CompetitiveGroupTargetItem> CompetitiveGroupTargetItems = new List<CompetitiveGroupTargetItem>();
		public List<BenefitItemC> CompetitiveGroupBenefitItems = new List<BenefitItemC>();
		public List<EntranceTestItemC> EntranceTestItems = new List<EntranceTestItemC>();
		public List<BenefitItemCOlympicType> BenefitItemCOlympicTypes = new List<BenefitItemCOlympicType>();
		public List<ApplicationEntranceTestDocument> AppEntranceTestDocumentItems = new List<ApplicationEntranceTestDocument>();

		public List<Application> Applications = new List<Application>();
		public List<OrderOfAdmission> OrdersOfAdmission = new List<OrderOfAdmission>();

		private Dictionary<Type, HashSet<string>> _addedDbObjects = new Dictionary<Type, HashSet<string>>();

		public void AddObject<T>(T obj) where T : class, IObjectWithUID 
		{
			if (obj == null || obj.UID == null) return;
			HashSet<string> list;
			if (!_addedDbObjects.TryGetValue(obj.GetType(), out list))
			{
				_addedDbObjects[obj.GetType()] = list = new HashSet<string>();
			}

			if (!list.Contains(obj.UID)) list.Add(obj.UID);
		}

		public bool ContainsObject<T>(string uid) where T : IObjectWithUID
		{
			HashSet<string> list;
			if (_addedDbObjects.TryGetValue(typeof(T), out list))
				return list.Contains(uid);
			return false;
		}

		public DbObjectStorage(DbObjectRepositoryBase dbObjectRepository)
		{
			_importEntities = dbObjectRepository.ImportEntities;
		}

		public void AddAdmissionVolume(AdmissionVolume avDb)
		{
			if (AdmissionVolumes.Contains(avDb)) return;
			AdmissionVolumes.Add(avDb);
			AddObject(avDb);
		}

		public void AddCampaign(Campaign cDb)
		{
			if (Campaigns.Contains(cDb)) return;
			Campaigns.Add(cDb);
			AddObject(cDb);
		}

		public void AddCompetitiveGroup(CompetitiveGroup competitiveGroup)
		{
			if (CompetitiveGroups.Contains(competitiveGroup)) return;
			CompetitiveGroups.Add(competitiveGroup);
			AddObject(competitiveGroup);
		}

		public void AddCompetitiveGroupItem(CompetitiveGroupItem cgItem)
		{
			if (CompetitiveGroupItems.Contains(cgItem)) return;
			CompetitiveGroupItems.Add(cgItem);
			AddObject(cgItem);
		}

		public void AddCompetitiveGroupTarget(CompetitiveGroupTarget cgTarget)
		{
			if (CompetitiveGroupTargets.Contains(cgTarget)) return;
			CompetitiveGroupTargets.Add(cgTarget);
			AddObject(cgTarget);
		}

		public void AddCompetitiveGroupTargetItem(CompetitiveGroupTargetItem competitiveGroupTargetItem)
		{
			if (CompetitiveGroupTargetItems.Contains(competitiveGroupTargetItem)) return;
			CompetitiveGroupTargetItems.Add(competitiveGroupTargetItem);
			AddObject(competitiveGroupTargetItem);
		}

		public void AddCompetitiveGroupBenefitItem(BenefitItemC cgBenefitItem)
		{
			if (CompetitiveGroupBenefitItems.Contains(cgBenefitItem)) return;
			CompetitiveGroupBenefitItems.Add(cgBenefitItem);
			AddObject(cgBenefitItem);
		}

		public void AddEntranceTestItem(EntranceTestItemC entranceTestItem)
		{
			if (EntranceTestItems.Contains(entranceTestItem)) return;
			EntranceTestItems.Add(entranceTestItem);
			AddObject(entranceTestItem);
		}

		public void AddEntranceTestItem(ApplicationEntranceTestDocument entranceTestDocument)
		{
			if (AppEntranceTestDocumentItems.Contains(entranceTestDocument)) return;
			AppEntranceTestDocumentItems.Add(entranceTestDocument);
			AddObject(entranceTestDocument);
		}

		public void AddApplication(Application application)
		{
			if (Applications.Contains(application)) return;
			Applications.Add(application);
			AddObject(application);
		}

		public ImportEntities ImportEntities
		{
			get { return _importEntities; }
		}
	}
}
