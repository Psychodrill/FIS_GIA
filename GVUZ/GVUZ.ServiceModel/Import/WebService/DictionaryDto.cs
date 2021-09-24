using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService
{
    /// <summary>
    ///     Информация о справочнике
    /// </summary>
    [XmlRoot("Dictionary")]
    public class Dictionary //Dto <- в целях сериализации и разных типов, тут не получается написать Dto
    {
        public string Code;
        public string Name;
    }

    [XmlInclude(typeof (DictionaryDataDto))]
    [XmlInclude(typeof (DirectionDictionaryDataDto))]
    [XmlInclude(typeof (OlympicDictionaryDataDto))]
    public abstract class IDictionaryDataDto
    {
        public string Code;
        public string Name;
    }

    [XmlRoot("DictionaryData")]
    public class DictionaryDataDto : IDictionaryDataDto
    {
        [XmlArrayItem("DictionaryItem")] public DictionaryItemDto[] DictionaryItems;
    }

    public class DictionaryItemDto
    {
        public string ID;
        public string Name;
    }

    [XmlRoot("DictionaryData")]
    public class DirectionDictionaryDataDto : IDictionaryDataDto
    {
        [XmlArrayItem("DictionaryItem")] public DirectionDictionaryItemDto[] DictionaryItems;
    }

    public class DirectionDictionaryItemDto
    {
        public string Code;
        public string NewCode;
        public string DirectionID;
        public string Name;
        public string Period;
        public string QualificationCode;
        public string UGSCode;
        public string UGSName;
        public string ParentDirectionID;
    }

    [XmlRoot("DictionaryData")]
    public class OlympicDictionaryDataDto : IDictionaryDataDto
    {
        [XmlArrayItem("DictionaryItem")] public OlympicDictionaryItemDto[] DictionaryItems;
    }

    public class OlympicDictionaryItemDto
    {
        public string OlympicID;
        //public string OlympicLevelID;
        public string OlympicNumber;
        public string OlympicName;
        public string Year;
        [XmlArrayItem("Profile")] public Profile[] Profiles;
        
    }
    public class Profile
    {
        public int ProfileID { get; set; }
        [XmlArrayItem("Subject")]
        public OlympicSubject[] Subjects;
        public int LevelID { get; set; }
    }


    public class OlympicSubject
    {
        public int SubjectID { get; set; }
        //public string LevelID { get; set; }
    }
}