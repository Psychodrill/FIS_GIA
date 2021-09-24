using GVUZ.DAL.Dapper.Model.Dictionary;
using GVUZ.DAL.Dapper.Model.LevelBudgets;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.DAL.Dapper.ViewModel.Olympic;
using GVUZ.DAL.Dto;
using System.Collections.Generic;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary
{
    public interface IDictionaryRepository
    {
        IEnumerable<CampaignStatusView> GetCampaignStatus();
        IEnumerable<CampaignTypesView> GetCampaignTypes();
        IEnumerable<EduLevelsToCampaignTypesView> GetEduLevelsToCampaignTypes();
        string GetConstDescription(short value);
        IEnumerable<LevelBudget> GetLevelBudget();

        IEnumerable<SubjectView> GetSubjects();

        /// <summary>
        /// Получение простого списка укрупненных групп специальностей (УГС)
        /// </summary>
        /// <returns>Список УГС упорядоченный по названию</returns>
        List<SimpleDto> GetUgsList();

        /// <summary>
        /// Получение простого списка уровней образования
        /// </summary>
        /// <returns>Список уровней, упорядоченный по DisplayOrder</returns>
        List<SimpleDto> GetEducationLevelsList();


        List<SimpleDto> GetOlympicLevels();
        List<SimpleDto> GetOlympicProfiles();
        List<SimpleDto> GetOlympicDiplomTypes();

        List<BenefitViewModel> GetBenefits();

        List<GlobalMinEge> GetGlobalMinEge();

        List<OlympicTypeViewModel> GetOlympicTypes();
     
        IEnumerable<AdmissionItemTypeView> GetEducationForms();
        IEnumerable<AdmissionItemTypeView> GetEducationFinanceSources();
    }
}
