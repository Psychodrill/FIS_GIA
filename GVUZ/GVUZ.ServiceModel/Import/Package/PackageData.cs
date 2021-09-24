using System;
using System.Xml.Serialization;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Package
{
    /// <summary>
    ///     Объект для обмена данными с клиентом.
    ///     Используется веб-сервисом для получения информации о ВУЗе.
    /// </summary>
    public class PackageData
    {
        public AdmissionInfoDto AdmissionInfo;

        [XmlArrayItem(ElementName = "Application")] public ApplicationDto[] Applications;
        public CampaignInfoDto CampaignInfo;

        ///// <summary>
        /////     Списки рассматриваемых заявлений абитуриентов
        ///// </summary>
        //[XmlArrayItem(ElementName = "ConsiderListItem")] public ConsideredApplicationDto[] ConsiderList;

        [XmlArrayItem(ElementName = "Orders")]
        public OrdersDto[] Orders;

        //[Obsolete("Списки рекомендованных к зачислению реализованы через RecommendedListDto")]
        ///// <summary>
        /////     Списки заявлений абитуриентов, рекомендованных к зачислению
        ///// </summary>
        //[XmlArrayItem(ElementName = "RecommendedListItem")] public RecommendedApplicationDto[] RecommendedList;

        ///// <summary>
        ///// Списки лиц, рекомендованных к зачислению. Новая редакция. Используется только она.
        ///// </summary>
        //[XmlArrayItem(ElementName = "RecommendedList")]
        //public RecommendedListDto[] RecommendedLists;

        /// <summary>
        /// Списки лиц, рекомендованных к зачислению. Новая редакция. Используется только она.
        /// </summary>
        [XmlArrayItem(ElementName = "InstitutionAchievement")]
        public InstitutionAchievementDto[] InstitutionAchievements;

        /* Такой вот хак :) */
        private ApplicationDto _application;

        public ApplicationDto Application
        {
            get { return _application; }
            set
            {
                _application = value;
                Applications = new[] {value};
            }
        }
    }

    public class AdmissionInfoDto
    {
        [XmlArrayItem(ElementName = "Item")] public AdmissionVolumeDto[] AdmissionVolume;

        [XmlArrayItem(ElementName = "CompetitiveGroup")] public CompetitiveGroupDto[] CompetitiveGroups;

        [XmlArrayItem(ElementName = "Item")] public DistributedAdmissionVolumeDto[] DistributedAdmissionVolume;
        
    }

    public class OrdersDto
    {
        //[XmlArrayItem(ElementName = "OrderOfAdmission")]
        //public OrdersOfAdmissionItemDto[] OrdersOfAdmission;
    }

    [Serializable]
    [XmlRoot("PackageData")]
    public class InstitutionInformationFilter
    {
        public string InstitutionDetails { get; set; }
        public string AllowedDirections { get; set; }
        public string DistributedAdmissionVolume { get; set; }
		public string InstitutionAchievements { get; set; }

		 
        [XmlArrayItem(ElementName = "CampaignUID")]
        public string[] Campaigns { get; set; }

        [XmlArrayItem(ElementName = "CampaignUID")]
        public string[] AdmissionVolume { get; set; }

        [XmlArrayItem(ElementName = "CampaignUID")]
        public string[] CompetitiveGroups { get; set; }

        [XmlArrayItem(ElementName = "StatusOrUID")]
        public StatusOrUid[] Applications { get; set; }

        [XmlArrayItem(ElementName = "CampaignUID")]
        public string[] OrdersOfAdmission { get; set; }

        [XmlArrayItem(ElementName = "ApplicationsInOrders")]
        public string[] ApplicationsInOrders { get; set; }

        public string Structure { get; set; }
        public string RecommendedLists { get; set; }

        public bool IsEmpty
        {
            get
            {
                return
                    InstitutionDetails == null &&
                    AllowedDirections == null &&
                    Campaigns == null &&
                    AdmissionVolume == null &&
                    CompetitiveGroups == null &&
                    Applications == null &&
                    OrdersOfAdmission == null &&
                    Structure == null &&
                    RecommendedLists == null &&
                    DistributedAdmissionVolume == null &&
                    ApplicationsInOrders == null; 
            }
        }
    }

    public class StatusOrUid
    {
        public string UID { get; set; }
        public int? StatusID { get; set; }
    }

    [Serializable]
    public class Apps
    {
        [XmlArrayItem(ElementName = "StatusOrUID")]
        public StatusOrUid[] StatusOrUIDs { get; set; }

        [XmlArrayItem(ElementName = "CampaignUID")]
        public string[] CampaignUIDs { get; set; }
    }
}