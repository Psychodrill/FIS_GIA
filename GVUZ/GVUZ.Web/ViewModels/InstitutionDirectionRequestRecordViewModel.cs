using System.ComponentModel;
using GVUZ.DAL.Dto;

namespace GVUZ.Web.ViewModels
{
    public class InstitutionDirectionRequestRecordViewModel
    {
        public static readonly InstitutionDirectionRequestRecordViewModel MetadataInstance = new InstitutionDirectionRequestRecordViewModel();

        public InstitutionDirectionRequestRecordViewModel()
        {
        }

        public InstitutionDirectionRequestRecordViewModel(InstitutionDirectionRequestSummaryDto dto)
        {
            InstitutionId = dto.InstitutionId;
            InstitutionName = dto.InstitutionName;
            NumRequests = dto.NumRequests;
            LastRequestDate = dto.LastRequestDate.ToString("dd.MM.yyyy");
        }

        public int InstitutionId { get; set; }

        [DisplayName("Полное наименование")]
        public string InstitutionName { get; set; }

        [DisplayName("Количество заявок")]
        public int NumRequests { get; set; }

        [DisplayName("Дата последней заявки")]
        public string LastRequestDate { get; set; }
        
    }
}