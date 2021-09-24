using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.DAL.Dapper.Repository.Model;

namespace GVUZ.Model
{
    public static partial class DictionaryCache
    {
        public enum DictionaryTypeEnum
        {
            [Description("Уровень образования")]
            EducationLevel,
            [Description("Форма образования")]
            Study,
            [Description("Источник финансирования")]
            AdmissionType,
            [Description("Тип документа")]
            DocumentType,
            [Description("Страна")]
            CountryType,
            [Description("Тип документа удостоверяющий личтость")]
            IdentityDocumentType,

            Subject,
            OlympicLevel,
            ApplicationStatusType,
            DisabilityType,
            ForeignLanguageType,
            InstitutionDocumentType,
            OrderOfAdmissionStatus,
            OrderOfAdmissionType
        }

        static DictionaryCache()
        {
            FillActions[DictionaryTypeEnum.EducationLevel] = dictionary => FillAdmissionItemType(dictionary, 2);
            FillActions[DictionaryTypeEnum.Study] = dictionary => FillAdmissionItemType(dictionary, 7);
            FillActions[DictionaryTypeEnum.AdmissionType] = dictionary => FillAdmissionItemType(dictionary, 8);
            FillActions[DictionaryTypeEnum.DocumentType] = FillDocumentType;
            FillActions[DictionaryTypeEnum.CountryType] = FillCountryType;
            FillActions[DictionaryTypeEnum.IdentityDocumentType] = FillIdentityDocumentType;
            FillActions[DictionaryTypeEnum.Subject] = FillSubject;
            FillActions[DictionaryTypeEnum.OlympicLevel] = FillOlympicLevel;
            FillActions[DictionaryTypeEnum.ApplicationStatusType] = FillApplicationStatusType;
            FillActions[DictionaryTypeEnum.DisabilityType] = FillDisabilityType;
            FillActions[DictionaryTypeEnum.ForeignLanguageType] = FillForeignLanguageType;
            FillActions[DictionaryTypeEnum.OrderOfAdmissionStatus] = FillOrderOfAdmissionStatus;
            FillActions[DictionaryTypeEnum.OrderOfAdmissionType] = FillOrderOfAdmissionType;
        }

        static void FillDocumentType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.DocumentType
                                          select new { d.DocumentID, d.Name }))
                {
                    dictionary.Add(itemType.DocumentID, itemType.Name);
                }
            }
        }

        static void FillCountryType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.CountryType
                                          orderby d.DisplayOrder, d.Name
                                          select new { d.CountryID, d.Name })
                                          )
                {
                    dictionary.Add(itemType.CountryID, itemType.Name);
                }
            }
        }

        static void FillIdentityDocumentType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.IdentityDocumentType
                                          orderby d.IdentityDocumentTypeID
                                          select new { d.IdentityDocumentTypeID, d.Name })
                                          )
                {
                    dictionary.Add(itemType.IdentityDocumentTypeID, itemType.Name);
                }
            }
        }

        static void FillAdmissionItemType(OrderedDictionary dictionary, short level)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from p in entities.AdmissionItemType
                                          where p.ItemLevel == level
                                          orderby p.DisplayOrder, p.Name
                                          select new { p.ItemTypeID, p.Name }))
                {
                    dictionary.Add((int)itemType.ItemTypeID, itemType.Name);
                }
            }
        }

        static void FillSubject(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.Subject
                                          orderby d.SubjectID
                                          select new { d.SubjectID, d.Name}))
                {
                    dictionary.Add(itemType.SubjectID, itemType.Name);
                }
            }
        }

        static void FillOlympicLevel(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.OlympicLevel
                                          orderby d.Name
                                          select new { d.OlympicLevelID, d.Name }))
                {
                    dictionary.Add(itemType.OlympicLevelID, itemType.Name);
                }
            }
        }

        static void FillApplicationStatusType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.ApplicationStatusType
                                          orderby d.Name
                                          select new { d.StatusID, d.Name }))
                {
                    dictionary.Add(itemType.StatusID, itemType.Name);
                }
            }
        }

        static void FillDisabilityType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.DisabilityType
                                          orderby d.Name
                                          select new { d.DisabilityID, d.Name }))
                {
                    dictionary.Add(itemType.DisabilityID, itemType.Name);
                }
            }
        }

        static void FillForeignLanguageType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.ForeignLanguageType
                                          orderby d.Name
                                          select new { d.LanguageID, d.Name }))
                {
                    dictionary.Add(itemType.LanguageID, itemType.Name);
                }
            }
        }

        static void FillInstitutionDocumentType(OrderedDictionary dictionary)
        {
            using (var entities = new EntrantsEntities())
            {
                foreach (var itemType in (from d in entities.InstitutionDocumentType
                                          orderby d.Name
                                          select new { d.InstitutionDocumentTypeID, d.Name }))
                {
                    dictionary.Add(itemType.InstitutionDocumentTypeID, itemType.Name);
                }
            }
        }  

        static void FillOrderOfAdmissionStatus(OrderedDictionary dictionary)
        {
            foreach (var item in new OrderOfAdmissionRepository().OrderStatuses_GetAll())
            {
                dictionary.Add(item.Id, item.Name);
            } 
        }

        static void FillOrderOfAdmissionType(OrderedDictionary dictionary)
        {
            foreach (var item in new OrderOfAdmissionRepository().OrderTypes_GetAll())
            {
                dictionary.Add(item.Id, item.Name);
            }
        }        
    }
}
