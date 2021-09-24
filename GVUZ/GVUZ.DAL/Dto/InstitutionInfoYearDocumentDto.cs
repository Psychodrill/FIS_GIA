using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Сведения о документе, прикрепленном к ОО с привязкой к году
    /// </summary>
    public class InstitutionInfoYearDocumentDto : InstitutionInfoDocumentDto
    {
        /// <summary>
        /// Год к которому относится текущий документ
        /// </summary>
        public int Year { get; set; }
    }
}
