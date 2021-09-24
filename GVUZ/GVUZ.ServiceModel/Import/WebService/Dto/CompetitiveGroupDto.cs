using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    [Description("Конкурс")]
    public class CompetitiveGroupDto : BaseDto
    {
        public string CampaignUID;
        [XmlArrayItem(ElementName = "CommonBenefitItem")] public BenefitItemDto[] CommonBenefit;
        public int CompetitiveGroupID;
        public string Course;

        [XmlArrayItem(ElementName = "CompetitiveGroupItem")] public CompetitiveGroupItemDto[] Items;
        public string Name;
        [XmlArrayItem(ElementName = "TargetOrganization")] public CompetitiveGroupTargetDto[] TargetOrganizations;
        [XmlIgnore] public bool UseAnyDirectionsFilter;

        private EntranceTestItemDto[] _entranceTestItems;

        [XmlArrayItem(ElementName = "EntranceTestItem")]
        public EntranceTestItemDto[] EntranceTestItems
        {
            get { return _entranceTestItems ?? new EntranceTestItemDto[0]; }
            set { _entranceTestItems = value; }
        }
    }

    [Description("Направление конкурса")]
    public class CompetitiveGroupItemDto : BaseDto
    {
        public string CompetitiveGroupItemID;
        [XmlIgnore] public string CompetitiveGroupName;
        [XmlIgnore] public string DirectionCode;
        public string DirectionID;
        [XmlIgnore] public string DirectionName;
        public string EducationLevelID;
        public string ParentDirectionID;

        public string NumberBudgetO;
        public string NumberBudgetOZ;
        public string NumberBudgetZ;
        public string NumberPaidO;
        public string NumberPaidOZ;
        public string NumberPaidZ;
        public string NumberQuotaO;
        public string NumberQuotaOZ;
        public string NumberQuotaZ;
    }

    [Description("Организация целевого приема")]
    public class CompetitiveGroupTargetDto : BaseDto
    {
        [XmlArrayItem(ElementName = "CompetitiveGroupTargetItem")] public CompetitiveGroupTargetItemDto[] Items;
        public string TargetOrganizationName;
    }

    [Description("Места для целевого приема")]
    public class CompetitiveGroupTargetItemDto : BaseDto
    {
        [XmlIgnore] public string CompetitiveGroupUID;
        public string DirectionID;
        public string EducationLevelID;
        public string NumberTargetO;
        public string NumberTargetOZ;
        public string NumberTargetZ;
    }

    [Description("Льгота")]
    public class BenefitItemDto : BaseDto
    {
        // Используется для фильтрации не импортируется.
        //public string[] OlympicLevelFlags;
        public string BenefitKindID;
        [XmlIgnore] public bool IsCommonBenefit;
        public string IsForAllOlympics;
        
        // Балл для льготы по предмету
        public string MinEgeMark;

        [XmlArrayItem(ElementName = "OlympicDiplomTypeID")]
        public string[] OlympicDiplomTypes;
        public int OlympicYear;

        [XmlArrayItem(ElementName = "Olympic")]
        public OlympicDto[] OlympicsLevels;

        [XmlArrayItem(ElementName = "OlympicID")]
        public string[] Olympics;

        [XmlArrayItem(ElementName = "MinMarks")]
        public MinEgeScoresDto[] MinEgeMarks;

        public int OlympicDiplomTypesParsed
        {
            get { return (OlympicDiplomTypes ?? new string[0]).Aggregate(0, (i, s) => i | s.To(0)); }
        }
    }

    public class OlympicDto : IEquatable<OlympicDto>
    {
        public string OlympicID;
        public string LevelID;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(OlympicDto)) return false;
            return Equals((OlympicDto)obj);
        }

        public bool Equals(OlympicDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                string.Equals(other.OlympicID, OlympicID) &&
                string.Equals(other.LevelID, LevelID);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + (OlympicID != null ? OlympicID.GetHashCode() : 0);
                result = result * 37 + (LevelID != null ? LevelID.GetHashCode() : 0);
                return result;
            }
        }
    }

    [Description("Вступительные испытания конкурса")]
    public class EntranceTestItemDto : BaseDto
    {
        [XmlArrayItem(ElementName = "EntranceTestBenefitItem")] public BenefitItemDto[] EntranceTestBenefits;
        public EntranceTestSubjectDto EntranceTestSubject;
        public string EntranceTestTypeID;
        public string Form;
        public string MinScore;
        public string EntranceTestPriority;
        public bool IsFirst;
        public bool IsSecond;
    }

    public static partial class Extensions
    {
        
    }

    [Description("Минимальные баллы для общей льготы")]
    public class MinEgeScoresDto : BaseDto
    {
        public string SubjectID;
        public string MinMark;
    }
}