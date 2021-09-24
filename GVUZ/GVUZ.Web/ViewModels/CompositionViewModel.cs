using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
    public class CompositionPage
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }

    public class CompositionViewModel
    {

        public CompositionViewModel()
        {
            CompositionPages = new List<CompositionPage>();
        }

        [DisplayName("Страницы сочинения")]
        public IList<CompositionPage> CompositionPages { get; set; }

        [DisplayName("Идентификатор заявления")]
        public int ApplicationId { get; set; }

        public string Path
        {
            get
            {
                if (CompositionPages.Count() > 0)
                    return CompositionPages[0].Path;
                else return "";
            }
        }
    }
}