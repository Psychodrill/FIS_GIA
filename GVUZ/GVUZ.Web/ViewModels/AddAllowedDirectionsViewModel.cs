using GVUZ.DAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    /// <summary>
    /// Модель для запроса на добавление разрешенных направлений для ОО
    /// </summary>
    public class AddAllowedDirectionsViewModel
    {
        private List<AddAllowedDirectionItemViewModel> _items;

        public int? Year { get; set; }

        public List<AddAllowedDirectionItemViewModel> Items
        {
            get { return _items ?? (_items = new List<AddAllowedDirectionItemViewModel>()); }
            set { _items = value; }
        }
        
        public IEnumerable<AllowedDirectionCreateDto> GetDto()
        {
            return Items.Select(x => new AllowedDirectionCreateDto { DirectionId = x.DirectionId, Year = Year });
        }
    }
}