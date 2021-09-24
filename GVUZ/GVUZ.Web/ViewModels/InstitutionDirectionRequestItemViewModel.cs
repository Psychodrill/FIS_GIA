using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using GVUZ.Web.Helpers;
using System.Collections.Generic;

namespace GVUZ.Web.ViewModels
{

    public class InstitutionDirectionRequestItemViewModel
    {
        public static readonly Dictionary<InstitutionDirectionRequestType, string> OperationTypes = new Dictionary<InstitutionDirectionRequestType, string>
        {
            { InstitutionDirectionRequestType.AddAllowedDirection, "Добавление в список разрешенных" },
            { InstitutionDirectionRequestType.RemoveAllowedDirection, "Удаление из списка разрешенных" },
            { InstitutionDirectionRequestType.AddProfDirection, "Добавление в список с профильными ВИ" }
        };

        public InstitutionDirectionRequestItemViewModel()
        {
        }

        public InstitutionDirectionRequestItemViewModel(InstitutionDirectionRequestDto dto)
        {
            RequestId = dto.RequestId;
            DirectionName = dto.DisplayName();
            OperationType = OperationTypes[dto.RequestType];
        }

        public int RequestId { get; set; }

        public string DirectionName { get; set; }

        public string OperationType { get; set; }
    }
}