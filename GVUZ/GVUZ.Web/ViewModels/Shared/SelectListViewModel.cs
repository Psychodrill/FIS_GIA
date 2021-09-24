﻿using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.Shared
{
    public class SelectListViewModelBase
    {
        private string _unselectedText;
        public const string DefaultUnselectedText = "Выберите значение";
        public bool ShowUnselectedText { get; set; }

        public string UnselectedText
        {
            get { return _unselectedText ?? (_unselectedText = DefaultUnselectedText); }
            set { _unselectedText = value; }
        }
    }

    public class SelectListViewModel<TId> : SelectListViewModelBase
    {
        private List<SelectListItemViewModel<TId>> _items;

        public List<SelectListItemViewModel<TId>> Items
        {
            get { return _items ?? (_items = new List<SelectListItemViewModel<TId>>()); }
            set { _items = value; }
        }

        public void Add(TId id, string displayName)
        {
            Add(new SelectListItemViewModel<TId>(id, displayName));
        }

        public void Add(TId id, string displayName, int campaignStatusID)
        {
            Add(new SelectListItemViewModel<TId>(id, displayName, campaignStatusID));
        }
        public void Add(SelectListItemViewModel<TId> model)
        {
            Items.Add(model);
        }
    }
}