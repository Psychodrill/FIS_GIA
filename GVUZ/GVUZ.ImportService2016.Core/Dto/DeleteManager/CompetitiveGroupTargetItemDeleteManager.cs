using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class CompetitiveGroupTargetItemDeleteManager : BaseDeleteManager<CompetitiveGroupTargetItemVocDto>
    {
        CGApplicationDependencyVoc cgApplicationDependencyVoc;

        public CompetitiveGroupTargetItemDeleteManager(CompetitiveGroupTargetItemVocDto dto, VocabularyStorage vocabularyStorage, CGApplicationDependencyVoc cgApplicationDependencyVoc)
            : base(dto, vocabularyStorage)
        {
            this.cgApplicationDependencyVoc = cgApplicationDependencyVoc;
        }


        public List<ApplicationVocDto> applications = null;
        public List<string> applicationUIDs { get { return applications.Select(t => t.UID).ToList(); } }

        public override bool CheckDependency()
        {
            if (applications == null)
            {
                /*
                 *  зависимость с Application (Application.OrderCompetitiveGroupTargetID 
                 *  или ApplicationCompetitiveGroupItem where Priority <> Null and CompetitiveGroupTargetID = @TargetID 
                 *  and CompetitiveGroupItemID = @CompetitiveGroupItemID)
                 */
                
                applications = new List<ApplicationVocDto>();
                //if (dto.CompetitiveGroupItemID != 0)
                //    applications.AddRange(ADODependencyRepository.GetCGTIApplicationDependency(dto.CompetitiveGroupItemID, dto.CompetitiveGroupTargetID));

                // TODO: 
                //applications.AddRange(cgApplicationDependencyVoc.Get(0, dto.CompetitiveGroupItemID, dto.CompetitiveGroupTargetID));
            }
            return !applications.Any();
        }

    }
}
