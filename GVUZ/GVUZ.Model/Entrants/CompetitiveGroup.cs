using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FogSoft.Helpers;

namespace GVUZ.Model.Entrants
{
    public partial class CompetitiveGroup
    {
        public static bool IsCompetitiveGroupExists(EntrantsEntities context, string groupName, int campaignID, int institutionID)
        {
            return (from cg in context.CompetitiveGroup
                    where cg.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase) &&
                          cg.InstitutionID == institutionID
                          && cg.CampaignID == campaignID
                    select cg.Name).Any();
        }

        public static bool IsCompetitiveGroupExists(EntrantsEntities context, int groupID, int institutionID)
        {
            return (from cg in context.CompetitiveGroup where cg.CompetitiveGroupID == groupID && cg.InstitutionID == institutionID select cg.Name).Any();
        }

        public static CompetitiveGroup GetCompetitiveGroup(EntrantsEntities context, string groupName, int institutionID)
        {
            return (from cg in context.CompetitiveGroup where cg.Name.Equals(groupName, StringComparison.CurrentCultureIgnoreCase) && cg.InstitutionID == institutionID select cg).FirstOrDefault();
        }


        public static string Delete(EntrantsEntities dbContext, int competitiveGroupID)
        {
            var competitiveGroup =
                dbContext.CompetitiveGroup.Where(x => x.CompetitiveGroupID == competitiveGroupID).FirstOrDefault();
            if (competitiveGroup == null)
            {
                string errorMessage = "Ошибка при удалении. Отсутствует конкурс с ID: " + competitiveGroupID;
                LogHelper.Log.Error(errorMessage);
                return errorMessage;
            }

            dbContext.DeleteObject(competitiveGroup);
            dbContext.SaveChanges();

            /*
						try
						{
							using (EntrantsEntities eContext = new EntrantsEntities())
							{
								if (eContext.Application.Where(x => x.CompetitiveGroupID == competitiveGroupID).Count() > 0)
								{
									return "Невозможно удалить группу, так как есть заявления, включенные в неё";
								}
							}
							using (BenefitsEntities bContext = new BenefitsEntities())
							{
								bContext.BenefitItemC.Where(x => x.CompetitiveGroupID == competitiveGroupID && x.EntranceTestItemID == null).ToList().ForEach(bContext.BenefitItemC.DeleteObject);
								bContext.SaveChanges();
							}
						}
						catch (Exception ex)
						{
							SqlException inner = ex.InnerException as SqlException;
							if (inner != null && inner.Message.Contains("FK_Application_CompetitiveGroup"))
								return "Невозможно удалить группу, так как есть заявления, включенные в неё";
							throw;
						}
			*/
            return String.Empty;
        }

        public CompetitiveGroupItem GetLinkedData(int competitiveGroupItemID)
        {
            return null;
        }

    }
}
