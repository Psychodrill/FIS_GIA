namespace GVUZ.Web.Portlets.Searches
{
    public class InstitutionSearchFields
    {
        /// <summary>
        /// Часть названия ОО</summary>
        public string NamePart { get; set; }

        /// <summary>
        /// Искать ли по ВУЗам</summary>
        public bool IsVUZ { get; set; }

        /// <summary>
        /// Искать ли по ССУЗам</summary>
        public bool IsSSUZ { get; set; }

        /// <summary>
        /// Искать ли по ВУЗам</summary>
        public bool VuzCheckDirection { get; set; }

        /// <summary>
        /// Искать ли по ССУЗам</summary>
        public bool SsuzCheckDirection { get; set; }

        /// <summary>
        /// Искать ли по ВУЗам</summary>
        public bool VuzCheckInstitution { get; set; }

        /// <summary>
        /// Искать ли по ССУЗам</summary>
        public bool SsuzCheckInstitution { get; set; }
        
        /// <summary>
        /// Направление (специальность)</summary>
        public string DirectionName { get; set; }

        /// <summary>
        /// Код специальности</summary>
        public string DirectionCode { get; set; }

        /// <summary>
        /// Город</summary>
        public string City { get; set; }

        /// <summary>
        /// Регион</summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Организационно-правовая форма</summary>
        public int FormOfLawID { get; set; }

        /// <summary>
        /// Форма обучения</summary>
        public short StudyID { get; set; }

        /// <summary>
        /// Уровень образования</summary>
        public short EducationLevelID { get; set; }

        /// <summary>
        /// Тип приема</summary>
        public short AdmissionTypeId { get; set; }

        /// <summary>
        /// Военная кафедра</summary>
        public string Military { get; set; }

        /// <summary>
        /// Курсы</summary>
        public string Courses { get; set; }

        /// <summary>
        /// Олимпиады </summary>
        public string Olympics { get; set; }

        /// <summary>
        /// Есть ли военная кафедра</summary>
        public bool? HasMilitaryDepartment
		{
			get { return TriStateCheckBox.GetValue(Military); }
		}

        /// <summary>
        /// Есть ли подготовительные курсы</summary>
        public bool? HasPreparatoryCourses 
		{
			get { return TriStateCheckBox.GetValue(Courses); }
		}

        /// <summary>
        /// Учитываются ли олимпиады </summary>
        public bool? HasOlympics
		{
			get { return TriStateCheckBox.GetValue(Olympics); }
		}

        /// <summary>
        /// Расширенный поиск </summary>
        public string AdvancedSearch { get; set; }

        /// <summary>
        /// Номер страницы </summary>
        public int PageNumber { get; set; }
    }
}
