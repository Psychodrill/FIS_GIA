namespace GVUZ.Model.Institutions
{
    public enum AdmissionItemLevel : short
    {
        /// <summary>
        ///     "Нет"
        /// </summary>
        Institution = 0,

        /// <summary>
        ///     Курс
        /// </summary>
        Course = 1,

        /// <summary>
        ///     Уровень образования
        /// </summary>
        EducationLevel = 2,

        /// <summary>
        ///     Факультет
        /// </summary>
        Faculty = 3,

        /// <summary>
        ///     Кафедра
        /// </summary>
        Department = 4,

        /// <summary>
        ///     Группа направлений
        /// </summary>
        DirectionGroup = 5,

        /// <summary>
        ///     Направление (специальность)
        /// </summary>
        Direction = 6,

        /// <summary>
        ///     Форма обучения
        /// </summary>
        Study = 7,

        /// <summary>
        ///     Тип набора (приема)
        /// </summary>
        AdmissionType = 8
    }
}