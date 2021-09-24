using GVUZ.Model.Institutions;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Структура вуза в заявлении. Не используется.
	/// </summary>
	public class ApplicationAdmissionStructure
	{
		public ApplicationAdmissionStructure()
		{
			IsVUZ = true;
			Institution = string.Empty;
			Faculty = string.Empty;
			Direction = string.Empty;
			Course = string.Empty;
			EducationLevel = string.Empty;
			StudyForm = string.Empty;
			AdmissionType = string.Empty;
		}

		public void AppendName(AdmissionItemLevel level, string name)
		{
			switch (level)
			{
				case AdmissionItemLevel.Institution:
					Institution = name;
					break;
				case AdmissionItemLevel.Faculty:
					Append(ref Faculty, name);
					break;
				case AdmissionItemLevel.Course:
					Append(ref Course, name);
					break;
				case AdmissionItemLevel.Direction:
					Append(ref Direction, name);
					break;
				case AdmissionItemLevel.EducationLevel:
					Append(ref EducationLevel, name);
					break;
				case AdmissionItemLevel.Study:
					Append(ref StudyForm, name);
					break;
				case AdmissionItemLevel.AdmissionType:
					Append(ref AdmissionType, name);
					break;
			}
		}

		private static void Append(ref string value, string name)
		{
			if (string.IsNullOrEmpty(value))
			{
				value = name;
				return;
			}

			if (value.Contains(name))
				return;

			value = value + "," + name;
		}

		public bool IsVUZ;
		public string Institution;
		public string Faculty;
		public string Direction;
		public string Course;
		public string EducationLevel;
		public string StudyForm;
		public string AdmissionType;

		public int PlaceCount;
		public int ApplicationCount;
	}
}