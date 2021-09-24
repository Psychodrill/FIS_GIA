using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class CompetitiveGroupItemDeleteManager : BaseDeleteManager<CompetitiveGroupItemVocDto>
    {
        CGApplicationDependencyVoc cgApplicationDependencyVoc;

        public CompetitiveGroupItemDeleteManager(CompetitiveGroupItemVocDto dto, VocabularyStorage vocabularyStorage) : this(dto, vocabularyStorage, null) { }
        public CompetitiveGroupItemDeleteManager(CompetitiveGroupItemVocDto dto, VocabularyStorage vocabularyStorage, CGApplicationDependencyVoc cgApplicationDependencyVoc)
            : base(dto, vocabularyStorage)
        {
            this.cgApplicationDependencyVoc = cgApplicationDependencyVoc;
        }

        public List<ApplicationVocDto> applications = null; //new List<ApplicationVocDto>();
        public List<string> applicationUIDs { get { return applications.Select(t => t.UID).ToList(); } }

        public List<ApplicationVocDto> GetOrderCompetitiveGroupDependency()
        {
            return ADODependencyRepository.GetCGIOrderCompetitiveGroupDependency(dto.CompetitiveGroupItemID);
        }

        public List<ApplicationVocDto> GetApplicationCompetitiveGroupItemDependency()
        {
            return ADODependencyRepository.GetCGIApplicationCompetitiveGroupItemDependency(dto.CompetitiveGroupItemID);
        }

        public override bool CheckDependency()
        {
            if (applications == null)
            {
                applications = new List<ApplicationVocDto>();

                if (cgApplicationDependencyVoc != null)
                    applications.AddRange(cgApplicationDependencyVoc.Get(dto.CompetitiveGroupID, 0));
                else
                {
                    applications.AddRange(GetOrderCompetitiveGroupDependency());
                    applications.AddRange(GetApplicationCompetitiveGroupItemDependency());
                }

            }
            return !applications.Any();
        }
    }
}
