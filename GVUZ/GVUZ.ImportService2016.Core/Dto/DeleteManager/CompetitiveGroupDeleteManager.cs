using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class CompetitiveGroupDeleteManager : BaseDeleteManager<CompetitiveGroupVocDto>
    {
        CGApplicationDependencyVoc cgApplicationDependencyVoc;

        public CompetitiveGroupDeleteManager(CompetitiveGroupVocDto dto, VocabularyStorage vocabularyStorage) : this(dto, vocabularyStorage, null) { }

        public CompetitiveGroupDeleteManager(CompetitiveGroupVocDto dto, VocabularyStorage vocabularyStorage, CGApplicationDependencyVoc cgApplicationDependencyVoc)
            : base(dto, vocabularyStorage)
        {
            this.cgApplicationDependencyVoc = cgApplicationDependencyVoc;

            //CompetitiveGroupItems = new List<CompetitiveGroupItemDeleteManager>();
            //CompetitiveGroupTargets = new List<CompetitiveGroupTargetDeleteManager>();
            EntranceTestItems = new List<EntranceTestItemDeleteManager>();
            BenefitItems = new List<BenefitItemDeleteManager>();
        }

        //List<CompetitiveGroupItemDeleteManager> CompetitiveGroupItems { get; set; }
        //List<CompetitiveGroupTargetDeleteManager> CompetitiveGroupTargets { get; set; }
        List<EntranceTestItemDeleteManager> EntranceTestItems { get; set; }
        List<BenefitItemDeleteManager> BenefitItems { get; set; }

        public override void FillChildren()
        {
            //foreach (var cgi in vocabularyStorage.CompetitiveGroupItemVoc.Items.Where(cgi => cgi.CompetitiveGroupID == dto.CompetitiveGroupID))
            //{
            //    var item = new CompetitiveGroupItemDeleteManager(cgi, vocabularyStorage, cgApplicationDependencyVoc);
            //    CompetitiveGroupItems.Add(item);
            //    item.FillChildren();
            //}

            // competitiveGroup.BenefitItemC
            foreach (var cgi in vocabularyStorage.BenefitItemCVoc.Items.Where(b => b.CompetitiveGroupID == dto.CompetitiveGroupID))
            {
                var item = new BenefitItemDeleteManager(cgi, vocabularyStorage);
                BenefitItems.Add(item);
                item.FillChildren();
            }

            // competitiveGroup.EntranceTestItemC
            foreach (var eti in vocabularyStorage.EntranceTestItemCVoc.Items.Where(e => e.CompetitiveGroupID == dto.CompetitiveGroupID))
            {
                var item = new EntranceTestItemDeleteManager(eti, vocabularyStorage);
                EntranceTestItems.Add(item);
                item.FillChildren();
            }

            //var cgis = vocabularyStorage.CompetitiveGroupItemVoc.Items.Where(t => t.CompetitiveGroupID == dto.CompetitiveGroupID).Select(t => t.CompetitiveGroupItemID);
            // TODO: 
            //var cgts = vocabularyStorage.CompetitiveGroupTargetItemVoc.Items.Where(t => cgis.Contains(t.CompetitiveGroupItemID)).Select(t => t.CompetitiveGroupTargetID).Distinct();
            //foreach (var competitiveGroupTarget in vocabularyStorage.CompetitiveGroupTargetVoc.Items.Where(t => cgts.Contains(t.CompetitiveGroupTargetID)))
            //{
            //    var item = new CompetitiveGroupTargetDeleteManager(competitiveGroupTarget, vocabularyStorage, cgApplicationDependencyVoc);
            //    CompetitiveGroupTargets.Add(item);
            //    item.FillChildren();
            //}
        }

        public List<ApplicationVocDto> applications = null;

        public List<string> applicationUIDs { get { return applications.Select(t => t.UID).ToList(); } }

        /// <summary>
        /// Проверка зависимостей по данной записи. (True - нет зависимостей, False - есть зависимости)
        /// </summary>
        /// <returns>True - нет зависимостей, False - есть зависимости</returns>
        public override bool CheckDependency()
        {
            if (applications == null)
            {
                // Еще одно ускорение: если один раз уже вычисляли зависимость, то не нужно еще раз, сразу берем результаты
                Stopwatch sw = new Stopwatch();
                sw.Start();

                applications = new List<ApplicationVocDto>();
                if (cgApplicationDependencyVoc != null)
                    applications.AddRange(cgApplicationDependencyVoc.Get(dto.CompetitiveGroupID, 0));
                else
                    applications.AddRange(ADODependencyRepository.GetCGApplicationDependency(dto.CompetitiveGroupID));

                sw.Stop();
                Console.WriteLine("CompetitiveGroupDeleteManager-CheckDependency - AddRange: {0} сек", sw.Elapsed.TotalSeconds);
            }
            return !applications.Any();
        }

        /// <summary>
        /// Получить все зависимости по данной записи, включая зависимости дочерних записей
        /// </summary>
        /// <returns>True говорит о наличии зависимостей, дальше нужно запрашивать свойства Менеджера, например applicationUIDs</returns>
        public bool GetDependencies()
        {
            applications = new List<ApplicationVocDto>();
            //if (!(CompetitiveGroupItems.Any() || CompetitiveGroupTargets.Any() || EntranceTestItems.Any()))
            if (!(BenefitItems.Any() || EntranceTestItems.Any()))
                    FillChildren();

            CheckDependency();
            //foreach (var item in CompetitiveGroupItems)
            //{
            //    if (!item.CheckDependency())
            //        applications.AddRange(item.applications);
            //}
            //foreach (var item in CompetitiveGroupTargets)
            //{
            //    if (item.GetDependencies())
            //        applications.AddRange(item.applications);
            //}
            foreach (var item in EntranceTestItems)
            {
                if (!item.CheckDependency())
                    applications.AddRange(item.applications);
            }


            return applications.Any();
        }

        public override void MarkDelete()
        {
            base.MarkDelete();
            //foreach (var item in CompetitiveGroupItems)
            //    item.MarkDelete();
            //foreach (var item in CompetitiveGroupTargets)
            //    item.MarkDelete();
            foreach (var item in EntranceTestItems)
                item.MarkDelete();
            foreach (var item in BenefitItems)
                item.MarkDelete();
        }

        public override List<VocabularyBaseDto> GetDeleteObjects()
        {
            var res = base.GetDeleteObjects();
            //foreach (var item in CompetitiveGroupItems)
            //    res.AddRange(item.GetDeleteObjects());
            //foreach (var item in CompetitiveGroupTargets)
            //    res.AddRange(item.GetDeleteObjects());
            foreach (var item in EntranceTestItems)
                res.AddRange(item.GetDeleteObjects());
            foreach (var item in BenefitItems)
                res.AddRange(item.GetDeleteObjects());

            return res;
        }
    }
}
