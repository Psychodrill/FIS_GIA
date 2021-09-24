using GVUZ.DAL.Dto;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.InstitutionInfo
{
    /// <summary>
    /// Модель представления данных для прикрепленного документа к сведениям об ОО
    /// </summary>
    public class InstitutionInfoDocumentViewModel : AttachmentDocumentViewModel
    {
        public InstitutionInfoDocumentViewModel()
        {
        }

        public InstitutionInfoDocumentViewModel(InstitutionInfoDocumentDto dto) 
            : base(dto)
        {
            InstitutionId = dto.InstitutionId;
        }

        public int InstitutionId { get; set; }
    }
}