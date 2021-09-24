using GVUZ.Data.Helpers;
using GVUZ.Data.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GVUZ.Web.ViewModels.OlympicDiplomant
{
    public class OlympicDiplomantCanceledEditViewModel : OlympicDiplomantBaseViewModel
    {
        //-----------------------------------------------------------------------------------------------------

        public OlympicDiplomantDocument Data { get; set; }

        public OlympicDiplomantCanceledEditViewModel() : base()
        {
            Data = new OlympicDiplomantDocument { };
        }

        public OlympicDiplomantCanceledEditViewModel(long id) : this()
        {
        }

        //-----------------------------------------------------------------------------------------------------
    }
}

