using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Web.Models;

namespace GVUZ.Web.Portlets.Applications
{
	public class ApplicationEntrantDocumentsViewModel
	{
	    public ApplicationEntrantDocumentsViewModel()
	    {
            ExistingDocuments = new List<DocumentShortInfoViewModel>();
            AttachedDocuments = new List<DocumentShortInfoViewModel>();
	    }

	    //Для view, описание полей
		private static readonly DocumentShortInfoViewModel _baseDocument = new DocumentShortInfoViewModel();
		public DocumentShortInfoViewModel BaseDocument
		{
			get { return _baseDocument; }
		}

		public int EntrantID { get; set; }
		
		[ScriptIgnore]
        public List<DocumentShortInfoViewModel> ExistingDocuments { get; set; }
		[ScriptIgnore]
        public List<DocumentShortInfoViewModel> AttachedDocuments { get; set; }

		public int[] AttachedDocumentIDs { get; set; }

		public ApplicationStepType ApplicationStep { get; set; }

		public class DocumentType
		{
			public int TypeID { get; set; }
			public string Name { get; set; }
		}

		public IEnumerable<DocumentType> DocumentTypes;

		public bool ShowDenyMessage { get; set; }
		public string StepDirection { get; set; }

		//[DisplayName("Предоставлены оригиналы документов")]
		//public bool? OriginalDocumentsReceived { get; set; }
		//[DisplayName("Предоставлены оригиналы документов")]
		//public bool OriginalDocumentsReceivedBool
		//{
		//    get { return OriginalDocumentsReceived.To(false); }
		//}

		public bool ApplicationIncludedInOrder { get; set; }

		public int ApplicationID { get; set; }

		public int ApplicationStatus { get; set; }
	}
}