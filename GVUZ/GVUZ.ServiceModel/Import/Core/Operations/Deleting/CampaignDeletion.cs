using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Удаление кампаний. Недописано. Неоотестировано.
	/// </summary>
	public class CampaignDeletion : ObjectDeletion
	{
		private readonly Campaign _campaign;
		public CampaignDeletion(StorageManager storageManager, Campaign campaign) : base(storageManager)
		{
			_campaign = campaign;
		}

		public override int GetDbObjectID()
		{
			return _campaign.CampaignID;
		}

		public override object GetDbObject()
		{
			return _campaign;
		}

		public override bool IsValidExtraConditions()
		{
			//нельзя удалять кампании, если есть КГ
			CompetitiveGroup[] linkedCompetitiveGroups = ObjectLinkManager.CampaignLinkWithCompetitiveGroups(_campaign);
			var linkedApps = ObjectLinkManager.CampaignLinkWithOrderOfAdmissions(_campaign)
                .Select(c => new ApplicationShortRef { ApplicationNumber = c.ApplicationNumber, RegistrationDateDate = c.RegistrationDate }).ToArray();
			if (linkedCompetitiveGroups.Length == 0 && linkedApps.Length == 0) return true;

			if(linkedCompetitiveGroups.Length > 0)
				ConflictStorage.AddCompetitiveGroups(_campaign, new HashSet<int>(linkedCompetitiveGroups.Select(x => x.CompetitiveGroupID)));

			foreach (var app in linkedApps)
				ConflictStorage.AddOrdersOfAdmission(_campaign, app);
				
			return false;
		}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				DeleteStorage.AddCampaign(_campaign);
				foreach (ObjectDeletion objectDeletion in DependedAndLinkedObjectsDeletionList)
					objectDeletion.TryDelete();

				return true;
			}

			return false;
		}
	}
}
