using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Удаление КГ
	/// </summary>
	public class CompetitiveGroupDeletion : ObjectDeletion
	{
		private readonly CompetitiveGroup _competitiveGroup;		

		public CompetitiveGroupDeletion(StorageManager storageManager, CompetitiveGroup competitiveGroup) :
			base(storageManager)
		{
			_competitiveGroup = competitiveGroup;
		}

		public CompetitiveGroupDeletion(StorageManager storageManager, CompetitiveGroup competitiveGroup, 
			CompetitiveGroupDto cgDto) : base(storageManager, cgDto)
		{
			_competitiveGroup = competitiveGroup;
		}

		protected override void FillDeletionList()
		{
			// связь с заявлениями в приказе - нельзя удалять если такие есть!!
            foreach (Application app in ObjectLinkManager.FindApplicationsByOrderCompetitiveGroup(_competitiveGroup))
                DependedAndLinkedObjectsDeletionList.Add(new ApplicationDeletion(StorageManager, app));
		}

        public override bool IsValidExtraConditions()
        {
            // К КГ привязаны Заявления
            var apps = ObjectLinkManager.CompetitiveGroupLinkWithApplications(_competitiveGroup).ToArray();
            if (apps.Length > 0)
            {
                ConflictStorage.AddApplications(GetDtoObject(), new HashSet<ApplicationShortRef>(apps.Select(x =>
                            new ApplicationShortRef { ApplicationNumber = x.ApplicationNumber, RegistrationDateDate = x.RegistrationDate })));
                ConflictStorage.AddConflictWithCustomMessage(
                    new CompetitiveGroupDto() { UID = _competitiveGroup.UID, Name = _competitiveGroup.Name},
                    new ConflictStorage.ConflictMessage()
                    {
                        Code = ConflictMessages.CompetitiveGroupCannotBeRemovedWithApplications,
                        Message = String.Format(
                        ConflictMessages.GetMessage(ConflictMessages.CompetitiveGroupCannotBeRemovedWithApplications),
                             string.Join(",", apps.Select(c => c.UID).ToArray()))
                    });
                return false;
            }

            return true;
        }

		public override bool TryDelete()
		{
			if (CanDelete()) //если можем удалить, добавляем все связанные объекты
			{
				DeleteStorage.AddCompetitiveGroup(_competitiveGroup);
				foreach (Application application in ObjectLinkManager.CompetitiveGroupLinkWithApplications(_competitiveGroup))
					DeleteStorage.AddApplication(application);
				foreach (BenefitItemC benefitItem in _competitiveGroup.BenefitItemC)
					DeleteStorage.AddCompetitiveGroupBenefitItem(benefitItem);
				foreach (var entranceTestItem in _competitiveGroup.EntranceTestItemC)
					DeleteStorage.AddEntranceTestItem(entranceTestItem);
			    foreach (var cgItem in _competitiveGroup.CompetitiveGroupItem)
			    {
			        DeleteStorage.AddCompetitiveGroupItem(cgItem);
			    }

			    //организации ЦП
				foreach (var competitiveGroupTarget in _competitiveGroup.CompetitiveGroupItem.SelectMany(x => x.CompetitiveGroupTargetItem)
					.Select(x => x.CompetitiveGroupTarget).Distinct())
				{
					//удаляем если только не осталось групп
					if(!competitiveGroupTarget.CompetitiveGroupTargetItem.Select(x => x.CompetitiveGroupItem.CompetitiveGroupID).ToArray()
						    .Except(DeleteStorage.CompetitiveGroups.Select(x => x.CompetitiveGroupID)).Any())
						DeleteStorage.AddCompetitiveGroupTarget(competitiveGroupTarget);
					// удалить направления целевого приема
					foreach (var competitiveGroupTargetItem in competitiveGroupTarget.CompetitiveGroupTargetItem)
						DeleteStorage.AddCompetitiveGroupTargetItem(competitiveGroupTargetItem);
				}

				return true;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return _competitiveGroup.CompetitiveGroupID;
		}

		public override object GetDbObject()
		{
			return _competitiveGroup;
		}
	}
}
