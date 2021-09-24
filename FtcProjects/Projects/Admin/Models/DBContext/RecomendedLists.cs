using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class RecomendedLists
    {
        public RecomendedLists()
        {
            RecomendedListsHistory = new HashSet<RecomendedListsHistory>();
        }

        public int RecListId { get; set; }
        public int InstitutionId { get; set; }
        public int CampaignId { get; set; }
        public int EduLevelId { get; set; }
        public int EduFormId { get; set; }
        public int CompetitiveGroupId { get; set; }
        public int DirectionId { get; set; }
        public byte Stage { get; set; }
        public int ApplicationId { get; set; }
        public decimal? Rating { get; set; }

        public virtual ICollection<RecomendedListsHistory> RecomendedListsHistory { get; set; }
    }
}
