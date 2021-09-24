using System.Collections.Generic;
using System.ComponentModel;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.Portlets.Searches
{
    public class SearchInstitutionViewModel
    {
        [DisplayName("Ошибка заполнения")]
        public string ValidationMessage { get; set; }

        [DisplayName("Название образовательного учреждения")]
        public string InstitutionName { get; set; }
        public string InstitutionNameIsFull { get; set; }

        [DisplayName("ВУЗы")]
        public bool VuzCheck { get; set; }

        [DisplayName("ССУЗы")]
        public bool SsuzCheck { get; set; }

        [DisplayName("Специальность")]
        public string DirectionName { get; set; }
		public string DirectionNameIsFull { get; set; }

        [DisplayName("Код специальности")]
        public string DirectionCode { get; set; }
		public string DirectionCodeIsFull { get; set; }

        [DisplayName("Город")]
        public string City { get; set; }

        [DisplayName("Регион")]
        public string Region { get; set; }
		public string RegionIsFull { get; set; }
        
        [DisplayName("Форма обучения")]
        public int StudyFormId { get; set; }

        [DisplayName("Организационно-правовая форма")]
        public int FormOfLawID { get; set; }

        [DisplayName("Уровень образования")]
        public int EducationLevelId { get; set; }

        [DisplayName("Тип приема")]
        public int AdmissionTypeId { get; set; }
        
        [DisplayName("Военная кафедра")]
        public string MilitaryCheckValue { get; set; }

        [DisplayName("Подготовительные курсы")]
        public string CoursesCheckValue { get; set; }

        [DisplayName("Олимпиады")]
        public string OlympicsCheckValue { get; set; }

        public List<AdmissionItemType> StudyFormList;
        public List<FormOfLaw> FormOfLawList;
        public List<AdmissionItemType> EducationLevelList;
        public List<AdmissionItemType> AdmissionTypeList;

        [DisplayName("Расширенный поиск")]
        public string AdvancedSearch { get; set; }

        public int PageNumber { get; set; }
    }
}