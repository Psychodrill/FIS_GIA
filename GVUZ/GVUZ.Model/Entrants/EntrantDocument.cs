namespace GVUZ.Model.Entrants
{
    public partial class EntrantDocument
    {
        ~EntrantDocument()
        {
            DocumentTypeID = 0;
            AttachmentID = null;
        }
    }
}
