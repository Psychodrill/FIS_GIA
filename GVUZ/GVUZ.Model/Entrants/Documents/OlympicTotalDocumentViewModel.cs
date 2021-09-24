using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class OlympicTotalDocumentViewModel : BaseDocumentViewModel
    {
        [ScriptIgnore] public string[] SubjectNameList;

        [DisplayName("Серия")]
        [LocalRequired]
        [StringLength(6)]
        public new string DocumentSeries
        {
            get { return base.DocumentSeries; }
            set { base.DocumentSeries = value; }
        }

        [DisplayName("№")]
        [LocalRequired]
        [StringLength(20)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [DisplayName("Место проведения")]
        //[LocalRequired]
        [StringLength(500)]
        [XmlIgnore]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }

        [ScriptIgnore]
        [StringLength(255)]
        public string OlympicPlace
        {
            get { return DocumentOrganization; }
            set { DocumentOrganization = value; }
        }

        [ScriptIgnore]
        public DateTime? OlympicDate
        {
            get { return DocumentDate; }
            set { DocumentDate = value; }
        }

        [DisplayName("Дата проведения")]
        //[LocalRequired]
        [XmlIgnore]
        [XmlElement(DataType = "string")]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [DisplayName("Тип диплома")]
        public int? DiplomaTypeID { get; set; }

        [ScriptIgnore]
        public string DiplomaTypeName { get; set; }

        [ScriptIgnore]
        public object[] DiplomaList { get; set; }

        public SubjectBriefData[] Subjects { get; set; }

        //для отображения текста во вьюхе
        [ScriptIgnore]
        [DisplayName("Предметы")]
        public SubjectBriefData SubjectDetails { get; set; }


        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            if (!isView)
            {
                DiplomaList =
                    dbContext.OlympicDiplomType.Where(x => x.OlympicDiplomTypeID != 3)
                             .OrderBy(x => x.Name)
                             .Select(x => new {ID = x.OlympicDiplomTypeID, x.Name})
                             .ToArray();

                SubjectNameList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).Select(x => x.Value).ToArray();
                if (Subjects == null) Subjects = new SubjectBriefData[0];
                var subjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).ToArray();
                foreach (SubjectBriefData data in Subjects)
                {
                    SubjectBriefData data1 = data;
                    data.SubjectName =
                        subjects.Where(x => x.Key == data1.SubjectID).Select(x => x.Value).FirstOrDefault();
                }
            }
            else
            {
                var subjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).ToArray();
                foreach (SubjectBriefData data in Subjects)
                {
                    SubjectBriefData data1 = data;
                    data.SubjectName =
                        subjects.Where(x => x.Key == data1.SubjectID).Select(x => x.Value).FirstOrDefault();
                }
                DiplomaTypeName =
                    dbContext.OlympicDiplomType.Where(x => x.OlympicDiplomTypeID == DiplomaTypeID)
                             .Select(x => x.Name)
                             .FirstOrDefault();
            }
        }

        public override void PrepareForSave(EntrantsEntities dbContext)
        {
            var subjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).ToArray();
            Subjects = (from data in Subjects
                        let f = subjects.FirstOrDefault(x => x.Value == data.SubjectName)
                        where f.Value != null
                        select new SubjectBriefData {SubjectID = f.Key}).ToArray();
        }

        public override string Validate()
        {
            var errors = new StringBuilder();
            var subjects = new HashSet<string>();
            if (Subjects == null) Subjects = new SubjectBriefData[0];
            foreach (SubjectBriefData data in Subjects)
            {
                if (subjects.Contains(data.SubjectName))
                    errors.AppendLine("Дублируется название дисциплины: " + data.SubjectName);
                else
                    subjects.Add(data.SubjectName);
            }
            if (Subjects.Length == 0)
                errors.AppendLine("Хотя бы одна дисциплина обязательна");

            return errors.ToString();
        }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            var subjectsPart = new StringBuilder();
            if (Subjects != null)
                foreach (SubjectBriefData subjectData in Subjects)
                {
                    subjectsPart.AppendFormat(
                        "INSERT INTO EntrantDocumentEgeAndOlympicSubject(EntrantDocumentID, SubjectID) VALUES({0}, {1})\r\n",
                        EntrantDocumentID, subjectData.SubjectID);
                }
            dbContext.ExecuteStoreCommand(@"
	            DELETE FROM EntrantDocumentOlympicTotal WHERE EntrantDocumentID={0}
	            DELETE FROM EntrantDocumentEgeAndOlympicSubject WHERE EntrantDocumentID={0}
	            INSERT INTO EntrantDocumentOlympicTotal(EntrantDocumentID, DiplomaTypeID, OlympicPlace, OlympicDate)
	            VALUES({0}, {1}, {2}, {3})
	            " + subjectsPart,
                                          EntrantDocumentID,
                                          DiplomaTypeID,
                                          OlympicPlace,
                                          OlympicDate);
        }

        public class SubjectBriefData
        {
            public int SubjectID { get; set; }

            [DisplayName("Дисциплина")]
            public string SubjectName { get; set; }
        }
    }
}