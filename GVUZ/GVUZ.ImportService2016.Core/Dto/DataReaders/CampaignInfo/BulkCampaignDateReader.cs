using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkCampaignDateReader : BulkReaderBase<BulkCampaignDateDto>
    {
        public BulkCampaignDateReader(PackageData packageData) //, VocabularyStorage vocabularyStorage)
        {
            foreach(var campaign in packageData.CampaignsToImport()){
                _records.AddRange(BulkCampaignDateDto.GetItems(campaign)); //, vocabularyStorage));
            }
            
            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.CampaignGuid);

            AddGetter("Course", dto => dto.Course);
            AddGetter("EducationLevelID", dto => dto.EducationLevelID);
            AddGetter("EducationFormID", dto => dto.EducationFormID);
            AddGetter("DateStart", dto => GetDateOrNull(dto.DateStart)); // dto.DateStart.Year > 1977 ? dto.DateStart : (object)DBNull.Value);
            AddGetter("DateEnd", dto => GetDateOrNull(dto.DateEnd));
            AddGetter("DateOrder", dto => GetDateOrNull(dto.DateOrder));
            AddGetter("UID", dto => dto.UID);
            AddGetter("IsActive", dto => true);
            AddGetter("Stage", dto => dto.Stage);
            AddGetter("EducationSourceID", dto => dto.EducationSourceID);

            AddGetter("ImportPackageID", dto => packageData.ImportPackageId);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
        }
    }

    public class BulkCampaignDateDto : ImportBase
    {
        public BulkCampaignDateDto() { }

        public BulkCampaignDateDto(PackageDataCampaignInfoCampaign campaign, uint eduLevel)
        {
            this.CampaignGuid = campaign.GUID;
            //this.Course = eduLevel.Course.To(0);
            this.EducationLevelID =  eduLevel.To(0);
            // остальное 0 или null
        }

        //public BulkCampaignDateDto(PackageDataCampaignInfoCampaign campaign, PackageDataCampaignInfoCampaignEducationLevel eduLevel)
        //{
        //    this.CampaignGuid = campaign.GUID;
        //    this.Course = eduLevel.Course.To(0);
        //    this.EducationLevelID = eduLevel.EducationLevelID.To(0);
        //    // остальное 0 или null
        //}

        //public BulkCampaignDateDto(PackageDataCampaignInfoCampaign campaign, PackageDataCampaignInfoCampaignCampaignDate campaignDate)
        //{
        //    this.CampaignGuid = campaign.GUID;
        //    this.UID = campaignDate.UID;
        //    this.Course = campaignDate.Course.To(0);
        //    this.EducationLevelID = campaignDate.EducationLevelID.To(0);
        //    this.DateStart = campaignDate.DateStart;
        //    this.DateEnd = campaignDate.DateEnd;
        //    this.DateOrder = campaignDate.DateOrder;
        //    this.Stage = campaignDate.Stage.To(0);
        //    this.EducationFormID = campaignDate.EducationFormID.To(0);
        //    this.EducationSourceID = campaignDate.EducationSourceID.To(0);
        //}

        public static List<BulkCampaignDateDto> GetItems(PackageDataCampaignInfoCampaign campaign)
        {
            List<BulkCampaignDateDto> res = new List<BulkCampaignDateDto>();
            if (campaign.EducationLevels!=null)
                foreach (var eduLevel in campaign.EducationLevels)
                {
                    res.Add(new BulkCampaignDateDto(campaign, eduLevel));
                }
            //if (campaign.CampaignDates != null)
            //    foreach (var campaignDate in campaign.CampaignDates)
            //    {
            //        res.Add(new BulkCampaignDateDto(campaign, campaignDate));
            //    }

            return res;
        }

        public Guid CampaignGuid { get; set; }
        public string UID { get; set; }
        public int Course { get; set; }
        public int EducationLevelID { get; set; }
        public int EducationFormID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateOrder { get; set; }
        public int Stage { get; set; }
        public int EducationSourceID { get; set; }
    }
}
