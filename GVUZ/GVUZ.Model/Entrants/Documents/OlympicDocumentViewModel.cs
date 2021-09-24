using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    public class OlympicDocumentViewModel : BaseDocumentViewModel
    {
        [DisplayName("Серия")]
        [LocalRequired]
        public new string DocumentSeries
        {
            get { return base.DocumentSeries; }
            set { base.DocumentSeries = value; }
        }

        [DisplayName("Номер")]
        [LocalRequired]
        [StringLength(20)]
        public new string DocumentNumber
        {
            get { return base.DocumentNumber; }
            set { base.DocumentNumber = value; }
        }

        [DisplayName("Тип диплома")]
        [LocalRequired]
        public int? DiplomaTypeID { get; set; }

        [ScriptIgnore]
        public string DiplomaTypeName { get; set; }

        [ScriptIgnore]
        public object[] DiplomaList { get; set; }

        [DisplayName("Олимпиада")]
        [LocalRequired]
        public int OlympicID { get; set; }

        [ScriptIgnore]
        public OlympicData[] OlympicDatas { get; set; }

        //для отображения текста во вьюхе
        [ScriptIgnore]
        public OlympicData OlympicDetails { get; set; }

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
        {
            // HACK: убрали, чтобы посмотреть, к чему приведёт
            //if (!isView)
            //{
            //    DiplomaList = dbContext.OlympicDiplomType.Where(x => x.OlympicDiplomTypeID != 3)
            //                 .OrderBy(x => x.Name)
            //                 .Select(x => new {ID = x.OlympicDiplomTypeID, x.Name})
            //                 .ToArray();
            //    var query = (from x in dbContext.OlympicType                           
            //            // join y in dbContext.OlympicTypeSubjectLink on x.OlympicID equals y.OlympicID
            //             join z in dbContext.Subject on x.SubjectID equals z.SubjectID
            //             join l in dbContext.OlympicLevel on x.OlympicLevelID equals l.OlympicLevelID into tmp from l0 in tmp.DefaultIfEmpty()
            //             join m in dbContext.OlympicLevel on y.SubjectLevelID equals m.OlympicLevelID into tmp2 from m0 in tmp2.DefaultIfEmpty()
            //             orderby x.Name, z.Name
            //             select new
            //                 {
            //                     x.OlympicID,
            //                     x.Name,
            //                     x.OrganizerName,
            //                     SubjectName = z.Name,
            //                     LevelName = (l0 != null ? l0.Name : m0.Name),
            //                     LevelID = (l0 != null ? l0.OlympicLevelID : m0.OlympicLevelID),
            //                     x.OlympicYear
            //                 });

            //    var allowedLevels = new List<short>();
            //    if (competitiveGroupId.HasValue)
            //    {
            //        /* Берем все разрешенные олимпиады по этой КГ - не для общих льгот */
            //        var benefitItemCQuery = dbContext.BenefitItemCOlympicType
            //            .Include("BenefitItemC")
            //            .Where(c => c.BenefitItemC.CompetitiveGroupID == competitiveGroupId.Value);

            //        /* Если это не общая олимпиада */
            //        if (subjectId.HasValue) benefitItemCQuery = benefitItemCQuery.Where(c => c.BenefitItemC.EntranceTestItemID != null);
            //        else benefitItemCQuery = benefitItemCQuery.Where(c => c.BenefitItemC.EntranceTestItemID == null);

            //        var _benefitItemsC = benefitItemCQuery.ToList();
            //        var isForAllOlympics = dbContext.BenefitItemC.Any(c => c.CompetitiveGroupID == competitiveGroupId.Value && c.IsForAllOlympic);
            //        var olympicYear = dbContext.BenefitItemC.Where(c => c.CompetitiveGroupID == competitiveGroupId.Value)
            //            .Select(c => c.OlympicYear).Distinct();
                    
            //        query = query.Where(c => olympicYear.Contains(c.OlympicYear));

            //        if (!isForAllOlympics)
            //        {
            //            var allowedOlympicsIds = _benefitItemsC.Select(c => c.OlympicTypeID).Distinct().ToList();
            //            allowedLevels = _benefitItemsC.Select(c => c.BenefitItemC.OlympicLevelFlags).Distinct().ToList();
            //            query = query.Where(c => allowedOlympicsIds.Contains(c.OlympicID));
            //        }
            //    }
            //    var q = query.ToList();

            //    if (allowedLevels.Count > 0)
            //    {
            //        /* Выбираем все возможные уровни разрешенные в КГ */
            //        short allowedLevelsParsed = 0;
            //        allowedLevels.ForEach(c => allowedLevelsParsed |= c);
            //        q = q.Where(c => (allowedLevelsParsed & c.LevelID.GetLevelFlag()) != 0).ToList();
            //    }

            //    var list = new List<OlympicData>();
            //    int prevID = -1;
            //    var subjects = new List<string>();
            //    foreach (var item in q)
            //    {
            //        if (prevID != item.OlympicID)
            //        {
            //            if (prevID > 0)
            //                list[list.Count - 1].SubjectNames = subjects.Distinct().ToArray();
            //            prevID = item.OlympicID;
            //            list.Add(new OlympicData
            //                {
            //                    OlympicID = item.OlympicID,
            //                    OlympicName = item.Name,
            //                    OrganizerName = item.OrganizerName,
            //                    LevelName = new [] {item.LevelName},
            //                    OlympicYear = item.OlympicYear
            //                });
            //            subjects.Clear();
            //        }
            //        subjects.Add(item.SubjectName);
            //    }
            //    if (list.Count > 0)
            //        list[list.Count - 1].SubjectNames = subjects.Distinct().ToArray();

            //    OlympicDatas = list.ToArray();
            //}
            //else
            //{
            //    DiplomaTypeName =
            //        dbContext.OlympicDiplomType.Where(x => x.OlympicDiplomTypeID == DiplomaTypeID)
            //                 .Select(x => x.Name)
            //                 .FirstOrDefault();
            //    var q = (from x in dbContext.OlympicType
            //             join y in dbContext.OlympicTypeSubjectLink on x.OlympicID equals y.OlympicID
            //             join z in dbContext.Subject on y.SubjectID equals z.SubjectID
            //             join l in dbContext.OlympicLevel on x.OlympicLevelID equals l.OlympicLevelID into tmp
            //             from l0 in tmp.DefaultIfEmpty()
            //             join m in dbContext.OlympicLevel on y.SubjectLevelID equals m.OlympicLevelID into tmp2
            //             from m0 in tmp2.DefaultIfEmpty()
            //             where x.OlympicID == OlympicID
            //             orderby x.Name, z.Name
            //             select new
            //                 {
            //                     x.OlympicID,
            //                     x.Name,
            //                     x.OrganizerName,
            //                     SubjectName = z.Name,
            //                     LevelName = (l0 != null ? l0.Name : m0.Name),
            //                     LevelID = (l0 != null ? l0.OlympicLevelID : m0.OlympicLevelID),
            //                     z.SubjectID,
            //                     x.OlympicYear
            //                 }).ToList();
            //    if (q.Count > 0)
            //    {
            //        OlympicDetails = new OlympicData
            //            {
            //                OlympicID = q[0].OlympicID,
            //                OlympicName = q[0].Name,
            //                OrganizerName = q[0].OrganizerName,
            //                OlympicYear = q[0].OlympicYear
            //            };

            //        OlympicDetails.LevelName = q.Select(x => x.LevelName).Distinct().ToArray();
            //        OlympicDetails.LevelIDs = q.Select(x => x.LevelID).Distinct().ToArray();
            //        OlympicDetails.SubjectNames = q.Select(x => x.SubjectName).Distinct().ToArray();
            //        OlympicDetails.SubjectIDs = q.Select(x => x.SubjectID).Distinct().ToArray();
            //    }
            //    else
            //    {
            //        OlympicDetails = new OlympicData();
            //        OlympicDetails.LevelName = new string[0];
            //        OlympicDetails.LevelIDs = new short[0];
            //        OlympicDetails.SubjectNames = new string[0];
            //        OlympicDetails.SubjectIDs = new int[0];
            //    }
            //}
        }

        public override void PrepareForSave(EntrantsEntities dbContext)
        {
            DocumentOrganization = dbContext.OlympicType.Where(x => x.OlympicID == OlympicID).Select(x => x.Name).FirstOrDefault();
        }

        public override string Validate()
        {
            //StringBuilder errors = new StringBuilder();
            //HashSet<string> subjects = new HashSet<string>();
            //if(Subjects == null) Subjects = new SubjectData[0];
            //foreach (SubjectData data in Subjects)
            //{
            //    if (subjects.Contains(data.SubjectName))
            //        errors.AppendLine("Дублируется название дисциплины: " + data.SubjectName);
            //    else
            //        subjects.Add(data.SubjectName);
            //    if(data.Value < 1 || data.Value > 100)
            //        errors.AppendLine("Некорректный балл для дисциплины: " + data.SubjectName);

            //}
            //return errors.ToString();
            return null;
        }

        public override void SaveToAdditionalTable(ObjectContext dbContext)
        {
            dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentOlympic WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentOlympic(EntrantDocumentID, DiplomaTypeID, OlympicID)
	VALUES({0}, {1}, {2})",
                                          EntrantDocumentID,
                                          DiplomaTypeID,
                                          OlympicID);
        }

        public class OlympicData
        {
            public int OlympicID { get; set; }
            public string OlympicName { get; set; }

            [DisplayName("Организатор")]
            public string OrganizerName { get; set; }

            [DisplayName("Уровень олимпиады")]
            public string[] LevelName { get; set; }

            [DisplayName("Год олимпиады")]
            public int OlympicYear { get; set; }

            public short[] LevelIDs { get; set; }

            [DisplayName("Профильные дисциплины")]
            public string[] SubjectNames { get; set; }

            [ScriptIgnore] //для списка профильных предметов
            public int[] SubjectIDs { get; set; }
        }
    }

    public static partial class Extensions
    {
        public static short GetLevelFlag(this short level)
        {
            short flagOlympic = 0;
            if (level == 2) flagOlympic |= 1;
            if (level == 3) flagOlympic |= 2;
            if (level == 4) flagOlympic |= 4;
            return flagOlympic;
        }
    }
}