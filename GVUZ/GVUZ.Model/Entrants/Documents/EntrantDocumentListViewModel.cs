using System.Collections.Generic;

namespace GVUZ.Model.Entrants.Documents
{
    public class EntrantDocumentListViewModel
    {
        //Для view, описание полей
        private static readonly DocumentShortInfoViewModel _baseDocument = new DocumentShortInfoViewModel();
        public IEnumerable<DocumentType> DocumentTypes;

        public DocumentShortInfoViewModel BaseDocument
        {
            get { return _baseDocument; }
        }

        public int EntrantID { get; set; }
        public IEnumerable<DocumentShortInfoViewModel> Documents { get; set; }

        public int ApplicationStep { get; set; }

        public class DocumentType
        {
            public int TypeID { get; set; }
            public string Name { get; set; }
        }
    }
}