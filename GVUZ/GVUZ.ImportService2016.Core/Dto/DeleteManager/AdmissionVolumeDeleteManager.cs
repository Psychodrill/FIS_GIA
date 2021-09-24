using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class AdmissionVolumeDeleteManager : BaseDeleteManager<AdmissionVolumeVocDto>
    {
        public AdmissionVolumeDeleteManager(AdmissionVolumeVocDto dto, VocabularyStorage vocabularyStorage)
            : base(dto, vocabularyStorage)
        {

            CompetitiveGroups = new List<CompetitiveGroupDeleteManager>();
        }

        List<CompetitiveGroupDeleteManager> CompetitiveGroups { get; set; }

        public override void FillChildren()
        {
            // TODO:
            // А заодно нужно удалить все CompetitiveGroups с таким же DirectionID и CompetitiveGroup.CampaignID
            var cgis = vocabularyStorage.CompetitiveGroupVoc.Items.Where(i => i.DirectionID == dto.DirectionID
                                            && vocabularyStorage.CompetitiveGroupVoc.Items.Any(cg => cg.CompetitiveGroupID == i.CompetitiveGroupID
                                            && cg.CampaignID == dto.CampaignID)).ToList();
            foreach (var cgi in cgis)
            {
                var item = new CompetitiveGroupDeleteManager(cgi, vocabularyStorage);
                CompetitiveGroups.Add(item);
                item.FillChildren();
            }
        }

        public override bool CheckDependency()
        {
            return true;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetDependencies()
        {
            List<string> applicationUIDs = new List<string>();
            if (!CompetitiveGroups.Any()) 
                FillChildren();

            foreach (var item in CompetitiveGroups)
            {
                if (!item.CheckDependency())
                    applicationUIDs.AddRange(item.applicationUIDs);
            }
            return applicationUIDs;
        }


        public override void MarkDelete()
        {
            base.MarkDelete();
            foreach (var item in CompetitiveGroups)
                item.MarkDelete();
        }

        public override List<VocabularyBaseDto> GetDeleteObjects()
        {
            var res= base.GetDeleteObjects();
            foreach (var item in CompetitiveGroups)
                res.AddRange(item.GetDeleteObjects());

            return res;
        }
    }
}
