using System;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Model.Helpers;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core
{
    /// <summary>
    ///     Хранилище объектов импорта, отправленных на разные цели
    /// </summary>
    public abstract class StorageConsumer
    {
        private readonly InstitutionLogger _institutionLogger;

        public StorageManager StorageManager;

        protected StorageConsumer(StorageManager storageManager)
        {
            if (storageManager == null) throw new ArgumentNullException("storageManager");
            StorageManager = storageManager;
            _institutionLogger = new InstitutionLogger(DbObjectRepository.InstitutionId);
        }

        public DbObjectRepositoryBase DbObjectRepository
        {
            get { return StorageManager.DbObjectRepository; }
        }

        public DtoObjectStorage InsertStorage
        {
            get { return StorageManager.InsertStorage; }
        }

        public DbObjectStorage DeleteStorage
        {
            get { return StorageManager.DeleteStorage; }
        }

        public DtoObjectStorage UpdateStorage
        {
            get { return StorageManager.UpdateStorage; }
        }

        public ConflictStorage ConflictStorage
        {
            get { return StorageManager.ConflictStorage; }
        }

        public DtoObjectStorage ProcessedDtoStorage
        {
            get { return StorageManager.ProcessedDtoStorage; }
        }

        public InstitutionLogger Log
        {
            get { return _institutionLogger; }
        }

        public int InstitutionID
        {
            get { return StorageManager.InstitutionID; }
        }

        public void AddBenefitItemCOlympicTypeAndSetOlympicLevelFlags(ImportEntities _importEntities, BenefitItemC benefitItem, BenefitItemDto benefitItemDto)
        {
            if (benefitItemDto.Olympics != null)
            {
                benefitItem.OlympicLevelFlags = 0;
                foreach (string olympicID in benefitItemDto.Olympics)
                {
                    BenefitItemCOlympicType benefitItemCOlympicType = _importEntities.BenefitItemCOlympicType.CreateObject();
                    benefitItemCOlympicType.OlympicTypeID = olympicID.To(0);
                    benefitItem.BenefitItemCOlympicType.Add(benefitItemCOlympicType);
                    _importEntities.BenefitItemCOlympicType.AddObject(benefitItemCOlympicType);

                    var dbOlympic = DbObjectRepository.GetOlympicType(benefitItemCOlympicType.OlympicTypeID);
                    if (dbOlympic != null)
                    {
                        benefitItem.OlympicYear = dbOlympic.OlympicYear;
                        //уровни олимпиады, автоматически выводим
                        if (!benefitItemDto.IsForAllOlympics.To(false))
                        {
                            if (dbOlympic.OlympicLevelID.HasValue)
                                benefitItem.SetOlympicLevelFlags(dbOlympic.OlympicLevelID.Value);
                            else
                            {
                                // проставляем все уровни которые есть в предметах 
                                var subjects = DbObjectRepository.OlympicTypeSubjectLinks.Where(c => c.OlympicID == dbOlympic.OlympicID);
                                foreach (var otsl in subjects)
                                    benefitItem.SetOlympicLevelFlags(otsl.SubjectLevelID.To<short>(0));
                            }
                        }
                    }
                }
            }
            else if (benefitItemDto.OlympicsLevels != null)
            {
                benefitItem.OlympicLevelFlags = 0;
                foreach (OlympicDto olympic in benefitItemDto.OlympicsLevels)
                {
                    /* Если выбран признак "все олимпиады", то не обязательно передавать OlympicID, 
                       в этом случае передаваемый уровень присваивается всем олимпиадам, у которых он вообще возможен */
                    if (!benefitItemDto.IsForAllOlympics.To(false))
                    {
                        var benefitItemCOlympicType = _importEntities.BenefitItemCOlympicType.CreateObject();
                        benefitItemCOlympicType.OlympicTypeID = olympic.OlympicID.To(0);
                        benefitItem.BenefitItemCOlympicType.Add(benefitItemCOlympicType);
                        _importEntities.BenefitItemCOlympicType.AddObject(benefitItemCOlympicType);

                        /* Присваиваем переданный уровень */
                        var dbOlympic = DbObjectRepository.GetOlympicType(olympic.OlympicID.To(0));
                        if (dbOlympic != null)
                        {
                            benefitItem.OlympicYear = dbOlympic.OlympicYear;
                            var level = dbOlympic.OlympicLevelID.HasValue ? dbOlympic.OlympicLevelID.Value : olympic.LevelID.To<short>(0);
                            benefitItem.SetOlympicLevelFlags(level);
                        }
                    }
                    else if (!string.IsNullOrEmpty(olympic.LevelID))
                    {
                        benefitItem.SetOlympicLevelFlags(olympic.LevelID.To<short>(0));                        
                    }
                }
            }
        }

        public void AddMinEgeScoresForCommonBenefit(ImportEntities _importEntities, BenefitItemC benefitItem, BenefitItemDto benefitItemDto)
        {
            if (benefitItemDto.MinEgeMarks == null) // Ничего не передали - ничего и не делаем
                return;

            foreach (MinEgeScoresDto score in benefitItemDto.MinEgeMarks)
            {
                BenefitItemSubject subject = _importEntities.BenefitItemSubject.CreateObject();

                subject.BenefitItemId = benefitItem.BenefitItemID;
                subject.SubjectId = score.SubjectID.To(0);
                subject.EgeMinValue = score.MinMark.To(0);
                _importEntities.BenefitItemSubject.AddObject(subject);
            }
        }
    }

    public static partial class Extensions
    {
        public static void SetOlympicLevelFlags(this BenefitItemC benefitItem, short level)
        {
            short flags = 0;
            if (level == 2) flags |= 1;
            if (level == 3) flags |= 3;
            if (level == 4) flags |= 7;
            benefitItem.OlympicLevelFlags |= flags;
        }
    }
}