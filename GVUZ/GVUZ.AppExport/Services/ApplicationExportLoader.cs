using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Model.Entrants;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportLoader : IApplicationExportLoader
    {
        public event EventHandler<ApplicationExportEventArgs> ApplicationFetched;
        private readonly long _institutionId;
        private readonly int _yearStart;

        public ApplicationExportLoader(long institutionId, int yearStart)
        {
            _institutionId = institutionId;
            _yearStart = yearStart;
        }

        public void Load()
        {
            if (ApplicationFetched == null)
            {
                return;
            }

            var ctx = new EntrantsEntities();

            HashSet<int> processed = new HashSet<int>();

            int count = ctx.ApplicationCompetitiveGroupItem.Count(x => x.CompetitiveGroup.InstitutionID == _institutionId && x.CompetitiveGroup.Campaign.YearStart == _yearStart);
            const int pageSize = 32;
            for (int offset = 0; offset < count; offset += pageSize)
            {
                foreach (var appItem in ctx.ApplicationCompetitiveGroupItem.Where(x => x.CompetitiveGroup.InstitutionID == _institutionId && x.CompetitiveGroup.Campaign.YearStart == _yearStart).OrderBy(x => x.ApplicationId).Skip(offset).Take(pageSize))
                {
                    if (!processed.Contains(appItem.ApplicationId) && IsValidApplication(appItem))
                    {
                        var dto = MapApplication(appItem.Application);
                        processed.Add(appItem.ApplicationId);
                        OnApplicationFetched(dto);
                    }
                }
            }
        }

        public int GetInstitutionEsrpId(long orgId)
        {
            using (var db = new EntrantsEntities())
            {
                return db.Institution.Single(x => x.InstitutionID == orgId).EsrpOrgID.GetValueOrDefault();
            }
        }

        private static bool IsValidApplication(ApplicationCompetitiveGroupItem app)
        { 
            //if (app.CompetitiveGroup.Campaign.IsFromKrym)
            //{
            //    return false;
            //}

            switch (app.CompetitiveGroupItem.EducationLevelID)
            {
                case 2: case 3: case 4: case 5: case 19:
                    return true;
            }

            return false;
        }

        private ApplicationExportDto MapApplication(Application app)
        {
            if (app.ApplicationID == 20508759)
            {
                
            }
            var dto = new ApplicationExportDto
                {
                    AppId = app.ApplicationID,
                    LastDenyDate = app.StatusID == 6 ? app.LastDenyDate : null,
                    RegistrationDate = app.RegistrationDate,
                    StatusId = app.StatusID,
                    FinSourceAndEduForms = app.GetFinSourceAndEduForms(_yearStart)
                };

            return dto;
        }

        private void OnApplicationFetched(ApplicationExportDto data)
        {
            var handler = ApplicationFetched;
            if (handler != null)
            {
                handler(this, new ApplicationExportEventArgs(data));
            }
        }
    }
}