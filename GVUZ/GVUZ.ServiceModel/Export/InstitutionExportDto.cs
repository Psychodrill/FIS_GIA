using System.Xml.Serialization;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Export
{
    [XmlType("InstitutionExport")]
    public class InstitutionExportDto
    {
        public InstitutionExportDto()
        {
            Applications = new ApplicationDto[0];
        }

        public AdmissionVolumeCollectionDto AdmissionVolume;
        [XmlArrayItem(ElementName = "AllowedDirections")] public AllowedDirectionsDto[] AllowedDirections;

        [XmlArrayItem(ElementName = "Application")] public ApplicationDto[] Applications;
        [XmlArrayItem(ElementName = "Campaign")] public CampaignDto[] Campaigns;
        [XmlArrayItem(ElementName = "CompetitiveGroup")] public CompetitiveGroupDto[] CompetitiveGroups;
        public InstitutionDetailsDto InstitutionDetails;

        [XmlArrayItem(ElementName = "OrderOfAdmission")] public OrderOfAdmissionItemDto[] OrdersOfAdmission;
        [XmlArrayItem(ElementName = "PreparatoryCourse")] public PreparatoryCourseDto[] PreparatoryCourses;

        [XmlArrayItem(ElementName = "RecommendedList")] public RecommendedListDto[] RecommendedLists;

        public InstitutionStructureDto Structure;
    }
}