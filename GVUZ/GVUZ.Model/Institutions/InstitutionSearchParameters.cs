using System.Collections.Generic;

namespace GVUZ.Model.Institutions
{
    public class InstitutionSearchHierarchyParameters : InstitutionSearchParameters<List<InstitutionSearchResult>>
    {
        public override ConvertResults Convert
        {
            get
            {
                if (base.Convert == null)
                    return (items, parameters) =>
                           SearchResultHelper.CreateHierarchy(items, (InstitutionSearchHierarchyParameters) parameters);
                return base.Convert;
            }
            set { base.Convert = value; }
        }
    }

    /// <summary>
    ///     Параметры для поиска образовательных учреждений. <see cref="IsVUZ" /> и <see cref="IsSSUZ" /> по умолчанию true.
    /// </summary>
    public class InstitutionSearchParameters<TResult> : SearchParameters<TResult>
    {
        public InstitutionSearchParameters()
        {
            IsVUZ = true;
            IsSSUZ = true;
        }

        /// <summary>
        ///     Часть названия ОУ
        /// </summary>
        public string NamePart { get; set; }

        /// <summary>
        ///     Искать ли по ВУЗам (по умолчанию true)
        /// </summary>
        public bool IsVUZ { get; set; }

        /// <summary>
        ///     Искать ли по ССУЗам (по умолчанию true)
        /// </summary>
        public bool IsSSUZ { get; set; }

        /// <summary>
        ///     СНИЛС - обязательный параметр, определяет пользователя ПГУ.
        /// </summary>
        public string Snils { get; set; }

        /// <summary>
        ///     Ограничение по поиску поддерева внутри одного <see cref="InstitutionSearchResult.AdmissionItemID" />.
        /// </summary>
        public int? ParentStructureID { get; set; }

        /// <summary>
        ///     Ограничение по глубине возвращаемого дерева.
        /// </summary>
        public short? DepthLimit { get; set; }

        /// <summary>
        ///     Направление (специальность)
        /// </summary>
        public string DirectionName { get; set; }

        /// <summary>
        ///     Код направления (специальности)
        /// </summary>
        public string DirectionCode { get; set; }

        /// <summary>
        ///     Регион
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        ///     Организационно-правовая форма
        /// </summary>
        public int? FormOfLawID { get; set; }

        /// <summary>
        ///     Уровень образования
        /// </summary>
        public short? EducationLevelID { get; set; }

        /// <summary>
        ///     Форма обучения
        /// </summary>
        public short? StudyID { get; set; }

        /// <summary>
        ///     Форма обучения
        /// </summary>
        public short? AdmissionTypeID { get; set; }

        /// <summary>
        ///     Есть ли военная кафедра
        /// </summary>
        public bool? HasMilitaryDepartment { get; set; }

        /// <summary>
        ///     Есть ли подготовительные курсы
        /// </summary>
        public bool? HasPreparatoryCourses { get; set; }

        /// <summary>
        ///     Есть ли льготы при поступлении
        /// </summary>
        public bool? HasOlympics { get; set; }

        /// <summary>
        ///     Размер страницы, по умолчанию 50 (не имеет смысла, если задан <see cref="ParentStructureID" />)
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        ///     Номер страницы, по умолчанию 1 (не имеет смысла, если задан <see cref="ParentStructureID" />)
        /// </summary>
        public int? PageNumber { get; set; }
    }
}