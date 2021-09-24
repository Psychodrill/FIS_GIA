using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class DisabilityDocumentViewModel : BaseDocumentViewModel
    {
        public DisabilityDocumentViewModel()
        {
            DocumentTypeID = 11;
        }

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

        [LocalRequired]
        [DisplayName("Кем выдана")]
        [StringLength(500)]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }

        [LocalRequired]
        [DisplayName("Группа")]
        public int DisabilityTypeID { get; set; }

        public string DisabilityTypeName { get; set; }

        [ScriptIgnore]
        public object[] DisabilityList { get; set; }


        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            if (!isView)
                DisabilityList =
                    DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DisabilityType).OrderBy(x => x.Value)
                             .Select(x => new {ID = x.Key, Name = x.Value})
                             .ToArray();
            else
                DisabilityTypeName =
                    DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DisabilityType).Where(x => x.Key == DisabilityTypeID)
                             .Select(x => x.Value)
                             .FirstOrDefault();
        }

        public override void PrepareForSave(EntrantsEntities dbContext)
        {
        }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentDisability WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentDisability(EntrantDocumentID, DisabilityTypeID)
	VALUES({0}, {1})",
                                          EntrantDocumentID,
                                          DisabilityTypeID);
        }
    }
}