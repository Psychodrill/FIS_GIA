using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using GVUZ.Web.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Web.ViewModels
{
    /// <summary>
    /// Сведения о направлениях в заявках от ОО
    /// </summary>
    public class InstitutionDirectionRequestDetailsViewModel
    {
        public static readonly InstitutionDirectionRequestDetailsViewModel DefaultInstance = new InstitutionDirectionRequestDetailsViewModel();

        private List<InstitutionDirectionRequestItemViewModel> _items;

        public InstitutionDirectionRequestDetailsViewModel()
        {
        }

        public InstitutionDirectionRequestDetailsViewModel(IEnumerable<InstitutionDirectionRequestDto> items)
        {
            _items = items.Select(x => new InstitutionDirectionRequestItemViewModel(x)).ToList();
        }

        public int InstitutionId { get; set; }
        public string InstitutionName { get; set; }

        public List<InstitutionDirectionRequestItemViewModel> Items
        {
            get { return _items ?? (_items = new List<InstitutionDirectionRequestItemViewModel>()); }
            set { _items = value; }
        }
    }
}