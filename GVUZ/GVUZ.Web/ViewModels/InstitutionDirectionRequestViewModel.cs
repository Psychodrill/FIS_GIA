using GVUZ.DAL.Dto;
using GVUZ.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class InstitutionDirectionRequestViewModel
    {
        public InstitutionDirectionRequestViewModel()
        {
        }

        public InstitutionDirectionRequestViewModel(InstitutionDirectionRequestDto dto)
        {
            RequestId = dto.RequestId;
            OperationType = InstitutionDirectionRequestItemViewModel.OperationTypes[dto.RequestType];
            InstitutionId = dto.InstitutionId;
            DirectionName = dto.DisplayName();
            Comment = dto.Comment;
            RequestDate = dto.RequestDate.ToString("dd.MM.yyyy");
        }

        public int RequestId { get; set; }

        [DisplayName("Вид заявки")]
        public string OperationType { get; set; }

        public int InstitutionId { get; set; }

        [DisplayName("Направление")]
        public string DirectionName { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        [DisplayName("Дата создания заявки")]
        public string RequestDate { get; set; }
    }
}