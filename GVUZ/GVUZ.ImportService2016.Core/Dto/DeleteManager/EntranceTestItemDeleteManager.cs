using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class EntranceTestItemDeleteManager : BaseDeleteManager<EntranceTestItemCVocDto>
    {
        public EntranceTestItemDeleteManager(EntranceTestItemCVocDto dto, VocabularyStorage vocabularyStorage)
            : base(dto, vocabularyStorage)
        {
            BenefitItems = new List<BenefitItemDeleteManager>();
        }

        List<BenefitItemDeleteManager> BenefitItems { get; set; }

        public override void FillChildren()
        {
            foreach (var b in vocabularyStorage.BenefitItemCVoc.Items.Where(t => t.EntranceTestItemID == dto.EntranceTestItemID))
            {
                var item = new BenefitItemDeleteManager(b, vocabularyStorage);
                BenefitItems.Add(item);
                item.FillChildren();
            }
        }

        public List<ApplicationVocDto> applications = new List<ApplicationVocDto>();
        public List<string> applicationUIDs { get { return applications.Select(t => t.UID).ToList(); } }

        public override bool CheckDependency()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var ids = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items.Where(t => t.EntranceTestItemID == dto.EntranceTestItemID).Select(t => t.ApplicationID).Distinct().ToList();
            applications = vocabularyStorage.ApplicationVoc.Items.Where(t => ids.Contains(t.ApplicationID)).ToList();

            sw.Stop();
            Console.WriteLine("EntranceTestItemDeleteManager-CheckDependency - AddRange: {0} сек", sw.Elapsed.TotalSeconds);
            
            return !applications.Any();
        }

        public override void MarkDelete()
        {
            base.MarkDelete();
            foreach (var item in BenefitItems)
                item.MarkDelete();
        }

        public override List<VocabularyBaseDto> GetDeleteObjects()
        {
            var res = base.GetDeleteObjects();
            foreach (var item in BenefitItems)
                res.AddRange(item.GetDeleteObjects());

            return res;
        }
    }
}
