using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    [Description("Индивидуальные достижения, учитываемые ОО")]
    public class InstitutionAchievementDto : BaseDto
    {
        public string IAUID;
        public string CampaignUID;
        public string Name;
        public int IdCategory;
        public decimal MaxValue;

        public override string UID
        {
            get { return IAUID; }
            set { IAUID = value; }
        }

        public string Key() { return IAUID + "||" + CampaignUID; }
    }
}
