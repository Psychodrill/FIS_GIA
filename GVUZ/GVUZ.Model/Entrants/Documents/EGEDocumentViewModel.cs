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
    public class EGEDocumentViewModel : BaseDocumentViewModel
    {
        [ScriptIgnore]
        public string[] SubjectNameList;

        public EGEDocumentViewModel()
        {
            DocumentTypeID = 2;
        }

        [DisplayName(@"Номер свидетельства")]
        [LocalRequired]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        //[LocalRequired]
        //[DisplayName(@"Дата выдачи")]
        //public new DateTime? DocumentDate
        //{
        //    get { return base.DocumentDate; }
        //    set { base.DocumentDate = value; }
        //}

        [LocalRequired]
        [DisplayName(@"Год выдачи")]
        [LocalRange(2000, 2100)]
        public int? DocumentYear
        {
            get { return base.DocumentDate.HasValue ? base.DocumentDate.Value.Year : (int?)null; }
            set { base.DocumentDate = value.HasValue ? new DateTime(value.Value, 1, 1) : (DateTime?)null; }
        }

        [StringLength(8)]
        [DisplayName(@"Типографский номер")]
        [LocalRegex("\\d{7,8}")]
        public string TypographicNumber { get; set; }

        /*
                [DisplayName(@"По решению")]
                [StringLength(200)]
                public string Decision { get; set; }

                //[LocalRequired]
                [StringLength(20)]
                [DisplayName(@"Номер решения")]
                public string DecisionNumber { get; set; }

                //[LocalRequired]
                [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
                [Date(">today-100y")]
                [Date("<=today")]
                [DisplayName(@"Дата решения")]
                public DateTime? DecisionDate { get; set; }
        */

        /*
                [DisplayName("Профессия")]
                public int? ProfessionTypeID { get; set; }

                [ScriptIgnore]
                public string ProfessionTypeName { get; set; }

                [ScriptIgnore]
                public object[] ProfessionList { get; set; }
        */

        public SubjectData[] Subjects { get; set; }

        //для отображения текста во вьюхе
        [ScriptIgnore]
        [DisplayName(@"Предметы")]
        public SubjectData SubjectDetails { get; set; }

        public override void FillDataImportLoadSave(EntrantsEntities dbContext)
        {
            Subject[] subjects = dbContext.Subject.OrderBy(x => x.Name).ToArray();
            foreach (SubjectData data in Subjects)
            {
                SubjectData data1 = data;
                data.SubjectName =
                    subjects.Where(x => x.SubjectID == data1.SubjectID).Select(x => x.Name).FirstOrDefault();
            }
        }

        public void FillData(EntrantsEntities dbContext, IEnumerable<Subject> dbSubjects, bool isView, int? competitiveGroupId, int? subjectId)
        {
            if (!isView)
            {
                SubjectNameList =
                    dbSubjects.Where(x => x.IsEge).OrderBy(x => x.Name).Select(x => x.Name).ToArray();
                //ProfessionList = professionList;
                if (Subjects == null) Subjects = new SubjectData[0];
                Subject[] subjects = dbSubjects.OrderBy(x => x.Name).ToArray();
                foreach (SubjectData data in Subjects)
                {
                    SubjectData data1 = data;
                    data.SubjectName =
                        subjects.Where(x => x.SubjectID == data1.SubjectID).Select(x => x.Name).FirstOrDefault();
                }
            }
            else
            {
                //ProfessionTypeName =
                //	dbContext.ProfessionType.Where(x => x.ProfessionID == ProfessionTypeID).Select(x => x.Name + " " + x.Code).FirstOrDefault();
                Subject[] subjects = dbSubjects.OrderBy(x => x.Name).ToArray();
                foreach (SubjectData data in Subjects)
                {
                    SubjectData data1 = data;
                    data.SubjectName =
                        subjects.Where(x => x.SubjectID == data1.SubjectID).Select(x => x.Name).FirstOrDefault();
                }
            }
        }

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            FillData(dbContext, dbContext.Subject, isView, competitiveGroupId, subjectId);
        }

        public override void PrepareForSave(EntrantsEntities dbContext)
        {
            DocumentOrganization = "";
            Subject[] subjects = dbContext.Subject.OrderBy(x => x.Name).ToArray();
            Subjects = (from data in Subjects
                        let f =
                            subjects.FirstOrDefault(
                                x => x.Name.Equals(data.SubjectName, StringComparison.CurrentCultureIgnoreCase))
                        where f != null
                        select new SubjectData { SubjectID = f.SubjectID, Value = data.Value }).ToArray();
        }

        public override void Validate(ModelStateDictionary modelState, int institutionID)
        {
            base.Validate(modelState, institutionID);
            //if (DecisionDate.HasValue && DocumentDate.HasValue)
            //{
            //    if (DecisionDate > DocumentDate)
            //        modelState.AddModelError("DecisionDate", "Дата решения не может быть больше даты выдачи");
            //}
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
	DELETE FROM EntrantDocumentEge WHERE EntrantDocumentID={0}
	DELETE FROM EntrantDocumentEgeAndOlympicSubject WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentEge(EntrantDocumentID, DecisionNumber, DecisionDate, TypographicNumber, Decision)
	VALUES({0}, {1}, {2}, {3}, {4})
	" + subjectsPart,
                                          EntrantDocumentID,
                /*DecisionNumber*/null,
                /*DecisionDate*/null,
                                          TypographicNumber,
                /*Decision*/null);
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