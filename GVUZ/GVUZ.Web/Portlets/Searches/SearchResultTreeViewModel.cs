using System.Collections.Generic;
using System.ComponentModel;

namespace GVUZ.Web.Portlets.Searches
{
    public class SearchResultTreeViewModel
    {
        [DisplayName("Всего найдено")]
        public int ResultCount { get; set; }

        [DisplayName("Страница")]
        public int CurrentPage { get; set; }
/*
        [DisplayName("Образовательные учреждения")]
        public List<string> Institutions { get; set; }*/

        public JsonInstitutionSearchResult TreeResult { get; set; }

    }
}