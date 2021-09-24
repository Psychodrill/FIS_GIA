using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class IndividualAchievementsCategory
    {
        public IndividualAchievementsCategory()
        {
            InstitutionAchievements = new HashSet<InstitutionAchievements>();
        }

        public int IdCategory { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<InstitutionAchievements> InstitutionAchievements { get; set; }
    }
}
