using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.Model.Campaigns;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.DAL.Dapper.Model.LevelBudgets;
using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dto;
using GVUZ.DAL.Dapper.ViewModel.Olympic;

namespace GVUZ.DAL.Dapper.Repository.Model.Dictionary
{
    public class DictionaryRepository: GvuzRepository, IDictionaryRepository
    {
        private static IEnumerable<CampaignStatusView> _campaignStatuses = null;
        private static IEnumerable<CampaignTypesView> _campaignTypes = null;
        private static IEnumerable<EduLevelsToCampaignTypesView> _eduLevelsToCampaignTypes = null;

        public static IEnumerable<AdmissionItemTypeView> _educationLevels = null;
        public static IEnumerable<AdmissionItemTypeView> _educationForms = null;
        public static IEnumerable<AdmissionItemTypeView> _educationFinanceSources = null;

        public static string _constDescription = null;

        public static IEnumerable<LevelBudget> _levelBudget = null;
        public static IEnumerable<SubjectView> _subjects = null;

        private static volatile List<SimpleDto> _ugsList = null;
        private static volatile List<SimpleDto> _eduLevelsList = null;
        //private static readonly string connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
        //private static BaseRepository baseRepository = new BaseRepository(connectionString);

        private static volatile List<SimpleDto> _olympicLevels = null;
        private static volatile List<SimpleDto> _olympicDiplomTypes = null;
        private static volatile List<SimpleDto> _olympicProfiles = null;

        private static List<BenefitViewModel> _benefits = null;

        private static List<GlobalMinEge> _globalMinEge = null;

        // Признак, что "вуз МВД" - определяется как Institution.FounderEsrpOrgId = 9549
        private static Dictionary<int, int> _isMvd = new Dictionary<int, int>();

        public DictionaryRepository() : base()
        {

        }

        public IEnumerable<CampaignStatusView> GetCampaignStatus()
        {
            if (_campaignStatuses == null)
            {
                _campaignStatuses = DbConnection(db =>
                    {
                       return db.Query<CampaignStatusView>(sql: SQLQuery.GetCampaignStatus);
                    });
            }
            return _campaignStatuses;
        }
        public IEnumerable<CampaignTypesView> GetCampaignTypes()
        {
            if (_campaignTypes == null)
            {
                _campaignTypes = DbConnection(db => { return db.Query<CampaignTypesView>(sql: SQLQuery.GetCampaignTypes); });
            }
            return _campaignTypes;
        }

        public IEnumerable<EduLevelsToCampaignTypesView> GetEduLevelsToCampaignTypes()
        {
            if (_eduLevelsToCampaignTypes == null)
            {
                _eduLevelsToCampaignTypes = DbConnection(db => { return db.Query<EduLevelsToCampaignTypesView>(sql: SQLQuery.GetEduLevelsToCampaignTypes); });
            }
            return _eduLevelsToCampaignTypes;
        }

        private void LoadAdmissionItemTypes()
        {
            var admissionItemTypes = DbConnection(db => { return db.Query<AdmissionItemTypeView>(sql: SQLQuery.GetAdmissionItemType); });
            _educationLevels = admissionItemTypes.Where(t => t.ItemLevel == AdmissionItemTypeView.EducationLevelType);
            _educationForms = admissionItemTypes.Where(t => t.ItemLevel == AdmissionItemTypeView.EducationFormType);
            _educationFinanceSources = admissionItemTypes.Where(t => t.ItemLevel == AdmissionItemTypeView.EducationFinanceSourceType);
        }
        public IEnumerable<AdmissionItemTypeView> GetEducationLevels()
        {
            if (_educationLevels == null)
                LoadAdmissionItemTypes();
            return _educationLevels;
        }
        public IEnumerable<AdmissionItemTypeView> GetEducationForms()
        {
            if (_educationForms == null)
                LoadAdmissionItemTypes();
            return _educationForms;
        }
        public IEnumerable<AdmissionItemTypeView> GetEducationFinanceSources()
        {
            if (_educationFinanceSources == null)
                LoadAdmissionItemTypes();
            return _educationFinanceSources;
        }
        public string GetConstDescription(short value)
        {
            if (_constDescription == null)
            {
                var val = DbConnection(db =>
                {
                    return db.Query<AdmissionItemTypeView>(sql: SQLQuery.GetAdmissionItemType); 
                }).FirstOrDefault(x => x.ItemTypeID == value);
                _constDescription = val == null ? string.Empty : val.Name;
            }
            return _constDescription;
        }
        public IEnumerable<LevelBudget> GetLevelBudget()
        {
            if (_levelBudget == null)
            {
                _levelBudget = DbConnection(db => {
                    return db.Query<LevelBudget>(sql: "SELECT * FROM LevelBudget AS lb");
                });
            }
            return _levelBudget;
        }

        public IEnumerable<SubjectView> GetSubjects()
        {
            if (_subjects == null)
            {
                _subjects = DbConnection(db => {
                    return db.Query<SubjectView>(sql: SQLQuery.GetSubjectsWithMinValue);
                });
            }
            return _subjects;
        }

        /// <summary>
        /// Получение простого списка укрупненных групп специальностей (УГС)
        /// </summary>
        /// <returns>Список УГС упорядоченный по названию</returns>
        public List<SimpleDto> GetUgsList()
        {
            return _ugsList ?? (_ugsList = (WithTransaction(tx => {
                return tx.Query<SimpleDto>(SQLQuery.GetSimpleUgsList).ToList();
            })));

        }

        /// <summary>
        /// Получение простого списка уровней образования
        /// </summary>
        /// <returns>Список уровней, упорядоченный по DisplayOrder</returns>
        public List<SimpleDto> GetEducationLevelsList()
        {
            return _eduLevelsList ?? (_eduLevelsList = (WithTransaction(tx =>
            {
                return tx.Query<SimpleDto>(SQLQuery.GetSimpleEduLevelsList).ToList();
            })));

        }

        /// <summary>
        /// Получение простого списка уровней олимпиад
        /// </summary>
        public List<SimpleDto> GetOlympicLevels()
        {
            return _olympicLevels ?? (_olympicLevels = (WithTransaction(tx =>
            {
                return tx.Query<SimpleDto>(SQLQuery.GetOlympicLevels).ToList();
            })));
        }

        /// <summary>
        /// Получение простого списка профилей олимпиад
        /// </summary>
        public List<SimpleDto> GetOlympicProfiles()
        {
            return _olympicProfiles ?? (_olympicProfiles = (WithTransaction(tx =>
            {
                return tx.Query<SimpleDto>(SQLQuery.GetOlympicProfiles).ToList();
            })));
        }

        /// <summary>
        /// Получение простого списка типов дипломов олимпиад
        /// </summary>
        public List<SimpleDto> GetOlympicDiplomTypes()
        {
            return _olympicDiplomTypes ?? (_olympicDiplomTypes = (WithTransaction(tx =>
            {
                return tx.Query<SimpleDto>(SQLQuery.GetOlympicDiplomTypes).ToList();
            })));
        }


        /// <summary>
        /// Получение списка льгот
        /// </summary>
        public List<BenefitViewModel> GetBenefits()
        {
            return _benefits ?? (_benefits = (WithTransaction(tx =>
            {
                return tx.Query<BenefitViewModel>(SQLQuery.GetBenefits).ToList();
            })));
        }

        /// <summary>
        /// Получение списка льгот
        /// </summary>
        public List<GlobalMinEge> GetGlobalMinEge()
        {
            return _globalMinEge ?? (_globalMinEge = (WithTransaction(tx =>
            {
                return tx.Query<GlobalMinEge>(SQLQuery.GetGlobalMinEge).ToList();
            })));
        }


        /// <summary>
        /// Получение олимпиад
        /// </summary>
        public List<OlympicTypeViewModel> GetOlympicTypes()
        {
            return WithTransaction(tx =>
            {
                return tx.Query<OlympicTypeViewModel>(SQLQuery.GetOlympicTypes).ToList();
            });
        }

        

        public int IsMVD(int institutionID)
        {
            if (!_isMvd.ContainsKey(institutionID))
            {
                var v = WithTransaction(tx =>
                {
                    return tx.Query<int>(SQLQuery.GetInstitutionFounderEsrpOrgId, new { institutionID }).FirstOrDefault();
                });
                _isMvd.Add(institutionID, v);
            }

            return _isMvd[institutionID];
        }

        /// <summary>
        /// Проверка ОО на принадлежность к МВД (хардкод ESRP-кода МВД)
        /// </summary>
        /// <param name="FounderEsrpOrgId"></param>
        /// <returns>true, если это ВУЗ МВД </returns>
        public bool CheckIsMVD(int FounderEsrpOrgId)
        {
            // id == 9549
            return FounderEsrpOrgId == 9549;

            // Доработку откатываем, РОН не согласовал...
            // return false;
        }
    }
}
