using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	public class CompetitiveGroupBenefitItemDeletion : ObjectDeletion
	{
		public readonly BenefitItemC CompetitiveGroupBenefitItem;

		public CompetitiveGroupBenefitItemDeletion(StorageManager storageManager, BenefitItemC competitiveGroupBenefitItem) :
			base(storageManager)
		{
			CompetitiveGroupBenefitItem = competitiveGroupBenefitItem;			
		}

		public CompetitiveGroupBenefitItemDeletion(StorageManager storageManager, 
			BenefitItemC competitiveGroupBenefitItem, BaseDto baseDto) :
			base(storageManager, baseDto)
		{
			CompetitiveGroupBenefitItem = competitiveGroupBenefitItem;
		}

		// FillDeletionList реализует функционал описанный в данном методе
		public override bool IsValidExtraConditions()
		{
//            if (_notImportedDto != null)
//            {
///*
//                // если это льгота для ВИ и заявление включено в приказ, то удалить можно
//                if(CompetitiveGroupBenefitItem.EntranceTestItemID.HasValue)
//                {
//                    var application = DbObjectRepository.ApplicationEntranceTestDocuments
//                        .Where(x => x.EntranceTestItemID == CompetitiveGroupBenefitItem.EntranceTestItemID &&
//                                    x.Application.OrderOfAdmissionID.HasValue).Select(x => x.Application).SingleOrDefault();
//                    if (application != null)
//                    {
//                        ConflictStorage.AddOrdersOfAdmission(_notImportedDto, application.GetApplicationShortRef());
//                        return false;
//                    }

//                    return true;
//                }
//*/


//                // Находим РВИ как для общей льготы, так и льготы для испытания
//                var benefits = ObjectLinkManager.CompetitiveGroupCommonOrEntranceTestBenefitLinkWithAppEntranceTest(CompetitiveGroupBenefitItem);
//                var applications = new HashSet<ApplicationShortRef>();
//                applications.UnionWith(benefits.Select(c => new ApplicationShortRef 
//                    { ApplicationNumber = c.Application.ApplicationNumber, RegistrationDateDate = c.Application.RegistrationDate }));

//                // если не найдено связанных РВИ для общей льготы или льготы для ВИ, то удалить льготу можно.
//                if (benefits == null || benefits.Count == 0) return true;
//                if (!CompetitiveGroupBenefitItem.EntranceTestItemID.HasValue)
//                {
//                    ConflictStorage.AddApplicationCommonBenefits(_notImportedDto, benefits.ToArray());
//                    ConflictStorage.AddApplications(_notImportedDto, applications);
//                }
//                else
//                {
//                    ConflictStorage.AddEntranceTestResults(_notImportedDto, benefits.ToArray());
//                    ConflictStorage.AddApplications(_notImportedDto, applications);
//                }
//                return false;
//            }

			return true;
		}

		protected override void FillDeletionList()
		{
            //foreach (ApplicationEntranceTestDocument entranceTestDocument in
            //        ObjectLinkManager.CompetitiveGroupCommonOrEntranceTestBenefitLinkWithAppEntranceTest(CompetitiveGroupBenefitItem))
            //    DependedAndLinkedObjectsDeletionList.Add(new EntranceTestDocumentDeletion(StorageManager, entranceTestDocument));

            foreach (BenefitItemSubject subject in ObjectLinkManager.CommonBenefitLinkSubjects(CompetitiveGroupBenefitItem))
                DependedAndLinkedObjectsDeletionList.Add(new CommonBenefitSubjectDeletion(StorageManager, subject));
		}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				DeleteStorage.AddCompetitiveGroupBenefitItem(CompetitiveGroupBenefitItem);
				//удаляем все связанные документы
                bool result = true;
                foreach (ObjectDeletion deletion in DependedAndLinkedObjectsDeletionList)
                {
                    result = result && deletion.TryDelete();
                }

                return result;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return CompetitiveGroupBenefitItem.BenefitItemID;
		}

		public override object GetDbObject()
		{
			return CompetitiveGroupBenefitItem;
		}
	}
}
