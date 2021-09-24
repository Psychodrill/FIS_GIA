using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using GVUZ.Model;
using GVUZ.Model.Entrants;

namespace GVUZ.Web.ViewModels.Administration.Catalogs
{
	public class AddOlympicViewModel
	{
		[DisplayName("Наименование олимпиады")]
		public int OlympicID { get; set; }

		[DisplayName("Наименование олимпиады")]
		[LocalRequired]
		[StringLength(1023)]
		public string Name { get; set; }

        [DisplayName("Уровень")]
		public short? OlympicLevelID { get; set; }

        [DisplayName("Год олимпиады")]
        [LocalRequired]
        public int OlympicYear { get; set; }

	    [DisplayName("Организатор")]
		[LocalRequired]
		[StringLength(1023)]
		public string OrganizerName { get; set; }

		[DisplayName("№ олимпиады")]
		[LocalRequired]
		public int? OlympicNumber { get; set; }

		public IEnumerable Levels { get; set; }

		public AddOlympicViewModel() { }

		public AddOlympicViewModel(int olympicId)
		{
			OlympicID = olympicId;
		}

		public class SubjectBriefData
		{
			public int SubjectID { get; set; }

			[DisplayName("Дисциплина")]
			public string SubjectName { get; set; }
            
            public short? SubjectLevelID { get; set; }

            [DisplayName("Уровень")]
            public string SubjectLevelName { get; set; }
		}

		public SubjectBriefData SubjectBriefDataDescr
		{
			get { return null; }
		}

		[DisplayName("Профильные предметы")]	
		public SubjectBriefData[] Subjects { get; set; }

		[ScriptIgnore]
		public string[] SubjectNameList;

	    [ScriptIgnore] 
        public string[] SubjectLevels;

	    public void FillData(EntrantsEntities dbContext)
		{
   //         SubjectNameList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).Select(x => x.Value).ToArray();
			//Subjects = dbContext.OlympicTypeSubjectLink.Where(x => x.OlympicID == OlympicID).Select(x => new SubjectBriefData()
			//{
			//	SubjectID = x.Subject.SubjectID,
			//	SubjectName = x.Subject.Name,
   //             SubjectLevelID = x.SubjectLevelID,
   //             SubjectLevelName = x.OlympicLevel != null ? x.OlympicLevel.Name : ""
			//}).ToArray();										
		}

		public void PrepareForSave(EntrantsEntities dbContext)
		{
			var subjects = dbContext.Subject.OrderBy(x => x.Name).ToArray();
            var levels = dbContext.OlympicLevel.OrderBy(x => x.Name).ToArray();
			Subjects = (from data in Subjects
						let f = subjects.FirstOrDefault(x => x.Name == data.SubjectName)
                        let m = levels.FirstOrDefault(x => x.Name == data.SubjectLevelName)
						where f != null
                        select new SubjectBriefData { SubjectID = f.SubjectID, SubjectLevelID = 
                            m != null ? m.OlympicLevelID : (short?)null }).ToArray();
		}		

		public string Validate()
		{
			var errors = new StringBuilder();
			var subjects = new HashSet<string>();
			if(Subjects == null) Subjects = new SubjectBriefData[0];
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
	}
}