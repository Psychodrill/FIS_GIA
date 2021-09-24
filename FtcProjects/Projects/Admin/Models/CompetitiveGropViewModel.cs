using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class CompetitiveGropViewModel
    {
        public int CompetitiveGroupId { get; set; }
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        public int? CampaignId { get; set; }
        public string CampaignName { get; set; }

        public SelectList EducationForms { get; set; }
        public short? EducationFormId { get; set; }

        public SelectList EducationLevels { get; set; }
        public short? EducationLevelId { get; set; }

        public SelectList Directions { get; set; }
        public int? DirectionId { get; set; }

        public short? EducationSourceId { get; set; }
    }

    public class EducationFormCg 
    {
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
    }

    public class EducationLevelCg
    {
        public int ItemTypeId { get; set; }
        public string Name { get; set; }
    }

    public class DirectionCg
    {
        public int DirectionID { get; set; }
        public string Name { get; set; }
    }
}
