using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class InstitutionAchievementsVoc : VocabularyBase<InstitutionAchievementsVocDto>
    {
        public InstitutionAchievementsVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class InstitutionAchievementsVocDto : VocabularyBaseDto
    {
        /// <summary>
        /// Итоговое сочинение
        /// </summary>
        public static int FinalEssey = 12;


        public int IdAchievement { get; set; }
        public override int ID
        {
            get { return IdAchievement; }
            set { IdAchievement = value; }
        }

        //public string Name { get; set; }
        public int IdCategory { get; set; }
        public decimal MaxValue { get; set; }
        public int CampaignID { get; set; }
    }
}
