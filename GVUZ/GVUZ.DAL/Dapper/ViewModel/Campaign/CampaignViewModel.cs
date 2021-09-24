using GDDMC = GVUZ.DAL.Dapper.Model.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GVUZ.DAL.Dapper.Repository.Interfaces.Campaign;
using GVUZ.DAL.Dapper.Repository.Model.Campaigns;
using System.Configuration;
using System.Collections;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;

namespace GVUZ.DAL.Dapper.ViewModel.Campaign
{
    public class CampaignViewModel
    {
        public int? SortID { get; set; }
        public int? PageNumber { get; set; }
        public int TotalPageCount { get; set; }
        public int TotalItemCount { get; set; }
        public IEnumerable<CampaignTypesView> CampaignTypes { get; set; }
        public IEnumerable<CampaignDataModel> CampaignList { get; set; }
        public CampaignEditModel CampaignEdit { get; set; }
        public bool isCountCampaignType { get; set; }
        public class CampaignDataModel
        {
            [DisplayName("Действие")]
            public int CampaignID { get; set; }
            public int InstitutionID { get; set; }
            [DisplayName("Наименование")]
            [Required]
            [StringLength(100)]
            public string CampaignName { get; set; }
            public int YearStart { get; set; }
            public int YearEnd { get; set; }
            [DisplayName("Сроки проведения")]
            public string YearStartToEnd { get { return string.Format("{0} - {1}", YearStart, YearEnd); } }
            [DisplayName("Форма обучения")]
            public int EducationFormFlag { get; set; }
            public int StatusID { get; set; }
            public string UID { get; set; }
            public DateTime? CampaignCreatedDate { get; set; }
            public DateTime? CampaignModifiedDate { get; set; }
            public Guid? CampaignGUID { get; set; }
            public Int16 CampaignTypeID { get; set; }
            [DisplayName("Статус")]
            [StringLength(50)]
            public string CampaignStatusName { get; set; }
            public DateTime? CampaignStatusCreatedDate { get; set; }
            public DateTime? CampaignStatusModifiedDate { get; set; }
            [DisplayName("Тип приемной кампании")]
            [StringLength(100)]
            public string CampaignTypeName { get; set; }
            [DisplayName("Уровни образования")]
            public string LevelsEducation { get; set; }
            public bool? isPresentInLicense { get; set; }
        }

        public class CampaignEditModel : CampaignDataModel
        {
            public IEnumerable YearRange { get; set; }
            public IEnumerable<EduLevelsToCampaignTypesView> LevelsEducationNames { get; set; }
            public IEnumerable<GDDMC.CampaignEducationLevel> CampaignEducationLevel { get; set; }
            public bool CanEdit { get; set; }
            
            /// <summary>
            /// Можно ли изменить тип ПК (а также год)
            /// </summary>
            public bool CanChangeType { get; set; }
            /// <summary>
            /// Флаг - занятые формы (есть конкурс или объем приема)
            /// </summary>
            public int UsedEducationFormFlags { get; set; }
        }
        
        //public class CampaignStatusType
        //{
        //    public const int NotStart = 0;
        //    public const int Started = 1;
        //    public const int Finished = 2;
        //}
    }
    public class CampaignStatusType
    {
        public const int NotStart = 0;
        public const int Started = 1;
        public const int Finished = 2;
    }

}
