using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels
{
	public class PrintTemplateInfoViewModel
	{
		public int EntrantID { get; set; }

		public int ApplicationID { get; set; }

		public string ApplicationNumber { get; set; }

		public DateTime ApplicationRegistrationDateTime { get; set; }

		public string ApplicationRegistrationDate { get { return ApplicationRegistrationDateTime.ToString("dd.MM.yyyy"); } }

		public string EntrantLastName { get; set; }

		public string EntrantMiddleName { get; set; }

		public string EntrantFistName { get; set; }

		public string NameInitials
		{
			get
			{
				string middleName = EntrantMiddleName ?? "";
				if (!String.IsNullOrEmpty(middleName))
					middleName = middleName[0] + ".";
				return EntrantFistName[0] + ". " + middleName;
			}
		}

		public DocumentData SchoolCertificateDocument { get; set; }
		public DocumentData[] AttachedDocuments { get; set; }

		private const string Absent = "отсутствует";

		public class DocumentData
		{			
			private string _series = Absent;
			private string _number = Absent;

			public string Series
			{
				get { return string.IsNullOrEmpty(_series) ? Absent : _series; }
				set { _series = value; }
			}

			public string Number
			{
				get { return string.IsNullOrEmpty(_number) ? Absent : _number; }
				set { _number = value; }
			}

			public string TypeName { get; set; }
		}

		public string CompetitiveGroupName { get; set; }

		public string InstitutionFullName { get; set; }

		public string FormList { get; set; }

		public bool OriginalDocumentsReceived { get; set; }

		public PrintTemplateInfoViewModel()
		{
			SchoolCertificateDocument = new DocumentData();
			AttachedDocuments = new[]{new DocumentData()};			
		}
	}
}