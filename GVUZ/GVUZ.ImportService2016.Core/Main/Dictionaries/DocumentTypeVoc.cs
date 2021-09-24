using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class DocumentTypeVoc : VocabularyBase<DocumentTypeVocDto>
    {
        public DocumentTypeVoc(DataTable dataTable) : base(dataTable) { }

        public string GetDocumentTypeName(int documentTypeId)
        {
            var d = items.Where(x => x.DocumentID == documentTypeId).FirstOrDefault();
            if (d != null) return d.Name;
            return null;
        }

        const int SportCategory = 7;
        public IEnumerable<DocumentTypeVocDto> SportDocuments
        {
            get
            {
                return Items.Where(t => t.CategoryID == SportCategory);
            }
        }
    }

    public class DocumentTypeVocDto : VocabularyBaseDto
    {
        public int DocumentID { get; set; }
        public override int ID
        {
            get { return DocumentID; }
            set { DocumentID = value; }
        }

        public bool IsMedical { get; set; }
        public int CategoryID { get; set; }
    }
}
