using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class RequestDirectionsDataViewModel
    {
        private List<RequestDirectionItemViewModel> _addedItems;

        private List<RequestDirectionItemViewModel> _deletedItems;

        private List<RequestDirectionItemViewModel> _deniedItems;

        public RequestDirectionsDataViewModel()
        {
        }

        public RequestDirectionsDataViewModel(IEnumerable<InstitutionDirectionRequestDto> items)
        {
            _addedItems = items.Where(x => x.RequestType == InstitutionDirectionRequestType.AddAllowedDirection && !x.IsDenied).Select(x => new RequestDirectionItemViewModel(x)).ToList();
            _deletedItems = items.Where(x => x.RequestType == InstitutionDirectionRequestType.RemoveAllowedDirection && !x.IsDenied).Select(x => new RequestDirectionItemViewModel(x)).ToList();
            _deniedItems = items.Where(x => x.IsDenied).Select(x => new RequestDirectionItemViewModel(x)).ToList();
        }

        /// <summary>
        /// Направления в заявках на добавление
        /// </summary>
        public List<RequestDirectionItemViewModel> AddedItems
        {
            get { return _addedItems ?? (_addedItems = new List<RequestDirectionItemViewModel>()); }
            set { _addedItems = value; }
        }

        /// <summary>
        /// Направления в заявках на удаление
        /// </summary>
        public List<RequestDirectionItemViewModel> DeletedItems
        {
            get { return _deletedItems ?? (_deletedItems = new List<RequestDirectionItemViewModel>()); }
            set { _deletedItems = value; }
        }

        /// <summary>
        /// Направления в отклоненных заявках
        /// </summary>
        public List<RequestDirectionItemViewModel> DeniedItems
        {
            get { return _deniedItems ?? (_deniedItems = new List<RequestDirectionItemViewModel>()); }
            set { _deniedItems = value; }
        }

        public IEnumerable<SubmitDirectionRequestDto> GetSubmits()
        {
            return AddedItems.Select(x => x.GetDto(InstitutionDirectionRequestType.AddAllowedDirection)).Union(DeletedItems.Select(x => x.GetDto(InstitutionDirectionRequestType.RemoveAllowedDirection)));
        }
    }
}