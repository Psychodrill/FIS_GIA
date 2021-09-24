using System;
using System.Collections.Generic;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Данные для отображения на вкладке "Структура ОО"
    /// </summary>
    public class StructureInfoDto
    {
        public int InstitutionId { get; set; }

        /// <summary>
        /// Полное наименование ОО
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Краткое наименование ОО
        /// </summary>
        public string BriefName { get; set; }
    }
}
