using System.ComponentModel;

namespace GVUZ.Web.ViewModels
{
	public class CompetitiveGroupViewModel1234
	{
		[DisplayName("������� �����������")]
		public short EducationalLevelID { get; set; }
		[DisplayName("������� �����������")]
		public string EducationalLevelName { get; set; }

		[DisplayName("�������������")]
		public int? DirectionID { get; set; }
		public string DirectionName { get; set; }

		public int GroupID { get; set; }

		[DisplayName("������������ ��������")]
		public string GroupName { get; set; }

		[DisplayName("����")]
		public string CourseName { get; set; }

		[DisplayName("�������� ��������")]
		public string CampaignName { get; set; }

		[DisplayName("��������� ����� (����� �������)")]
		public string BudgetName { get { return null; } }

		[DisplayName("����� ��������")]
		public int? NumberBudgetO { get; set; }

		[DisplayName("����-������� ��������")]
		public int? NumberBudgetOZ { get; set; }

		[DisplayName("������� ��������")]
		public int? NumberBudgetZ { get; set; }

        [DisplayName("����� ����� ���, ������� ������ �����")]
        public string QuotaName { get { return null; } }

        [DisplayName("����� ��������")]
        public int? NumberQuotaO { get; set; }

        [DisplayName("����-������� ��������")]
        public int? NumberQuotaOZ { get; set; }

        [DisplayName("������� ��������")]
        public int? NumberQuotaZ { get; set; }
        
        [DisplayName("����� � ������� ��������� ��������")]
		public string PaidName { get { return null; } }

		[DisplayName("����� ��������")]
		public int? NumberPaidO { get; set; }

		[DisplayName("����-������� ��������")]
		public int? NumberPaidOZ { get; set; }

		[DisplayName("������� ��������")]
		public int? NumberPaidZ { get; set; }


		[DisplayName("������� �����")]
		public string TargetName { get { return null; } }

		[DisplayName("����� ��������")]
		public int? NumberTargetO { get; set; }

		[DisplayName("����-������� ��������")]
		public int? NumberTargetOZ { get; set; }

		[DisplayName("������� ��������")]
		public int? NumberTargetZ { get; set; }


		[DisplayName("������������� ���������")]
		public int EntranceTestCount { get; set; }

		public bool CanEdit { get; set; }
	}
}