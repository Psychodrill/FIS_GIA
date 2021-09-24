using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
	public class AddBenefitViewModelC
	{
		public int EntranceTestItemID { get; set; }
		public int CompetitiveGroupID { get; set; }

		public int BenefitItemID { get; set; }

		[DisplayName("Тип диплома")]
		public int DiplomaTypeID { get; set; }

		[DisplayName("Уровень")]
		public int OlympicLevelFlags { get; set; }

        [DisplayName("Год олимпиады")]
        public int OlympicYearID { get; set; }

		[DisplayName("Вид льготы")]
		public int BenefitTypeID { get; set; }

	    public IEnumerable BenefitTypes { get; set; }
		public string FirstBenefitTypeName { get; set; }

		[DisplayName("Совпадение с профильным предметом олимпиады")]
		public bool IsProfileSubject { get; set; }

		[DisplayName("Все олимпиады")]
		public bool IsForAllOlympic { get; set; }

        [DisplayName("Минимальный балл ЕГЭ, необходимый для использования льготы (с 2014 года)")]
        public int? MinEgeValue { get; set; }

		public const int OLYMPIC_ALL = 255;

		public class OlympicData
		{
			public int OlympicID { get; set; }

			[DisplayName("Номер в перечне")]
			public string Number { get { return NumberInt.ToString(); }}

			public int NumberInt { get; set; }

			[DisplayName("Уровень")]
			public short[] Level { get; set; }

            [DisplayName("Год олимпиады")]
            public int Year { get; set; }

			[DisplayName("Название олипиады")]
			public string Name { get; set; }

			public bool IsProfileSubject { get; set; }
		}


		public OlympicData[] AllOlympic { get; set; }

		public int[] AttachedOlympic { get; set; }
        public int[] OlympicYears { get; set; }
        public int SubjectID { get; set; }

	    public OlympicData OlympicDescr
		{
			get { return null; }
		}

        /// <summary>
        /// ТОлько для получения описания свойств
        /// </summary>
        public BenefitItemSubjectViewModel SubjectDescr
        {
            get { return null; }
        }

        public BenefitItemSubjectViewModel[] BenefitItemSubjects { get; set; }

        public class Subject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public Subject[] AllSubjects { get; set; }

		public bool HideProfileSubject { get; set; }

		[DisplayName("UID")]
		[StringLength(200)]
		public string UID { get; set; }
	}
}