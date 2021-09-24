using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class DiplomaDocumentViewModel : BaseDocumentViewModel
    {
        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [StringLength(10)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [StringLength(6)]
        public new string DocumentSeries
        {
            get { return base.DocumentSeries; }
            set { base.DocumentSeries = value; }
        }

//        [LocalRequired]
        [StringLength(15)]
        [DisplayName("Рег. номер")]
        public string RegistrationNumber { get; set; }

        //[LocalRequired]
        [DisplayName("Квалификация")]
        public int QualificationTypeID { get; set; }

        [ScriptIgnore]
        [StringLength(200)]
        public string QualificationTypeName { get; set; }

        [XmlIgnore]
        public string QualificationCustomName { get; set; }

        [ScriptIgnore]
        public object[] QualificationList { get; set; }

        //[LocalRequired]
        [DisplayName("Направление подготовки / специальность")]
        [XmlElement(ElementName = "SpecialityID")]
        public int SpecialityTypeID { get; set; }

        [ScriptIgnore]
        [StringLength(200)]
        public string SpecialityTypeName { get; set; }

        [XmlIgnore]
        public string SpecialityCustomName { get; set; }

        [ScriptIgnore]
        public object[] SpecialityList { get; set; }

        [DisplayName("Специализация")]
        [XmlElement(ElementName = "SpecializationID")]
        public int? SpecializationTypeID { get; set; }

        [ScriptIgnore]
        public string SpecializationTypeName { get; set; }

        [ScriptIgnore]
        public object[] SpecializationList { get; set; }

        [XmlIgnore]
        public int? InstitutionID { get; set; }

        [DisplayName("ОУ, выдавшее документ")]
        [LocalRequired]
        [XmlIgnore]
        [StringLength(500)]
        public string InstitutionName { get; set; }

        [ScriptIgnore]
        public string[] InstitutionList { get; set; }

        [DisplayName("Средний балл")]
        public float? GPA { get; set; }

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            var qualificationAndSpeciality = dbContext.GetQualificationAndSpeciality(EntrantDocumentID);
            
            QualificationTypeName = qualificationAndSpeciality.Item1;
            SpecialityTypeName = qualificationAndSpeciality.Item2;

            if (!isView){
                InstitutionList = dbContext.Institution.OrderBy(x => x.FullName).Select(x => x.FullName).ToArray();
                QualificationList = dbContext.LoadQualifications().ToArray();
                SpecialityList = dbContext.FindSpecialityByQualification(QualificationTypeName).ToArray();
            }
        }

        public override void PrepareForSave(EntrantsEntities dbContext)
        {
            DocumentOrganization = InstitutionName;
            InstitutionID =
                dbContext.Institution.Where(x => x.FullName == InstitutionName)
                         .Select(x => x.InstitutionID)
                         .FirstOrDefault();
            QualificationCustomName = QualificationTypeName;
            SpecialityCustomName = SpecialityTypeID != 0 ? null : SpecialityTypeName;
        }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentEdu WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentEdu(EntrantDocumentID, RegistrationNumber, InstitutionName, QualificationName, SpecialityName, GPA)
	VALUES({0}, {1}, {2}, {3}, {4}, {5})",
                                          EntrantDocumentID,
                                          RegistrationNumber,
                                          InstitutionName,
                                          QualificationCustomName,
                                          SpecialityCustomName,
                                          GPA);
        }
    }
}

/*, {5}, {6}, {7}*/