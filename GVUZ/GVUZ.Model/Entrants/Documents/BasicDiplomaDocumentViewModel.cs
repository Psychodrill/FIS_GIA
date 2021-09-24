using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class BasicDiplomaDocumentViewModel : BaseDocumentViewModel
    {
        public BasicDiplomaDocumentViewModel()
        {
            DocumentTypeID = 6;
        }

        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [StringLength(15)]
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
        public string QualificationTypeName { get; set; }

        [XmlIgnore]
        public string QualificationCustomName { get; set; }

        [ScriptIgnore]
        public object[] QualificationList { get; set; }

        //[LocalRequired]
        [DisplayName("Профессия")]
        [XmlElement(ElementName = "ProfessionID")]
        public int ProfessionTypeID { get; set; }

        [ScriptIgnore]
        public string ProfessionTypeName { get; set; }

        [XmlIgnore]
        public string ProfessionCustomName { get; set; }

        [ScriptIgnore]
        public object[] ProfessionList { get; set; }

        [XmlIgnore]
        public int? InstitutionID { get; set; }

        [DisplayName("ОУ, выдавшее документ")]
        [LocalRequired]
        [StringLength(500)]
        public string InstitutionName { get; set; }

        [ScriptIgnore]
        public string[] InstitutionList { get; set; }

        [DisplayName("Средний балл")]
        public float? GPA { get; set; }

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            if (!isView)
            {
                QualificationTypeName = QualificationCustomName;
                ProfessionTypeName = ProfessionCustomName;
                InstitutionList = dbContext.Institution.OrderBy(x => x.FullName).Select(x => x.FullName).ToArray();
            }
            else
            {
                QualificationTypeName = QualificationCustomName;
                ProfessionTypeName = ProfessionCustomName;
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
            ProfessionCustomName = ProfessionTypeName;
        }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentEdu WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentEdu(EntrantDocumentID, RegistrationNumber, QualificationName, SpecialityName, GPA)
	VALUES({0}, {1}, {2}, {3}, {4})",
                                          EntrantDocumentID,
                                          RegistrationNumber,
                                          QualificationCustomName,
                                          ProfessionCustomName,
                                          GPA);
        }

        public override void Validate(ModelStateDictionary modelState, int institutionID)
        {
            base.Validate(modelState, institutionID);
            //теперь не обязательны
            //а теперь могут быть вообще любые
            //if (ProfessionTypeID < 1 && !String.IsNullOrEmpty(ProfessionTypeName)) modelState.AddModelError("ProfessionTypeID", "Некорректная профессия");
            //if (QualificationTypeID < 1 && !String.IsNullOrEmpty(QualificationTypeName)) modelState.AddModelError("QualificationTypeID", "Некорректная квалификация");
        }
    }
}