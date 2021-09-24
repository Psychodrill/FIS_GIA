using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Storages;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Удаление объёма приёма
	/// </summary>
	public class AdmissionVolumeDeletion : ObjectDeletion
	{
		private readonly AdmissionVolume _admissionVolume;		

		public AdmissionVolumeDeletion(StorageManager storageManager, AdmissionVolume admissionVolume) : base(storageManager)
		{
			_admissionVolume = admissionVolume;
		}

		protected override void FillDeletionList()
		{
			// К ОП привязаны направления в КГ
			foreach (CompetitiveGroupItem cgItem in ObjectLinkManager.AdmissionVolumeLinkWithCompetitiveGroupItem(_admissionVolume))
				DependedAndLinkedObjectsDeletionList.Add(new CompetitiveGroupItemDeletion(StorageManager, cgItem));
		}

        //public override bool IsValidExtraConditions()
        //{
        //    //если есть направления в КГ - удалять нельзя
        //    CompetitiveGroupItem[] linkedCompetitiveGroupItems = ObjectLinkManager.AdmissionVolumeLinkWithCompetitiveGroupItem(_admissionVolume);
        //    if (linkedCompetitiveGroupItems.Length == 0) return true;

        //    ConflictStorage.AddCompetitiveGroupItems(_admissionVolume, 
        //        new HashSet<int>(linkedCompetitiveGroupItems.Select(x => x.CompetitiveGroupItemID)));
        //    return false;
        //}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				DeleteStorage.AddAdmissionVolume(_admissionVolume);
				foreach (ObjectDeletion objectDeletion in DependedAndLinkedObjectsDeletionList)
					objectDeletion.TryDelete();

				return true;
			}			

			return false;
		}

		public override int GetDbObjectID()
		{
			return _admissionVolume.AdmissionVolumeID;
		}

		public override object GetDbObject()
		{
			return _admissionVolume;
		}
	}
}
