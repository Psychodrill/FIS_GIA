using GVUZ.DAL.Dto;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.InstitutionInfo
{
    /// <summary>
    /// Модель представления данных для прикрепленного документа к сведениям об ОО с привязкой к году
    /// </summary>
    public class InstitutionInfoYearDocumentViewModel : InstitutionInfoDocumentViewModel
    {
        public InstitutionInfoYearDocumentViewModel()
        {
        }

        public InstitutionInfoYearDocumentViewModel(InstitutionInfoYearDocumentDto dto)
            :base(dto) 
        {
            Year = dto.Year;
        }

        public int Year { get; set; }
    }
}