using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class GiaDocumentViewModel : BaseDocumentViewModel
    {
        [ScriptIgnore] public string[] SubjectNameList;

        public GiaDocumentViewModel()
        {
            DocumentTypeID = 17;
        }

        [DisplayName(@"Номер документа")]
        [LocalRequired]
        [StringLength(10)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [LocalRequired]
        [DisplayName(@"Дата выдачи")]
        public new DateTime? DocumentDate
        {
            get { return base.DocumentDate; }
            set { base.DocumentDate = value; }
        }

        [LocalRequired]
        [DisplayName(@"Кем выдана")]
        public new string DocumentOrganization
        {
            get { return base.DocumentOrganization; }
            set { base.DocumentOrganization = value; }
        }

        public SubjectData[] Subjects { get; set; }

        //для отображения текста во вьюхе
        [ScriptIgnore]
        [DisplayName(@"Предметы")]
        public SubjectData SubjectDetails { get; set; }

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            if (!isView)
            {
                SubjectNameList =
                    DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject) /*.Where(x => x.IsEge)*/.OrderBy(x => x.Value).Select(x => x.Value).ToArray();
                if (Subjects == null) Subjects = new SubjectData[0];
                var subjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).ToArray();
                foreach (SubjectData data in Subjects)
                {
                    SubjectData data1 = data;
                    data.SubjectName =
                        subjects.Where(x => x.Key == data1.SubjectID).Select(x => x.Value).FirstOrDefault();
                }
            }
            else
            {
                Subject[] subjects = dbContext.Subject.OrderBy(x => x.Name).ToArray();
                foreach (SubjectData data in Subjects)
                {
                    SubjectData data1 = data;
                    data.SubjectName =
                        subjects.Where(x => x.SubjectID == data1.SubjectID).Select(x => x.Name).FirstOrDefault();
                }
            }
        }

        public override void PrepareForSave(EntrantsEntities dbContext)
        {
//			DocumentOrganization = "";
            var subjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).ToArray();
            Subjects = (from data in Subjects
                        let f = subjects.FirstOrDefault(x => x.Value.Equals(data.SubjectName, StringComparison.CurrentCultureIgnoreCase))
                        where f.Value != null
                        select new SubjectData {SubjectID = f.Key, Value = data.Value}).ToArray();
        }

        public override void Validate(ModelStateDictionary modelState, int institutionID)
        {
            base.Validate(modelState, institutionID);
        }

        public override string Validate()
        {
            var errors = new StringBuilder();
            var subjects = new HashSet<string>();
            if (Subjects == null) Subjects = new SubjectData[0];
            foreach (SubjectData data in Subjects)
            {
                if (subjects.Contains(data.SubjectName, StringComparer.CurrentCultureIgnoreCase))
                    errors.AppendLine("Дублируется название дисциплины: " + data.SubjectName);
                else
                    subjects.Add(data.SubjectName);
                if (data.Value < 1 || data.Value > 100)
                    errors.AppendLine("Некорректный балл для дисциплины: " + data.SubjectName);
            }
            return errors.ToString();
        }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            var subjectsPart = new StringBuilder();
            if (Subjects != null)
                foreach (SubjectData subjectData in Subjects)
                {
                    subjectsPart.AppendFormat(
                        "INSERT INTO EntrantDocumentEgeAndOlympicSubject(EntrantDocumentID, SubjectID, Value) VALUES({0}, {1}, {2})\r\n",
                        EntrantDocumentID, subjectData.SubjectID,
                        subjectData.Value.ToString(CultureInfo.InvariantCulture));
                }
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentEgeAndOlympicSubject WHERE EntrantDocumentID={0}
	" + subjectsPart,
                                          EntrantDocumentID);
        }

        public class SubjectData
        {
            public int SubjectID { get; set; }

            [DisplayName(@"Дисциплина")]
            public string SubjectName { get; set; }

            [DisplayName(@"Балл")]
            public decimal Value { get; set; }
        }
    }
}