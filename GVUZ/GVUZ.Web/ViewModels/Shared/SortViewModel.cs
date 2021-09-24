using System;
using GVUZ.DAL.Dto;
using GVUZ.Web.Infrastructure;

namespace GVUZ.Web.ViewModels.Shared
{
    public class SortViewModel : GVUZ.Web.Infrastructure.ISortable, GVUZ.DAL.Dto.ISortable
    {
        private readonly Func<string, bool> _sortKeyValidator;
        private readonly string _defaultSortKey;
        private string _userSortKey;

        public SortViewModel(string defaultSortKey, Func<string, bool> sortKeyValidator)
        {
            _defaultSortKey = defaultSortKey;
            _sortKeyValidator = sortKeyValidator;
        }

        public string SortKey
        {
            get { return _userSortKey ?? (_userSortKey = _defaultSortKey); }
            set
            {
                if (_sortKeyValidator != null && _sortKeyValidator(value))
                {
                    _userSortKey = value;
                }
                else
                {
                    _userSortKey = null;
                }
            }
        }

        public bool? SortDescending { get; set; }

        bool Infrastructure.ISortable.SortDescending
        {
            get { return SortDescending.GetValueOrDefault(); }
        }

        string DAL.Dto.ISortable.SortExpression
        {
            get
            {
                return SortKey;
            }
        }

        bool DAL.Dto.ISortable.SortDescending
        {
            get
            {
                return this.SortDescending.GetValueOrDefault();
            }
        }
    }
}