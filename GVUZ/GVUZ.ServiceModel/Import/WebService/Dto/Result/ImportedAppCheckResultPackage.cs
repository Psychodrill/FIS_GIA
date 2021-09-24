using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Result
{
	public class ImportedAppCheckResultPackage
	{
		public string PackageID;
		public string StatusCheckCode;
		public string StatusCheckMessage;

		[XmlArrayItem(ElementName = "EgeDocumentCheckResult")]
		public EgeDocumentCheckResultDto[] EgeDocumentCheckResults;
		[XmlArrayItem(ElementName = "GetEgeDocument")]
		public GetEgeDocumentDto[] GetEgeDocuments;
	}

    public class AppSingleCheckResult
    {
        public EgeDocumentCheckResultDto EgeDocumentCheckResults;
        public GetEgeDocumentDto GetEgeDocuments;
    }

	#region EgeDocumentCheckResultDto objects
	public class EgeDocumentCheckResultDto
	{
		public ApplicationShortRef Application;
		[XmlArrayItem(ElementName = "EgeDocument")]
		public EgeDocumentCheckItemDto[] EgeDocuments;
	}

	public class EgeDocumentCheckItemDto
	{
		public string StatusCode;
		public string StatusMessage;
		public string DocumentNumber;
		public string DocumentDate;

        [XmlArrayItem(ElementName = "CorrectResultItem")]
		public CorrectResultItemDto[] CorrectResults;
        [XmlArrayItem(ElementName = "IncorrectResultItem")]
		public IncorrectResultItemDto[] IncorrectResults;
	}

	public class CorrectResultItemDto
	{
		public string SubjectName;
		public string Score;
	}

	public class IncorrectResultItemDto : CorrectResultItemDto
	{
		public string Comment;
	}

	#endregion

	#region GetEgeDocumentDto objects
	public class GetEgeDocumentDto
	{
		public ApplicationShortRef Application;
		
        [XmlArrayItem(ElementName = "EgeDocument")]
		public EgeDocumentDto[] EgeDocuments;
		public string Error;
	}

	public class EgeDocumentDto
	{
		public string CertificateNumber;
		public string TypographicNumber;
		public string Year;
		public string Status;
		[XmlArrayItem(ElementName = "Mark")]
		public SubjectMarkDto[] Marks;
	}

	public class SubjectMarkDto
	{
        public string SubjectID;
		public string SubjectName;
		public string SubjectMark;
	}
	#endregion

}
