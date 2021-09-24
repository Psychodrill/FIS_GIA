using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text;

namespace GVUZ.Web.ViewModels
{
	public class BenefitViewModelC
	{
		public int EntranceTestItemID { get; set; }
		public int CompetitiveGroupID { get; set; }

		[DisplayName("Тип диплома")]
		public string DiplomaType { get; set; }
		[DisplayName("Уровень")]
		public string ComptetitionLevel { get; set; }
        [DisplayName("Год олимпиады")]
        public int OlympicYear { get; set; }
		[DisplayName("Вид льготы")]
		public string BenefitType { get; set; }

	    [DisplayName("Номера олимпиад из перечня")]
		public string OlympicCaption
		{
			get { return null; }
		}

		[DisplayName("UID")]
		[StringLength(200)]
		public string UID { get; set; }


		[DisplayName("Все")]
		public bool IsAllOlympicCaption { get { return false; }}

		public class OlympicData
		{
			public int OlympicID { get; set; }
			public int Number { get; set; }
		    public string Name { get; set; }
		}

		public BenefitViewModelC(int entranceTestItemID, int competitiveGroupID)
		{
			EntranceTestItemID = entranceTestItemID;
			CompetitiveGroupID = competitiveGroupID;
		}

		public class BenefitData
		{
			public int BenefitItemID { get; set; }
			public int DiplomaTypeID { get; set; }
			public string DiplomaType { get; set; }
			public int CompetitionLevelFlags { get; set; }
            public int OlympicYear { get; set; }

		    public string CompetitionLevel
			{
				get
				{
					List<String> res = new List<string>();
					if(CompetitionLevelFlags == 255)
						res.Add("Все уровни");
					else
					{
						if ((CompetitionLevelFlags & 1) != 0) res.Add("I");
						if ((CompetitionLevelFlags & 2) != 0) res.Add("II");
						if ((CompetitionLevelFlags & 4) != 0) res.Add("III");
					}
					return String.Join(", ", res.ToArray());
				}
			}
			public int BenefitTypeID { get; set; }
			public string BenefitType { get; set; }

			public OlympicData[] AttachedOlympic { get; set; }

			[DisplayName("Все")]
			public bool IsAllOlympic { get; set; }

			[StringLength(200)]
			public string UID { get; set; }

            [DisplayName("Минимальный балл ЕГЭ, необходимый для использования льготы (с 2014 года)")]
            public int? MinEgeValue { get; set; }

            public SubjectData[] AttachedSubjects { get; set; }

            public string AttachedSubjectsAsHtml
            {
                get
                {
                    if (AttachedSubjects == null || AttachedSubjects.Count() == 0)
                        return string.Empty;

                    StringBuilder sb = new StringBuilder();

                    foreach (SubjectData subject in AttachedSubjects)
                        sb.AppendFormat("{0}<br/>", subject.ToString());

                    return sb.ToString();
                }
            }
		}

		public BenefitData[] Benefits { get; set; }


        public BenefitData Benefit
        {
            get { return null; }
        }

        public class SubjectData
        {
            public string SubjectName { get; set; }
            public int? EgeMinValue { get; set; }

            public override string ToString()
            {
                return string.Format("{0} - {1}", SubjectName, EgeMinValue);
            }
        }


		public bool CanEdit { get; set; }
	}
}