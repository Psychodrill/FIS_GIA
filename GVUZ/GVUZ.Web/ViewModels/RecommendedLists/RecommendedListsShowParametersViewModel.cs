using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels.RecommendedLists
{
    public class RecommendedListsShowParametersViewModel
    {
        public int SortDirection { get; set; }
        public int PageToShow { get; set; }
        public FilterValuesModel Filter { get; set; }
    }
}