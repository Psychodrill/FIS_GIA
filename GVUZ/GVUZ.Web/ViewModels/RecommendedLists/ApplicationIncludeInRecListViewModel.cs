using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.ViewModels.RecommendedLists
{
    public class ApplicationIncludeInRecListViewModel
    {
        public int ApplicationId { get; set; }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("ФИО абитуриента")]
        public string EntrantName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string DocumentData { get; set; }

        public StageListIncludeViewModel[] Stage1List { get; set; }

        public StageListIncludeViewModel[] Stage2List { get; set; }

        public StageListIncludeViewModel ViewData { get { return null; } }
    }

    public class StageListIncludeViewModel
    {
        private Guid tempid;
        public Guid Id // Временный идентификатор объекта для работы в UI
        {
            get { return tempid; }
        }

        public StageListIncludeViewModel()
        {
            tempid = Guid.NewGuid();
        }



        public int CompetitiveGroupId { get; set; }
        public int DirectionId { get; set; }
        public int CampaignId { get; set; }

        private short edFormid = 0;
        public short EduFormId 
        {
            get
            {
                return edFormid;
            }
            set
            {
                edFormid = value;
                edFormName = EDFormsConst.GetConstDescription(EduFormId);
            }
        }

        private short edLevelId = 0;
        public short EduLevelId 
        {
            get { return edLevelId; }
            set
            {
                edLevelId = value;
                edLevelName = EDLevelConst.GetConstDescription(edLevelId);
            }
        }

        private string edLevelName;
        [DisplayName("Уровень образования")]
        public string EduLevelName
        {
            get
            {
                if (string.IsNullOrEmpty(edLevelName))
                    edLevelName = EDLevelConst.GetConstDescription(edLevelId);
                return edLevelName;
            }
        }

        public short EduSource { get { return EDSourceConst.Budget; } } //Раз источник финансирования только один - пусть будет для отображения

        [DisplayName("Приоритет")]
        public int Priority { get; set; }

        [DisplayName("Рекомендован")]
        public bool Recommended { get; set; }

        private string edSourceName;
        [DisplayName("Источник финансирования")]
        public string EdSource 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(edSourceName))
                    edSourceName = EDSourceConst.GetConstDescription(EduSource);
                return edSourceName;
            } 
        }

        private string edFormName;
        [DisplayName("Форма обучения")]
        public string EduForm 
        { 
            get 
            { 
                if (string.IsNullOrEmpty(edFormName))
                    edFormName = EDFormsConst.GetConstDescription(EduFormId);
                return edFormName;
            } 
        }

        [DisplayName("Конкурс")]
        public string CompetitveGroupName { get; set; }

        [DisplayName("Направление подготовки")]
        public string DirectionName { get; set; }
    }
}