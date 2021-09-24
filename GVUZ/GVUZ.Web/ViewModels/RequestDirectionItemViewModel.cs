using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using GVUZ.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class RequestDirectionItemViewModel
    {
        public RequestDirectionItemViewModel()
        {
        }

        public RequestDirectionItemViewModel(InstitutionDirectionRequestDto dto)
        {
            RequestId = dto.RequestId;
            DirectionId = dto.DirectionId;
            DisplayName = dto.DisplayName();
        }

        public int RequestId { get; set; }

        public int DirectionId { get; set; }

        public string DisplayName { get; set; }

        public string Comment { get; set; }

        public SubmitDirectionRequestDto GetDto(InstitutionDirectionRequestType requestType)
        {
            return new SubmitDirectionRequestDto
            {
                DirectionId = DirectionId,
                Comment = Comment,
                RequestType = requestType
            };
        }
    }
}