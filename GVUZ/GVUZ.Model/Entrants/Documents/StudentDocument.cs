using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class StudentDocumentViewModel : BaseDocumentViewModel
    {
        public StudentDocumentViewModel()
        {
            DocumentTypeID = 18;
        }

        [LocalRequired]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [StringLength(10)]
        [DisplayName("Номер документа")]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [LocalRequired]
        [DisplayName("Кем выдан")]
        [StringLength(500)]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }

        [ScriptIgnore]
        public string[] InstitutionList { get; set; }

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            InstitutionList =
                dbContext.Institution.Where(x => x.InstitutionTypeID == 1)
                         .OrderBy(x => x.FullName)
                         .Select(x => x.FullName)
                         .ToArray();
        }

        /*public override void SaveToAdditionalTable(ObjectContext dbContext)
		{
		}*/
    }
}