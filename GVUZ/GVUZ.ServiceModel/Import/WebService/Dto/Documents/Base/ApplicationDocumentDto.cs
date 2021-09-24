using System.ComponentModel;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base
{
    /// <summary>
    ///     Класс отвечает сразу за несколько типов документов.
    ///     Содержит поля от различных типов документов за исключением IdentityDocumentDto и
    /// </summary>
    [Description("Документ")]
    public abstract class ApplicationDocumentDto : BaseDocumentDto
    {
        public string AdditionalInfo;
        public string DiplomaTypeID;
        public string DisabilityTypeID;
        public string DocumentSeries;
        public string DocumentTypeNameText;
        public string EndYear;
        public string OlympicDate;
        public string OlympicID;
        public string LevelID;
        public string OlympicPlace;
        public string ProfessionID;
        public string QualificationTypeID;
        public string RegistrationNumber;
        public string SpecialityID;
        public string SpecializationID;

        //public string DecisionNumber;
        //public string DecisionDate;
        public string SubjectID;

        public string SubjectName;

        [XmlArrayItem(ElementName = "SubjectBriefData")] public SubjectBriefDataDto[] Subjects =
            new SubjectBriefDataDto[0];

        public string Value;
        public float? GPA { get; set; }
    }
}