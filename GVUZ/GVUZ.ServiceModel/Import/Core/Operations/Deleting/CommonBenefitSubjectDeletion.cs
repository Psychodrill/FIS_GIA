using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
    public class CommonBenefitSubjectDeletion: ObjectDeletion
    {
        private BenefitItemSubject CommonBenefitItemSubject;

        public CommonBenefitSubjectDeletion(StorageManager storageManager, BenefitItemSubject subject)
            : base(storageManager)
        {
            CommonBenefitItemSubject = subject;
            CanDeleteResult = true;
        }

        public CommonBenefitSubjectDeletion(StorageManager storageManager, BaseDto baseDto, BenefitItemSubject subject)
            : base(storageManager, baseDto)
        {
            CommonBenefitItemSubject = subject;
            CanDeleteResult = true;
        }

        public override int GetDbObjectID()
        {
            return CommonBenefitItemSubject.Id;
        }

        public override object GetDbObject()
        {
            return CommonBenefitItemSubject;
        }

        public override bool IsValidExtraConditions()
        {
            return true; // Это можно удалять всегда
        }

        public override bool TryDelete()
        {
            StorageManager.DeleteStorage.ImportEntities.BenefitItemSubject.DeleteObject(CommonBenefitItemSubject);

            try
            {
                StorageManager.DeleteStorage.ImportEntities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
