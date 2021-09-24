using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.ViewModel.Olympic
{
    public class OlympicTypeViewModel
    {
        public OlympicTypeViewModel()
        {
            OlympicTypeProfiles = new List<OlympicTypeProfileViewModel>();
        }

        public int OlympicID { get; set; }
        [Required]
        [StringLength(1023)]
        public string Name { get; set; }
        public int OlympicNumber { get; set; }
        [Required]
        public int OlympicYear { get; set; }

        public List<OlympicTypeProfileViewModel> OlympicTypeProfiles { get; set; }
    }

    public class OlympicTypeProfileViewModel
    {
        public int OlympicLevelID { get; set; }
        public int OlympicProfileID { get; set; }
        public string ProfileName { get; set; }
    }
}
