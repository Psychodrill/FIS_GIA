namespace GVUZ.Model.Helpers
{
	public class UserInfo
	{
		public string SURNAME { get; set; }
		public string NAME { get; set; }
		public string PATRONYMIC { get; set; }
		public string INN { get; set; }
		public string SNILS { get; set; }

		public static UserInfo Default
		{
			get { return new UserInfo {SNILS = null, NAME = "", PATRONYMIC = "", SURNAME = ""}; }
		}
	}
}