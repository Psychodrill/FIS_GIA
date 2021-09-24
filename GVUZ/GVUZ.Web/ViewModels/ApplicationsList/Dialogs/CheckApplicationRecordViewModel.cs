using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class CheckApplicationRecordViewModel
    {
        public static CheckApplicationRecordViewModel MetadataInstance = new CheckApplicationRecordViewModel();

        public int ApplicationId { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Нарушения")]
        public string ViolationMessage { get; set; }
    }
}