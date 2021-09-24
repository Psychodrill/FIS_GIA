using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using FogSoft.Helpers;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class BaseDocumentViewModel
    {
        [XmlIgnore]
        [DisplayName("UID")]
        [StringLength(200)]
        public string UID { get; set; }

        [XmlIgnore]
        public int EntrantID { get; set; }

        [XmlIgnore]
        public int EntrantDocumentID { get; set; }

        [DisplayName("Тип документа")]
        [XmlIgnore]
        public int DocumentTypeID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string DocumentTypeName { get; set; }

        public string DocumentSeries { get; set; }

        [DisplayName("Серия и номер документа")]
        public string DocumentNumber { get; set; }

        [DisplayName("Дата выдачи")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime? DocumentDate { get; set; }

        [DisplayName("Кем выдан")]
        [StringLength(500)]
        public string DocumentOrganization { get; set; }

        [DisplayName("Ссылка на документ")]
        [XmlIgnore]
        public Guid DocumentAttachmentID { get; set; }

        [ScriptIgnore]
        [XmlIgnore]
        public string DocumentAttachmentName { get; set; }

        [ScriptIgnore]
        public int MaxFileSize
        {
            get { return AppSettings.Get("MaxPostFileLength", 10000); }
        }

        /// Для сохранения документа для заявления
        [XmlIgnore]
        [ScriptIgnore]
        public int ApplicationID { get; set; }


        public virtual void FillDataImportLoadSave(EntrantsEntities dbContext)
        {
            FillData(dbContext, false, null, null);
        }

        public virtual void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
        }

        public virtual void PrepareForSave(EntrantsEntities dbContext)
        {
        }

        public virtual string Validate()
        {
            return null;
        }

        public virtual void Validate(ModelStateDictionary modelStateContainer, int institutionID)
        {
            if (!String.IsNullOrEmpty(UID))
            {
                using (var dbContext = new EntrantsEntities())
                {
                    if (
                        dbContext.EntrantDocument.Any(x => x.Entrant.InstitutionID == institutionID && x.UID == UID &&
                                                           x.EntrantDocumentID != EntrantDocumentID))
                        modelStateContainer.AddModelError("UID", "Уже существует документ с данным UID.");
                }
            }
        }

        public virtual void SaveToAdditionalTable(ObjectContext dbContext)
        {
        }
    }
}