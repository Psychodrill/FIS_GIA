using System.ComponentModel;

namespace GVUZ.Model.Entrants
{
	public class EntranceTestSource
	{
		public const int EgeDocumentSourceId = 1;
		public const int OUTestSourceId = 2;
		public const int OlympiadSourceId = 3;
		public const int GiaDocumentSourceId = 4;
	}

    public enum InstitutionDocumentTypeEnum
    {
        [Description("Экзаменационная ведомость")]
        ExamsVedomost = 1,
        [Description("Экзаменационный лист")]
        ExamsList = 2,
        [Description("Апелляционная ведомость")]
        AppilateVedomost = 3
    }
}