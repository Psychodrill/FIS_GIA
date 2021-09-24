using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.Models
{    public class CampaignViewModel
    {
        public int CampaignId { get; set; }
        public string Name { get; set; }
        public int YearStart { get; set; }
        public SelectList YearsStart { get; set; }
        public int YearEnd { get; set; }
        public SelectList YearsEnd { get; set; }

        public SelectList EducationForms { get; set; }
        public int EducationFormFlag { get; set; } 

        public SelectList CampaignTypes { get; set; }
        public int CampaignTypeId { get; set; }

        public SelectList CampaignStatus { get; set; }
        public int StatusId { get; set; }

    }

    public class CampaignStatuses
    {
        public int StatusID { get; set; }
        public string Name { get; set; }
    }

    public class EducationForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CampaignType
    {
        public int CampaignTypeId { get; set; }

        public string Name { get; set; }
    }
}
