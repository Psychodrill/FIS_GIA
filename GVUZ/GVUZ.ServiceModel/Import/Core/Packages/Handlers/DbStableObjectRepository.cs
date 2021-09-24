using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using FogSoft.Helpers;
using GVUZ.Model.Entrants;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
	/// <summary>
	/// Хранилище стабильных объектов (справочников)
	/// </summary>
	public static class DbStableObjectRepository
	{
		private static DateTime _lastLoaded = DateTime.MinValue;

		public static ConcurrentDictionary<short, OlympicDiplomType> OlympicDiplomTypes;
		public static ConcurrentDictionary<int, OlympicType> OlympicTypes;
        
        
		public static ConcurrentDictionary<int, Direction> Directions;
		public static ConcurrentDictionary<short, Benefit> Benefits;
		public static ConcurrentDictionary<int, DocumentType> DocumentTypes;
		public static ConcurrentDictionary<int, InstitutionDocumentType> InstitutionDocumentTypes;
		public static ConcurrentDictionary<int, IdentityDocumentType> IdentityDocumentTypes;
		public static ConcurrentDictionary<short, EntranceTestType> EntranceTestTypes;
        public static ConcurrentDictionary<int, GenderType> GenderTypes;
		public static ConcurrentDictionary<int, DisabilityType> DisabilityTypes;
		public static ConcurrentDictionary<int, ApplicationStatusType> ApplicationStatusTypes;
		public static ConcurrentDictionary<int, Subject> Subjects;
		public static ConcurrentDictionary<int, SubjectEgeMinValue> SubjectEgeMinValues;
		public static ConcurrentDictionary<short, AdmissionItemType> AdmissionItemTypes;
		public static ConcurrentDictionary<int, RegionType> RegionTypes;
		public static ConcurrentDictionary<int, CountryType> CountryTypes;
		public static ConcurrentDictionary<int, EntranceTestResultSource> EntranceTestResultSources;
		
		public static List<DirectionSubjectLinkDirection> DirectionSubjectLinkDirections;
		public static List<DirectionSubjectLink> DirectionSubjectLinks;
		public static List<DirectionSubjectLinkSubject> DirectionSubjectLinkSubjects;
		public static List<EntranceTestCreativeDirection> EntranceTestCreativeDirections;
        public static List<OlympicTypeSubjectLink> OlympicTypeSubjectLinks;

        public static ConcurrentDictionary<int, LevelBudget> LevelBudgets;

		private static readonly int _threadCount;
		static DbStableObjectRepository()
		{
			_threadCount = ConfigurationManager.AppSettings["ImportProcessorThreadcount"].To(1);
		}

		private static readonly object _loadLocker = new object();

		public static void Load(ImportEntities dbContext)
		{
#warning Что за маразм!?
			// загружаем данные заново раз в 20 минут. Для маленьких пакетов всё ускорит, для больших
			// принципиальной разницы в загрузке нет, зато есть возможность обновить базу без
			// перезапуска приложения

			//большой и злобный лок, но если попадём на него, то лучше посидеть другим в нём, чем 2 раза грузить данные
			//а то что он всегда, по сравнению с остальными операциями — не страшно. 
			lock (_loadLocker)
			{
                if (_lastLoaded == DateTime.MinValue || DateTime.Now.Subtract(_lastLoaded).TotalMinutes > 20)
                {
					OlympicDiplomTypes = GenerateDictionary(dbContext.OlympicDiplomType, x => x.OlympicDiplomTypeID);
                    OlympicTypes = GenerateDictionary(dbContext.OlympicType, x => x.OlympicID);
    			    
					Directions = GenerateDictionary(dbContext.Direction, x => x.DirectionID);
					Benefits = GenerateDictionary(dbContext.Benefit, x => x.BenefitID);
					DocumentTypes = GenerateDictionary(dbContext.DocumentType, x => x.DocumentID);
					InstitutionDocumentTypes = GenerateDictionary(dbContext.InstitutionDocumentType, x => x.InstitutionDocumentTypeID);
					IdentityDocumentTypes = GenerateDictionary(dbContext.IdentityDocumentType, x => x.IdentityDocumentTypeID);
					EntranceTestTypes = GenerateDictionary(dbContext.EntranceTestType, x => x.EntranceTestTypeID);
					//GenderTypes = GenerateDictionary(dbContext.GenderType, x => x.GenderID);
					DisabilityTypes = GenerateDictionary(dbContext.DisabilityType, x => x.DisabilityID);
					ApplicationStatusTypes = GenerateDictionary(dbContext.ApplicationStatusType, x => x.StatusID);
					Subjects = GenerateDictionary(dbContext.Subject, x => x.SubjectID);
					SubjectEgeMinValues = GenerateDictionary(dbContext.SubjectEgeMinValue, x => x.SubjectID);
					AdmissionItemTypes = GenerateDictionary(dbContext.AdmissionItemType, x => x.ItemTypeID);
					RegionTypes = GenerateDictionary(dbContext.RegionType, x => x.RegionId);
					CountryTypes = GenerateDictionary(dbContext.CountryType, x => x.CountryID);
					EntranceTestResultSources = GenerateDictionary(dbContext.EntranceTestResultSource, x => x.SourceID);
				
					DirectionSubjectLinkDirections = dbContext.DirectionSubjectLinkDirection.Include(c => c.DirectionSubjectLink).ToList();
					DirectionSubjectLinks = dbContext.DirectionSubjectLink.ToList();
					DirectionSubjectLinkSubjects = dbContext.DirectionSubjectLinkSubject.ToList();
					EntranceTestCreativeDirections = dbContext.EntranceTestCreativeDirection.ToList();
                    OlympicTypeSubjectLinks = dbContext.OlympicTypeSubjectLink.ToList();
                    
                   
					_lastLoaded = DateTime.Now;
				}
			}
		}

		private static ConcurrentDictionary<TKey, TValue> GenerateDictionary<TKey, TValue>(IQueryable<TValue> objectSet, Func<TValue, TKey> selector) where TValue : class 
		{
			return new ConcurrentDictionary<TKey, TValue>(_threadCount, 
				objectSet.ToDictionary(selector, x => x),
				EqualityComparer<TKey>.Default);
		}
	}
}
