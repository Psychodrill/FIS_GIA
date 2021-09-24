namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Сведения о направлении подготовки
    /// </summary>
    public class DirectionDto : IDirectionDescription
    {
        /// <summary>
        /// Id направления
        /// </summary>
        public int DirectionId { get; set; }

        ///// <summary>
        ///// Код направления
        ///// </summary>
        //public string Code { get; set; }

        /// <summary>
        /// Новый код направления
        /// </summary>
        public string NewCode { get; set; }

        /// <summary>
        /// Код квалификации
        /// </summary>
        public string QualificationCode { get; set; }

        /// <summary>
        /// Наименование направления
        /// </summary>
        public string DirectionName { get; set; }

        /// <summary>
        /// Наименование квалификации
        /// </summary>
        public string QualificationName { get; set; }

        /// <summary>
        /// Id уровня образования
        /// </summary>
        public int EducationLevelId { get; set; }

        /// <summary>
        /// Наименование уровня образования
        /// </summary>
        public string EducationLevelName { get; set; }
    }
}
