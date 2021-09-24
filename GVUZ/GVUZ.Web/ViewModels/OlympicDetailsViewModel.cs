using System.ComponentModel;
using System.Linq;
using System.Web.Script.Serialization;
using GVUZ.Model.Benefits;

namespace GVUZ.Web.ViewModels
{
	public class OlympicDetailsViewModel
	{
		[DisplayName("Тип диплома")]
		public int? DiplomaTypeID { get; set; }

		public class OlympicData
		{
			public int OlympicID { get; set; }
			public string OlympicName { get; set; }
			[DisplayName("Организатор")]
			public string OrganizerName { get; set; }
			[DisplayName("Уровень")]
			public string LevelName { get; set; }

            [DisplayName("Год олимпиады")]
            public int OlympicYear { get; set; }

			public int LevelID { get; set; }
			[DisplayName("Профильные дисциплины")]
			public string[] SubjectNames { get; set; }
		}

		[DisplayName("Олимпиада")]
		public int OlympicID { get; set; }

		[ScriptIgnore]
		public OlympicData[] OlympicDatas { get; set; }

		//для отображения текста во вьюхе
		[ScriptIgnore]
		public OlympicData OlympicDetails { get; set; }

		public void FillData(BenefitsEntities dbContext)
		{
            var q = (from x in dbContext.OlympicType
                     join y in dbContext.OlympicTypeSubjectLink on x.OlympicID equals y.OlympicID
                     join z in dbContext.Subject on y.SubjectID equals z.SubjectID
                     join l in dbContext.OlympicLevel on x.OlympicLevelID equals l.OlympicLevelID into tmp from l0 in tmp.DefaultIfEmpty()
                     join m in dbContext.OlympicLevel on y.SubjectLevelID equals m.OlympicLevelID into tmp2 from m0 in tmp2.DefaultIfEmpty()
                     where x.OlympicID == OlympicID
                     orderby x.Name, z.Name
                     select new
                         {
                             x.OlympicID, 
                             x.Name, 
                             x.OrganizerName, 
                             SubjectName = z.Name,
                             LevelName = (l0 != null ? l0.Name : m0.Name),
                             LevelID = (l0 != null ? l0.OlympicLevelID : m0.OlympicLevelID),
                             x.OlympicYear
                         }).ToList();
            if (q.Count > 0)
            {
                OlympicDetails = new OlympicData()
                {
                    OlympicID = q[0].OlympicID,
                    OlympicName = q[0].Name,
                    OrganizerName = q[0].OrganizerName,
                    LevelName = q[0].LevelName,
                    OlympicYear = q[0].OlympicYear,
                    LevelID = q[0].LevelID
                };
                OlympicDetails.SubjectNames = q.Select(x => x.SubjectName).ToArray();
            }
            else
            {
                OlympicDetails = new OlympicData();
                OlympicDetails.SubjectNames = new string[0];
            }
		}
	}
}