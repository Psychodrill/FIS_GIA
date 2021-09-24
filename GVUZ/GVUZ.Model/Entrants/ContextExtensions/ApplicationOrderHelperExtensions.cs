using System;
using System.Linq;
using GVUZ.Model.Institutions;

namespace GVUZ.Model.Entrants.ContextExtensions
{
    public static class ApplicationOrderHelperExtensions
    {
        public static void SetCompetitiveGroupTarget(this Application app)
        {
            // для целевиков ставим выбранную организацию ЦП
            if (app.OrderEducationSourceID == EDSourceConst.Target)
            {
                var targetId = app.ApplicationCompetitiveGroupItem.Where(
                    x =>
                    x.CompetitiveGroupItemId == app.OrderCompetitiveGroupItemID && x.EducationFormId == app.OrderEducationFormID &&
                    x.EducationSourceId == app.OrderEducationSourceID)
                                  .OrderBy(x => x.Priority == null)
                                  .ThenBy(x => x.Priority)
                                  .Select(x => x.CompetitiveGroupTargetId)
                                  .FirstOrDefault();

                if (targetId.HasValue)
                {
                    Func<ApplicationSelectedCompetitiveGroupTarget, bool> predicate = null;

                    switch (app.OrderEducationFormID)
                    {
                        case EDFormsConst.O:
                            predicate = x => x.CompetitiveGroupTargetID == targetId.Value && x.IsForO;
                            break;
                        case EDFormsConst.OZ:
                            predicate = x => x.CompetitiveGroupTargetID == targetId.Value && x.IsForOZ;
                            break;
                        case EDFormsConst.Z:
                            predicate = x => x.CompetitiveGroupTargetID == targetId.Value && x.IsForZ;
                            break;
                    }

                    if (predicate != null)
                    {
                        app.OrderCompetitiveGroupTargetID =
                            app.ApplicationSelectedCompetitiveGroupTarget.Where(predicate)
                               .Select(x => (int?)x.CompetitiveGroupTargetID)
                               .FirstOrDefault();
                    }
                }
            }
            else
            {
                app.OrderCompetitiveGroupTargetID = null;
            }
        }
    }
}