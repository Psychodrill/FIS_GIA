using System;
using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Model.Entrants.Documents
{
    public static class EduDocumentHelper
    {
        private const string FindSpecialityQuery =
            @"SELECT ltrim(rtrim(ISNULL(NewCode, '') + ' / ' + ISNULL(ltrim(rtrim(Code)), '') + ' ' + ISNULL(ltrim(Name), '')))
              from Direction
              where QUALIFICATIONNAME = {0}
              order by newcode";

        public static IEnumerable<object> FindSpecialityByQualification(this EntrantsEntities dbContext, string qualificationName)
        {
            if (!string.IsNullOrEmpty(qualificationName))
            {
                return
                    dbContext.ExecuteStoreQuery<string>(FindSpecialityQuery, qualificationName)
                             .Distinct()
                             .Select(x => new {Name = x});
            }

            return Enumerable.Empty<object>();
        }

        public static IEnumerable<object> LoadQualifications(this EntrantsEntities dbContext)
        {
            return
                dbContext.Direction.Where(x => x.QUALIFICATIONNAME != null)
                         .Select(x => x.QUALIFICATIONNAME)
                         .Distinct()
                         .OrderBy(x => x)
                         .Select(x => new {Name = x});
        }

        public static Tuple<string, string> GetQualificationAndSpeciality(this EntrantsEntities dbContext,
                                                                          int entrantDocumentId)
        {
            var qualificationAndSpeciality = dbContext.ExecuteStoreQuery<string>(@"SELECT TOP 1 QualificationName FROM EntrantDocumentEdu (NOLOCK) WHERE EntrantDocumentId={0} UNION ALL SELECT TOP 1 SpecialityName from EntrantDocumentEdu WHERE EntrantDocumentId = {0}", entrantDocumentId).ToList();

            string qualification = null, speciality = null;

            if (qualificationAndSpeciality.Count > 0)
            {
                qualification = qualificationAndSpeciality[0];

                if (qualificationAndSpeciality.Count > 1)
                {
                    speciality = qualificationAndSpeciality[1];
                }
            }

            return new Tuple<string, string>(qualification, speciality);
        }
    }
}