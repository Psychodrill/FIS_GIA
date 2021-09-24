using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class CompetitiveGroupTargetDeleteManager : BaseDeleteManager<CompetitiveGroupTargetVocDto>
    {
        CGApplicationDependencyVoc cgApplicationDependencyVoc;


        public CompetitiveGroupTargetDeleteManager(CompetitiveGroupTargetVocDto dto, VocabularyStorage vocabularyStorage, CGApplicationDependencyVoc cgApplicationDependencyVoc)
            : base(dto, vocabularyStorage)
        {
            this.cgApplicationDependencyVoc = cgApplicationDependencyVoc;
            CompetitiveGroupTargetItems = new List<CompetitiveGroupTargetItemDeleteManager>();
        }

        List<CompetitiveGroupTargetItemDeleteManager> CompetitiveGroupTargetItems { get; set; }
        public override void FillChildren()
        {
            foreach (var cgi in vocabularyStorage.CompetitiveGroupTargetItemVoc.Items.Where(cgi => cgi.CompetitiveGroupTargetID == dto.CompetitiveGroupTargetID))
            {
                var item = new CompetitiveGroupTargetItemDeleteManager(cgi, vocabularyStorage, cgApplicationDependencyVoc);
                CompetitiveGroupTargetItems.Add(item);
                item.FillChildren();
            }
        }

        public List<ApplicationVocDto> applications = null;
        public List<string> applicationUIDs { get { return applications.Select(t => t.UID).ToList(); } }

        public override bool CheckDependency()
        {
            if (applications == null)
            {
                applications = new List<ApplicationVocDto>();

                //applications.AddRange(ADODependencyRepository.GetCGTApplicationDependency(dto.CompetitiveGroupTargetID));
                applications.AddRange(cgApplicationDependencyVoc.Get(0, dto.CompetitiveGroupTargetID));
            }
            return !applications.Any();
        }

        public bool GetDependencies()
        {
            applications = new List<ApplicationVocDto>();
            if (!CompetitiveGroupTargetItems.Any())
                FillChildren();

            CheckDependency();
            foreach (var item in CompetitiveGroupTargetItems)
            {
                if (!item.CheckDependency())
                    applications.AddRange(item.applications);
            }

            return applications.Any();
        }

        public override void MarkDelete()
        {
            base.MarkDelete();
            foreach (var item in CompetitiveGroupTargetItems)
                item.MarkDelete();
        }

        public override List<VocabularyBaseDto> GetDeleteObjects()
        {
            var res = base.GetDeleteObjects();
            foreach (var item in CompetitiveGroupTargetItems)
                res.AddRange(item.GetDeleteObjects());
            
            return res;
        }
    }
}
