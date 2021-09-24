namespace GVUZ.Util.UI.Importing
{
    public class ImportResult
    {
        public int TotalRecords { get; set; }
        public int SuccessRecords { get; set; }
        public int FailedRecords { get; set; }

        public int ProcessedRecords 
        {
            get { return SuccessRecords + FailedRecords; }
        }
    }
}