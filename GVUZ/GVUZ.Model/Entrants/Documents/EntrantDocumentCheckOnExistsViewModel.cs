namespace GVUZ.Model.Entrants.Documents
{
    public class EntrantDocumentCheckOnExistsViewModel
    {
        public int EntrantID { get; set; }
        public int ApplicationID { get; set; }
        public int EntrantDocumentID { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
    }
}