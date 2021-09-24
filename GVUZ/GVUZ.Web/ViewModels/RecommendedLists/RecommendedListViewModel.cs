using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels
{
    public class RecommendedListViewModel
    {
        #region Сырые данные из БД (Id, которые надо заменить на текстовые данные из специальных таблиц)
        public int RecListId { get; set; }
        public int ApplicationId { get; set; }
        public int InstitutionId { get; set; }
        public int CampaignId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int DirectionId { get; set; }
        public int EduLevelId { get; set; }
        public int EduFormId { get; set; }
        public int ApplicationStatus { get; set; } // Это нужно для включения в приказы и удаления из списка
        #endregion

        #region Отображаемые свойства

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("ФИО абитуриента")]
        public string EntrantName { get; set; }

        [DisplayName("Приёмная кампания")]
        public string CampaignName { get; set; }

        [DisplayName("Этап")]
        public int Stage { get; set; }

        [DisplayName("Уровень")]
        public string EduLevel { get; set; }

        [DisplayName("Форма обучения")]
        public string EduForm { get; set; }

        [DisplayName("Конкурс")]
        public string CompetitiveGroupName { get; set; }

        [DisplayName("Направление")]
        public string DirectionName { get; set; }

        [DisplayName("Сдал документы")]
        public string OriginalsReceived { get; set; }

        [DisplayName("Рейтинг")]
        public decimal Rating { get; set; }

        #endregion

        #region Вспомогательные данные
        /// <summary>
        /// Признак завершённости кампании
        /// </summary>
        public bool IsCampaignFinished { get; set; }

        /// <summary>
        /// Дата регистрации заявления (для выгрузки списка из UI)
        /// </summary>
        public DateTime AppRegistrationDate { get; set; }

        /// <summary>
        /// UID конкурсной группы (для выгрузки списка из UI)
        /// </summary>
        public string CompetitiveGroupUID { get; set; }
        #endregion
    }

    public class DropDownData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FilterDataViewModel
    {
        public DropDownData[] Campaigns { get; set; }
        public DropDownData[] EduLevels { get; set; }
        public DropDownData[] EduForms { get; set; }
        public DropDownData[] Stages { get; set; }
        public DropDownData[] OrigsReceived { get; set; }
        public DropDownData[] CompetitiveGroups { get; set; }
        public DropDownData[] Directions { get; set; }

        public FilterDataViewModel()
        {
            Stages = new DropDownData[3] 
                { 
                    new DropDownData() { Id = -1, Name = "Любой" }, 
                    new DropDownData() { Id = 1, Name = "Этап 1" }, 
                    new DropDownData() { Id = 2, Name = "Этап 2" } 
                };

            OrigsReceived = new DropDownData[3]
            {
                new DropDownData() { Id = -1, Name = "Не важно" },
                new DropDownData() { Id = 0, Name = "Нет" },
                new DropDownData() { Id = 1, Name = "Да" }
            };
        }
    }

    public class FilterValuesModel
    {
        public int CampaignId { get; set; }
        public int Stage { get; set; }
        public int Edulevel { get; set; }
        public int EduForm { get; set; }
        public int OriginalsReceived { get; set; }
        public int CompetitiveGroup { get; set; }
        public int Direction { get; set; }

        public string ApplicationNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
    }


    public class ListOfRecommendedListViewModel
    {
        public const int PAGESIZE = 25;

        public RecommendedListViewModel[] RecommendedLists;

        public int PageCount
        {
            get;
            set;
        }

        public RecommendedListViewModel ListHeader { get { return null; } }

        public FilterDataViewModel FilterData { get; set; }

        public FilterValuesModel FilterValues { get; set; }
    }
}